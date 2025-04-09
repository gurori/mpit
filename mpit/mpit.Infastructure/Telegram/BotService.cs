using Microsoft.AspNetCore.SignalR;
using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.Core.DTOs.Chat;
using mpit.mpit.DataAccess.Entities;
using mpit.mpit.Infastructure.Cache;
using mpit.mpit.Infastructure.Chat;
using mpit.mpit.Infastructure.Chat.Hubs;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace mpit.mpit.Infastructure.Telegram;

public class BotService(
    IConfiguration configuration,
    CacheClient cache,
    ChatsClient chatsClient,
    IServiceScopeFactory serviceScopeFactory
//IHubContext<ChatHub> hubContext
)
{
    private readonly TelegramBotClient _botClient = new TelegramBotClient(
        configuration["TelegramBot:Token"]!
    );
    private readonly CacheClient _cache = cache;
    private readonly ChatsClient _chatsClient = chatsClient;
    private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

    //private readonly IHubContext<ChatHub> _hubContext = hubContext;
    private const string startMessage = """
        Привет! Я бот для помощи учатникам СВО, их семьям и переселенцам.
        Начни помогать людям, напиши "/list", чтобы получить список пользователей, которым нужна помощь.
        """;

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        if (update.Type == UpdateType.Message)
            await HandleMessageAsync(update.Message!);
        else if (update.Type == UpdateType.CallbackQuery)
            await HandleCallbackQueryAsync(update.CallbackQuery!);
    }

    private async Task HandleMessageAsync(Message message)
    {
        string messageText = message.Text!;
        var chatId = message.Chat.Id;
        if (messageText.StartsWith('/'))
            await HandleCommandAsync(messageText, chatId);
    }

    private async Task HandleCommandAsync(string messageText, long chatId)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var usersRepository = scope.ServiceProvider.GetRequiredService<IUsersRepository>();
        switch (messageText.Split(" ").First())
        {
            case "/start":
                await _botClient.SendMessage(chatId: chatId, text: startMessage);
                break;
            case "/list":
                await _botClient.SendMessage(chatId: chatId, text: "Выбери кому помочь:");
                var logins = (await _cache.GetByKeyAsync<string[]>("logins") ?? []).ToHashSet();

                var users = await usersRepository.GetInfosByLogins(logins.ToArray());
                foreach (var user in users)
                    await _botClient.SendMessage(
                        chatId: chatId,
                        text: GetInfoText(user.Info!, user.Login),
                        replyMarkup: new InlineKeyboardMarkup(
                            [
                                [InlineKeyboardButton.WithCallbackData("Написать", user.Login)],
                            ]
                        ),
                        parseMode: ParseMode.MarkdownV2
                    );
                break;
            case "/send":
                var data = messageText.Split('"');
                var login = data[1];
                var sendText = data[2].Substring(1);
                var userEntity = await usersRepository.GetEntityByLoginAsync(login);
                await _chatsClient.AddMessageToDbAsync(login, "~" + sendText);
                break;
            case "/update":
                var loginUpadte = messageText.Split(" ").ToList();
                if (loginUpadte.Count < 2)
                    return;
                loginUpadte.Remove("/update");
                var messages =
                    await _cache.GetByKeyAsync<ChatMessage[]>(
                        $"messages-{string.Join(" ", loginUpadte)}"
                    ) ?? [];
                foreach (var message in messages)
                    await _botClient.SendMessage(chatId, message.Text);
                break;
            default:
                await _botClient.SendMessage(chatId, "Неизвестная команда.");
                break;
        }
    }

    private async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        var data = callbackQuery.Data;
        var chatId = callbackQuery.Message!.Chat.Id;
        await _botClient.SendMessage(
            chatId,
            $"""
            Чтобы написать пользователю, сообщение напишите 
            `/send "{data}" Ваше сообщение`, где "Ваше сообщение" \- сообщение, которое вы хотите отправить\.
            Вы можете отправлять несколько сообщений\.

            Вот сообщения, которые отправил Вам пользователь:
            """,
            parseMode: ParseMode.MarkdownV2
        );

        var messages = await _cache.GetByKeyAsync<ChatMessage[]>($"messages-{data}") ?? [];

        foreach (var message in messages)
            await _botClient.SendMessage(chatId, message.Text);

        return;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>(), // Получаем все типы обновлений
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken
        );

        var me = await _botClient.GetMe();
        Console.WriteLine($"Бот запущен: @{me.Username}");
    }

    private async Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine($"Ошибка polling'а: {exception.Message}");
        await Task.CompletedTask;
    }

    private string GetInfoText(InfoEntity info, string login)
    {
        return $"""
            ФИО участника СВО: {info.Name}
            Дата рождения: {info.Date}

            Проблема: {info.Need}

            Логин: `{login}`
            """;
    }
}

using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using mpit.mpit.Core.DTOs.Chat;

namespace mpit.mpit.Infastructure.Chat.Hubs;

public interface IChatMethods
{
    public Task ReceiveMessageAsync(string UserName, string message);
}

public class ChatHub(IDistributedCache cache, ChatsClient chatsService) : Hub<IChatMethods>
{
    private readonly IDistributedCache _cache = cache;
    private readonly ChatsClient _chatsService = chatsService;

    public async Task JoinChatAsync(UserChatConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatId);

        string stringConnection = JsonSerializer.Serialize(connection);
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);
        var loginsString = await _cache.GetStringAsync("logins") ?? "[]";
        var logins = (JsonSerializer.Deserialize<string[]>(loginsString) ?? []).ToHashSet();
        logins.Add(connection.UserName);
        loginsString = JsonSerializer.Serialize(logins);
        await _cache.SetStringAsync("logins", loginsString);
    }

    public async Task SendMessageAsync(string message)
    {
        var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);
        if (stringConnection is not null)
        {
            var connection = JsonSerializer.Deserialize<UserChatConnection>(stringConnection);
            if (connection is not null)
            {
                await Clients
                    .Group(connection.ChatId)
                    .ReceiveMessageAsync(connection.UserName, message);
                await _chatsService.AddMessageToDbAsync(connection.UserName, message);
            }
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);
        if (stringConnection is not null)
        {
            var connection = JsonSerializer.Deserialize<UserChatConnection>(stringConnection);

            if (connection is not null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}

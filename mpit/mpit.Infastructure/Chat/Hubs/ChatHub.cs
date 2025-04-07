using Microsoft.AspNetCore.SignalR;
using mpit.mpit.Core.DTOs.Chat;

namespace mpit.mpit.Infastructure.Chat.Hubs;

public interface IChatMethods
{
    public Task ReceiveMessageAsync();
}

public class ChatHub : Hub<IChatMethods>
{
    public async Task JoinChatAsync(UserChatConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatId);
    }

    public async Task SendMessageAsync(string message)
    {
        // await Clients
        //     .Group()
        //     .ReceiveMessageAsync(message);
    }
}

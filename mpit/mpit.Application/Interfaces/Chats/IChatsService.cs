using mpit.mpit.Core.DTOs.Chat;

namespace mpit.mpit.Application.Interfaces.Chats;

public interface IChatsService
{
    public Task<ChatResponse> GetMessagesAsync(string token);
}

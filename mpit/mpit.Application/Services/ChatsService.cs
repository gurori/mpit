using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using mpit.mpit.Application.Interfaces.Auth;
using mpit.mpit.Application.Interfaces.Chats;
using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.Core.DTOs.Chat;

namespace mpit.mpit.Application.Services;

public sealed class ChatsService(
    IDistributedCache cache,
    IJwtProvider jwtProvider,
    IUsersRepository usersRepository
) : IChatsService
{
    private readonly IDistributedCache _cache = cache;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<ChatResponse> GetMessagesAsync(string token)
    {
        Guid userId = await _jwtProvider.GetUserIdAsync(token);
        string userName = await _usersRepository.GetLoginByIdAsync(userId);

        string key = $"messages-{userName}";
        string stringMessages = await _cache.GetStringAsync(key) ?? "[]";
        var messages = new ChatResponse(
            JsonSerializer.Deserialize<ChatMessage[]>(stringMessages) ?? [],
            userName,
            userId.ToString()
        );
        return messages;
    }
}

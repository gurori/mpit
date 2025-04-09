using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using mpit.mpit.Core.DTOs.Chat;

namespace mpit.mpit.Infastructure.Chat;

public sealed class ChatsClient(IDistributedCache cache) {
    private readonly IDistributedCache _cache = cache;

    public async Task AddMessageToDbAsync(string userName, string message)
    {
        ChatMessage chatMessage = new(userName, message);
        string key = $"messages-{userName}";

        var stringChatMessages = await _cache.GetStringAsync(key) ?? "[]";
        var chatMessages = JsonSerializer.Deserialize<ChatMessage[]>(stringChatMessages) ?? [];

        var newChatMessages = chatMessages.ToList();
        newChatMessages.Add(chatMessage);

        string stringMessages = JsonSerializer.Serialize(newChatMessages);
        await _cache.SetStringAsync(key, stringMessages);
    }
}
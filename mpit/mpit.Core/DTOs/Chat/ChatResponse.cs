namespace mpit.mpit.Core.DTOs.Chat;

public record ChatResponse(ChatMessage[] Messages, string UserName, string ChatId);

namespace mpit.mpit.Core.DTOs.Chat;

public class ChatMessage
{
    public ChatMessage(string userName, string text, Guid? userId = null)
    {
        UserId = userId;
        UserName = userName;
        Text = text;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid? UserId { get; private set; }
    public string UserName { get; private set; }
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }
}

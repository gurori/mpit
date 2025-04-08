using Microsoft.AspNetCore.Mvc;
using mpit.mpit.Application.Interfaces.Chats;
using mpit.mpit.Core.Enums;
using mpit.mpit.Infastructure.Auth;

namespace mpit.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ChatsController(IChatsService chatsService) : BaseController
{
    private readonly IChatsService _chatsService = chatsService;

    [HttpGet("messages")]
    [HasPermission(Permission.SendMessage)]
    public async Task<IActionResult> GetMessages()
    {
        string token = GetTokenFromHeaders();
        var messages = await _chatsService.GetMessagesAsync(token);
        return Ok(messages);
    }
}

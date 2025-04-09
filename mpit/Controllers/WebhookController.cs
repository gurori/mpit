// using Microsoft.AspNetCore.Mvc;
// using mpit.mpit.Infastructure.Telegram;
// using Telegram.Bot.Types;

// namespace TelegramBot.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class WebhookController : ControllerBase
//     {
//         private readonly BotService _botService;

//         public WebhookController(BotService botService)
//         {
//             _botService = botService;
//         }

//         [HttpPost]
//         public async Task<IActionResult> Post([FromBody] Update update)
//         {
//             await _botService.HandleUpdateAsync(update);
//             return Ok();
//         }
//     }
// }

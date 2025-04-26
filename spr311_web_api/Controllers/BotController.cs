using Microsoft.AspNetCore.Mvc;
using spr311_web_api.BLL.Services.Telegram;
using Telegram.Bot.Types;

namespace spr311_web_api.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : Controller
    {
        private ITelegramService _telegramService;

        public BotController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Bot started");
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Update update)
        {
            if (update != null)
            {
                await _telegramService.UpdateHandlerAsync(update);
            }

            return Ok();
        }
    }
}

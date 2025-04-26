using Telegram.Bot;

namespace spr311_web_api.BLL.Services.Bot
{
    public interface ITelegramBotService
    {
        public TelegramBotClient GetTelegramBot();
    }
}

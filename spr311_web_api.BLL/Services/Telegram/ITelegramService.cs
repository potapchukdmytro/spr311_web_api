using Telegram.Bot.Types;

namespace spr311_web_api.BLL.Services.Telegram
{
    public interface ITelegramService
    {
        public Task UpdateHandlerAsync(Update update);
    }
}

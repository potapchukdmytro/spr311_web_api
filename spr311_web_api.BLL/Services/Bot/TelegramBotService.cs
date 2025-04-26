using Microsoft.Extensions.Options;
using spr311_web_api.BLL.Configuration;
using Telegram.Bot;

namespace spr311_web_api.BLL.Services.Bot
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly TelegramBotClient _client;

        public TelegramBotService(IOptions<BotSettings> options)
        {
            var botSettings = options.Value;
            string webhook = botSettings.Webhook ?? "";
            string token = botSettings.Token ?? "";
            _client = new TelegramBotClient(token);
            _client.SetWebhook(webhook).Wait();
        }

        public TelegramBotClient GetTelegramBot()
        {
            return _client;
        }
    }
}

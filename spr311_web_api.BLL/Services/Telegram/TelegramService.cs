using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using spr311_web_api.BLL.Services.Bot;
using spr311_web_api.BLL.Services.Role;
using spr311_web_api.DAL.Entities.Identity;
using spr311_web_api.DAL.settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace spr311_web_api.BLL.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient _client;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRoleService _roleService;

        public TelegramService(ITelegramBotService botService, UserManager<AppUser> userManager, IRoleService roleService)
        {
            _client = botService.GetTelegramBot();
            _userManager = userManager;
            _roleService = roleService;
        }

        public async Task UpdateHandlerAsync(Update update)
        {
            if (update.Message != null)
            {
                var user = await SaveUserAsync(update.Message);

                if (update.Message.Type == MessageType.Text)
                {
                    await TextMessageHandlerAsync(update.Message, user);
                }
            }
            else if(update.CallbackQuery != null)
            {
                await InlineHanlderAsync(update);
            }
        }

        private async Task InlineHanlderAsync(Update update)
        {
            string id = update.CallbackQuery.Id;
            string data = update.CallbackQuery.Data;
            await _client.SendMessage(update.CallbackQuery.From.Id, update.CallbackQuery.Message.Id.ToString());
            await _client.AnswerCallbackQuery(id);


            var keyboard = new InlineKeyboardMarkup
                            (new List<List<InlineKeyboardButton>>
                            {
                                new List<InlineKeyboardButton>
                                {
                                    new InlineKeyboardButton("Створити", "/roles"),
                                    new InlineKeyboardButton("Видалити", "/user")
                                },
                                new List<InlineKeyboardButton>
                                {
                                    new InlineKeyboardButton("Товари", "/products"),
                                    new InlineKeyboardButton("Категорії", "/categories")
                                }
                            });

            await _client.EditMessageReplyMarkup(update.CallbackQuery.From.Id, update.CallbackQuery.Message.Id, keyboard);
        }

        private async Task<AppUser?> SaveUserAsync(Message message)
        {
            long chatId = message.Chat.Id;
            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.ChatId == chatId);

            if (message.From != null
                && message.From.Username != null
                && user == null)
            {
                string userName = message.From.Username;

                user = new AppUser
                {
                    UserName = userName,
                    ChatId = chatId,
                    Email = $"{userName}@spr311.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, RoleSettings.UserRole);
            }

            return user;
        }

        private async Task TextMessageHandlerAsync(Message message, AppUser? user)
        {
            if (message.Text != null)
            {
                long chatId = message.Chat.Id;
                string text = message.Text;
                string answer = "Оберіть дію";

                switch(text)
                {
                    case "/random_number":
                        int randomNumber = new Random().Next(1, 1000);
                        answer = $"Твоє випадкове число: {randomNumber}";
                        break;
                    case "/roles":
                        if(user != null 
                            && await _userManager.IsInRoleAsync(user, RoleSettings.AdminRole))
                        {
                            var roles = await _roleService.GetAllAsync();
                            answer = string.Join(", ", roles.Select(r => r.Name));
                        }
                        else
                        {
                            answer = "У вас не достатньо прав";
                        }
                        break;
                    case "/menu":
                        answer = "Оберіть один із пунктів";

                        var keyboard = new InlineKeyboardMarkup
                            (new List<List<InlineKeyboardButton>>
                            {
                                new List<InlineKeyboardButton>
                                {
                                    new InlineKeyboardButton("Ролі", "/roles"),
                                    new InlineKeyboardButton("Користувачі", "/user")
                                },
                                new List<InlineKeyboardButton>
                                {
                                    new InlineKeyboardButton("Товари", "/products"),
                                    new InlineKeyboardButton("Категорії", "/categories")
                                }
                            });

                        await _client.DeleteMessage(message.Chat.Id, message.Id);

                        await _client.SendMessage(
                            chatId,
                            answer,
                            replyMarkup: keyboard);
                        return;
                }                

                await _client.SendMessage(chatId, answer,
                    replyMarkup: new string[][] 
                    { 
                        [ "Кнопка 1", "Кнопка 2" ],
                        [ "Кнопка 3", "Кнопка 4" ]
                    });
            }
        }
    }
}

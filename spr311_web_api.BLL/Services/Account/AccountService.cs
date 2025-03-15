using Microsoft.AspNetCore.Identity;
using spr311_web_api.BLL.Dtos.Account;
using spr311_web_api.DAL.Entities.Identity;

namespace spr311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser?> RegisterAsync(RegisterDto dto)
        {
            if(!await IsUniqueEmailAsync(dto.Email))
            {
                return null;
            }
            if (!await IsUniqueUserNameAsync(dto.UserName))
            {
                return null;
            }

            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if(result.Succeeded)
            {
                return user;
            }

            return null;
        }

        private async Task<bool> IsUniqueEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        private async Task<bool> IsUniqueUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user == null;
        }
    }
}

using spr311_web_api.BLL.Dtos.Account;
using spr311_web_api.DAL.Entities.Identity;

namespace spr311_web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        Task<AppUser?> RegisterAsync(RegisterDto dto);
    }
}

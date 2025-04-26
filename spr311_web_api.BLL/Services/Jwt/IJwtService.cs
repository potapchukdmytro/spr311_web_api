using spr311_web_api.BLL.Dtos.Auth;
using spr311_web_api.DAL.Entities.Identity;

namespace spr311_web_api.BLL.Services.Jwt
{
    public interface IJwtService
    {
        Task<ServiceResponse> GenerateTokensAsync(AppUser user);
        Task<ServiceResponse> RefreshTokensAsync(JwtDto dto);
    }
}

using spr311_web_api.DAL.Entities;

namespace spr311_web_api.DAL.Repositories.Auth
{
    public interface IAuthRepository
        : IGenericRepository<RefreshTokenEntity, string>
    {
    }
}

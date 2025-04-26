using spr311_web_api.DAL.Entities;

namespace spr311_web_api.DAL.Repositories.Auth
{
    public class AuthRepository
        : GenericRepository<RefreshTokenEntity, string>, IAuthRepository
    {
        public AuthRepository(AppDbContext context) 
            : base(context){}
    }
}

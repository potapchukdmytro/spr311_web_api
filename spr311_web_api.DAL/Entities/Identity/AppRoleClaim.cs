using Microsoft.AspNetCore.Identity;

namespace spr311_web_api.DAL.Entities.Identity
{
    public class AppRoleClaim : IdentityRoleClaim<string>
    {
        public virtual AppRole? Role { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace spr311_web_api.DAL.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual AppUser? User { get; set; }
        public virtual AppRole? Role { get; set; }
    }
}

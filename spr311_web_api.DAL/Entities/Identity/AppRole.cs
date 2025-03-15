using Microsoft.AspNetCore.Identity;

namespace spr311_web_api.DAL.Entities.Identity
{
    public class AppRole : IdentityRole
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; } = [];
    }
}

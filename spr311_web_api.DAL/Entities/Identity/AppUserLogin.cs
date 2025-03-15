using Microsoft.AspNetCore.Identity;

namespace spr311_web_api.DAL.Entities.Identity
{
    public class AppUserLogin : IdentityUserLogin<string>
    {
        public virtual AppUser? User { get; set; }
    }
}

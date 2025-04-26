using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace spr311_web_api.DAL.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        [MaxLength(255)]
        public string? FirstName { get; set; }
        [MaxLength(255)]
        public string? LastName { get; set; }
        [MaxLength(255)]
        public string? Image { get; set; }
        public long ChatId { get; set; }

        public virtual ICollection<AppUserClaim> Claims { get; set; } = [];
        public virtual ICollection<AppUserLogin> Logins { get; set; } = [];
        public virtual ICollection<AppUserToken> Tokens { get; set; } = [];
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}

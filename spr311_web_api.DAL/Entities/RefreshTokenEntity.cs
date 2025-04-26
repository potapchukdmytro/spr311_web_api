using spr311_web_api.DAL.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spr311_web_api.DAL.Entities
{
    public class RefreshTokenEntity : BaseEntity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(450)]
        public required string Token { get; set; }
        [MaxLength(256)]
        public required string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiredDate { get; set; }

        [ForeignKey("User")]
        public required string UserId { get; set; }
        public AppUser? User { get; set; }
    }
}

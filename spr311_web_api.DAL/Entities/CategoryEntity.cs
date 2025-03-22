using System.ComponentModel.DataAnnotations;

namespace spr311_web_api.DAL.Entities
{
    public class CategoryEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
        [MaxLength(255)]
        public string? NormalizedName { get; set; }
        [MaxLength]
        public string? Description { get; set; }
        [MaxLength(255)]
        public string? Image { get; set; }

        public ICollection<ProductEntity> Products { get; set; } = [];
    }
}

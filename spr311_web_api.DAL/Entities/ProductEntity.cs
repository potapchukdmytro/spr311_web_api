using System.ComponentModel.DataAnnotations;

namespace spr311_web_api.DAL.Entities
{
    public class ProductEntity : BaseEntity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
        [MaxLength]
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }

        public ICollection<CategoryEntity> Categories { get; set; } = [];
        public ICollection<ProductImageEntity> Images { get; set; } = [];
    }
}

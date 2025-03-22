using Microsoft.AspNetCore.Http;

namespace spr311_web_api.BLL.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}

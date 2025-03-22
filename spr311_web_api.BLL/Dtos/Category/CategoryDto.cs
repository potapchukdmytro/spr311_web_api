namespace spr311_web_api.BLL.Dtos.Category
{
    public class CategoryDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}

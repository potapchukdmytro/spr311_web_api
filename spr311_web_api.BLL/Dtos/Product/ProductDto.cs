namespace spr311_web_api.BLL.Dtos.Product
{
    public class ProductDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public List<string> Categories { get; set; } = [];
        public List<string> Images { get; set; } = [];
    }
}

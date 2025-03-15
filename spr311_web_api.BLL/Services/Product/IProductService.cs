using spr311_web_api.BLL.Dtos.Product;

namespace spr311_web_api.BLL.Services.Product
{
    public interface IProductService
    {
        public Task<bool> CreateAsync(CreateProductDto dto); 
        public Task<bool> UpdateAsync(UpdateProductDto dto); 
        public Task<bool> DeleteAsync(string id);
        public Task<ProductDto?> GetByIdAsync(string id);
        public Task<IEnumerable<ProductDto>> GetAllAsync();
    }
}

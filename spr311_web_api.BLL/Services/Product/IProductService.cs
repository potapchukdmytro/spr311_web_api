using spr311_web_api.BLL.Dtos.Product;

namespace spr311_web_api.BLL.Services.Product
{
    public interface IProductService
    {
        public Task<ServiceResponse> CreateAsync(CreateProductDto dto); 
        public Task<bool> UpdateAsync(UpdateProductDto dto); 
        public Task<bool> DeleteAsync(string id);
        public Task<ProductDto?> GetByIdAsync(string id);
        public Task<ServiceResponse> GetAllAsync(string? category);
    }
}

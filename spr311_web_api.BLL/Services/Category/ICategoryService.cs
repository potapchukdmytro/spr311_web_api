using spr311_web_api.BLL.Dtos.Category;

namespace spr311_web_api.BLL.Services.Category
{
    public interface ICategoryService
    {
        Task<bool> CreateAsync(CreateCategoryDto dto);
        Task<bool> UpdateAsync(UpdateCategoryDto dto);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(string id);
        Task<CategoryDto?> GetByNameAsync(string name);
    }
}

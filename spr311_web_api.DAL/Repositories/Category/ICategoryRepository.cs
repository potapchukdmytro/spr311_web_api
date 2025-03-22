using spr311_web_api.DAL.Entities;

namespace spr311_web_api.DAL.Repositories.Category
{
    public interface ICategoryRepository
    {
        Task<bool> CreateAsync(CategoryEntity entity);
        Task<bool> UpdateAsync(CategoryEntity entity);
        Task<bool> DeleteAsync(CategoryEntity entity);
        Task<CategoryEntity?> GetByIdAsync(string id);
        Task<CategoryEntity?> GetByNameAsync(string name);
        IQueryable<CategoryEntity> GetAll();
        bool IsUniqueName(string name);
    }
}

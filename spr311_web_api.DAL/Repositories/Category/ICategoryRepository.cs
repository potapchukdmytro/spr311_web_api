using spr311_web_api.DAL.Entities;

namespace spr311_web_api.DAL.Repositories.Category
{
    public interface ICategoryRepository
        : IGenericRepository<CategoryEntity, string>
    {
        Task<CategoryEntity?> GetByNameAsync(string name);
        bool IsUniqueName(string name);
    }
}

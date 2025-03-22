using Microsoft.EntityFrameworkCore;
using spr311_web_api.DAL.Entities;

namespace spr311_web_api.DAL.Repositories.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(CategoryEntity entity)
        {
            if (IsUniqueName(entity.Name))
            {
                entity.NormalizedName = entity.Name.ToUpper();
                await _context.Categories.AddAsync(entity);
                int result = await _context.SaveChangesAsync();

                return result != 0;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(CategoryEntity entity)
        {
            _context.Categories.Remove(entity);
            int result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public IQueryable<CategoryEntity> GetAll()
        {
            return _context.Categories;
        }

        public async Task<CategoryEntity?> GetByIdAsync(string id)
        {
            var entity = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
            return entity;
        }

        public async Task<CategoryEntity?> GetByNameAsync(string name)
        {
            var entity = await _context.Categories
                .FirstOrDefaultAsync(c => c.NormalizedName == name.ToUpper());
            return entity;
        }

        public bool IsUniqueName(string name)
        {
            return !_context.Categories
                .Any(c => c.NormalizedName == name.ToUpper());
        }

        public async Task<bool> UpdateAsync(CategoryEntity entity)
        {
            if (IsUniqueName(entity.Name))
            {
                entity.NormalizedName = entity.Name.ToUpper();
                _context.Categories.Update(entity);
                int result = await _context.SaveChangesAsync();
                return result != 0;
            }
            return false;
        }
    }
}

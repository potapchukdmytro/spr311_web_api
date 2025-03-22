using AutoMapper;
using Microsoft.EntityFrameworkCore;
using spr311_web_api.BLL.Dtos.Category;
using spr311_web_api.DAL.Entities;
using spr311_web_api.DAL.Repositories.Category;

namespace spr311_web_api.BLL.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(CreateCategoryDto dto)
        {
            var entity = _mapper.Map<CategoryEntity>(dto);

            bool result = await _categoryRepository.CreateAsync(entity);
            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            bool result = await _categoryRepository.DeleteAsync(entity);
            return result;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var entities = await _categoryRepository
                .GetAll()
                .ToListAsync();

            var dtos = _mapper.Map<List<CategoryDto>>(entities);

            return dtos;
        }

        public async Task<CategoryDto?> GetByIdAsync(string id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);

            if(entity != null)
            {
                var dto = _mapper.Map<CategoryDto>(entity);
                return dto;
            }

            return null;
        }

        public async Task<CategoryDto?> GetByNameAsync(string name)
        {
            var entity = await _categoryRepository.GetByNameAsync(name);

            if (entity != null)
            {
                var dto = _mapper.Map<CategoryDto>(entity);
                return dto;
            }

            return null;
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
        {
            var entity = _mapper.Map<CategoryEntity>(dto);

            bool result = await _categoryRepository.UpdateAsync(entity);
            return result;
        }
    }
}

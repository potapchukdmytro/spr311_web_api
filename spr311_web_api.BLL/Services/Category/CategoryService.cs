using AutoMapper;
using Microsoft.EntityFrameworkCore;
using spr311_web_api.BLL.Dtos.Category;
using spr311_web_api.BLL.Services.Image;
using spr311_web_api.DAL.Entities;
using spr311_web_api.DAL.Repositories.Category;

namespace spr311_web_api.BLL.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IImageService imageService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<bool> CreateAsync(CreateCategoryDto dto)
        {
            var entity = _mapper.Map<CategoryEntity>(dto);

            if (dto.Image != null)
            {
                string? imageName = await _imageService.SaveImageAsync(dto.Image, Settings.CategoriesDir);

                if(!string.IsNullOrEmpty(entity.Image))
                {
                    entity.Image = Settings.CategoriesDir + "/" + imageName;
                }
            }

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

            if (!string.IsNullOrEmpty(entity.Image))
            {
                _imageService.DeleteImage(entity.Image);
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
            var entity = await _categoryRepository.GetByIdAsync(dto.Id);

            if(entity == null)
            {
                return false;
            }

            entity = _mapper.Map(dto, entity);

            if(dto.Image != null)
            {
                string? imageName = await _imageService.SaveImageAsync(dto.Image, Settings.CategoriesDir);

                if (!string.IsNullOrEmpty(entity.Image))
                {
                    _imageService.DeleteImage(entity.Image);
                }
                entity.Image = Settings.CategoriesDir + "/" + imageName;
            }

            bool result = await _categoryRepository.UpdateAsync(entity);
            return result;
        }
    }
}

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

        public async Task<ServiceResponse> CreateAsync(CreateCategoryDto dto)
        {
            if(!_categoryRepository.IsUniqueName(dto.Name))
            {
                return ServiceResponse.Error($"Категорія з іменем '{dto.Name}' вже існує");
            }

            var entity = _mapper.Map<CategoryEntity>(dto);

            if (dto.Image != null)
            {
                string? imageName = await _imageService.SaveImageAsync(dto.Image, Settings.CategoriesDir);

                if(!string.IsNullOrEmpty(imageName))
                {
                    entity.Image = Settings.CategoriesDir + "/" + imageName;
                }
            }

            bool result = await _categoryRepository.CreateAsync(entity);

            if (result)
            {
                return ServiceResponse.Success("Категорію успішно додано");
            }

            return ServiceResponse.Error("Не вдалося додати категорію");
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Категорію з id '{id}' не знайдено");
            }

            if (!string.IsNullOrEmpty(entity.Image))
            {
                _imageService.DeleteImage(entity.Image);
            }

            bool result = await _categoryRepository.DeleteAsync(entity);

            if (result)
            {
                return ServiceResponse.Success("Категорію успішно видалено");
            }

            return ServiceResponse.Error("Не вдалося видалити категорію");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _categoryRepository
                .GetAll()
                .ToListAsync();

            var dtos = _mapper.Map<List<CategoryDto>>(entities);

            return ServiceResponse.Success("Категорії отримано", dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);

            if(entity != null)
            {
                var dto = _mapper.Map<CategoryDto>(entity);
                return ServiceResponse.Success($"Категорію {entity.Name} отримано", dto);
            }

            return ServiceResponse.Error("Категорію не знайдено");
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            var entity = await _categoryRepository.GetByNameAsync(name);

            if (entity != null)
            {
                var dto = _mapper.Map<CategoryDto>(entity);
                return ServiceResponse.Success($"Категорію '{entity.Name}' отримано", dto);
            }

            return ServiceResponse.Error($"Категорію з іменем '{name}' не знайдено");
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateCategoryDto dto)
        {
            if (!_categoryRepository.IsUniqueName(dto.Name))
            {
                return ServiceResponse.Error($"Категорія з іменем '{dto.Name}' вже існує");
            }

            var entity = await _categoryRepository.GetByIdAsync(dto.Id);

            if(entity == null)
            {
                return ServiceResponse.Error($"Категорію з id '{dto.Id}' не знайдено");
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
            
            if(result)
            {
                return ServiceResponse.Success("Категорію успішно оновлено");
            }

            return ServiceResponse.Error("Не вдалося оновити категорію");
        }
    }
}

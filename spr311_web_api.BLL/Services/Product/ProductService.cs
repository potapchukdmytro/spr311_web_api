using AutoMapper;
using Microsoft.EntityFrameworkCore;
using spr311_web_api.BLL.Dtos.Product;
using spr311_web_api.BLL.Services.Image;
using spr311_web_api.DAL;
using spr311_web_api.DAL.Entities;
using spr311_web_api.DAL.Repositories.Category;
using spr311_web_api.DAL.Repositories.Product;

namespace spr311_web_api.BLL.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageService _imageService;

        public ProductService(AppDbContext context, IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _imageService = imageService;
        }

        public async Task<ServiceResponse> CreateAsync(CreateProductDto dto)
        {
            var entity = _mapper.Map<ProductEntity>(dto);

            // images
            if (dto.Images.Count > 0)
            {
                string dirPath = Path.Combine(Settings.ProductsDir, entity.Id);

                var imagesName = await _imageService.SaveProductImagesAsync(dto.Images, dirPath);

                var imageEntities = imagesName.Select(i =>
                    new ProductImageEntity
                    {
                        Name = i,
                        Path = dirPath,
                        ProductId = entity.Id
                    });

                entity.Images = imageEntities.ToArray();
            }

            // categories
            var categories = _categoryRepository
                .GetAll()
                .Where(c => dto.Categories.Select(x => x.ToUpper()).Contains(c.NormalizedName))
                .ToList();

            entity.Categories = categories;

            var result = await _productRepository.CreateAsync(entity);

            if (result)
            {
                return ServiceResponse.Success("Товар додано");
            }

            return ServiceResponse.Error("Не вдалося додати товар");
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            _context.Products.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ServiceResponse> GetAllAsync(string? category)
        {
            var entities = _productRepository
                .GetAll()
                .Include(p => p.Categories)
                .Include(p => p.Images)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(category))
            {
                entities = entities.Where(e => e.Categories.Select(c => c.NormalizedName).Contains(category.ToUpper()));
            }

            var list = await entities.ToListAsync();

            var dtos = _mapper.Map<List<ProductDto>>(list);

            return ServiceResponse.Success("Товари отримано", dtos);
        }

        public async Task<ProductDto?> GetByIdAsync(string id)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return null;
            }

            var dto = new ProductDto
            {
                Amount = entity.Amount,
                Description = entity.Description,
                Price = entity.Price,
                Id = entity.Id,
                Name = entity.Name
            };

            return dto;
        }

        public async Task<bool> UpdateAsync(UpdateProductDto dto)
        {
            var entity = new ProductEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Amount = dto.Amount,
                Description = dto.Description,
                Price = dto.Price
            };

            _context.Products.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}

using spr311_web_api.DAL.Entities;

namespace spr311_web_api.DAL.Repositories.Product
{
    public class ProductRepository
        : GenericRepository<ProductEntity, string>, IProductRepository
    {
        public ProductRepository(AppDbContext context) 
            : base(context){}
    }
}

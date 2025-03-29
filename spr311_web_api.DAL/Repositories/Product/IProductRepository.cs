using spr311_web_api.DAL.Entities;

namespace spr311_web_api.DAL.Repositories.Product
{
    public interface IProductRepository
        : IGenericRepository<ProductEntity, string>
    {
    }
}

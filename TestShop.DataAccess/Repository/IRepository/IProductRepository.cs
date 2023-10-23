using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository.IRepository;

public interface IProductRepository:IRepository<Product>
{
    Task Update(Product obj);
}
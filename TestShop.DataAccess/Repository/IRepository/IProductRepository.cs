using TestShop.Models;

namespace TestShop.DataAccess.Repository.IRepository;

public interface IProductRepository:IRepository<Product>
{
    void Update(Product obj);
}
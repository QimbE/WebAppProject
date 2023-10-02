using TestShop.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task Update(ShoppingCart obj);
    }
}
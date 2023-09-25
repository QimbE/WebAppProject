using TestShop.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository: IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
    }
}
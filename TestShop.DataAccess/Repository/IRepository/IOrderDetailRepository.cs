using TestShop.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository: IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
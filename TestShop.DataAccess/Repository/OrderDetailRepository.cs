using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;

namespace TestShop.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
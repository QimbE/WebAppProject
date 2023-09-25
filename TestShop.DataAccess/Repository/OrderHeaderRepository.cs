using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;

namespace TestShop.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }
    }
}
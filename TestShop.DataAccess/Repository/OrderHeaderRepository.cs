using Microsoft.EntityFrameworkCore;
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

        public async Task Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public async Task UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
	        var order = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
	        if (order != null)
	        {
                order.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    order.PaymentStatus = paymentStatus;
                }

	        }
        }

        public async Task UpdateStripePaymentId(int id, string sessionId, string? paymentIntentId = null)
        {
			var order = _db.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id);
			if (!string.IsNullOrEmpty(sessionId))
			{
				order.Result.SessionId = sessionId;
			}
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				order.Result.PaymentIntentId = paymentIntentId;
                order.Result.PaymentDate = DateTime.Now;
			}
		}
	}
}
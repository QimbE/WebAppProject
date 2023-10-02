using TestShop.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository: IRepository<OrderHeader>
    {
        Task Update(OrderHeader obj);
        Task UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        Task UpdateStripePaymentId(int id, string sessionId, string? paymentIntentId = null);
	}
}
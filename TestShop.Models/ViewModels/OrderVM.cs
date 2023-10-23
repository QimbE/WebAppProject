using TestShop.Models.Models;

namespace TestShop.Models.ViewModels;

public class OrderVM
{
	public OrderHeader OrderHeader { get; set; }
	public IEnumerable<OrderDetail>  OrderDetails { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace TestShop.Models.ViewModels;

public class ShoppingCartVM
{
	public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

	public OrderHeader OrderHeader { get; set; }
}
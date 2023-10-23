using System.ComponentModel.DataAnnotations.Schema;
using TestShop.Models.Models;

namespace TestShop.Models.ViewModels;

public class ShoppingCartVM
{
	public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

	public OrderHeader OrderHeader { get; set; }
}
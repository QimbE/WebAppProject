using System.ComponentModel.DataAnnotations.Schema;

namespace TestShop.Models.ViewModels;

public class ShoppingCartVM
{
	public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
	[Column(TypeName = "decimal(18, 2)")]
	public decimal OrderTotal { get; set; }
}
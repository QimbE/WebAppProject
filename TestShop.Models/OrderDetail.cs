using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TestShop.Models;

public class OrderDetail
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int OrderHeaderId { get; set; }
	[ValidateNever]
	[ForeignKey("OrderHeaderId")]
	public OrderHeader OrderHeader { get; set; }

	[Required]
	public int ProductId { get; set; }
	[ForeignKey("ProductId")]
	[ValidateNever]
	public Product Product { get; set; }

	public int Count { get; set; }
	[Column(TypeName = "decimal(18, 2)")]
	public decimal Price { get; set; }
}
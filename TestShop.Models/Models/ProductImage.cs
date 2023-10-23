using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestShop.Models.Models;

public class ProductImage
{
    public int Id { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
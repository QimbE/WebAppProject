using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TestShop.Models.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        [ValidateNever]
        public List<Product> Products { get; set; }
    }
}

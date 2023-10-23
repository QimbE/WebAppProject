using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TestShop.Models.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }

        [Display(Name = "List Price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ListPrice { get; set; }

        [Display(Name = "Price for 1-50")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Price for 50+")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price50 { get; set; }

        [Display(Name = "Price for 100+")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price100 { get; set; }

        [ValidateNever]
        public List<Category> Categories { get; set; }

        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }
    }
}

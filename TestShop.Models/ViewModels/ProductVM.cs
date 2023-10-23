using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestShop.Models.Models;

namespace TestShop.Models.ViewModels
{
    public class ProductVM
	{
		public Product Product { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> CategoryList { get; set; }
		[Display(Name = "Categories")]
		public IEnumerable<int> CategoryIds { get; set; }
	}
}
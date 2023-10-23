using Microsoft.AspNetCore.Mvc.Rendering;
using TestShop.Models.Models;

namespace TestShop.Models.ViewModels;

public class RoleManagmentVM
{
	public ApplicationUser ApplicationUser { get; set; }
	public IEnumerable<SelectListItem> RoleList { get; set; }
	public IEnumerable<SelectListItem> CompanyList { get; set; }
}
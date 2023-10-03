using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Utility;

namespace TestShopProject.ViewComponents;

public class ShoppingCartViewComponent:ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null)
        {
            if (HttpContext.Session.GetInt32(StaticDetails.SessionCart)==null)
            {
                HttpContext.Session.SetInt32(StaticDetails.SessionCart, (await _unitOfWork.ShoppingCart
                    .GetAll(x => x.ApplicationUserId == claim.Value)).Count());
            }
            
            return View(HttpContext.Session.GetInt32(StaticDetails.SessionCart));
        }
        else
        {
            HttpContext.Session.Clear();
            return View(0);
        }
    }
}
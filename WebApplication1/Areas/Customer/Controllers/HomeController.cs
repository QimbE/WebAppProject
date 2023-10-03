using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Utility;

namespace TestShopProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
	        IEnumerable<Product> productList = await _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(productList);
        }
        public async Task<IActionResult> Details(int id)
        {
	        ShoppingCart cart = new()
	        {
		        Product = await _unitOfWork.Product.Get(x => x.Id == id, "Category"),
                Count = 1,
                ProductId = id
	        };
	        return View(cart);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
	        shoppingCart.Id = default(int);
	        ClaimsIdentity claimsIdentity= (ClaimsIdentity)User.Identity;
	        string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = await _unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId &&
										x.ProductId == shoppingCart.ProductId);
            if (cartFromDb is not null)
            {
                //cart already exist
                cartFromDb.Count += shoppingCart.Count;
                await _unitOfWork.ShoppingCart.Update(cartFromDb);
                await _unitOfWork.Save();
            }
            else
            {
				//add new cart
				await _unitOfWork.ShoppingCart.Add(shoppingCart);
                await _unitOfWork.Save();
                HttpContext.Session.SetInt32(StaticDetails.SessionCart, (await _unitOfWork.ShoppingCart
                    .GetAll(x => x.ApplicationUserId == userId)).Count());
			}

            TempData["success"] = "Cart updated successfully";

            

	        return RedirectToAction(nameof(Index));
        }

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
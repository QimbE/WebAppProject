using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;

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

        public IActionResult Index()
        {
	        IEnumerable<Product> productList = _unitOfWork.Product.GetAll("Category");
            return View(productList);
        }
        public IActionResult Details(int id)
        {
	        ShoppingCart cart = new()
	        {
		        Product = _unitOfWork.Product.Get(x => x.Id == id, "Category"),
                Count = 1,
                ProductId = id
	        };
	        return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
	        ClaimsIdentity claimsIdentity= (ClaimsIdentity)User.Identity;
	        string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId &&
										x.ProductId == shoppingCart.ProductId);
            if (cartFromDb is not null)
            {
                //cart already exist
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
				//add new cart
				_unitOfWork.ShoppingCart.Add(shoppingCart);
			}

            TempData["success"] = "Cart updated successfully";

            _unitOfWork.Save();

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
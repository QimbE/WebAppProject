using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Stripe.Checkout;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Models.ViewModels;
using TestShop.Utility;

namespace TestShopProject.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }

		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index()
		{
			ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
			string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = await _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId,
					includeProperties: "Product"),
				OrderHeader = new()
			};

			IEnumerable<ProductImage> productImages = await _unitOfWork.ProductImage.GetAll();

			foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
			{
				shoppingCart.Product.ProductImages = productImages.Where(x => x.ProductId == shoppingCart.ProductId).ToList();
				shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart);
				ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
			}

			return View(ShoppingCartVM);
		}

		public async Task<IActionResult> Summary()
		{
			ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
			string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = await _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId,
					includeProperties: "Product"),
				OrderHeader = new()
			};

			ShoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUser.Get(x => x.Id == userId);

			ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
			ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
			ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

			foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
			{
				shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart);
				ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
			}
			return View(ShoppingCartVM);
		}

		[HttpPost]
		[ActionName("Summary")]
		public async Task<IActionResult> SummaryPOST()
		{
			ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
			string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM.ShoppingCartList = await _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId,
				includeProperties: "Product");

			ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
			ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

			ApplicationUser applicationUser = await _unitOfWork.ApplicationUser.Get(x => x.Id == userId);


			foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
			{
				shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart);
				ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
			}

			if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			{
				//it is a regular user
				ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;

			}
			else
			{
				//it is a company user
				ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;
				ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusApproved;

			}
			await _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			await _unitOfWork.Save();
			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				OrderDetail orderDetail = new()
				{
					ProductId = cart.ProductId,
					OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
					Price = cart.Price,
					Count = cart.Count,

				};
				await _unitOfWork.OrderDetail.Add(orderDetail);
			}
			await _unitOfWork.Save();

			if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			{
				//it is a regular user
				//stripe logic

				var domain = Request.Scheme + "://" + Request.Host.Value + "/";

				var options = new SessionCreateOptions
				{
					SuccessUrl = domain+$"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
					CancelUrl = domain+$"customer/cart/index",
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
				};

				//adding goods to list
				foreach (var item in ShoppingCartVM.ShoppingCartList)
				{
					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.Price * 100), //20.50 == 2050
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Product.Title
							}
						},
						Quantity = item.Count
					};
					options.LineItems.Add(sessionLineItem);
				}

				//service creating
				var service = new SessionService();
				Session session = await service.CreateAsync(options);

				//updating ids
				await _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
				await _unitOfWork.Save();
				Response.Headers.Add("Location",session.Url);

				return new StatusCodeResult(303);
			}

			return RedirectToAction(nameof(OrderConfirmation), new {id = ShoppingCartVM.OrderHeader.Id});
		}

		public async Task<IActionResult> OrderConfirmation(int id)
		{
			OrderHeader orderHeader = await 
				_unitOfWork.OrderHeader.Get(x => x.Id == id, includeProperties: "ApplicationUser");

			if (orderHeader.PaymentStatus!=StaticDetails.PaymentStatusDelayedPayment)
			{
				//this is an order by customer.
				var service = new SessionService();
				Session session = await service.GetAsync(orderHeader.SessionId);

				if (session.PaymentStatus.ToLower() == "paid")
				{
					//_unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
					await _unitOfWork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
					await _unitOfWork.OrderHeader.UpdateStatus(id, StaticDetails.StatusApproved, StaticDetails.PaymentStatusApproved );
					await _unitOfWork.Save();
				}
				
			}

			List<ShoppingCart> carts = (await _unitOfWork.ShoppingCart.GetAll(x=> x.ApplicationUserId == orderHeader.ApplicationUserId)).ToList();
			await _unitOfWork.ShoppingCart.RemoveRange(carts);
			await _unitOfWork.Save();
			HttpContext.Session.SetInt32(StaticDetails.SessionCart, 0);
			return View(id);
		}

		public async Task<IActionResult> Plus(int cartId)
		{
			var cartFromDb = await _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
			cartFromDb.Count += 1;
			await _unitOfWork.ShoppingCart.Update(cartFromDb);
			await _unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Minus(int cartId)
		{
			var cartFromDb = await _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
			if (cartFromDb.Count <= 1)
			{
				await _unitOfWork.ShoppingCart.Remove(cartFromDb);
                HttpContext.Session.SetInt32(StaticDetails.SessionCart, (await _unitOfWork.ShoppingCart
                    .GetAll(x => x.ApplicationUserId == cartFromDb.ApplicationUserId)).Count() - 1);
            }
			else
			{
				cartFromDb.Count -= 1;
				await _unitOfWork.ShoppingCart.Update(cartFromDb);
			}

			await _unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Remove(int cartId)
		{
			var cartFromDb = await _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);

			await _unitOfWork.ShoppingCart.Remove(cartFromDb);
            HttpContext.Session.SetInt32(StaticDetails.SessionCart, (await _unitOfWork.ShoppingCart
                .GetAll(x => x.ApplicationUserId == cartFromDb.ApplicationUserId)).Count()-1);
            await _unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		private decimal GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
		{
			if (shoppingCart.Count <= 50)
			{
				return shoppingCart.Product.Price;
			}
			else if (shoppingCart.Count <= 100)
			{
				return shoppingCart.Product.Price50;
			}
			else
			{
				return shoppingCart.Product.Price100;
			}
		}
	}
}
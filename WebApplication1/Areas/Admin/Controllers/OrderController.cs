using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Utility;

namespace TestShopProject.Areas.Admin.Controllers
{
	[Area("admin")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			return View();
		}
		#region API CALLS

		[HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");

			switch (status)
			{
				case "pending":
					objOrderHeaders =
						objOrderHeaders.Where(x => x.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
					break;
				case "inprocess":
					objOrderHeaders =
						objOrderHeaders.Where(x => x.OrderStatus == StaticDetails.StatusInProcess);
					break;
				case "completed":
					objOrderHeaders =
						objOrderHeaders.Where(x => x.OrderStatus == StaticDetails.StatusShipped);
					break;
				case "approved":
					objOrderHeaders =
						objOrderHeaders.Where(x => x.OrderStatus == StaticDetails.StatusApproved);
					break;
				default:

					break;
			}

			return Json(objOrderHeaders);
		}
		#endregion
	}
}

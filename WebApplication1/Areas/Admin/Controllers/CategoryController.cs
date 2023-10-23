using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Concurrent;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models.Models;
using TestShop.Models.ViewModels;
using TestShop.Utility;

namespace TestShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
	        return View();
        }

		/// <summary>
		/// HtttpGet for upsert action
		/// </summary>
		/// <param name="id">Category id</param>
		/// <returns>Upsert View</returns>
		public async Task<IActionResult> Upsert(int? id)
		{
			if (id is null || id == 0)
			{
				//Create case
				return View(new Category());
			}
			else
			{
				//Update case
				Category category = await _unitOfWork.Category.Get(x => x.Id == id);
				return View(category);
			}

		}

		/// <summary>
		/// HttpPost upsert action method
		/// </summary>
		/// <param name="id">Category id</param>
		/// <returns>Redirects to index page if success, else returns Upsert View</returns>
		[HttpPost, ActionName("Upsert")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpsertPOST(Category? obj)
		{
			if (ModelState.IsValid)
			{
				if (obj.Id == 0)
				{
					await _unitOfWork.Category.Add(obj);
				}
				else
				{
					await _unitOfWork.Category.Update(obj);
				}
				await _unitOfWork.Save();
				TempData["success"] = (obj.Id != 0) ? "Category updated successfully" : "Category created successfully";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				return View(obj);
			}
		}

		#region API CALLS

		[HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Category> categoryList = await _unitOfWork.Category.GetAll();
            return Json(categoryList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
			Category categoryToBeDeleted = await _unitOfWork.Category.Get(x => x.Id == id);
			if (categoryToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			await _unitOfWork.Category.Remove(categoryToBeDeleted);
			await _unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}
		#endregion


	}
}

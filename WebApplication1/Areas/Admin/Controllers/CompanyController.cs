using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Models.ViewModels;
using TestShop.Utility;

namespace TestShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

		}
        public async Task<IActionResult> Index()
        {
            IEnumerable<Company> objProductList = await _unitOfWork.Company.GetAll();
            
            return View(objProductList);
        }
        //GET
        // Update/Insert
        public async Task<IActionResult> Upsert(int? id)
        {
            if (id is null || id == 0)
            {
                //Create case
	            return View(new Company());
			}
            else
            {
                //Update case
                Company company = await _unitOfWork.Company.Get(x => x.Id==id);
				return View(company);
			}
			
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
	            if (obj.Id == 0)
	            {
		            await _unitOfWork.Company.Add(obj);
				}
	            else
	            {
		            await _unitOfWork.Company.Update(obj);
				}
                await _unitOfWork.Save();
                TempData["success"] = (obj.Id!=0)? "Company updated successfully": "Company created successfully";
                return RedirectToAction("Index");
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
	        IEnumerable<Company> objCompanyList = await _unitOfWork.Company.GetAll();
	        return Json(objCompanyList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            Company сompanyToBeDeleted = await _unitOfWork.Company.Get(x => x.Id == id);
	        if (сompanyToBeDeleted == null)
	        {
		        return Json(new { success = false, message = "Error while deleting" });
	        }

            await _unitOfWork.Company.Remove(сompanyToBeDeleted);
            await _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}

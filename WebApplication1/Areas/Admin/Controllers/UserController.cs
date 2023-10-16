using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;
using TestShop.Models.ViewModels;
using TestShop.Utility;

namespace TestShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
	    private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
	        _unitOfWork = unitOfWork;
			_roleManager = roleManager;
			_userManager = userManager;
		}
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> RoleManagment(string userId)
        {
	        RoleManagmentVM roleVM = new()
	        {
                ApplicationUser = await _unitOfWork.ApplicationUser
	                .Get(x=> x.Id == userId, includeProperties:"Company"),
                RoleList = _roleManager.Roles.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                CompanyList = (await _unitOfWork.Company.GetAll()).Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
				})
	        };

	        roleVM.ApplicationUser.Role = (await _userManager
		        .GetRolesAsync(await _unitOfWork.ApplicationUser
			        .Get(x => x.Id == userId))).FirstOrDefault();

	        return View(roleVM);
        }

        [HttpPost]
        public async Task<IActionResult> RoleManagment(RoleManagmentVM roleManagmentVM)
        {
	        string oldRole = (await _userManager
		        .GetRolesAsync(await _unitOfWork.ApplicationUser
			        .Get(x => x.Id == roleManagmentVM.ApplicationUser.Id)))
		        .FirstOrDefault();
			ApplicationUser applicationUser = await _unitOfWork.ApplicationUser
		        .Get(x => x.Id == roleManagmentVM.ApplicationUser.Id);

	        

			if (roleManagmentVM.ApplicationUser.Role != oldRole)
	        {
				//user role was updated
				

				if (roleManagmentVM.ApplicationUser.Role == StaticDetails.Role_Company)
				{
					//assign to company
					applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
				}

				if (oldRole == StaticDetails.Role_Company)
				{
					//unassign user
					applicationUser.CompanyId = null;
				}

				await _unitOfWork.ApplicationUser.Update(applicationUser);

				await _unitOfWork.Save();

				await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);

				await _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role);
	        }
	        else if (oldRole==StaticDetails.Role_Company && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId)
	        {
				//if it is a company user, we should check if the company changed

		        applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;

		        await _unitOfWork.ApplicationUser.Update(applicationUser);

		        await _unitOfWork.Save();
				

	        }
			//to the user list page
	        return RedirectToAction(nameof(Index));
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
	        IEnumerable<ApplicationUser> objUserList = await _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company");


	        foreach (var user in objUserList)
	        {
		        user.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                user.Company ??= new Company
                {
                    Name = ""
                };
            }
	        return Json(objUserList);
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody]string id)
        {
	        ApplicationUser objFromDb = await _unitOfWork.ApplicationUser.Get(x => x.Id ==id);

	        if (objFromDb == null)
	        {
		        return Json(new { success = false, message = "Can't find such user in database"});
	        }

	        if (objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now)
	        {
                //if user is locked - unlocking
                objFromDb.LockoutEnd = DateTime.Now;
	        }
	        else
	        {
				//if user is unlocked - locking
				objFromDb.LockoutEnd = DateTimeOffset.Now.AddYears(1000);
	        }

	        await _unitOfWork.ApplicationUser.Update(objFromDb);
	        await _unitOfWork.Save();
            return Json(new {success = true, message = "Operation Successful"});
        }

		#endregion
	}
}

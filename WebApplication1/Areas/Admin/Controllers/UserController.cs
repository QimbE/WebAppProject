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
        private readonly ApplicationDbContext _db;
		private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
	        _db = db;
			_userManager = userManager;
		}
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> RoleManagment(string userId)
        {
	        string roleId = (await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId)).RoleId;
	        RoleManagmentVM roleVM = new()
	        {
                ApplicationUser = await _db.ApplicationUsers
	                .Include(x=> x.Company)
	                .FirstOrDefaultAsync(x => x.Id == userId),
                RoleList = _db.Roles.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                CompanyList = _db.Companies.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
				})
	        };

	        roleVM.ApplicationUser.Role = (await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleId)).Name;

	        return View(roleVM);
        }

        [HttpPost]
        public async Task<IActionResult> RoleManagment(RoleManagmentVM roleManagmentVM)
        {
	        string roleId = (await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == roleManagmentVM.ApplicationUser.Id)).RoleId;
	        string oldRole = (await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleId)).Name;

	        if (roleManagmentVM.ApplicationUser.Role != oldRole)
	        {
				//user role was updated
				ApplicationUser applicationUser = await _db.ApplicationUsers
					.Include(x => x.Company)
					.FirstOrDefaultAsync(x => x.Id == roleManagmentVM.ApplicationUser.Id);

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

				await _db.SaveChangesAsync();

				await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);

				await _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role);
	        }
	        else if (roleManagmentVM.ApplicationUser.Role == StaticDetails.Role_Company)
	        {
				//if it is a company user, we should check if the company changed
		        ApplicationUser applicationUser = await _db.ApplicationUsers
			        .Include(x => x.Company)
			        .FirstOrDefaultAsync(x => x.Id == roleManagmentVM.ApplicationUser.Id);
				if (applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId)
		        {
			        applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
			        await _db.SaveChangesAsync();
				}

	        }
			//to the user list page
	        return RedirectToAction(nameof(Index));
        }
        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
	        IEnumerable<ApplicationUser> objUserList = await _db.ApplicationUsers.Include(x => x.Company).ToListAsync();

	        var userRoles = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();


	        foreach (var user in objUserList)
	        {

		        var roleId = userRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;
		        user.Role = roles.FirstOrDefault(x => x.Id == roleId).Name;


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
	        ApplicationUser objFromDb = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id ==id);

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
	        await _db.SaveChangesAsync();
            return Json(new {success = true, message = "Operation Successful"});
        }

		#endregion
	}
}

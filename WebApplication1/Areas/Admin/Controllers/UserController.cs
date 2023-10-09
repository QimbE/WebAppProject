using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
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
        public UserController(ApplicationDbContext db)
        {
	        _db = db;

		}
        public async Task<IActionResult> Index()
        {
            return View();
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

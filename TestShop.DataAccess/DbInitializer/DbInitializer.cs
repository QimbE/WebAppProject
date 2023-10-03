using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestShop.DataAccess.Data;
using TestShop.Models;
using TestShop.Utility;

namespace TestShop.DataAccess.DbInitializer;

public class DbInitializer:IDbInitializer
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly ApplicationDbContext _db;

	public DbInitializer(
		UserManager<IdentityUser> userManager,
		RoleManager<IdentityRole> roleManager,
		ApplicationDbContext db)
	{
		_roleManager = roleManager;
		_userManager = userManager;
		_db = db;
	}
	public async Task InitializeAsync()
	{
		//Apply migrations.
		try
		{
			if ((await _db.Database.GetPendingMigrationsAsync()).Any())
			{
				await _db.Database.MigrateAsync();
			}
		}
		catch (OperationCanceledException ex)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw;
		}

		//Create roles if needed.
		if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_Custumer))
		{
			await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Custumer));
			await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee));
			await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin));
			await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company));

			//Create first admin user.
			await _userManager.CreateAsync(new ApplicationUser
			{
				UserName = "admin@adm.com",
				Email = "admin@adm.com",
				Name = "Mr Admin",
				PhoneNumber = "123123123",
				StreetAddress = "Baker 221B street",
				State = "UK",
				PostalCode = "123123",
				City = "Moscow"
			}, "Admin123;");

			ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@adm.com");
			await _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin);
		}

	}
}
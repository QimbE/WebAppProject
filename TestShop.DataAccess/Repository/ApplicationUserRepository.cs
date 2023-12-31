﻿using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository;

public class ApplicationUserRepository:Repository<ApplicationUser>, IApplicationUserRepository
{
	private readonly ApplicationDbContext _db;
	public ApplicationUserRepository(ApplicationDbContext db)
		: base(db)
	{
		_db = db;
	}


	public async Task Update(ApplicationUser applicationUser)
	{
		_db.ApplicationUsers.Update(applicationUser);
	}
}
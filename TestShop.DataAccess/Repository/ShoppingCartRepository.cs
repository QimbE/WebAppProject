using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;

namespace TestShop.DataAccess.Repository;

public class ShoppingCartRepository:Repository<ShoppingCart>, IShoppingCartRepository
{
	private readonly ApplicationDbContext _db;
	public ShoppingCartRepository(ApplicationDbContext db)
		: base(db)
	{
		_db = db;
	}

	public async Task Update(ShoppingCart obj)
	{
		_db.ShoppingCarts.Update(obj);
	}
}
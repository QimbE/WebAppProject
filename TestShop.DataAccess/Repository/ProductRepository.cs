using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;

namespace TestShop.DataAccess.Repository;

public class ProductRepository: Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
        : base(db)
    {
        _db = db;
    }
    public async Task Update(Product obj)
    {
	    var objectFromDb = _db.Products.FirstOrDefaultAsync(x => x.Id == obj.Id);
	    if (objectFromDb != null)
	    {
            objectFromDb.Result.Title = obj.Title;
            objectFromDb.Result.Description = obj.Description;
            objectFromDb.Result.CategoryId = obj.CategoryId;
            objectFromDb.Result.ISBN = obj.ISBN;
            objectFromDb.Result.Price = obj.Price;
            objectFromDb.Result.ListPrice = obj.ListPrice;
            objectFromDb.Result.Author = obj.Author;
            objectFromDb.Result.Price50 = obj.Price50;
            objectFromDb.Result.Price100 = obj.Price100;
            //if (obj.ImageUrl != null)
            //{
	           // objectFromDb.Result.ImageUrl = obj.ImageUrl;
            //}
	    }
    }

    
}
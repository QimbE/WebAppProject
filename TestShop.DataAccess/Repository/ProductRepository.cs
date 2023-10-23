using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models.Models;

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
	    var objectFromDb = await _db.Products.FirstOrDefaultAsync(x => x.Id == obj.Id);
	    if (objectFromDb != null)
	    {
            objectFromDb.Title = obj.Title;
            objectFromDb.Description = obj.Description;
            objectFromDb.Categories = obj.Categories;
            objectFromDb.ISBN = obj.ISBN;
            objectFromDb.Price = obj.Price;
            objectFromDb.ListPrice = obj.ListPrice;
            objectFromDb.Author = obj.Author;
            objectFromDb.Price50 = obj.Price50;
            objectFromDb.Price100 = obj.Price100;
            objectFromDb.ProductImages = obj.ProductImages;
            //if (obj.ImageUrl != null)
            //{
	           // objectFromDb.Result.ImageUrl = obj.ImageUrl;
            //}
	    }
    }

    
}
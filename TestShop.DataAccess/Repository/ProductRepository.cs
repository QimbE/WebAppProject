using System.Linq.Expressions;
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
    public void Update(Product obj)
    {
	    var objectFromDb = _db.Products.FirstOrDefault(x => x.Id == obj.Id);
	    if (objectFromDb != null)
	    {
            objectFromDb.Title = obj.Title;
            objectFromDb.Description = obj.Description;
            objectFromDb.CategoryId = obj.CategoryId;
            objectFromDb.ISBN = obj.ISBN;
            objectFromDb.Price = obj.Price;
            objectFromDb.ListPrice = obj.ListPrice;
            objectFromDb.Author = obj.Author;
            objectFromDb.Price50 = obj.Price50;
            objectFromDb.Price100 = obj.Price100;
            if (obj.ImageUrl != null)
            {
	            objectFromDb.ImageUrl = obj.ImageUrl;
            }
	    }
    }

    
}
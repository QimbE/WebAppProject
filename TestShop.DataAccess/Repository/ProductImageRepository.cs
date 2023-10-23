using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
	{
        private readonly ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task Update(ProductImage obj)
        {
            _db.ProductImages.Update(obj);
        }
    }
}
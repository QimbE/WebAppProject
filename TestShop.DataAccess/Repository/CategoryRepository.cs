using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
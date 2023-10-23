using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task Update(Category obj);
    }
}
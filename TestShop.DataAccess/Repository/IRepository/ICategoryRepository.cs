using TestShop.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task Update(Category obj);
    }
}
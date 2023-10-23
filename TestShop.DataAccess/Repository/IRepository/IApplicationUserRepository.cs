using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
	    Task Update(ApplicationUser applicationUser);
    }
}
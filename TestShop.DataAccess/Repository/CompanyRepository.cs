using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models;

namespace TestShop.DataAccess.Repository;

public class CompanyRepository:Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _db;
    public CompanyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task Update(Company obj)
    {
        _db.Companies.Update(obj);
    }
}
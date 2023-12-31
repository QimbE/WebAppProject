﻿using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository.IRepository;

public interface ICompanyRepository:IRepository<Company>
{
    Task Update(Company obj);
}
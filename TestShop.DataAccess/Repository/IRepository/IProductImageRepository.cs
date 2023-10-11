﻿using TestShop.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IProductImageRepository: IRepository<ProductImage>
    {
        Task Update(ProductImage obj);
    }
}
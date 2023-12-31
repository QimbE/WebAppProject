﻿using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository: IRepository<OrderDetail>
    {
        Task Update(OrderDetail obj);
    }
}
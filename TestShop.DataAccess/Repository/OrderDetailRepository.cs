﻿using TestShop.DataAccess.Data;
using TestShop.DataAccess.Repository.IRepository;
using TestShop.Models.Models;

namespace TestShop.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
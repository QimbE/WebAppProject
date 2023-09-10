﻿using Microsoft.EntityFrameworkCore;
using TestShop.Models;

namespace TestShop.DataAccess.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
    }
}
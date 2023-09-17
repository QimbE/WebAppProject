﻿using Microsoft.EntityFrameworkCore;
using TestShop.Models;

namespace TestShop.DataAccess.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fear and Loathing in Las Vegas",
                    Description = "A wild journey to the heart of the American Dream.",
                    ISBN = "978-0679785897",
                    Author = "Hunter S. Thompson",
                    ListPrice = 19.99m,
                    Price = 15.99m,
                    Price50 = 12.99m,
                    Price100 = 10.99m
                },
                new Product
                {
                    Id = 2,
                    Title = "To Kill a Mockingbird",
                    Description = "A classic novel about racial injustice and moral growth.",
                    ISBN = "978-0061120084",
                    Author = "Harper Lee",
                    ListPrice = 14.99m,
                    Price = 12.49m,
                    Price50 = 10.99m,
                    Price100 = 9.99m
                },
                new Product
                {
                    Id = 3,
                    Title = "The Great Gatsby",
                    Description = "A story of wealth, love, and tragedy in the Jazz Age.",
                    ISBN = "978-0743273565",
                    Author = "F. Scott Fitzgerald",
                    ListPrice = 12.99m,
                    Price = 10.99m,
                    Price50 = 9.49m,
                    Price100 = 8.99m
                },
                new Product
                {
                    Id = 4,
                    Title = "1984",
                    Description = "A dystopian novel depicting a totalitarian society.",
                    ISBN = "978-0451524935",
                    Author = "George Orwell",
                    ListPrice = 11.99m,
                    Price = 9.99m,
                    Price50 = 8.49m,
                    Price100 = 7.99m
                },
                new Product
                {
                    Id = 5,
                    Title = "Pride and Prejudice",
                    Description = "A timeless story of love and class in 19th-century England.",
                    ISBN = "978-0141439518",
                    Author = "Jane Austen",
                    ListPrice = 10.99m,
                    Price = 8.99m,
                    Price50 = 7.49m,
                    Price100 = 6.99m
                }
            );
        }
    }
}

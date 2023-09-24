using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestShop.Models;

namespace TestShop.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
        //Sample
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //because of identity
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
				new Company
				{
					Id = 1,
					Name = "Sample Company",
					StreetAddress = "123 Main St",
					City = "Sample City",
					State = "Sample State",
					PostalCode = "12345",
					PhoneNumber = "555-555-5555"
				},
				new Company
				{
					Id = 2,
					Name = "ABC Corporation",
					StreetAddress = "456 Elm St",
					City = "Another City",
					State = "Another State",
					PostalCode = "54321",
					PhoneNumber = "555-123-4567"
				},
				new Company
				{
					Id = 3,
					Name = "XYZ Ltd.",
					StreetAddress = "789 Oak St",
					City = "Yet Another City",
					State = "Yet Another State",
					PostalCode = "98765",
					PhoneNumber = "555-987-6543"
				}
            );
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
                    Price100 = 10.99m,
                    CategoryId = 1,
                    ImageUrl = ""
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
                    Price100 = 9.99m,
                    CategoryId = 1,
                    ImageUrl = ""
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
                    Price100 = 8.99m,
                    CategoryId = 1,
                    ImageUrl = ""
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
                    Price100 = 7.99m,
                    CategoryId = 1,
                    ImageUrl = ""
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
                    Price100 = 6.99m,
                    CategoryId = 1,
                    ImageUrl = ""
				}
            );
        }
    }
}

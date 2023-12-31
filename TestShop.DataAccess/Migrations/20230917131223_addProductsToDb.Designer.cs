﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestShop.DataAccess.Data;

#nullable disable

namespace TestShop.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230917131223_addProductsToDb")]
    partial class addProductsToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestShop.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDateTime = new DateTime(2023, 9, 17, 18, 12, 22, 965, DateTimeKind.Local).AddTicks(2267),
                            DisplayOrder = 1,
                            Name = "Action"
                        },
                        new
                        {
                            Id = 2,
                            CreatedDateTime = new DateTime(2023, 9, 17, 18, 12, 22, 965, DateTimeKind.Local).AddTicks(2276),
                            DisplayOrder = 2,
                            Name = "SciFi"
                        },
                        new
                        {
                            Id = 3,
                            CreatedDateTime = new DateTime(2023, 9, 17, 18, 12, 22, 965, DateTimeKind.Local).AddTicks(2277),
                            DisplayOrder = 3,
                            Name = "History"
                        });
                });

            modelBuilder.Entity("TestShop.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ListPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Price100")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Price50")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "Hunter S. Thompson",
                            Description = "A wild journey to the heart of the American Dream.",
                            ISBN = "978-0679785897",
                            ListPrice = 19.99m,
                            Price = 15.99m,
                            Price100 = 10.99m,
                            Price50 = 12.99m,
                            Title = "Fear and Loathing in Las Vegas"
                        },
                        new
                        {
                            Id = 2,
                            Author = "Harper Lee",
                            Description = "A classic novel about racial injustice and moral growth.",
                            ISBN = "978-0061120084",
                            ListPrice = 14.99m,
                            Price = 12.49m,
                            Price100 = 9.99m,
                            Price50 = 10.99m,
                            Title = "To Kill a Mockingbird"
                        },
                        new
                        {
                            Id = 3,
                            Author = "F. Scott Fitzgerald",
                            Description = "A story of wealth, love, and tragedy in the Jazz Age.",
                            ISBN = "978-0743273565",
                            ListPrice = 12.99m,
                            Price = 10.99m,
                            Price100 = 8.99m,
                            Price50 = 9.49m,
                            Title = "The Great Gatsby"
                        },
                        new
                        {
                            Id = 4,
                            Author = "George Orwell",
                            Description = "A dystopian novel depicting a totalitarian society.",
                            ISBN = "978-0451524935",
                            ListPrice = 11.99m,
                            Price = 9.99m,
                            Price100 = 7.99m,
                            Price50 = 8.49m,
                            Title = "1984"
                        },
                        new
                        {
                            Id = 5,
                            Author = "Jane Austen",
                            Description = "A timeless story of love and class in 19th-century England.",
                            ISBN = "978-0141439518",
                            ListPrice = 10.99m,
                            Price = 8.99m,
                            Price100 = 6.99m,
                            Price50 = 7.49m,
                            Title = "Pride and Prejudice"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

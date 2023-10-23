﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TestShop.Models.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [ValidateNever]
        public Product Product { get; set; }
        public int ProductId { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public decimal Price { get; set; }
    }
}
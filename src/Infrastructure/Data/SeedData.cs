﻿using ApplicationCore.Models;
using IdentityServer4.EntityFramework.Options;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProductDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProductDbContext>>(),
                serviceProvider.GetRequiredService<
                    IOptions<OperationalStoreOptions>>()))
            {
                SeedDB(context);
            }
        }

        public static void SeedDB(ProductDbContext context)
        {
            // Look for any products.
            if (context.Products.Any())
            {
                return;     // DB has been seeded
            }

            context.Products.AddRange(
                new Product()
                {
                    Title = "Toy",
                    Description = "Plastic",
                    Seller = "Michael",
                    Price = 50,
                    Quantity = 2,
                    ImageUrl = "duck.png"
                },
                new Product()
                {
                    Title = "Book",
                    Description = "Hard back",
                    Seller = "Peter",
                    Price = 25,
                    Quantity = 4,
                    ImageUrl = "book.png"
                },
                new Product()
                {
                    Title = "Lamp",
                    Description = "Bright",
                    Seller = "David",
                    Price = 75,
                    Quantity = 1,
                    ImageUrl = "lamp.png"
                }
            );
            context.SaveChanges();
        }
    }
}

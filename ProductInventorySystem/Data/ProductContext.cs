﻿using Microsoft.EntityFrameworkCore;
using ProductInventorySystem.Models;

namespace ProductInventorySystem.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}

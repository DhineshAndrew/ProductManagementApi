using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Models;
using System.Collections.Generic;

namespace ProductManagementApi.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}

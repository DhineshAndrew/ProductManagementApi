using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService
{
    private readonly ProductContext _context;

    public ProductService(ProductContext context)
    {
        _context = context;
    }

    public async Task<int> AddProductAsync(Product product)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_AddProduct @p0, @p1, @p2, @p3, @p4",
            product.productName, product.description, product.price, product.category, product.ImagePath);
        return result;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.FromSqlRaw("EXEC sp_GetProducts").ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var products = await _context.Products.FromSqlRaw("EXEC sp_GetProductById @p0", id).ToListAsync();
        return products.FirstOrDefault();
    }

    public async Task<int> UpdateProductAsync(Product product)
    {
        return await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_UpdateProduct @p0, @p1, @p2, @p3, @p4, @p5",
             product.id, product.productName, product.description, product.price, product.category, product.ImagePath);
    }

    public async Task<int> DeleteProductAsync(int id)
    {
        return await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteProduct @p0", id);
    }
}

using Microsoft.EntityFrameworkCore;
using ProductInventorySystem.Data;
using ProductInventorySystem.Models;

namespace ProductInventorySystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        public ProductRepository(ProductContext context) 
        {
            _context = context;
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetAllInStock()
        {
            return await _context.Products
                .Where(p => p.Quantity > 0)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<int> UpdateAsync(Product product)
        {
            return await _context.Products
                .Where(p => p.Id == product.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Name, product.Name)
                    .SetProperty(p => p.Quantity, product.Quantity)
                    .SetProperty(p => p.Price, product.Price)
                    .SetProperty(p => p.Category, product.Category));
        }
    }
}

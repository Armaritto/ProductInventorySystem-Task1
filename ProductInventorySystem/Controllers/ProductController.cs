using Microsoft.AspNetCore.Mvc;
using ProductInventorySystem.Data;
using ProductInventorySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductInventorySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                var product = await _context.Products.FindAsync(id.Value);
                if (product == null)
                    return NotFound(new { message = "Product not found!" });

                return Ok(product);
            }

            var allProducts = await _context.Products.ToListAsync();
            return Ok(allProducts);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product created successfully!" }); ;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromQuery] int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
                return NotFound(new { message = "Product not found!" });

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Product updated successfully!" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery]  int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound(new { message = "Product not found!" });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully!" }); ;
        }

        [HttpGet("in-stock")]
        public async Task<IActionResult> GetInStockProducts()
        {
            var inStockProducts = await _context.Products.Where(p => p.Quantity > 0).ToListAsync();

            if (inStockProducts.Count == 0)
                return NotFound(new { message = "No products in stock!" });

            return Ok(inStockProducts);
        }
    }
}

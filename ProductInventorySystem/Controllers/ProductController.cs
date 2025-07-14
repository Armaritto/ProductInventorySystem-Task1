using Microsoft.AspNetCore.Mvc;
using ProductInventorySystem.Data;
using ProductInventorySystem.Models;
using Microsoft.EntityFrameworkCore;
using ProductInventorySystem.Dtos;
using AutoMapper;

namespace ProductInventorySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;

        public ProductController(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                var product = await _context.Products.FindAsync(id.Value);
                if (product == null)
                    return NotFound(new { message = "Product not found!" });
                var dto = _mapper.Map<ProductDto>(product);
                return Ok(dto);
            }

            var allProducts = await _context.Products.ToListAsync();
            var dtoList = _mapper.Map<List<ProductDto>>(allProducts);
            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
                return BadRequest(new { message = "ProductDto is required." });

            Product product = _mapper.Map<Product>(productDto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product created successfully!" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromBody] ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            if (product == null)
                return BadRequest(new { message = "ProductDto is required." });

            var rowsAffected = await _context.Products
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Name, product.Name)
                    .SetProperty(p => p.Price, product.Price)
                    .SetProperty(p => p.Quantity, product.Quantity));

            if (rowsAffected == 0)
                return NotFound(new { message = "Product not found!" });

            return Ok(new { message = "Product updated successfully!" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id)
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

            var inStockProductDtos = _mapper.Map<List<ProductDto>>(inStockProducts);

            return Ok(inStockProductDtos);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProductInventorySystem.Repositories;
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
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                var product = await _repository.GetByIdAsync(id.Value);
                if (product == null)
                    return NotFound(new { message = "Product not found!" });
                var dto = _mapper.Map<ProductDto>(product);
                return Ok(dto);
            }

            var allProducts = await _repository.GetAllAsync();
            var dtoList = _mapper.Map<List<ProductDto>>(allProducts);
            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
                return BadRequest(new { message = "ProductDto is required." });

            Product product = _mapper.Map<Product>(productDto);

            await _repository.AddAsync(product);

            return Ok(new { message = "Product created successfully!" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromBody] ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            if (product == null)
                return BadRequest(new { message = "ProductDto is required." });

            var rowsAffected = await _repository.UpdateAsync(product);

            if (rowsAffected == 0)
                return NotFound(new { message = "Product not found!" });

            return Ok(new { message = "Product updated successfully!" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id)
        {
           var rowsAffected = await _repository.DeleteAsync(id);

            if (rowsAffected == 0)
                return NotFound(new { message = "Product not found!" });

            return Ok(new { message = "Product deleted successfully!" }); ;
        }

        [HttpGet("in-stock")]
        public async Task<IActionResult> GetInStockProducts()
        {
            var inStockProducts = await _repository.GetAllInStock();

            if (inStockProducts.Count == 0)
                return NotFound(new { message = "No products in stock!" });

            var inStockProductDtos = _mapper.Map<List<ProductDto>>(inStockProducts);

            return Ok(inStockProductDtos);
        }
    }
}

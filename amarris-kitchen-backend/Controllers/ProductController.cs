
using amarris_kitchen_backend.Data;
using amarris_kitchen_backend.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amarris_kitchen_backend.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AmarrisKitchenContext _context;
        public ProductController(AmarrisKitchenContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
        [HttpGet("{category}")]
        public async Task<ActionResult> GetProductsPerCategory(string category)
        {
            var filteredProducts = await _context.Products
                .Where(p => p.Category.CategoryName == category)
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    CategoryName = p.Category.CategoryName,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
            return Ok(filteredProducts);
        }
    }
}

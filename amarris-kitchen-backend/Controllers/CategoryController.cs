using amarris_kitchen_backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amarris_kitchen_backend.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AmarrisKitchenContext _context;
        public CategoryController(AmarrisKitchenContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
    }
}

using amarris_kitchen_backend.Data;
using amarris_kitchen_backend.DTOs;
using amarris_kitchen_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amarris_kitchen_backend.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AdminController: ControllerBase
    {
        private readonly AmarrisKitchenContext _context;
        public AdminController(AmarrisKitchenContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminName.ToLower() == loginDto.AdminName.ToLower());
            var password = await _context.Admins.Where(p => p.Password == loginDto.Password).Select(p => p.Password).FirstOrDefaultAsync();
            if(admin == null)
            {
                return Unauthorized("Admin Doesnt Exist");
            }
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.Password);
            if(!isPasswordCorrect)
            {
                return Unauthorized("Invalid Password");
            }

            return Ok(new { message = "Success", admin = admin.AdminName });
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDTO createAdminDto)
        {
            string secureHash = BCrypt.Net.BCrypt.HashPassword(createAdminDto.Password);

            var newAdmin = new Admin
            {
                AdminName = createAdminDto.AdminName,
                Password = secureHash,
                Email = createAdminDto.Email,
                Role = createAdminDto.Role
            };
            _context.Admins.Add(newAdmin);
            await _context.SaveChangesAsync();

            return Ok(createAdminDto.AdminName);
        }
    }
}

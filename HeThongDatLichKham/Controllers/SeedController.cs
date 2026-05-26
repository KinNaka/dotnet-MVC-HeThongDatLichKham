using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HeThongDatLichKham.Models;

namespace HeThongDatLichKham.Controllers
{
    public class SeedController : Controller
    {
        private readonly UserManager<UserClass> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedController(UserManager<UserClass> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {

            string adminEmail = "admin@bv.com";
            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin != null)
                return Content("Admin already exists");

            // Tạo admin mới
            var adminUser = new UserClass
            {
                Fullname = "Quản Trị Viên",
                UserName ="admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            string password = "Admin@123";
            var result = await _userManager.CreateAsync(adminUser, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                return Content("Admin created successfully");
            }

            return Content("Failed to create admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

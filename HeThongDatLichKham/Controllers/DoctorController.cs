using HeThongDatLichKham.Data;
using HeThongDatLichKham.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HeThongDatLichKham.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserClass> _userManager;

        public DoctorController(ApplicationDbContext context, UserManager<UserClass> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MainDR()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.MaBacSi == null)
            {
                return Unauthorized(); 
            }

            var danhSach = await _context.DatLiches
                .Where(d => d.MaBacSi == user.MaBacSi)
                .OrderByDescending(d => d.NgayKham)
                .ToListAsync();

            return View(danhSach);
        }



        [HttpPost]
        public async Task<IActionResult> DaKham(int id)
        {
            var lich = await _context.DatLiches.FindAsync(id);
            if (lich != null)
            {
                _context.DatLiches.Remove(lich);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MainDR");
        }
    }
}

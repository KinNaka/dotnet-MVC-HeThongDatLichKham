using HeThongDatLichKham.Data;
using HeThongDatLichKham.Models;
using HeThongDatLichKham.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace HeThongDatLichKham.Controllers
{
    [Authorize(Roles = "Admin")]


    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<UserClass> _userManager;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger, UserManager<UserClass> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        // Tạo bác sĩ 
        [HttpGet]
        public IActionResult CreateDoctor()
        {
            var model = new BacSiViewModel
            {
                ChuyenKhoaList = _context.ChuyenKhoas
                    .Select(c => new SelectListItem
                    {
                        Value = c.IDChuyenKhoa,
                        Text = c.TenChuyenKhoa
                    }).ToList(),

                BenhVienList = _context.BenhViens
                    .Select(b => new SelectListItem
                    {
                        Value = b.MaBenhVien,
                        Text = b.TenBenhVien
                    }).ToList()
            };

            return View(model);
        }
        // POST Tạo bác sĩ
        [HttpPost]
        public IActionResult CreateDoctor(BacSiViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var doctor = new BacSi
                {
                    MaBacSi = vm.MaBacSi,
                    HoTen = vm.HoTen,
                    SoDienThoai = vm.SoDienThoai,
                    Email = vm.Email,
                    ThongTin = vm.ThongTin,
                    IDChuyenKhoa = vm.IDChuyenKhoa,
                    MaBenhVien = vm.MaBenhVien
                };

                _context.BacSis.Add(doctor);
                _context.SaveChanges();
                return RedirectToAction("ManageDoctors");
            }
            vm.ChuyenKhoaList = _context.ChuyenKhoas
                .Select(c => new SelectListItem
                {
                    Value = c.IDChuyenKhoa,
                    Text = c.TenChuyenKhoa
                }).ToList();

            vm.BenhVienList = _context.BenhViens
                .Select(bv => new SelectListItem
                {
                    Value = bv.MaBenhVien,
                    Text = bv.TenBenhVien
                }).ToList();

            return View(vm);
        }
        //Tạo tài khoản cho bác sĩ
        [HttpGet]
        public IActionResult CreateDoctorAccount()
        {
            var model = new CreateDoctorAccountViewModel
            {
                BacSiList = _context.BacSis
                    .Select(b => new SelectListItem
                    {
                        Value = b.MaBacSi,
                        Text = b.HoTen
                    }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateDoctorAccount(CreateDoctorAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.BacSiList = _context.BacSis.Select(b => new SelectListItem
                {
                    Value = b.MaBacSi,
                    Text = b.HoTen
                }).ToList();
                return View(model);
            }
            var bacSi = _context.BacSis.FirstOrDefault(b => b.MaBacSi == model.SelectedMaBacSi);
            var user = new UserClass
            {
                UserName = model.Email,
                Email = model.Email,
                MaBacSi = model.SelectedMaBacSi,
                Fullname = bacSi.HoTen
              
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Doctor");
                return RedirectToAction("Index", "Admin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            model.BacSiList = _context.BacSis.Select(b => new SelectListItem
            {
                Value = b.MaBacSi,
                Text = b.HoTen
            }).ToList();

            return View(model);
        }



        public IActionResult Index()
        {
            return View();
        }

        // Quản lý bác sĩ
        public IActionResult ManageDoctors()
        {
            var doctors = _context.BacSis
                .Include(d => d.ChuyenKhoa)
                .ToList();
            return View(doctors);
        }

        // Quản lý chuyên khoa
        public async Task<IActionResult> ManageSpecialties()
        {
            var chuyenKhoaList = await _context.ChuyenKhoas.ToListAsync();
            return View(chuyenKhoaList);
        }
        [HttpGet]
        public IActionResult AddSpecialty()
        {
            return View(new ChuyenKhoaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialty(ChuyenKhoaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var chuyenKhoa = new ChuyenKhoa
            {
                IDChuyenKhoa = model.IDChuyenKhoa,
                TenChuyenKhoa = model.TenChuyenKhoa,
                MoTa = model.MoTa
            };

            _context.ChuyenKhoas.Add(chuyenKhoa);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageSpecialties");
        }
        // Chỉnh sửa chuyên khoa
        [HttpGet]
        public async Task<IActionResult> EditSpecialty(string id)
        {
            var ck = await _context.ChuyenKhoas.FindAsync(id);
            if (ck == null) return NotFound();

            var vm = new ChuyenKhoaViewModel
            {
                IDChuyenKhoa = ck.IDChuyenKhoa,
                TenChuyenKhoa = ck.TenChuyenKhoa,
                MoTa = ck.MoTa
            };

            return View(vm);
        }
        // POST Chỉnh sửa chuyên khoa
        [HttpPost]
        public async Task<IActionResult> EditSpecialty(ChuyenKhoaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ck = await _context.ChuyenKhoas.FindAsync(model.IDChuyenKhoa);
            if (ck == null) return NotFound();

            ck.TenChuyenKhoa = model.TenChuyenKhoa;
            ck.MoTa = model.MoTa;

            _context.ChuyenKhoas.Update(ck);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageSpecialties");
        }

        // Xóa chuyên khoa
        [HttpPost]
        public async Task<IActionResult> DeleteSpecialty(string id)
        {
            var ck = await _context.ChuyenKhoas.FindAsync(id);
            if (ck == null) return NotFound();

            _context.ChuyenKhoas.Remove(ck);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageSpecialties");
        }

       

        // Xóa bác sĩ
        public IActionResult DeleteDoctor(string id)
        {
            var doctor = _context.BacSis.Find(id);
            if (doctor == null) return NotFound();

            _context.BacSis.Remove(doctor);
            _context.SaveChanges();
            return RedirectToAction("ManageDoctors");
        }
        // Chỉnh sửa bác sĩ
        [HttpGet]
        public IActionResult EditDoctor(string id)
        {
            var doctor = _context.BacSis.Find(id);
            if (doctor == null) return NotFound();

            var vm = new BacSiViewModel
            {
                MaBacSi = doctor.MaBacSi,
                HoTen = doctor.HoTen,
                SoDienThoai = doctor.SoDienThoai,
                Email = doctor.Email,
                ThongTin = doctor.ThongTin,
                IDChuyenKhoa = doctor.IDChuyenKhoa,
                MaBenhVien = doctor.MaBenhVien,
                BenhVienList = _context.BenhViens
                    .Select(bv => new SelectListItem
                    {
                        Value = bv.MaBenhVien,
                        Text = bv.TenBenhVien
                    }).ToList()
            };

            ViewBag.ChuyenKhoaList = new SelectList(_context.ChuyenKhoas, "IDChuyenKhoa", "TenChuyenKhoa", vm.IDChuyenKhoa);
            return View(vm);
        }

        //POST Chỉnh sửa bác sĩ
        [HttpPost]
        public IActionResult EditDoctor(BacSiViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var doctor = _context.BacSis.Find(vm.MaBacSi);
                if (doctor == null) return NotFound();

                doctor.HoTen = vm.HoTen;
                doctor.SoDienThoai = vm.SoDienThoai;
                doctor.Email = vm.Email;
                doctor.ThongTin = vm.ThongTin;
                doctor.IDChuyenKhoa = vm.IDChuyenKhoa;
                doctor.MaBenhVien = vm.MaBenhVien;

                _context.SaveChanges();
                return RedirectToAction("ManageDoctors");
            }

            ViewBag.ChuyenKhoaList = new SelectList(_context.ChuyenKhoas, "IDChuyenKhoa", "TenChuyenKhoa", vm.IDChuyenKhoa);
            vm.BenhVienList = _context.BenhViens
                .Select(bv => new SelectListItem { Value = bv.MaBenhVien, Text = bv.TenBenhVien }).ToList();

            return View(vm);
        }


        // Quản lý lịch làm việc
        public IActionResult ManageSchedules()
        {
            var schedules = _context.LichLamViecs
                .Include(l => l.BacSi)
                .ThenInclude(b => b.ChuyenKhoa)
                .OrderBy(l => l.Ngay)
                .ToList();
            return View(schedules);
        }
        // Tạo lịch làm việc
        public IActionResult CreateSchedule()
        {
            var model = new CreateScheduleViewModel
            {
                Ngay = DateTime.Today,
                BacSiList = _context.BacSis
                    .Select(b => new SelectListItem { Value = b.MaBacSi, Text = b.HoTen })
                    .ToList()
            };
            return View(model);
        }
        // Post tạo lịch làm việc
        [HttpPost]
        public IActionResult CreateSchedule(CreateScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState không hợp lệ.");
                model.BacSiList = _context.BacSis.Select(d => new SelectListItem
                {
                    Value = d.MaBacSi,
                    Text = d.HoTen
                }).ToList();
                return View(model);
            }

            // Check trùng lịch
            var isExists = _context.LichLamViecs.Any(l =>
                l.MaBacSi == model.MaBacSi &&
                l.Ngay.Date == model.Ngay.Date &&
                l.Ca == model.Ca);

            if (isExists)
            {
                ModelState.AddModelError("", "Bác sĩ đã có lịch vào ngày và ca này.");
                model.BacSiList = _context.BacSis.Select(d => new SelectListItem
                {
                    Value = d.MaBacSi,
                    Text = d.HoTen
                }).ToList();
                return View(model);
            }

            var schedule = new LichLamViec
            {
                MaBacSi = model.MaBacSi,
                Ngay = model.Ngay,
                Ca = model.Ca
            };

            _context.LichLamViecs.Add(schedule);
            _context.SaveChanges();

            return RedirectToAction("ManageSchedules");
        }
        // Chnhr sửa lịch làm việc
        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            var schedule = _context.LichLamViecs.FirstOrDefault(x => x.MaLich == id);
            if (schedule == null) return NotFound();

            var model = new CreateScheduleViewModel
            {
                MaBacSi = schedule.MaBacSi,
                Ngay = schedule.Ngay,
                Ca = schedule.Ca,
                BacSiList = _context.BacSis.Select(b => new SelectListItem
                {
                    Value = b.MaBacSi,
                    Text = b.HoTen
                }).ToList()
            };

            ViewBag.MaLich = id;
            return View("EditSchedule", model);
        }
        // POST chỉnh sủa lịch làm việc
        [HttpPost]
        public IActionResult EditSchedule(int id, CreateScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.BacSiList = _context.BacSis.Select(b => new SelectListItem
                {
                    Value = b.MaBacSi,
                    Text = b.HoTen
                }).ToList();
                ViewBag.MaLich = id;
                return View(model);
            }

            var schedule = _context.LichLamViecs.FirstOrDefault(x => x.MaLich == id);
            if (schedule == null) return NotFound();

            schedule.MaBacSi = model.MaBacSi;
            schedule.Ngay = model.Ngay;
            schedule.Ca = model.Ca;

            _context.SaveChanges();
            return RedirectToAction("ManageSchedules");
        }

        // Xóa lịch làm việc 
        [HttpPost]
        public IActionResult DeleteSchedule(int id)
        {
            var schedule = _context.LichLamViecs.FirstOrDefault(x => x.MaLich == id);
            if (schedule == null) return NotFound();

            _context.LichLamViecs.Remove(schedule);
            _context.SaveChanges();

            return RedirectToAction("ManageSchedules");
        }

    }
}

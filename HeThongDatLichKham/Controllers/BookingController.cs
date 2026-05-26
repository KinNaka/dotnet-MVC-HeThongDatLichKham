using HeThongDatLichKham.Data;
using HeThongDatLichKham.Models;
using HeThongDatLichKham.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace HeThongDatLichKham.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotiHub> _hub;

        public BookingController(ApplicationDbContext context, IHubContext<NotiHub> hub)
        {
            _context = context;
            _hub = hub;
        }
        // Check cccd để ajax dựa vào tái khám hoặc lần đầu khám
        [HttpGet]
        public async Task<IActionResult> CheckCCCD(string cccd, string chuyenKhoaId)
        {
            var hoSo = await _context.HoSoBenhAns
                .Include(h => h.BacSi)
                .Where(h => h.CCCD == cccd && h.BacSi.IDChuyenKhoa == chuyenKhoaId)
                .OrderByDescending(h => h.NgayTao)
                .FirstOrDefaultAsync();

            if (hoSo != null)
            {
                return Json(new
                {
                    isFollowUp = true,
                    bacSi = new
                    {
                        maBacSi = hoSo.MaBacSi,
                        hoTen = hoSo.BacSi.HoTen,
                        maBenhVien = hoSo.BacSi.MaBenhVien

                    }
                });
            }

            return Json(new { isFollowUp = false });
        }
        // Lấy bác sĩ dựa vào chuyên khoa đã chọn
        [HttpGet]
        public async Task<IActionResult> GetBacSiByChuyenKhoa(string chuyenKhoaId)
        {
            var bacSiList = await _context.BacSis
                .Where(b => b.IDChuyenKhoa == chuyenKhoaId)
                .Select(b => new
                {
                    maBacSi = b.MaBacSi,
                    hoTen = b.HoTen
                })
                .ToListAsync();

            return Json(bacSiList);
        }
        // Lấy lịch làm việc dựa vào chuyên khoa và bác sĩ
        [HttpGet]
        public async Task<IActionResult> GetLichLamViec(string maBacSi)
        {
            var lichLamViec = await _context.LichLamViecs
                .Where(l => l.MaBacSi == maBacSi && l.Ngay >= DateTime.Today )
                .Select(l => new
                {
                    ngay = l.Ngay.ToString("yyyy-MM-dd"),
                    ca = l.Ca
                })
                .ToListAsync();

            return Json(lichLamViec);
        }
        // Lấy ngày và ca khám theo trường hợp tái khám
        [HttpGet]
        public async Task<IActionResult> GetNgayVaCa_TaiKham(string cccd, string chuyenKhoaId)
        {
            var hoSo = await _context.HoSoBenhAns
                .Include(h => h.BacSi)
                .Where(h => h.CCCD == cccd && h.BacSi.IDChuyenKhoa == chuyenKhoaId)
                .OrderByDescending(h => h.NgayTao)
                .FirstOrDefaultAsync();

            if (hoSo == null)
                return Json(new { isFollowUp = false });

            var lichLamViec = await _context.LichLamViecs
                .Where(l => l.MaBacSi == hoSo.MaBacSi && l.Ngay >= DateTime.Today)
                .Select(l => new
                {
                    ngay = l.Ngay.ToString("yyyy-MM-dd"),
                    ca = l.Ca
                })
                .ToListAsync();

            return Json(new
            {
                isFollowUp = true,
                maBacSi = hoSo.MaBacSi,
                hoTen = hoSo.BacSi.HoTen,
                lich = lichLamViec
            });
        }

        // Lấy ngày và ca khám theo trường hợp khám lần đầu
        [HttpGet]
        public async Task<IActionResult> GetNgayVaCa_KhamLanDau(string chuyenKhoaId)
        {
            var bacSiList = await _context.BacSis
                .Where(b => b.IDChuyenKhoa == chuyenKhoaId)
                .Select(b => b.MaBacSi)
                .ToListAsync();

            var lichLamViec = await _context.LichLamViecs
                .Where(l => bacSiList.Contains(l.MaBacSi) && l.Ngay >= DateTime.Today)
                .Select(l => new
                {
                    ngay = l.Ngay.ToString("yyyy-MM-dd"),
                    ca = l.Ca
                })
                .Distinct()
                .ToListAsync();

            return Json(new
            {
                isFollowUp = false,
                lich = lichLamViec
            });
        }
        // Render form đặt lịch
        [HttpGet]
        public async Task<IActionResult> Booking()
        {
            var chuyenKhoas = await _context.ChuyenKhoas.ToListAsync();

            var viewModel = new BookingViewModel
            {
                ChuyenKhoas = chuyenKhoas
            };

            return View(viewModel);
        }
        // Luồng chính dùng các action 
        [HttpPost]
        public async Task<IActionResult> BookApo(
            string CCCD,
            string HoTen,
            DateTime NgaySinh,
            bool GioiTinh,
            string DiaChi,
            string SoDienThoai,
            DateTime NgayKham,
            string SelectedChuyenKhoaId,
            string CaKham)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            // Lấy uid
            string maBacSi;
            string maBenhVien;
            string loaiKham;

            var hoSoList = await _context.HoSoBenhAns
                .Include(h => h.BacSi)
                .Where(h => h.CCCD == CCCD)
                .ToListAsync();

            var daTungKhamChuyenKhoa = hoSoList
                .Any(h => h.BacSi.IDChuyenKhoa == SelectedChuyenKhoaId);
            // check hồ sơ bệnh án
            if (daTungKhamChuyenKhoa)
            {
              
                var hoSoCu = hoSoList.First(h => h.BacSi.IDChuyenKhoa == SelectedChuyenKhoaId);
                maBacSi = hoSoCu.MaBacSi;
                loaiKham = "Tái khám";

                maBenhVien = await _context.DatLiches
                    .Where(d => d.MaBacSi == maBacSi)
                    .Select(d => d.MaBenhVien)
                    .FirstOrDefaultAsync();
            }
            else
            {
                var bacSiList = await _context.BacSis
                    .Where(b => b.IDChuyenKhoa == SelectedChuyenKhoaId)
                    .ToListAsync();

                if (bacSiList.Count == 0)
                    return BadRequest("Không có bác sĩ trong chuyên khoa này.");
                var random = new Random();
                var bacSi = bacSiList[random.Next(bacSiList.Count)];
                maBacSi = bacSi.MaBacSi;
                loaiKham = "Khám lần đầu";
                maBenhVien = bacSi.MaBenhVien;
                _context.HoSoBenhAns.Add(new HoSoBenhAn
                {
                    CCCD = CCCD,
                    MaBacSi = maBacSi,
                    NgayTao = DateTime.Now
                });// thêm vào ho so bệnh án

                var daTonTai = await _context.BenhNhans.AnyAsync(b => b.CCCD == CCCD);
                if (!daTonTai)
                {
                    _context.BenhNhans.Add(new BenhNhan
                    {
                        CCCD = CCCD,
                        HoTen = HoTen,
                        NgaySinh = NgaySinh,
                        GioiTinh = GioiTinh,
                        DiaChi = DiaChi,
                        SoDienThoai = SoDienThoai,
                        IDNguoiDung = userId
                    });
                }
                await _context.SaveChangesAsync();
                maBenhVien = await _context.DatLiches
                    .Where(d => d.MaBacSi == maBacSi)
                    .Select(d => d.MaBenhVien)
                    .FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(maBenhVien))
                {
                    maBenhVien = await _context.BenhViens
                        .Select(b => b.MaBenhVien)
                        .FirstOrDefaultAsync();
                }
            }
            var ca = CaKham;
            var soDaDat = await _context.DatLiches
                .Where(d => d.MaBacSi == maBacSi && d.NgayKham.Date == NgayKham.Date && d.Ca == ca)
                .CountAsync();
            //check số lượng khám ca đã đày chưa
            if (soDaDat >= 20)
            {
                ModelState.AddModelError("", "Ca khám đã đầy. Vui lòng chọn thời gian khác.");
                return RedirectToAction("Booking");
            }
            var coLichLamViec = await _context.LichLamViecs.AnyAsync(lv =>
    lv.MaBacSi == maBacSi &&
    lv.Ngay.Date == NgayKham.Date &&
    lv.Ca == ca
);// Check lịch làm việc theo bác sĩ

            if (!coLichLamViec)
            {
                ModelState.AddModelError("", "Bác sĩ không làm việc vào thời gian đã chọn.");
                return RedirectToAction("Booking");
            }
            var newDatLich = new DatLich
            {
                CCCD = CCCD,
                MaBacSi = maBacSi,
                NgayKham = NgayKham,
                LoaiKham = loaiKham,
                TrangThai = false,
                MaBenhVien = maBenhVien,
                Ca = ca,
                STT = soDaDat + 1
            };
            _context.DatLiches.Add(newDatLich);
            await _context.SaveChangesAsync();
            var benhNhan = await _context.BenhNhans.FirstOrDefaultAsync(b => b.CCCD == CCCD);
            var bacSiInfo = await _context.BacSis.FirstOrDefaultAsync(b => b.MaBacSi == maBacSi);
            var benhVienInfo = await _context.BenhViens.FirstOrDefaultAsync(b => b.MaBenhVien == maBenhVien);
            var doctorUserId = await _context.Users
                .Where(u => u.MaBacSi == maBacSi)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(doctorUserId))
            {
                try
                {
                    await _hub.Clients
    .Group($"user-{doctorUserId}")
    .SendAsync("NewAppointment", "Bạn có lịch hẹn mới.", NgayKham.ToString("dd/MM/yyyy"));

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi gửi notification: {ex.Message}");
                }
            }
            var viewModel = new BookingViewModel
            {
                CCCD = CCCD,
                HoTenBenhNhan = benhNhan?.HoTen ?? "Không rõ",
                TenBacSi = bacSiInfo?.HoTen ?? "Không rõ",
                TenBenhVien = benhVienInfo?.TenBenhVien ?? "Không rõ",
                NgayKham = NgayKham,
                LoaiKham = loaiKham,
                STT = newDatLich.STT
            };
            return View("Result", viewModel);
        }
    }
}

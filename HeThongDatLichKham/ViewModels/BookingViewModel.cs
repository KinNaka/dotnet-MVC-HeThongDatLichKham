using HeThongDatLichKham.Models;

namespace HeThongDatLichKham.ViewModels
{
    public class BookingViewModel
    {
        public string CCCD { get; set; }
        public string HoTenBenhNhan { get; set; }
        public string TenBacSi { get; set; }

        public string? TenBenhVien { get; set; }
        public DateTime NgayKham { get; set; }
        public string LoaiKham { get; set; }
        public string? MaBenhVien { get; set; }

        public List<ChuyenKhoa> ChuyenKhoas { get; set; }
        public string SelectedChuyenKhoaId { get; set; }

        public int STT { get; set; }

    }
}

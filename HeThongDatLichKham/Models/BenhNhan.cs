using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.Models
{
    public class BenhNhan
    {
        [Key]
        public string CCCD { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }

        public string IDNguoiDung { get; set; }
        public UserClass TaiKhoanNguoiDung { get; set; }

        public ICollection<HoSoBenhAn> HoSos { get; set; }
    }

}

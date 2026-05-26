using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongDatLichKham.Models
{
    public class BacSi
    {
        [Key]
        public string MaBacSi { get; set; }

        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string ThongTin { get; set; }


        [ForeignKey("ChuyenKhoa")]
        public string IDChuyenKhoa { get; set; }
        public ChuyenKhoa ChuyenKhoa { get; set; }

        [ForeignKey("BenhVien")]
        public string MaBenhVien { get; set; }
        public BenhVien BenhVien { get; set; }
    }


}

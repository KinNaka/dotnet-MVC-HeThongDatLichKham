using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.Models
{
    public class DatLich
    {
        [Key]
        public int IDDatLich { get; set; }
        public DateTime NgayKham { get; set; }
        public string LoaiKham { get; set; }
        public bool TrangThai { get; set; }

        public string CCCD { get; set; }
        public BenhNhan BenhNhan { get; set; }

        public string MaBacSi { get; set; }
        public BacSi BacSi { get; set; }

        public string MaBenhVien { get; set; }
        public BenhVien BenhVien { get; set; }
        public int STT { get; set; }
        public string Ca { get; set; }
    }

}

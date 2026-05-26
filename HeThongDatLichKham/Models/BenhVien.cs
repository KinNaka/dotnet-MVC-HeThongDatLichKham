using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.Models
{
    public class BenhVien
    {
        [Key]
        public string MaBenhVien { get; set; }
        public string TenBenhVien { get; set; }
        public string DiaChi { get; set; }
    }

}

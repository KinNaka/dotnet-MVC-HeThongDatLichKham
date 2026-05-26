using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.Models
{
    public class ChuyenKhoa
    {
        [Key]
        public string IDChuyenKhoa { get; set; }
        public string TenChuyenKhoa { get; set; }
        public string MoTa { get; set; }

        public ICollection<BacSi> BacSis { get; set; }
    }

}

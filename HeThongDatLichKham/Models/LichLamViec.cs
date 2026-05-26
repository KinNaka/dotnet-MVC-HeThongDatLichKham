using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.Models
{
    public class LichLamViec
    {
        [Key]
        public int MaLich { get; set; }

        [Required]
        public string MaBacSi { get; set; }

        [Required]
        public DateTime Ngay { get; set; }

        [Required]
        public string Ca { get; set; }

        public int SoLuongToiDa { get; set; } = 20;

        [ForeignKey("MaBacSi")]
        public BacSi BacSi { get; set; }

        public ICollection<LichHen> LichHens { get; set; }
    }
}

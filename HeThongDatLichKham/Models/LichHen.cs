using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeThongDatLichKham.Models
{
    public class LichHen
    {
        [Key]
        public int MaLichHen { get; set; }

        public int MaLich { get; set; }

        public string CCCD { get; set; } // thông tin người bệnh

        public int STT { get; set; }

        public DateTime ThoiGianDat { get; set; } = DateTime.Now;

        [ForeignKey(nameof(MaLich))]
        public LichLamViec LichLamViec { get; set; }
    }


}

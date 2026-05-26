using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.Models
{
    public class HoSoBenhAn
    {
        [Key]
        public int IDHoSo { get; set; }
        public DateTime NgayTao { get; set; }

        public string MaBacSi { get; set; }
        public BacSi BacSi { get; set; }

        public string CCCD { get; set; }
        public BenhNhan BenhNhan { get; set; }
    }

}

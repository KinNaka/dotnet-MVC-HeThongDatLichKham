namespace HeThongDatLichKham.Models
{
    public class BenhVienDatLich
    {
        public string MaBacSi { get; set; }
        public BacSi BacSi { get; set; }

        public int IDDatLich { get; set; }
        public DatLich DatLich { get; set; }
    }

}

using Microsoft.AspNetCore.Identity;

namespace HeThongDatLichKham.Models
{
    public class UserClass : IdentityUser
    {
        public string Fullname { get; set; }
        public string? MaBacSi { get; set; }

        public ICollection<BenhNhan> BenhNhans { get; set; }
    }
}

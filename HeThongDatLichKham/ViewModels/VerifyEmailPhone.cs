using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.ViewModels
{
    public class VerifyEmailPhone
    {
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}

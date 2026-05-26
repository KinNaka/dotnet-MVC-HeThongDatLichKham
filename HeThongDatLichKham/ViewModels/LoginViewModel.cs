using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Bạn phải nhập email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Bạn phải nhập Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [Display(Name ="Ghi nhớ đăng nhập?")]
        public bool Rememberme { get; set; }
    }
}

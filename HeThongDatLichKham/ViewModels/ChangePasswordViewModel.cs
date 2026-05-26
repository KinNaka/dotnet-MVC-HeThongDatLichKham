using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone]
        public string phonenumber { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("confirmnewpassword", ErrorMessage = "Mật khẩu không khớp")]
        [Display(Name = "Mật khẩu mới")]
        public string newpassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string confirmnewpassword { get; set; }
    }
}

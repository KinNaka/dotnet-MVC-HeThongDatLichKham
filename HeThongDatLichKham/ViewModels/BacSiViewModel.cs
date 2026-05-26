using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.ViewModels
{
    public class BacSiViewModel
    {
        [Required]
        [Display(Name = "Mã bác sĩ")]
        public string MaBacSi { get; set; }

        [Required]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Display(Name = "Thông tin")]
        public string ThongTin { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chuyên khoa")]
        [Display(Name = "Chuyên khoa")]
        public string IDChuyenKhoa { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bệnh viện")]
        [Display(Name = "Bệnh viện")]
        public string MaBenhVien { get; set; }

        public List<SelectListItem>? ChuyenKhoaList { get; set; }
        public List<SelectListItem>? BenhVienList { get; set; }
    }
}

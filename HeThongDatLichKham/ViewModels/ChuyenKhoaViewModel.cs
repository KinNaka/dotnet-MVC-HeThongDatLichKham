using System.ComponentModel.DataAnnotations;

namespace HeThongDatLichKham.ViewModels
{
    public class ChuyenKhoaViewModel
    {
        [Required(ErrorMessage = "Mã chuyên khoa là bắt buộc")]
        [Display(Name = "Mã chuyên khoa")]
        public string IDChuyenKhoa { get; set; }

        [Required(ErrorMessage = "Tên chuyên khoa là bắt buộc")]
        [Display(Name = "Tên chuyên khoa")]
        public string TenChuyenKhoa { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
    }
}

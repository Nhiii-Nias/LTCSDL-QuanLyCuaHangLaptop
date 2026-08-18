using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class DangKyViewModel
    {
        // ── Thông tin khách hàng ──────────────────────────────────────────
        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [StringLength(50, ErrorMessage = "Họ và tên không quá 50 ký tự.")]
        [Display(Name = "Họ và tên")]
        public string TenKH { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm đúng 10 chữ số.")]
        [Display(Name = "Số điện thoại")]
        public string? SDT { get; set; }

        [StringLength(200, ErrorMessage = "Địa chỉ không quá 200 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        // ── Thông tin HSSV ────────────────────────────────────────────────
        [Display(Name = "Là học sinh / sinh viên")]
        public bool LaHSSV { get; set; } = false;

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? SinhNhat { get; set; }

        // ── Thông tin tài khoản ───────────────────────────────────────────
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Tên đăng nhập từ 4–50 ký tự.")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string XacNhanMatKhau { get; set; } = string.Empty;
    }
}

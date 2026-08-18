using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class DatHangViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        [Display(Name = "Phương thức thanh toán")]
        public string PhuongThucThanhToan { get; set; } = "Tiền Mặt";

        [StringLength(200, ErrorMessage = "Địa chỉ không quá 200 ký tự.")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string? DiaChiGiaoHang { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú không quá 500 ký tự.")]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        // Tóm tắt giỏ hàng (đọc từ Session — không phải input)
        public List<GioHangItem> DanhSachSanPham { get; set; } = new();
        public decimal TongTien => DanhSachSanPham.Sum(x => x.ThanhTien);

        [Display(Name = "Mã khuyến mãi")]
        public string? MaKM { get; set; }
    }
}

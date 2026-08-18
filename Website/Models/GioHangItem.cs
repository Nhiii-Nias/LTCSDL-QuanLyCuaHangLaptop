namespace Website.Models
{
    /// <summary>
    /// Đại diện 1 dòng sản phẩm trong giỏ hàng (Session["GioHang"]).
    /// Giỏ hàng lưu theo LoaiSanPham — serial được chọn khi xác nhận đặt hàng.
    /// </summary>
    public class GioHangItem
    {
        public string MaLoaiSP { get; set; } = string.Empty;
        public string TenLoai  { get; set; } = string.Empty;
        public string DanhMuc  { get; set; } = string.Empty;
        public string MaHang   { get; set; } = string.Empty;
        public string TenHang  { get; set; } = string.Empty;
        public decimal GiaBan  { get; set; }
        public int SoLuong     { get; set; } = 1;

        public decimal ThanhTien => GiaBan * SoLuong;
    }
}

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: KhachHangSi — Bảng con kế thừa từ KhachHang, đại diện khách hàng sỉ (doanh nghiệp).
    /// Khóa chính MaKHSi đồng thời là FK → KhachHang(MaKH).
    /// Bảng này chỉ có 1 cột duy nhất (theo SQL), không có thêm thuộc tính riêng.

    public class DTO_KhachHangSi
    {
        /// MaKHSi — CHAR(10), PK + FK → KhachHang(MaKH), NOT NULL
        public string MaKHSi { get; set; }
    }
}

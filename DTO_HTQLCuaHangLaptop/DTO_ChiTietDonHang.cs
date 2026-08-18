namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: ChiTietDonHang — Chi tiết từng sản phẩm (serial) trong một đơn hàng.
    /// Khóa chính kép: (MaDH, MaSerialSP). MaSerialSP còn có ràng buộc UNIQUE riêng
    /// để đảm bảo mỗi serial chỉ được bán đúng 1 lần. Không có audit columns (theo SQL).
    
    public class DTO_ChiTietDonHang
    {
        /// MaDH — CHAR(10), PK (kép), FK → DonHang(MaDH), NOT NULL
        public string MaDH { get; set; }

        /// MaSerialSP — VARCHAR(50), PK (kép), FK → SanPham(MaSerialSP), NOT NULL, UNIQUE
        public string MaSerialSP { get; set; }

        /// GiaBan — DECIMAL(15,2), NOT NULL, >= 0 (giá tại thời điểm mua)
        public decimal GiaBan { get; set; }

        /// PhanTramGiam — DECIMAL(5,2), NULL, 0–100 (NULL nếu không giảm giá sản phẩm)
        public decimal? PhanTramGiam { get; set; }
    }
}

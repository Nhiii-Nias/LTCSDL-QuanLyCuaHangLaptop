namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: ChiTietPhieuNhap — Chi tiết từng loại sản phẩm trong một phiếu nhập.
    /// Khóa chính kép: (MaLoaiSP, MaPhieuNhap). Không có audit columns (theo SQL).
    
    public class DTO_ChiTietPhieuNhap
    {
        /// MaLoaiSP — CHAR(10), PK (kép), FK → LoaiSanPham(MaLoaiSP), NOT NULL
        public string MaLoaiSP { get; set; }

        /// MaPhieuNhap — CHAR(10), PK (kép), FK → PhieuNhap(MaPhieuNhap), NOT NULL
        public string MaPhieuNhap { get; set; }

        /// SoLuong — INT, NOT NULL, > 0
        public int SoLuong { get; set; }

        /// GiaNhap — DECIMAL(15,2), NOT NULL, >= 0
        public decimal GiaNhap { get; set; }
    }
}

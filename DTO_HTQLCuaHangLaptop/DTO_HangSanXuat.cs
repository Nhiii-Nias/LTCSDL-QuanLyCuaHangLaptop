namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: HangSanXuat — Hãng sản xuất (ASUS, Dell, HP, Lenovo, Apple, Logitech…).
    
    public class DTO_HangSanXuat
    {
        /// MaHang — CHAR(10), PK, NOT NULL
        public string MaHang { get; set; }

        /// TenHang — NVARCHAR(100), NOT NULL, UNIQUE
        public string TenHang { get; set; }

        /// QuocGia — NVARCHAR(100), NULL
        public string QuocGia { get; set; }

        /// IsDeleted — BIT, NOT NULL, DEFAULT 0
        public bool IsDeleted { get; set; }
    }
}

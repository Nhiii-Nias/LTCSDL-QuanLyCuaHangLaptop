using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: LoaiSanPham — Một dòng sản phẩm của một hãng (VD: "ASUS VivoBook 15 X1504VA").
    /// Một LoaiSanPham chứa nhiều SanPham có serial riêng biệt.
    /// Lưu ý: bảng này KHÔNG có cột NguoiCapNhat (theo SQL).

    public class DTO_LoaiSanPham
    {
        /// MaLoaiSP — CHAR(10), PK, NOT NULL
        public string MaLoaiSP { get; set; }

        /// MaHang — CHAR(10), NOT NULL, FK → HangSanXuat(MaHang)
        public string MaHang { get; set; }

        /// TenLoai — NVARCHAR(200), NOT NULL
        public string TenLoai { get; set; }

        /// DanhMuc — NVARCHAR(50), NOT NULL, CHECK (Laptop | Chuột | Bàn Phím)
        public string DanhMuc { get; set; }

        /// ThoiGianBaoHanh — INT, NOT NULL, > 0 (đơn vị: tháng)
        public int ThoiGianBaoHanh { get; set; }

        /// GiaBanGoc — DECIMAL(15,2), NOT NULL, >= 0
        public decimal GiaBanGoc { get; set; }

        // ── Audit columns ─────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }

        /// NguoiTao — CHAR(10), NULL (FK → TaiKhoanNV.MaTK)
        public string NguoiTao { get; set; }

        /// IsDeleted — BIT, NOT NULL, DEFAULT 0 → bool
        public bool IsDeleted { get; set; }
    }
}

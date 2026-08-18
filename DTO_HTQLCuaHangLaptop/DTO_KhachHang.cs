using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: KhachHang — Bảng cha lưu thông tin chung của mọi khách hàng.
    /// Lưu ý: bảng này KHÔNG có cột NguoiCapNhat (theo SQL).
    
    public class DTO_KhachHang
    {
        /// MaKH — CHAR(10), PK, NOT NULL
        public string MaKH { get; set; }

        /// TenKH — NVARCHAR(50), NOT NULL
        public string TenKH { get; set; }

        /// Email — VARCHAR(100), NULL
        public string Email { get; set; }

        /// SDT — VARCHAR(10), NULL, chỉ chứa chữ số
        public string SDT { get; set; }

        /// DiaChi — NVARCHAR(200), NULL
        public string DiaChi { get; set; }

        /// LoaiKH — NVARCHAR(10), NOT NULL, CHECK (Lẻ | Sỉ)
        public string LoaiKH { get; set; }

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

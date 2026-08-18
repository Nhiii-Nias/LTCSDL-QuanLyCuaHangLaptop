using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: NhanVien — Thông tin nhân viên cửa hàng.

    public class DTO_NhanVien
    {
        /// MaNV — CHAR(10), PK, NOT NULL
        public string MaNV { get; set; }

        /// TenNV — NVARCHAR(50), NOT NULL
        public string TenNV { get; set; }

        /// GioiTinh — NVARCHAR(10), NULL, CHECK (Nam | Nữ)
        public string GioiTinh { get; set; }

        /// SinhNhat — DATE, NOT NULL → DateTime (bỏ phần Time khi dùng)
        public DateTime SinhNhat { get; set; }

        /// SDT — VARCHAR(10), NOT NULL, chỉ chứa chữ số
        public string SDT { get; set; }

        /// DiaChi — NVARCHAR(300), NOT NULL
        public string DiaChi { get; set; }

        /// Email — VARCHAR(100), NULL
        public string Email { get; set; }

        /// NgayVaoLam — DATE, NOT NULL → DateTime
        public DateTime NgayVaoLam { get; set; }

        /// Luong — DECIMAL(15,2), NOT NULL, >= 0
        public decimal Luong { get; set; }

        /// ChucVu — NVARCHAR(100), NOT NULL
        public string ChucVu { get; set; }

        // ── Audit columns ─────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }

        /// NguoiTao — CHAR(10), NULL (FK → TaiKhoanNV.MaTK)
        public string NguoiTao { get; set; }

        /// NguoiCapNhat — CHAR(10), NULL (FK → TaiKhoanNV.MaTK)
        public string NguoiCapNhat { get; set; }

        /// IsDeleted — BIT, NOT NULL, DEFAULT 0 → bool
        public bool IsDeleted { get; set; }
    }
}

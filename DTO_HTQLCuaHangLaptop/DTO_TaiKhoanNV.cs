using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: TaiKhoanNV — Tài khoản đăng nhập của nhân viên (WinForm).
    /// Lưu ý: bảng này KHÔNG có cột NguoiTao/NguoiCapNhat (theo SQL).

    public class DTO_TaiKhoanNV
    {
        /// MaTK — CHAR(10), PK, NOT NULL
        public string MaTK { get; set; }

        /// MaNV — CHAR(10), NOT NULL, FK → NhanVien(MaNV), UNIQUE
        public string MaNV { get; set; }

        /// MaVaiTro — CHAR(10), NOT NULL, FK → VaiTro(MaVaiTro)
        public string MaVaiTro { get; set; }

        /// TenDangNhap — VARCHAR(50), NOT NULL, UNIQUE
        public string TenDangNhap { get; set; }

        /// MatKhau — VARCHAR(255), NOT NULL (lưu dạng hash)
        public string MatKhau { get; set; }

        /// TrangThai — NVARCHAR(20), NOT NULL, CHECK (Hoạt Động | Khóa)
        public string TrangThai { get; set; }

        // ── Audit columns ─────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }
    }
}

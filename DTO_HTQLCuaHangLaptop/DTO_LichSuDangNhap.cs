using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: LichSuDangNhap — Ghi nhận mọi lần đăng nhập của tài khoản nhân viên.
    /// FK MaTK chỉ trỏ về TaiKhoanNV (không áp dụng cho TaiKhoanKH).

    public class DTO_LichSuDangNhap
    {
        /// MaLSDN — CHAR(10), PK, NOT NULL
        public string MaLSDN { get; set; }

        /// MaTK — CHAR(10), NOT NULL, FK → TaiKhoanNV(MaTK)
        public string MaTK { get; set; }

        /// ThoiGian — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime ThoiGian { get; set; }

        /// DiaChiIP — VARCHAR(45), NULL (đủ chứa cả IPv4 và IPv6)
        public string DiaChiIP { get; set; }

        /// TrangThai — NVARCHAR(20), NOT NULL, CHECK (Thành Công | Thất Bại)
        public string TrangThai { get; set; }
    }
}

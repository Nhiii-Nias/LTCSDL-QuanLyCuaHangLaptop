using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: TaiKhoanKH — Tài khoản đăng nhập của khách hàng lẻ (Website MVC).
    /// Lưu ý: NgayTao kiểu DATE (không phải DATETIME) theo đúng SQL.

    public class DTO_TaiKhoanKH
    {
        /// MaTK — CHAR(10), PK, NOT NULL
        public string MaTK { get; set; }

        /// MaKH — CHAR(10), NOT NULL, FK → KhachHang(MaKH), UNIQUE
        public string MaKH { get; set; }

        /// TenDangNhap — VARCHAR(50), NOT NULL, UNIQUE
        public string TenDangNhap { get; set; }

        /// MatKhau — VARCHAR(255), NOT NULL (lưu dạng hash)
        public string MatKhau { get; set; }

        /// 
        /// NgayTao — DATE, NOT NULL, DEFAULT CAST(GETDATE() AS DATE).
        /// Dùng DateTime (bỏ phần Time khi đọc/ghi); không nullable vì NOT NULL.
        /// 
        public DateTime NgayTao { get; set; }

        /// TrangThai — NVARCHAR(20), NOT NULL, CHECK (Hoạt Động | Khóa)
        public string TrangThai { get; set; }
    }
}

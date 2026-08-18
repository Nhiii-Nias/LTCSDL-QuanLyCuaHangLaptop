using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: DonKhieuNai — Đơn khiếu nại của khách hàng liên quan đến một đơn hàng.
    /// Lưu ý: NgayGui kiểu DATE; NgayTao kiểu DATETIME (theo SQL).
    /// Bảng này không có NgayCapNhat/NguoiTao (theo SQL).
    
    public class DTO_DonKhieuNai
    {
        /// MaDonKN — CHAR(10), PK, NOT NULL
        public string MaDonKN { get; set; }

        /// MaDH — CHAR(10), NOT NULL, FK → DonHang(MaDH)
        public string MaDH { get; set; }

        /// MaKH — CHAR(10), NOT NULL, FK → KhachHang(MaKH)
        public string MaKH { get; set; }

        /// NoiDung — NVARCHAR(1000), NOT NULL
        public string NoiDung { get; set; }

        /// NgayGui — DATE, NOT NULL, DEFAULT CAST(GETDATE() AS DATE) → DateTime
        public DateTime NgayGui { get; set; }

        /// TrangThai — NVARCHAR(50), NOT NULL, CHECK (Đang Xử Lý | Đã Giải Quyết | Từ Chối)
        public string TrangThai { get; set; }

        /// KetQua — NVARCHAR(500), NULL
        public string KetQua { get; set; }

        // ── Audit column ──────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }
    }
}

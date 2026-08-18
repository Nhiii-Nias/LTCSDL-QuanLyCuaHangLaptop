using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: PhieuNhap — Phiếu nhập hàng từ nhà cung cấp.
    /// Lưu ý: NgayNhap kiểu DATE (không phải DATETIME) theo SQL.

    public class DTO_PhieuNhap
    {
        /// MaPhieuNhap — CHAR(10), PK, NOT NULL
        public string MaPhieuNhap { get; set; }

        /// MaNV — CHAR(10), NOT NULL, FK → NhanVien(MaNV)
        public string MaNV { get; set; }

        /// MaNCC — CHAR(10), NOT NULL, FK → NhaCungCap(MaNCC)
        public string MaNCC { get; set; }

        /// 
        /// NgayNhap — DATE, NOT NULL, DEFAULT CAST(GETDATE() AS DATE) → DateTime.
        /// Chỉ dùng phần Date khi đọc/ghi.
        /// 
        public DateTime NgayNhap { get; set; }

        /// TongTien — DECIMAL(15,2), NOT NULL, >= 0
        public decimal TongTien { get; set; }

        /// TrangThai — NVARCHAR(50), NOT NULL, CHECK (Chờ Xác Nhận | Đã Nhập | Huỷ)
        public string TrangThai { get; set; }

        // ── Audit columns ─────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }

        /// NguoiTao — CHAR(10), NULL (FK → TaiKhoanNV.MaTK)
        public string NguoiTao { get; set; }
    }
}

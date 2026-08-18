using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: DonHang — Đơn hàng của khách hàng (lẻ hoặc sỉ).
    /// MaKM và MaHD là nullable vì không phải đơn nào cũng có khuyến mãi / hợp đồng.
    /// Lưu ý: bảng có 2 cột thời gian riêng: NgayDat (DATETIME) và NgayTao (DATETIME).
    
    public class DTO_DonHang
    {
        /// MaDH — CHAR(10), PK, NOT NULL
        public string MaDH { get; set; }

        /// MaNV — CHAR(10), NOT NULL, FK → NhanVien(MaNV)
        public string MaNV { get; set; }

        /// MaKH — CHAR(10), NOT NULL, FK → KhachHang(MaKH)
        public string MaKH { get; set; }

        /// MaKM — CHAR(10), NULL, FK → KhuyenMai(MaKM)
        public string MaKM { get; set; }

        /// MaHD — CHAR(10), NULL, FK → HopDong(MaHD) (chỉ có với khách sỉ)
        public string MaHD { get; set; }

        /// NgayDat — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayDat { get; set; }

        /// TongTien — DECIMAL(15,2), NOT NULL, >= 0 (tổng trước giảm)
        public decimal TongTien { get; set; }

        /// TienSauGiam — DECIMAL(15,2), NULL, >= 0 (sau khi áp dụng khuyến mãi)
        public decimal? TienSauGiam { get; set; }

        /// PhuongThucThanhToan — NVARCHAR(100), NOT NULL, CHECK (Tiền Mặt | Chuyển Khoản | Thẻ)
        public string PhuongThucThanhToan { get; set; }

        /// TrangThai — NVARCHAR(50), NOT NULL, CHECK (Chờ Xử Lý | Đang Giao | Hoàn Thành | Huỷ)
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

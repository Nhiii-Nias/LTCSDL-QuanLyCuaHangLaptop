using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: HopDong — Hợp đồng mua bán với khách hàng sỉ (doanh nghiệp).
    
    public class DTO_HopDong
    {
        /// MaHD — CHAR(10), PK, NOT NULL
        public string MaHD { get; set; }

        /// MaNV — CHAR(10), NOT NULL, FK → NhanVien(MaNV)
        public string MaNV { get; set; }

        /// MaKH — CHAR(10), NOT NULL, FK → KhachHang(MaKH)
        public string MaKH { get; set; }

        /// NgayKy — DATE, NOT NULL → DateTime
        public DateTime NgayKy { get; set; }

        /// GiaTriHD — DECIMAL(15,2), NOT NULL, >= 0
        public decimal GiaTriHD { get; set; }

        /// NgayHieuLuc — DATE, NOT NULL → DateTime
        public DateTime NgayHieuLuc { get; set; }

        /// NgayHetHan — DATE, NOT NULL, > NgayHieuLuc → DateTime
        public DateTime NgayHetHan { get; set; }

        /// TrangThai — NVARCHAR(50), NOT NULL, CHECK (Hiệu Lực | Hết Hạn | Huỷ)
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

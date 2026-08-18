using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: PhieuDoiTra — Phiếu đổi trả sản phẩm (trong vòng 30 ngày, lỗi nhà sản xuất).
    /// MaSerialSP có ràng buộc UNIQUE: mỗi serial chỉ có 1 phiếu đổi trả.

    public class DTO_PhieuDoiTra
    {
        /// MaPhieuDT — CHAR(10), PK, NOT NULL
        public string MaPhieuDT { get; set; }

        /// MaDH — CHAR(10), NOT NULL, FK → DonHang(MaDH)
        public string MaDH { get; set; }

        /// MaSerialSP — VARCHAR(50), NOT NULL, FK → SanPham(MaSerialSP), UNIQUE
        public string MaSerialSP { get; set; }

        /// MaKH — CHAR(10), NOT NULL, FK → KhachHang(MaKH)
        public string MaKH { get; set; }

        /// NgayYeuCau — DATE, NOT NULL, DEFAULT CAST(GETDATE() AS DATE) → DateTime
        public DateTime NgayYeuCau { get; set; }

        /// LyDo — NVARCHAR(500), NOT NULL
        public string LyDo { get; set; }

        /// LoaiXuLy — NVARCHAR(50), NOT NULL, CHECK (Đổi Máy | Hoàn Tiền | Từ Chối)
        public string LoaiXuLy { get; set; }

        /// TrangThai — NVARCHAR(50), NOT NULL, CHECK (Đang Xử Lý | Hoàn Thành | Từ Chối)
        public string TrangThai { get; set; }

        // ── Audit columns ─────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }
    }
}

using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: PhieuBaoHanh — Phiếu bảo hành sản phẩm của khách hàng lẻ.
    /// LoaiBH xác định bảo hành tại cửa hàng hay tại hãng.

    public class DTO_PhieuBaoHanh
    {
        /// MaPBH — CHAR(10), PK, NOT NULL
        public string MaPBH { get; set; }

        /// MaDH — CHAR(10), NOT NULL, FK → DonHang(MaDH)
        public string MaDH { get; set; }

        /// MaKH — CHAR(10), NOT NULL, FK → KhachHang(MaKH)
        public string MaKH { get; set; }

        /// MaSerialSP — VARCHAR(50), NOT NULL, FK → SanPham(MaSerialSP)
        public string MaSerialSP { get; set; }

        /// LoaiBH — NVARCHAR(50), NOT NULL, CHECK (Cửa Hàng | Hãng)
        public string LoaiBH { get; set; }

        /// TrangThai — NVARCHAR(50), NOT NULL, CHECK (Đang Xử Lý | Hoàn Thành | Từ Chối)
        public string TrangThai { get; set; }

        /// NgayBatDau — DATE, NOT NULL → DateTime
        public DateTime NgayBatDau { get; set; }

        /// NgayKetThuc — DATE, NOT NULL, > NgayBatDau → DateTime
        public DateTime NgayKetThuc { get; set; }

        /// LyDoLoi — NVARCHAR(500), NULL
        public string? LyDoLoi { get; set; }

        /// KetQua — NVARCHAR(500), NULL
        public string? KetQua { get; set; }

        // ── Audit columns ─────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }
    }
}

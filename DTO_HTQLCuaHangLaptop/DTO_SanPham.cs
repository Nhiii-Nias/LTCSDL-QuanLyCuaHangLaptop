using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: SanPham — Sản phẩm vật lý, phân biệt bằng số serial duy nhất.
    /// Lưu ý: bảng này KHÔNG có cột NguoiTao/NguoiCapNhat (theo SQL).

    public class DTO_SanPham
    {
        /// MaSerialSP — VARCHAR(50), PK, NOT NULL
        public string MaSerialSP { get; set; }

        /// MaPhieuNhap — CHAR(10), NOT NULL, FK → PhieuNhap(MaPhieuNhap)
        public string MaPhieuNhap { get; set; }

        /// MaLoaiSP — CHAR(10), NOT NULL, FK → LoaiSanPham(MaLoaiSP)
        public string MaLoaiSP { get; set; }

        /// NgayNhap — DATE, NOT NULL → DateTime
        public DateTime NgayNhap { get; set; }

        /// NgaySX — DATE, NULL → DateTime?
        public DateTime? NgaySX { get; set; }

        /// TrangThai — NVARCHAR(50), NOT NULL, CHECK (Trong Kho | Đã Bán | Bảo Hành | Lỗi | Đổi Trả)
        public string TrangThai { get; set; }

        // ── Audit columns ─────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }

        /// IsDeleted — BIT, NOT NULL, DEFAULT 0 → bool
        public bool IsDeleted { get; set; }
    }
}

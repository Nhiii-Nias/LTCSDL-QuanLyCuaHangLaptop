using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: KhuyenMai — Chương trình khuyến mãi (tối đa 4 loại, không áp dụng đồng thời).
    /// Lưu ý: bảng này chỉ có NgayTao, không có NgayCapNhat hay NguoiTao (theo SQL).

    public class DTO_KhuyenMai
    {
        /// MaKM — CHAR(10), PK, NOT NULL
        public string MaKM { get; set; }

        /// TenKM — NVARCHAR(200), NOT NULL
        public string TenKM { get; set; }

        /// DoiTuong — NVARCHAR(100), NOT NULL, CHECK (Tất Cả | HSSV | Doanh Nghiệp)
        public string DoiTuong { get; set; }

        /// DieuKien — NVARCHAR(500), NULL
        public string DieuKien { get; set; }

        /// NgayBatDau — DATE, NOT NULL → DateTime
        public DateTime NgayBatDau { get; set; }

        /// NgayKetThuc — DATE, NOT NULL, >= NgayBatDau → DateTime
        public DateTime NgayKetThuc { get; set; }

        /// MoTa — NVARCHAR(500), NULL
        public string MoTa { get; set; }

        /// MucGiamSP — DECIMAL(5,2), NULL (% giảm trên từng sản phẩm)
        public decimal? MucGiamSP { get; set; }

        /// MucGiamDH — DECIMAL(5,2), NULL (% giảm trên tổng đơn hàng)
        public decimal? MucGiamDH { get; set; }

        /// SLToiThieu — INT, NULL (số lượng sản phẩm tối thiểu để đủ điều kiện)
        public int? SLToiThieu { get; set; }

        /// isHienThi — BIT, NOT NULL, DEFAULT 1
        public bool IsHienThi { get; set; }

        // ── Audit column ──────────────────────────────────────────────
        /// NgayTao — DATETIME, NOT NULL, DEFAULT GETDATE()
        public DateTime NgayTao { get; set; }
    }
}

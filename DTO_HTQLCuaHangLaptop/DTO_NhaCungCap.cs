using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: NhaCungCap — Nhà cung cấp hàng hóa cho cửa hàng.
    /// Không có audit columns (theo SQL).

    public class DTO_NhaCungCap
    {
        /// MaNCC — CHAR(10), PK, NOT NULL
        public string MaNCC { get; set; }

        /// TenNCC — NVARCHAR(200), NOT NULL
        public string TenNCC { get; set; }

        /// Email — VARCHAR(150), NULL
        public string Email { get; set; }

        /// SDT — VARCHAR(10), NULL, chỉ chứa chữ số
        public string SDT { get; set; }

        /// DiaChi — NVARCHAR(300), NULL
        public string DiaChi { get; set; }

        /// NgayCapNhat — DATETIME, NULL
        public DateTime? NgayCapNhat { get; set; }

        /// IsDeleted — BIT, NOT NULL, DEFAULT 0 → bool
        public bool IsDeleted { get; set; }
    }
}


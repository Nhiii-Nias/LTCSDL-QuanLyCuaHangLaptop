using System;

namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: KhachHangLe — Bảng con kế thừa từ KhachHang.
    /// Khóa chính MaKHLe đồng thời là FK → KhachHang(MaKH).

    public class DTO_KhachHangLe
    {
        /// MaKHLe — CHAR(10), PK + FK → KhachHang(MaKH), NOT NULL
        public string MaKHLe { get; set; }

        /// LaHSSV — BIT, NOT NULL → bool (true = là học sinh/sinh viên)
        public bool LaHSSV { get; set; }

        /// SinhNhat — DATE, NULL → DateTime?
        public DateTime? SinhNhat { get; set; }
    }
}

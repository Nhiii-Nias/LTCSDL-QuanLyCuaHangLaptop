namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: VaiTro — Vai trò phân quyền của tài khoản nhân viên.

    public class DTO_VaiTro
    {
        /// MaVaiTro — CHAR(10), PK, NOT NULL
        public string MaVaiTro { get; set; }

        /// TenVaiTro — NVARCHAR(50), NOT NULL
        public string TenVaiTro { get; set; }

        /// MoTaQuyen — NVARCHAR(500), NULL
        public string MoTaQuyen { get; set; }
    }
}

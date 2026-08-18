namespace DTO_HTQLCuaHangLaptop
{
    /// Bảng: CauHinh — Thuộc tính kỹ thuật của một loại sản phẩm (CPU, RAM, màn hình…).
    /// Mỗi bản ghi là một dòng thông số; không có audit columns (theo SQL).
    /// 
    public class DTO_CauHinh
    {
        /// MaCauHinh — CHAR(10), PK, NOT NULL
        public string MaCauHinh { get; set; }

        /// MaLoaiSP — CHAR(10), NOT NULL, FK → LoaiSanPham(MaLoaiSP)
        public string MaLoaiSP { get; set; }

        /// TenThuocTinh — NVARCHAR(150), NOT NULL
        public string TenThuocTinh { get; set; }
    }
}

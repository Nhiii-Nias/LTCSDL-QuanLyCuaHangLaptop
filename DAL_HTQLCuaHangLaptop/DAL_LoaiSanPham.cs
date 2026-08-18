using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_LoaiSanPham : DBConnect
    {
        /// Lấy toàn bộ danh sách loại sản phẩm chưa bị xóa (IsDeleted = 0).
        /// Trả về DataTable chứa danh sách loại sản phẩm.
        public DataTable DSLoaiSP()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT l.MaLoaiSP, l.MaHang, h.TenHang, l.TenLoai, l.DanhMuc, l.ThoiGianBaoHanh, l.GiaBanGoc, l.NgayTao, l.NgayCapNhat, l.NguoiTao, l.IsDeleted " +
                         "FROM LoaiSanPham l " +
                         "INNER JOIN HangSanXuat h ON l.MaHang = h.MaHang";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            
            try
            {
                _conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return dt;
        }

        /// Lấy thông tin loại sản phẩm theo mã loại (chỉ tìm loại chưa bị xóa).
        /// Trả về DTO_LoaiSanPham nếu tìm thấy, ngược lại trả về null.
        public DTO_LoaiSanPham TimLoaiSP(string maLoaiSP)
        {
            DTO_LoaiSanPham? lsp = null;
            string sql = "SELECT MaLoaiSP, MaHang, TenLoai, DanhMuc, ThoiGianBaoHanh, GiaBanGoc, NgayTao, NgayCapNhat, NguoiTao, IsDeleted " +
                         "FROM LoaiSanPham WHERE MaLoaiSP = @MaLoaiSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lsp = new DTO_LoaiSanPham
                        {
                            MaLoaiSP = reader["MaLoaiSP"].ToString()!.Trim(),
                            MaHang = reader["MaHang"].ToString()!.Trim(),
                            TenLoai = reader["TenLoai"].ToString()!,
                            DanhMuc = reader["DanhMuc"].ToString()!,
                            ThoiGianBaoHanh = Convert.ToInt32(reader["ThoiGianBaoHanh"]),
                            GiaBanGoc = Convert.ToDecimal(reader["GiaBanGoc"]),
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"]),
                            NguoiTao = reader["NguoiTao"] == DBNull.Value ? null : reader["NguoiTao"].ToString()!.Trim(),
                            IsDeleted = Convert.ToBoolean(reader["IsDeleted"])
                        };
                    }
                }
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return lsp;
        }

        /// Thêm một loại sản phẩm mới vào cơ sở dữ liệu.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemLoaiSP(DTO_LoaiSanPham lsp)
        {
            string sql = "INSERT INTO LoaiSanPham (MaLoaiSP, MaHang, TenLoai, DanhMuc, ThoiGianBaoHanh, GiaBanGoc, NgayTao, NgayCapNhat, NguoiTao, IsDeleted) " +
                         "VALUES (@MaLoaiSP, @MaHang, @TenLoai, @DanhMuc, @ThoiGianBaoHanh, @GiaBanGoc, @NgayTao, @NgayCapNhat, @NguoiTao, 0)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = lsp.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@MaHang", SqlDbType.Char, 10) { Value = lsp.MaHang });
            cmd.Parameters.Add(new SqlParameter("@TenLoai", SqlDbType.NVarChar, 200) { Value = lsp.TenLoai });
            cmd.Parameters.Add(new SqlParameter("@DanhMuc", SqlDbType.NVarChar, 50) { Value = lsp.DanhMuc });
            cmd.Parameters.Add(new SqlParameter("@ThoiGianBaoHanh", SqlDbType.Int) { Value = lsp.ThoiGianBaoHanh });
            cmd.Parameters.Add(new SqlParameter("@GiaBanGoc", SqlDbType.Decimal) { Value = lsp.GiaBanGoc, Precision = 15, Scale = 2 });
            cmd.Parameters.Add(new SqlParameter("@NgayTao", SqlDbType.DateTime) { Value = lsp.NgayTao == default(DateTime) ? DateTime.Now : lsp.NgayTao });
            
            // Đảm bảo các kiểu dữ liệu nullable (DateTime?, string/nullable char) được kiểm tra DBNull.Value trước khi nạp vào SqlParameter
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = lsp.NgayCapNhat.HasValue ? (object)lsp.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiTao", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(lsp.NguoiTao) ? DBNull.Value : lsp.NguoiTao });

            int rowsAffected = 0;
            try
            {
                _conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return rowsAffected > 0;
        }

        /// Cập nhật thông tin loại sản phẩm (chỉ cập nhật loại chưa bị xóa).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatLoaiSP(DTO_LoaiSanPham lsp)
        {
            string sql = "UPDATE LoaiSanPham SET MaHang = @MaHang, TenLoai = @TenLoai, DanhMuc = @DanhMuc, " +
                         "ThoiGianBaoHanh = @ThoiGianBaoHanh, GiaBanGoc = @GiaBanGoc, NgayCapNhat = @NgayCapNhat, NguoiTao = @NguoiTao, IsDeleted = @IsDeleted " +
                         "WHERE MaLoaiSP = @MaLoaiSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = lsp.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@MaHang", SqlDbType.Char, 10) { Value = lsp.MaHang });
            cmd.Parameters.Add(new SqlParameter("@TenLoai", SqlDbType.NVarChar, 200) { Value = lsp.TenLoai });
            cmd.Parameters.Add(new SqlParameter("@DanhMuc", SqlDbType.NVarChar, 50) { Value = lsp.DanhMuc });
            cmd.Parameters.Add(new SqlParameter("@ThoiGianBaoHanh", SqlDbType.Int) { Value = lsp.ThoiGianBaoHanh });
            cmd.Parameters.Add(new SqlParameter("@GiaBanGoc", SqlDbType.Decimal) { Value = lsp.GiaBanGoc, Precision = 15, Scale = 2 });
            
            // Đảm bảo các kiểu dữ liệu nullable (DateTime?, string/nullable char) được kiểm tra DBNull.Value trước khi nạp vào SqlParameter
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = lsp.NgayCapNhat.HasValue ? (object)lsp.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiTao", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(lsp.NguoiTao) ? DBNull.Value : lsp.NguoiTao });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = lsp.IsDeleted });

            int rowsAffected = 0;
            try
            {
                _conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return rowsAffected > 0;
        }

        /// Xóa mềm loại sản phẩm bằng cách cập nhật cột IsDeleted = 1.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaMemLoaiSP(string maLoaiSP)
        {
            string sql = "UPDATE LoaiSanPham SET IsDeleted = 1 WHERE MaLoaiSP = @MaLoaiSP AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });

            int rowsAffected = 0;
            try
            {
                _conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return rowsAffected > 0;
        }

        /// Lấy danh sách loại sản phẩm theo hãng sản xuất (chưa bị xóa).
        /// Trả về DataTable chứa danh sách loại sản phẩm theo hãng.
        public DataTable DSLoaiSPTheoHang(string maHang)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT l.MaLoaiSP, l.MaHang, h.TenHang, l.TenLoai, l.DanhMuc, l.ThoiGianBaoHanh, l.GiaBanGoc, l.NgayTao, l.NgayCapNhat, l.NguoiTao, l.IsDeleted " +
                         "FROM LoaiSanPham l " +
                         "INNER JOIN HangSanXuat h ON l.MaHang = h.MaHang " +
                         "WHERE l.MaHang = @MaHang";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaHang", SqlDbType.Char, 10) { Value = maHang });

            try
            {
                _conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return dt;
        }

        /// Lấy danh sách loại sản phẩm theo danh mục (chưa bị xóa).
        /// Trả về DataTable chứa danh sách loại sản phẩm theo danh mục.
        public DataTable DSLoaiSPTheoDanhMuc(string danhMuc)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT l.MaLoaiSP, l.MaHang, h.TenHang, l.TenLoai, l.DanhMuc, l.ThoiGianBaoHanh, l.GiaBanGoc, l.NgayTao, l.NgayCapNhat, l.NguoiTao, l.IsDeleted " +
                         "FROM LoaiSanPham l " +
                         "INNER JOIN HangSanXuat h ON l.MaHang = h.MaHang " +
                         "WHERE l.DanhMuc = @DanhMuc";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@DanhMuc", SqlDbType.NVarChar, 50) { Value = danhMuc });

            try
            {
                _conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return dt;
        }
    }
}

using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_SanPham : DBConnect
    {
        /// Lấy toàn bộ danh sách sản phẩm chưa bị xóa (IsDeleted = 0).
        /// Trả về DataTable chứa danh sách sản phẩm.
        public DataTable DSTatCaSanPham()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT sp.MaSerialSP, sp.MaPhieuNhap, sp.MaLoaiSP, sp.NgayNhap, sp.NgaySX, sp.TrangThai, sp.NgayTao, sp.NgayCapNhat, sp.IsDeleted, lsp.TenLoai, lsp.DanhMuc " +
                         "FROM SanPham sp " +
                         "LEFT JOIN LoaiSanPham lsp ON sp.MaLoaiSP = lsp.MaLoaiSP";
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

        /// Lấy thông tin sản phẩm theo số serial (chỉ tìm sản phẩm chưa bị xóa).
        /// Trả về DTO_SanPham nếu tìm thấy, ngược lại trả về null.
        public DTO_SanPham? DSTheoMaSerialSP(string maSerial)
        {
            DTO_SanPham? sp = null;
            string sql = "SELECT MaSerialSP, MaPhieuNhap, MaLoaiSP, NgayNhap, NgaySX, TrangThai, NgayTao, NgayCapNhat, IsDeleted " +
                         "FROM SanPham WHERE MaSerialSP = @MaSerialSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sp = new DTO_SanPham
                        {
                            MaSerialSP = reader["MaSerialSP"].ToString()!.Trim(),
                            MaPhieuNhap = reader["MaPhieuNhap"].ToString()!.Trim(),
                            MaLoaiSP = reader["MaLoaiSP"].ToString()!.Trim(),
                            NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                            NgaySX = reader["NgaySX"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgaySX"]),
                            TrangThai = reader["TrangThai"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"]),
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
            return sp;
        }

        /// Thêm một sản phẩm mới vào cơ sở dữ liệu.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemSanPham(DTO_SanPham sp)
        {
            string sql = "INSERT INTO SanPham (MaSerialSP, MaPhieuNhap, MaLoaiSP, NgayNhap, NgaySX, TrangThai, NgayTao, NgayCapNhat, IsDeleted) " +
                         "VALUES (@MaSerialSP, @MaPhieuNhap, @MaLoaiSP, @NgayNhap, @NgaySX, @TrangThai, @NgayTao, @NgayCapNhat, 0)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = sp.MaSerialSP });
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = sp.MaPhieuNhap });
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = sp.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@NgayNhap", SqlDbType.Date) { Value = sp.NgayNhap });
            cmd.Parameters.Add(new SqlParameter("@NgaySX", SqlDbType.Date) { Value = sp.NgaySX.HasValue ? (object)sp.NgaySX.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = sp.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayTao", SqlDbType.DateTime) { Value = sp.NgayTao == default(DateTime) ? DateTime.Now : sp.NgayTao });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = sp.NgayCapNhat.HasValue ? (object)sp.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật thông tin sản phẩm (chỉ cập nhật sản phẩm chưa bị xóa).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatSanPham(DTO_SanPham sp)
        {
            string sql = "UPDATE SanPham SET MaPhieuNhap = @MaPhieuNhap, MaLoaiSP = @MaLoaiSP, NgayNhap = @NgayNhap, " +
                         "NgaySX = @NgaySX, TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat, IsDeleted = @IsDeleted " +
                         "WHERE MaSerialSP = @MaSerialSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = sp.MaSerialSP });
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = sp.MaPhieuNhap });
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = sp.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@NgayNhap", SqlDbType.Date) { Value = sp.NgayNhap });
            cmd.Parameters.Add(new SqlParameter("@NgaySX", SqlDbType.Date) { Value = sp.NgaySX.HasValue ? (object)sp.NgaySX.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = sp.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = sp.NgayCapNhat.HasValue ? (object)sp.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = sp.IsDeleted });

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

        /// Xóa mềm sản phẩm bằng cách cập nhật IsDeleted = 1 và TrangThai = N'Lỗi'.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaMemSanPham(string maSerial)
        {
            string sql = "UPDATE SanPham SET IsDeleted = 1, TrangThai = N'Lỗi' WHERE MaSerialSP = @MaSerialSP AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });

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

        /// Lấy danh sách sản phẩm theo trạng thái (chưa bị xóa).
        /// Trả về DataTable chứa danh sách sản phẩm.
        public DataTable DSTheoTrangThai(string trangThai)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT sp.MaSerialSP, sp.MaPhieuNhap, sp.MaLoaiSP, sp.NgayNhap, sp.NgaySX, sp.TrangThai, sp.NgayTao, sp.NgayCapNhat, sp.IsDeleted, lsp.TenLoai " +
                         "FROM SanPham sp " +
                         "LEFT JOIN LoaiSanPham lsp ON sp.MaLoaiSP = lsp.MaLoaiSP " +
                         "WHERE sp.TrangThai = @TrangThai";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = trangThai });

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

        /// Lấy danh sách sản phẩm theo loại sản phẩm (chưa bị xóa).
        /// Trả về DataTable chứa danh sách sản phẩm.
        public DataTable DSTheoLoaiSP(string maLoaiSP)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT sp.MaSerialSP, sp.MaPhieuNhap, sp.MaLoaiSP, sp.NgayNhap, sp.NgaySX, sp.TrangThai, sp.NgayTao, sp.NgayCapNhat, sp.IsDeleted, lsp.TenLoai " +
                         "FROM SanPham sp " +
                         "LEFT JOIN LoaiSanPham lsp ON sp.MaLoaiSP = lsp.MaLoaiSP " +
                         "WHERE sp.MaLoaiSP = @MaLoaiSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });

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

        /// Cập nhật trạng thái sản phẩm độc lập.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTrangThai(string maSerial, string trangThai)
        {
            string sql = "UPDATE SanPham SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() " +
                         "WHERE MaSerialSP = @MaSerialSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = trangThai });

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

        /// Lấy danh sách sản phẩm theo mã phiếu nhập (chưa bị xóa mềm).
        /// Dùng trong BUS_KhoHang khi xác nhận phiếu để cập nhật TrangThai các serial.
        /// Trả về DataTable chứa danh sách serial của phiếu nhập đó.
        public DataTable DSTheoPhieuNhap(string maPhieuNhap)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT sp.MaSerialSP, sp.MaPhieuNhap, sp.MaLoaiSP, sp.NgayNhap, sp.NgaySX, sp.TrangThai, sp.NgayTao, sp.NgayCapNhat, sp.IsDeleted, lsp.TenLoai " +
                         "FROM SanPham sp " +
                         "LEFT JOIN LoaiSanPham lsp ON sp.MaLoaiSP = lsp.MaLoaiSP " +
                         "WHERE sp.MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap });

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

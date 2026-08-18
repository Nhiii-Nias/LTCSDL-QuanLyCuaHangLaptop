using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_TaiKhoanNV : DBConnect
    {
        /// Lấy toàn bộ danh sách tài khoản nhân viên.
        /// Trả về DataTable chứa danh sách tài khoản nhân viên.
        public DataTable DSTatCaTaiKhoanNV()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaTK, MaNV, MaVaiTro, TenDangNhap, MatKhau, TrangThai, NgayTao, NgayCapNhat FROM TaiKhoanNV";
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

        /// Lấy thông tin tài khoản theo mã tài khoản.
        /// Trả về DTO_TaiKhoanNV nếu tìm thấy, ngược lại trả về null.
        public DTO_TaiKhoanNV? DSTheoMaTK(string maTK)
        {
            DTO_TaiKhoanNV? tk = null;
            string sql = "SELECT MaTK, MaNV, MaVaiTro, TenDangNhap, MatKhau, TrangThai, NgayTao, NgayCapNhat FROM TaiKhoanNV WHERE MaTK = @MaTK";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = maTK });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tk = new DTO_TaiKhoanNV
                        {
                            MaTK = reader["MaTK"].ToString()!.Trim(),
                            MaNV = reader["MaNV"].ToString()!.Trim(),
                            MaVaiTro = reader["MaVaiTro"].ToString()!.Trim(),
                            TenDangNhap = reader["TenDangNhap"].ToString()!,
                            MatKhau = reader["MatKhau"].ToString()!,
                            TrangThai = reader["TrangThai"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"])
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
            return tk;
        }

        /// Lấy thông tin tài khoản theo tên đăng nhập (hỗ trợ đăng nhập).
        /// Trả về DTO_TaiKhoanNV nếu tìm thấy, ngược lại trả về null.
        public DTO_TaiKhoanNV? DSTheoTenDangNhap(string tenDangNhap)
        {
            DTO_TaiKhoanNV? tk = null;
            string sql = "SELECT MaTK, MaNV, MaVaiTro, TenDangNhap, MatKhau, TrangThai, NgayTao, NgayCapNhat FROM TaiKhoanNV WHERE TenDangNhap = @TenDangNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tk = new DTO_TaiKhoanNV
                        {
                            MaTK = reader["MaTK"].ToString()!.Trim(),
                            MaNV = reader["MaNV"].ToString()!.Trim(),
                            MaVaiTro = reader["MaVaiTro"].ToString()!.Trim(),
                            TenDangNhap = reader["TenDangNhap"].ToString()!,
                            MatKhau = reader["MatKhau"].ToString()!,
                            TrangThai = reader["TrangThai"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"])
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
            return tk;
        }

        /// Thêm tài khoản nhân viên mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemTaiKhoanNV(DTO_TaiKhoanNV tk)
        {
            string sql = "INSERT INTO TaiKhoanNV (MaTK, MaNV, MaVaiTro, TenDangNhap, MatKhau, TrangThai, NgayTao, NgayCapNhat) " +
                         "VALUES (@MaTK, @MaNV, @MaVaiTro, @TenDangNhap, @MatKhau, @TrangThai, @NgayTao, @NgayCapNhat)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = tk.MaTK });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = tk.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaVaiTro", SqlDbType.Char, 10) { Value = tk.MaVaiTro });
            cmd.Parameters.Add(new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tk.TenDangNhap });
            cmd.Parameters.Add(new SqlParameter("@MatKhau", SqlDbType.VarChar, 255) { Value = tk.MatKhau });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = tk.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayTao", SqlDbType.DateTime) { Value = tk.NgayTao == default(DateTime) ? DateTime.Now : tk.NgayTao });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = tk.NgayCapNhat.HasValue ? (object)tk.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật thông tin tài khoản nhân viên.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTaiKhoanNV(DTO_TaiKhoanNV tk)
        {
            string sql = "UPDATE TaiKhoanNV SET MaNV = @MaNV, MaVaiTro = @MaVaiTro, TenDangNhap = @TenDangNhap, " +
                         "MatKhau = @MatKhau, TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat WHERE MaTK = @MaTK";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = tk.MaTK });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = tk.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaVaiTro", SqlDbType.Char, 10) { Value = tk.MaVaiTro });
            cmd.Parameters.Add(new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tk.TenDangNhap });
            cmd.Parameters.Add(new SqlParameter("@MatKhau", SqlDbType.VarChar, 255) { Value = tk.MatKhau });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = tk.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = tk.NgayCapNhat.HasValue ? (object)tk.NgayCapNhat.Value : DBNull.Value });

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

        /// Lấy mã tài khoản NV mới nhất (để sinh mã tự động).
        public string? LayMaTKNVMoiNhat()
        {
            string? maMax = null;
            string sql = "SELECT MAX(MaTK) FROM TaiKhoanNV WHERE MaTK LIKE 'TKNV%'";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            try
            {
                _conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                    maMax = result.ToString();
            }
            finally
            {
                if (_conn.State == ConnectionState.Open) _conn.Close();
            }
            return maMax;
        }

        /// Lấy danh sách nhân viên chưa có tài khoản (để điền vào combobox khi tạo TK mới).
        public DataTable LayDanhSachNVChuaCoTaiKhoan()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaNV, TenNV FROM NhanVien WHERE IsDeleted = 0 AND MaNV NOT IN (SELECT MaNV FROM TaiKhoanNV)";
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
                if (_conn.State == ConnectionState.Open) _conn.Close();
            }
            return dt;
        }

        /// Cập nhật trạng thái hoạt động của tài khoản (Khóa hoặc Hoạt Động).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTrangThai(string maTK, string trangThai)
        {
            string sql = "UPDATE TaiKhoanNV SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() WHERE MaTK = @MaTK";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = maTK });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = trangThai });

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

        /// Cập nhật trạng thái hoạt động của tài khoản theo mã nhân viên.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTrangThaiTheoMaNV(string maNV, string trangThai)
        {
            string sql = "UPDATE TaiKhoanNV SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() WHERE MaNV = @MaNV";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = maNV });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = trangThai });

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
    }
}

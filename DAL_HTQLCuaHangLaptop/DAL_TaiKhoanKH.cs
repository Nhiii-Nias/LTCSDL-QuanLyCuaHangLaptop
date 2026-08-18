using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_TaiKhoanKH : DBConnect
    {
        /// Lấy toàn bộ danh sách tài khoản khách hàng.
        /// Trả về DataTable chứa danh sách tài khoản.
        public DataTable DSTatCaTaiKhoanKH()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaTK, MaKH, TenDangNhap, MatKhau, NgayTao, TrangThai FROM TaiKhoanKH";
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
        /// Trả về DTO_TaiKhoanKH nếu tìm thấy, ngược lại trả về null.
        public DTO_TaiKhoanKH? DSTheoMaTK(string maTK)
        {
            DTO_TaiKhoanKH? tk = null;
            string sql = "SELECT MaTK, MaKH, TenDangNhap, MatKhau, NgayTao, TrangThai FROM TaiKhoanKH WHERE MaTK = @MaTK";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = maTK });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tk = new DTO_TaiKhoanKH
                        {
                            MaTK = reader["MaTK"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            TenDangNhap = reader["TenDangNhap"].ToString()!,
                            MatKhau = reader["MatKhau"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            TrangThai = reader["TrangThai"].ToString()!
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

        /// Lấy thông tin tài khoản theo tên đăng nhập (hỗ trợ đăng nhập website).
        /// Trả về DTO_TaiKhoanKH nếu tìm thấy, ngược lại trả về null.
        public DTO_TaiKhoanKH? DSTheoTenDangNhap(string tenDangNhap)
        {
            DTO_TaiKhoanKH? tk = null;
            string sql = "SELECT MaTK, MaKH, TenDangNhap, MatKhau, NgayTao, TrangThai FROM TaiKhoanKH WHERE TenDangNhap = @TenDangNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tenDangNhap });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tk = new DTO_TaiKhoanKH
                        {
                            MaTK = reader["MaTK"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            TenDangNhap = reader["TenDangNhap"].ToString()!,
                            MatKhau = reader["MatKhau"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            TrangThai = reader["TrangThai"].ToString()!
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

        /// Lấy thông tin tài khoản theo mã khách hàng (để kiểm tra xem đã có tài khoản chưa trước khi tạo mới).
        /// Trả về DTO_TaiKhoanKH nếu tìm thấy, ngược lại trả về null.
        public DTO_TaiKhoanKH? DSTheoMaKH(string maKH)
        {
            DTO_TaiKhoanKH? tk = null;
            string sql = "SELECT MaTK, MaKH, TenDangNhap, MatKhau, NgayTao, TrangThai FROM TaiKhoanKH WHERE MaKH = @MaKH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = maKH });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tk = new DTO_TaiKhoanKH
                        {
                            MaTK = reader["MaTK"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            TenDangNhap = reader["TenDangNhap"].ToString()!,
                            MatKhau = reader["MatKhau"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            TrangThai = reader["TrangThai"].ToString()!
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

        /// Thêm tài khoản khách hàng mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemTaiKhoanKH(DTO_TaiKhoanKH tk)
        {
            string sql = "INSERT INTO TaiKhoanKH (MaTK, MaKH, TenDangNhap, MatKhau, NgayTao, TrangThai) " +
                         "VALUES (@MaTK, @MaKH, @TenDangNhap, @MatKhau, @NgayTao, @TrangThai)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = tk.MaTK });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = tk.MaKH });
            cmd.Parameters.Add(new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tk.TenDangNhap });
            cmd.Parameters.Add(new SqlParameter("@MatKhau", SqlDbType.VarChar, 255) { Value = tk.MatKhau });
            cmd.Parameters.Add(new SqlParameter("@NgayTao", SqlDbType.Date) { Value = tk.NgayTao == default(DateTime) ? DateTime.Now : tk.NgayTao });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = tk.TrangThai });

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

        /// Cập nhật thông tin tài khoản khách hàng.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTaiKhoanKH(DTO_TaiKhoanKH tk)
        {
            string sql = "UPDATE TaiKhoanKH SET MaKH = @MaKH, TenDangNhap = @TenDangNhap, MatKhau = @MatKhau, " +
                         "TrangThai = @TrangThai WHERE MaTK = @MaTK";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = tk.MaTK });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = tk.MaKH });
            cmd.Parameters.Add(new SqlParameter("@TenDangNhap", SqlDbType.VarChar, 50) { Value = tk.TenDangNhap });
            cmd.Parameters.Add(new SqlParameter("@MatKhau", SqlDbType.VarChar, 255) { Value = tk.MatKhau });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = tk.TrangThai });

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

        /// Lấy mã tài khoản khách hàng lớn nhất hiện có trong bảng TaiKhoanKH (dùng để sinh mã tự động tăng tiến ở BUS).
        /// Trả về chuỗi mã lớn nhất (ví dụ "TKKH000006"), hoặc null nếu bảng còn rỗng.
        public string? LayMaTKKHMoiNhat()
        {
            string sql = "SELECT MAX(MaTK) FROM TaiKhoanKH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            try
            {
                _conn.Open();
                object result = cmd.ExecuteScalar();
                // Kiểm tra nếu kết quả rỗng hoặc là DBNull thì trả về null, ngược lại trả về chuỗi mã lớn nhất
                return (result == null || result == DBNull.Value) ? null : result.ToString()!.Trim();
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
        }

        /// Cập nhật trạng thái tài khoản khách hàng (vô hiệu hóa/mở khóa).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTrangThai(string maTK, string trangThai)
        {
            string sql = "UPDATE TaiKhoanKH SET TrangThai = @TrangThai WHERE MaTK = @MaTK";
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
    }
}

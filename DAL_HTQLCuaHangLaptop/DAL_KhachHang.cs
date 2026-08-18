using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_KhachHang : DBConnect
    {
        /// Lấy toàn bộ danh sách khách hàng chưa bị xóa (IsDeleted = 0).
        /// Trả về DataTable chứa danh sách khách hàng.
        public DataTable DSTatCaKhachHang()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT kh.MaKH, kh.TenKH, kh.Email, kh.SDT, kh.DiaChi, kh.LoaiKH, kh.NgayTao, kh.NgayCapNhat, kh.NguoiTao, kh.IsDeleted, " +
                         "khl.SinhNhat, khl.LaHSSV " +
                         "FROM KhachHang kh " +
                         "LEFT JOIN KhachHangLe khl ON kh.MaKH = khl.MaKHLe";
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

        /// Lấy thông tin khách hàng theo mã khách hàng (chỉ tìm khách hàng chưa bị xóa).
        /// Trả về DTO_KhachHang nếu tìm thấy, ngược lại trả về null.
        public DTO_KhachHang? DSTheoMaKH(string maKH)
        {
            DTO_KhachHang? kh = null;
            string sql = "SELECT MaKH, TenKH, Email, SDT, DiaChi, LoaiKH, NgayTao, NgayCapNhat, NguoiTao, IsDeleted " +
                         "FROM KhachHang WHERE MaKH = @MaKH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = maKH });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        kh = new DTO_KhachHang
                        {
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            TenKH = reader["TenKH"].ToString()!,
                            Email = reader["Email"] == DBNull.Value ? null! : reader["Email"].ToString()!,
                            SDT = reader["SDT"] == DBNull.Value ? null! : reader["SDT"].ToString()!,
                            DiaChi = reader["DiaChi"] == DBNull.Value ? null! : reader["DiaChi"].ToString()!,
                            LoaiKH = reader["LoaiKH"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"]),
                            NguoiTao = reader["NguoiTao"] == DBNull.Value ? null! : reader["NguoiTao"].ToString()!.Trim(),
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
            return kh;
        }

        /// Lấy danh sách khách hàng theo loại khách hàng (Lẻ / Sỉ, chỉ tìm khách hàng chưa bị xóa).
        /// Trả về DataTable chứa danh sách khách hàng.
        public DataTable DSTheoLoaiKH(string loaiKH)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaKH, TenKH, Email, SDT, DiaChi, LoaiKH, NgayTao, NgayCapNhat, NguoiTao, IsDeleted " +
                         "FROM KhachHang WHERE LoaiKH = @LoaiKH AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@LoaiKH", SqlDbType.NVarChar, 10) { Value = loaiKH });

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

        /// Thêm khách hàng mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemKhachHang(DTO_KhachHang kh)
        {
            string sql = "INSERT INTO KhachHang (MaKH, TenKH, Email, SDT, DiaChi, LoaiKH, NgayTao, NgayCapNhat, NguoiTao, IsDeleted) " +
                         "VALUES (@MaKH, @TenKH, @Email, @SDT, @DiaChi, @LoaiKH, @NgayTao, @NgayCapNhat, @NguoiTao, 0)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = kh.MaKH });
            cmd.Parameters.Add(new SqlParameter("@TenKH", SqlDbType.NVarChar, 50) { Value = kh.TenKH });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = string.IsNullOrEmpty(kh.Email) ? DBNull.Value : kh.Email });
            cmd.Parameters.Add(new SqlParameter("@SDT", SqlDbType.VarChar, 10) { Value = string.IsNullOrEmpty(kh.SDT) ? DBNull.Value : kh.SDT });
            cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 200) { Value = string.IsNullOrEmpty(kh.DiaChi) ? DBNull.Value : kh.DiaChi });
            cmd.Parameters.Add(new SqlParameter("@LoaiKH", SqlDbType.NVarChar, 10) { Value = kh.LoaiKH });
            cmd.Parameters.Add(new SqlParameter("@NgayTao", SqlDbType.DateTime) { Value = kh.NgayTao == default(DateTime) ? DateTime.Now : kh.NgayTao });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = kh.NgayCapNhat.HasValue ? (object)kh.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiTao", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(kh.NguoiTao) ? DBNull.Value : kh.NguoiTao });

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

        /// Cập nhật thông tin khách hàng.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatKhachHang(DTO_KhachHang kh)
        {
            string sql = "UPDATE KhachHang SET TenKH = @TenKH, Email = @Email, SDT = @SDT, DiaChi = @DiaChi, " +
                         "LoaiKH = @LoaiKH, NgayCapNhat = @NgayCapNhat, IsDeleted = @IsDeleted WHERE MaKH = @MaKH";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = kh.MaKH });
            cmd.Parameters.Add(new SqlParameter("@TenKH", SqlDbType.NVarChar, 50) { Value = kh.TenKH });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = string.IsNullOrEmpty(kh.Email) ? DBNull.Value : kh.Email });
            cmd.Parameters.Add(new SqlParameter("@SDT", SqlDbType.VarChar, 10) { Value = string.IsNullOrEmpty(kh.SDT) ? DBNull.Value : kh.SDT });
            cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 200) { Value = string.IsNullOrEmpty(kh.DiaChi) ? DBNull.Value : kh.DiaChi });
            cmd.Parameters.Add(new SqlParameter("@LoaiKH", SqlDbType.NVarChar, 10) { Value = kh.LoaiKH });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = kh.NgayCapNhat.HasValue ? (object)kh.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = kh.IsDeleted });

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

        /// Lấy mã khách hàng lᮻ (prefix KH) lớn nhất hiện có trong bảng KhachHang.
        /// Dùng để sinh mã KH tự động tăng tiến ở BUS (KH00000001, KH00000002, ...).
        /// Trả về chuỗi mã lớn nhất (ví dụ "KH00000006"), hoặc null nếu chưa có khách hàng lᮻ nào.
        public string? LayMaKHLeMoiNhat()
        {
            // Lọc theo prefix KH để không lấy nhầm mã DN của khách hàng sỉ
            string sql = "SELECT MAX(MaKH) FROM KhachHang WHERE MaKH LIKE 'KH%'";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            try
            {
                _conn.Open();
                object result = cmd.ExecuteScalar();
                return (result == null || result == DBNull.Value) ? null : result.ToString()!.Trim();
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
        }

        /// Lấy mã khách hàng sỉ (prefix DN) lớn nhất hiện có trong bảng KhachHang.
        /// Dùng để sinh mã DN tự động tăng tiến ở BUS (DN00000001, DN00000002, ...).
        /// Trả về chuỗi mã lớn nhất (ví dụ "DN00000009"), hoặc null nếu chưa có khách hàng sỉ nào.
        public string? LayMaKHSiMoiNhat()
        {
            // Lọc theo prefix DN để không lấy nhầm mã KH của khách hàng lᮻ
            string sql = "SELECT MAX(MaKH) FROM KhachHang WHERE MaKH LIKE 'DN%'";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            try
            {
                _conn.Open();
                object result = cmd.ExecuteScalar();
                return (result == null || result == DBNull.Value) ? null : result.ToString()!.Trim();
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
        }

        /// Xóa mềm khách hàng bằng cách cập nhật IsDeleted = 1.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaMemKhachHang(string maKH)
        {
            string sql = "UPDATE KhachHang SET IsDeleted = 1 WHERE MaKH = @MaKH AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = maKH });

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

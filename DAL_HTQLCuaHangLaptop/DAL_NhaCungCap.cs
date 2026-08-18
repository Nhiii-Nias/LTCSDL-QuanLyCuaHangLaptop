using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_NhaCungCap : DBConnect
    {
        /// Lấy toàn bộ danh sách nhà cung cấp chưa bị xóa (IsDeleted = 0).
        /// Trả về DataTable chứa danh sách nhà cung cấp.
        public DataTable DSTatCaNCC()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaNCC, TenNCC, Email, SDT, DiaChi, NgayCapNhat, IsDeleted " +
                         "FROM NhaCungCap";
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

        /// Lấy thông tin nhà cung cấp theo mã (chỉ tìm nhà cung cấp chưa bị xóa).
        /// Trả về DTO_NhaCungCap nếu tìm thấy, ngược lại trả về null.
        public DTO_NhaCungCap? DSTheoMaNCC(string maNCC)
        {
            DTO_NhaCungCap? ncc = null;
            string sql = "SELECT MaNCC, TenNCC, Email, SDT, DiaChi, NgayCapNhat, IsDeleted " +
                         "FROM NhaCungCap WHERE MaNCC = @MaNCC";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Char, 10) { Value = maNCC });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ncc = new DTO_NhaCungCap
                        {
                            // Đảm bảo kiểm tra DBNull.Value khi đọc từ SqlDataReader
                            MaNCC = reader["MaNCC"].ToString()!.Trim(),
                            TenNCC = reader["TenNCC"].ToString()!,
                            Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString()!,
                            SDT = reader["SDT"] == DBNull.Value ? null : reader["SDT"].ToString()!,
                            DiaChi = reader["DiaChi"] == DBNull.Value ? null : reader["DiaChi"].ToString()!,
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
            return ncc;
        }

        /// Thêm một nhà cung cấp mới vào cơ sở dữ liệu.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemNCC(DTO_NhaCungCap ncc)
        {
            string sql = "INSERT INTO NhaCungCap (MaNCC, TenNCC, Email, SDT, DiaChi, NgayCapNhat, IsDeleted) " +
                         "VALUES (@MaNCC, @TenNCC, @Email, @SDT, @DiaChi, @NgayCapNhat, 0)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Char, 10) { Value = ncc.MaNCC });
            cmd.Parameters.Add(new SqlParameter("@TenNCC", SqlDbType.NVarChar, 200) { Value = ncc.TenNCC });
            
            // Đảm bảo kiểm tra DBNull.Value khi nạp vào SqlParameter
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 150) { Value = string.IsNullOrEmpty(ncc.Email) ? DBNull.Value : ncc.Email });
            cmd.Parameters.Add(new SqlParameter("@SDT", SqlDbType.VarChar, 10) { Value = string.IsNullOrEmpty(ncc.SDT) ? DBNull.Value : ncc.SDT });
            cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 300) { Value = string.IsNullOrEmpty(ncc.DiaChi) ? DBNull.Value : ncc.DiaChi });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = ncc.NgayCapNhat.HasValue ? (object)ncc.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật thông tin nhà cung cấp (chỉ cập nhật nhà cung cấp chưa bị xóa).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatNCC(DTO_NhaCungCap ncc)
        {
            string sql = "UPDATE NhaCungCap SET TenNCC = @TenNCC, Email = @Email, SDT = @SDT, DiaChi = @DiaChi, NgayCapNhat = @NgayCapNhat, IsDeleted = @IsDeleted " +
                         "WHERE MaNCC = @MaNCC";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Char, 10) { Value = ncc.MaNCC });
            cmd.Parameters.Add(new SqlParameter("@TenNCC", SqlDbType.NVarChar, 200) { Value = ncc.TenNCC });
            
            // Đảm bảo kiểm tra DBNull.Value khi nạp vào SqlParameter
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 150) { Value = string.IsNullOrEmpty(ncc.Email) ? DBNull.Value : ncc.Email });
            cmd.Parameters.Add(new SqlParameter("@SDT", SqlDbType.VarChar, 10) { Value = string.IsNullOrEmpty(ncc.SDT) ? DBNull.Value : ncc.SDT });
            cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 300) { Value = string.IsNullOrEmpty(ncc.DiaChi) ? DBNull.Value : ncc.DiaChi });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = ncc.NgayCapNhat.HasValue ? (object)ncc.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = ncc.IsDeleted });

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

        /// Xóa mềm nhà cung cấp bằng cách cập nhật cột IsDeleted = 1.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaMemNCC(string maNCC)
        {
            string sql = "UPDATE NhaCungCap SET IsDeleted = 1 WHERE MaNCC = @MaNCC AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Char, 10) { Value = maNCC });

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

using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_NhanVien : DBConnect
    {
        /// Lấy toàn bộ danh sách nhân viên chưa bị xóa (IsDeleted = 0).
        /// Trả về DataTable chứa danh sách nhân viên.
        public DataTable DSTatCaNhanVien()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaNV, TenNV, GioiTinh, SinhNhat, SDT, DiaChi, Email, NgayVaoLam, Luong, ChucVu, NgayTao, NgayCapNhat, NguoiTao, NguoiCapNhat, IsDeleted " +
                         "FROM NhanVien WHERE IsDeleted = 0";
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

        /// Lấy thông tin nhân viên theo mã nhân viên (chỉ tìm nhân viên chưa bị xóa).
        /// Trả về DTO_NhanVien nếu tìm thấy, ngược lại trả về null.
        public DTO_NhanVien? DSTheoMaNV(string maNV)
        {
            DTO_NhanVien? nv = null;
            string sql = "SELECT MaNV, TenNV, GioiTinh, SinhNhat, SDT, DiaChi, Email, NgayVaoLam, Luong, ChucVu, NgayTao, NgayCapNhat, NguoiTao, NguoiCapNhat, IsDeleted " +
                         "FROM NhanVien WHERE MaNV = @MaNV AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = maNV });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nv = new DTO_NhanVien
                        {
                            MaNV = reader["MaNV"].ToString()!.Trim(),
                            TenNV = reader["TenNV"].ToString()!,
                            GioiTinh = reader["GioiTinh"] == DBNull.Value ? null! : reader["GioiTinh"].ToString()!,
                            SinhNhat = Convert.ToDateTime(reader["SinhNhat"]),
                            SDT = reader["SDT"].ToString()!,
                            DiaChi = reader["DiaChi"].ToString()!,
                            Email = reader["Email"] == DBNull.Value ? null! : reader["Email"].ToString()!,
                            NgayVaoLam = Convert.ToDateTime(reader["NgayVaoLam"]),
                            Luong = Convert.ToDecimal(reader["Luong"]),
                            ChucVu = reader["ChucVu"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"]),
                            NguoiTao = reader["NguoiTao"] == DBNull.Value ? null! : reader["NguoiTao"].ToString()!.Trim(),
                            NguoiCapNhat = reader["NguoiCapNhat"] == DBNull.Value ? null! : reader["NguoiCapNhat"].ToString()!.Trim(),
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
            return nv;
        }

        /// Thêm một nhân viên mới vào cơ sở dữ liệu.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemNhanVien(DTO_NhanVien nv)
        {
            string sql = "INSERT INTO NhanVien (MaNV, TenNV, GioiTinh, SinhNhat, SDT, DiaChi, Email, NgayVaoLam, Luong, ChucVu, NgayTao, NgayCapNhat, NguoiTao, NguoiCapNhat, IsDeleted) " +
                         "VALUES (@MaNV, @TenNV, @GioiTinh, @SinhNhat, @SDT, @DiaChi, @Email, @NgayVaoLam, @Luong, @ChucVu, @NgayTao, @NgayCapNhat, @NguoiTao, @NguoiCapNhat, 0)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = nv.MaNV });
            cmd.Parameters.Add(new SqlParameter("@TenNV", SqlDbType.NVarChar, 50) { Value = nv.TenNV });
            cmd.Parameters.Add(new SqlParameter("@GioiTinh", SqlDbType.NVarChar, 10) { Value = string.IsNullOrEmpty(nv.GioiTinh) ? DBNull.Value : nv.GioiTinh });
            cmd.Parameters.Add(new SqlParameter("@SinhNhat", SqlDbType.Date) { Value = nv.SinhNhat });
            cmd.Parameters.Add(new SqlParameter("@SDT", SqlDbType.VarChar, 10) { Value = nv.SDT });
            cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 300) { Value = nv.DiaChi });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = string.IsNullOrEmpty(nv.Email) ? DBNull.Value : nv.Email });
            cmd.Parameters.Add(new SqlParameter("@NgayVaoLam", SqlDbType.Date) { Value = nv.NgayVaoLam });
            cmd.Parameters.Add(new SqlParameter("@Luong", SqlDbType.Decimal) { Value = nv.Luong });
            cmd.Parameters.Add(new SqlParameter("@ChucVu", SqlDbType.NVarChar, 100) { Value = nv.ChucVu });
            cmd.Parameters.Add(new SqlParameter("@NgayTao", SqlDbType.DateTime) { Value = nv.NgayTao == default(DateTime) ? DateTime.Now : nv.NgayTao });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = nv.NgayCapNhat.HasValue ? (object)nv.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiTao", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(nv.NguoiTao) ? DBNull.Value : nv.NguoiTao });
            cmd.Parameters.Add(new SqlParameter("@NguoiCapNhat", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(nv.NguoiCapNhat) ? DBNull.Value : nv.NguoiCapNhat });

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

        /// Cập nhật thông tin nhân viên (chỉ cập nhật nhân viên chưa bị xóa).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatNhanVien(DTO_NhanVien nv)
        {
            string sql = "UPDATE NhanVien SET TenNV = @TenNV, GioiTinh = @GioiTinh, SinhNhat = @SinhNhat, SDT = @SDT, " +
                         "DiaChi = @DiaChi, Email = @Email, NgayVaoLam = @NgayVaoLam, Luong = @Luong, ChucVu = @ChucVu, " +
                         "NgayCapNhat = @NgayCapNhat, NguoiCapNhat = @NguoiCapNhat " +
                         "WHERE MaNV = @MaNV AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = nv.MaNV });
            cmd.Parameters.Add(new SqlParameter("@TenNV", SqlDbType.NVarChar, 50) { Value = nv.TenNV });
            cmd.Parameters.Add(new SqlParameter("@GioiTinh", SqlDbType.NVarChar, 10) { Value = string.IsNullOrEmpty(nv.GioiTinh) ? DBNull.Value : nv.GioiTinh });
            cmd.Parameters.Add(new SqlParameter("@SinhNhat", SqlDbType.Date) { Value = nv.SinhNhat });
            cmd.Parameters.Add(new SqlParameter("@SDT", SqlDbType.VarChar, 10) { Value = nv.SDT });
            cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar, 300) { Value = nv.DiaChi });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100) { Value = string.IsNullOrEmpty(nv.Email) ? DBNull.Value : nv.Email });
            cmd.Parameters.Add(new SqlParameter("@NgayVaoLam", SqlDbType.Date) { Value = nv.NgayVaoLam });
            cmd.Parameters.Add(new SqlParameter("@Luong", SqlDbType.Decimal) { Value = nv.Luong });
            cmd.Parameters.Add(new SqlParameter("@ChucVu", SqlDbType.NVarChar, 100) { Value = nv.ChucVu });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = nv.NgayCapNhat.HasValue ? (object)nv.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiCapNhat", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(nv.NguoiCapNhat) ? DBNull.Value : nv.NguoiCapNhat });

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

        /// Xóa mềm nhân viên bằng cách cập nhật IsDeleted = 1.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaMemNhanVien(string maNV)
        {
            string sql = "UPDATE NhanVien SET IsDeleted = 1 WHERE MaNV = @MaNV AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = maNV });

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

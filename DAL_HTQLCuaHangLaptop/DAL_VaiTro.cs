using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_VaiTro : DBConnect
    {
        /// Lấy toàn bộ danh sách vai trò.
        /// Trả về DataTable chứa danh sách vai trò.
        public DataTable DSTatCaVaiTro()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaVaiTro, TenVaiTro, MoTaQuyen FROM VaiTro";
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

        /// Lấy thông tin vai trò theo mã vai trò.
        /// Trả về DTO_VaiTro nếu tìm thấy, ngược lại trả về null.
        public DTO_VaiTro? DSTheoMaVaiTro(string maVaiTro)
        {
            DTO_VaiTro? vt = null;
            string sql = "SELECT MaVaiTro, TenVaiTro, MoTaQuyen FROM VaiTro WHERE MaVaiTro = @MaVaiTro";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaVaiTro", SqlDbType.Char, 10) { Value = maVaiTro });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        vt = new DTO_VaiTro
                        {
                            MaVaiTro = reader["MaVaiTro"].ToString()!.Trim(),
                            TenVaiTro = reader["TenVaiTro"].ToString()!,
                            MoTaQuyen = reader["MoTaQuyen"] == DBNull.Value ? null : reader["MoTaQuyen"].ToString()!
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
            return vt;
        }

        /// Thêm một vai trò mới vào cơ sở dữ liệu.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemVaiTro(DTO_VaiTro vt)
        {
            string sql = "INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTaQuyen) VALUES (@MaVaiTro, @TenVaiTro, @MoTaQuyen)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaVaiTro", SqlDbType.Char, 10) { Value = vt.MaVaiTro });
            cmd.Parameters.Add(new SqlParameter("@TenVaiTro", SqlDbType.NVarChar, 50) { Value = vt.TenVaiTro });
            cmd.Parameters.Add(new SqlParameter("@MoTaQuyen", SqlDbType.NVarChar, 500) { Value = string.IsNullOrEmpty(vt.MoTaQuyen) ? DBNull.Value : vt.MoTaQuyen });

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

        /// Cập nhật thông tin vai trò.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatVaiTro(DTO_VaiTro vt)
        {
            string sql = "UPDATE VaiTro SET TenVaiTro = @TenVaiTro, MoTaQuyen = @MoTaQuyen WHERE MaVaiTro = @MaVaiTro";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaVaiTro", SqlDbType.Char, 10) { Value = vt.MaVaiTro });
            cmd.Parameters.Add(new SqlParameter("@TenVaiTro", SqlDbType.NVarChar, 50) { Value = vt.TenVaiTro });
            cmd.Parameters.Add(new SqlParameter("@MoTaQuyen", SqlDbType.NVarChar, 500) { Value = string.IsNullOrEmpty(vt.MoTaQuyen) ? DBNull.Value : vt.MoTaQuyen });

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

        /// Xóa vật lý vai trò khỏi cơ sở dữ liệu.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaVaiTro(string maVaiTro)
        {
            string sql = "DELETE FROM VaiTro WHERE MaVaiTro = @MaVaiTro";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaVaiTro", SqlDbType.Char, 10) { Value = maVaiTro });

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

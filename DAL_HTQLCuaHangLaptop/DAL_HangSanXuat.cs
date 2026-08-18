using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_HangSanXuat : DBConnect
    {
        /// Lấy toàn bộ danh sách hãng sản xuất chưa bị xóa (IsDeleted = 0).
        /// Trả về DataTable chứa danh sách hãng sản xuất.
        public DataTable DSTatCaHSX()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaHang, TenHang, QuocGia, IsDeleted FROM HangSanXuat";
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

        /// Lấy thông tin hãng sản xuất theo mã hãng (chỉ tìm hãng chưa bị xóa).
        /// Trả về DTO_HangSanXuat nếu tìm thấy, ngược lại trả về null.
        public DTO_HangSanXuat DSTheoMaHSX(string maHang)
        {
            DTO_HangSanXuat hsx = null;
            string sql = "SELECT MaHang, TenHang, QuocGia, IsDeleted FROM HangSanXuat WHERE MaHang = @MaHang";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaHang", SqlDbType.Char, 10) { Value = maHang });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hsx = new DTO_HangSanXuat
                        {
                            MaHang = reader["MaHang"].ToString().Trim(),
                            TenHang = reader["TenHang"].ToString(),
                            QuocGia = reader["QuocGia"] == DBNull.Value ? null : reader["QuocGia"].ToString(),
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
            return hsx;
        }

        /// Thêm một hãng sản xuất mới vào cơ sở dữ liệu.
        /// Trả về true nếu thêm thành công, ngược lại false.
        public bool ThemHSX(DTO_HangSanXuat hsx)
        {
            string sql = "INSERT INTO HangSanXuat (MaHang, TenHang, QuocGia, IsDeleted) VALUES (@MaHang, @TenHang, @QuocGia, 0)";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            
            cmd.Parameters.Add(new SqlParameter("@MaHang", SqlDbType.Char, 10) { Value = hsx.MaHang });
            cmd.Parameters.Add(new SqlParameter("@TenHang", SqlDbType.NVarChar, 100) { Value = hsx.TenHang });
            cmd.Parameters.Add(new SqlParameter("@QuocGia", SqlDbType.NVarChar, 100) { Value = (object)hsx.QuocGia ?? DBNull.Value });

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

        
        /// Cập nhật thông tin hãng sản xuất (chỉ cập nhật hãng chưa bị xóa).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool UpdateHangSanXuat(DTO_HangSanXuat hsx)
        {
            string sql = "UPDATE HangSanXuat SET TenHang = @TenHang, QuocGia = @QuocGia, IsDeleted = @IsDeleted WHERE MaHang = @MaHang";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaHang", SqlDbType.Char, 10) { Value = hsx.MaHang });
            cmd.Parameters.Add(new SqlParameter("@TenHang", SqlDbType.NVarChar, 100) { Value = hsx.TenHang });
            cmd.Parameters.Add(new SqlParameter("@QuocGia", SqlDbType.NVarChar, 100) { Value = (object)hsx.QuocGia ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = hsx.IsDeleted });

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
        
        /// Xóa mềm hãng sản xuất bằng cách cập nhật cột IsDeleted = 1.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaMemHSX(string maHang)
        {
            string sql = "UPDATE HangSanXuat SET IsDeleted = 1 WHERE MaHang = @MaHang AND IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaHang", SqlDbType.Char, 10) { Value = maHang });

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

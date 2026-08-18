using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_CauHinh : DBConnect
    {
        /// Lấy toàn bộ danh sách cấu hình.
        /// Trả về DataTable chứa danh sách cấu hình.
        public DataTable DSTatCaCauHinh()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaCauHinh, MaLoaiSP, TenThuocTinh FROM CauHinh";
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

        /// Lấy thông tin cấu hình theo mã cấu hình.
        /// Trả về DTO_CauHinh nếu tìm thấy, ngược lại trả về null.
        public DTO_CauHinh? DSTheoMaCauHinh(string maCauHinh)
        {
            DTO_CauHinh? ch = null;
            string sql = "SELECT MaCauHinh, MaLoaiSP, TenThuocTinh FROM CauHinh WHERE MaCauHinh = @MaCauHinh";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaCauHinh", SqlDbType.Char, 10) { Value = maCauHinh });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ch = new DTO_CauHinh
                        {
                            // Đảm bảo kiểm tra DBNull.Value khi đọc từ SqlDataReader
                            MaCauHinh = reader["MaCauHinh"] == DBNull.Value ? string.Empty : reader["MaCauHinh"].ToString()!.Trim(),
                            MaLoaiSP = reader["MaLoaiSP"] == DBNull.Value ? string.Empty : reader["MaLoaiSP"].ToString()!.Trim(),
                            TenThuocTinh = reader["TenThuocTinh"] == DBNull.Value ? string.Empty : reader["TenThuocTinh"].ToString()!
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
            return ch;
        }

        /// Thêm một cấu hình mới vào cơ sở dữ liệu.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemCauHinh(DTO_CauHinh ch)
        {
            string sql = "INSERT INTO CauHinh (MaCauHinh, MaLoaiSP, TenThuocTinh) VALUES (@MaCauHinh, @MaLoaiSP, @TenThuocTinh)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaCauHinh", SqlDbType.Char, 10) { Value = ch.MaCauHinh });
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = ch.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@TenThuocTinh", SqlDbType.NVarChar, 150) { Value = ch.TenThuocTinh });

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

        /// Cập nhật thông tin cấu hình.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatCauHinh(DTO_CauHinh ch)
        {
            string sql = "UPDATE CauHinh SET MaLoaiSP = @MaLoaiSP, TenThuocTinh = @TenThuocTinh WHERE MaCauHinh = @MaCauHinh";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaCauHinh", SqlDbType.Char, 10) { Value = ch.MaCauHinh });
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = ch.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@TenThuocTinh", SqlDbType.NVarChar, 150) { Value = ch.TenThuocTinh });

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

        /// Xóa vật lý cấu hình khỏi cơ sở dữ liệu.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaCauHinh(string maCauHinh)
        {
            string sql = "DELETE FROM CauHinh WHERE MaCauHinh = @MaCauHinh";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaCauHinh", SqlDbType.Char, 10) { Value = maCauHinh });

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

        /// Lấy danh sách cấu hình theo loại sản phẩm.
        /// Trả về DataTable chứa toàn bộ cấu hình của loại sản phẩm đó.
        public DataTable DSCauHinhTheoLoaiSP(string maLoaiSP)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaCauHinh, MaLoaiSP, TenThuocTinh FROM CauHinh WHERE MaLoaiSP = @MaLoaiSP";
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

    }
}

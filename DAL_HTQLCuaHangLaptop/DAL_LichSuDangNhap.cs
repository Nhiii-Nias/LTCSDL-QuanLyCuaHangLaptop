using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_LichSuDangNhap : DBConnect
    {
        /// Lấy toàn bộ danh sách lịch sử đăng nhập hệ thống.
        /// Trả về DataTable chứa danh sách lịch sử đăng nhập.
        public DataTable DSTatCaLichSuDangNhap()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaLSDN, MaTK, ThoiGian, DiaChiIP, TrangThai FROM LichSuDangNhap";
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

        /// Lấy danh sách lịch sử đăng nhập của một tài khoản nhân viên.
        /// Trả về DataTable chứa các bản ghi lịch sử đăng nhập của tài khoản đó.
        public DataTable DSTheoMaTK(string maTK)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaLSDN, MaTK, ThoiGian, DiaChiIP, TrangThai FROM LichSuDangNhap WHERE MaTK = @MaTK";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = maTK });

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

        /// Thêm bản ghi lịch sử đăng nhập mới (ThoiGian dùng GETDATE() mặc định từ database).
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemLichSuDangNhap(DTO_LichSuDangNhap ls)
        {
            string sql = "INSERT INTO LichSuDangNhap (MaLSDN, MaTK, DiaChiIP, TrangThai) VALUES (@MaLSDN, @MaTK, @DiaChiIP, @TrangThai)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaLSDN", SqlDbType.Char, 10) { Value = ls.MaLSDN });
            cmd.Parameters.Add(new SqlParameter("@MaTK", SqlDbType.Char, 10) { Value = ls.MaTK });
            cmd.Parameters.Add(new SqlParameter("@DiaChiIP", SqlDbType.VarChar, 45) { Value = string.IsNullOrEmpty(ls.DiaChiIP) ? DBNull.Value : ls.DiaChiIP });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = ls.TrangThai });

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

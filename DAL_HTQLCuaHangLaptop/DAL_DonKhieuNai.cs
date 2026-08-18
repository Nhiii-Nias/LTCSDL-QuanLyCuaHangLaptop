using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_DonKhieuNai : DBConnect
    {
        /// Lấy toàn bộ danh sách đơn khiếu nại.
        public DataTable DSTatCaDonKhieuNai()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaDonKN, MaDH, MaKH, NoiDung, NgayGui, TrangThai, KetQua, NgayTao FROM DonKhieuNai";
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

        /// Lấy đơn khiếu nại theo mã đơn.
        public DTO_DonKhieuNai? DSTheoMaDonKhieuNai(string maDonKN)
        {
            DTO_DonKhieuNai? dkn = null;
            string sql = "SELECT MaDonKN, MaDH, MaKH, NoiDung, NgayGui, TrangThai, KetQua, NgayTao FROM DonKhieuNai WHERE MaDonKN = @MaDonKN";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaDonKN", SqlDbType.Char, 10) { Value = maDonKN });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dkn = new DTO_DonKhieuNai
                        {
                            MaDonKN = reader["MaDonKN"].ToString()!.Trim(),
                            MaDH = reader["MaDH"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            NoiDung = reader["NoiDung"].ToString()!,
                            NgayGui = Convert.ToDateTime(reader["NgayGui"]),
                            TrangThai = reader["TrangThai"].ToString()!,
                            KetQua = reader["KetQua"] == DBNull.Value ? null : reader["KetQua"].ToString(),
                            NgayTao = Convert.ToDateTime(reader["NgayTao"])
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
            return dkn;
        }

        /// Thêm một đơn khiếu nại mới.
        /// NgayGui và NgayTao dùng DEFAULT của database.
        public bool ThemDonKhieuNai(DTO_DonKhieuNai dkn)
        {
            string sql = "INSERT INTO DonKhieuNai (MaDonKN, MaDH, MaKH, NoiDung, TrangThai, KetQua) VALUES (@MaDonKN, @MaDH, @MaKH, @NoiDung, @TrangThai, @KetQua)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaDonKN", SqlDbType.Char, 10) { Value = dkn.MaDonKN });
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = dkn.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = dkn.MaKH });
            cmd.Parameters.Add(new SqlParameter("@NoiDung", SqlDbType.NVarChar, 1000) { Value = dkn.NoiDung });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = dkn.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@KetQua", SqlDbType.NVarChar, 500) { Value = (object)dkn.KetQua ?? DBNull.Value });

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

        /// Cập nhật thông tin đơn khiếu nại.
        public bool CapNhatDonKhieuNai(DTO_DonKhieuNai dkn)
        {
            string sql = "UPDATE DonKhieuNai SET MaDH = @MaDH, MaKH = @MaKH, NoiDung = @NoiDung, TrangThai = @TrangThai, KetQua = @KetQua WHERE MaDonKN = @MaDonKN";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaDonKN", SqlDbType.Char, 10) { Value = dkn.MaDonKN });
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = dkn.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = dkn.MaKH });
            cmd.Parameters.Add(new SqlParameter("@NoiDung", SqlDbType.NVarChar, 1000) { Value = dkn.NoiDung });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = dkn.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@KetQua", SqlDbType.NVarChar, 500) { Value = (object)dkn.KetQua ?? DBNull.Value });

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

        /// Cập nhật trạng thái đơn khiếu nại.
        public bool CapNhatTrangThai(string maDonKN, string trangThai)
        {
            string sql = "UPDATE DonKhieuNai SET TrangThai = @TrangThai WHERE MaDonKN = @MaDonKN";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaDonKN", SqlDbType.Char, 10) { Value = maDonKN });
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

        /// Cập nhật kết quả đơn khiếu nại.
        public bool CapNhatKetQua(string maDonKN, string ketQua)
        {
            string sql = "UPDATE DonKhieuNai SET KetQua = @KetQua WHERE MaDonKN = @MaDonKN";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaDonKN", SqlDbType.Char, 10) { Value = maDonKN });
            cmd.Parameters.Add(new SqlParameter("@KetQua", SqlDbType.NVarChar, 500) { Value = (object)ketQua ?? DBNull.Value });

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

        /// Lấy toàn bộ khiếu nại liên quan đến một serial sản phẩm cụ thể.
        public DataTable DSTheoMaSerial(string maSerial)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT dkn.MaDonKN, dkn.MaDH, dkn.MaKH, dkn.NoiDung, dkn.NgayGui, dkn.TrangThai, dkn.KetQua, dkn.NgayTao 
                FROM DonKhieuNai dkn
                JOIN DonHang dh ON dh.MaDH = dkn.MaDH
                JOIN ChiTietDonHang ctdh ON ctdh.MaDH = dh.MaDH
                WHERE ctdh.MaSerialSP = @MaSerialSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });

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

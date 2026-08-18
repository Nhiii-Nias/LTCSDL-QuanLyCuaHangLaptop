using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_DonHang : DBConnect
    {
        /// Lấy danh sách toàn bộ đơn hàng.
        /// Trả về DataTable chứa danh sách đơn hàng.
        public DataTable DSTatCaDonHang()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaDH, MaNV, MaKH, MaKM, MaHD, NgayDat, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai, NgayTao, NgayCapNhat, NguoiTao FROM DonHang";
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

        /// Lấy thông tin chi tiết đơn hàng theo mã đơn.
        /// Trả về DTO_DonHang nếu tìm thấy, ngược lại trả về null.
        public DTO_DonHang? DSTheoMaDH(string maDH)
        {
            DTO_DonHang? dh = null;
            string sql = "SELECT MaDH, MaNV, MaKH, MaKM, MaHD, NgayDat, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai, NgayTao, NgayCapNhat, NguoiTao FROM DonHang WHERE MaDH = @MaDH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dh = new DTO_DonHang
                        {
                            MaDH = reader["MaDH"].ToString()!.Trim(),
                            MaNV = reader["MaNV"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            MaKM = reader["MaKM"] == DBNull.Value ? null! : reader["MaKM"].ToString()!.Trim(),
                            MaHD = reader["MaHD"] == DBNull.Value ? null! : reader["MaHD"].ToString()!.Trim(),
                            NgayDat = Convert.ToDateTime(reader["NgayDat"]),
                            TongTien = Convert.ToDecimal(reader["TongTien"]),
                            TienSauGiam = reader["TienSauGiam"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TienSauGiam"]),
                            PhuongThucThanhToan = reader["PhuongThucThanhToan"].ToString()!,
                            TrangThai = reader["TrangThai"].ToString()!,
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"]),
                            NguoiTao = reader["NguoiTao"] == DBNull.Value ? null! : reader["NguoiTao"].ToString()!.Trim()
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
            return dh;
        }

        /// Lấy danh sách toàn bộ đơn hàng của một khách hàng cụ thể.
        /// Trả về DataTable chứa các đơn hàng.
        public DataTable DSTheoKhachHang(string maKH)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaDH, MaNV, MaKH, MaKM, MaHD, NgayDat, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai, NgayTao, NgayCapNhat, NguoiTao " +
                         "FROM DonHang WHERE MaKH = @MaKH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = maKH });

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

        /// Lấy danh sách đơn hàng lọc theo trạng thái đơn hàng.
        /// Trả về DataTable chứa danh sách đơn hàng.
        public DataTable DSTheoTrangThai(string trangThai)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaDH, MaNV, MaKH, MaKM, MaHD, NgayDat, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai, NgayTao, NgayCapNhat, NguoiTao " +
                         "FROM DonHang WHERE TrangThai = @TrangThai";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = trangThai });

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

        /// Thêm đơn hàng mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemDonHang(DTO_DonHang dh)
        {
            string sql = "INSERT INTO DonHang (MaDH, MaNV, MaKH, MaKM, MaHD, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai, NgayCapNhat, NguoiTao) " +
                         "VALUES (@MaDH, @MaNV, @MaKH, @MaKM, @MaHD, @TongTien, @TienSauGiam, @PhuongThucThanhToan, @TrangThai, @NgayCapNhat, @NguoiTao)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = dh.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = dh.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = dh.MaKH });
            cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(dh.MaKM) ? DBNull.Value : dh.MaKM });
            cmd.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(dh.MaHD) ? DBNull.Value : dh.MaHD });
            cmd.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = dh.TongTien });
            cmd.Parameters.Add(new SqlParameter("@TienSauGiam", SqlDbType.Decimal) { Value = dh.TienSauGiam.HasValue ? (object)dh.TienSauGiam.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@PhuongThucThanhToan", SqlDbType.NVarChar, 100) { Value = dh.PhuongThucThanhToan });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = dh.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = dh.NgayCapNhat.HasValue ? (object)dh.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiTao", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(dh.NguoiTao) ? DBNull.Value : dh.NguoiTao });

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

        /// Cập nhật thông tin đơn hàng.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatDonHang(DTO_DonHang dh)
        {
            string sql = "UPDATE DonHang SET MaNV = @MaNV, MaKH = @MaKH, MaKM = @MaKM, MaHD = @MaHD, " +
                         "TongTien = @TongTien, TienSauGiam = @TienSauGiam, PhuongThucThanhToan = @PhuongThucThanhToan, " +
                         "TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat WHERE MaDH = @MaDH";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = dh.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = dh.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = dh.MaKH });
            cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(dh.MaKM) ? DBNull.Value : dh.MaKM });
            cmd.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(dh.MaHD) ? DBNull.Value : dh.MaHD });
            cmd.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = dh.TongTien });
            cmd.Parameters.Add(new SqlParameter("@TienSauGiam", SqlDbType.Decimal) { Value = dh.TienSauGiam.HasValue ? (object)dh.TienSauGiam.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@PhuongThucThanhToan", SqlDbType.NVarChar, 100) { Value = dh.PhuongThucThanhToan });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = dh.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = dh.NgayCapNhat.HasValue ? (object)dh.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật trạng thái đơn hàng (Chờ Xử Lý / Đang Giao / Hoàn Thành / Huỷ).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTrangThai(string maDH, string trangThai)
        {
            string sql = "UPDATE DonHang SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() WHERE MaDH = @MaDH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });
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

        /// Lấy mã đơn hàng lớn nhất hiện có trong bảng DonHang.
        /// Dùng để sinh mã DH tự động tăng tiến ở BUS.
        public string? LayMaDHMoiNhat()
        {
            string sql = "SELECT MAX(MaDH) FROM DonHang WHERE MaDH LIKE 'DH%'";
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
    }
}

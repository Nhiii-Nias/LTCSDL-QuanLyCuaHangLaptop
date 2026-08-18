using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_HopDong : DBConnect
    {
        /// Lấy danh sách toàn bộ hợp đồng.
        /// Trả về DataTable chứa danh sách hợp đồng.
        public DataTable DSTatCaHopDong()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaHD, MaNV, MaKH, NgayKy, GiaTriHD, NgayHieuLuc, NgayHetHan, TrangThai, NgayTao, NgayCapNhat, NguoiTao FROM HopDong";
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

        /// Lấy thông tin hợp đồng theo mã hợp đồng.
        /// Trả về DTO_HopDong nếu tìm thấy, ngược lại trả về null.
        public DTO_HopDong? DSTheoMaHD(string maHD)
        {
            DTO_HopDong? hd = null;
            string sql = "SELECT MaHD, MaNV, MaKH, NgayKy, GiaTriHD, NgayHieuLuc, NgayHetHan, TrangThai, NgayTao, NgayCapNhat, NguoiTao FROM HopDong WHERE MaHD = @MaHD";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = maHD });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hd = new DTO_HopDong
                        {
                            MaHD = reader["MaHD"].ToString()!.Trim(),
                            MaNV = reader["MaNV"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            NgayKy = Convert.ToDateTime(reader["NgayKy"]),
                            GiaTriHD = Convert.ToDecimal(reader["GiaTriHD"]),
                            NgayHieuLuc = Convert.ToDateTime(reader["NgayHieuLuc"]),
                            NgayHetHan = Convert.ToDateTime(reader["NgayHetHan"]),
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
            return hd;
        }

        /// Lấy danh sách toàn bộ hợp đồng của một khách hàng sỉ.
        /// Trả về DataTable chứa các hợp đồng của khách hàng đó.
        public DataTable DSTheoKhachHang(string maKH)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaHD, MaNV, MaKH, NgayKy, GiaTriHD, NgayHieuLuc, NgayHetHan, TrangThai, NgayTao, NgayCapNhat, NguoiTao " +
                         "FROM HopDong WHERE MaKH = @MaKH";
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

        /// Thêm một hợp đồng mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemHopDong(DTO_HopDong hd)
        {
            string sql = "INSERT INTO HopDong (MaHD, MaNV, MaKH, NgayKy, GiaTriHD, NgayHieuLuc, NgayHetHan, TrangThai, NgayCapNhat, NguoiTao) " +
                         "VALUES (@MaHD, @MaNV, @MaKH, @NgayKy, @GiaTriHD, @NgayHieuLuc, @NgayHetHan, @TrangThai, @NgayCapNhat, @NguoiTao)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = hd.MaHD });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = hd.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = hd.MaKH });
            cmd.Parameters.Add(new SqlParameter("@NgayKy", SqlDbType.Date) { Value = hd.NgayKy });
            cmd.Parameters.Add(new SqlParameter("@GiaTriHD", SqlDbType.Decimal) { Value = hd.GiaTriHD });
            cmd.Parameters.Add(new SqlParameter("@NgayHieuLuc", SqlDbType.Date) { Value = hd.NgayHieuLuc });
            cmd.Parameters.Add(new SqlParameter("@NgayHetHan", SqlDbType.Date) { Value = hd.NgayHetHan });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = hd.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = hd.NgayCapNhat.HasValue ? (object)hd.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiTao", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(hd.NguoiTao) ? DBNull.Value : hd.NguoiTao });

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

        /// Cập nhật thông tin hợp đồng.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatHopDong(DTO_HopDong hd)
        {
            string sql = "UPDATE HopDong SET MaNV = @MaNV, MaKH = @MaKH, NgayKy = @NgayKy, GiaTriHD = @GiaTriHD, " +
                         "NgayHieuLuc = @NgayHieuLuc, NgayHetHan = @NgayHetHan, TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat " +
                         "WHERE MaHD = @MaHD";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = hd.MaHD });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = hd.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = hd.MaKH });
            cmd.Parameters.Add(new SqlParameter("@NgayKy", SqlDbType.Date) { Value = hd.NgayKy });
            cmd.Parameters.Add(new SqlParameter("@GiaTriHD", SqlDbType.Decimal) { Value = hd.GiaTriHD });
            cmd.Parameters.Add(new SqlParameter("@NgayHieuLuc", SqlDbType.Date) { Value = hd.NgayHieuLuc });
            cmd.Parameters.Add(new SqlParameter("@NgayHetHan", SqlDbType.Date) { Value = hd.NgayHetHan });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = hd.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = hd.NgayCapNhat.HasValue ? (object)hd.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật trạng thái hợp đồng (Hiệu Lực / Hết Hạn / Huỷ).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTrangThai(string maHD, string trangThai)
        {
            string sql = "UPDATE HopDong SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() WHERE MaHD = @MaHD";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = maHD });
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

        /// <summary>
        /// Cập nhật trạng thái hợp đồng liên đới đến đơn hàng và kho sản phẩm.
        /// </summary>
        public bool CapNhatTrangThaiHopDongLienQuan(string maHD, string trangThaiMoi)
        {
            if (trangThaiMoi == "Huỷ")
            {
                string sqlHopDong = "UPDATE HopDong SET TrangThai = N'Huỷ', NgayCapNhat = GETDATE() WHERE MaHD = @MaHD";
                string sqlRestoreSP = @"
                    UPDATE SanPham 
                    SET TrangThai = N'Trong Kho', NgayCapNhat = GETDATE() 
                    WHERE MaSerialSP IN (
                        SELECT ct.MaSerialSP 
                        FROM ChiTietDonHang ct 
                        INNER JOIN DonHang dh ON ct.MaDH = dh.MaDH
                        WHERE dh.MaHD = @MaHD AND dh.TrangThai != N'Huỷ'
                    ) AND TrangThai = N'Đã Bán' AND IsDeleted = 0";
                string sqlCancelDH = @"
                    UPDATE DonHang 
                    SET TrangThai = N'Huỷ', NgayCapNhat = GETDATE() 
                    WHERE MaHD = @MaHD AND TrangThai != N'Huỷ'";

                using (SqlCommand cmdHD = new SqlCommand(sqlHopDong, _conn))
                using (SqlCommand cmdSP = new SqlCommand(sqlRestoreSP, _conn))
                using (SqlCommand cmdDH = new SqlCommand(sqlCancelDH, _conn))
                {
                    cmdHD.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = maHD });
                    cmdSP.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = maHD });
                    cmdDH.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = maHD });

                    try
                    {
                        _conn.Open();
                        using (SqlTransaction tran = _conn.BeginTransaction())
                        {
                            cmdHD.Transaction = tran;
                            cmdSP.Transaction = tran;
                            cmdDH.Transaction = tran;
                            try
                            {
                                cmdHD.ExecuteNonQuery();
                                cmdSP.ExecuteNonQuery();
                                cmdDH.ExecuteNonQuery();
                                tran.Commit();
                                return true;
                            }
                            catch
                            {
                                tran.Rollback();
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        if (_conn.State == ConnectionState.Open)
                            _conn.Close();
                    }
                }
            }
            else if (trangThaiMoi == "Hết Hạn")
            {
                string sqlHopDong = "UPDATE HopDong SET TrangThai = N'Hết Hạn', NgayCapNhat = GETDATE() WHERE MaHD = @MaHD";
                string sqlCompleteDH = @"
                    UPDATE DonHang 
                    SET TrangThai = N'Hoàn Thành', NgayCapNhat = GETDATE() 
                    WHERE MaHD = @MaHD AND (TrangThai = N'Chờ Xử Lý' OR TrangThai = N'Đang Giao')";

                using (SqlCommand cmdHD = new SqlCommand(sqlHopDong, _conn))
                using (SqlCommand cmdDH = new SqlCommand(sqlCompleteDH, _conn))
                {
                    cmdHD.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = maHD });
                    cmdDH.Parameters.Add(new SqlParameter("@MaHD", SqlDbType.Char, 10) { Value = maHD });

                    try
                    {
                        _conn.Open();
                        using (SqlTransaction tran = _conn.BeginTransaction())
                        {
                            cmdHD.Transaction = tran;
                            cmdDH.Transaction = tran;
                            try
                            {
                                cmdHD.ExecuteNonQuery();
                                cmdDH.ExecuteNonQuery();
                                tran.Commit();
                                return true;
                            }
                            catch
                            {
                                tran.Rollback();
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        if (_conn.State == ConnectionState.Open)
                            _conn.Close();
                    }
                }
            }
            else
            {
                return CapNhatTrangThai(maHD, trangThaiMoi);
            }
        }
    }
}


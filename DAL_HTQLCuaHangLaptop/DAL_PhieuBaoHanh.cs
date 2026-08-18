using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_PhieuBaoHanh : DBConnect
    {
        /// Lấy toàn bộ danh sách phiếu bảo hành.
        public DataTable DSTatCaPhieuBaoHanh()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPBH, MaDH, MaKH, MaSerialSP, LoaiBH, TrangThai, NgayBatDau, NgayKetThuc, LyDoLoi, KetQua, NgayTao, NgayCapNhat FROM PhieuBaoHanh";
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

        /// Lấy thông tin phiếu bảo hành theo mã phiếu.
        public DTO_PhieuBaoHanh? DSTheoMaPhieuBaoHanh(string maPBH)
        {
            DTO_PhieuBaoHanh? pbh = null;
            string sql = "SELECT MaPBH, MaDH, MaKH, MaSerialSP, LoaiBH, TrangThai, NgayBatDau, NgayKetThuc, LyDoLoi, KetQua, NgayTao, NgayCapNhat FROM PhieuBaoHanh WHERE MaPBH = @MaPBH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaPBH", SqlDbType.Char, 10) { Value = maPBH });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pbh = new DTO_PhieuBaoHanh
                        {
                            MaPBH = reader["MaPBH"].ToString()!.Trim(),
                            MaDH = reader["MaDH"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            MaSerialSP = reader["MaSerialSP"].ToString()!.Trim(),
                            LoaiBH = reader["LoaiBH"].ToString()!,
                            TrangThai = reader["TrangThai"].ToString()!,
                            NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                            NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]),
                            LyDoLoi = reader["LyDoLoi"] == DBNull.Value ? null : reader["LyDoLoi"].ToString(),
                            KetQua = reader["KetQua"] == DBNull.Value ? null : reader["KetQua"].ToString(),
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayCapNhat"])
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
            return pbh;
        }

        public bool ThemPhieuBaoHanh(DTO_PhieuBaoHanh pbh)
        {
            string sql = "INSERT INTO PhieuBaoHanh (MaPBH, MaDH, MaKH, MaSerialSP, LoaiBH, TrangThai, NgayBatDau, NgayKetThuc, LyDoLoi, KetQua, NgayCapNhat) VALUES (@MaPBH, @MaDH, @MaKH, @MaSerialSP, @LoaiBH, @TrangThai, @NgayBatDau, @NgayKetThuc, @LyDoLoi, @KetQua, @NgayCapNhat)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPBH", SqlDbType.Char, 10) { Value = pbh.MaPBH });
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = pbh.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = pbh.MaKH });
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = pbh.MaSerialSP });
            cmd.Parameters.Add(new SqlParameter("@LoaiBH", SqlDbType.NVarChar, 50) { Value = pbh.LoaiBH });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pbh.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayBatDau", SqlDbType.Date) { Value = pbh.NgayBatDau });
            cmd.Parameters.Add(new SqlParameter("@NgayKetThuc", SqlDbType.Date) { Value = pbh.NgayKetThuc });
            cmd.Parameters.Add(new SqlParameter("@LyDoLoi", SqlDbType.NVarChar, 500) { Value = (object)pbh.LyDoLoi ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@KetQua", SqlDbType.NVarChar, 500) { Value = (object)pbh.KetQua ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = pbh.NgayCapNhat.HasValue ? (object)pbh.NgayCapNhat.Value : DBNull.Value });

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

        public bool CapNhatPhieuBaoHanh(DTO_PhieuBaoHanh pbh)
        {
            string sql = "UPDATE PhieuBaoHanh SET MaDH = @MaDH, MaKH = @MaKH, MaSerialSP = @MaSerialSP, LoaiBH = @LoaiBH, TrangThai = @TrangThai, NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, LyDoLoi = @LyDoLoi, KetQua = @KetQua, NgayCapNhat = @NgayCapNhat WHERE MaPBH = @MaPBH";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPBH", SqlDbType.Char, 10) { Value = pbh.MaPBH });
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = pbh.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = pbh.MaKH });
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = pbh.MaSerialSP });
            cmd.Parameters.Add(new SqlParameter("@LoaiBH", SqlDbType.NVarChar, 50) { Value = pbh.LoaiBH });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pbh.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayBatDau", SqlDbType.Date) { Value = pbh.NgayBatDau });
            cmd.Parameters.Add(new SqlParameter("@NgayKetThuc", SqlDbType.Date) { Value = pbh.NgayKetThuc });
            cmd.Parameters.Add(new SqlParameter("@LyDoLoi", SqlDbType.NVarChar, 500) { Value = (object)pbh.LyDoLoi ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@KetQua", SqlDbType.NVarChar, 500) { Value = (object)pbh.KetQua ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = pbh.NgayCapNhat.HasValue ? (object)pbh.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật trạng thái phiếu bảo hành (Đang Xử Lý | Hoàn Thành | Từ Chối) và tự động cập nhật NgayCapNhat, NgayKetThuc.
        public bool CapNhatTrangThai(string maPBH, string trangThai)
        {
            string sql;
            if (trangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) || trangThai.Equals("Từ Chối", StringComparison.OrdinalIgnoreCase))
            {
                sql = "UPDATE PhieuBaoHanh SET TrangThai = @TrangThai, NgayKetThuc = CASE WHEN CAST(NgayBatDau AS DATE) = CAST(GETDATE() AS DATE) THEN DATEADD(day, 1, NgayBatDau) ELSE GETDATE() END, NgayCapNhat = GETDATE() WHERE MaPBH = @MaPBH";
            }
            else
            {
                sql = "UPDATE PhieuBaoHanh SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() WHERE MaPBH = @MaPBH";
            }
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPBH", SqlDbType.Char, 10) { Value = maPBH });
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

        /// Cập nhật kết quả phiếu bảo hành và tự động cập nhật NgayCapNhat.
        public bool CapNhatKetQua(string maPBH, string ketQua)
        {
            string sql = "UPDATE PhieuBaoHanh SET KetQua = @KetQua, NgayCapNhat = GETDATE() WHERE MaPBH = @MaPBH";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPBH", SqlDbType.Char, 10) { Value = maPBH });
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

        /// Cập nhật lý do lỗi phiếu bảo hành và tự động cập nhật NgayCapNhat.
        public bool CapNhatLyDoLoi(string maPBH, string lyDoLoi)
        {
            string sql = "UPDATE PhieuBaoHanh SET LyDoLoi = @LyDoLoi, NgayCapNhat = GETDATE() WHERE MaPBH = @MaPBH";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPBH", SqlDbType.Char, 10) { Value = maPBH });
            cmd.Parameters.Add(new SqlParameter("@LyDoLoi", SqlDbType.NVarChar, 500) { Value = (object)lyDoLoi ?? DBNull.Value });

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

        /// Lấy toàn bộ phiếu bảo hành của một serial sản phẩm cụ thể.
        public DataTable DSTheoMaSerial(string maSerial)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPBH, MaDH, MaKH, MaSerialSP, LoaiBH, TrangThai, NgayBatDau, NgayKetThuc, LyDoLoi, KetQua, NgayTao, NgayCapNhat FROM PhieuBaoHanh WHERE MaSerialSP = @MaSerialSP";
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

        /// Lấy toàn bộ phiếu bảo hành của một khách hàng cụ thể.
        public DataTable DSTheoKhachHang(string maKH)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPBH, MaDH, MaKH, MaSerialSP, LoaiBH, TrangThai, NgayBatDau, NgayKetThuc, LyDoLoi, KetQua, NgayTao, NgayCapNhat FROM PhieuBaoHanh WHERE MaKH = @MaKH";
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
    }
}

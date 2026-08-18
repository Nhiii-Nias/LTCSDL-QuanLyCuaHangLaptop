using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_PhieuDoiTra : DBConnect
    {
        /// Lấy toàn bộ danh sách phiếu đổi trả.
        public DataTable DSTatCaPhieuDoiTra()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPhieuDT, MaDH, MaSerialSP, MaKH, NgayYeuCau, LyDo, LoaiXuLy, TrangThai, NgayTao, NgayCapNhat FROM PhieuDoiTra";
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

        /// Lấy phiếu đổi trả theo mã phiếu.
        public DTO_PhieuDoiTra? DSTheoMaPhieuDoiTra(string maPhieuDT)
        {
            DTO_PhieuDoiTra? pdt = null;
            string sql = "SELECT MaPhieuDT, MaDH, MaSerialSP, MaKH, NgayYeuCau, LyDo, LoaiXuLy, TrangThai, NgayTao, NgayCapNhat FROM PhieuDoiTra WHERE MaPhieuDT = @MaPhieuDT";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaPhieuDT", SqlDbType.Char, 10) { Value = maPhieuDT });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pdt = new DTO_PhieuDoiTra
                        {
                            MaPhieuDT = reader["MaPhieuDT"].ToString()!.Trim(),
                            MaDH = reader["MaDH"].ToString()!.Trim(),
                            MaSerialSP = reader["MaSerialSP"].ToString()!.Trim(),
                            MaKH = reader["MaKH"].ToString()!.Trim(),
                            NgayYeuCau = Convert.ToDateTime(reader["NgayYeuCau"]),
                            LyDo = reader["LyDo"].ToString()!,
                            LoaiXuLy = reader["LoaiXuLy"].ToString()!,
                            TrangThai = reader["TrangThai"].ToString()!,
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
            return pdt;
        }

        /// Thêm một phiếu đổi trả mới.
        /// NgayYeuCau và NgayTao được DB Server tự động gán giá trị mặc định.
        public bool ThemPhieuDoiTra(DTO_PhieuDoiTra pdt)
        {
            string sql = "INSERT INTO PhieuDoiTra (MaPhieuDT, MaDH, MaSerialSP, MaKH, LyDo, LoaiXuLy, TrangThai, NgayCapNhat) VALUES (@MaPhieuDT, @MaDH, @MaSerialSP, @MaKH, @LyDo, @LoaiXuLy, @TrangThai, @NgayCapNhat)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPhieuDT", SqlDbType.Char, 10) { Value = pdt.MaPhieuDT });
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = pdt.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = pdt.MaSerialSP });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = pdt.MaKH });
            cmd.Parameters.Add(new SqlParameter("@LyDo", SqlDbType.NVarChar, 500) { Value = pdt.LyDo });
            cmd.Parameters.Add(new SqlParameter("@LoaiXuLy", SqlDbType.NVarChar, 50) { Value = pdt.LoaiXuLy });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pdt.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = pdt.NgayCapNhat.HasValue ? (object)pdt.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật thông tin phiếu đổi trả.
        public bool CapNhatPhieuDoiTra(DTO_PhieuDoiTra pdt)
        {
            string sql = "UPDATE PhieuDoiTra SET MaDH = @MaDH, MaSerialSP = @MaSerialSP, MaKH = @MaKH, LyDo = @LyDo, LoaiXuLy = @LoaiXuLy, TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat WHERE MaPhieuDT = @MaPhieuDT";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPhieuDT", SqlDbType.Char, 10) { Value = pdt.MaPhieuDT });
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = pdt.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = pdt.MaSerialSP });
            cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.Char, 10) { Value = pdt.MaKH });
            cmd.Parameters.Add(new SqlParameter("@LyDo", SqlDbType.NVarChar, 500) { Value = pdt.LyDo });
            cmd.Parameters.Add(new SqlParameter("@LoaiXuLy", SqlDbType.NVarChar, 50) { Value = pdt.LoaiXuLy });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pdt.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = pdt.NgayCapNhat.HasValue ? (object)pdt.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật trạng thái phiếu đổi trả (Đang Xử Lý | Hoàn Thành | Từ Chối) và tự động cập nhật NgayCapNhat.
        public bool CapNhatTrangThai(string maPhieuDT, string trangThai)
        {
            string sql = "UPDATE PhieuDoiTra SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() WHERE MaPhieuDT = @MaPhieuDT";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPhieuDT", SqlDbType.Char, 10) { Value = maPhieuDT });
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

        /// Cập nhật loại xử lý của phiếu đổi trả (Đổi Máy | Hoàn Tiền | Từ Chối) và tự động cập nhật NgayCapNhat.
        public bool CapNhatLoaiXuLy(string maPhieuDT, string loaiXuLy)
        {
            string sql = "UPDATE PhieuDoiTra SET LoaiXuLy = @LoaiXuLy, NgayCapNhat = GETDATE() WHERE MaPhieuDT = @MaPhieuDT";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPhieuDT", SqlDbType.Char, 10) { Value = maPhieuDT });
            cmd.Parameters.Add(new SqlParameter("@LoaiXuLy", SqlDbType.NVarChar, 50) { Value = loaiXuLy });

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

        /// Lấy toàn bộ phiếu đổi trả của một serial sản phẩm cụ thể.
        public DataTable DSTheoMaSerial(string maSerial)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPhieuDT, MaDH, MaSerialSP, MaKH, NgayYeuCau, LyDo, LoaiXuLy, TrangThai, NgayTao, NgayCapNhat FROM PhieuDoiTra WHERE MaSerialSP = @MaSerialSP";
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

        /// Lấy toàn bộ phiếu đổi trả của một khách hàng cụ thể.
        public DataTable DSTheoKhachHang(string maKH)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPhieuDT, MaDH, MaSerialSP, MaKH, NgayYeuCau, LyDo, LoaiXuLy, TrangThai, NgayTao, NgayCapNhat FROM PhieuDoiTra WHERE MaKH = @MaKH";
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

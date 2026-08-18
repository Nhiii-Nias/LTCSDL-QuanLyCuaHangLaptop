using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_PhieuNhap : DBConnect
    {
        /// Lấy toàn bộ danh sách phiếu nhập.
        /// Trả về DataTable chứa danh sách phiếu nhập.
        public DataTable DSTatCaPhieuNhap()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPhieuNhap, MaNV, MaNCC, NgayNhap, TongTien, TrangThai, NgayTao, NgayCapNhat, NguoiTao FROM PhieuNhap";
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

        /// Lấy thông tin phiếu nhập theo mã.
        /// Trả về DTO_PhieuNhap nếu tìm thấy, ngược lại trả về null.
        public DTO_PhieuNhap? DSTheoMaPhieuNhap(string maPhieuNhap)
        {
            DTO_PhieuNhap? pn = null;
            string sql = "SELECT MaPhieuNhap, MaNV, MaNCC, NgayNhap, TongTien, TrangThai, NgayTao, NgayCapNhat, NguoiTao FROM PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pn = new DTO_PhieuNhap
                        {
                            MaPhieuNhap = reader["MaPhieuNhap"].ToString()!.Trim(),
                            MaNV = reader["MaNV"].ToString()!.Trim(),
                            MaNCC = reader["MaNCC"].ToString()!.Trim(),
                            NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                            TongTien = Convert.ToDecimal(reader["TongTien"]),
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
            return pn;
        }

        /// Lấy danh sách phiếu nhập của một nhà cung cấp.
        /// Trả về DataTable chứa danh sách phiếu nhập.
        public DataTable DSTheoNhaCungCap(string maNCC)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaPhieuNhap, MaNV, MaNCC, NgayNhap, TongTien, TrangThai, NgayTao, NgayCapNhat, NguoiTao " +
                         "FROM PhieuNhap WHERE MaNCC = @MaNCC";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Char, 10) { Value = maNCC });

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

        /// Thêm một phiếu nhập mới (NgayNhap, NgayTao dùng mặc định của DB nếu không set đặc thù).
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemPhieuNhap(DTO_PhieuNhap pn)
        {
            string sql = "INSERT INTO PhieuNhap (MaPhieuNhap, MaNV, MaNCC, TongTien, TrangThai, NgayCapNhat, NguoiTao) " +
                         "VALUES (@MaPhieuNhap, @MaNV, @MaNCC, @TongTien, @TrangThai, @NgayCapNhat, @NguoiTao)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = pn.MaPhieuNhap });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = pn.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Char, 10) { Value = pn.MaNCC });
            cmd.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = pn.TongTien });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pn.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = pn.NgayCapNhat.HasValue ? (object)pn.NgayCapNhat.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@NguoiTao", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(pn.NguoiTao) ? DBNull.Value : pn.NguoiTao });

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

        /// Cập nhật phiếu nhập.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatPhieuNhap(DTO_PhieuNhap pn)
        {
            string sql = "UPDATE PhieuNhap SET MaNV = @MaNV, MaNCC = @MaNCC, TongTien = @TongTien, " +
                         "TrangThai = @TrangThai, NgayCapNhat = @NgayCapNhat WHERE MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = pn.MaPhieuNhap });
            cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = pn.MaNV });
            cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Char, 10) { Value = pn.MaNCC });
            cmd.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = pn.TongTien });
            cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = pn.TrangThai });
            cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = pn.NgayCapNhat.HasValue ? (object)pn.NgayCapNhat.Value : DBNull.Value });

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

        /// Cập nhật trạng thái phiếu nhập (Chờ Xác Nhận / Đã Nhập / Huỷ).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatTrangThai(string maPhieuNhap, string trangThai)
        {
            string sql = "UPDATE PhieuNhap SET TrangThai = @TrangThai, NgayCapNhat = GETDATE() WHERE MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap });
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
    }
}

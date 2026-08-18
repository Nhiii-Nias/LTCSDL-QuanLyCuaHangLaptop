using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_ChiTietDonHang : DBConnect
    {
        /// Lấy toàn bộ danh sách chi tiết đơn hàng.
        /// Trả về DataTable chứa danh sách chi tiết đơn hàng.
        public DataTable DSTatCaChiTietDonHang()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaDH, MaSerialSP, GiaBan, PhanTramGiam FROM ChiTietDonHang";
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

        /// Lấy chi tiết đơn hàng theo MaSerialSP
        /// Trả về DTO_ChiTietDonHang nếu tìm thấy, ngược lại trả về null.
        public DTO_ChiTietDonHang? DSTheoMaSerialSP(string maSerialSP)
        {
            DTO_ChiTietDonHang? ctdh = null;
            string sql = "SELECT MaDH, MaSerialSP, GiaBan, PhanTramGiam FROM ChiTietDonHang WHERE MaSerialSP = @MaSerialSP";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerialSP });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ctdh = new DTO_ChiTietDonHang
                        {
                            MaDH = reader["MaDH"].ToString()!.Trim(),
                            MaSerialSP = reader["MaSerialSP"].ToString()!.Trim(),
                            GiaBan = Convert.ToDecimal(reader["GiaBan"]),
                            PhanTramGiam = reader["PhanTramGiam"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["PhanTramGiam"])
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
            return ctdh;
        }

        /// Lấy toàn bộ chi tiết đơn hàng của một đơn hàng cụ thể.
        /// Trả về DataTable chứa các chi tiết đơn hàng.
        public DataTable DSTheoDonHang(string maDH)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaDH, MaSerialSP, GiaBan, PhanTramGiam FROM ChiTietDonHang WHERE MaDH = @MaDH";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });

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

        /// Thêm chi tiết đơn hàng mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemChiTietDonHang(DTO_ChiTietDonHang ctdh)
        {
            string sql = "INSERT INTO ChiTietDonHang (MaDH, MaSerialSP, GiaBan, PhanTramGiam) VALUES (@MaDH, @MaSerialSP, @GiaBan, @PhanTramGiam)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = ctdh.MaDH });
            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = ctdh.MaSerialSP });
            cmd.Parameters.Add(new SqlParameter("@GiaBan", SqlDbType.Decimal) { Value = ctdh.GiaBan });
            cmd.Parameters.Add(new SqlParameter("@PhanTramGiam", SqlDbType.Decimal) { Value = ctdh.PhanTramGiam.HasValue ? (object)ctdh.PhanTramGiam.Value : DBNull.Value });

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
        /// Lấy danh sách chi tiết đơn hàng kèm theo thông tin chi tiết của sản phẩm (Tên loại, Tên hãng, Danh mục) và thành tiền.
        /// </summary>
        /// <param name="maDH">Mã đơn hàng</param>
        /// <returns>DataTable chứa thông tin chi tiết</returns>
        public DataTable DSChiTietCoThongTinSanPham(string maDH)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    ctdh.MaDH, ctdh.MaSerialSP, sp.MaLoaiSP,
                    lsp.TenLoai, h.TenHang, lsp.DanhMuc,
                    ctdh.GiaBan, ctdh.PhanTramGiam,
                    ctdh.GiaBan * (1 - ISNULL(ctdh.PhanTramGiam, 0) / 100.0) AS ThanhTien
                FROM ChiTietDonHang ctdh, SanPham sp, LoaiSanPham lsp, HangSanXuat h
                WHERE ctdh.MaDH = @MaDH AND sp.MaSerialSP = ctdh.MaSerialSP AND lsp.MaLoaiSP = sp.MaLoaiSP AND h.MaHang = lsp.MaHang";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });

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

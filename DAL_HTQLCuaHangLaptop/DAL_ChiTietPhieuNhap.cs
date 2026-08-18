using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_ChiTietPhieuNhap : DBConnect
    {
        /// Lấy toàn bộ danh sách chi tiết phiếu nhập.
        /// Trả về DataTable chứa danh sách chi tiết phiếu nhập.
        public DataTable DSTatCaChiTietPhieuNhap()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaLoaiSP, MaPhieuNhap, SoLuong, GiaNhap FROM ChiTietPhieuNhap";
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

        /// Lấy thông tin chi tiết phiếu nhập theo khóa chính kép (MaLoaiSP, MaPhieuNhap).
        /// Trả về DTO_ChiTietPhieuNhap nếu tìm thấy, ngược lại trả về null.
        public DTO_ChiTietPhieuNhap? DSTheoKhoaKep(string maLoaiSP, string maPhieuNhap)
        {
            DTO_ChiTietPhieuNhap? ctpn = null;
            string sql = "SELECT MaLoaiSP, MaPhieuNhap, SoLuong, GiaNhap FROM ChiTietPhieuNhap WHERE MaLoaiSP = @MaLoaiSP AND MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ctpn = new DTO_ChiTietPhieuNhap
                        {
                            MaLoaiSP = reader["MaLoaiSP"].ToString()!.Trim(),
                            MaPhieuNhap = reader["MaPhieuNhap"].ToString()!.Trim(),
                            SoLuong = Convert.ToInt32(reader["SoLuong"]),
                            GiaNhap = Convert.ToDecimal(reader["GiaNhap"])
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
            return ctpn;
        }

        /// Lấy toàn bộ chi tiết phiếu nhập của một phiếu nhập cụ thể.
        /// Trả về DataTable chứa các dòng chi tiết.
        public DataTable DSTheoPhieuNhap(string maPhieuNhap)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaLoaiSP, MaPhieuNhap, SoLuong, GiaNhap FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap });

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

        /// Thêm chi tiết phiếu nhập mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemChiTietPhieuNhap(DTO_ChiTietPhieuNhap ctpn)
        {
            string sql = "INSERT INTO ChiTietPhieuNhap (MaLoaiSP, MaPhieuNhap, SoLuong, GiaNhap) VALUES (@MaLoaiSP, @MaPhieuNhap, @SoLuong, @GiaNhap)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = ctpn.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = ctpn.MaPhieuNhap });
            cmd.Parameters.Add(new SqlParameter("@SoLuong", SqlDbType.Int) { Value = ctpn.SoLuong });
            cmd.Parameters.Add(new SqlParameter("@GiaNhap", SqlDbType.Decimal) { Value = ctpn.GiaNhap });

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

        /// Cập nhật chi tiết phiếu nhập (sử dụng khóa kép để xác định dòng cần sửa).
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatChiTietPhieuNhap(DTO_ChiTietPhieuNhap ctpn)
        {
            string sql = "UPDATE ChiTietPhieuNhap SET SoLuong = @SoLuong, GiaNhap = @GiaNhap WHERE MaLoaiSP = @MaLoaiSP AND MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = ctpn.MaLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = ctpn.MaPhieuNhap });
            cmd.Parameters.Add(new SqlParameter("@SoLuong", SqlDbType.Int) { Value = ctpn.SoLuong });
            cmd.Parameters.Add(new SqlParameter("@GiaNhap", SqlDbType.Decimal) { Value = ctpn.GiaNhap });

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

        /// Xóa vật lý chi tiết phiếu nhập theo khóa chính kép.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaChiTietPhieuNhap(string maLoaiSP, string maPhieuNhap)
        {
            string sql = "DELETE FROM ChiTietPhieuNhap WHERE MaLoaiSP = @MaLoaiSP AND MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap });

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
        /// Lấy danh sách chi tiết phiếu nhập kèm theo thông tin chi tiết của sản phẩm (Tên loại, Danh mục, Tên hãng) và thành tiền.
        /// </summary>
        /// <param name="maPhieuNhap">Mã phiếu nhập</param>
        /// <returns>DataTable chứa thông tin chi tiết</returns>
        public DataTable DSChiTietCoThongTinSanPham(string maPhieuNhap)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    ctpn.MaLoaiSP,
                    ctpn.MaPhieuNhap,
                    lsp.TenLoai,
                    lsp.DanhMuc,
                    h.TenHang,
                    ctpn.SoLuong,
                    ctpn.GiaNhap,
                    ctpn.SoLuong * ctpn.GiaNhap AS ThanhTien
                FROM ChiTietPhieuNhap ctpn
                JOIN LoaiSanPham lsp ON lsp.MaLoaiSP = ctpn.MaLoaiSP
                JOIN HangSanXuat h   ON h.MaHang     = lsp.MaHang
                WHERE ctpn.MaPhieuNhap = @MaPhieuNhap";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap });

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

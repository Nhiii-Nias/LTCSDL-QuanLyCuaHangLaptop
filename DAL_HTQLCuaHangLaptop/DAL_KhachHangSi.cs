using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_KhachHangSi : DBConnect
    {
        /// Lấy danh sách toàn bộ khách hàng sỉ (JOIN với bảng KhachHang để lấy thông tin chi tiết, chỉ lấy khách hàng chưa bị xóa).
        /// Trả về DataTable chứa thông tin khách hàng sỉ.
        public DataTable DSTatCaKhachHangSi()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT kh.MaKH, kh.TenKH, kh.Email, kh.SDT, kh.DiaChi, kh.LoaiKH, kh.NgayTao, kh.NgayCapNhat, kh.NguoiTao " +
                         "FROM KhachHangSi khSi " +
                         "JOIN KhachHang kh ON khSi.MaKHSi = kh.MaKH " +
                         "WHERE kh.IsDeleted = 0";
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

        /// Lấy thông tin khách hàng sỉ theo mã.
        /// Trả về DTO_KhachHangSi nếu tìm thấy, ngược lại trả về null.
        public DTO_KhachHangSi? DSTheoMaKHSi(string maKHSi)
        {
            DTO_KhachHangSi? khSi = null;
            string sql = "SELECT MaKHSi FROM KhachHangSi WHERE MaKHSi = @MaKHSi";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKHSi", SqlDbType.Char, 10) { Value = maKHSi });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        khSi = new DTO_KhachHangSi
                        {
                            MaKHSi = reader["MaKHSi"].ToString()!.Trim()
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
            return khSi;
        }

        /// Thêm thông tin khách hàng sỉ mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemKhachHangSi(DTO_KhachHangSi khSi)
        {
            string sql = "INSERT INTO KhachHangSi (MaKHSi) VALUES (@MaKHSi)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaKHSi", SqlDbType.Char, 10) { Value = khSi.MaKHSi });

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

        /// Xóa vật lý thông tin khách hàng sỉ.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaKhachHangSi(string maKHSi)
        {
            string sql = "DELETE FROM KhachHangSi WHERE MaKHSi = @MaKHSi";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKHSi", SqlDbType.Char, 10) { Value = maKHSi });

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

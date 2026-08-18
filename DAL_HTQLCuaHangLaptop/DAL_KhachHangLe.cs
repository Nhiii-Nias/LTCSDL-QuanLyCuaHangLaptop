using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_KhachHangLe : DBConnect
    {
        /// Lấy danh sách toàn bộ khách hàng lẻ (JOIN với bảng KhachHang để lấy thông tin chi tiết, chỉ lấy khách hàng chưa bị xóa).
        /// Trả về DataTable chứa thông tin khách hàng lẻ.
        public DataTable DSTatCaKhachHangLe()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT kh.MaKH, kh.TenKH, kh.Email, kh.SDT, kh.DiaChi, kh.LoaiKH, kh.NgayTao, kh.NgayCapNhat, kh.NguoiTao, " +
                         "khLe.LaHSSV, khLe.SinhNhat " +
                         "FROM KhachHangLe khLe " +
                         "JOIN KhachHang kh ON khLe.MaKHLe = kh.MaKH " +
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

        /// Lấy thông tin chi tiết của một khách hàng lẻ theo mã.
        /// Trả về DTO_KhachHangLe nếu tìm thấy, ngược lại trả về null.
        public DTO_KhachHangLe? DSTheoMaKHLe(string maKHLe)
        {
            DTO_KhachHangLe? khLe = null;
            string sql = "SELECT MaKHLe, LaHSSV, SinhNhat FROM KhachHangLe WHERE MaKHLe = @MaKHLe";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKHLe", SqlDbType.Char, 10) { Value = maKHLe });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        khLe = new DTO_KhachHangLe
                        {
                            MaKHLe = reader["MaKHLe"].ToString()!.Trim(),
                            LaHSSV = Convert.ToBoolean(reader["LaHSSV"]),
                            SinhNhat = reader["SinhNhat"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["SinhNhat"])
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
            return khLe;
        }

        /// Lấy danh sách khách hàng lẻ là Học sinh / Sinh viên (LaHSSV = 1 và chưa bị xóa).
        /// Trả về DataTable chứa thông tin khách hàng lẻ HSSV.
        public DataTable DSKhachHangLeHSSV()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT kh.MaKH, kh.TenKH, kh.Email, kh.SDT, kh.DiaChi, kh.LoaiKH, kh.NgayTao, kh.NgayCapNhat, kh.NguoiTao, " +
                         "khLe.LaHSSV, khLe.SinhNhat " +
                         "FROM KhachHangLe khLe " +
                         "JOIN KhachHang kh ON khLe.MaKHLe = kh.MaKH " +
                         "WHERE khLe.LaHSSV = 1 AND kh.IsDeleted = 0";
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

        /// Thêm thông tin khách hàng lẻ mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemKhachHangLe(DTO_KhachHangLe khLe)
        {
            string sql = "INSERT INTO KhachHangLe (MaKHLe, LaHSSV, SinhNhat) VALUES (@MaKHLe, @LaHSSV, @SinhNhat)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaKHLe", SqlDbType.Char, 10) { Value = khLe.MaKHLe });
            cmd.Parameters.Add(new SqlParameter("@LaHSSV", SqlDbType.Bit) { Value = khLe.LaHSSV });
            cmd.Parameters.Add(new SqlParameter("@SinhNhat", SqlDbType.Date) { Value = khLe.SinhNhat.HasValue ? (object)khLe.SinhNhat.Value : DBNull.Value });

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

        /// Cập nhật thông tin khách hàng lẻ.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatKhachHangLe(DTO_KhachHangLe khLe)
        {
            string sql = "UPDATE KhachHangLe SET LaHSSV = @LaHSSV, SinhNhat = @SinhNhat WHERE MaKHLe = @MaKHLe";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaKHLe", SqlDbType.Char, 10) { Value = khLe.MaKHLe });
            cmd.Parameters.Add(new SqlParameter("@LaHSSV", SqlDbType.Bit) { Value = khLe.LaHSSV });
            cmd.Parameters.Add(new SqlParameter("@SinhNhat", SqlDbType.Date) { Value = khLe.SinhNhat.HasValue ? (object)khLe.SinhNhat.Value : DBNull.Value });

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

        /// Xóa vật lý thông tin khách hàng lẻ.
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaKhachHangLe(string maKHLe)
        {
            string sql = "DELETE FROM KhachHangLe WHERE MaKHLe = @MaKHLe";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKHLe", SqlDbType.Char, 10) { Value = maKHLe });

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

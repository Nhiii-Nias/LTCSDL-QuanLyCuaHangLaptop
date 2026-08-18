using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_HTQLCuaHangLaptop;

namespace DAL_HTQLCuaHangLaptop
{
    public class DAL_KhuyenMai : DBConnect
    {
        /// Lấy toàn bộ danh sách chương trình khuyến mãi.
        /// Trả về DataTable chứa danh sách khuyến mãi.
        public DataTable DSTatCaKhuyenMai()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaKM, TenKM, DoiTuong, DieuKien, NgayBatDau, NgayKetThuc, MoTa, MucGiamSP, MucGiamDH, SLToiThieu, NgayTao, isHienThi FROM KhuyenMai";
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

        /// Lấy thông tin chương trình khuyến mãi theo mã.
        /// Trả về DTO_KhuyenMai nếu tìm thấy, ngược lại trả về null.
        public DTO_KhuyenMai? DSTheoMaKM(string maKM)
        {
            DTO_KhuyenMai? km = null;
            string sql = "SELECT MaKM, TenKM, DoiTuong, DieuKien, NgayBatDau, NgayKetThuc, MoTa, MucGiamSP, MucGiamDH, SLToiThieu, NgayTao, isHienThi FROM KhuyenMai WHERE MaKM = @MaKM";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = maKM });

            try
            {
                _conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        km = new DTO_KhuyenMai
                        {
                            MaKM = reader["MaKM"].ToString()!.Trim(),
                            TenKM = reader["TenKM"].ToString()!,
                            DoiTuong = reader["DoiTuong"] == DBNull.Value ? null! : reader["DoiTuong"].ToString()!,
                            DieuKien = reader["DieuKien"] == DBNull.Value ? null! : reader["DieuKien"].ToString()!,
                            NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                            NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]),
                            MoTa = reader["MoTa"] == DBNull.Value ? null! : reader["MoTa"].ToString()!,
                            MucGiamSP = reader["MucGiamSP"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["MucGiamSP"]),
                            MucGiamDH = reader["MucGiamDH"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["MucGiamDH"]),
                            SLToiThieu = reader["SLToiThieu"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["SLToiThieu"]),
                            IsHienThi = reader["isHienThi"] == DBNull.Value ? true : Convert.ToBoolean(reader["isHienThi"]),
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
            return km;
        }

        /// Lấy các chương trình khuyến mãi đang hoạt động tại thời điểm được truyền vào.
        /// Trả về DataTable chứa danh sách khuyến mãi đang hoạt động.
        public DataTable DSTrongThoiGianHieuLuc(DateTime ngayHienTai)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaKM, TenKM, DoiTuong, DieuKien, NgayBatDau, NgayKetThuc, MoTa, MucGiamSP, MucGiamDH, SLToiThieu, NgayTao, isHienThi " +
                         "FROM KhuyenMai WHERE NgayBatDau <= @NgayHienTai AND NgayKetThuc >= @NgayHienTai";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@NgayHienTai", SqlDbType.Date) { Value = ngayHienTai });

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

        /// Lấy danh sách khuyến mãi lọc theo đối tượng áp dụng (Tất Cả / HSSV / Doanh Nghiệp).
        /// Trả về DataTable chứa danh sách khuyến mãi.
        public DataTable DSTheoDoiTuong(string doiTuong)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT MaKM, TenKM, DoiTuong, DieuKien, NgayBatDau, NgayKetThuc, MoTa, MucGiamSP, MucGiamDH, SLToiThieu, NgayTao, isHienThi " +
                         "FROM KhuyenMai WHERE DoiTuong = @DoiTuong";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@DoiTuong", SqlDbType.NVarChar, 100) { Value = doiTuong });

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

        /// Thêm chương trình khuyến mãi mới.
        /// Trả về true nếu thêm thành công, ngược lại False.
        public bool ThemKhuyenMai(DTO_KhuyenMai km)
        {
            string sql = "INSERT INTO KhuyenMai (MaKM, TenKM, DoiTuong, DieuKien, NgayBatDau, NgayKetThuc, MoTa, MucGiamSP, MucGiamDH, SLToiThieu, isHienThi) " +
                         "VALUES (@MaKM, @TenKM, @DoiTuong, @DieuKien, @NgayBatDau, @NgayKetThuc, @MoTa, @MucGiamSP, @MucGiamDH, @SLToiThieu, @IsHienThi)";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = km.MaKM });
            cmd.Parameters.Add(new SqlParameter("@TenKM", SqlDbType.NVarChar, 200) { Value = km.TenKM });
            cmd.Parameters.Add(new SqlParameter("@DoiTuong", SqlDbType.NVarChar, 100) { Value = string.IsNullOrEmpty(km.DoiTuong) ? "Tất Cả" : km.DoiTuong });
            cmd.Parameters.Add(new SqlParameter("@DieuKien", SqlDbType.NVarChar, 500) { Value = string.IsNullOrEmpty(km.DieuKien) ? DBNull.Value : km.DieuKien });
            cmd.Parameters.Add(new SqlParameter("@NgayBatDau", SqlDbType.Date) { Value = km.NgayBatDau });
            cmd.Parameters.Add(new SqlParameter("@NgayKetThuc", SqlDbType.Date) { Value = km.NgayKetThuc });
            cmd.Parameters.Add(new SqlParameter("@MoTa", SqlDbType.NVarChar, 500) { Value = string.IsNullOrEmpty(km.MoTa) ? DBNull.Value : km.MoTa });
            cmd.Parameters.Add(new SqlParameter("@MucGiamSP", SqlDbType.Decimal) { Value = km.MucGiamSP.HasValue ? (object)km.MucGiamSP.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@MucGiamDH", SqlDbType.Decimal) { Value = km.MucGiamDH.HasValue ? (object)km.MucGiamDH.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@SLToiThieu", SqlDbType.Int) { Value = km.SLToiThieu.HasValue ? (object)km.SLToiThieu.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsHienThi", SqlDbType.Bit) { Value = km.IsHienThi });

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

        /// Cập nhật thông tin khuyến mãi.
        /// Trả về true nếu cập nhật thành công, ngược lại False.
        public bool CapNhatKhuyenMai(DTO_KhuyenMai km)
        {
            string sql = "UPDATE KhuyenMai SET TenKM = @TenKM, DoiTuong = @DoiTuong, DieuKien = @DieuKien, " +
                         "NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, MoTa = @MoTa, " +
                         "MucGiamSP = @MucGiamSP, MucGiamDH = @MucGiamDH, SLToiThieu = @SLToiThieu, isHienThi = @IsHienThi " +
                         "WHERE MaKM = @MaKM";
            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = km.MaKM });
            cmd.Parameters.Add(new SqlParameter("@TenKM", SqlDbType.NVarChar, 200) { Value = km.TenKM });
            cmd.Parameters.Add(new SqlParameter("@DoiTuong", SqlDbType.NVarChar, 100) { Value = string.IsNullOrEmpty(km.DoiTuong) ? "Tất Cả" : km.DoiTuong });
            cmd.Parameters.Add(new SqlParameter("@DieuKien", SqlDbType.NVarChar, 500) { Value = string.IsNullOrEmpty(km.DieuKien) ? DBNull.Value : km.DieuKien });
            cmd.Parameters.Add(new SqlParameter("@NgayBatDau", SqlDbType.Date) { Value = km.NgayBatDau });
            cmd.Parameters.Add(new SqlParameter("@NgayKetThuc", SqlDbType.Date) { Value = km.NgayKetThuc });
            cmd.Parameters.Add(new SqlParameter("@MoTa", SqlDbType.NVarChar, 500) { Value = string.IsNullOrEmpty(km.MoTa) ? DBNull.Value : km.MoTa });
            cmd.Parameters.Add(new SqlParameter("@MucGiamSP", SqlDbType.Decimal) { Value = km.MucGiamSP.HasValue ? (object)km.MucGiamSP.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@MucGiamDH", SqlDbType.Decimal) { Value = km.MucGiamDH.HasValue ? (object)km.MucGiamDH.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@SLToiThieu", SqlDbType.Int) { Value = km.SLToiThieu.HasValue ? (object)km.SLToiThieu.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsHienThi", SqlDbType.Bit) { Value = km.IsHienThi });

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

        /// Xóa vật lý chương trình khuyến mãi (chỉ thành công khi chưa bị ràng buộc ngoại).
        /// Trả về true nếu xóa thành công, ngược lại False.
        public bool XoaKhuyenMai(string maKM)
        {
            string sql = "DELETE FROM KhuyenMai WHERE MaKM = @MaKM";
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = maKM });

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

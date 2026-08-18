using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ quản lý khách hàng (lẻ và sỉ) của cửa hàng.
    /// Chịu trách nhiệm: Kiểm tra thông tin, tạo khách hàng mới (transaction 2 bảng),
    /// cập nhật, xóa mềm, và quản lý trạng thái tài khoản khách hàng.
    public class BUS_KhachHang
    {
        // Khai báo các DAL và BUS liên quan
        private readonly DAL_KhachHang    _dalKH    = new DAL_KhachHang();
        private readonly DAL_KhachHangLe  _dalKHLe  = new DAL_KhachHangLe();
        private readonly DAL_KhachHangSi  _dalKHSi  = new DAL_KhachHangSi();
        private readonly DAL_TaiKhoanKH   _dalTKKH  = new DAL_TaiKhoanKH();
        private readonly BUS_TaiKhoan     _busTK    = new BUS_TaiKhoan();


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: TRUY VẤN
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ khách hàng (cả lẻ lẫn sỉ) chưa bị xóa mềm.
        /// Trả về DataTable chứa danh sách khách hàng.
        public DataTable LayDanhSach()
        {
            try
            {
                return _dalKH.DSTatCaKhachHang();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khách hàng: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách khách hàng lẻ (kèm thông tin LaHSSV, SinhNhat).
        /// Trả về DataTable chứa danh sách khách hàng lẻ.
        public DataTable LayDanhSachKhachHangLe()
        {
            try
            {
                return _dalKHLe.DSTatCaKhachHangLe();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khách hàng lẻ: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách khách hàng sỉ (doanh nghiệp).
        /// Trả về DataTable chứa danh sách khách hàng sỉ.
        public DataTable LayDanhSachKhachHangSi()
        {
            try
            {
                return _dalKHSi.DSTatCaKhachHangSi();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khách hàng sỉ: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách khách hàng lẻ là HSSV (LaHSSV = 1).
        /// Trả về DataTable chứa danh sách khách hàng lẻ HSSV.
        public DataTable LayDanhSachHSSV()
        {
            try
            {
                return _dalKHLe.DSKhachHangLeHSSV();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách HSSV: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin khách hàng theo mã.
        /// <param name="maKH">Mã khách hàng cần tìm.
        /// Trả về DTO_KhachHang nếu tìm thấy, null nếu không tồn tại.
        public DTO_KhachHang? LayTheoMa(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            try
            {
                return _dalKH.DSTheoMaKH(maKH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin khách hàng: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin chi tiết khách hàng lẻ theo mã.
        /// <param name="maKH">Mã khách hàng lẻ.
        /// Trả về DTO_KhachHangLe nếu tìm thấy, null nếu không tồn tại.
        public DTO_KhachHangLe? LayThongTinLe(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            try
            {
                return _dalKHLe.DSTheoMaKHLe(maKH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin khách hàng lẻ: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin chi tiết khách hàng sỉ theo mã.
        /// <param name="maKH">Mã khách hàng sỉ.
        /// Trả về DTO_KhachHangSi nếu tìm thấy, null nếu không tồn tại.
        public DTO_KhachHangSi? LayThongTinSi(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            try
            {
                return _dalKHSi.DSTheoMaKHSi(maKH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin khách hàng sỉ: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách khách hàng theo loại.
        /// <param name="loaiKH">"Lẻ" hoặc "Sỉ".
        /// Trả về DataTable chứa danh sách.
        public DataTable LayDanhSachTheoLoai(string loaiKH)
        {
            if (loaiKH != "Lẻ" && loaiKH != "Sỉ")
                throw new ArgumentException("Loại khách hàng phải là 'Lẻ' hoặc 'Sỉ'.");
            try
            {
                return _dalKH.DSTheoLoaiKH(loaiKH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khách hàng theo loại: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: THÊM KHÁCH HÀNG (TRANSACTION)
        // ══════════════════════════════════════════════════════════════════

        /// Tạo khách hàng lẻ mới — INSERT vào KhachHang VÀ KhachHangLe trong cùng 1 transaction.
        /// Nếu bất kỳ bước nào thất bại thì rollback cả 2.
        /// <param name="kh">Thông tin chung khách hàng (LoaiKH sẽ tự đặt = "Lẻ").
        /// <param name="khLe">Thông tin riêng khách hàng lẻ (LaHSSV, SinhNhat).
        /// <param name="maTKNguoiTao">Mã tài khoản nhân viên thực hiện tạo (ghi NguoiTao).
        /// Trả về True nếu tạo thành công.
        public bool ThemKhachHangLe(DTO_KhachHang kh, DTO_KhachHangLe khLe, string? maTKNguoiTao = null)
        {
            // Validate chung
            KiemTraHopLe(kh);

            // Ép đúng loại
            kh.LoaiKH = "Lẻ";

            // Sinh mã KH lᮻ tự động (prefix KH, ví dụ KH00000007) — luôn ghi đè, không cho phép caller tự đặt mã
            kh.MaKH = _busTK.TaoMaKHLeMoi();

            // Đồng bộ mã giữa bảng cha và bảng con
            khLe.MaKHLe = kh.MaKH;

            // Gán thông tin tạo
            kh.NgayTao  = DateTime.Now;
            kh.NguoiTao = maTKNguoiTao?.Trim();
            kh.IsDeleted = false;

            // Thực thi giao dịch
            string connStr = ConfigurationManager
                .ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;

            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Bước 1: INSERT KhachHang
                string sqlKH = "INSERT INTO KhachHang (MaKH, TenKH, Email, SDT, DiaChi, LoaiKH, NgayTao, NgayCapNhat, NguoiTao, IsDeleted) " +
                               "VALUES (@MaKH, @TenKH, @Email, @SDT, @DiaChi, @LoaiKH, @NgayTao, @NgayCapNhat, @NguoiTao, 0)";
                using (SqlCommand cmdKH = new SqlCommand(sqlKH, conn, tran))
                {
                    cmdKH.Parameters.Add(new SqlParameter("@MaKH",      SqlDbType.Char,     10)  { Value = kh.MaKH });
                    cmdKH.Parameters.Add(new SqlParameter("@TenKH",     SqlDbType.NVarChar, 50)  { Value = kh.TenKH });
                    cmdKH.Parameters.Add(new SqlParameter("@Email",     SqlDbType.VarChar,  100) { Value = string.IsNullOrEmpty(kh.Email)  ? (object)DBNull.Value : kh.Email });
                    cmdKH.Parameters.Add(new SqlParameter("@SDT",       SqlDbType.VarChar,  10)  { Value = string.IsNullOrEmpty(kh.SDT)    ? (object)DBNull.Value : kh.SDT });
                    cmdKH.Parameters.Add(new SqlParameter("@DiaChi",    SqlDbType.NVarChar, 200) { Value = string.IsNullOrEmpty(kh.DiaChi) ? (object)DBNull.Value : kh.DiaChi });
                    cmdKH.Parameters.Add(new SqlParameter("@LoaiKH",    SqlDbType.NVarChar, 10)  { Value = kh.LoaiKH });
                    cmdKH.Parameters.Add(new SqlParameter("@NgayTao",   SqlDbType.DateTime)      { Value = kh.NgayTao });
                    cmdKH.Parameters.Add(new SqlParameter("@NgayCapNhat",SqlDbType.DateTime)     { Value = kh.NgayCapNhat.HasValue ? (object)kh.NgayCapNhat.Value : DBNull.Value });
                    cmdKH.Parameters.Add(new SqlParameter("@NguoiTao",  SqlDbType.Char,     10)  { Value = string.IsNullOrEmpty(kh.NguoiTao) ? (object)DBNull.Value : kh.NguoiTao });
                    cmdKH.ExecuteNonQuery();
                }

                // Bước 2: INSERT KhachHangLe
                string sqlKHLe = "INSERT INTO KhachHangLe (MaKHLe, LaHSSV, SinhNhat) VALUES (@MaKHLe, @LaHSSV, @SinhNhat)";
                using (SqlCommand cmdKHLe = new SqlCommand(sqlKHLe, conn, tran))
                {
                    cmdKHLe.Parameters.Add(new SqlParameter("@MaKHLe",  SqlDbType.Char, 10) { Value = khLe.MaKHLe });
                    cmdKHLe.Parameters.Add(new SqlParameter("@LaHSSV",  SqlDbType.Bit)      { Value = khLe.LaHSSV });
                    cmdKHLe.Parameters.Add(new SqlParameter("@SinhNhat",SqlDbType.Date)     { Value = khLe.SinhNhat.HasValue ? (object)khLe.SinhNhat.Value : DBNull.Value });
                    cmdKHLe.ExecuteNonQuery();
                }

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        /// Tạo khách hàng sỉ mới — INSERT vào KhachHang VÀ KhachHangSi trong cùng 1 transaction.
        /// Nếu bất kỳ bước nào thất bại thì rollback cả 2.
        /// <param name="kh">Thông tin chung khách hàng (LoaiKH sẽ tự đặt = "Sỉ").
        /// <param name="maTKNguoiTao">Mã tài khoản nhân viên thực hiện tạo (ghi NguoiTao).
        /// Trả về True nếu tạo thành công.
        public bool ThemKhachHangSi(DTO_KhachHang kh, string? maTKNguoiTao = null)
        {
            // Validate chung
            KiemTraHopLe(kh);

            // Ép đúng loại
            kh.LoaiKH = "Sỉ";

            // Sinh mã KH sỉ tự động (prefix DN, ví dụ DN00000010) — luôn ghi đè, không cho phép tự đặt mã
            kh.MaKH = _busTK.TaoMaKHSiMoi();

            // Gán audit
            kh.NgayTao  = DateTime.Now;
            kh.NguoiTao = maTKNguoiTao?.Trim();
            kh.IsDeleted = false;

            // Thực thi transaction
            string connStr = ConfigurationManager
                .ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;

            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Bước 1: INSERT KhachHang
                string sqlKH = "INSERT INTO KhachHang (MaKH, TenKH, Email, SDT, DiaChi, LoaiKH, NgayTao, NgayCapNhat, NguoiTao, IsDeleted) " +
                               "VALUES (@MaKH, @TenKH, @Email, @SDT, @DiaChi, @LoaiKH, @NgayTao, @NgayCapNhat, @NguoiTao, 0)";
                using (SqlCommand cmdKH = new SqlCommand(sqlKH, conn, tran))
                {
                    cmdKH.Parameters.Add(new SqlParameter("@MaKH",       SqlDbType.Char,     10)  { Value = kh.MaKH });
                    cmdKH.Parameters.Add(new SqlParameter("@TenKH",      SqlDbType.NVarChar, 50)  { Value = kh.TenKH });
                    cmdKH.Parameters.Add(new SqlParameter("@Email",      SqlDbType.VarChar,  100) { Value = string.IsNullOrEmpty(kh.Email)  ? (object)DBNull.Value : kh.Email });
                    cmdKH.Parameters.Add(new SqlParameter("@SDT",        SqlDbType.VarChar,  10)  { Value = string.IsNullOrEmpty(kh.SDT)    ? (object)DBNull.Value : kh.SDT });
                    cmdKH.Parameters.Add(new SqlParameter("@DiaChi",     SqlDbType.NVarChar, 200) { Value = string.IsNullOrEmpty(kh.DiaChi) ? (object)DBNull.Value : kh.DiaChi });
                    cmdKH.Parameters.Add(new SqlParameter("@LoaiKH",     SqlDbType.NVarChar, 10)  { Value = kh.LoaiKH });
                    cmdKH.Parameters.Add(new SqlParameter("@NgayTao",    SqlDbType.DateTime)      { Value = kh.NgayTao });
                    cmdKH.Parameters.Add(new SqlParameter("@NgayCapNhat",SqlDbType.DateTime)      { Value = kh.NgayCapNhat.HasValue ? (object)kh.NgayCapNhat.Value : DBNull.Value });
                    cmdKH.Parameters.Add(new SqlParameter("@NguoiTao",   SqlDbType.Char,     10)  { Value = string.IsNullOrEmpty(kh.NguoiTao) ? (object)DBNull.Value : kh.NguoiTao });
                    cmdKH.ExecuteNonQuery();
                }

                // Bước 2: INSERT KhachHangSi
                string sqlKHSi = "INSERT INTO KhachHangSi (MaKHSi) VALUES (@MaKHSi)";
                using (SqlCommand cmdKHSi = new SqlCommand(sqlKHSi, conn, tran))
                {
                    cmdKHSi.Parameters.Add(new SqlParameter("@MaKHSi", SqlDbType.Char, 10) { Value = kh.MaKH });
                    cmdKHSi.ExecuteNonQuery();
                }

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 3: CẬP NHẬT
        // ══════════════════════════════════════════════════════════════════

        /// Cập nhật thông tin chung của khách hàng (TenKH, Email, SDT, DiaChi).
        /// Không cho phép đổi LoaiKH sau khi đã tạo.
        /// <param name="kh">DTO chứa thông tin cần cập nhật. MaKH bắt buộc phải có.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhat(DTO_KhachHang kh)
        {
            if (string.IsNullOrWhiteSpace(kh.MaKH))
                throw new ArgumentException("Mã khách hàng không được để trống khi cập nhật.");

            // Kiểm tra khách hàng có tồn tại
            var existing = _dalKH.DSTheoMaKH(kh.MaKH.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Khách hàng '{kh.MaKH}' không tồn tại hoặc đã bị xóa.");

            // Validate thông tin cập nhật
            if (string.IsNullOrWhiteSpace(kh.TenKH))
                throw new ArgumentException("Tên khách hàng không được để trống.");
            if (!string.IsNullOrEmpty(kh.Email) && !Regex.IsMatch(kh.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Địa chỉ Email không đúng định dạng.");
            if (!string.IsNullOrEmpty(kh.SDT) && !Regex.IsMatch(kh.SDT, @"^\d{10}$"))
                throw new ArgumentException("Số điện thoại phải gồm đúng 10 chữ số.");

            // Giữ nguyên LoaiKH từ bản ghi gốc (không cho đổi loại)
            kh.LoaiKH = existing.LoaiKH;
            kh.NgayCapNhat = DateTime.Now;

            try
            {
                return _dalKH.CapNhatKhachHang(kh);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật khách hàng: {ex.Message}", ex);
            }
        }

        /// Cập nhật thông tin riêng của khách hàng lẻ (LaHSSV, SinhNhat).
        /// Chỉ áp dụng cho LoaiKH = 'Lẻ'.
        /// <param name="khLe">DTO chứa thông tin cần cập nhật. MaKHLe bắt buộc phải có.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatThongTinLe(DTO_KhachHangLe khLe)
        {
            if (string.IsNullOrWhiteSpace(khLe.MaKHLe))
                throw new ArgumentException("Mã khách hàng không được để trống.");

            // Kiểm tra đây thực sự là khách hàng lẻ
            var kh = _dalKH.DSTheoMaKH(khLe.MaKHLe.Trim());
            if (kh == null)
                throw new InvalidOperationException($"Khách hàng '{khLe.MaKHLe}' không tồn tại.");
            if (kh.LoaiKH != "Lẻ")
                throw new InvalidOperationException($"Khách hàng '{khLe.MaKHLe}' không phải khách hàng lẻ. LaHSSV chỉ áp dụng cho loại 'Lẻ'.");

            try
            {
                return _dalKHLe.CapNhatKhachHangLe(khLe);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật thông tin khách hàng lẻ: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 4: XÓA MỀM
        // ══════════════════════════════════════════════════════════════════

        /// Xóa mềm khách hàng bằng cách đánh dấu IsDeleted = 1. Không xóa vật lý.
        /// Kiểm tra khách hàng tồn tại trước khi xóa.
        /// <param name="maKH">Mã khách hàng cần xóa mềm.
        /// Trả về True nếu xóa mềm thành công.
        public bool Xoa(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");

            var existing = _dalKH.DSTheoMaKH(maKH.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Khách hàng '{maKH}' không tồn tại hoặc đã bị xóa trước đó.");

            try
            {
                return _dalKH.XoaMemKhachHang(maKH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa mềm khách hàng: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 5: QUẢN LÝ TÀI KHOẢN KHÁCH HÀNG
        // ══════════════════════════════════════════════════════════════════

        /// Lấy thông tin tài khoản của một khách hàng theo mã KH.
        /// <param name="maKH">Mã khách hàng.
        /// Trả về DTO_TaiKhoanKH nếu tìm thấy, null nếu chưa có tài khoản.
        public DTO_TaiKhoanKH? LayTaiKhoan(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            try
            {
                return _dalTKKH.DSTheoMaKH(maKH.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tài khoản khách hàng: {ex.Message}", ex);
            }
        }

        /// Khóa tài khoản khách hàng (TrangThai = 'Khóa').
        /// <param name="maTK">Mã tài khoản cần khóa.
        /// Trả về True nếu khóa thành công.
        public bool KhoaTaiKhoan(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");

            var tk = _dalTKKH.DSTheoMaTK(maTK.Trim());
            if (tk == null)
                throw new InvalidOperationException($"Tài khoản '{maTK}' không tồn tại.");
            if (tk.TrangThai == "Khóa")
                throw new InvalidOperationException($"Tài khoản '{maTK}' đã ở trạng thái khóa.");

            try
            {
                return _dalTKKH.CapNhatTrangThai(maTK.Trim(), "Khóa");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khóa tài khoản: {ex.Message}", ex);
            }
        }

        /// Mở khóa tài khoản khách hàng (TrangThai = 'Hoạt Động').
        /// <param name="maTK">Mã tài khoản cần mở khóa.
        /// Trả về True nếu mở khóa thành công.
        public bool MoKhoaTaiKhoan(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");

            var tk = _dalTKKH.DSTheoMaTK(maTK.Trim());
            if (tk == null)
                throw new InvalidOperationException($"Tài khoản '{maTK}' không tồn tại.");
            if (tk.TrangThai == "Hoạt Động")
                throw new InvalidOperationException($"Tài khoản '{maTK}' đang hoạt động bình thường.");

            try
            {
                return _dalTKKH.CapNhatTrangThai(maTK.Trim(), "Hoạt Động");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi mở khóa tài khoản: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 6: KIỂM TRA DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════

        /// Kiểm tra tính hợp lệ của thông tin khách hàng trước khi gọi DAL.
        /// Ném ArgumentException nếu dữ liệu không hợp lệ.
        /// <param name="kh">DTO_KhachHang cần kiểm tra.
        public void KiemTraHopLe(DTO_KhachHang kh)
        {
            if (kh == null)
                throw new ArgumentNullException(nameof(kh), "Thông tin khách hàng không được null.");

            if (string.IsNullOrWhiteSpace(kh.TenKH))
                throw new ArgumentException("Tên khách hàng không được để trống.");
            if (kh.TenKH.Length > 50)
                throw new ArgumentException("Tên khách hàng không được vượt quá 50 ký tự.");

            if (kh.LoaiKH != "Lẻ" && kh.LoaiKH != "Sỉ")
                throw new ArgumentException("Loại khách hàng phải là 'Lẻ' hoặc 'Sỉ'.");

            if (!string.IsNullOrEmpty(kh.Email) && !Regex.IsMatch(kh.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Địa chỉ Email không đúng định dạng.");

            if (!string.IsNullOrEmpty(kh.SDT) && !Regex.IsMatch(kh.SDT, @"^\d{10}$"))
                throw new ArgumentException("Số điện thoại phải gồm đúng 10 chữ số.");
        }
    }
}

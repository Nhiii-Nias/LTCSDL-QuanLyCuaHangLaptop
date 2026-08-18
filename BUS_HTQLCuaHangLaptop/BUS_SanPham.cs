using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ quản lý danh mục sản phẩm: HangSanXuat, LoaiSanPham, CauHinh, SanPham.
    /// Chịu trách nhiệm: Kiểm tra dữ liệu đầu vào, thêm/sửa/xóa mềm, kiểm tra ràng buộc phân cấp,
    /// và cập nhật trạng thái serial khi bán hàng / bảo hành / đổi trả.
    public class BUS_SanPham
    {
        // Khai báo các DAL liên quan
        private readonly DAL_HangSanXuat _dalHSX = new DAL_HangSanXuat();
        private readonly DAL_LoaiSanPham _dalLSP = new DAL_LoaiSanPham();
        private readonly DAL_CauHinh     _dalCH  = new DAL_CauHinh();
        private readonly DAL_SanPham     _dalSP  = new DAL_SanPham();

        // Hằng số giá trị hợp lệ
        private static readonly string[] DANH_MUC_HOP_LE   = { "Laptop", "Chuột", "Bàn Phím" };
        private static readonly string[] TRANG_THAI_HOP_LE  = { "Trong Kho", "Đã Bán", "Bảo Hành", "Lỗi", "Đổi Trả" };


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: HÃNG SẢN XUẤT (HangSanXuat)
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ hãng sản xuất chưa bị xóa mềm.
        /// Trả về DataTable chứa danh sách hãng.
        public DataTable LayDanhSachHSX()
        {
            try
            {
                return _dalHSX.DSTatCaHSX();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách hãng sản xuất: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin hãng sản xuất theo mã.
        /// <param name="maHang">Mã hãng sản xuất.
        /// Trả về DTO_HangSanXuat nếu tìm thấy, null nếu không tồn tại.
        public DTO_HangSanXuat? LayHSXTheoMa(string maHang)
        {
            if (string.IsNullOrWhiteSpace(maHang))
                throw new ArgumentException("Mã hãng sản xuất không được để trống.");
            try
            {
                return _dalHSX.DSTheoMaHSX(maHang.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin hãng sản xuất: {ex.Message}", ex);
            }
        }

        /// Thêm hãng sản xuất mới.
        /// <param name="hsx">DTO chứa thông tin hãng.
        /// Trả về True nếu thêm thành công.
        public bool ThemHSX(DTO_HangSanXuat hsx)
        {
            KiemTraHopLeHSX(hsx);

            // Kiểm tra trùng tên hãng (không cho 2 hãng cùng tên đang hoạt động)
            var dsHSX = _dalHSX.DSTatCaHSX();
            foreach (DataRow row in dsHSX.Rows)
            {
                if (string.Equals(row["TenHang"].ToString()?.Trim(), hsx.TenHang.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"Hãng '{hsx.TenHang}' đã tồn tại trong hệ thống.");
            }

            hsx.IsDeleted = false;
            try
            {
                return _dalHSX.ThemHSX(hsx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm hãng sản xuất: {ex.Message}", ex);
            }
        }

        /// Cập nhật thông tin hãng sản xuất (TenHang, QuocGia).
        /// <param name="hsx">DTO chứa thông tin cần cập nhật. MaHang bắt buộc.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatHSX(DTO_HangSanXuat hsx)
        {
            if (string.IsNullOrWhiteSpace(hsx.MaHang))
                throw new ArgumentException("Mã hãng sản xuất không được để trống khi cập nhật.");

            var existing = _dalHSX.DSTheoMaHSX(hsx.MaHang.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Hãng sản xuất '{hsx.MaHang}' không tồn tại hoặc đã bị xóa.");

            KiemTraHopLeHSX(hsx);
            try
            {
                return _dalHSX.UpdateHangSanXuat(hsx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật hãng sản xuất: {ex.Message}", ex);
            }
        }

        /// Xóa mềm hãng sản xuất (IsDeleted = 1).
        /// Không được xóa nếu còn LoaiSanPham đang IsDeleted = 0 thuộc hãng đó.
        /// <param name="maHang">Mã hãng cần xóa mềm.
        /// Trả về True nếu xóa mềm thành công.
        public bool XoaHSX(string maHang)
        {
            if (string.IsNullOrWhiteSpace(maHang))
                throw new ArgumentException("Mã hãng sản xuất không được để trống.");

            var existing = _dalHSX.DSTheoMaHSX(maHang.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Hãng sản xuất '{maHang}' không tồn tại hoặc đã bị xóa trước đó.");

            // Ràng buộc: còn LoaiSanPham đang hoạt động → không cho xóa hãng
            var dsLSP = _dalLSP.DSLoaiSPTheoHang(maHang.Trim());
            if (dsLSP.Rows.Count > 0)
                throw new InvalidOperationException(
                    $"Không thể xóa hãng '{existing.TenHang}' vì còn {dsLSP.Rows.Count} loại sản phẩm đang hoạt động thuộc hãng này. " +
                    "Hãy xóa mềm các loại sản phẩm trước.");

            try
            {
                return _dalHSX.XoaMemHSX(maHang.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa mềm hãng sản xuất: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: LOẠI SẢN PHẨM (LoaiSanPham)
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ loại sản phẩm chưa bị xóa mềm.
        /// Trả về DataTable chứa danh sách loại sản phẩm.
        public DataTable LayDanhSachLoaiSP()
        {
            try
            {
                return _dalLSP.DSLoaiSP();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách loại sản phẩm: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách loại sản phẩm theo hãng.
        /// <param name="maHang">Mã hãng sản xuất.
        /// Trả về DataTable chứa danh sách loại SP theo hãng.
        public DataTable LayLoaiSPTheoHang(string maHang)
        {
            if (string.IsNullOrWhiteSpace(maHang))
                throw new ArgumentException("Mã hãng sản xuất không được để trống.");
            try
            {
                return _dalLSP.DSLoaiSPTheoHang(maHang.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy loại sản phẩm theo hãng: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách loại sản phẩm theo danh mục.
        /// <param name="danhMuc">"Laptop", "Chuột" hoặc "Bàn Phím".
        /// Trả về DataTable chứa danh sách loại SP theo danh mục.
        public DataTable LayLoaiSPTheoDanhMuc(string danhMuc)
        {
            KiemTraDanhMuc(danhMuc);
            try
            {
                return _dalLSP.DSLoaiSPTheoDanhMuc(danhMuc);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy loại sản phẩm theo danh mục: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin loại sản phẩm theo mã.
        /// <param name="maLoaiSP">Mã loại sản phẩm.
        /// Trả về DTO_LoaiSanPham nếu tìm thấy, null nếu không tồn tại.
        public DTO_LoaiSanPham? LayLoaiSPTheoMa(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");
            try
            {
                return _dalLSP.TimLoaiSP(maLoaiSP.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin loại sản phẩm: {ex.Message}", ex);
            }
        }

        /// Thêm loại sản phẩm mới.
        /// <param name="lsp">DTO chứa thông tin loại SP.
        /// <param name="maTKNguoiTao">Mã tài khoản nhân viên thực hiện tạo.
        /// Trả về True nếu thêm thành công.
        public bool ThemLoaiSP(DTO_LoaiSanPham lsp, string? maTKNguoiTao = null)
        {
            KiemTraHopLeLoaiSP(lsp);

            // Kiểm tra hãng sản xuất tồn tại
            var hsx = _dalHSX.DSTheoMaHSX(lsp.MaHang.Trim());
            if (hsx == null)
                throw new InvalidOperationException($"Hãng sản xuất '{lsp.MaHang}' không tồn tại hoặc đã bị xóa.");

            lsp.NgayTao   = DateTime.Now;
            lsp.NguoiTao  = maTKNguoiTao?.Trim();
            lsp.IsDeleted = false;

            try
            {
                return _dalLSP.ThemLoaiSP(lsp);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm loại sản phẩm: {ex.Message}", ex);
            }
        }

        /// Cập nhật thông tin loại sản phẩm (TenLoai, DanhMuc, ThoiGianBaoHanh, GiaBanGoc).
        /// <param name="lsp">DTO chứa thông tin cần cập nhật. MaLoaiSP bắt buộc.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatLoaiSP(DTO_LoaiSanPham lsp)
        {
            if (string.IsNullOrWhiteSpace(lsp.MaLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống khi cập nhật.");

            var existing = _dalLSP.TimLoaiSP(lsp.MaLoaiSP.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Loại sản phẩm '{lsp.MaLoaiSP}' không tồn tại hoặc đã bị xóa.");

            KiemTraHopLeLoaiSP(lsp);

            // Kiểm tra hãng tồn tại
            var hsx = _dalHSX.DSTheoMaHSX(lsp.MaHang.Trim());
            if (hsx == null)
                throw new InvalidOperationException($"Hãng sản xuất '{lsp.MaHang}' không tồn tại hoặc đã bị xóa.");

            lsp.NgayCapNhat = DateTime.Now;
            try
            {
                return _dalLSP.CapNhatLoaiSP(lsp);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật loại sản phẩm: {ex.Message}", ex);
            }
        }

        /// Xóa mềm loại sản phẩm (IsDeleted = 1).
        /// Không được xóa nếu còn SanPham có TrangThai != 'Đã Bán' thuộc loại đó.
        /// <param name="maLoaiSP">Mã loại sản phẩm cần xóa mềm.
        /// Trả về True nếu xóa mềm thành công.
        public bool XoaLoaiSP(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");

            var existing = _dalLSP.TimLoaiSP(maLoaiSP.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Loại sản phẩm '{maLoaiSP}' không tồn tại hoặc đã bị xóa trước đó.");

            // Ràng buộc: còn serial chưa bán → không cho xóa loại SP
            var dsSP = _dalSP.DSTheoLoaiSP(maLoaiSP.Trim());
            int soConLai = 0;
            foreach (DataRow row in dsSP.Rows)
            {
                string tt = row["TrangThai"].ToString() ?? "";
                if (tt != "Đã Bán")
                    soConLai++;
            }
            if (soConLai > 0)
                throw new InvalidOperationException(
                    $"Không thể xóa loại sản phẩm '{existing.TenLoai}' vì còn {soConLai} sản phẩm chưa được bán " +
                    "(TrangThai khác 'Đã Bán'). Hãy xử lý hết hàng tồn kho trước.");

            try
            {
                return _dalLSP.XoaMemLoaiSP(maLoaiSP.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa mềm loại sản phẩm: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 3: CẤU HÌNH (CauHinh)
        // ══════════════════════════════════════════════════════════════════

        /// Lấy toàn bộ danh sách cấu hình.
        /// Trả về DataTable chứa toàn bộ cấu hình.
        public DataTable LayTatCaCauHinh()
        {
            try
            {
                return _dalCH.DSTatCaCauHinh();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy toàn bộ danh sách cấu hình sản phẩm: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách cấu hình theo loại sản phẩm.
        /// <param name="maLoaiSP">Mã loại sản phẩm.
        /// Trả về DataTable chứa danh sách cấu hình.
        public DataTable LayCauHinhTheoLoaiSP(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");
            try
            {
                return _dalCH.DSCauHinhTheoLoaiSP(maLoaiSP.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy cấu hình sản phẩm: {ex.Message}", ex);
            }
        }

        /// Thêm cấu hình mới cho loại sản phẩm.
        /// <param name="ch">DTO chứa thông tin cấu hình.
        /// Trả về True nếu thêm thành công.
        public bool ThemCauHinh(DTO_CauHinh ch)
        {
            if (string.IsNullOrWhiteSpace(ch.MaCauHinh))
                throw new ArgumentException("Mã cấu hình không được để trống.");
            if (string.IsNullOrWhiteSpace(ch.MaLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(ch.TenThuocTinh))
                throw new ArgumentException("Tên thuộc tính cấu hình không được để trống.");
            if (ch.TenThuocTinh.Length > 150)
                throw new ArgumentException("Tên thuộc tính không được vượt quá 150 ký tự.");

            // Kiểm tra loại SP tồn tại
            var lsp = _dalLSP.TimLoaiSP(ch.MaLoaiSP.Trim());
            if (lsp == null)
                throw new InvalidOperationException($"Loại sản phẩm '{ch.MaLoaiSP}' không tồn tại hoặc đã bị xóa.");

            try
            {
                return _dalCH.ThemCauHinh(ch);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm cấu hình: {ex.Message}", ex);
            }
        }

        /// Cập nhật cấu hình sản phẩm.
        /// <param name="ch">DTO chứa thông tin cần cập nhật. MaCauHinh bắt buộc.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatCauHinh(DTO_CauHinh ch)
        {
            if (string.IsNullOrWhiteSpace(ch.MaCauHinh))
                throw new ArgumentException("Mã cấu hình không được để trống khi cập nhật.");
            if (string.IsNullOrWhiteSpace(ch.TenThuocTinh))
                throw new ArgumentException("Tên thuộc tính cấu hình không được để trống.");

            var existing = _dalCH.DSTheoMaCauHinh(ch.MaCauHinh.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Cấu hình '{ch.MaCauHinh}' không tồn tại.");

            try
            {
                return _dalCH.CapNhatCauHinh(ch);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật cấu hình: {ex.Message}", ex);
            }
        }

        /// Xóa vật lý cấu hình (CauHinh không phải bảng master — không cần xóa mềm).
        /// <param name="maCauHinh">Mã cấu hình cần xóa.
        /// Trả về True nếu xóa thành công.
        public bool XoaCauHinh(string maCauHinh)
        {
            if (string.IsNullOrWhiteSpace(maCauHinh))
                throw new ArgumentException("Mã cấu hình không được để trống.");

            var existing = _dalCH.DSTheoMaCauHinh(maCauHinh.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Cấu hình '{maCauHinh}' không tồn tại.");

            try
            {
                return _dalCH.XoaCauHinh(maCauHinh.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa cấu hình: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 4: SẢN PHẨM VẬT LÝ / SERIAL (SanPham)
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ sản phẩm chưa bị xóa mềm.
        /// Trả về DataTable chứa danh sách sản phẩm.
        public DataTable LayDanhSachSanPham()
        {
            try
            {
                return _dalSP.DSTatCaSanPham();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách sản phẩm: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách sản phẩm đang tồn kho (TrangThai = 'Trong Kho').
        /// Trả về DataTable chứa danh sách sản phẩm tồn kho.
        public DataTable LayDanhSachTonKho()
        {
            try
            {
                return _dalSP.DSTheoTrangThai("Trong Kho");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách tồn kho: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách sản phẩm theo trạng thái.
        /// <param name="trangThai">"Trong Kho" | "Đã Bán" | "Bảo Hành" | "Lỗi" | "Đổi Trả".
        /// Trả về DataTable chứa danh sách sản phẩm.
        public DataTable LayDanhSachTheoTrangThai(string trangThai)
        {
            KiemTraTrangThaiSP(trangThai);
            try
            {
                return _dalSP.DSTheoTrangThai(trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách sản phẩm theo trạng thái: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách sản phẩm theo loại sản phẩm.
        /// <param name="maLoaiSP">Mã loại sản phẩm.
        /// Trả về DataTable chứa danh sách sản phẩm.
        public DataTable LayDanhSachTheoLoaiSP(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");
            try
            {
                return _dalSP.DSTheoLoaiSP(maLoaiSP.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách sản phẩm theo loại: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin sản phẩm theo số serial.
        /// <param name="maSerial">Số serial sản phẩm (VARCHAR 50).
        /// Trả về DTO_SanPham nếu tìm thấy, null nếu không tồn tại.
        public DTO_SanPham? LayTheoSerial(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Số serial sản phẩm không được để trống.");
            try
            {
                return _dalSP.DSTheoMaSerialSP(maSerial.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin sản phẩm: {ex.Message}", ex);
            }
        }

        /// Thêm sản phẩm mới (ghi nhận serial nhập kho).
        /// MaSerialSP là PRIMARY KEY — validate không rỗng, không trùng.
        /// TrangThai mặc định 'Trong Kho' khi mới nhập.
        /// <param name="sp">DTO chứa thông tin sản phẩm.
        /// Trả về True nếu thêm thành công.
        public bool ThemSanPham(DTO_SanPham sp)
        {
            KiemTraHopLeSanPham(sp);

            // Kiểm tra loại SP tồn tại
            var lsp = _dalLSP.TimLoaiSP(sp.MaLoaiSP.Trim());
            if (lsp == null)
                throw new InvalidOperationException($"Loại sản phẩm '{sp.MaLoaiSP}' không tồn tại hoặc đã bị xóa.");

            // Kiểm tra serial không trùng (bao gồm cả serial đã xóa mềm — PK không được phép tái sử dụng)
            var existing = _dalSP.DSTheoMaSerialSP(sp.MaSerialSP.Trim());
            if (existing != null)
                throw new InvalidOperationException($"Serial '{sp.MaSerialSP}' đã tồn tại trong hệ thống.");

            // Mặc định trạng thái khi nhập kho
            if (string.IsNullOrWhiteSpace(sp.TrangThai))
                sp.TrangThai = "Trong Kho";

            sp.NgayTao   = DateTime.Now;
            sp.IsDeleted = false;

            try
            {
                bool success = _dalSP.ThemSanPham(sp);
                if (success && sp.TrangThai == "Trong Kho")
                {
                    // Check if there is a pending virtual serial for this MaLoaiSP
                    string connStr = ConfigurationManager.ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        using (SqlTransaction tran = conn.BeginTransaction())
                        {
                            try
                            {
                                string sqlFindVirtual = "SELECT TOP 1 ct.MaSerialSP, ct.MaDH FROM ChiTietDonHang ct " +
                                                        "INNER JOIN SanPham sp ON ct.MaSerialSP = sp.MaSerialSP " +
                                                        "WHERE sp.MaLoaiSP = @MaLoaiSP AND ct.MaSerialSP LIKE 'x-%'";
                                string? virtualSerial = null;
                                string? maDH = null;
                                using (SqlCommand cmdFind = new SqlCommand(sqlFindVirtual, conn, tran))
                                {
                                    cmdFind.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = sp.MaLoaiSP });
                                    using (SqlDataReader reader = cmdFind.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            virtualSerial = reader["MaSerialSP"].ToString()!.Trim();
                                            maDH = reader["MaDH"].ToString()!.Trim();
                                        }
                                    }
                                }

                                if (virtualSerial != null && maDH != null)
                                {
                                    // 1. UPDATE TrangThai of the actual product to N'Đã Bán' (and set NgayCapNhat)
                                    string sqlUpdateActual = "UPDATE SanPham SET TrangThai = N'Đã Bán', NgayCapNhat = GETDATE() " +
                                                             "WHERE MaSerialSP = @MaSerialSP AND IsDeleted = 0";
                                    using (SqlCommand cmdAct = new SqlCommand(sqlUpdateActual, conn, tran))
                                    {
                                        cmdAct.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = sp.MaSerialSP });
                                        cmdAct.ExecuteNonQuery();
                                    }

                                    // 2. UPDATE ChiTietDonHang.MaSerialSP to the new actual serial
                                    string sqlUpdateCT = "UPDATE ChiTietDonHang SET MaSerialSP = @MaSerialSP " +
                                                         "WHERE MaDH = @MaDH AND MaSerialSP = @VirtualSerial";
                                    using (SqlCommand cmdCT = new SqlCommand(sqlUpdateCT, conn, tran))
                                    {
                                        cmdCT.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = sp.MaSerialSP });
                                        cmdCT.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });
                                        cmdCT.Parameters.Add(new SqlParameter("@VirtualSerial", SqlDbType.VarChar, 50) { Value = virtualSerial });
                                        cmdCT.ExecuteNonQuery();
                                    }

                                    // 3. DELETE the temporary virtual serial from SanPham table
                                    string sqlDeleteVirtual = "DELETE FROM SanPham WHERE MaSerialSP = @VirtualSerial";
                                    using (SqlCommand cmdDel = new SqlCommand(sqlDeleteVirtual, conn, tran))
                                    {
                                        cmdDel.Parameters.Add(new SqlParameter("@VirtualSerial", SqlDbType.VarChar, 50) { Value = virtualSerial });
                                        cmdDel.ExecuteNonQuery();
                                    }
                                }
                                tran.Commit();
                            }
                            catch
                            {
                                tran.Rollback();
                                // We don't throw to not rollback the actual insert of the SanPham since that succeeded
                            }
                        }
                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm sản phẩm: {ex.Message}", ex);
            }
        }

        /// Cập nhật thông tin sản phẩm (NgaySX, MaLoaiSP, NgayNhap).
        /// Không cho phép đổi MaSerialSP hoặc MaPhieuNhap sau khi tạo.
        /// <param name="sp">DTO chứa thông tin cần cập nhật. MaSerialSP bắt buộc.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhat(DTO_SanPham sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSerialSP))
                throw new ArgumentException("Số serial sản phẩm không được để trống khi cập nhật.");

            var existing = _dalSP.DSTheoMaSerialSP(sp.MaSerialSP.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Sản phẩm serial '{sp.MaSerialSP}' không tồn tại hoặc đã bị xóa.");

            KiemTraHopLeSanPham(sp);

            // Giữ nguyên phiếu nhập gốc, không cho đổi
            sp.MaPhieuNhap  = existing.MaPhieuNhap;
            sp.NgayCapNhat  = DateTime.Now;

            try
            {
                return _dalSP.CapNhatSanPham(sp);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật sản phẩm: {ex.Message}", ex);
            }
        }

        /// Cập nhật trạng thái serial — được gọi từ BUS_DonHang, BUS_PhieuBaoHanh, BUS_PhieuDoiTra.
        /// Đây là điểm tập trung duy nhất để thay đổi TrangThai của serial.
        /// <param name="maSerial">Số serial cần cập nhật.
        /// <param name="trangThaiMoi">"Trong Kho" | "Đã Bán" | "Bảo Hành" | "Lỗi" | "Đổi Trả".
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatTrangThaiSerial(string maSerial, string trangThaiMoi)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Số serial sản phẩm không được để trống.");

            KiemTraTrangThaiSP(trangThaiMoi);

            var sp = _dalSP.DSTheoMaSerialSP(maSerial.Trim());
            if (sp == null)
                throw new InvalidOperationException($"Sản phẩm serial '{maSerial}' không tồn tại hoặc đã bị xóa.");

            // Ngăn cập nhật trùng trạng thái (tránh ghi log vô nghĩa)
            if (sp.TrangThai == trangThaiMoi)
                return true;

            try
            {
                return _dalSP.CapNhatTrangThai(maSerial.Trim(), trangThaiMoi);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật trạng thái serial '{maSerial}': {ex.Message}", ex);
            }
        }

        /// Xóa mềm sản phẩm (IsDeleted = 1, TrangThai = 'Lỗi').
        /// Chỉ được xóa serial có TrangThai = 'Trong Kho' (chưa bán, chưa bảo hành).
        /// <param name="maSerial">Số serial cần xóa mềm.
        /// Trả về True nếu xóa mềm thành công.
        public bool Xoa(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Số serial sản phẩm không được để trống.");

            var sp = _dalSP.DSTheoMaSerialSP(maSerial.Trim());
            if (sp == null)
                throw new InvalidOperationException($"Sản phẩm serial '{maSerial}' không tồn tại hoặc đã bị xóa trước đó.");

            if (sp.TrangThai != "Trong Kho")
                throw new InvalidOperationException(
                    $"Không thể xóa sản phẩm serial '{maSerial}' vì đang ở trạng thái '{sp.TrangThai}'. " +
                    "Chỉ được xóa sản phẩm ở trạng thái 'Trong Kho'.");

            try
            {
                return _dalSP.XoaMemSanPham(maSerial.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa mềm sản phẩm: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 5: KIỂM TRA DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════

        /// Kiểm tra thông tin hãng sản xuất hợp lệ.
        public void KiemTraHopLeHSX(DTO_HangSanXuat hsx)
        {
            if (hsx == null)
                throw new ArgumentNullException(nameof(hsx), "Thông tin hãng sản xuất không được null.");
            if (string.IsNullOrWhiteSpace(hsx.TenHang))
                throw new ArgumentException("Tên hãng sản xuất không được để trống.");
            if (hsx.TenHang.Length > 100)
                throw new ArgumentException("Tên hãng sản xuất không được vượt quá 100 ký tự.");
        }

        /// Kiểm tra thông tin loại sản phẩm hợp lệ.
        public void KiemTraHopLeLoaiSP(DTO_LoaiSanPham lsp)
        {
            if (lsp == null)
                throw new ArgumentNullException(nameof(lsp), "Thông tin loại sản phẩm không được null.");

            if (string.IsNullOrWhiteSpace(lsp.MaHang))
                throw new ArgumentException("Mã hãng sản xuất không được để trống.");
            if (string.IsNullOrWhiteSpace(lsp.TenLoai))
                throw new ArgumentException("Tên loại sản phẩm không được để trống.");
            if (lsp.TenLoai.Length > 200)
                throw new ArgumentException("Tên loại sản phẩm không được vượt quá 200 ký tự.");

            KiemTraDanhMuc(lsp.DanhMuc);

            if (lsp.ThoiGianBaoHanh <= 0)
                throw new ArgumentException("Thời gian bảo hành phải lớn hơn 0 (đơn vị: tháng).");
            if (lsp.GiaBanGoc < 0)
                throw new ArgumentException("Giá bán gốc không được âm.");
        }

        /// Kiểm tra thông tin sản phẩm vật lý hợp lệ.
        public void KiemTraHopLeSanPham(DTO_SanPham sp)
        {
            if (sp == null)
                throw new ArgumentNullException(nameof(sp), "Thông tin sản phẩm không được null.");

            if (string.IsNullOrWhiteSpace(sp.MaSerialSP))
                throw new ArgumentException("Số serial sản phẩm không được để trống.");
            if (sp.MaSerialSP.Length > 50)
                throw new ArgumentException("Số serial không được vượt quá 50 ký tự.");

            if (string.IsNullOrWhiteSpace(sp.MaLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(sp.MaPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");

            if (sp.NgayNhap == default(DateTime))
                throw new ArgumentException("Ngày nhập không hợp lệ.");
            if (sp.NgayNhap > DateTime.Today)
                throw new ArgumentException("Ngày nhập không được là ngày trong tương lai.");

            if (!string.IsNullOrWhiteSpace(sp.TrangThai))
                KiemTraTrangThaiSP(sp.TrangThai);
        }

        /// Kiểm tra giá trị DanhMuc hợp lệ.
        /// <param name="danhMuc">Giá trị cần kiểm tra.
        public void KiemTraDanhMuc(string danhMuc)
        {
            if (string.IsNullOrWhiteSpace(danhMuc))
                throw new ArgumentException("Danh mục sản phẩm không được để trống.");

            bool hopLe = false;
            foreach (var dm in DANH_MUC_HOP_LE)
                if (dm == danhMuc) { hopLe = true; break; }

            if (!hopLe)
                throw new ArgumentException($"Danh mục '{danhMuc}' không hợp lệ. Chỉ nhận: 'Laptop', 'Chuột', 'Bàn Phím'.");
        }

        /// Kiểm tra giá trị TrangThai sản phẩm hợp lệ.
        /// <param name="trangThai">Giá trị cần kiểm tra.
        public void KiemTraTrangThaiSP(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                throw new ArgumentException("Trạng thái sản phẩm không được để trống.");

            bool hopLe = false;
            foreach (var tt in TRANG_THAI_HOP_LE)
                if (tt == trangThai) { hopLe = true; break; }

            if (!hopLe)
                throw new ArgumentException(
                    $"Trạng thái '{trangThai}' không hợp lệ. " +
                    "Chỉ nhận: 'Trong Kho', 'Đã Bán', 'Bảo Hành', 'Lỗi', 'Đổi Trả'.");
        }
    }
}

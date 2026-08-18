using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ xử lý tài khoản đăng nhập của cả nhân viên (WinForm) và khách hàng (Website).
    /// Chịu trách nhiệm: hash mật khẩu, xác thực đăng nhập, quản lý trạng thái tài khoản, và ghi lịch sử đăng nhập vào bảng LichSuDangNhap.
    public class BUS_TaiKhoan   
    {
        // Gọi DAL xử lý dữ liệu
        private readonly DAL_TaiKhoanNV _dalNV = new DAL_TaiKhoanNV();
        private readonly DAL_TaiKhoanKH _dalKH = new DAL_TaiKhoanKH();
        private readonly DAL_LichSuDangNhap _dalLSDN = new DAL_LichSuDangNhap();
        private readonly DAL_KhachHang _dalKhachHang = new DAL_KhachHang();
        private readonly DAL_VaiTro _dalVaiTro = new DAL_VaiTro();



        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: TIỆN ÍCH MẬT KHẨU
        // ══════════════════════════════════════════════════════════════════

        /// Hash mật khẩu bằng SHA-256. Không bao giờ lưu mật khẩu dạng plaintext vào CSDL.
        /// <param name="matKhauGoc">Mật khẩu gốc (plaintext).
        /// Trả về chuỗi hash SHA-256 dạng hex (64 ký tự).
        public string HashMatKhau(string matKhauGoc)
        {
            // Kiểm tra mật khẩu không được để trống
            if (string.IsNullOrWhiteSpace(matKhauGoc))
                throw new ArgumentException("Mật khẩu không được để trống.");

            // Tạo chuỗi hash SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(matKhauGoc);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder(); //Dùng để nối các chuỗi hash lại với nhau
                // Chuyển đổi byte hash thành chuỗi hex
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// Kiểm tra mật khẩu gốc có khớp với hash đã lưu không.
        /// <param name="matKhauGoc">Mật khẩu người dùng nhập vào.
        /// <param name="hashDaLuu">Hash đang lưu trong CSDL.
        /// Trả vềTrue nếu khớp, False nếu không khớp.
        public bool XacNhanMatKhau(string matKhauGoc, string hashDaLuu)
        {
            if (string.IsNullOrWhiteSpace(matKhauGoc) || string.IsNullOrWhiteSpace(hashDaLuu))
                return false;

            // Hỗ trợ cả trường hợp lưu mật khẩu thô (plaintext) trong DB để phục vụ kiểm tra dễ dàng
            if (string.Equals(matKhauGoc, hashDaLuu, StringComparison.Ordinal))
                return true;

            string hashNhap = HashMatKhau(matKhauGoc);
            return string.Equals(hashNhap, hashDaLuu, StringComparison.OrdinalIgnoreCase);
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: XÁC THỰC ĐĂNG NHẬP — NHÂN VIÊN (WinForm)
        // ══════════════════════════════════════════════════════════════════

        
        /// Xác thực đăng nhập cho nhân viên (WinForm). Ghi lịch sử đăng nhập dù thành công hay thất bại.
        
        /// <param name="tenDangNhap">Tên đăng nhập của nhân viên.
        /// <param name="matKhauGoc">Mật khẩu gốc (plaintext) nhập vào.
        /// <param name="diaChiIP">Địa chỉ IP của máy đăng nhập (có thể null).
        /// Trả vềDTO_TaiKhoanNV nếu đăng nhập thành công, null nếu thất bại.
        public DTO_TaiKhoanNV? DangNhapNV(string tenDangNhap, string matKhauGoc, string? diaChiIP = null)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            if (string.IsNullOrWhiteSpace(matKhauGoc))
                throw new ArgumentException("Mật khẩu không được để trống.");

            try
            {
                DTO_TaiKhoanNV? tk = _dalNV.DSTheoTenDangNhap(tenDangNhap.Trim());

                // Tài khoản không tồn tại
                if (tk == null)
                {
                    return null;
                }

                // Tài khoản bị khóa — ghi log thất bại và trả về null
                if (tk.TrangThai == "Khóa")
                {
                    GhiLichSuDangNhapNV(tk.MaTK, diaChiIP, "Thất Bại");
                    return null;
                }

                // Kiểm tra mật khẩu
                if (!XacNhanMatKhau(matKhauGoc, tk.MatKhau))
                {
                    GhiLichSuDangNhapNV(tk.MaTK, diaChiIP, "Thất Bại");
                    return null;
                }

                // Đăng nhập thành công — ghi log thành công
                GhiLichSuDangNhapNV(tk.MaTK, diaChiIP, "Thành Công");
                return tk;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xác thực đăng nhập nhân viên: {ex.Message}", ex);
            }
        }

        
        /// Ghi một bản ghi vào LichSuDangNhap cho tài khoản nhân viên.
        /// Được gọi nội bộ từ DangNhapNV — không gọi trực tiếp từ GUI.
        
        private void GhiLichSuDangNhapNV(string maTK, string? diaChiIP, string trangThai)
        {
            try
            {
                var ls = new DTO_LichSuDangNhap
                {
                    MaLSDN = TaoMaLSDN(),
                    MaTK = maTK,
                    DiaChiIP = diaChiIP ?? string.Empty,
                    TrangThai = trangThai
                };
                _dalLSDN.ThemLichSuDangNhap(ls);
            }
            catch
            {
                // Không để lỗi ghi log làm gián đoạn luồng đăng nhập
            }
        }

        
        /// Tạo mã lịch sử đăng nhập tự động theo định dạng "LSDN" + timestamp ngắn.
        
        private string TaoMaLSDN()
        {
            return "LS" + DateTime.Now.ToString("yyMMddHHmm").Substring(0, 8);
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 3: XÁC THỰC ĐĂNG NHẬP — KHÁCH HÀNG (Website)
        // ══════════════════════════════════════════════════════════════════

        
        /// Xác thực đăng nhập cho khách hàng (Website MVC).
        /// Không ghi LichSuDangNhap (bảng này chỉ dùng cho nhân viên theo).
        
        /// <param name="tenDangNhap">Tên đăng nhập khách hàng.
        /// <param name="matKhauGoc">Mật khẩu gốc (plaintext).
        /// Trả về DTO_TaiKhoanKH nếu thành công, null nếu thất bại.
        public DTO_TaiKhoanKH? DangNhapKH(string tenDangNhap, string matKhauGoc)
        {
            // Kiểm tra tên đăng nhập không được để trống
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            // Kiểm tra mật khẩu không được để trống
            if (string.IsNullOrWhiteSpace(matKhauGoc))
                throw new ArgumentException("Mật khẩu không được để trống.");
            //
            try
            {
                // Lấy danh sách tài khoản khách hàng theo tên đăng nhập
                DTO_TaiKhoanKH? tk = _dalKH.DSTheoTenDangNhap(tenDangNhap.Trim());

                // Tài khoản không tồn tại
                if (tk == null)
                    return null;

                // Tài khoản bị khóa
                if (tk.TrangThai == "Khóa")
                    return null;

                // Kiểm tra mật khẩu
                if (!XacNhanMatKhau(matKhauGoc, tk.MatKhau))
                    return null;

                return tk;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xác thực đăng nhập khách hàng: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 4: QUẢN LÝ TÀI KHOẢN NHÂN VIÊN
        // ══════════════════════════════════════════════════════════════════

        
        /// Lấy danh sách toàn bộ tài khoản nhân viên.
        /// Trả về DataTable chứa danh sách tài khoản.
        public DataTable LayDanhSachTaiKhoanNV()
        {
            try
            {
                return _dalNV.DSTatCaTaiKhoanNV();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách tài khoản nhân viên: {ex.Message}", ex);
            }
        }

        
        /// Lấy thông tin tài khoản nhân viên theo mã tài khoản.
        /// <param name="maTK">Mã tài khoản cần tìm.
        /// Trả về DTO_TaiKhoanNV nếu tìm thấy, null nếu không.
        public DTO_TaiKhoanNV? LayTaiKhoanNVTheoMa(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            try
            {
                return _dalNV.DSTheoMaTK(maTK.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin tài khoản nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy vai trò theo mã vai trò.
        /// </summary>
        public DTO_VaiTro? LayVaiTroTheoMa(string maVaiTro)
        {
            if (string.IsNullOrWhiteSpace(maVaiTro))
                return null;
            try
            {
                return _dalVaiTro.DSTheoMaVaiTro(maVaiTro.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy vai trò: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin tài khoản nhân viên theo tên đăng nhập.
        /// </summary>
        public DTO_TaiKhoanNV? LayTaiKhoanNVTheoTenDangNhap(string tenDangNhap)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                return null;
            try
            {
                return _dalNV.DSTheoTenDangNhap(tenDangNhap.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tài khoản nhân viên theo tên đăng nhập: {ex.Message}", ex);
            }
        }


        
        /// Thêm tài khoản nhân viên mới. Mật khẩu sẽ được hash SHA-256 trước khi lưu.
        /// <param name="tk">DTO chứa thông tin tài khoản. MatKhau phải là mật khẩu gốc.
        /// Trả về True nếu thêm thành công.
        public bool ThemTaiKhoanNV(DTO_TaiKhoanNV tk)
        {
            if (string.IsNullOrWhiteSpace(tk.MaTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.MaNV))
                throw new ArgumentException("Mã nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.MaVaiTro))
                throw new ArgumentException("Mã vai trò không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.MatKhau))
                throw new ArgumentException("Mật khẩu không được để trống.");
            if (tk.MatKhau.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");

            // Kiểm tra trùng tên đăng nhập
            var existing = _dalNV.DSTheoTenDangNhap(tk.TenDangNhap.Trim());
            if (existing != null)
                throw new InvalidOperationException($"Tên đăng nhập '{tk.TenDangNhap}' đã tồn tại.");

            try
            {
                // Hash mật khẩu trước khi gọi DAL
                tk.MatKhau = HashMatKhau(tk.MatKhau);
                tk.TrangThai = string.IsNullOrWhiteSpace(tk.TrangThai) ? "Hoạt Động" : tk.TrangThai;
                tk.NgayTao = DateTime.Now;
                return _dalNV.ThemTaiKhoanNV(tk);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm tài khoản nhân viên: {ex.Message}", ex);
            }
        }

        
        /// Đổi mật khẩu tài khoản nhân viên. Kiểm tra mật khẩu cũ trước khi cho phép đổi.
        /// <param name="maTK">Mã tài khoản cần đổi mật khẩu.
        /// <param name="matKhauCu">Mật khẩu cũ (plaintext) để xác nhận danh tính.
        /// <param name="matKhauMoi">Mật khẩu mới (plaintext).
        /// Trả về True nếu đổi thành công.
        public bool DoiMatKhauNV(string maTK, string matKhauCu, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            if (string.IsNullOrWhiteSpace(matKhauCu))
                throw new ArgumentException("Mật khẩu cũ không được để trống.");
            if (string.IsNullOrWhiteSpace(matKhauMoi))
                throw new ArgumentException("Mật khẩu mới không được để trống.");
            if (matKhauMoi.Length < 6)
                throw new ArgumentException("Mật khẩu mới phải có ít nhất 6 ký tự.");
            
            //Kiểm tra mật khẩu cũ
            try
            {
                DTO_TaiKhoanNV? tk = _dalNV.DSTheoMaTK(maTK.Trim());
                if (tk == null)
                    throw new InvalidOperationException("Tài khoản không tồn tại.");

                if (!XacNhanMatKhau(matKhauCu, tk.MatKhau))
                    throw new InvalidOperationException("Mật khẩu cũ không chính xác.");

                tk.MatKhau = HashMatKhau(matKhauMoi);
                tk.NgayCapNhat = DateTime.Now;
                return _dalNV.CapNhatTaiKhoanNV(tk);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đổi mật khẩu: {ex.Message}", ex);
            }
        }

        
        /// Khóa hoặc mở khóa tài khoản nhân viên.
        /// <param name="maTK">Mã tài khoản cần thay đổi trạng thái.
        /// <param name="trangThai">Trạng thái mới: "Hoạt Động" hoặc "Khóa".
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatTrangThaiNV(string maTK, string trangThai)
        {
            //Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            if (trangThai != "Hoạt Động" && trangThai != "Khóa")
                throw new ArgumentException("Trạng thái phải là 'Hoạt Động' hoặc 'Khóa'.");
            //Kiểm tra tài khoản có tồn tại
            try
            {
                return _dalNV.CapNhatTrangThai(maTK.Trim(), trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật trạng thái tài khoản: {ex.Message}", ex);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // PHẦN 4b: SINH MÃ TÀI KHOẢN NHÂN VIÊN
        // ══════════════════════════════════════════════════════════════════

        /// Sinh mã tài khoản nhân viên mới theo định dạng TKNV000001, TKNV000002, ... tự động tăng tiến.
        public string TaoMaTKNVMoi()
        {
            string? maMax = _dalNV.LayMaTKNVMoiNhat();
            int soTiepTheo = 1;
            if (!string.IsNullOrWhiteSpace(maMax) && maMax.StartsWith("TKNV") && maMax.Length == 10)
            {
                if (int.TryParse(maMax.Substring(4), out int soHienTai))
                    soTiepTheo = soHienTai + 1;
            }
            return "TKNV" + soTiepTheo.ToString().PadLeft(6, '0');
        }

        /// Lấy danh sách nhân viên chưa có tài khoản (để bind vào combobox).
        public DataTable LayDanhSachNVChuaCoTaiKhoan()
        {
            try
            {
                return _dalNV.LayDanhSachNVChuaCoTaiKhoan();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách nhân viên chưa có tài khoản: {ex.Message}", ex);
            }
        }

        /// Lấy toàn bộ danh sách vai trò (để bind vào combobox).
        public DataTable LayDanhSachVaiTro()
        {
            try
            {
                return _dalVaiTro.DSTatCaVaiTro();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách vai trò: {ex.Message}", ex);
            }
        }

        /// Cập nhật tên và mô tả quyền của vai trò (chỉ dành cho admin).
        public bool CapNhatVaiTro(DTO_VaiTro vt)
        {
            if (string.IsNullOrWhiteSpace(vt.MaVaiTro))
                throw new ArgumentException("Mã vai trò không được để trống.");
            if (string.IsNullOrWhiteSpace(vt.TenVaiTro))
                throw new ArgumentException("Tên vai trò không được để trống.");
            try
            {
                return _dalVaiTro.CapNhatVaiTro(vt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật vai trò: {ex.Message}", ex);
            }
        }

        /// Cập nhật tài khoản NV (vai trò, trạng thái, tên đăng nhập, mật khẩu).
        public bool CapNhatTaiKhoanNV(DTO_TaiKhoanNV tk)
        {
            if (string.IsNullOrWhiteSpace(tk.MaTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.MaVaiTro))
                throw new ArgumentException("Mã vai trò không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            if (trangThai_HopLe(tk.TrangThai) == false)
                throw new ArgumentException("Trạng thái phải là 'Hoạt Động' hoặc 'Khóa'.");

            // Kiểm tra trùng tên đăng nhập cho tài khoản khác
            var existing = _dalNV.DSTheoTenDangNhap(tk.TenDangNhap.Trim());
            if (existing != null && existing.MaTK != tk.MaTK)
                throw new InvalidOperationException($"Tên đăng nhập '{tk.TenDangNhap}' đã tồn tại ở tài khoản khác.");

            try
            {
                tk.NgayCapNhat = DateTime.Now;
                return _dalNV.CapNhatTaiKhoanNV(tk);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật tài khoản nhân viên: {ex.Message}", ex);
            }
        }

        private bool trangThai_HopLe(string trangThai)
            => trangThai == "Hoạt Động" || trangThai == "Khóa";

        // ══════════════════════════════════════════════════════════════════
        // PHẦN 5: SINH MÃ TỰ ĐỘNG KHÁCH HÀNG
        // ══════════════════════════════════════════════════════════════════

        /// Sinh mã khách hàng lᮻ mới theo định dạng KH00000001, KH00000002, ... tự động tăng tiến.
        /// Query MAX(MaKH LIKE 'KH%') từ DB, parse số nguyên, cộng 1, format lại thành chuỗi 10 ký tự.
        /// Trả về mã mới chưa tồn tại trong DB (ví dụ "KH00000007").
        public string TaoMaKHLeMoi()
        {
            string? maMax = _dalKhachHang.LayMaKHLeMoiNhat();
            int soTiepTheo = 1;
            if (!string.IsNullOrWhiteSpace(maMax) && maMax.StartsWith("KH") && maMax.Length == 10)
            {
                if (int.TryParse(maMax.Substring(2), out int soHienTai))
                    soTiepTheo = soHienTai + 1;
            }
            return "KH" + soTiepTheo.ToString().PadLeft(8, '0');
        }

        /// Sinh mã khách hàng sỉ mới theo định dạng DN00000001, DN00000002, ... tự động tăng tiến.
        /// Query MAX(MaKH LIKE 'DN%') từ DB, parse số nguyên, cộng 1, format lại thành chuỗi 10 ký tự.
        /// Trả về mã mới chưa tồn tại trong DB (ví dụ "DN00000010").
        public string TaoMaKHSiMoi()
        {
            string? maMax = _dalKhachHang.LayMaKHSiMoiNhat();
            int soTiepTheo = 1;
            if (!string.IsNullOrWhiteSpace(maMax) && maMax.StartsWith("DN") && maMax.Length == 10)
            {
                if (int.TryParse(maMax.Substring(2), out int soHienTai))
                    soTiepTheo = soHienTai + 1;
            }
            return "DN" + soTiepTheo.ToString().PadLeft(8, '0');
        }

        /// Sinh mã tài khoản khách hàng mới theo định dạng TKKH000001, TKKH000002, ... tự động tăng tiến.
        /// Query MAX(MaTK) từ TaiKhoanKH, parse số nguyên, cộng 1, format lại thành chuỗi 10 ký tự.
        /// Trả về mã mới chưa tồn tại trong DB (ví dụ "TKKH000007").
        public string TaoMaTKKHMoi()
        {
            string? maMax = _dalKH.LayMaTKKHMoiNhat();
            int soTiepTheo = 1;
            if (!string.IsNullOrWhiteSpace(maMax) && maMax.StartsWith("TKKH") && maMax.Length == 10)
            {
                if (int.TryParse(maMax.Substring(4), out int soHienTai))
                    soTiepTheo = soHienTai + 1;
            }
            return "TKKH" + soTiepTheo.ToString().PadLeft(6, '0');
        }

        // ══════════════════════════════════════════════════════════════════
        // PHẦN 6: QUẢN LÝ TÀI KHOẢN KHÁCH HÀNG
        // ══════════════════════════════════════════════════════════════════

        
        /// Lấy danh sách toàn bộ tài khoản khách hàng.
        
        /// Trả vềDataTable chứa danh sách tài khoản.
        public DataTable LayDanhSachTaiKhoanKH()
        {
            try
            {
                return _dalKH.DSTatCaTaiKhoanKH();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách tài khoản khách hàng: {ex.Message}", ex);
            }
        }

        
        /// Đăng ký tài khoản mới cho khách hàng (Website). Mật khẩu sẽ được hash trước khi lưu.
        
        /// <param name="tk">DTO chứa thông tin tài khoản. MatKhau phải là mật khẩu gốc.
        /// Trả về True nếu đăng ký thành công.
        public bool DangKyTaiKhoanKH(DTO_TaiKhoanKH tk)
        {
            // MaTK luôn được sinh tự động
            tk.MaTK = TaoMaTKKHMoi();
            // Lưu ý: MaKH phải được gán từ BUS_KhachHang trước khi gọi hàm này
            if (string.IsNullOrWhiteSpace(tk.MaKH))
                throw new ArgumentException("Mã khách hàng không được để trống. Hãy tạo khách hàng trước khi đăng ký tài khoản.");
            if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.MatKhau))
                throw new ArgumentException("Mật khẩu không được để trống.");
            if (tk.MatKhau.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");

            // Kiểm tra trùng tên đăng nhập
            var existingByTen = _dalKH.DSTheoTenDangNhap(tk.TenDangNhap.Trim());
            if (existingByTen != null)
                throw new InvalidOperationException($"Tên đăng nhập '{tk.TenDangNhap}' đã tồn tại.");

            // Kiểm tra khách hàng đã có tài khoản chưa
            var existingByKH = _dalKH.DSTheoMaKH(tk.MaKH.Trim());
            if (existingByKH != null)
                throw new InvalidOperationException($"Khách hàng '{tk.MaKH}' đã có tài khoản.");

            try
            {
                tk.MatKhau = HashMatKhau(tk.MatKhau);
                tk.TrangThai = string.IsNullOrWhiteSpace(tk.TrangThai) ? "Hoạt Động" : tk.TrangThai;
                tk.NgayTao = DateTime.Now;
                return _dalKH.ThemTaiKhoanKH(tk);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đăng ký tài khoản khách hàng: {ex.Message}", ex);
            }
        }

        
        /// Đổi mật khẩu tài khoản khách hàng. Kiểm tra mật khẩu cũ trước khi cho phép.
        
        /// <param name="maTK">Mã tài khoản.
        /// <param name="matKhauCu">Mật khẩu cũ (plaintext).
        /// <param name="matKhauMoi">Mật khẩu mới (plaintext).
        /// Trả vềTrue nếu đổi thành công.
        public bool DoiMatKhauKH(string maTK, string matKhauCu, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            if (string.IsNullOrWhiteSpace(matKhauCu))
                throw new ArgumentException("Mật khẩu cũ không được để trống.");
            if (string.IsNullOrWhiteSpace(matKhauMoi))
                throw new ArgumentException("Mật khẩu mới không được để trống.");
            if (matKhauMoi.Length < 6)
                throw new ArgumentException("Mật khẩu mới phải có ít nhất 6 ký tự.");

            try
            {
                DTO_TaiKhoanKH? tk = _dalKH.DSTheoMaTK(maTK.Trim());
                if (tk == null)
                    throw new InvalidOperationException("Tài khoản không tồn tại.");

                if (!XacNhanMatKhau(matKhauCu, tk.MatKhau))
                    throw new InvalidOperationException("Mật khẩu cũ không chính xác.");

                tk.MatKhau = HashMatKhau(matKhauMoi);
                return _dalKH.CapNhatTaiKhoanKH(tk);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đổi mật khẩu khách hàng: {ex.Message}", ex);
            }
        }

        
        /// Khóa hoặc mở khóa tài khoản khách hàng.
        
        /// <param name="maTK">Mã tài khoản.
        /// <param name="trangThai">Trạng thái mới: "Hoạt Động" hoặc "Khóa".
        /// Trả vềTrue nếu cập nhật thành công.
        public bool CapNhatTrangThaiKH(string maTK, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            if (trangThai != "Hoạt Động" && trangThai != "Khóa")
                throw new ArgumentException("Trạng thái phải là 'Hoạt Động' hoặc 'Khóa'.");

            try
            {
                return _dalKH.CapNhatTrangThai(maTK.Trim(), trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật trạng thái tài khoản khách hàng: {ex.Message}", ex);
            }
        }

        public DTO_TaiKhoanKH? LayTaiKhoanKHTheoMaTK(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK)) return null;
            return _dalKH.DSTheoMaTK(maTK.Trim());
        }

        public bool CapNhatTaiKhoanKH(DTO_TaiKhoanKH tk)
        {
            if (string.IsNullOrWhiteSpace(tk.MaTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            if (string.IsNullOrWhiteSpace(tk.MatKhau))
                throw new ArgumentException("Mật khẩu không được để trống.");

            try
            {
                DTO_TaiKhoanKH? existing = _dalKH.DSTheoMaTK(tk.MaTK);
                if (existing != null && existing.MatKhau != tk.MatKhau)
                {
                    if (tk.MatKhau.Length != 64)
                    {
                        tk.MatKhau = HashMatKhau(tk.MatKhau);
                    }
                }
                return _dalKH.CapNhatTaiKhoanKH(tk);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật tài khoản khách hàng: {ex.Message}", ex);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // PHẦN 6: LỊCH SỬ ĐĂNG NHẬP
        // ══════════════════════════════════════════════════════════════════

        
        /// Lấy toàn bộ lịch sử đăng nhập của hệ thống (chỉ dành cho Quản trị).
        /// Trả về DataTable chứa toàn bộ lịch sử đăng nhập.
        public DataTable LayLichSuDangNhap()
        {
            try
            {
                return _dalLSDN.DSTatCaLichSuDangNhap();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy lịch sử đăng nhập: {ex.Message}", ex);
            }
        }

        
        /// Lấy lịch sử đăng nhập của một tài khoản nhân viên cụ thể.
        /// <param name="maTK">Mã tài khoản nhân viên.
        /// Trả về DataTable chứa lịch sử đăng nhập.
        public DataTable LayLichSuDangNhapTheoTK(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new ArgumentException("Mã tài khoản không được để trống.");
            try
            {
                return _dalLSDN.DSTheoMaTK(maTK.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy lịch sử đăng nhập: {ex.Message}", ex);
            }
        }
    }
}

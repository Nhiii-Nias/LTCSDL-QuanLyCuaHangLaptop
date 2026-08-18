using System;
using System.Data;
using System.Text.RegularExpressions;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ quản lý nhân viên cửa hàng.
    /// Chịu trách nhiệm: validate thông tin nhân viên, thêm/sửa/xóa mềm,
    /// và đảm bảo các ràng buộc nghiệp vụ trước khi gọi DAL.
    
    public class BUS_NhanVien
    {
        private readonly DAL_NhanVien _dal = new DAL_NhanVien();
        private readonly DAL_TaiKhoanNV _dalTK = new DAL_TaiKhoanNV();


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: TRUY VẤN
        // ══════════════════════════════════════════════════════════════════

        
        /// Lấy danh sách toàn bộ nhân viên chưa bị xóa mềm (IsDeleted = 0).
        /// Trả về DataTable chứa danh sách nhân viên còn hoạt động.
        public DataTable LayDanhSach()
        {
            try
            {
                return _dal.DSTatCaNhanVien();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }

        
        /// Lấy thông tin nhân viên theo mã nhân viên.
        
        /// <param name="maNV">Mã nhân viên cần tìm.
        /// Trả về DTO_NhanVien nếu tìm thấy và chưa bị xóa, null nếu không tồn tại.
        public DTO_NhanVien? LayTheoMa(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
                throw new ArgumentException("Mã nhân viên không được để trống.");
            try
            {
                return _dal.DSTheoMaNV(maNV.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin nhân viên: {ex.Message}", ex);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: THÊM / SỬA / XÓA MỀM
        // ══════════════════════════════════════════════════════════════════

        
        /// Thêm nhân viên mới vào hệ thống sau khi kiểm tra đầy đủ thông tin.
        /// <param name="nv">DTO chứa thông tin nhân viên mới.
        /// <param name="maTKNguoiTao">Mã tài khoản nhân viên thực hiện thêm (ghi NguoiTao).
        /// Trả về True nếu thêm thành công.
        public bool Them(DTO_NhanVien nv, string maTKNguoiTao)
        {
            KiemTraHopLe(nv);

            try
            {
                nv.NgayTao = DateTime.Now;
                nv.NguoiTao = maTKNguoiTao?.Trim();
                nv.IsDeleted = false;
                return _dal.ThemNhanVien(nv);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm nhân viên: {ex.Message}", ex);
            }
        }

        
        /// Cập nhật thông tin nhân viên sau khi kiểm tra đầy đủ thông tin. Nhân viên phải còn hoạt động (IsDeleted = 0).
        /// <param name="nv">DTO chứa thông tin nhân viên cần cập nhật.
        /// <param name="maTKNguoiCapNhat">Mã tài khoản nhân viên thực hiện sửa (ghi NguoiCapNhat).
        /// Trả về True nếu cập nhật thành công.
        public bool Sua(DTO_NhanVien nv, string maTKNguoiCapNhat)
        {
            if (string.IsNullOrWhiteSpace(nv.MaNV))
                throw new ArgumentException("Mã nhân viên không được để trống khi cập nhật.");

            KiemTraHopLe(nv);

            var existing = _dal.DSTheoMaNV(nv.MaNV.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Nhân viên '{nv.MaNV}' không tồn tại hoặc đã bị xóa.");

            try
            {
                nv.NgayCapNhat = DateTime.Now;
                nv.NguoiCapNhat = maTKNguoiCapNhat?.Trim();
                return _dal.CapNhatNhanVien(nv);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        
        /// Xóa mềm nhân viên bằng cách đánh dấu IsDeleted = 1.
        /// Không được xóa vật lý do ràng buộc dữ liệu lịch sử giao dịch.
        /// <param name="maNV">Mã nhân viên cần xóa mềm.
        /// Trả về True nếu xóa mềm thành công.
        public bool Xoa(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
                throw new ArgumentException("Mã nhân viên không được để trống.");

            var existing = _dal.DSTheoMaNV(maNV.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Nhân viên '{maNV}' không tồn tại hoặc đã bị xóa trước đó.");

            try
            {
                bool deleted = _dal.XoaMemNhanVien(maNV.Trim());
                if (deleted)
                {
                    _dalTK.CapNhatTrangThaiTheoMaNV(maNV.Trim(), "Khóa");
                }
                return deleted;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa mềm nhân viên: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 3: Kiểm tra dữ liệu
        // ══════════════════════════════════════════════════════════════════

        
        /// Kiểm tra tính hợp lệ của toàn bộ thông tin nhân viên trước khi gọi DAL.
        /// Ném ArgumentException nếu dữ liệu không hợp lệ.
        /// <param name="nv">DTO_NhanVien cần kiểm tra.
        public void KiemTraHopLe(DTO_NhanVien nv)
        {
            if (nv == null)
                throw new ArgumentNullException(nameof(nv), "Thông tin nhân viên không được null.");

            if (string.IsNullOrWhiteSpace(nv.TenNV))
                throw new ArgumentException("Tên nhân viên không được để trống.");
            if (nv.TenNV.Length > 50)
                throw new ArgumentException("Tên nhân viên không được vượt quá 50 ký tự.");

            if (nv.SinhNhat == default(DateTime))
                throw new ArgumentException("Ngày sinh không hợp lệ.");
            if (nv.SinhNhat > DateTime.Today.AddYears(-18))
                throw new ArgumentException("Nhân viên phải từ 18 tuổi trở lên.");

            if (string.IsNullOrWhiteSpace(nv.SDT))
                throw new ArgumentException("Số điện thoại không được để trống.");
            if (!Regex.IsMatch(nv.SDT, @"^\d{10}$"))
                throw new ArgumentException("Số điện thoại phải gồm đúng 10 chữ số.");

            if (string.IsNullOrWhiteSpace(nv.DiaChi))
                throw new ArgumentException("Địa chỉ không được để trống.");

            if (!string.IsNullOrEmpty(nv.Email))
            {
                if (!Regex.IsMatch(nv.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Địa chỉ Email không đúng định dạng.");
            }

            if (nv.NgayVaoLam == default(DateTime))
                throw new ArgumentException("Ngày vào làm không hợp lệ.");
            if (nv.NgayVaoLam > DateTime.Today)
                throw new ArgumentException("Ngày vào làm không được là ngày trong tương lai.");

            if (nv.Luong < 0)
                throw new ArgumentException("Lương không được âm.");

            if (string.IsNullOrWhiteSpace(nv.ChucVu))
                throw new ArgumentException("Chức vụ không được để trống.");

            if (!string.IsNullOrEmpty(nv.GioiTinh) && nv.GioiTinh != "Nam" && nv.GioiTinh != "Nữ")
                throw new ArgumentException("Giới tính phải là 'Nam' hoặc 'Nữ'.");
        }
    }
}

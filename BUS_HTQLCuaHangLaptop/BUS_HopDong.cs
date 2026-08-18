using System;
using System.Data;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ quản lý hợp đồng mua bán với khách hàng sỉ (doanh nghiệp).
    /// HopDong chỉ tạo được cho KhachHangSi — validate LoaiKH = 'Sỉ' bắt buộc.
    /// Vòng đời: 'Hiệu Lực' → 'Hết Hạn' / 'Huỷ'.
    public class BUS_HopDong
    {
        // Khai báo DAL liên quan
        private readonly DAL_HopDong   _dalHD = new DAL_HopDong();
        private readonly DAL_KhachHang _dalKH = new DAL_KhachHang();

        // Trạng thái hợp đồng hợp lệ
        private static readonly string[] TRANG_THAI_HOP_LE = { "Hiệu Lực", "Hết Hạn", "Huỷ" };


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: TRUY VẤN
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ hợp đồng.
        /// Trả về DataTable chứa danh sách hợp đồng.
        public DataTable LayDanhSachHopDong()
        {
            try { return _dalHD.DSTatCaHopDong(); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy danh sách hợp đồng: {ex.Message}", ex); }
        }

        /// Lấy thông tin hợp đồng theo mã.
        /// <param name="maHD">Mã hợp đồng.
        /// Trả về DTO_HopDong nếu tìm thấy, null nếu không tồn tại.
        public DTO_HopDong? LayTheoMa(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
                throw new ArgumentException("Mã hợp đồng không được để trống.");
            try { return _dalHD.DSTheoMaHD(maHD.Trim()); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy thông tin hợp đồng: {ex.Message}", ex); }
        }

        /// Lấy danh sách hợp đồng của một khách hàng sỉ.
        /// <param name="maKH">Mã khách hàng.
        /// Trả về DataTable chứa danh sách hợp đồng.
        public DataTable LayTheoKhachHang(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            try { return _dalHD.DSTheoKhachHang(maKH.Trim()); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy hợp đồng theo khách hàng: {ex.Message}", ex); }
        }

        /// Lấy hợp đồng đang 'Hiệu Lực' của khách hàng sỉ tại thời điểm chỉ định.
        /// Dùng để kiểm tra khi tạo đơn hàng mới cho KhachHangSi.
        /// <param name="maKH">Mã khách hàng sỉ.
        /// <param name="ngayDat">Ngày đặt hàng cần kiểm tra.
        /// Trả về DTO_HopDong nếu có hợp đồng hiệu lực, null nếu không có.
        public DTO_HopDong? LayHopDongHieuLuc(string maKH, DateTime ngayDat)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");

            var dsHD = _dalHD.DSTheoKhachHang(maKH.Trim());
            foreach (DataRow row in dsHD.Rows)
            {
                string trangThai = row["TrangThai"].ToString() ?? "";
                if (trangThai != "Hiệu Lực") continue;

                DateTime ngayHieuLuc = Convert.ToDateTime(row["NgayHieuLuc"]);
                DateTime ngayHetHan  = Convert.ToDateTime(row["NgayHetHan"]);

                if (ngayDat.Date >= ngayHieuLuc.Date && ngayDat.Date <= ngayHetHan.Date)
                {
                    string maHD = row["MaHD"].ToString()!.Trim();
                    return _dalHD.DSTheoMaHD(maHD);
                }
            }
            return null;
        }

        /// Kiểm tra hợp đồng có thể được dùng để tạo đơn hàng mới không.
        /// Trả về true nếu hợp đồng đang 'Hiệu Lực', false nếu 'Hết Hạn' hoặc 'Huỷ'.
        public bool KiemTraHopDongCoTheTaoDon(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
                return false;

            var hd = _dalHD.DSTheoMaHD(maHD.Trim());
            if (hd == null)
                throw new InvalidOperationException($"Hợp đồng '{maHD}' không tồn tại.");

            if (hd.TrangThai != "Hiệu Lực")
                throw new InvalidOperationException(
                    $"Hợp đồng '{maHD}' đang ở trạng thái '{hd.TrangThai}'. " +
                    "Không thể tạo đơn hàng mới cho hợp đồng đã 'Hết Hạn' hoặc 'Huỷ'.");

            return true;
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: THÊM / CẬP NHẬT HỢP ĐỒNG
        // ══════════════════════════════════════════════════════════════════

        /// Thêm hợp đồng mới — chỉ dành cho KhachHangSi.
        /// <param name="hd">DTO chứa thông tin hợp đồng.
        /// <param name="maTKNguoiTao">Mã tài khoản nhân viên ký hợp đồng.
        /// Trả về True nếu thêm thành công.
        public bool ThemHopDong(DTO_HopDong hd, string? maTKNguoiTao = null)
        {
            KiemTraHopLeHopDong(hd);

            // Bắt buộc phải là khách hàng sỉ
            var kh = _dalKH.DSTheoMaKH(hd.MaKH.Trim());
            if (kh == null)
                throw new InvalidOperationException($"Khách hàng '{hd.MaKH}' không tồn tại hoặc đã bị xóa.");
            if (kh.LoaiKH != "Sỉ")
                throw new InvalidOperationException(
                    $"Khách hàng '{hd.MaKH}' ({kh.TenKH}) là khách hàng lẻ. " +
                    "Hợp đồng chỉ được tạo cho khách hàng sỉ (doanh nghiệp).");

            hd.TrangThai = "Hiệu Lực";
            hd.NgayTao   = DateTime.Now;
            hd.NguoiTao  = maTKNguoiTao?.Trim();

            try { return _dalHD.ThemHopDong(hd); }
            catch (Exception ex) { throw new Exception($"Lỗi thêm hợp đồng: {ex.Message}", ex); }
        }

        /// Cập nhật thông tin hợp đồng (GiaTriHD, NgayHieuLuc, NgayHetHan).
        /// Chỉ cập nhật được hợp đồng đang 'Hiệu Lực'.
        /// <param name="hd">DTO chứa thông tin cần cập nhật. MaHD bắt buộc.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhat(DTO_HopDong hd)
        {
            if (string.IsNullOrWhiteSpace(hd.MaHD))
                throw new ArgumentException("Mã hợp đồng không được để trống khi cập nhật.");

            var existing = _dalHD.DSTheoMaHD(hd.MaHD.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Hợp đồng '{hd.MaHD}' không tồn tại.");
            if (existing.TrangThai != "Hiệu Lực")
                throw new InvalidOperationException(
                    $"Hợp đồng '{hd.MaHD}' đang ở trạng thái '{existing.TrangThai}'. " +
                    "Chỉ được cập nhật hợp đồng đang 'Hiệu Lực'.");

            KiemTraHopLeHopDong(hd);

            // Giữ nguyên MaKH, MaNV, NgayTao, NguoiTao gốc
            hd.MaKH       = existing.MaKH;
            hd.NgayCapNhat = DateTime.Now;

            try { 
                bool ok = _dalHD.CapNhatHopDong(hd); 
                if (ok && (hd.TrangThai == "Huỷ" || hd.TrangThai == "Hết Hạn"))
                {
                    _dalHD.CapNhatTrangThaiHopDongLienQuan(hd.MaHD.Trim(), hd.TrangThai);
                }
                return ok;
            }
            catch (Exception ex) { throw new Exception($"Lỗi cập nhật hợp đồng: {ex.Message}", ex); }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 3: THAY ĐỔI TRẠNG THÁI HỢP ĐỒNG
        // ══════════════════════════════════════════════════════════════════

        /// Hủy hợp đồng — chuyển TrangThai sang 'Huỷ'.
        /// Chỉ hủy được hợp đồng đang 'Hiệu Lực'.
        /// <param name="maHD">Mã hợp đồng cần hủy.
        /// Trả về True nếu hủy thành công.
        public bool HuyHopDong(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
                throw new ArgumentException("Mã hợp đồng không được để trống.");

            var hd = _dalHD.DSTheoMaHD(maHD.Trim());
            if (hd == null)
                throw new InvalidOperationException($"Hợp đồng '{maHD}' không tồn tại.");
            if (hd.TrangThai == "Huỷ")
                throw new InvalidOperationException($"Hợp đồng '{maHD}' đã bị hủy trước đó.");
            if (hd.TrangThai == "Hết Hạn")
                throw new InvalidOperationException($"Hợp đồng '{maHD}' đã hết hạn. Không thể hủy.");

            try { return _dalHD.CapNhatTrangThaiHopDongLienQuan(maHD.Trim(), "Huỷ"); }
            catch (Exception ex) { throw new Exception($"Lỗi hủy hợp đồng: {ex.Message}", ex); }
        }

        /// Đánh dấu hợp đồng hết hạn — chuyển TrangThai sang 'Hết Hạn'.
        /// Thường được gọi bởi scheduler hoặc khi nhân viên xác nhận thủ công.
        /// <param name="maHD">Mã hợp đồng cần đánh dấu hết hạn.
        /// Trả về True nếu thành công.
        public bool DanhDauHetHan(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
                throw new ArgumentException("Mã hợp đồng không được để trống.");

            var hd = _dalHD.DSTheoMaHD(maHD.Trim());
            if (hd == null)
                throw new InvalidOperationException($"Hợp đồng '{maHD}' không tồn tại.");
            if (hd.TrangThai != "Hiệu Lực")
                throw new InvalidOperationException(
                    $"Hợp đồng '{maHD}' đang ở trạng thái '{hd.TrangThai}'. " +
                    "Chỉ có thể đánh dấu hết hạn từ trạng thái 'Hiệu Lực'.");

            try { return _dalHD.CapNhatTrangThaiHopDongLienQuan(maHD.Trim(), "Hết Hạn"); }
            catch (Exception ex) { throw new Exception($"Lỗi cập nhật trạng thái hợp đồng: {ex.Message}", ex); }
        }

        /// Tự động kiểm tra và cập nhật hợp đồng hết hạn theo ngày hiện tại.
        /// Gọi khi mở ứng dụng hoặc theo lịch định kỳ.
        /// Trả về số hợp đồng đã được cập nhật sang 'Hết Hạn'.
        public int TuDongCapNhatHetHan()
        {
            int soCapNhat = 0;
            try
            {
                var dsHD = _dalHD.DSTatCaHopDong();
                DateTime hom_nay = DateTime.Today;

                foreach (DataRow row in dsHD.Rows)
                {
                    if (row["TrangThai"].ToString() != "Hiệu Lực") continue;

                    DateTime ngayHetHan = Convert.ToDateTime(row["NgayHetHan"]);
                    if (hom_nay > ngayHetHan)
                    {
                        string maHD = row["MaHD"].ToString()!.Trim();
                        if (_dalHD.CapNhatTrangThaiHopDongLienQuan(maHD, "Hết Hạn"))
                            soCapNhat++;
                    }
                }
                return soCapNhat;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tự động cập nhật hợp đồng hết hạn: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 4: KIỂM TRA DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════

        /// Kiểm tra thông tin hợp đồng hợp lệ.
        public void KiemTraHopLeHopDong(DTO_HopDong hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd), "Thông tin hợp đồng không được null.");

            if (string.IsNullOrWhiteSpace(hd.MaHD))
                throw new ArgumentException("Mã hợp đồng không được để trống.");
            if (string.IsNullOrWhiteSpace(hd.MaNV))
                throw new ArgumentException("Mã nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(hd.MaKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");

            if (hd.GiaTriHD < 0)
                throw new ArgumentException("Giá trị hợp đồng không được âm.");

            if (hd.NgayHieuLuc == default)
                throw new ArgumentException("Ngày hiệu lực không hợp lệ.");
            if (hd.NgayHetHan == default)
                throw new ArgumentException("Ngày hết hạn không hợp lệ.");
            if (hd.NgayHetHan <= hd.NgayHieuLuc)
                throw new ArgumentException("Ngày hết hạn phải sau ngày hiệu lực.");

            if (hd.NgayKy == default)
                hd.NgayKy = DateTime.Today;
        }
    }
}

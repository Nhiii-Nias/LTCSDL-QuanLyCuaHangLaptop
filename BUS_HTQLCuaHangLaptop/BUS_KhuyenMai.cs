using System;
using System.Collections.Generic;
using System.Data;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ khuyến mãi — data-driven, không hardcode mã KM.
    ///
    /// Cơ chế hoạt động (áp dụng cho MỌI chương trình KM, kể cả KM mới thêm vào):
    ///   Bước 1 — Thời gian: NgayDat phải nằm trong [NgayBatDau, NgayKetThuc].
    ///   Bước 2 — Đối tượng (DoiTuong): 'Tất Cả' | 'HSSV' | 'Doanh Nghiệp'.
    ///   Bước 3 — Điều kiện số lượng (DieuKien + SLToiThieu):
    ///     DieuKien = NULL: không giới hạn loại SP (đếm toàn bộ sản phẩm trong đơn).
    ///     DieuKien = 'Laptop'/'Chuột'/'Bàn Phím': chỉ đếm SP thuộc DanhMuc đó.
    ///     SLToiThieu = NULL: không có yêu cầu số lượng tối thiểu.
    ///
    /// Cách tính tiền giảm:
    ///   MucGiamSP set → giảm % trên từng SP hợp lệ (lọc theo DieuKien nếu có).
    ///   MucGiamDH set → giảm % trên tổng đơn hàng.
    ///   (Chỉ 1 trong 2 được set cho 1 chương trình KM.)
    public class BUS_KhuyenMai
    {
        // Khai báo DAL liên quan
        private readonly DAL_KhuyenMai   _dalKM   = new DAL_KhuyenMai();
        private readonly DAL_KhachHang   _dalKH   = new DAL_KhachHang();
        private readonly DAL_KhachHangLe _dalKHLe = new DAL_KhachHangLe();
        private readonly DAL_LoaiSanPham _dalLSP  = new DAL_LoaiSanPham();
        private readonly DAL_SanPham     _dalSP   = new DAL_SanPham();


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: TRUY VẤN
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ chương trình khuyến mãi.
        public DataTable LayDanhSachKhuyenMai()
        {
            try { return _dalKM.DSTatCaKhuyenMai(); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy danh sách khuyến mãi: {ex.Message}", ex); }
        }

        /// Lấy danh sách khuyến mãi đang trong thời gian hiệu lực tại ngày chỉ định.
        /// <param name="ngay">Ngày cần kiểm tra.
        public DataTable LayKhuyenMaiHieuLuc(DateTime ngay)
        {
            try { return _dalKM.DSTrongThoiGianHieuLuc(ngay); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy khuyến mãi hiệu lực: {ex.Message}", ex); }
        }

        /// Lấy thông tin một chương trình khuyến mãi theo mã.
        /// Trả về DTO_KhuyenMai nếu tìm thấy, null nếu không tồn tại.
        public DTO_KhuyenMai? LayTheoMa(string maKM)
        {
            if (string.IsNullOrWhiteSpace(maKM))
                throw new ArgumentException("Mã khuyến mãi không được để trống.");
            try { return _dalKM.DSTheoMaKM(maKM.Trim()); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy thông tin khuyến mãi: {ex.Message}", ex); }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: NGHIỆP VỤ TÍNH KHUYẾN MÃI (DATA-DRIVEN)
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách CÁC chương trình KM mà khách hàng có thể áp dụng cho đơn hàng.
        /// Dùng để hiển thị danh sách cho nhân viên chọn, hoặc để auto-chọn KM tốt nhất.
        ///
        /// <param name="maKH">Mã khách hàng đặt đơn.
        /// <param name="chiTiet">Danh sách DTO_ChiTietDonHang (cần MaSerialSP, GiaBan).
        /// <param name="ngayDat">Ngày đặt hàng.
        /// Trả về List<DTO_KhuyenMai> các KM đủ điều kiện (rỗng nếu không có KM nào).
        public List<DTO_KhuyenMai> LayDanhSachKMCoTheThuHuong(
            string maKH,
            List<DTO_ChiTietDonHang> chiTiet,
            DateTime ngayDat)
        {
            var ketQua = new List<DTO_KhuyenMai>();

            if (string.IsNullOrWhiteSpace(maKH) || chiTiet == null || chiTiet.Count == 0)
                return ketQua;

            var kh = _dalKH.DSTheoMaKH(maKH.Trim());
            if (kh == null) return ketQua;

            // Bước 1: Lấy tất cả KM đang trong thời gian hiệu lực
            var dtKM = _dalKM.DSTrongThoiGianHieuLuc(ngayDat.Date);

            foreach (DataRow row in dtKM.Rows)
            {
                string maKM = row["MaKM"].ToString()!.Trim();
                var km = _dalKM.DSTheoMaKM(maKM);
                if (km == null) continue;

                // Bước 2 + 3: Kiểm tra đối tượng và điều kiện đặt hàng
                if (KiemTraDieuKienKM(km, kh, chiTiet, ngayDat))
                    ketQua.Add(km);
            }

            return ketQua;
        }

        /// Tự động chọn chương trình KM có lợi nhất cho khách hàng (giảm nhiều nhất).
        /// Chỉ 1 KM được áp dụng trên 1 đơn hàng — không áp dụng đồng thời.
        ///
        /// <param name="dh">DTO_DonHang (cần MaKH, NgayDat, TongTien).
        /// <param name="chiTiet">Danh sách DTO_ChiTietDonHang.
        /// Trả về tuple (decimal tienGiam, string maKMApDung).
        ///   tienGiam = 0 và maKMApDung = "" nếu không có KM nào phù hợp.
        public (decimal tienGiam, string maKMApDung) TinhKhuyenMai(
            DTO_DonHang dh,
            List<DTO_ChiTietDonHang> chiTiet)
        {
            if (dh == null) throw new ArgumentNullException(nameof(dh));
            if (chiTiet == null || chiTiet.Count == 0) return (0m, string.Empty);

            var ngayDat = dh.NgayDat == default ? DateTime.Today : dh.NgayDat.Date;

            // Lấy danh sách các KM đủ điều kiện
            var danhSachKM = LayDanhSachKMCoTheThuHuong(dh.MaKH, chiTiet, ngayDat);
            if (danhSachKM.Count == 0) return (0m, string.Empty);

            // Chọn KM giảm nhiều nhất
            decimal maxGiam = 0m;
            string maKMChon = string.Empty;

            foreach (var km in danhSachKM)
            {
                decimal giam = TinhTienGiam(km, chiTiet);
                if (giam > maxGiam)
                {
                    maxGiam = giam;
                    maKMChon = km.MaKM;
                }
            }

            return (maxGiam, maKMChon);
        }

        /// Kiểm tra một chương trình KM cụ thể có áp dụng được cho đơn hàng không.
        /// Đây là hàm trung tâm — áp dụng chung cho MỌI chương trình KM (kể cả KM mới).
        ///
        /// Luồng kiểm tra:
        ///   1. Thời gian hiệu lực (NgayBatDau ≤ ngayDat ≤ NgayKetThuc)
        ///   2. Đối tượng áp dụng (DoiTuong: 'Tất Cả' | 'HSSV' | 'Doanh Nghiệp')
        ///   3. Điều kiện số lượng (DieuKien = DanhMuc lọc SP, SLToiThieu = số lượng tối thiểu)
        ///
        /// <param name="km">Chương trình khuyến mãi cần kiểm tra.
        /// <param name="kh">Thông tin khách hàng đặt đơn.
        /// <param name="chiTiet">Danh sách chi tiết đơn hàng.
        /// <param name="ngayDat">Ngày đặt hàng.
        /// Trả về true nếu đủ tất cả điều kiện.
        public bool KiemTraDieuKienKM(
            DTO_KhuyenMai km,
            DTO_KhachHang kh,
            List<DTO_ChiTietDonHang> chiTiet,
            DateTime ngayDat)
        {
            if (km == null || kh == null || chiTiet == null) return false;

            // ── BƯỚC 1: Thời gian hiệu lực ──────────────────────────────
            if (ngayDat.Date < km.NgayBatDau.Date || ngayDat.Date > km.NgayKetThuc.Date)
                return false;

            // ── BƯỚC 2: Đối tượng áp dụng ───────────────────────────────
            switch (km.DoiTuong?.Trim())
            {
                case "Tất Cả":
                    // Không giới hạn đối tượng
                    break;

                case "HSSV":
                    // Chỉ áp dụng cho khách hàng lẻ có LaHSSV = true
                    if (kh.LoaiKH != "Lẻ") return false;
                    var khLe = _dalKHLe.DSTheoMaKHLe(kh.MaKH.Trim());
                    if (khLe == null || !khLe.LaHSSV) return false;
                    break;

                case "Doanh Nghiệp":
                    // Chỉ áp dụng cho khách hàng sỉ
                    if (kh.LoaiKH != "Sỉ") return false;
                    break;

                default:
                    // DoiTuong không hợp lệ → không áp dụng
                    return false;
            }

            // ── BƯỚC 3: Điều kiện số lượng ──────────────────────────────
            // Chỉ kiểm tra khi có SLToiThieu
            if (km.SLToiThieu.HasValue && km.SLToiThieu.Value > 0)
            {
                int soLuong;

                if (!string.IsNullOrWhiteSpace(km.DieuKien))
                {
                    // DieuKien là tên DanhMuc (Laptop / Chuột / Bàn Phím)
                    // → Chỉ đếm số serial thuộc DanhMuc đó trong đơn hàng
                    soLuong = _DemSanPhamTheoDanhMuc(chiTiet, km.DieuKien.Trim());
                }
                else
                {
                    // DieuKien = NULL → đếm tổng số serial trong đơn
                    soLuong = chiTiet.Count;
                }

                if (soLuong < km.SLToiThieu.Value) return false;
            }

            return true;
        }

        /// Tính tiền giảm của một chương trình KM cho danh sách chi tiết đơn hàng.
        ///
        /// Logic tính:
        ///   MucGiamSP set → giảm % trên GiaBan của từng SP hợp lệ:
        ///     DieuKien set  → chỉ giảm SP thuộc DanhMuc đó.
        ///     DieuKien null → giảm tất cả SP trong đơn.
        ///   MucGiamDH set → giảm % trên tổng tiền đơn hàng (SUM GiaBan).
        ///
        /// <param name="km">Chương trình khuyến mãi.
        /// <param name="chiTiet">Danh sách chi tiết đơn hàng.
        /// Trả về số tiền giảm (decimal, làm tròn 2 chữ số thập phân).
        public decimal TinhTienGiam(DTO_KhuyenMai km, List<DTO_ChiTietDonHang> chiTiet)
        {
            if (km == null || chiTiet == null || chiTiet.Count == 0) return 0m;

            // Trường hợp 1: Giảm trên từng sản phẩm (MucGiamSP)
            if (km.MucGiamSP.HasValue && km.MucGiamSP.Value > 0)
            {
                decimal tongGiam = 0m;
                foreach (var ct in chiTiet)
                {
                    bool apDung = true;

                    // Nếu DieuKien có giá trị → chỉ giảm SP thuộc DanhMuc đó
                    if (!string.IsNullOrWhiteSpace(km.DieuKien))
                    {
                        apDung = _KiemTraSerialThuocDanhMuc(ct.MaSerialSP, km.DieuKien.Trim());
                    }

                    if (apDung)
                        tongGiam += ct.GiaBan * km.MucGiamSP.Value / 100m;
                }
                return Math.Round(tongGiam, 2);
            }

            // Trường hợp 2: Giảm trên tổng đơn hàng (MucGiamDH)
            if (km.MucGiamDH.HasValue && km.MucGiamDH.Value > 0)
            {
                decimal tongGia = 0m;
                foreach (var ct in chiTiet)
                    tongGia += ct.GiaBan;

                return Math.Round(tongGia * km.MucGiamDH.Value / 100m, 2);
            }

            return 0m;
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 3: QUẢN LÝ CHƯƠNG TRÌNH KM (CRUD)
        // ══════════════════════════════════════════════════════════════════

        /// Thêm chương trình khuyến mãi mới.
        /// <param name="km">DTO_KhuyenMai chứa thông tin chương trình.
        /// Trả về True nếu thêm thành công.
        public bool ThemKhuyenMai(DTO_KhuyenMai km)
        {
            KiemTraHopLeKM(km);
            km.NgayTao = DateTime.Now;
            try { return _dalKM.ThemKhuyenMai(km); }
            catch (Exception ex) { throw new Exception($"Lỗi thêm khuyến mãi: {ex.Message}", ex); }
        }

        /// Cập nhật thông tin chương trình khuyến mãi. MaKM bắt buộc.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatKhuyenMai(DTO_KhuyenMai km)
        {
            if (string.IsNullOrWhiteSpace(km.MaKM))
                throw new ArgumentException("Mã khuyến mãi không được để trống khi cập nhật.");

            var existing = _dalKM.DSTheoMaKM(km.MaKM.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Khuyến mãi '{km.MaKM}' không tồn tại.");

            KiemTraHopLeKM(km);
            try { return _dalKM.CapNhatKhuyenMai(km); }
            catch (Exception ex) { throw new Exception($"Lỗi cập nhật khuyến mãi: {ex.Message}", ex); }
        }

        /// Xóa vật lý chương trình khuyến mãi.
        /// Chỉ xóa được khi chưa có đơn hàng nào tham chiếu (ràng buộc FK bảo vệ tầng DB).
        /// Trả về True nếu xóa thành công.
        public bool XoaKhuyenMai(string maKM)
        {
            if (string.IsNullOrWhiteSpace(maKM))
                throw new ArgumentException("Mã khuyến mãi không được để trống.");

            var existing = _dalKM.DSTheoMaKM(maKM.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Khuyến mãi '{maKM}' không tồn tại.");

            try { return _dalKM.XoaKhuyenMai(maKM.Trim()); }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Lỗi xóa khuyến mãi '{maKM}'. Kiểm tra xem có đơn hàng nào đang dùng KM này không. " +
                    $"Chi tiết: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 4: HELPER PRIVATE
        // ══════════════════════════════════════════════════════════════════

        /// Đếm số serial trong đơn hàng thuộc DanhMuc chỉ định.
        /// <param name="chiTiet">Danh sách chi tiết đơn hàng.
        /// <param name="danhMuc">Tên danh mục cần đếm ('Laptop' | 'Chuột' | 'Bàn Phím').
        private int _DemSanPhamTheoDanhMuc(List<DTO_ChiTietDonHang> chiTiet, string danhMuc)
        {
            int dem = 0;
            foreach (var ct in chiTiet)
            {
                if (_KiemTraSerialThuocDanhMuc(ct.MaSerialSP, danhMuc))
                    dem++;
            }
            return dem;
        }

        /// Kiểm tra serial có thuộc DanhMuc chỉ định không (tra qua SanPham → LoaiSanPham).
        /// <param name="maSerial">Số serial sản phẩm.
        /// <param name="danhMuc">Tên danh mục cần kiểm tra.
        /// Trả về true nếu serial thuộc danh mục đó.
        private bool _KiemTraSerialThuocDanhMuc(string maSerial, string danhMuc)
        {
            if (string.IsNullOrWhiteSpace(maSerial)) return false;

            string maLoaiSP = "";
            if (maSerial.StartsWith("x-", StringComparison.OrdinalIgnoreCase))
            {
                var parts = maSerial.Split('-');
                if (parts.Length > 1)
                {
                    maLoaiSP = parts[1];
                }
            }

            if (string.IsNullOrEmpty(maLoaiSP))
            {
                var sp = _dalSP.DSTheoMaSerialSP(maSerial.Trim());
                if (sp == null) return false;
                maLoaiSP = sp.MaLoaiSP;
            }

            var lsp = _dalLSP.TimLoaiSP(maLoaiSP);
            return lsp != null && lsp.DanhMuc == danhMuc;
        }

        /// Kiểm tra thông tin chương trình khuyến mãi hợp lệ.
        public void KiemTraHopLeKM(DTO_KhuyenMai km)
        {
            if (km == null)
                throw new ArgumentNullException(nameof(km), "Thông tin khuyến mãi không được null.");
            if (string.IsNullOrWhiteSpace(km.TenKM))
                throw new ArgumentException("Tên khuyến mãi không được để trống.");
            if (string.IsNullOrWhiteSpace(km.DoiTuong))
                throw new ArgumentException("Đối tượng áp dụng không được để trống.");

            var doiTuongHopLe = new[] { "Tất Cả", "HSSV", "Doanh Nghiệp" };
            bool hopLe = false;
            foreach (var dt in doiTuongHopLe)
                if (dt == km.DoiTuong.Trim()) { hopLe = true; break; }
            if (!hopLe)
                throw new ArgumentException("Đối tượng áp dụng chỉ nhận: 'Tất Cả', 'HSSV', 'Doanh Nghiệp'.");

            if (km.NgayBatDau == default || km.NgayKetThuc == default)
                throw new ArgumentException("Ngày bắt đầu và ngày kết thúc không hợp lệ.");
            if (km.NgayKetThuc < km.NgayBatDau)
                throw new ArgumentException("Ngày kết thúc phải >= ngày bắt đầu.");

            if (km.MucGiamSP.HasValue && (km.MucGiamSP < 0 || km.MucGiamSP > 100))
                throw new ArgumentException("Mức giảm sản phẩm phải trong khoảng 0–100%.");
            if (km.MucGiamDH.HasValue && (km.MucGiamDH < 0 || km.MucGiamDH > 100))
                throw new ArgumentException("Mức giảm đơn hàng phải trong khoảng 0–100%.");
            if (km.MucGiamSP.HasValue && km.MucGiamDH.HasValue)
                throw new ArgumentException("Một chương trình KM chỉ được đặt MucGiamSP HOẶC MucGiamDH, không được cả hai.");
            if (!km.MucGiamSP.HasValue && !km.MucGiamDH.HasValue)
                throw new ArgumentException("Chương trình KM phải có ít nhất MucGiamSP hoặc MucGiamDH.");

            if (km.SLToiThieu.HasValue && km.SLToiThieu.Value < 0)
                throw new ArgumentException("Số lượng tối thiểu không được âm.");

            // DieuKien (nếu có) phải là tên DanhMuc hợp lệ
            if (!string.IsNullOrWhiteSpace(km.DieuKien))
            {
                var danhMucHopLe = new[] { "Laptop", "Chuột", "Bàn Phím" };
                bool danhMucOK = false;
                foreach (var dm in danhMucHopLe)
                    if (dm == km.DieuKien.Trim()) { danhMucOK = true; break; }
                if (!danhMucOK)
                    throw new ArgumentException("Điều kiện sản phẩm (DieuKien) chỉ nhận: 'Laptop', 'Chuột', 'Bàn Phím'.");
            }
        }
    }
}

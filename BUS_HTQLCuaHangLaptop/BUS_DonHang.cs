using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ quản lý đơn hàng.
    /// Tạo đơn hàng là thao tác atomic — INSERT DonHang + INSERT ChiTietDonHang
    /// + UPDATE SanPham.TrangThai → 'Đã Bán' trong cùng 1 transaction.
    /// Hủy đơn cũng là atomic — UPDATE DonHang + RESTORE SanPham → 'Trong Kho'.
    public class BUS_DonHang
    {
        // Khai báo DAL liên quan
        private readonly DAL_DonHang       _dalDH   = new DAL_DonHang();
        private readonly DAL_ChiTietDonHang _dalCTDH = new DAL_ChiTietDonHang();
        private readonly DAL_SanPham        _dalSP   = new DAL_SanPham();
        private readonly DAL_KhachHang      _dalKH   = new DAL_KhachHang();
        private readonly DAL_LoaiSanPham    _dalLSP  = new DAL_LoaiSanPham();

        // Các BUS phụ thuộc
        private readonly BUS_KhuyenMai _busKM  = new BUS_KhuyenMai();
        private readonly BUS_HopDong   _busHD  = new BUS_HopDong();

        // Giá trị hợp lệ cho TrangThai và PhuongThucThanhToan
        private static readonly string[] TRANG_THAI_HOP_LE     = { "Chờ Xử Lý", "Đang Giao", "Hoàn Thành", "Huỷ" };
        private static readonly string[] PHUONG_THUC_HOP_LE    = { "Tiền Mặt", "Chuyển Khoản", "Thẻ" };


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: TRUY VẤN
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ đơn hàng.
        public DataTable LayDanhSachDonHang()
        {
            try { return _dalDH.DSTatCaDonHang(); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy danh sách đơn hàng: {ex.Message}", ex); }
        }

        /// Lấy thông tin đơn hàng theo mã.
        /// Trả về DTO_DonHang nếu tìm thấy, null nếu không tồn tại.
        public DTO_DonHang? LayTheoMa(string maDH)
        {
            if (string.IsNullOrWhiteSpace(maDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            try { return _dalDH.DSTheoMaDH(maDH.Trim()); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy thông tin đơn hàng: {ex.Message}", ex); }
        }

        /// Lấy danh sách đơn hàng của một khách hàng.
        /// <param name="maKH">Mã khách hàng.
        public DataTable LayTheoKhachHang(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            try { return _dalDH.DSTheoKhachHang(maKH.Trim()); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy đơn hàng theo khách hàng: {ex.Message}", ex); }
        }

        /// Lấy danh sách đơn hàng theo trạng thái.
        /// <param name="trangThai">'Chờ Xử Lý' | 'Đang Giao' | 'Hoàn Thành' | 'Huỷ'.
        public DataTable LayTheoTrangThai(string trangThai)
        {
            KiemTraTrangThaiDH(trangThai);
            try { return _dalDH.DSTheoTrangThai(trangThai); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy đơn hàng theo trạng thái: {ex.Message}", ex); }
        }

        /// Lấy chi tiết đơn hàng (kèm TenLoai, TenHang, DanhMuc, ThanhTien).
        /// <param name="maDH">Mã đơn hàng.
        public DataTable LayChiTietDonHang(string maDH)
        {
            if (string.IsNullOrWhiteSpace(maDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            try { return _dalCTDH.DSChiTietCoThongTinSanPham(maDH.Trim()); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy chi tiết đơn hàng: {ex.Message}", ex); }
        }

        /// Lấy chi tiết đơn hàng theo số Serial.
        public DTO_ChiTietDonHang? LayChiTietTheoSerial(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Mã serial không được để trống.");
            try { return _dalCTDH.DSTheoMaSerialSP(maSerial.Trim()); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy chi tiết đơn hàng theo serial: {ex.Message}", ex); }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: TẠO ĐƠN HÀNG (TRANSACTION 3 BƯỚC)
        // ══════════════════════════════════════════════════════════════════

        /// Tạo đơn hàng mới — thao tác ATOMIC gồm 3 bước trong 1 transaction:
        ///   Bước 1: INSERT DonHang
        ///   Bước 2: INSERT từng dòng ChiTietDonHang
        ///   Bước 3: UPDATE TrangThai SanPham → 'Đã Bán' cho mỗi serial
        ///
        /// GiaBan lấy từ LoaiSanPham.GiaBanGoc tại thời điểm gọi (snapshot giá).
        /// TongTien = SUM(GiaBan). TienSauGiam do BUS_KhuyenMai tính (nếu có MaKM).
        ///
        /// <param name="dh">DTO_DonHang (cần: MaDH, MaNV, MaKH, PhuongThucThanhToan, MaKM?, MaHD?).
        /// <param name="danhSachSerial">Danh sách mã serial cần bán (phải đang 'Trong Kho').
        /// <param name="maTKNguoiTao">Mã tài khoản nhân viên thực hiện.
        /// Trả về True nếu tạo thành công.
        public bool TaoDonHang(
            DTO_DonHang dh,
            List<string> danhSachSerial,
            string? maTKNguoiTao = null)
        {
            // ── VALIDATE ĐẦU VÀO ────────────────────────────────────────
            if (dh == null) throw new ArgumentNullException(nameof(dh));
            if (string.IsNullOrWhiteSpace(dh.MaDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(dh.MaNV))
                throw new ArgumentException("Mã nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(dh.MaKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");

            KiemTraPhuongThuc(dh.PhuongThucThanhToan);

            if (danhSachSerial == null || danhSachSerial.Count == 0)
                throw new ArgumentException("Đơn hàng phải có ít nhất một sản phẩm.");

            // Kiểm tra không có serial trùng nhau trong cùng đơn
            var serialSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var s in danhSachSerial)
            {
                if (!serialSet.Add(s.Trim()))
                    throw new InvalidOperationException($"Serial '{s}' xuất hiện trùng lặp trong danh sách đơn hàng.");
            }

            // Kiểm tra khách hàng tồn tại
            var kh = _dalKH.DSTheoMaKH(dh.MaKH.Trim());
            if (kh == null)
                throw new InvalidOperationException($"Khách hàng '{dh.MaKH}' không tồn tại hoặc đã bị xóa.");

            // ── VALIDATE HỢP ĐỒNG ────────────────────────────────────────
            // KH Lẻ: MaHD phải là NULL
            // KH Sỉ: MaHD phải thuộc HopDong đang 'Hiệu Lực'
            if (kh.LoaiKH == "Lẻ")
            {
                if (!string.IsNullOrEmpty(dh.MaHD))
                    throw new InvalidOperationException("Khách hàng lẻ không có hợp đồng. MaHD phải để trống.");
                dh.MaHD = null!;
            }
            else if (kh.LoaiKH == "Sỉ")
            {
                if (!string.IsNullOrEmpty(dh.MaHD))
                {
                    // Kiểm tra hợp đồng được chỉ định có hiệu lực không
                    _busHD.KiemTraHopDongCoTheTaoDon(dh.MaHD.Trim());
                }
                // MaHD có thể null nếu KH sỉ mua không theo hợp đồng
            }

            // ── KIỂM TRA TỪNG SERIAL & LẤY GIÁ ─────────────────────────
            var chiTietList = new List<DTO_ChiTietDonHang>();
            decimal tongTien = 0m;

            foreach (var serial in danhSachSerial)
            {
                if (serial.Trim().StartsWith("x-", StringComparison.OrdinalIgnoreCase))
                {
                    string[] parts = serial.Split('-');
                    if (parts.Length < 4)
                        throw new InvalidOperationException($"Định dạng serial ảo '{serial}' không hợp lệ.");
                    
                    string maLoaiSP = parts[1].Trim();
                    var lsp = _dalLSP.TimLoaiSP(maLoaiSP);
                    if (lsp == null)
                        throw new InvalidOperationException($"Không tìm thấy loại sản phẩm cho serial ảo '{serial}'.");

                    decimal giaBan = lsp.GiaBanGoc;
                    tongTien += giaBan;

                    chiTietList.Add(new DTO_ChiTietDonHang
                    {
                        MaDH        = dh.MaDH.Trim(),
                        MaSerialSP  = serial.Trim(),
                        GiaBan      = giaBan,
                        PhanTramGiam = null
                    });
                }
                else
                {
                    var sp = _dalSP.DSTheoMaSerialSP(serial.Trim());

                    if (sp == null)
                        throw new InvalidOperationException($"Sản phẩm serial '{serial}' không tồn tại.");
                    if (sp.IsDeleted)
                        throw new InvalidOperationException($"Sản phẩm serial '{serial}' đã bị xóa khỏi hệ thống.");
                    if (sp.TrangThai != "Trong Kho")
                        throw new InvalidOperationException(
                            $"Sản phẩm serial '{serial}' đang ở trạng thái '{sp.TrangThai}'. " +
                            "Chỉ bán được sản phẩm đang 'Trong Kho'.");

                    var lsp = _dalLSP.TimLoaiSP(sp.MaLoaiSP);
                    if (lsp == null)
                        throw new InvalidOperationException(
                            $"Không tìm thấy thông tin loại sản phẩm cho serial '{serial}' (MaLoaiSP: {sp.MaLoaiSP}).");

                    decimal giaBan = lsp.GiaBanGoc;
                    tongTien += giaBan;

                    chiTietList.Add(new DTO_ChiTietDonHang
                    {
                        MaDH        = dh.MaDH.Trim(),
                        MaSerialSP  = serial.Trim(),
                        GiaBan      = giaBan,
                        PhanTramGiam = null
                    });
                }
            }

            // ── TÍNH KHUYẾN MÃI ─────────────────────────────────────────
            dh.TongTien  = tongTien;
            dh.NgayDat   = dh.NgayDat == default ? DateTime.Now : dh.NgayDat;
            dh.NgayTao   = DateTime.Now;
            dh.NguoiTao  = maTKNguoiTao?.Trim();
            dh.TrangThai = "Chờ Xử Lý";

            decimal tienGiam      = 0m;
            string  maKMApDung    = string.Empty;

            if (dh.MaKM != null && dh.MaKM.Trim().Equals("NONE", StringComparison.OrdinalIgnoreCase))
            {
                dh.MaKM = null!;
                tienGiam = 0m;
                maKMApDung = string.Empty;
            }
            else if (!string.IsNullOrEmpty(dh.MaKM))
            {
                var kmChon = _busKM.LayTheoMa(dh.MaKM.Trim());
                if (kmChon == null)
                    throw new InvalidOperationException($"Chương trình khuyến mãi '{dh.MaKM}' không tồn tại.");

                if (_busKM.KiemTraDieuKienKM(kmChon, kh, chiTietList, dh.NgayDat))
                {
                    tienGiam   = _busKM.TinhTienGiam(kmChon, chiTietList);
                    maKMApDung = dh.MaKM.Trim();
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Đơn hàng không đủ điều kiện áp dụng khuyến mãi '{kmChon.TenKM}'. " +
                        "Vui lòng kiểm tra lại đối tượng, thời gian và số lượng sản phẩm.");
                }
            }
            else
            {
                (tienGiam, maKMApDung) = _busKM.TinhKhuyenMai(dh, chiTietList);
                dh.MaKM = string.IsNullOrEmpty(maKMApDung) ? null! : maKMApDung;
            }

            dh.TienSauGiam = tongTien - tienGiam;
            if (dh.TienSauGiam < 0) dh.TienSauGiam = 0m;

            if (!string.IsNullOrEmpty(maKMApDung))
            {
                var km = _busKM.LayTheoMa(maKMApDung);
                if (km?.MucGiamSP.HasValue == true)
                {
                    var dalSPTemp = new DAL_SanPham();
                    foreach (var ct in chiTietList)
                    {
                        bool apDung = true;
                        if (!string.IsNullOrWhiteSpace(km.DieuKien))
                        {
                            string maLoai2;
                            if (ct.MaSerialSP.StartsWith("x-", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] parts2 = ct.MaSerialSP.Split('-');
                                maLoai2 = parts2[1];
                            }
                            else
                            {
                                var sp2  = dalSPTemp.DSTheoMaSerialSP(ct.MaSerialSP);
                                maLoai2 = sp2 != null ? sp2.MaLoaiSP : null!;
                            }
                            var lsp2 = maLoai2 != null ? _dalLSP.TimLoaiSP(maLoai2) : null;
                            apDung = lsp2 != null && lsp2.DanhMuc == km.DieuKien;
                        }
                        if (apDung)
                            ct.PhanTramGiam = km.MucGiamSP;
                    }
                }
            }

            // ── THỰC THI TRANSACTION ─────────────────────────────────────
            string connStr = ConfigurationManager
                .ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;

            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Ensure dummy PhieuNhap PN00000000 exists
                string sqlEnsurePN = @"
                    IF NOT EXISTS (SELECT 1 FROM PhieuNhap WHERE MaPhieuNhap = 'PN00000000')
                    BEGIN
                        DECLARE @DummyNV CHAR(10) = (SELECT TOP 1 MaNV FROM NhanVien);
                        DECLARE @DummyNCC CHAR(10) = (SELECT TOP 1 MaNCC FROM NhaCungCap);
                        INSERT INTO PhieuNhap (MaPhieuNhap, MaNV, MaNCC, NgayNhap, TongTien, TrangThai)
                        VALUES ('PN00000000', @DummyNV, @DummyNCC, GETDATE(), 0, N'Đã Nhập');
                    END";
                using (SqlCommand cmdEnsurePN = new SqlCommand(sqlEnsurePN, conn, tran))
                {
                    cmdEnsurePN.ExecuteNonQuery();
                }

                // Insert placeholder SanPham rows
                foreach (var ct in chiTietList)
                {
                    if (ct.MaSerialSP.StartsWith("x-", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] parts = ct.MaSerialSP.Split('-');
                        string maLoaiSP = parts[1].Trim();
                        string sqlInsertSP = "INSERT INTO SanPham (MaSerialSP, MaLoaiSP, MaPhieuNhap, NgayNhap, NgaySX, TrangThai, IsDeleted) " +
                                             "VALUES (@MaSerialSP, @MaLoaiSP, 'PN00000000', GETDATE(), GETDATE(), N'Trong Kho', 0)";
                        using SqlCommand cmdSP = new SqlCommand(sqlInsertSP, conn, tran);
                        cmdSP.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = ct.MaSerialSP });
                        cmdSP.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });
                        cmdSP.ExecuteNonQuery();
                    }
                }

                // Bước 1: INSERT DonHang
                string sqlDH = "INSERT INTO DonHang " +
                    "(MaDH, MaNV, MaKH, MaKM, MaHD, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai, NgayCapNhat, NguoiTao) " +
                    "VALUES (@MaDH, @MaNV, @MaKH, @MaKM, @MaHD, @TongTien, @TienSauGiam, @PhuongThucThanhToan, @TrangThai, @NgayCapNhat, @NguoiTao)";

                using (SqlCommand cmd = new SqlCommand(sqlDH, conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaDH",                SqlDbType.Char, 10)    { Value = dh.MaDH });
                    cmd.Parameters.Add(new SqlParameter("@MaNV",                SqlDbType.Char, 10)    { Value = dh.MaNV });
                    cmd.Parameters.Add(new SqlParameter("@MaKH",                SqlDbType.Char, 10)    { Value = dh.MaKH });
                    cmd.Parameters.Add(new SqlParameter("@MaKM",                SqlDbType.Char, 10)    { Value = string.IsNullOrEmpty(dh.MaKM) ? (object)DBNull.Value : dh.MaKM });
                    cmd.Parameters.Add(new SqlParameter("@MaHD",                SqlDbType.Char, 10)    { Value = string.IsNullOrEmpty(dh.MaHD) ? (object)DBNull.Value : dh.MaHD });
                    cmd.Parameters.Add(new SqlParameter("@TongTien",            SqlDbType.Decimal)     { Value = dh.TongTien, Precision = 15, Scale = 2 });
                    cmd.Parameters.Add(new SqlParameter("@TienSauGiam",         SqlDbType.Decimal)     { Value = dh.TienSauGiam.HasValue ? (object)dh.TienSauGiam.Value : DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@PhuongThucThanhToan", SqlDbType.NVarChar, 100){ Value = dh.PhuongThucThanhToan });
                    cmd.Parameters.Add(new SqlParameter("@TrangThai",           SqlDbType.NVarChar, 50){ Value = dh.TrangThai });
                    cmd.Parameters.Add(new SqlParameter("@NgayCapNhat",         SqlDbType.DateTime)    { Value = DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@NguoiTao",            SqlDbType.Char, 10)    { Value = string.IsNullOrEmpty(dh.NguoiTao) ? (object)DBNull.Value : dh.NguoiTao });
                    cmd.ExecuteNonQuery();
                }

                // Bước 2: INSERT từng dòng ChiTietDonHang
                foreach (var ct in chiTietList)
                {
                    string sqlCT = "INSERT INTO ChiTietDonHang (MaDH, MaSerialSP, GiaBan, PhanTramGiam) " +
                                   "VALUES (@MaDH, @MaSerialSP, @GiaBan, @PhanTramGiam)";
                    using SqlCommand cmd = new SqlCommand(sqlCT, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaDH",        SqlDbType.Char, 10)   { Value = ct.MaDH });
                    cmd.Parameters.Add(new SqlParameter("@MaSerialSP",  SqlDbType.VarChar, 50){ Value = ct.MaSerialSP });
                    cmd.Parameters.Add(new SqlParameter("@GiaBan",      SqlDbType.Decimal)    { Value = ct.GiaBan, Precision = 15, Scale = 2 });
                    cmd.Parameters.Add(new SqlParameter("@PhanTramGiam",SqlDbType.Decimal)    { Value = ct.PhanTramGiam.HasValue ? (object)ct.PhanTramGiam.Value : DBNull.Value });
                    cmd.ExecuteNonQuery();
                }

                // Bước 3: UPDATE TrangThai SanPham → 'Đã Bán'
                foreach (var ct in chiTietList)
                {
                    string sqlSP = "UPDATE SanPham SET TrangThai = N'Đã Bán', NgayCapNhat = GETDATE() " +
                                   "WHERE MaSerialSP = @MaSerialSP AND TrangThai = N'Trong Kho' AND IsDeleted = 0";
                    using SqlCommand cmd = new SqlCommand(sqlSP, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = ct.MaSerialSP });
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                        throw new InvalidOperationException(
                            $"Serial '{ct.MaSerialSP}' không còn ở trạng thái 'Trong Kho'. " +
                            "Có thể đã được bán bởi giao dịch khác. Vui lòng thử lại.");
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
        // PHẦN 3: CHUYỂN TRẠNG THÁI ĐƠN HÀNG
        // ══════════════════════════════════════════════════════════════════

        /// Chuyển đơn hàng sang trạng thái 'Đang Giao'.
        /// Chỉ được chuyển từ 'Chờ Xử Lý'.
        /// <param name="maDH">Mã đơn hàng.
        public bool ChuyenSangDangGiao(string maDH)
        {
            var dh = _LayVaKiemTraDonTonTai(maDH);

            if (!string.IsNullOrEmpty(dh.MaHD))
                throw new InvalidOperationException("Đơn hàng thuộc hợp đồng phải luôn ở trạng thái Chờ Xử Lý cho đến khi hợp đồng hết hạn.");

            if (dh.TrangThai != "Chờ Xử Lý")
                throw new InvalidOperationException(
                    $"Đơn hàng '{maDH}' đang ở '{dh.TrangThai}'. " +
                    "Chỉ chuyển sang 'Đang Giao' từ trạng thái 'Chờ Xử Lý'.");

            try { return _dalDH.CapNhatTrangThai(maDH.Trim(), "Đang Giao"); }
            catch (Exception ex) { throw new Exception($"Lỗi cập nhật trạng thái đơn hàng: {ex.Message}", ex); }
        }

        /// Chuyển đơn hàng sang trạng thái 'Hoàn Thành'.
        /// Chỉ được chuyển từ 'Đang Giao'.
        /// <param name="maDH">Mã đơn hàng.
        public bool HoanThanhDonHang(string maDH)
        {
            var dh = _LayVaKiemTraDonTonTai(maDH);

            if (!string.IsNullOrEmpty(dh.MaHD))
                throw new InvalidOperationException("Đơn hàng thuộc hợp đồng phải luôn ở trạng thái Chờ Xử Lý cho đến khi hợp đồng hết hạn.");

            if (dh.TrangThai != "Đang Giao")
                throw new InvalidOperationException(
                    $"Đơn hàng '{maDH}' đang ở '{dh.TrangThai}'. " +
                    "Chỉ hoàn thành đơn từ trạng thái 'Đang Giao'.");

            try { return _dalDH.CapNhatTrangThai(maDH.Trim(), "Hoàn Thành"); }
            catch (Exception ex) { throw new Exception($"Lỗi hoàn thành đơn hàng: {ex.Message}", ex); }
        }

        /// Hủy đơn hàng — ATOMIC: cập nhật TrangThai DonHang → 'Huỷ'
        /// VÀ khôi phục TrangThai các SanPham → 'Trong Kho' trong cùng 1 transaction.
        /// Chỉ được hủy khi TrangThai = 'Chờ Xử Lý'.
        /// <param name="maDH">Mã đơn hàng cần hủy.
        public bool HuyDonHang(string maDH)
        {
            var dh = _LayVaKiemTraDonTonTai(maDH);

            if (dh.TrangThai != "Chờ Xử Lý")
                throw new InvalidOperationException(
                    $"Không thể hủy đơn hàng '{maDH}' vì đang ở trạng thái '{dh.TrangThai}'. " +
                    "Chỉ được hủy đơn ở trạng thái 'Chờ Xử Lý'.");

            // Lấy danh sách serial của đơn hàng để khôi phục
            var dtCT = _dalCTDH.DSTheoDonHang(maDH.Trim());

            string connStr = ConfigurationManager
                .ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;

            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Bước 1: UPDATE DonHang → 'Huỷ'
                string sqlDH = "UPDATE DonHang SET TrangThai = N'Huỷ', NgayCapNhat = GETDATE() WHERE MaDH = @MaDH";
                using (SqlCommand cmd = new SqlCommand(sqlDH, conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH.Trim() });
                    cmd.ExecuteNonQuery();
                }

                // Bước 2: Khôi phục TrangThai SanPham → 'Trong Kho'
                foreach (DataRow row in dtCT.Rows)
                {
                    string maSerial = row["MaSerialSP"].ToString()!.Trim();
                    string sqlSP = "UPDATE SanPham SET TrangThai = N'Trong Kho', NgayCapNhat = GETDATE() " +
                                   "WHERE MaSerialSP = @MaSerialSP AND TrangThai = N'Đã Bán' AND IsDeleted = 0";
                    using SqlCommand cmd = new SqlCommand(sqlSP, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });
                    cmd.ExecuteNonQuery();
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
        // PHẦN 4: TIỆN ÍCH
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách serial đang 'Trong Kho' của một loại sản phẩm, để hiển thị
        /// cho nhân viên chọn khi tạo đơn.
        /// <param name="maLoaiSP">Mã loại sản phẩm.
        public DataTable LaySerialTonKhoTheoLoaiSP(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");

            var dsTatCa = _dalSP.DSTheoLoaiSP(maLoaiSP.Trim());
            var dt = dsTatCa.Clone();
            foreach (DataRow row in dsTatCa.Rows)
                if (row["TrangThai"].ToString() == "Trong Kho" &&
                    Convert.ToBoolean(row["IsDeleted"]) == false)
                    dt.ImportRow(row);
            return dt;
        }

        /// Tính trước tiền giảm và TienSauGiam khi nhân viên chọn KM để xem trước kết quả.
        /// <param name="dh">DTO_DonHang với MaKH, NgayDat đã điền.
        /// <param name="chiTietTam">Danh sách CTDH tạm (GiaBan đã lấy từ LoaiSanPham).
        /// <param name="maKMChon">Mã KM muốn áp dụng (null = tự động chọn).
        /// Trả về (decimal tongTien, decimal tienGiam, decimal tienSauGiam, string maKMApDung).
        public (decimal tongTien, decimal tienGiam, decimal tienSauGiam, string maKMApDung)
            XemTruocGiaDon(DTO_DonHang dh, List<DTO_ChiTietDonHang> chiTietTam, string? maKMChon = null)
        {
            if (dh == null || chiTietTam == null || chiTietTam.Count == 0)
                return (0m, 0m, 0m, string.Empty);

            decimal tongTien = 0m;
            foreach (var ct in chiTietTam)
                tongTien += ct.GiaBan;

            dh.TongTien = tongTien;
            dh.NgayDat  = dh.NgayDat == default ? DateTime.Now : dh.NgayDat;

            decimal tienGiam;
            string  maApDung;

            if (!string.IsNullOrEmpty(maKMChon))
            {
                var km = _busKM.LayTheoMa(maKMChon.Trim());
                var kh = _dalKH.DSTheoMaKH(dh.MaKH);
                if (km != null && kh != null &&
                    _busKM.KiemTraDieuKienKM(km, kh, chiTietTam, dh.NgayDat))
                {
                    tienGiam = _busKM.TinhTienGiam(km, chiTietTam);
                    maApDung = km.MaKM;
                }
                else
                {
                    tienGiam = 0m;
                    maApDung = string.Empty;
                }
            }
            else
            {
                (tienGiam, maApDung) = _busKM.TinhKhuyenMai(dh, chiTietTam);
            }

            decimal tienSauGiam = Math.Max(0m, tongTien - tienGiam);
            return (tongTien, tienGiam, tienSauGiam, maApDung);
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 5: KIỂM TRA DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════

        /// Kiểm tra TrangThai đơn hàng hợp lệ.
        public void KiemTraTrangThaiDH(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                throw new ArgumentException("Trạng thái đơn hàng không được để trống.");
            bool hopLe = false;
            foreach (var tt in TRANG_THAI_HOP_LE)
                if (tt == trangThai) { hopLe = true; break; }
            if (!hopLe)
                throw new ArgumentException(
                    $"Trạng thái '{trangThai}' không hợp lệ. " +
                    "Chỉ nhận: 'Chờ Xử Lý', 'Đang Giao', 'Hoàn Thành', 'Huỷ'.");
        }

        /// Kiểm tra phương thức thanh toán hợp lệ.
        public void KiemTraPhuongThuc(string phuongThuc)
        {
            if (string.IsNullOrWhiteSpace(phuongThuc))
                throw new ArgumentException("Phương thức thanh toán không được để trống.");
            bool hopLe = false;
            foreach (var pt in PHUONG_THUC_HOP_LE)
                if (pt == phuongThuc) { hopLe = true; break; }
            if (!hopLe)
                throw new ArgumentException(
                    $"Phương thức '{phuongThuc}' không hợp lệ. " +
                    "Chỉ nhận: 'Tiền Mặt', 'Chuyển Khoản', 'Thẻ'.");
        }

        /// Sinh mã đơn hàng mới dạng DHXXXXXXXX tự động tăng tiến.
        public string TaoMaDHMoi()
        {
            string? maMax = _dalDH.LayMaDHMoiNhat();
            int soTiepTheo = 1;
            if (!string.IsNullOrWhiteSpace(maMax) && maMax.StartsWith("DH") && maMax.Length == 10)
            {
                if (int.TryParse(maMax.Substring(2), out int soHienTai))
                    soTiepTheo = soHienTai + 1;
            }
            return "DH" + soTiepTheo.ToString().PadLeft(8, '0');
        }

        /// Tìm kiếm đơn hàng theo nhiều điều kiện kết hợp (lọc phía ứng dụng).
        /// Các tham số nào để null/rỗng sẽ không được áp dụng làm bộ lọc.
        /// maLoaiSP: lọc theo DanhMuc của LoaiSanPham (tra qua ChiTietDonHang → SanPham → LoaiSanPham).
        public DataTable TimKiemNhieuDieuKien(
            string? maKH              = null,
            string? tenNhanVien       = null,
            string? phuongThucThanhToan = null,
            string? maKM              = null,
            string? maHD              = null,
            string? maDH              = null,
            string? maLoaiSP          = null,
            string? trangThai         = null)
        {
            // Lấy toàn bộ đơn hàng rồi lọc phía ứng dụng
            var dtTatCa = _dalDH.DSTatCaDonHang();

            // Lấy thêm thông tin NhanVien nếu cần lọc theo tên
            System.Collections.Generic.Dictionary<string, string> maNVtoTen
                = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(tenNhanVien))
            {
                var dalNV = new DAL_HTQLCuaHangLaptop.DAL_NhanVien();
                var dtNV  = dalNV.DSTatCaNhanVien();
                foreach (System.Data.DataRow row in dtNV.Rows)
                    maNVtoTen[row["MaNV"].ToString()!.Trim()] = row["TenNV"].ToString()!;
            }

            // Set mã đơn hàng cần lọc theo loại sản phẩm (tra qua CTDH)
            System.Collections.Generic.HashSet<string> maDHCoLoaiSP
                = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(maLoaiSP))
            {
                // Lấy tất cả serial thuộc loại SP đó
                var dsSP = _dalSP.DSTheoLoaiSP(maLoaiSP.Trim());
                var serialSet = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (System.Data.DataRow row in dsSP.Rows)
                    serialSet.Add(row["MaSerialSP"].ToString()!.Trim());

                if (serialSet.Count > 0)
                {
                    // Lấy tất cả CTDH và tìm các MaDH có serial thuộc loại SP đó
                    var dtAllCTDH = _dalCTDH.DSTatCaChiTietDonHang();
                    foreach (System.Data.DataRow row in dtAllCTDH.Rows)
                    {
                        string serial = row["MaSerialSP"].ToString()!.Trim();
                        if (serialSet.Contains(serial))
                            maDHCoLoaiSP.Add(row["MaDH"].ToString()!.Trim());
                    }
                }
            }

            // Lọc
            var dtKetQua = dtTatCa.Clone();
            foreach (System.Data.DataRow row in dtTatCa.Rows)
            {
                // Lọc theo mã khách hàng
                if (!string.IsNullOrWhiteSpace(maKH))
                {
                    string val = row["MaKH"].ToString()!.Trim();
                    if (!val.StartsWith(maKH.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                }

                // Lọc theo tên nhân viên
                if (!string.IsNullOrWhiteSpace(tenNhanVien))
                {
                    string maNV = row["MaNV"].ToString()!.Trim();
                    if (!maNVtoTen.TryGetValue(maNV, out string? ten) ||
                        !ten.Contains(tenNhanVien.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                }

                // Lọc theo phương thức thanh toán
                if (!string.IsNullOrWhiteSpace(phuongThucThanhToan) && !phuongThucThanhToan.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
                {
                    string val = row["PhuongThucThanhToan"].ToString()!;
                    if (!val.Equals(phuongThucThanhToan.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                }

                // Lọc theo mã khuyến mãi
                if (!string.IsNullOrWhiteSpace(maKM) && !maKM.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
                {
                    if (maKM.Equals("Không khuyến mãi", StringComparison.OrdinalIgnoreCase))
                    {
                        if (row["MaKM"] != System.DBNull.Value && !string.IsNullOrWhiteSpace(row["MaKM"].ToString()))
                            continue;
                    }
                    else
                    {
                        string val = row["MaKM"] == System.DBNull.Value ? "" : row["MaKM"].ToString()!.Trim();
                        if (!val.StartsWith(maKM.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                    }
                }

                // Lọc theo mã hợp đồng
                if (!string.IsNullOrWhiteSpace(maHD))
                {
                    string val = row["MaHD"] == System.DBNull.Value ? "" : row["MaHD"].ToString()!.Trim();
                    if (!val.StartsWith(maHD.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                }

                // Lọc theo mã đơn hàng
                if (!string.IsNullOrWhiteSpace(maDH))
                {
                    string val = row["MaDH"].ToString()!.Trim();
                    if (!val.StartsWith(maDH.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                }

                // Lọc theo loại sản phẩm
                if (!string.IsNullOrWhiteSpace(maLoaiSP))
                {
                    string madh = row["MaDH"].ToString()!.Trim();
                    if (!maDHCoLoaiSP.Contains(madh)) continue;
                }

                // Lọc theo trạng thái
                if (!string.IsNullOrWhiteSpace(trangThai) && !trangThai.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
                {
                    string val = row["TrangThai"].ToString()!;
                    if (!val.Equals(trangThai.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                }

                dtKetQua.ImportRow(row);
            }

            return dtKetQua;
        }

        /// Lấy danh sách toàn bộ sản phẩm (kèm thông tin LoaiSanPham) trong toàn bộ CSDL.
        /// Dùng để hiển thị tab "Danh sách sản phẩm".
        public DataTable LayDanhSachSanPhamDaDan()
        {
            try { return _dalSP.DSTatCaSanPham(); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy danh sách sản phẩm: {ex.Message}", ex); }
        }

        /// Lấy danh sách sản phẩm tồn kho (Trong Kho) theo loại sản phẩm.
        public DataTable LaySanPhamTonKhoTheoLoaiSP(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");
            try { return LaySerialTonKhoTheoLoaiSP(maLoaiSP); }
            catch (Exception ex) { throw new Exception($"Lỗi lấy sản phẩm tồn kho: {ex.Message}", ex); }
        }

        /// Cập nhật giới hạn đơn hàng: chỉ được sửa MaKM, PhuongThucThanhToan và TrangThai.
        /// Kiểm tra TrangThai hợp lệ trước khi cập nhật.
        public bool CapNhatGioiHan(string maDH, string? maKMMoi, string phuongThucMoi, string trangThaiMoi, string? maNVMoi = null)
        {
            var dh = _LayVaKiemTraDonTonTai(maDH);
            string trangThaiCu = dh.TrangThai;

            if (!string.IsNullOrEmpty(dh.MaHD) && trangThaiMoi != "Chờ Xử Lý" && trangThaiMoi != "Huỷ")
            {
                throw new InvalidOperationException("Đơn hàng thuộc hợp đồng phải luôn ở trạng thái Chờ Xử Lý cho đến khi hợp đồng hết hạn.");
            }

            KiemTraPhuongThuc(phuongThucMoi);
            KiemTraTrangThaiDH(trangThaiMoi);

            // Cập nhật các trường cho phép
            dh.PhuongThucThanhToan = phuongThucMoi;
            dh.TrangThai = trangThaiMoi;
            if (!string.IsNullOrEmpty(maNVMoi))
            {
                dh.MaNV = maNVMoi.Trim();
            }
            dh.NgayCapNhat = DateTime.Now;

            // Tính lại khuyến mãi
            decimal tongTien = 0m;
            var dtCT = _dalCTDH.DSTheoDonHang(maDH.Trim());
            var chiTietList = new List<DTO_ChiTietDonHang>();
            foreach (DataRow row in dtCT.Rows)
            {
                decimal giaBan = Convert.ToDecimal(row["GiaBan"]);
                tongTien += giaBan;
                chiTietList.Add(new DTO_ChiTietDonHang
                {
                    MaDH = maDH.Trim(),
                    MaSerialSP = row["MaSerialSP"].ToString()!.Trim(),
                    GiaBan = giaBan,
                    PhanTramGiam = null
                });
            }

            dh.TongTien = tongTien;
            dh.MaKM = string.IsNullOrWhiteSpace(maKMMoi) || maKMMoi.Trim().Equals("NONE", StringComparison.OrdinalIgnoreCase) ? null! : maKMMoi.Trim();

            decimal tienGiam = 0m;
            if (!string.IsNullOrEmpty(dh.MaKM))
            {
                var km = _busKM.LayTheoMa(dh.MaKM);
                var kh = _dalKH.DSTheoMaKH(dh.MaKH);
                if (km == null)
                    throw new InvalidOperationException($"Khuyến mãi '{dh.MaKM}' không tồn tại.");
                if (kh == null)
                    throw new InvalidOperationException("Không tìm thấy thông tin khách hàng.");

                if (_busKM.KiemTraDieuKienKM(km, kh, chiTietList, dh.NgayDat))
                {
                    tienGiam = _busKM.TinhTienGiam(km, chiTietList);
                    if (km.MucGiamSP.HasValue && km.MucGiamSP.Value > 0)
                    {
                        var dalSPTemp = new DAL_SanPham();
                        foreach (var ct in chiTietList)
                        {
                            bool apDung = true;
                            if (!string.IsNullOrWhiteSpace(km.DieuKien))
                            {
                                var sp2 = dalSPTemp.DSTheoMaSerialSP(ct.MaSerialSP);
                                var lsp2 = sp2 != null ? _dalLSP.TimLoaiSP(sp2.MaLoaiSP) : null;
                                apDung = lsp2 != null && lsp2.DanhMuc == km.DieuKien;
                            }
                            if (apDung)
                                ct.PhanTramGiam = km.MucGiamSP;
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Đơn hàng không đủ điều kiện áp dụng chương trình khuyến mãi này.");
                }
            }

            dh.TienSauGiam = tongTien - tienGiam;
            if (dh.TienSauGiam < 0) dh.TienSauGiam = 0m;

            // Thực thi Transaction cập nhật
            string connStr = ConfigurationManager.ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;

            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // 1. UPDATE DonHang
                string sqlDH = "UPDATE DonHang SET MaKM = @MaKM, TongTien = @TongTien, TienSauGiam = @TienSauGiam, " +
                               "PhuongThucThanhToan = @PhuongThucThanhToan, TrangThai = @TrangThai, " +
                               "MaNV = @MaNV, NgayCapNhat = @NgayCapNhat WHERE MaDH = @MaDH";
                using (SqlCommand cmd = new SqlCommand(sqlDH, conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });
                    cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(dh.MaKM) ? (object)DBNull.Value : dh.MaKM });
                    cmd.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = dh.TongTien, Precision = 15, Scale = 2 });
                    cmd.Parameters.Add(new SqlParameter("@TienSauGiam", SqlDbType.Decimal) { Value = dh.TienSauGiam.HasValue ? (object)dh.TienSauGiam.Value : DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@PhuongThucThanhToan", SqlDbType.NVarChar, 100) { Value = dh.PhuongThucThanhToan });
                    cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = dh.TrangThai });
                    cmd.Parameters.Add(new SqlParameter("@MaNV", SqlDbType.Char, 10) { Value = dh.MaNV });
                    cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime) { Value = dh.NgayCapNhat });
                    cmd.ExecuteNonQuery();
                }

                // 2. UPDATE ChiTietDonHang
                foreach (var ct in chiTietList)
                {
                    string sqlCT = "UPDATE ChiTietDonHang SET PhanTramGiam = @PhanTramGiam WHERE MaDH = @MaDH AND MaSerialSP = @MaSerialSP";
                    using SqlCommand cmd = new SqlCommand(sqlCT, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = ct.MaDH });
                    cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = ct.MaSerialSP });
                    cmd.Parameters.Add(new SqlParameter("@PhanTramGiam", SqlDbType.Decimal) { Value = ct.PhanTramGiam.HasValue ? (object)ct.PhanTramGiam.Value : DBNull.Value });
                    cmd.ExecuteNonQuery();
                }

                // 3. Nếu chuyển trạng thái sang "Huỷ"
                if (trangThaiMoi == "Huỷ" && trangThaiCu != "Huỷ")
                {
                    foreach (var ct in chiTietList)
                    {
                        string sqlSP = "UPDATE SanPham SET TrangThai = N'Trong Kho', NgayCapNhat = GETDATE() " +
                                       "WHERE MaSerialSP = @MaSerialSP AND TrangThai = N'Đã Bán' AND IsDeleted = 0";
                        using SqlCommand cmd = new SqlCommand(sqlSP, conn, tran);
                        cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = ct.MaSerialSP });
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception($"Lỗi thực hiện transaction cập nhật đơn hàng: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách chi tiết đơn hàng toàn bộ (dùng để hiển thị bảng chi tiết).
        public DataTable LayTatCaChiTietDonHang()
        {
            try
            {
                // Lấy tất cả CTDH kèm thông tin sản phẩm từ bảng JOIN
                var dtAll = _dalDH.DSTatCaDonHang();
                // Ta cần tổng hợp từ từng đơn hàng
                var dtResult = new System.Data.DataTable();
                dtResult.Columns.Add("MaDH");
                dtResult.Columns.Add("MaSerialSP");
                dtResult.Columns.Add("GiaBan");
                dtResult.Columns.Add("PhanTramGiam");
                dtResult.Columns.Add("MaLoaiSP");
                dtResult.Columns.Add("TenLoai");

                foreach (System.Data.DataRow row in dtAll.Rows)
                {
                    string madh = row["MaDH"].ToString()!.Trim();
                    var dtCT = _dalCTDH.DSChiTietCoThongTinSanPham(madh);
                    foreach (System.Data.DataRow ct in dtCT.Rows)
                    {
                        var newRow = dtResult.NewRow();
                        newRow["MaDH"]        = madh;
                        newRow["MaSerialSP"]  = ct["MaSerialSP"];
                        newRow["GiaBan"]      = ct["GiaBan"];
                        newRow["PhanTramGiam"]= ct["PhanTramGiam"];
                        newRow["MaLoaiSP"]    = ct.Table.Columns.Contains("MaLoaiSP") ? ct["MaLoaiSP"] : (ct.Table.Columns.Contains("DanhMuc") ? ct["DanhMuc"] : "");
                        newRow["TenLoai"]     = ct.Table.Columns.Contains("TenLoai") ? ct["TenLoai"] : "";
                        dtResult.Rows.Add(newRow);
                    }
                }
                return dtResult;
            }
            catch (Exception ex) { throw new Exception($"Lỗi lấy chi tiết đơn hàng: {ex.Message}", ex); }
        }

        /// Lấy và kiểm tra đơn hàng tồn tại — helper dùng nội bộ.
        private DTO_DonHang _LayVaKiemTraDonTonTai(string maDH)
        {
            if (string.IsNullOrWhiteSpace(maDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            var dh = _dalDH.DSTheoMaDH(maDH.Trim());
            if (dh == null)
                throw new InvalidOperationException($"Đơn hàng '{maDH}' không tồn tại.");
            return dh;
        }

        /// Xóa một dòng chi tiết đơn hàng (chỉ khi đơn hàng đang 'Chờ Xử Lý').
        /// Khôi phục trạng thái sản phẩm về 'Trong Kho' và cập nhật lại tiền đơn hàng.
        public bool XoaDongChiTietDonHang(string maDH, string maSerialSP)
        {
            var dh = _LayVaKiemTraDonTonTai(maDH);
            if (dh.TrangThai != "Chờ Xử Lý")
                throw new InvalidOperationException("Chỉ được phép xóa chi tiết đơn hàng của đơn hàng đang ở trạng thái 'Chờ Xử Lý'.");

            string connStr = ConfigurationManager.ConnectionStrings["HTQLCuaHangLaptopDB"].ConnectionString;
            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // 1. Xóa dòng trong ChiTietDonHang
                string sqlDelete = "DELETE FROM ChiTietDonHang WHERE MaDH = @MaDH AND MaSerialSP = @MaSerialSP";
                int rowsDeleted = 0;
                using (SqlCommand cmd = new SqlCommand(sqlDelete, conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });
                    cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerialSP });
                    rowsDeleted = cmd.ExecuteNonQuery();
                }

                if (rowsDeleted == 0)
                    throw new InvalidOperationException("Không tìm thấy dòng chi tiết đơn hàng cần xóa.");

                // 2. Khôi phục trạng thái sản phẩm về 'Trong Kho'
                if (!maSerialSP.StartsWith("x-", StringComparison.OrdinalIgnoreCase))
                {
                    string sqlSP = "UPDATE SanPham SET TrangThai = N'Trong Kho', NgayCapNhat = GETDATE() WHERE MaSerialSP = @MaSerialSP AND IsDeleted = 0";
                    using (SqlCommand cmd = new SqlCommand(sqlSP, conn, tran))
                    {
                        cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerialSP });
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Nếu là serial ảo, xóa hẳn sản phẩm ảo khỏi CSDL
                    string sqlSP = "DELETE FROM SanPham WHERE MaSerialSP = @MaSerialSP";
                    using (SqlCommand cmd = new SqlCommand(sqlSP, conn, tran))
                    {
                        cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerialSP });
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                
                // 3. Cập nhật lại tiền đơn hàng
                CapNhatLaiTienDonHang(maDH);

                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private void CapNhatLaiTienDonHang(string maDH)
        {
            var dh = _dalDH.DSTheoMaDH(maDH);
            if (dh == null) return;

            var dtCT = _dalCTDH.DSTheoDonHang(maDH);
            decimal tongTien = 0m;
            var chiTietList = new List<DTO_ChiTietDonHang>();
            foreach (DataRow row in dtCT.Rows)
            {
                decimal giaBan = Convert.ToDecimal(row["GiaBan"]);
                tongTien += giaBan;
                chiTietList.Add(new DTO_ChiTietDonHang
                {
                    MaDH = maDH,
                    MaSerialSP = row["MaSerialSP"].ToString()!.Trim(),
                    GiaBan = giaBan,
                    PhanTramGiam = row["PhanTramGiam"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row["PhanTramGiam"])
                });
            }

            dh.TongTien = tongTien;
            
            decimal tienGiam = 0m;
            if (!string.IsNullOrEmpty(dh.MaKM))
            {
                var km = _busKM.LayTheoMa(dh.MaKM);
                var kh = _dalKH.DSTheoMaKH(dh.MaKH);
                if (km != null && kh != null && _busKM.KiemTraDieuKienKM(km, kh, chiTietList, dh.NgayDat))
                {
                    tienGiam = _busKM.TinhTienGiam(km, chiTietList);
                }
                else
                {
                    dh.MaKM = null!; // Hủy KM nếu không đủ điều kiện
                }
            }

            dh.TienSauGiam = Math.Max(0m, tongTien - tienGiam);

            string connStr = ConfigurationManager.ConnectionStrings["HTQLCuaHangLaptopDB"].ConnectionString;
            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            string sqlUpdate = "UPDATE DonHang SET TongTien = @TongTien, TienSauGiam = @TienSauGiam, MaKM = @MaKM WHERE MaDH = @MaDH";
            using SqlCommand cmd = new SqlCommand(sqlUpdate, conn);
            cmd.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });
            cmd.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = dh.TongTien });
            cmd.Parameters.Add(new SqlParameter("@TienSauGiam", SqlDbType.Decimal) { Value = dh.TienSauGiam.HasValue ? (object)dh.TienSauGiam.Value : DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@MaKM", SqlDbType.Char, 10) { Value = string.IsNullOrEmpty(dh.MaKM) ? (object)DBNull.Value : dh.MaKM });
            cmd.ExecuteNonQuery();
        }

        /// Thêm sản phẩm mới vào một đơn hàng hiện có (chỉ khi đơn hàng đang 'Chờ Xử Lý').
        public bool ThemSanPhamVaoDonHangHienCo(string maDH, string maLoaiSP, List<string> listSerials)
        {
            var dh = _LayVaKiemTraDonTonTai(maDH);
            if (dh.TrangThai != "Chờ Xử Lý")
                throw new InvalidOperationException("Chỉ được phép thêm sản phẩm vào đơn hàng đang ở trạng thái 'Chờ Xử Lý'.");

            var lsp = _dalLSP.TimLoaiSP(maLoaiSP);
            if (lsp == null)
                throw new InvalidOperationException("Loại sản phẩm không tồn tại.");

            decimal giaBan = lsp.GiaBanGoc;

            string connStr = ConfigurationManager.ConnectionStrings["HTQLCuaHangLaptopDB"].ConnectionString;
            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Ensure PN00000000 dummy exists
                string sqlEnsurePN = @"
                    IF NOT EXISTS (SELECT 1 FROM PhieuNhap WHERE MaPhieuNhap = 'PN00000000')
                    BEGIN
                        DECLARE @DummyNV CHAR(10) = (SELECT TOP 1 MaNV FROM NhanVien);
                        DECLARE @DummyNCC CHAR(10) = (SELECT TOP 1 MaNCC FROM NhaCungCap);
                        INSERT INTO PhieuNhap (MaPhieuNhap, MaNV, MaNCC, NgayNhap, TongTien, TrangThai)
                        VALUES ('PN00000000', @DummyNV, @DummyNCC, GETDATE(), 0, N'Đã Nhập');
                    END";
                using (SqlCommand cmdEnsurePN = new SqlCommand(sqlEnsurePN, conn, tran))
                {
                    cmdEnsurePN.ExecuteNonQuery();
                }

                foreach (var serial in listSerials)
                {
                    // 1. Nếu là serial ảo, chèn sản phẩm ảo vào DB
                    if (serial.StartsWith("x-", StringComparison.OrdinalIgnoreCase))
                    {
                        string sqlInsertSP = "INSERT INTO SanPham (MaSerialSP, MaLoaiSP, MaPhieuNhap, NgayNhap, NgaySX, TrangThai, IsDeleted) " +
                                             "VALUES (@MaSerialSP, @MaLoaiSP, 'PN00000000', GETDATE(), GETDATE(), N'Trong Kho', 0)";
                        using SqlCommand cmdSP = new SqlCommand(sqlInsertSP, conn, tran);
                        cmdSP.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = serial });
                        cmdSP.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });
                        cmdSP.ExecuteNonQuery();
                    }

                    // 2. Chèn vào ChiTietDonHang
                    string sqlInsertCT = "INSERT INTO ChiTietDonHang (MaDH, MaSerialSP, GiaBan, PhanTramGiam) VALUES (@MaDH, @MaSerialSP, @GiaBan, @PhanTramGiam)";
                    using (SqlCommand cmdCT = new SqlCommand(sqlInsertCT, conn, tran))
                    {
                        cmdCT.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });
                        cmdCT.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = serial });
                        cmdCT.Parameters.Add(new SqlParameter("@GiaBan", SqlDbType.Decimal) { Value = giaBan });
                        cmdCT.Parameters.Add(new SqlParameter("@PhanTramGiam", SqlDbType.Decimal) { Value = dh.MaKM != null ? (object)Convert.ToDecimal(0) : DBNull.Value });
                        cmdCT.ExecuteNonQuery();
                    }

                    // 3. Cập nhật trạng thái sản phẩm sang 'Đã Bán'
                    string sqlUpdateSP = "UPDATE SanPham SET TrangThai = N'Đã Bán', NgayCapNhat = GETDATE() WHERE MaSerialSP = @MaSerialSP AND TrangThai = N'Trong Kho' AND IsDeleted = 0";
                    using (SqlCommand cmdSP = new SqlCommand(sqlUpdateSP, conn, tran))
                    {
                        cmdSP.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = serial });
                        int rows = cmdSP.ExecuteNonQuery();
                        if (rows == 0)
                            throw new InvalidOperationException($"Sản phẩm serial '{serial}' không khả dụng.");
                    }
                }

                tran.Commit();

                // 4. Cập nhật lại tiền đơn hàng và khuyến mãi
                CapNhatLaiTienDonHang(maDH);

                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}

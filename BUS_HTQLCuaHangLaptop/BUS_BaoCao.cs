using System;
using System.Collections.Generic;
using System.Data;
using DAL_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    
    /// Lớp nghiệp vụ báo cáo thống kê hệ thống, bao gồm 3 nhóm chính:
    /// - Báo cáo tồn kho  — thống kê số lượng sản phẩm theo trạng thái và danh mục.
    /// - Báo cáo nhập hàng — danh sách phiếu nhập lọc theo thời gian / trạng thái.
    /// - Báo cáo doanh thu — thống kê đơn hàng hoàn thành, tổng hợp theo tháng/năm.
    ///
    /// Nguyên tắc: lấy raw data từ các DAL, toàn bộ tính toán / lọc / tổng hợp
    /// thực hiện tại lớp BUS này. Không có câu SQL và không import SqlClient.
    /// Trả về DataTable để GUI/Controller bind trực tiếp vào DataGridView hoặc View.
    
    public class BUS_BaoCao
    {
         
        // DAL instances
         
        private readonly DAL_SanPham         _dalSP   = new DAL_SanPham();
        private readonly DAL_LoaiSanPham     _dalLSP  = new DAL_LoaiSanPham();
        private readonly DAL_DonHang         _dalDH   = new DAL_DonHang();
        private readonly DAL_ChiTietDonHang  _dalCTDH = new DAL_ChiTietDonHang();
        private readonly DAL_PhieuNhap       _dalPN   = new DAL_PhieuNhap();

        // ══════════════════════════════════════════════════════════════════════════════
        //  KHU VỰC: BÁO CÁO TỒN KHO
        // ══════════════════════════════════════════════════════════════════════════════

        
        /// Tạo báo cáo tồn kho tổng hợp theo từng LoaiSanPham.
        /// Cột kết quả: TenHang, TenLoai, DanhMuc, GiaBanGoc,
        ///              SoLuongTonKho, SoLuongDaBan, SoLuongBaoHanh, TongSoLuong.
        /// Chỉ lấy LoaiSanPham và HangSanXuat có IsDeleted = 0 (đã được DAL lọc sẵn).
        
        /// <param name="danhMucLoc">
        /// Danh mục cần lọc: 'Laptop' | 'Chuột' | 'Bàn Phím'.
        /// Truyền null hoặc chuỗi rỗng để lấy tất cả danh mục.
        
        /// Trả về DataTable báo cáo tồn kho đã tổng hợp.
        public DataTable BaoCaoTonKho(string? danhMucLoc = null)
        {
            try
            {
                // Lấy danh sách LoaiSanPham (IsDeleted = 0 — đã lọc trong DAL)
                DataTable dtLoai = string.IsNullOrWhiteSpace(danhMucLoc)
                    ? _dalLSP.DSLoaiSP()
                    : _dalLSP.DSLoaiSPTheoDanhMuc(danhMucLoc);

                // Lấy toàn bộ SanPham (IsDeleted = 0 — đã lọc trong DAL)
                DataTable dtSP = _dalSP.DSTatCaSanPham();

                // Xây dựng bảng kết quả
                DataTable dtKetQua = TaoCauTrucTonKho();

                foreach (DataRow rowLoai in dtLoai.Rows)
                {
                    string maLoaiSP  = rowLoai["MaLoaiSP"]?.ToString()?.Trim() ?? string.Empty;
                    string tenLoai   = rowLoai["TenLoai"]?.ToString() ?? string.Empty;
                    string danhMuc   = rowLoai["DanhMuc"]?.ToString() ?? string.Empty;
                    decimal giaBanGoc = rowLoai["GiaBanGoc"] != DBNull.Value
                        ? Convert.ToDecimal(rowLoai["GiaBanGoc"]) : 0m;

                    // Đếm số lượng serial theo từng TrangThai trong LoaiSanPham này
                    int tonKho    = 0;
                    int daBan     = 0;
                    int baoHanh   = 0;
                    int doiTra    = 0;
                    int loi       = 0;

                    foreach (DataRow rowSP in dtSP.Rows)
                    {
                        string maLoaiSPSP = rowSP["MaLoaiSP"]?.ToString()?.Trim() ?? string.Empty;
                        if (!maLoaiSPSP.Equals(maLoaiSP, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string trangThai = rowSP["TrangThai"]?.ToString() ?? string.Empty;
                        switch (trangThai)
                        {
                            case "Trong Kho":  tonKho++;  break;
                            case "Đã Bán":     daBan++;   break;
                            case "Bảo Hành":   baoHanh++; break;
                            case "Đổi Trả":    doiTra++;  break;
                            case "Lỗi":        loi++;     break;
                        }
                    }

                    int tongSoLuong = tonKho + daBan + baoHanh + doiTra + loi;

                    // Chỉ thêm dòng nếu có ít nhất 1 serial thuộc LoaiSanPham này
                    if (tongSoLuong == 0)
                        continue;

                    // Lấy TenHang từ bảng LoaiSanPham (join thủ công)
                    string tenHang = LayTenHangTuLoai(rowLoai);

                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["TenHang"]          = tenHang;
                    rowKQ["TenLoai"]          = tenLoai;
                    rowKQ["DanhMuc"]          = danhMuc;
                    rowKQ["GiaBanGoc"]        = giaBanGoc;
                    rowKQ["SoLuongTonKho"]    = tonKho;
                    rowKQ["SoLuongDaBan"]     = daBan;
                    rowKQ["SoLuongBaoHanh"]   = baoHanh;
                    rowKQ["TongSoLuong"]      = tongSoLuong;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo báo cáo tồn kho: " + ex.Message, ex);
            }
        }

        
        /// Tạo cấu trúc DataTable kết quả cho báo cáo tồn kho.
        private DataTable TaoCauTrucTonKho()
        {
            DataTable dt = new DataTable("BaoCaoTonKho");
            dt.Columns.Add("TenHang",        typeof(string));
            dt.Columns.Add("TenLoai",        typeof(string));
            dt.Columns.Add("DanhMuc",        typeof(string));
            dt.Columns.Add("GiaBanGoc",      typeof(decimal));
            dt.Columns.Add("SoLuongTonKho",  typeof(int));
            dt.Columns.Add("SoLuongDaBan",   typeof(int));
            dt.Columns.Add("SoLuongBaoHanh", typeof(int));
            dt.Columns.Add("TongSoLuong",    typeof(int));
            return dt;
        }

        
        /// Trích xuất TenHang từ một DataRow của bảng LoaiSanPham.
        /// DAL_LoaiSanPham trả về cột MaHang; vì BUS không có DAL_HangSanXuat
        /// và dữ liệu join đã có trong BUS_SanPham, ở đây trả về MaHang tạm thời.
        /// GUI có thể gọi thêm BUS_SanPham.LayTenHang() nếu cần hiển thị tên đầy đủ.
        /// Để tự túc, phương thức này trả MaHang kèm dấu ngoặc để phân biệt.
        
        private string LayTenHangTuLoai(DataRow rowLoai)
        {
            // DAL_LoaiSanPham có cột MaHang (CHAR 10); tên hãng cần join HangSanXuat.
            // Vì BUS_BaoCao không inject DAL_HangSanXuat (tránh phụ thuộc thừa),
            // trả về MaHang. Nếu GUI cần TenHang đẹp, dùng BUS_SanPham hoặc
            // thêm DAL_HangSanXuat vào lớp này.
            return rowLoai["MaHang"]?.ToString()?.Trim() ?? string.Empty;
        }

        
        /// Tóm tắt thống kê tồn kho theo từng danh mục.
        /// Trả về DataTable gồm: DanhMuc, SoLoaiSP, TongTonKho, TongDaBan, TongBaoHanh, TongTatCa.
        /// Trả về DataTable thống kê tổng hợp theo danh mục.
        public DataTable ThongKeTonKhoTheoDanhMuc()
        {
            try
            {
                DataTable dtKetQua = new DataTable("ThongKeTonKhoTheoDanhMuc");
                dtKetQua.Columns.Add("DanhMuc",      typeof(string));
                dtKetQua.Columns.Add("SoLoaiSP",     typeof(int));
                dtKetQua.Columns.Add("TongTonKho",   typeof(int));
                dtKetQua.Columns.Add("TongDaBan",    typeof(int));
                dtKetQua.Columns.Add("TongBaoHanh",  typeof(int));
                dtKetQua.Columns.Add("TongTatCa",    typeof(int));

                string[] danhMucList = { "Laptop", "Chuột", "Bàn Phím" };
                foreach (string dm in danhMucList)
                {
                    DataTable dtTonKho = BaoCaoTonKho(dm);
                    int soLoai   = dtTonKho.Rows.Count;
                    int tongTK   = 0, tongDB = 0, tongBH = 0, tongAll = 0;

                    foreach (DataRow row in dtTonKho.Rows)
                    {
                        tongTK  += Convert.ToInt32(row["SoLuongTonKho"]);
                        tongDB  += Convert.ToInt32(row["SoLuongDaBan"]);
                        tongBH  += Convert.ToInt32(row["SoLuongBaoHanh"]);
                        tongAll += Convert.ToInt32(row["TongSoLuong"]);
                    }

                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["DanhMuc"]     = dm;
                    rowKQ["SoLoaiSP"]    = soLoai;
                    rowKQ["TongTonKho"]  = tongTK;
                    rowKQ["TongDaBan"]   = tongDB;
                    rowKQ["TongBaoHanh"] = tongBH;
                    rowKQ["TongTatCa"]   = tongAll;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê tồn kho theo danh mục: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════════════════════════════════════════
        //  KHU VỰC: BÁO CÁO NHẬP HÀNG
        // ══════════════════════════════════════════════════════════════════════════════

        
        /// Tạo báo cáo nhập hàng, lọc theo khoảng thời gian và / hoặc trạng thái.
        /// Cột kết quả: MaPhieuNhap, NgayNhap, MaNCC, MaNV,
        ///              TongTien, TrangThai, SoLoaiSP, TongSoLuong.
        /// Lưu ý: TenNCC và TenNV hiển thị bằng MaNCC / MaNV vì BUS_BaoCao không inject
        /// DAL_NhaCungCap và DAL_NhanVien. GUI có thể tra cứu thêm nếu cần tên đầy đủ.
        
        /// <param name="tuNgay">
        /// Ngày bắt đầu lọc (theo NgayNhap). Truyền null để không lọc cận dưới.
        
        /// <param name="denNgay">
        /// Ngày kết thúc lọc (theo NgayNhap). Truyền null để không lọc cận trên.
        
        /// <param name="trangThaiLoc">
        /// Trạng thái phiếu nhập cần lọc: 'Chờ Xác Nhận' | 'Đã Nhập' | 'Huỷ'.
        /// Truyền null hoặc rỗng để lấy tất cả trạng thái.
        
        /// Trả về DataTable báo cáo nhập hàng.
        public DataTable BaoCaoNhapHang(DateTime? tuNgay = null, DateTime? denNgay = null,
                                        string? trangThaiLoc = null)
        {
            // Validate khoảng ngày
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            // Validate TrangThai nếu có truyền vào
            if (!string.IsNullOrWhiteSpace(trangThaiLoc))
            {
                string[] trangThaiHopLe = { "Chờ Xác Nhận", "Đã Nhập", "Huỷ" };
                if (!Array.Exists(trangThaiHopLe, tt =>
                        tt.Equals(trangThaiLoc, StringComparison.OrdinalIgnoreCase)))
                    throw new ArgumentException(
                        $"Trạng thái '{trangThaiLoc}' không hợp lệ. Chỉ chấp nhận: Chờ Xác Nhận | Đã Nhập | Huỷ.");
            }

            try
            {
                // Lấy toàn bộ phiếu nhập
                DataTable dtPN = _dalPN.DSTatCaPhieuNhap();

                // Xây dựng bảng kết quả
                DataTable dtKetQua = TaoCauTrucNhapHang();

                foreach (DataRow rowPN in dtPN.Rows)
                {
                    string maPhieuNhap = rowPN["MaPhieuNhap"]?.ToString()?.Trim() ?? string.Empty;
                    DateTime ngayNhap  = Convert.ToDateTime(rowPN["NgayNhap"]);
                    string trangThai   = rowPN["TrangThai"]?.ToString() ?? string.Empty;

                    // Lọc theo khoảng thời gian 
                    if (tuNgay.HasValue && ngayNhap.Date < tuNgay.Value.Date)
                        continue;
                    if (denNgay.HasValue && ngayNhap.Date > denNgay.Value.Date)
                        continue;

                    // Lọc theo trạng thái 
                    if (!string.IsNullOrWhiteSpace(trangThaiLoc)
                        && !trangThai.Equals(trangThaiLoc, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Đếm số loại SP và tổng số lượng qua bảng ChiTietPhieuNhap
                    // (không có DAL_ChiTietPhieuNhap trong BUS_BaoCao → dùng DAL_SanPham)
                    DataTable dtSerials = _dalSP.DSTheoPhieuNhap(maPhieuNhap);
                    int tongSoLuong     = dtSerials.Rows.Count;

                    // Đếm số loại SP duy nhất trong phiếu nhập
                    var maLoaiSPSet = new System.Collections.Generic.HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);
                    foreach (DataRow rowSP in dtSerials.Rows)
                    {
                        string maLoai = rowSP["MaLoaiSP"]?.ToString()?.Trim() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(maLoai))
                            maLoaiSPSet.Add(maLoai);
                    }
                    int soLoaiSP = maLoaiSPSet.Count;

                    decimal tongTien = rowPN["TongTien"] != DBNull.Value
                        ? Convert.ToDecimal(rowPN["TongTien"]) : 0m;
                    string maNCC = rowPN["MaNCC"]?.ToString()?.Trim() ?? string.Empty;
                    string maNV  = rowPN["MaNV"]?.ToString()?.Trim() ?? string.Empty;

                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["MaPhieuNhap"]  = maPhieuNhap;
                    rowKQ["NgayNhap"]     = ngayNhap;
                    rowKQ["MaNCC"]        = maNCC;
                    rowKQ["MaNV"]         = maNV;
                    rowKQ["TongTien"]     = tongTien;
                    rowKQ["TrangThai"]    = trangThai;
                    rowKQ["SoLoaiSP"]     = soLoaiSP;
                    rowKQ["TongSoLuong"]  = tongSoLuong;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo báo cáo nhập hàng: " + ex.Message, ex);
            }
        }

        
        /// Tạo cấu trúc DataTable kết quả cho báo cáo nhập hàng.
        /// Cột MaNCC / MaNV thay cho TenNCC / TenNV (GUI tra cứu thêm nếu cần).
        private DataTable TaoCauTrucNhapHang()
        {
            DataTable dt = new DataTable("BaoCaoNhapHang");
            dt.Columns.Add("MaPhieuNhap", typeof(string));
            dt.Columns.Add("NgayNhap",    typeof(DateTime));
            dt.Columns.Add("MaNCC",       typeof(string));
            dt.Columns.Add("MaNV",        typeof(string));
            dt.Columns.Add("TongTien",    typeof(decimal));
            dt.Columns.Add("TrangThai",   typeof(string));
            dt.Columns.Add("SoLoaiSP",    typeof(int));
            dt.Columns.Add("TongSoLuong", typeof(int));
            return dt;
        }

        
        /// Tóm tắt thống kê nhập hàng theo tháng/năm trong một khoảng thời gian.
        /// Trả về DataTable gồm: Nam, Thang, SoPhieuNhap, TongTienNhap, TongSoLuongNhap.
        /// Chỉ tính phiếu có TrangThai = 'Đã Nhập'.
        /// <param name="tuNgay">Ngày bắt đầu thống kê (null = không giới hạn).</param>
        /// <param name="denNgay">Ngày kết thúc thống kê (null = không giới hạn).</param>
        /// Trả về DataTable thống kê nhập hàng theo tháng.
        public DataTable ThongKeNhapHangTheoThang(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            try
            {
                DataTable dtNhapHang = BaoCaoNhapHang(tuNgay, denNgay, "Đã Nhập");

                DataTable dtKetQua = new DataTable("ThongKeNhapHangTheoThang");
                dtKetQua.Columns.Add("Nam",              typeof(int));
                dtKetQua.Columns.Add("Thang",            typeof(int));
                dtKetQua.Columns.Add("SoPhieuNhap",      typeof(int));
                dtKetQua.Columns.Add("TongTienNhap",     typeof(decimal));
                dtKetQua.Columns.Add("TongSoLuongNhap",  typeof(int));

                // Nhóm theo năm-tháng
                var nhomThang = new Dictionary<string, (int soPhieu, decimal tongTien, int tongSL)>();
                foreach (DataRow row in dtNhapHang.Rows)
                {
                    DateTime ngay     = Convert.ToDateTime(row["NgayNhap"]);
                    string   key      = $"{ngay.Year:0000}-{ngay.Month:00}";
                    decimal  tongTien = Convert.ToDecimal(row["TongTien"]);
                    int      tongSL   = Convert.ToInt32(row["TongSoLuong"]);

                    if (nhomThang.ContainsKey(key))
                    {
                        var (sp, tt, sl) = nhomThang[key];
                        nhomThang[key]   = (sp + 1, tt + tongTien, sl + tongSL);
                    }
                    else
                    {
                        nhomThang[key] = (1, tongTien, tongSL);
                    }
                }

                // Sắp xếp theo năm-tháng tăng dần rồi xuất vào DataTable
                var keys = new List<string>(nhomThang.Keys);
                keys.Sort(StringComparer.Ordinal);
                foreach (string key in keys)
                {
                    int nam   = int.Parse(key.Substring(0, 4));
                    int thang = int.Parse(key.Substring(5, 2));
                    var (soPhieu, tongTien, tongSL) = nhomThang[key];

                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["Nam"]             = nam;
                    rowKQ["Thang"]           = thang;
                    rowKQ["SoPhieuNhap"]     = soPhieu;
                    rowKQ["TongTienNhap"]    = tongTien;
                    rowKQ["TongSoLuongNhap"] = tongSL;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê nhập hàng theo tháng: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════════════════════════════════════════
        //  KHU VỰC: BÁO CÁO DOANH THU
        // ══════════════════════════════════════════════════════════════════════════════

        
        /// Tạo báo cáo doanh thu, chỉ tính đơn hàng TrangThai = 'Hoàn Thành'.
        /// Lọc tùy chọn theo khoảng thời gian (NgayDat).
        /// Cột kết quả: MaDH, NgayDat, MaKH, LoaiKH (từ DonHang.MaKH),
        ///              MaNV, TongTien, TienSauGiam, MaKM, PhuongThucThanhToan, TrangThai.
        /// Lưu ý: TenKH / TenNV / TenKhuyenMai hiển thị bằng mã (MaKH, MaNV, MaKM)
        /// vì BUS_BaoCao không inject thêm DAL. GUI tra cứu thêm nếu cần tên đầy đủ.
        
        /// <param name="tuNgay">
        /// Ngày bắt đầu lọc (theo NgayDat). Truyền null để không lọc cận dưới.
        
        /// <param name="denNgay">
        /// Ngày kết thúc lọc (theo NgayDat). Truyền null để không lọc cận trên.
        
        /// Trả về DataTable báo cáo doanh thu.
        public DataTable BaoCaoDoanhThu(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            try
            {
                // Chỉ lấy đơn hàng đã Hoàn Thành
                DataTable dtDH = _dalDH.DSTheoTrangThai("Hoàn Thành");

                DataTable dtKetQua = TaoCauTrucDoanhThu();

                foreach (DataRow rowDH in dtDH.Rows)
                {
                    DateTime ngayDat = Convert.ToDateTime(rowDH["NgayDat"]);

                    //  Lọc theo khoảng thời gian 
                    if (tuNgay.HasValue && ngayDat.Date < tuNgay.Value.Date)
                        continue;
                    if (denNgay.HasValue && ngayDat.Date > denNgay.Value.Date)
                        continue;

                    string  maDH      = rowDH["MaDH"]?.ToString()?.Trim() ?? string.Empty;
                    string  maKH      = rowDH["MaKH"]?.ToString()?.Trim() ?? string.Empty;
                    string  maNV      = rowDH["MaNV"]?.ToString()?.Trim() ?? string.Empty;
                    string  maKM      = rowDH["MaKM"] == DBNull.Value
                                        ? string.Empty
                                        : rowDH["MaKM"]?.ToString()?.Trim() ?? string.Empty;
                    decimal tongTien  = rowDH["TongTien"] != DBNull.Value
                                        ? Convert.ToDecimal(rowDH["TongTien"]) : 0m;
                    decimal? tienSauGiam = rowDH["TienSauGiam"] == DBNull.Value
                                        ? (decimal?)null
                                        : Convert.ToDecimal(rowDH["TienSauGiam"]);
                    string pttt      = rowDH["PhuongThucThanhToan"]?.ToString() ?? string.Empty;
                    string trangThai = rowDH["TrangThai"]?.ToString() ?? string.Empty;

                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["MaDH"]                  = maDH;
                    rowKQ["NgayDat"]               = ngayDat;
                    rowKQ["MaKH"]                  = maKH;
                    rowKQ["MaNV"]                  = maNV;
                    rowKQ["TongTien"]              = tongTien;
                    rowKQ["TienSauGiam"]           = tienSauGiam.HasValue
                                                    ? (object)tienSauGiam.Value : DBNull.Value;
                    rowKQ["MaKM"]                  = string.IsNullOrWhiteSpace(maKM)
                                                    ? (object)DBNull.Value : maKM;
                    rowKQ["PhuongThucThanhToan"]   = pttt;
                    rowKQ["TrangThai"]             = trangThai;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo báo cáo doanh thu: " + ex.Message, ex);
            }
        }

    
        /// Tạo cấu trúc DataTable kết quả cho báo cáo doanh thu.
        private DataTable TaoCauTrucDoanhThu()
        {
            DataTable dt = new DataTable("BaoCaoDoanhThu");
            dt.Columns.Add("MaDH",                  typeof(string));
            dt.Columns.Add("NgayDat",               typeof(DateTime));
            dt.Columns.Add("MaKH",                  typeof(string));
            dt.Columns.Add("MaNV",                  typeof(string));
            dt.Columns.Add("TongTien",              typeof(decimal));
            dt.Columns.Add("TienSauGiam",           typeof(decimal));
            dt.Columns.Add("MaKM",                  typeof(string));
            dt.Columns.Add("PhuongThucThanhToan",   typeof(string));
            dt.Columns.Add("TrangThai",             typeof(string));
            return dt;
        }

        
        /// Tổng hợp thống kê doanh thu theo tháng/năm.
        /// Trả về DataTable gồm: Nam, Thang, SoDonHang, TongDoanhThu, TongDoanhThuSauGiam.
        /// Chỉ tính đơn hàng TrangThai = 'Hoàn Thành'.
        /// <param name="tuNgay">Ngày bắt đầu thống kê (null = không giới hạn).</param>
        /// <param name="denNgay">Ngày kết thúc thống kê (null = không giới hạn).</param>
        /// Trả về DataTable thống kê doanh thu theo tháng.
        public DataTable ThongKeDoanhThuTheoThang(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            try
            {
                DataTable dtDT = BaoCaoDoanhThu(tuNgay, denNgay);

                DataTable dtKetQua = new DataTable("ThongKeDoanhThuTheoThang");
                dtKetQua.Columns.Add("Nam",                  typeof(int));
                dtKetQua.Columns.Add("Thang",               typeof(int));
                dtKetQua.Columns.Add("SoDonHang",           typeof(int));
                dtKetQua.Columns.Add("TongDoanhThu",        typeof(decimal));
                dtKetQua.Columns.Add("TongDoanhThuSauGiam", typeof(decimal));

                // Nhóm theo năm-tháng
                var nhom = new Dictionary<string, (int soDon, decimal tongDT, decimal tongDTSG)>();
                foreach (DataRow row in dtDT.Rows)
                {
                    DateTime ngay     = Convert.ToDateTime(row["NgayDat"]);
                    string   key      = $"{ngay.Year:0000}-{ngay.Month:00}";
                    decimal  tongTien = Convert.ToDecimal(row["TongTien"]);
                    decimal  sauGiam  = row["TienSauGiam"] == DBNull.Value
                                        ? tongTien
                                        : Convert.ToDecimal(row["TienSauGiam"]);

                    if (nhom.ContainsKey(key))
                    {
                        var (sd, dt, dtsg) = nhom[key];
                        nhom[key] = (sd + 1, dt + tongTien, dtsg + sauGiam);
                    }
                    else
                    {
                        nhom[key] = (1, tongTien, sauGiam);
                    }
                }

                // Sắp xếp tăng dần rồi xuất
                var keys = new List<string>(nhom.Keys);
                keys.Sort(StringComparer.Ordinal);
                foreach (string key in keys)
                {
                    int nam   = int.Parse(key.Substring(0, 4));
                    int thang = int.Parse(key.Substring(5, 2));
                    var (soDon, tongDT, tongDTSG) = nhom[key];

                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["Nam"]                  = nam;
                    rowKQ["Thang"]               = thang;
                    rowKQ["SoDonHang"]           = soDon;
                    rowKQ["TongDoanhThu"]        = tongDT;
                    rowKQ["TongDoanhThuSauGiam"] = tongDTSG;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê doanh thu theo tháng: " + ex.Message, ex);
            }
        }

        
        /// Thống kê doanh thu theo từng năm (tổng hợp các tháng trong năm).
        /// Trả về DataTable gồm: Nam, SoDonHang, TongDoanhThu, TongDoanhThuSauGiam.
        /// Chỉ tính đơn hàng TrangThai = 'Hoàn Thành'.
        /// <param name="tuNgay">Ngày bắt đầu thống kê (null = không giới hạn).</param>
        /// <param name="denNgay">Ngày kết thúc thống kê (null = không giới hạn).</param>
        /// Trả về DataTable thống kê doanh thu theo năm.
        public DataTable ThongKeDoanhThuTheoNam(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            try
            {
                DataTable dtDT = BaoCaoDoanhThu(tuNgay, denNgay);

                DataTable dtKetQua = new DataTable("ThongKeDoanhThuTheoNam");
                dtKetQua.Columns.Add("Nam",                  typeof(int));
                dtKetQua.Columns.Add("SoDonHang",           typeof(int));
                dtKetQua.Columns.Add("TongDoanhThu",        typeof(decimal));
                dtKetQua.Columns.Add("TongDoanhThuSauGiam", typeof(decimal));

                var nhom = new Dictionary<int, (int soDon, decimal tongDT, decimal tongDTSG)>();
                foreach (DataRow row in dtDT.Rows)
                {
                    int     nam       = Convert.ToDateTime(row["NgayDat"]).Year;
                    decimal tongTien  = Convert.ToDecimal(row["TongTien"]);
                    decimal sauGiam   = row["TienSauGiam"] == DBNull.Value
                                        ? tongTien
                                        : Convert.ToDecimal(row["TienSauGiam"]);

                    if (nhom.ContainsKey(nam))
                    {
                        var (sd, dt, dtsg) = nhom[nam];
                        nhom[nam] = (sd + 1, dt + tongTien, dtsg + sauGiam);
                    }
                    else
                    {
                        nhom[nam] = (1, tongTien, sauGiam);
                    }
                }

                var namList = new List<int>(nhom.Keys);
                namList.Sort();
                foreach (int nam in namList)
                {
                    var (soDon, tongDT, tongDTSG) = nhom[nam];
                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["Nam"]                  = nam;
                    rowKQ["SoDonHang"]           = soDon;
                    rowKQ["TongDoanhThu"]        = tongDT;
                    rowKQ["TongDoanhThuSauGiam"] = tongDTSG;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê doanh thu theo năm: " + ex.Message, ex);
            }
        }

        
        /// Tính tổng doanh thu và tổng doanh thu sau giảm trong một khoảng thời gian.
        /// Chỉ tính đơn hàng TrangThai = 'Hoàn Thành'.
        /// <param name="tuNgay">Ngày bắt đầu (null = không giới hạn).</param>
        /// <param name="denNgay">Ngày kết thúc (null = không giới hạn).</param>
        /// Trả về 
        /// Tuple (tongDoanhThu, tongDoanhThuSauGiam, soDonHang):
        ///   tongDoanhThu        = tổng TongTien của các đơn hoàn thành,
        ///   tongDoanhThuSauGiam = tổng TienSauGiam (dùng TongTien nếu không có giảm giá),
        ///   soDonHang           = số đơn hàng hoàn thành trong kỳ.
        /// 
        public (decimal tongDoanhThu, decimal tongDoanhThuSauGiam, int soDonHang) TinhTongDoanhThu(
            DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            try
            {
                DataTable dtDT = BaoCaoDoanhThu(tuNgay, denNgay);

                decimal tongDT   = 0m;
                decimal tongDTSG = 0m;
                int     soDon    = dtDT.Rows.Count;

                foreach (DataRow row in dtDT.Rows)
                {
                    decimal tt  = Convert.ToDecimal(row["TongTien"]);
                    decimal tsg = row["TienSauGiam"] == DBNull.Value ? tt
                                : Convert.ToDecimal(row["TienSauGiam"]);
                    tongDT   += tt;
                    tongDTSG += tsg;
                }

                return (tongDT, tongDTSG, soDon);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng doanh thu: " + ex.Message, ex);
            }
        }

        
        /// Lấy danh sách chi tiết đơn hàng của một đơn hàng cụ thể kèm thông tin sản phẩm.
        /// Wrapper trực tiếp sang DAL_ChiTietDonHang.DSChiTietCoThongTinSanPham().
        /// Trả về DataTable gồm: MaDH, MaSerialSP, TenLoai, TenHang, DanhMuc,
        ///                        GiaBan, PhanTramGiam, ThanhTien.
        /// <param name="maDH">Mã đơn hàng cần lấy chi tiết.</param>
        /// Trả về DataTable chi tiết đơn hàng kèm thông tin sản phẩm.
        public DataTable LayChiTietDonHangCoThongTinSP(string maDH)
        {
            if (string.IsNullOrWhiteSpace(maDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.", nameof(maDH));

            try
            {
                return _dalCTDH.DSChiTietCoThongTinSanPham(maDH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết đơn hàng {maDH}: " + ex.Message, ex);
            }
        }

        
        /// Thống kê doanh thu theo phương thức thanh toán trong khoảng thời gian.
        /// Trả về DataTable gồm: PhuongThucThanhToan, SoDonHang, TongDoanhThu, TongDoanhThuSauGiam.
        /// Chỉ tính đơn hàng TrangThai = 'Hoàn Thành'.
        /// <param name="tuNgay">Ngày bắt đầu (null = không giới hạn).</param>
        /// <param name="denNgay">Ngày kết thúc (null = không giới hạn).</param>
        /// Trả về DataTable thống kê theo phương thức thanh toán.
        public DataTable ThongKeTheoHinhThucThanhToan(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            try
            {
                DataTable dtDT = BaoCaoDoanhThu(tuNgay, denNgay);

                DataTable dtKetQua = new DataTable("ThongKeTheoHTTT");
                dtKetQua.Columns.Add("PhuongThucThanhToan",   typeof(string));
                dtKetQua.Columns.Add("SoDonHang",             typeof(int));
                dtKetQua.Columns.Add("TongDoanhThu",          typeof(decimal));
                dtKetQua.Columns.Add("TongDoanhThuSauGiam",   typeof(decimal));

                var nhom = new Dictionary<string, (int soDon, decimal tongDT, decimal tongDTSG)>(
                    StringComparer.OrdinalIgnoreCase);

                foreach (DataRow row in dtDT.Rows)
                {
                    string  pttt      = row["PhuongThucThanhToan"]?.ToString() ?? string.Empty;
                    decimal tongTien  = Convert.ToDecimal(row["TongTien"]);
                    decimal sauGiam   = row["TienSauGiam"] == DBNull.Value
                                        ? tongTien
                                        : Convert.ToDecimal(row["TienSauGiam"]);

                    if (nhom.ContainsKey(pttt))
                    {
                        var (sd, dt, dtsg) = nhom[pttt];
                        nhom[pttt] = (sd + 1, dt + tongTien, dtsg + sauGiam);
                    }
                    else
                    {
                        nhom[pttt] = (1, tongTien, sauGiam);
                    }
                }

                foreach (var kv in nhom)
                {
                    var (soDon, tongDT, tongDTSG) = kv.Value;
                    DataRow rowKQ = dtKetQua.NewRow();
                    rowKQ["PhuongThucThanhToan"]   = kv.Key;
                    rowKQ["SoDonHang"]             = soDon;
                    rowKQ["TongDoanhThu"]          = tongDT;
                    rowKQ["TongDoanhThuSauGiam"]   = tongDTSG;
                    dtKetQua.Rows.Add(rowKQ);
                }

                return dtKetQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê theo hình thức thanh toán: " + ex.Message, ex);
            }
        }
    }
}

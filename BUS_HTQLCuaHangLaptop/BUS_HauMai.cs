using System;
using System.Collections.Generic;
using System.Data;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ hậu mãi, tổng hợp các chức năng:
    /// - Phiếu Bảo Hành (bảo hành tại cửa hàng cho KH lẻ / bảo hành tại hãng cho KH sỉ, 
    ///   trường hợp đặc biệt ≥10 serial lỗi NSX trong 30 ngày cùng LoaiSanPham / DonHang).
    /// - Phiếu Đổi Trả  (trong 30 ngày, lỗi do NSX, mỗi serial chỉ đổi trả 1 lần).
    /// - Đơn Khiếu Nại  (liên kết với DonHang đã Hoàn Thành, nội dung không rỗng).
    
    public class BUS_HauMai
    {

        private readonly DAL_PhieuBaoHanh _dalPBH = new DAL_PhieuBaoHanh();
        private readonly DAL_PhieuDoiTra  _dalPDT = new DAL_PhieuDoiTra();
        private readonly DAL_DonKhieuNai  _dalDKN = new DAL_DonKhieuNai();
        private readonly DAL_SanPham      _dalSP  = new DAL_SanPham();
        private readonly DAL_DonHang      _dalDH  = new DAL_DonHang();
        private readonly DAL_ChiTietDonHang _dalCTDH = new DAL_ChiTietDonHang();
        private readonly DAL_LoaiSanPham    _dalLSP  = new DAL_LoaiSanPham();

        // ══════════════════════════════════════════════════════════════════════════════
        //  HẰNG SỐ NGHIỆP VỤ
        // ══════════════════════════════════════════════════════════════════════════════

        /// Số ngày tối đa được phép đổi trả kể từ ngày đặt hàng.
        private const int THOI_HAN_DOI_TRA_NGAY = 30;

        /// Số ngày quan sát khi kiểm tra điều kiện đặc biệt KH sỉ (≥10 lỗi NSX).
        private const int THOI_HAN_QUAN_SAT_KH_SI_NGAY = 30;

        /// Ngưỡng số serial lỗi NSX để kích hoạt điều kiện đặc biệt KH sỉ.
        private const int NGUONG_SERIAL_LOI_KH_SI = 10;

        // ══════════════════════════════════════════════════════════════════════════════
        //  KHU VỰC: PHIẾU ĐỔI TRẢ
        // ══════════════════════════════════════════════════════════════════════════════

        /// Lấy toàn bộ danh sách phiếu đổi trả trong hệ thống.
        /// DataTable chứa tất cả phiếu đổi trả.
        public DataTable LayDanhSachDoiTra()
        {
            try
            {
                return _dalPDT.DSTatCaPhieuDoiTra();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phiếu đổi trả: " + ex.Message, ex);
            }
        }

        /// Lấy danh sách phiếu đổi trả của một khách hàng cụ thể.
        /// <param name="maKH">Mã khách hàng cần lọc.
        /// DataTable chứa các phiếu đổi trả của khách hàng đó.
        public DataTable LayDanhSachDoiTraTheoKH(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(maKH));

            try
            {
                return _dalPDT.DSTheoKhachHang(maKH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu đổi trả của khách hàng {maKH}: " + ex.Message, ex);
            }
        }

        /// Lấy danh sách phiếu đổi trả của một serial sản phẩm cụ thể.
        /// <param name="maSerial">Số serial sản phẩm.
        /// DataTable chứa các phiếu đổi trả liên quan đến serial đó.
        public DataTable LayDanhSachDoiTraTheoSerial(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Mã serial không được để trống.", nameof(maSerial));

            try
            {
                return _dalPDT.DSTheoMaSerial(maSerial);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu đổi trả của serial {maSerial}: " + ex.Message, ex);
            }
        }

        /// Lấy thông tin chi tiết một phiếu đổi trả theo mã phiếu.
        /// <param name="maPhieuDT">Mã phiếu đổi trả cần tìm.
        /// DTO_PhieuDoiTra nếu tìm thấy, null nếu không có.
        public DTO_PhieuDoiTra? LayTheoMaPhieuDoiTra(string maPhieuDT)
        {
            if (string.IsNullOrWhiteSpace(maPhieuDT))
                throw new ArgumentException("Mã phiếu đổi trả không được để trống.", nameof(maPhieuDT));

            try
            {
                return _dalPDT.DSTheoMaPhieuDoiTra(maPhieuDT);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu đổi trả {maPhieuDT}: " + ex.Message, ex);
            }
        }

        /// Kiểm tra điều kiện hợp lệ để thực hiện đổi trả cho một serial sản phẩm.
        /// Điều kiện:
        ///   1. Serial tồn tại và chưa bị xóa mềm.
        ///   2. Đơn hàng tương ứng tồn tại.
        ///   3. Ngày yêu cầu đổi trả trong vòng 30 ngày kể từ NgayDat của DonHang.
        ///   4. Serial chưa có phiếu đổi trả nào (UNIQUE constraint).
        /// Tuple (hopLe, lyDoTuChoi): hopLe = true nếu đủ điều kiện đổi trả,
        /// lyDoTuChoi chứa lý do nếu không đủ điều kiện (chuỗi rỗng khi hopLe = true).
        public (bool hopLe, string lyDoTuChoi) KiemTraDieuKienDoiTra(string maSerial, DateTime ngayMua)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                return (false, "Mã serial không được để trống.");

            // Kiểm tra serial tồn tại
            DTO_SanPham? sp = _dalSP.DSTheoMaSerialSP(maSerial);
            if (sp == null)
                return (false, $"Không tìm thấy sản phẩm với serial '{maSerial}'.");

            // Kiểm tra thời hạn 30 ngày kể từ ngày đặt hàng
            DateTime ngayHetHanDoiTra = ngayMua.Date.AddDays(THOI_HAN_DOI_TRA_NGAY);
            if (DateTime.Today > ngayHetHanDoiTra)
                return (false, $"Đã quá thời hạn đổi trả 30 ngày (hết hạn ngày {ngayHetHanDoiTra:dd/MM/yyyy}).");

            // Kiểm tra serial chưa có phiếu đổi trả (UNIQUE — mỗi serial chỉ đổi 1 lần)
            DataTable dtExisting = _dalPDT.DSTheoMaSerial(maSerial);
            if (dtExisting.Rows.Count > 0)
                return (false, $"Serial '{maSerial}' đã có phiếu đổi trả trong hệ thống. Mỗi serial chỉ được đổi trả một lần.");

            return (true, string.Empty);
        }

        /// Tạo phiếu đổi trả mới sau khi đã kiểm tra đầy đủ điều kiện nghiệp vụ:
        /// - Kiểm tra tất cả trường bắt buộc.
        /// - Kiểm tra DonHang tồn tại.
        /// - Gọi KiemTraDieuKienDoiTra để xác nhận tính hợp lệ.
        /// - LoaiXuLy phải là 'Đổi Máy', 'Hoàn Tiền' hoặc 'Từ Chối'.
        /// - Khi tạo thành công: cập nhật TrangThai SanPham → 'Đổi Trả'.
        
        /// <param name="pdt">Đối tượng DTO_PhieuDoiTra cần tạo.
        /// true nếu tạo phiếu thành công, false nếu thất bại.
        /// <exception cref="ArgumentException">Khi dữ liệu đầu vào không hợp lệ.
        /// <exception cref="InvalidOperationException">Khi vi phạm điều kiện nghiệp vụ đổi trả.
        public bool TaoPhieuDoiTra(DTO_PhieuDoiTra pdt)
        {
            // Kiểm tra trường bắt buộc 
            if (pdt == null)
                throw new ArgumentException("Thông tin phiếu đổi trả không được null.");
            if (string.IsNullOrWhiteSpace(pdt.MaPhieuDT))
                throw new ArgumentException("Mã phiếu đổi trả không được để trống.");
            if (string.IsNullOrWhiteSpace(pdt.MaDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(pdt.MaSerialSP))
                throw new ArgumentException("Mã serial sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(pdt.MaKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(pdt.LyDo))
                throw new ArgumentException("Lý do đổi trả không được để trống. Chỉ chấp nhận lỗi do nhà sản xuất.");

            // Kiểm tra LoaiXuLy 
            string[] loaiXuLyHopLe = { "Đổi Máy", "Hoàn Tiền", "Từ Chối" };
            if (!Array.Exists(loaiXuLyHopLe, lxl => lxl.Equals(pdt.LoaiXuLy, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Loại xử lý '{pdt.LoaiXuLy}' không hợp lệ. Chỉ chấp nhận: Đổi Máy | Hoàn Tiền | Từ Chối.");

            // Kiểm tra DonHang tồn tại 
            DTO_DonHang? donHang = _dalDH.DSTheoMaDH(pdt.MaDH);
            if (donHang == null)
                throw new InvalidOperationException($"Đơn hàng '{pdt.MaDH}' không tồn tại trong hệ thống.");

            // Kiểm tra điều kiện đổi trả (30 ngày + chưa có phiếu) 
            var (hopLe, lyDoTuChoi) = KiemTraDieuKienDoiTra(pdt.MaSerialSP, donHang.NgayDat);
            if (!hopLe)
                throw new InvalidOperationException($"Không đủ điều kiện đổi trả: {lyDoTuChoi}");

            // Thiết lập trạng thái mặc định khi tạo mới
            if (string.IsNullOrWhiteSpace(pdt.TrangThai))
                pdt.TrangThai = "Đang Xử Lý";

            // Gọi DAL thêm phiếu đổi trả 
            try
            {
                bool ketQua = _dalPDT.ThemPhieuDoiTra(pdt);
                if (ketQua)
                {
                    // Sau khi tạo phiếu thành công → cập nhật TrangThai SanPham → 'Đổi Trả'
                    bool capNhatSP = _dalSP.CapNhatTrangThai(pdt.MaSerialSP, "Đổi Trả");
                    if (!capNhatSP)
                        throw new Exception($"Tạo phiếu đổi trả thành công nhưng không cập nhật được trạng thái sản phẩm serial '{pdt.MaSerialSP}'.");
                }
                return ketQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo phiếu đổi trả: " + ex.Message, ex);
            }
        }

        /// 
        /// Cập nhật trạng thái phiếu đổi trả.
        /// TrangThai hợp lệ: 'Đang Xử Lý' | 'Hoàn Thành' | 'Từ Chối'.
        
        /// <param name="maPhieuDT">Mã phiếu đổi trả cần cập nhật.
        /// <param name="trangThai">Trạng thái mới.
        /// true nếu cập nhật thành công.
        public bool CapNhatTrangThaiDoiTra(string maPhieuDT, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maPhieuDT))
                throw new ArgumentException("Mã phiếu đổi trả không được để trống.", nameof(maPhieuDT));

            string[] trangThaiHopLe = { "Đang Xử Lý", "Hoàn Thành", "Từ Chối" };
            if (!Array.Exists(trangThaiHopLe, tt => tt.Equals(trangThai, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Trạng thái '{trangThai}' không hợp lệ. Chỉ chấp nhận: Đang Xử Lý | Hoàn Thành | Từ Chối.");

            DTO_PhieuDoiTra? phieu = _dalPDT.DSTheoMaPhieuDoiTra(maPhieuDT);
            if (phieu == null)
                throw new InvalidOperationException($"Không tìm thấy phiếu đổi trả '{maPhieuDT}'.");

            try
            {
                return _dalPDT.CapNhatTrangThai(maPhieuDT, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái phiếu đổi trả {maPhieuDT}: " + ex.Message, ex);
            }
        }

        /// 
        /// Cập nhật loại xử lý của phiếu đổi trả.
        /// LoaiXuLy hợp lệ: 'Đổi Máy' | 'Hoàn Tiền' | 'Từ Chối'.
        
        /// <param name="maPhieuDT">Mã phiếu đổi trả.
        /// <param name="loaiXuLy">Loại xử lý mới.
        /// true nếu cập nhật thành công.
        public bool CapNhatLoaiXuLyDoiTra(string maPhieuDT, string loaiXuLy)
        {
            if (string.IsNullOrWhiteSpace(maPhieuDT))
                throw new ArgumentException("Mã phiếu đổi trả không được để trống.", nameof(maPhieuDT));

            string[] loaiXuLyHopLe = { "Đổi Máy", "Hoàn Tiền", "Từ Chối" };
            if (!Array.Exists(loaiXuLyHopLe, lxl => lxl.Equals(loaiXuLy, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Loại xử lý '{loaiXuLy}' không hợp lệ. Chỉ chấp nhận: Đổi Máy | Hoàn Tiền | Từ Chối.");

            DTO_PhieuDoiTra? phieu = _dalPDT.DSTheoMaPhieuDoiTra(maPhieuDT);
            if (phieu == null)
                throw new InvalidOperationException($"Không tìm thấy phiếu đổi trả '{maPhieuDT}'.");

            try
            {
                return _dalPDT.CapNhatLoaiXuLy(maPhieuDT, loaiXuLy);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật loại xử lý phiếu đổi trả {maPhieuDT}: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════════════════════════════════════════
        //  KHU VỰC: PHIẾU BẢO HÀNH
        // ══════════════════════════════════════════════════════════════════════════════

        /// 
        /// Lấy toàn bộ danh sách phiếu bảo hành trong hệ thống.
        
        /// DataTable chứa tất cả phiếu bảo hành.
        public DataTable LayDanhSachBaoHanh()
        {
            try
            {
                return _dalPBH.DSTatCaPhieuBaoHanh();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phiếu bảo hành: " + ex.Message, ex);
            }
        }

        /// 
        /// Lấy danh sách phiếu bảo hành của một khách hàng cụ thể.
        
        /// <param name="maKH">Mã khách hàng.
        /// DataTable chứa các phiếu bảo hành của khách hàng đó.
        public DataTable LayDanhSachBaoHanhTheoKH(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(maKH));

            try
            {
                return _dalPBH.DSTheoKhachHang(maKH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu bảo hành của khách hàng {maKH}: " + ex.Message, ex);
            }
        }

        /// 
        /// Lấy danh sách phiếu bảo hành của một serial sản phẩm.
        
        /// <param name="maSerial">Số serial sản phẩm.
        /// DataTable chứa các phiếu bảo hành liên quan đến serial.
        public DataTable LayDanhSachBaoHanhTheoSerial(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Mã serial không được để trống.", nameof(maSerial));

            try
            {
                return _dalPBH.DSTheoMaSerial(maSerial);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu bảo hành của serial {maSerial}: " + ex.Message, ex);
            }
        }

        /// 
        /// Lấy thông tin chi tiết một phiếu bảo hành theo mã phiếu.
        
        /// <param name="maPBH">Mã phiếu bảo hành cần tìm.
        /// DTO_PhieuBaoHanh nếu tìm thấy, null nếu không có.
        public DTO_PhieuBaoHanh? LayTheoMaPhieuBaoHanh(string maPBH)
        {
            if (string.IsNullOrWhiteSpace(maPBH))
                throw new ArgumentException("Mã phiếu bảo hành không được để trống.", nameof(maPBH));

            try
            {
                return _dalPBH.DSTheoMaPhieuBaoHanh(maPBH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu bảo hành {maPBH}: " + ex.Message, ex);
            }
        }

        /// 
        /// Tính ngày kết thúc bảo hành cho khách hàng lẻ:
        /// NgayKetThuc = NgayDat (trên hóa đơn) + ThoiGianBaoHanh (tháng) của LoaiSanPham.
        
        /// <param name="ngayDatHoaDon">Ngày đặt hàng (NgayDat) từ DonHang.
        /// <param name="thoiGianBaoHanhThang">Số tháng bảo hành theo LoaiSanPham.
        /// DateTime ngày kết thúc bảo hành.
        public DateTime TinhNgayKetThucBaoHanh(DateTime ngayDatHoaDon, int thoiGianBaoHanhThang)
        {
            if (thoiGianBaoHanhThang <= 0)
                throw new ArgumentException("Thời gian bảo hành phải lớn hơn 0 tháng.", nameof(thoiGianBaoHanhThang));

            return ngayDatHoaDon.AddMonths(thoiGianBaoHanhThang);
        }

        /// 
        /// Tạo phiếu bảo hành mới với các quy tắc:
        /// - KH lẻ  → LoaiBH = 'Cửa Hàng', NgayKetThuc tính từ NgayDat + ThoiGianBaoHanh.
        /// - KH sỉ  → LoaiBH = 'Hãng', NgayKetThuc do caller cung cấp (theo hãng quy định).
        /// - NgayKetThuc phải > NgayBatDau.
        /// - Khi tạo thành công: cập nhật TrangThai SanPham → 'Bảo Hành'.
        
        /// <param name="pbh">Đối tượng DTO_PhieuBaoHanh cần tạo.
        /// <param name="loaiKhachHang">Loại khách hàng ('Lẻ' hoặc 'Sỉ') để xác định LoaiBH.
        /// <param name="thoiGianBaoHanhThang">
        /// Số tháng bảo hành theo LoaiSanPham. Chỉ dùng khi loaiKhachHang = 'Lẻ'.
        /// 
        /// true nếu tạo phiếu thành công.
        /// <exception cref="ArgumentException">Khi dữ liệu đầu vào không hợp lệ.
        /// <exception cref="InvalidOperationException">Khi vi phạm điều kiện nghiệp vụ bảo hành.
        public bool TaoPhieuBaoHanh(DTO_PhieuBaoHanh pbh, string loaiKhachHang, int thoiGianBaoHanhThang = 0)
        {
            // Kiểm tra trường bắt buộc 
            if (pbh == null)
                throw new ArgumentException("Thông tin phiếu bảo hành không được null.");
            if (string.IsNullOrWhiteSpace(pbh.MaPBH))
                throw new ArgumentException("Mã phiếu bảo hành không được để trống.");
            if (string.IsNullOrWhiteSpace(pbh.MaDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(pbh.MaKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(pbh.MaSerialSP))
                throw new ArgumentException("Mã serial sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(loaiKhachHang))
                throw new ArgumentException("Loại khách hàng không được để trống.");

            // Kiểm tra DonHang tồn tại 
            DTO_DonHang? donHang = _dalDH.DSTheoMaDH(pbh.MaDH);
            if (donHang == null)
                throw new InvalidOperationException($"Đơn hàng '{pbh.MaDH}' không tồn tại trong hệ thống.");

            // Kiểm tra serial tồn tại 
            DTO_SanPham? sanPham = _dalSP.DSTheoMaSerialSP(pbh.MaSerialSP);
            if (sanPham == null)
                throw new InvalidOperationException($"Sản phẩm với serial '{pbh.MaSerialSP}' không tồn tại hoặc đã bị xóa.");

            // Kiểm tra xem sản phẩm có phiếu bảo hành nào đang xử lý hay không
            DataTable dtExisting = _dalPBH.DSTheoMaSerial(pbh.MaSerialSP);
            foreach (DataRow row in dtExisting.Rows)
            {
                string tt = row["TrangThai"]?.ToString()?.Trim() ?? string.Empty;
                if (tt.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Sản phẩm với số Serial '{pbh.MaSerialSP}' đang có phiếu bảo hành chưa xử lý (Đang Xử Lý) trong hệ thống.");
                }
            }

            // Xác định LoaiBH và NgayKetThuc theo loại khách hàng 
            string loaiKH = loaiKhachHang.Trim();
            if (loaiKH.Equals("Lẻ", StringComparison.OrdinalIgnoreCase))
            {
                // KH lẻ: bảo hành tại cửa hàng, thời hạn từ NgayDat + ThoiGianBaoHanh
                pbh.LoaiBH = "Cửa Hàng";
                if (thoiGianBaoHanhThang <= 0)
                    throw new ArgumentException("Thời gian bảo hành (tháng) phải lớn hơn 0 với khách hàng lẻ.");
                pbh.NgayBatDau = pbh.NgayBatDau == default(DateTime) ? DateTime.Today : pbh.NgayBatDau;
                pbh.NgayKetThuc = TinhNgayKetThucBaoHanh(donHang.NgayDat, thoiGianBaoHanhThang);
            }
            else if (loaiKH.Equals("Sỉ", StringComparison.OrdinalIgnoreCase))
            {
                // KH sỉ: bảo hành tại hãng, NgayKetThuc do caller cung cấp
                pbh.LoaiBH = "Hãng";
                pbh.NgayBatDau = pbh.NgayBatDau == default(DateTime) ? DateTime.Today : pbh.NgayBatDau;
                // NgayKetThuc phải được caller cung cấp (kiểm tra bên dưới)
            }
            else
            {
                throw new ArgumentException($"Loại khách hàng '{loaiKhachHang}' không hợp lệ. Chỉ chấp nhận 'Lẻ' hoặc 'Sỉ'.");
            }

            // Kiểm tra NgayKetThuc > NgayBatDau 
            if (pbh.NgayKetThuc <= pbh.NgayBatDau)
                throw new InvalidOperationException(
                    $"Ngày kết thúc bảo hành ({pbh.NgayKetThuc:dd/MM/yyyy}) phải lớn hơn ngày bắt đầu ({pbh.NgayBatDau:dd/MM/yyyy}).");

            // Kiểm tra LoaiBH 
            string[] loaiBHHopLe = { "Cửa Hàng", "Hãng" };
            if (!Array.Exists(loaiBHHopLe, lbh => lbh.Equals(pbh.LoaiBH, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Loại bảo hành '{pbh.LoaiBH}' không hợp lệ. Chỉ chấp nhận: Cửa Hàng | Hãng.");

            // Thiết lập TrangThai mặc định 
            if (string.IsNullOrWhiteSpace(pbh.TrangThai))
                pbh.TrangThai = "Đang Xử Lý";

            // Gọi DAL thêm phiếu bảo hành 
            try
            {
                bool ketQua = _dalPBH.ThemPhieuBaoHanh(pbh);
                if (ketQua)
                {
                    // Sau khi tạo phiếu thành công → cập nhật TrangThai SanPham → 'Bảo Hành'
                    bool capNhatSP = _dalSP.CapNhatTrangThai(pbh.MaSerialSP, "Bảo Hành");
                    if (!capNhatSP)
                        throw new Exception($"Tạo phiếu bảo hành thành công nhưng không cập nhật được trạng thái sản phẩm serial '{pbh.MaSerialSP}'.");
                }
                return ketQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo phiếu bảo hành: " + ex.Message, ex);
            }
        }

        /// 
        /// Xử lý trường hợp đặc biệt khách hàng sỉ: trong vòng 30 ngày kể từ NgayDat
        /// của một DonHang, nếu có ≥10 serial của cùng một LoaiSanPham bị lỗi do NSX
        /// → cửa hàng đổi 1:1, cập nhật TrangThai các serial lỗi đó → 'Lỗi'.
        /// Caller cần cung cấp danh sách serial được xác nhận là lỗi do NSX.
        
        /// <param name="maDH">Mã đơn hàng gốc của khách sỉ.
        /// <param name="maLoaiSP">Mã loại sản phẩm bị lỗi.
        /// <param name="danhSachSerialLoi">Danh sách serial xác nhận lỗi do NSX.
        /// 
        /// Tuple (duKieuKien, soSerialLoi):
        ///   duKieuKien = true nếu đủ điều kiện ≥10 serial lỗi trong 30 ngày từ DonHang,
        ///   soSerialLoi = số serial thực sự được đánh dấu lỗi.
        /// 
        public (bool duDieuKien, int soSerialDaCapNhat) XuLyTruongHopDacBietKHSi(
            string maDH, string maLoaiSP, List<string> danhSachSerialLoi)
        {
            // Kiểm tra đầu vào 
            if (string.IsNullOrWhiteSpace(maDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.", nameof(maDH));
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.", nameof(maLoaiSP));
            if (danhSachSerialLoi == null || danhSachSerialLoi.Count == 0)
                return (false, 0);

            // Kiểm tra DonHang tồn tại 
            DTO_DonHang? donHang = _dalDH.DSTheoMaDH(maDH);
            if (donHang == null)
                throw new InvalidOperationException($"Đơn hàng '{maDH}' không tồn tại.");

            // Kiểm tra trong vòng 30 ngày kể từ NgayDat 
            DateTime ngayHetHanQuanSat = donHang.NgayDat.Date.AddDays(THOI_HAN_QUAN_SAT_KH_SI_NGAY);
            if (DateTime.Today > ngayHetHanQuanSat)
                return (false, 0);  // Đã quá 30 ngày — không áp dụng điều kiện đặc biệt

            // Lọc chỉ những serial thuộc đúng LoaiSanPham được chỉ định 
            var serialHopLe = new List<string>();
            foreach (string serial in danhSachSerialLoi)
            {
                if (string.IsNullOrWhiteSpace(serial))
                    continue;
                DTO_SanPham? sp = _dalSP.DSTheoMaSerialSP(serial);
                if (sp != null && sp.MaLoaiSP.Trim().Equals(maLoaiSP.Trim(), StringComparison.OrdinalIgnoreCase))
                    serialHopLe.Add(serial);
            }

            // Kiểm tra ngưỡng ≥10 serial lỗi 
            if (serialHopLe.Count < NGUONG_SERIAL_LOI_KH_SI)
                return (false, 0);  // Chưa đủ điều kiện ≥10 serial lỗi

            // Cập nhật TrangThai các serial lỗi → 'Lỗi' (điều kiện đặc biệt đã thỏa) 
            int soCapNhat = 0;
            foreach (string serial in serialHopLe)
            {
                bool capNhat = _dalSP.CapNhatTrangThai(serial, "Lỗi");
                if (capNhat)
                    soCapNhat++;
            }

            return (true, soCapNhat);
        }

        /// 
        /// Cập nhật trạng thái phiếu bảo hành.
        /// TrangThai hợp lệ: 'Đang Xử Lý' | 'Hoàn Thành' | 'Từ Chối'.
        
        /// <param name="maPBH">Mã phiếu bảo hành cần cập nhật.
        /// <param name="trangThai">Trạng thái mới.
        /// true nếu cập nhật thành công.
        public bool CapNhatTrangThaiBaoHanh(string maPBH, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maPBH))
                throw new ArgumentException("Mã phiếu bảo hành không được để trống.", nameof(maPBH));

            string[] trangThaiHopLe = { "Đang Xử Lý", "Hoàn Thành", "Từ Chối" };
            if (!Array.Exists(trangThaiHopLe, tt => tt.Equals(trangThai, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Trạng thái '{trangThai}' không hợp lệ. Chỉ chấp nhận: Đang Xử Lý | Hoàn Thành | Từ Chối.");

            DTO_PhieuBaoHanh? phieu = _dalPBH.DSTheoMaPhieuBaoHanh(maPBH);
            if (phieu == null)
                throw new InvalidOperationException($"Không tìm thấy phiếu bảo hành '{maPBH}'.");

            try
            {
                return _dalPBH.CapNhatTrangThai(maPBH, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái phiếu bảo hành {maPBH}: " + ex.Message, ex);
            }
        }

        /// 
        /// Cập nhật kết quả phiếu bảo hành sau khi xử lý xong.
        
        /// <param name="maPBH">Mã phiếu bảo hành cần cập nhật.
        /// <param name="ketQua">Nội dung kết quả xử lý bảo hành.
        /// true nếu cập nhật thành công.
        public bool CapNhatKetQuaBaoHanh(string maPBH, string ketQua)
        {
            if (string.IsNullOrWhiteSpace(maPBH))
                throw new ArgumentException("Mã phiếu bảo hành không được để trống.", nameof(maPBH));

            DTO_PhieuBaoHanh? phieu = _dalPBH.DSTheoMaPhieuBaoHanh(maPBH);
            if (phieu == null)
                throw new InvalidOperationException($"Không tìm thấy phiếu bảo hành '{maPBH}'.");

            try
            {
                return _dalPBH.CapNhatKetQua(maPBH, ketQua);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật kết quả phiếu bảo hành {maPBH}: " + ex.Message, ex);
            }
        }

        /// 
        /// Kiểm tra sản phẩm có còn trong thời hạn bảo hành hay không,
        /// dựa vào NgayKetThuc trên phiếu bảo hành hiện tại (TrangThai = 'Đang Xử Lý' hoặc 'Hoàn Thành').
        
        /// <param name="maSerial">Số serial sản phẩm cần kiểm tra.
        /// 
        /// Tuple (conBaoHanh, ngayHetHan):
        ///   conBaoHanh = true nếu sản phẩm còn trong hạn bảo hành,
        ///   ngayHetHan = ngày kết thúc bảo hành (DateTime.MinValue nếu không có phiếu).
        /// 
        public (bool conBaoHanh, DateTime ngayHetHan) KiemTraConBaoHanh(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Mã serial không được để trống.", nameof(maSerial));

            DataTable dt = _dalPBH.DSTheoMaSerial(maSerial);
            if (dt.Rows.Count == 0)
                return (false, DateTime.MinValue);

            // Lấy phiếu bảo hành mới nhất (NgayBatDau lớn nhất)
            DateTime ngayKetThucMuaNhat = DateTime.MinValue;
            foreach (DataRow row in dt.Rows)
            {
                string trangThai = row["TrangThai"]?.ToString() ?? string.Empty;
                // Chỉ tính phiếu đang xử lý hoặc đã hoàn thành
                if (trangThai.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase)
                    || trangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime ngayKT = Convert.ToDateTime(row["NgayKetThuc"]);
                    if (ngayKT > ngayKetThucMuaNhat)
                        ngayKetThucMuaNhat = ngayKT;
                }
            }

            if (ngayKetThucMuaNhat == DateTime.MinValue)
                return (false, DateTime.MinValue);

            return (DateTime.Today <= ngayKetThucMuaNhat, ngayKetThucMuaNhat);
        }

        /// <summary>
        /// Lấy thông tin bảo hành của sản phẩm dựa trên mã Serial.
        /// </summary>
        /// <param name="maSerial">Mã serial sản phẩm.</param>
        /// <returns>Tuple (spTonTai, daBan, conBaoHanh, ngayMua, thoiGianBaoHanhThang, ngayHetHan)</returns>
        public (bool spTonTai, bool daBan, bool conBaoHanh, DateTime ngayMua, int thoiGianBaoHanhThang, DateTime ngayHetHan) LayThongTinBaoHanhSanPham(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                return (false, false, false, DateTime.MinValue, 0, DateTime.MinValue);

            // 1. Kiểm tra sản phẩm tồn tại
            DTO_SanPham? sp = _dalSP.DSTheoMaSerialSP(maSerial);
            if (sp == null)
                return (false, false, false, DateTime.MinValue, 0, DateTime.MinValue);

            // 2. Kiểm tra xem sản phẩm đã được bán chưa
            DTO_ChiTietDonHang? ctdh = _dalCTDH.DSTheoMaSerialSP(maSerial);
            if (ctdh == null)
            {
                // Sản phẩm tồn tại nhưng chưa bán
                return (true, false, false, DateTime.MinValue, 0, DateTime.MinValue);
            }

            // 3. Tìm thông tin đơn hàng để lấy ngày đặt hàng
            DTO_DonHang? dh = _dalDH.DSTheoMaDH(ctdh.MaDH);
            if (dh == null)
            {
                // Không tìm thấy đơn hàng tương ứng (lỗi dữ liệu hoặc trường hợp đặc biệt), coi như chưa bán hoặc không hợp lệ
                return (true, false, false, DateTime.MinValue, 0, DateTime.MinValue);
            }

            // 4. Lấy thời gian bảo hành từ Loại Sản Phẩm
            int thoiGianBaoHanhThang = 0;
            DTO_LoaiSanPham? lsp = _dalLSP.TimLoaiSP(sp.MaLoaiSP);
            if (lsp != null)
            {
                thoiGianBaoHanhThang = lsp.ThoiGianBaoHanh;
            }

            // Ngày mua là ngày đặt hàng
            DateTime ngayMua = dh.NgayDat;
            // Tính ngày hết hạn bảo hành dựa trên ngày mua và thời gian bảo hành
            DateTime ngayHetHan = ngayMua.AddMonths(thoiGianBaoHanhThang);

            // Kiểm tra xem hiện tại còn trong thời hạn bảo hành không
            bool conBaoHanh = DateTime.Today <= ngayHetHan;

            return (true, true, conBaoHanh, ngayMua, thoiGianBaoHanhThang, ngayHetHan);
        }

        /// <summary>
        /// Cập nhật lý do lỗi của phiếu bảo hành.
        /// </summary>
        public bool CapNhatLyDoLoiBaoHanh(string maPBH, string lyDoLoi)
        {
            if (string.IsNullOrWhiteSpace(maPBH))
                throw new ArgumentException("Mã phiếu bảo hành không được để trống.", nameof(maPBH));

            DTO_PhieuBaoHanh? phieu = _dalPBH.DSTheoMaPhieuBaoHanh(maPBH);
            if (phieu == null)
                throw new InvalidOperationException($"Không tìm thấy phiếu bảo hành '{maPBH}'.");

            try
            {
                return _dalPBH.CapNhatLyDoLoi(maPBH, lyDoLoi);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật lý do lỗi phiếu bảo hành {maPBH}: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════════════════════════════════════════
        //  KHU VỰC: ĐƠN KHIẾU NẠI
        // ══════════════════════════════════════════════════════════════════════════════

        /// 
        /// Lấy toàn bộ danh sách đơn khiếu nại trong hệ thống.
        
        /// DataTable chứa tất cả đơn khiếu nại.
        public DataTable LayDanhSachKhieuNai()
        {
            try
            {
                return _dalDKN.DSTatCaDonKhieuNai();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách đơn khiếu nại: " + ex.Message, ex);
            }
        }

        /// 
        /// Lấy thông tin chi tiết một đơn khiếu nại theo mã đơn.
        
        /// <param name="maDonKN">Mã đơn khiếu nại cần tìm.
        /// DTO_DonKhieuNai nếu tìm thấy, null nếu không có.
        public DTO_DonKhieuNai? LayTheoMaDonKhieuNai(string maDonKN)
        {
            if (string.IsNullOrWhiteSpace(maDonKN))
                throw new ArgumentException("Mã đơn khiếu nại không được để trống.", nameof(maDonKN));

            try
            {
                return _dalDKN.DSTheoMaDonKhieuNai(maDonKN);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn khiếu nại {maDonKN}: " + ex.Message, ex);
            }
        }

        /// 
        /// Lấy danh sách đơn khiếu nại liên quan đến một serial sản phẩm.
        
        /// <param name="maSerial">Số serial sản phẩm.
        /// DataTable chứa các đơn khiếu nại liên quan.
        public DataTable LayDanhSachKhieuNaiTheoSerial(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
                throw new ArgumentException("Mã serial không được để trống.", nameof(maSerial));

            try
            {
                return _dalDKN.DSTheoMaSerial(maSerial);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn khiếu nại của serial {maSerial}: " + ex.Message, ex);
            }
        }

        /// 
        /// Tạo đơn khiếu nại mới với các quy tắc nghiệp vụ:
        /// - MaDH phải liên kết với DonHang có TrangThai = 'Hoàn Thành'.
        /// - NoiDung không được rỗng.
        /// - TrangThai khởi tạo = 'Đang Xử Lý'.
        
        /// <param name="dkn">Đối tượng DTO_DonKhieuNai cần tạo.
        /// true nếu tạo đơn thành công, false nếu thất bại.
        /// <exception cref="ArgumentException">Khi dữ liệu đầu vào không hợp lệ.
        /// <exception cref="InvalidOperationException">Khi vi phạm điều kiện nghiệp vụ khiếu nại.
        public bool TaoDonKhieuNai(DTO_DonKhieuNai dkn)
        {
            // Kiểm tra trường bắt buộc 
            if (dkn == null)
                throw new ArgumentException("Thông tin đơn khiếu nại không được null.");
            if (string.IsNullOrWhiteSpace(dkn.MaDonKN))
                throw new ArgumentException("Mã đơn khiếu nại không được để trống.");
            if (string.IsNullOrWhiteSpace(dkn.MaDH))
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(dkn.MaKH))
                throw new ArgumentException("Mã khách hàng không được để trống.");
            if (string.IsNullOrWhiteSpace(dkn.NoiDung))
                throw new ArgumentException("Nội dung khiếu nại không được để trống.");

            // Kiểm tra DonHang tồn tại và đã Hoàn Thành 
            DTO_DonHang? donHang = _dalDH.DSTheoMaDH(dkn.MaDH);
            if (donHang == null)
                throw new InvalidOperationException($"Đơn hàng '{dkn.MaDH}' không tồn tại trong hệ thống.");
            if (!donHang.TrangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    $"Chỉ có thể khiếu nại đơn hàng đã 'Hoàn Thành'. " +
                    $"Đơn hàng '{dkn.MaDH}' hiện có trạng thái '{donHang.TrangThai}'.");

            // Kiểm tra MaKH trên đơn khiếu nại khớp với DonHang 
            if (!donHang.MaKH.Trim().Equals(dkn.MaKH.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    $"Khách hàng '{dkn.MaKH}' không phải người đặt đơn hàng '{dkn.MaDH}'.");

            // Thiết lập TrangThai mặc định 
            dkn.TrangThai = "Đang Xử Lý";

            // Gọi DAL thêm đơn khiếu nại 
            try
            {
                return _dalDKN.ThemDonKhieuNai(dkn);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo đơn khiếu nại: " + ex.Message, ex);
            }
        }

        /// 
        /// Cập nhật trạng thái đơn khiếu nại.
        /// Luồng hợp lệ: 'Đang Xử Lý' → 'Đã Giải Quyết' hoặc 'Từ Chối'.
        /// TrangThai hợp lệ: 'Đang Xử Lý' | 'Đã Giải Quyết' | 'Từ Chối'.
        
        /// <param name="maDonKN">Mã đơn khiếu nại cần cập nhật.
        /// <param name="trangThai">Trạng thái mới.
        /// true nếu cập nhật thành công.
        public bool CapNhatTrangThaiKhieuNai(string maDonKN, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maDonKN))
                throw new ArgumentException("Mã đơn khiếu nại không được để trống.", nameof(maDonKN));

            string[] trangThaiHopLe = { "Đang Xử Lý", "Đã Giải Quyết", "Từ Chối" };
            if (!Array.Exists(trangThaiHopLe, tt => tt.Equals(trangThai, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException(
                    $"Trạng thái '{trangThai}' không hợp lệ. Chỉ chấp nhận: Đang Xử Lý | Đã Giải Quyết | Từ Chối.");

            DTO_DonKhieuNai? donKN = _dalDKN.DSTheoMaDonKhieuNai(maDonKN);
            if (donKN == null)
                throw new InvalidOperationException($"Không tìm thấy đơn khiếu nại '{maDonKN}'.");

            // Kiểm tra luồng trạng thái hợp lệ: chỉ được chuyển từ 'Đang Xử Lý'
            if (!donKN.TrangThai.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase)
                && !trangThai.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    $"Không thể thay đổi trạng thái đơn khiếu nại đã '{donKN.TrangThai}'. " +
                    $"Chỉ có thể cập nhật từ trạng thái 'Đang Xử Lý'.");

            try
            {
                return _dalDKN.CapNhatTrangThai(maDonKN, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái đơn khiếu nại {maDonKN}: " + ex.Message, ex);
            }
        }

        /// 
        /// Cập nhật kết quả xử lý đơn khiếu nại.
        
        /// <param name="maDonKN">Mã đơn khiếu nại cần cập nhật kết quả.
        /// <param name="ketQua">Nội dung kết quả xử lý.
        /// true nếu cập nhật thành công.
        public bool CapNhatKetQuaKhieuNai(string maDonKN, string ketQua)
        {
            if (string.IsNullOrWhiteSpace(maDonKN))
                throw new ArgumentException("Mã đơn khiếu nại không được để trống.", nameof(maDonKN));

            DTO_DonKhieuNai? donKN = _dalDKN.DSTheoMaDonKhieuNai(maDonKN);
            if (donKN == null)
                throw new InvalidOperationException($"Không tìm thấy đơn khiếu nại '{maDonKN}'.");

            try
            {
                return _dalDKN.CapNhatKetQua(maDonKN, ketQua);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật kết quả đơn khiếu nại {maDonKN}: " + ex.Message, ex);
            }
        }

        /// 
        /// Cập nhật toàn bộ thông tin đơn khiếu nại (dùng khi sửa chi tiết).
        /// Không cho phép thay đổi MaDH hay MaKH sau khi đã tạo.
        
        /// <param name="dkn">Đối tượng DTO_DonKhieuNai với dữ liệu đã cập nhật.
        /// true nếu cập nhật thành công.
        public bool CapNhatDonKhieuNai(DTO_DonKhieuNai dkn)
        {
            if (dkn == null)
                throw new ArgumentException("Thông tin đơn khiếu nại không được null.");
            if (string.IsNullOrWhiteSpace(dkn.MaDonKN))
                throw new ArgumentException("Mã đơn khiếu nại không được để trống.");
            if (string.IsNullOrWhiteSpace(dkn.NoiDung))
                throw new ArgumentException("Nội dung khiếu nại không được để trống.");

            string[] trangThaiHopLe = { "Đang Xử Lý", "Đã Giải Quyết", "Từ Chối" };
            if (!Array.Exists(trangThaiHopLe, tt => tt.Equals(dkn.TrangThai, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException(
                    $"Trạng thái '{dkn.TrangThai}' không hợp lệ. Chỉ chấp nhận: Đang Xử Lý | Đã Giải Quyết | Từ Chối.");

            DTO_DonKhieuNai? existing = _dalDKN.DSTheoMaDonKhieuNai(dkn.MaDonKN);
            if (existing == null)
                throw new InvalidOperationException($"Không tìm thấy đơn khiếu nại '{dkn.MaDonKN}'.");

            try
            {
                return _dalDKN.CapNhatDonKhieuNai(dkn);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật đơn khiếu nại: " + ex.Message, ex);
            }
        }
    }
}

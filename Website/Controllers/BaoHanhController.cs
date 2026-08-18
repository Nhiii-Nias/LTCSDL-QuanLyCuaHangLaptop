using Microsoft.AspNetCore.Mvc;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace Website.Controllers
{
    public class BaoHanhController : Controller
    {
        private readonly BUS_HauMai _busHM = new BUS_HauMai();
        private readonly BUS_SanPham _busSP = new BUS_SanPham();
        private readonly BUS_DonHang _busDH = new BUS_DonHang();

        // GET /BaoHanh/Index — Form tra cứu
        public IActionResult Index(string? maSerial)
        {
            if (!string.IsNullOrWhiteSpace(maSerial))
            {
                return TraCuu(maSerial);
            }
            return View();
        }

        // POST /BaoHanh/TraCuu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TraCuu(string maSerial)
        {
            if (string.IsNullOrWhiteSpace(maSerial))
            {
                ModelState.AddModelError("", "Vui lòng nhập số serial sản phẩm.");
                return View("Index");
            }

            try
            {
                var (spTonTai, daBan, conBaoHanh, ngayMua, thoiGianBaoHanhThang, ngayHetHan) = _busHM.LayThongTinBaoHanhSanPham(maSerial.Trim());
                var dsBaoHanh = _busHM.LayDanhSachBaoHanhTheoSerial(maSerial.Trim());
                var dsDoiTra  = _busHM.LayDanhSachDoiTraTheoSerial(maSerial.Trim());

                ViewBag.MaSerial    = maSerial.Trim();
                ViewBag.SpTonTai    = spTonTai;
                ViewBag.DaBan       = daBan;
                ViewBag.ConBaoHanh  = conBaoHanh;
                ViewBag.NgayMua     = ngayMua;
                ViewBag.ThoiGianBaoHanhThang = thoiGianBaoHanhThang;
                ViewBag.NgayHetHan  = ngayHetHan;
                ViewBag.DsBaoHanh   = dsBaoHanh;
                ViewBag.DsDoiTra    = dsDoiTra;
                ViewBag.DaTimKiem   = true;
            }
            catch (Exception ex)
            {
                ViewBag.LoiTimKiem = "Lỗi tra cứu: " + ex.Message;
                ViewBag.DaTimKiem  = true;
            }

            return View("Index");
        }

        // GET /BaoHanh/DanhSach — Lịch sử bảo hành KH (cần đăng nhập)
        public IActionResult DanhSach()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/BaoHanh/DanhSach" });

            try
            {
                ViewBag.DsBaoHanh = _busHM.LayDanhSachBaoHanhTheoKH(maKH);
                ViewBag.DsDoiTra  = _busHM.LayDanhSachDoiTraTheoKH(maKH);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải dữ liệu bảo hành: " + ex.Message;
            }

            return View();
        }

        // GET /BaoHanh/YeuCau — Tạo yêu cầu bảo hành mới
        [HttpGet]
        public IActionResult YeuCau()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/BaoHanh/YeuCau" });

            var listSerials = new List<KeyValuePair<string, string>>();
            try
            {
                var dtDH = _busDH.LayTheoKhachHang(maKH);
                foreach (System.Data.DataRow rowDH in dtDH.Rows)
                {
                    string maDH = rowDH["MaDH"].ToString()!.Trim();
                    string trangThaiDH = rowDH["TrangThai"].ToString()!;
                    if (trangThaiDH.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase))
                    {
                        var dtCT = _busDH.LayChiTietDonHang(maDH);
                        foreach (System.Data.DataRow rowCT in dtCT.Rows)
                        {
                            string serial = rowCT["MaSerialSP"].ToString()!.Trim();
                            string tenLoai = rowCT["TenLoai"].ToString()!;
                            string danhMuc = rowCT["DanhMuc"].ToString()!;
                            string icon = danhMuc switch { "Laptop" => "💻", "Chuột" => "🖱️", "Bàn Phím" => "⌨️", _ => "📦" };
                            listSerials.Add(new KeyValuePair<string, string>(serial, $"{icon} {serial} - {tenLoai}"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi lấy danh sách sản phẩm: " + ex.Message;
            }

            ViewBag.Serials = listSerials;
            return View();
        }

        // POST /BaoHanh/YeuCau — Xác nhận tạo yêu cầu bảo hành
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YeuCau(string maSerial, string lyDoLoi)
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan");

            if (string.IsNullOrWhiteSpace(maSerial) || string.IsNullOrWhiteSpace(lyDoLoi))
            {
                TempData["Error"] = "Vui lòng chọn sản phẩm và nhập lý do lỗi.";
                return RedirectToAction("YeuCau");
            }

            try
            {
                maSerial = maSerial.Trim();
                lyDoLoi = lyDoLoi.Trim();

                // 1. Kiểm tra tính hợp lệ của Serial (phải thuộc về khách hàng)
                var ctdh = _busDH.LayChiTietTheoSerial(maSerial);
                if (ctdh == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin mua hàng của sản phẩm này.";
                    return RedirectToAction("YeuCau");
                }

                var dh = _busDH.LayTheoMa(ctdh.MaDH);
                if (dh == null || dh.MaKH != maKH || !dh.TrangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Error"] = "Sản phẩm này không thuộc đơn hàng đã hoàn thành của bạn.";
                    return RedirectToAction("YeuCau");
                }

                // 2. Kiểm tra xem sản phẩm này có đang nằm trong yêu cầu bảo hành chưa xử lý hay không
                var dtTatCaBH = _busHM.LayDanhSachBaoHanhTheoSerial(maSerial);
                foreach (System.Data.DataRow row in dtTatCaBH.Rows)
                {
                    string tt = row["TrangThai"]?.ToString()?.Trim() ?? "";
                    if (tt.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase))
                    {
                        TempData["Error"] = "Sản phẩm này đang có một yêu cầu bảo hành chưa được xử lý.";
                        return RedirectToAction("YeuCau");
                    }
                }

                // 3. Lấy thông tin loại sản phẩm để lấy thời hạn bảo hành
                var sp = _busSP.LayTheoSerial(maSerial);
                if (sp == null)
                {
                    TempData["Error"] = "Không tìm thấy sản phẩm trên hệ thống.";
                    return RedirectToAction("YeuCau");
                }
                var lsp = _busSP.LayLoaiSPTheoMa(sp.MaLoaiSP);
                if (lsp == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin loại sản phẩm.";
                    return RedirectToAction("YeuCau");
                }

                // 4. Tạo phiếu bảo hành mới
                string maPBH = TaoMaPBHMoi();
                var pbh = new DTO_PhieuBaoHanh
                {
                    MaPBH = maPBH,
                    MaDH = dh.MaDH,
                    MaKH = maKH,
                    MaSerialSP = maSerial,
                    NgayBatDau = DateTime.Today,
                    NgayKetThuc = DateTime.Today.AddMonths(lsp.ThoiGianBaoHanh), // Mặc định tính theo thời hạn sản phẩm
                    TrangThai = "Đang Xử Lý",
                    LyDoLoi = lyDoLoi,
                    KetQua = string.Empty
                };

                // Lấy thông tin khách hàng lẻ/sỉ
                var busKH = new BUS_KhachHang();
                var kh = busKH.LayTheoMa(maKH);
                string loaiKH = kh?.LoaiKH ?? "Lẻ";

                bool ok = _busHM.TaoPhieuBaoHanh(pbh, loaiKH, lsp.ThoiGianBaoHanh);
                if (ok)
                {
                    TempData["Success"] = $"Gửi yêu cầu bảo hành thành công! Mã phiếu: {maPBH}";
                    return RedirectToAction("DanhSach");
                }
                else
                {
                    TempData["Error"] = "Gửi yêu cầu bảo hành thất bại.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi xử lý yêu cầu bảo hành: " + ex.Message;
            }

            return RedirectToAction("YeuCau");
        }

        private string TaoMaPBHMoi()
        {
            try
            {
                var dt = _busHM.LayDanhSachBaoHanh();
                int soLon = 0;
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string ma = row["MaPBH"]?.ToString()?.Trim() ?? "";
                    if (ma.StartsWith("PBH") && ma.Length == 10)
                    {
                        if (int.TryParse(ma.Substring(3), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "PBH" + (soLon + 1).ToString().PadLeft(7, '0');
            }
            catch
            {
                return "PBH0000001";
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;
using Website.Models;
using System.Text.Json;

namespace Website.Controllers
{
    public class GioHangController : Controller
    {
        private readonly BUS_SanPham  _busSP = new BUS_SanPham();
        private readonly BUS_DonHang  _busDH = new BUS_DonHang();
        private readonly BUS_KhuyenMai _busKM = new BUS_KhuyenMai();
        private readonly IConfiguration _config;

        private const string SESSION_GIO_HANG = "GioHang_JSON";

        public GioHangController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // Đọc giỏ hàng từ Session
        private List<GioHangItem> DocGioHang()
        {
            var json = HttpContext.Session.GetString(SESSION_GIO_HANG);
            if (string.IsNullOrEmpty(json)) return new List<GioHangItem>();
            try { return JsonSerializer.Deserialize<List<GioHangItem>>(json) ?? new(); }
            catch { return new(); }
        }

        // Ghi giỏ hàng vào Session + cập nhật badge
        private void LuuGioHang(List<GioHangItem> gio)
        {
            HttpContext.Session.SetString(SESSION_GIO_HANG, JsonSerializer.Serialize(gio));
            HttpContext.Session.SetString("SoLuongGio",  gio.Sum(x => x.SoLuong).ToString());
            HttpContext.Session.SetString("TongTienGio", gio.Sum(x => x.ThanhTien).ToString());
        }

        // GET /GioHang
        public IActionResult Index()
        {
            var gio = DocGioHang();
            return View(gio);
        }

        // POST /GioHang/Them
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Them(string maLoaiSP, int soLuong = 1, bool redirectToDatHang = false)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
            {
                TempData["Error"] = "Sản phẩm không hợp lệ.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var lsp = _busSP.LayLoaiSPTheoMa(maLoaiSP);
                if (lsp == null)
                {
                    TempData["Error"] = "Sản phẩm không tồn tại.";
                    return Redirect(Request.Headers["Referer"].ToString() ?? "/");
                }

                // Kiểm tra tồn kho
                var dsSP = _busSP.LayDanhSachTheoLoaiSP(maLoaiSP);
                int tonKho = 0;
                foreach (System.Data.DataRow r in dsSP.Rows)
                {
                    if (r["TrangThai"].ToString() == "Trong Kho" && !Convert.ToBoolean(r["IsDeleted"]))
                    {
                        tonKho++;
                    }
                }

                if (tonKho <= 0)
                {
                    TempData["Warning"] = $"Sản phẩm '{lsp.TenLoai}' hiện đang hết hàng.";
                    return Redirect(Request.Headers["Referer"].ToString() ?? "/");
                }

                var hsx = _busSP.LayHSXTheoMa(lsp.MaHang);

                var gio = DocGioHang();
                var existing = gio.FirstOrDefault(x => x.MaLoaiSP == maLoaiSP);

                if (existing != null)
                {
                    int soLuongMoi = existing.SoLuong + soLuong;
                    existing.SoLuong = Math.Min(soLuongMoi, tonKho);
                }
                else
                {
                    gio.Add(new GioHangItem
                    {
                        MaLoaiSP = lsp.MaLoaiSP,
                        TenLoai  = lsp.TenLoai,
                        DanhMuc  = lsp.DanhMuc,
                        MaHang   = lsp.MaHang,
                        TenHang  = hsx?.TenHang ?? "",
                        GiaBan   = lsp.GiaBanGoc,
                        SoLuong  = Math.Min(soLuong, tonKho)
                    });
                }

                LuuGioHang(gio);
                TempData["Success"] = $"Đã thêm '{lsp.TenLoai}' vào giỏ hàng.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi thêm vào giỏ: " + ex.Message;
            }

            if (redirectToDatHang)
                return RedirectToAction("DatHang");

            return Redirect(Request.Headers["Referer"].ToString() ?? "/GioHang");
        }

        // POST /GioHang/Xoa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Xoa(string maLoaiSP)
        {
            var gio = DocGioHang();
            gio.RemoveAll(x => x.MaLoaiSP == maLoaiSP);
            LuuGioHang(gio);
            TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction("Index");
        }

        // POST /GioHang/CapNhatSoLuong
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatSoLuong(string maLoaiSP, int soLuong)
        {
            var gio = DocGioHang();
            var item = gio.FirstOrDefault(x => x.MaLoaiSP == maLoaiSP);
            if (item != null && soLuong > 0)
                item.SoLuong = soLuong;
            else if (item != null && soLuong <= 0)
                gio.Remove(item);
            LuuGioHang(gio);
            return RedirectToAction("Index");
        }

        // GET /GioHang/DatHang
        [HttpGet]
        public IActionResult DatHang()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/GioHang/DatHang" });

            var gio = DocGioHang();
            if (!gio.Any())
            {
                TempData["Warning"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }

            // 1. Tạo chi tiết đơn hàng tạm từ giỏ hàng với các serial thực tế trong kho để kiểm tra khuyến mãi
            var chiTietTam = new List<DTO_ChiTietDonHang>();
            foreach (var item in gio)
            {
                var dsSerial = _busSP.LayDanhSachTheoLoaiSP(item.MaLoaiSP);
                int count = 0;
                foreach (System.Data.DataRow r in dsSerial.Rows)
                {
                    if (r["TrangThai"].ToString() == "Trong Kho" && !Convert.ToBoolean(r["IsDeleted"]))
                    {
                        chiTietTam.Add(new DTO_ChiTietDonHang
                        {
                            MaSerialSP = r["MaSerialSP"].ToString()!.Trim(),
                            GiaBan = item.GiaBan
                        });
                        count++;
                        if (count == item.SoLuong)
                            break;
                    }
                }
            }

            // 2. Lấy danh sách khuyến mãi hợp lệ
            var dsKM = _busKM.LayDanhSachKMCoTheThuHuong(maKH, chiTietTam, DateTime.Now);
            dsKM = dsKM.FindAll(km => km.IsHienThi);
            var listKM = new List<object>();
            foreach (var km in dsKM)
            {
                decimal reduction = _busKM.TinhTienGiam(km, chiTietTam);
                listKM.Add(new
                {
                    MaKM = km.MaKM,
                    TenKM = km.TenKM,
                    MoTa = km.MoTa,
                    TienGiam = reduction
                });
            }
            ViewBag.DanhSachKM = listKM;

            var vm = new DatHangViewModel { DanhSachSanPham = gio };
            return View(vm);
        }

        // POST /GioHang/XacNhanDatHang
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhanDatHang(DatHangViewModel model)
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var gio = DocGioHang();
            if (!gio.Any())
            {
                TempData["Warning"] = "Giỏ hàng trống.";
                return RedirectToAction("Index");
            }

            try
            {
                // MaNV mặc định cho đặt hàng từ website
                string maNV = _config["AppSettings:MaNVWebMacDinh"] ?? "NV00000001";
                var phuongThucOK = new[] { "Tiền Mặt", "Chuyển Khoản", "Thẻ" };
                if (!phuongThucOK.Contains(model.PhuongThucThanhToan))
                    model.PhuongThucThanhToan = "Tiền Mặt";

                // Tạo danh sách mã serial cần bán — lấy serial tự động từ BUS
                var danhSachSerial = new List<string>();
                foreach (var item in gio)
                {
                    // Lấy các serial còn hàng của LoaiSP
                    var dsSerial = _busSP.LayDanhSachTheoLoaiSP(item.MaLoaiSP);
                    var serials = new List<string>();
                    foreach (System.Data.DataRow r in dsSerial.Rows)
                    {
                        if (r["TrangThai"].ToString() == "Trong Kho" && !Convert.ToBoolean(r["IsDeleted"]))
                        {
                            serials.Add(r["MaSerialSP"].ToString()!);
                            if (serials.Count == item.SoLuong)
                                break;
                        }
                    }

                    if (serials.Count < item.SoLuong)
                    {
                        TempData["Error"] = $"Sản phẩm '{item.TenLoai}' không đủ số lượng trong kho (cần {item.SoLuong}, còn {serials.Count}).";
                        return RedirectToAction("DatHang");
                    }

                    danhSachSerial.AddRange(serials);
                }

                decimal tongTien = gio.Sum(x => x.ThanhTien);

                string maDH = _busDH.TaoMaDHMoi();
                var donHang = new DTO_DonHang
                {
                    MaDH                 = maDH,
                    MaNV                 = maNV,
                    MaKH                 = maKH,
                    NgayDat              = DateTime.Now,
                    TongTien             = tongTien,
                    PhuongThucThanhToan  = model.PhuongThucThanhToan,
                    TrangThai            = "Chờ Xử Lý",
                    MaKM                 = model.MaKM
                };

                bool ketQua = _busDH.TaoDonHang(donHang, danhSachSerial);
                if (!ketQua)
                {
                    TempData["Error"] = "Tạo đơn hàng không thành công.";
                    return RedirectToAction("DatHang");
                }

                // Xóa giỏ hàng
                LuuGioHang(new List<GioHangItem>());

                if (model.PhuongThucThanhToan == "Chuyển Khoản")
                {
                    return RedirectToAction("ThanhToanQR", new { maDH });
                }

                TempData["Success"] = $"Đặt hàng thành công! Mã đơn: {maDH}";
                return RedirectToAction("ChiTiet", "DonHang", new { maDH });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi đặt hàng: " + ex.Message;
                return RedirectToAction("DatHang");
            }
        }

        // GET /GioHang/ThanhToanQR
        [HttpGet]
        public IActionResult ThanhToanQR(string maDH)
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan");

            if (string.IsNullOrWhiteSpace(maDH))
                return RedirectToAction("Index", "Home");

            try
            {
                var donHang = _busDH.LayTheoMa(maDH);
                if (donHang == null || donHang.MaKH != maKH)
                {
                    TempData["Error"] = "Đơn hàng không tồn tại hoặc không thuộc về bạn.";
                    return RedirectToAction("Index", "Home");
                }

                // Nếu đơn hàng đã thay đổi trạng thái (ví dụ đã được quét và chuyển trạng thái)
                if (donHang.TrangThai != "Chờ Xử Lý")
                {
                    return RedirectToAction("ChiTiet", "DonHang", new { maDH });
                }

                // Tạo URL quét QR tuyệt đối
                string scheme = Request.Scheme ?? "http";
                string host = Request.Host.Value;
                string scanUrl = $"{scheme}://{host}/GioHang/XacNhanQuetQR?maDH={maDH}";
                
                // Sử dụng API QRserver
                string qrImageUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=250x250&data={Uri.EscapeDataString(scanUrl)}";

                ViewBag.DonHang = donHang;
                ViewBag.ScanUrl = scanUrl;
                ViewBag.QrImageUrl = qrImageUrl;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải thông tin thanh toán QR: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET /GioHang/KiemTraTrangThaiDH
        [HttpGet]
        public IActionResult KiemTraTrangThaiDH(string maDH)
        {
            if (string.IsNullOrWhiteSpace(maDH))
                return Json(new { success = false, message = "Mã đơn hàng không hợp lệ." });

            try
            {
                var donHang = _busDH.LayTheoMa(maDH);
                if (donHang == null)
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng." });

                return Json(new { success = true, trangThai = donHang.TrangThai });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET /GioHang/XacNhanQuetQR
        [HttpGet]
        public IActionResult XacNhanQuetQR(string maDH)
        {
            if (string.IsNullOrWhiteSpace(maDH))
            {
                ViewBag.Message = "Mã đơn hàng không hợp lệ.";
                ViewBag.IsSuccess = false;
                return View();
            }

            try
            {
                var donHang = _busDH.LayTheoMa(maDH);
                if (donHang == null)
                {
                    ViewBag.Message = $"Không tìm thấy đơn hàng {maDH}.";
                    ViewBag.IsSuccess = false;
                    return View();
                }

                if (donHang.TrangThai == "Chờ Xử Lý")
                {
                    bool ok = _busDH.ChuyenSangDangGiao(maDH);
                    if (ok)
                    {
                        ViewBag.Message = $"Đơn hàng {maDH} đã được xác nhận thanh toán chuyển khoản thành công và chuyển trạng thái sang Đang Giao.";
                        ViewBag.IsSuccess = true;
                    }
                    else
                    {
                        ViewBag.Message = "Không thể cập nhật trạng thái đơn hàng sang Đang Giao.";
                        ViewBag.IsSuccess = false;
                    }
                }
                else if (donHang.TrangThai == "Đang Giao" || donHang.TrangThai == "Hoàn Thành")
                {
                    ViewBag.Message = $"Đơn hàng {maDH} đã được xác nhận thanh toán chuyển khoản từ trước.";
                    ViewBag.IsSuccess = true;
                }
                else
                {
                    ViewBag.Message = $"Đơn hàng {maDH} ở trạng thái '{donHang.TrangThai}' nên không thể chuyển thanh toán.";
                    ViewBag.IsSuccess = false;
                }

                ViewBag.DonHang = donHang;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi xử lý quét QR: " + ex.Message;
                ViewBag.IsSuccess = false;
                return View();
            }
        }
    }
}

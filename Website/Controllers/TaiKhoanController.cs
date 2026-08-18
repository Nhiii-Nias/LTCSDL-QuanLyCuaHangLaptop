using Microsoft.AspNetCore.Mvc;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;
using Website.Models;

namespace Website.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly BUS_TaiKhoan  _busTK = new BUS_TaiKhoan();
        private readonly BUS_KhachHang _busKH = new BUS_KhachHang();

        // ── ĐĂNG KÝ ─────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult DangKy()
        {
            if (HttpContext.Session.GetString("MaKH") != null)
                return RedirectToAction("Index", "Home");
            return View(new DangKyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangKy(DangKyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Bước 1: Tạo DTO_KhachHang
                var kh = new DTO_KhachHang
                {
                    TenKH  = model.TenKH.Trim(),
                    Email  = model.Email?.Trim() ?? string.Empty,
                    SDT    = model.SDT?.Trim()   ?? string.Empty,
                    DiaChi = model.DiaChi?.Trim() ?? string.Empty,
                    LoaiKH = "Lẻ"
                };

                // Bước 2: DTO_KhachHangLe
                var khLe = new DTO_KhachHangLe
                {
                    LaHSSV  = model.LaHSSV,
                    SinhNhat = model.LaHSSV ? model.SinhNhat : null
                };

                // Bước 3: Tạo khách hàng (transaction: KhachHang + KhachHangLe)
                bool taokh = _busKH.ThemKhachHangLe(kh, khLe);
                if (!taokh)
                {
                    ModelState.AddModelError("", "Không thể tạo tài khoản. Vui lòng thử lại.");
                    return View(model);
                }

                // Bước 4: Tạo tài khoản đăng nhập
                var tk = new DTO_TaiKhoanKH
                {
                    MaKH        = kh.MaKH,
                    TenDangNhap = model.TenDangNhap.Trim(),
                    MatKhau     = model.MatKhau   // BUS sẽ hash SHA-256
                };

                bool taotk = _busTK.DangKyTaiKhoanKH(tk);
                if (!taotk)
                {
                    ModelState.AddModelError("", "Không thể tạo tài khoản đăng nhập. Vui lòng thử lại.");
                    return View(model);
                }

                TempData["Success"] = $"Đăng ký thành công! Chào mừng {model.TenKH}. Vui lòng đăng nhập.";
                return RedirectToAction("DangNhap");
            }
            catch (InvalidOperationException ex)
            {
                // Trùng tên đăng nhập, trùng email, ...
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                return View(model);
            }
        }

        // ── ĐĂNG NHẬP ───────────────────────────────────────────────────
        [HttpGet]
        public IActionResult DangNhap(string? returnUrl)
        {
            if (HttpContext.Session.GetString("MaKH") != null)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View(new DangNhapViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangNhap(DangNhapViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var tk = _busTK.DangNhapKH(model.TenDangNhap.Trim(), model.MatKhau);

                if (tk == null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản bị khóa.");
                    return View(model);
                }

                // Lấy thông tin khách hàng
                var kh = _busKH.LayTheoMa(tk.MaKH);
                if (kh == null)
                {
                    ModelState.AddModelError("", "Tài khoản không hợp lệ.");
                    return View(model);
                }

                // Lưu session
                HttpContext.Session.SetString("MaKH",    kh.MaKH);
                HttpContext.Session.SetString("MaTK",    tk.MaTK);
                HttpContext.Session.SetString("TenKH",   kh.TenKH);
                HttpContext.Session.SetString("LoaiKH",  kh.LoaiKH);

                TempData["Success"] = $"Đăng nhập thành công! Chào mừng {kh.TenKH}.";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                return View(model);
            }
        }

        // ── ĐĂNG XUẤT ───────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Đã đăng xuất thành công.";
            return RedirectToAction("Index", "Home");
        }

        // ── THÔNG TIN TÀI KHOẢN ─────────────────────────────────────────
        [HttpGet]
        public IActionResult ThongTin()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", new { returnUrl = "/TaiKhoan/ThongTin" });

            try
            {
                var kh    = _busKH.LayTheoMa(maKH);
                var khLe  = _busKH.LayThongTinLe(maKH);
                ViewBag.KhachHang     = kh;
                ViewBag.KhachHangLe   = khLe;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải thông tin tài khoản: " + ex.Message;
            }

            return View();
        }

        // ── CẬP NHẬT THÔNG TIN LIÊN HỆ ──────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatThongTin(string email, string sdt, string diaChi)
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap");

            try
            {
                var kh = _busKH.LayTheoMa(maKH);
                if (kh == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToAction("ThongTin");
                }

                kh.Email = email?.Trim() ?? string.Empty;
                kh.SDT = sdt?.Trim() ?? string.Empty;
                kh.DiaChi = diaChi?.Trim() ?? string.Empty;

                _busKH.CapNhat(kh);
                TempData["Success"] = "Cập nhật thông tin thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("ThongTin");
        }

        // ── ĐỔI MẬT KHẨU ────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DoiMatKhau(string matKhauCu, string matKhauMoi, string xacNhanMatKhauMoi)
        {
            var maTK = HttpContext.Session.GetString("MaTK");
            if (string.IsNullOrEmpty(maTK))
                return RedirectToAction("DangNhap");

            if (matKhauMoi != xacNhanMatKhauMoi)
            {
                TempData["Error"] = "Mật khẩu mới và xác nhận không khớp.";
                return RedirectToAction("ThongTin");
            }

            try
            {
                _busTK.DoiMatKhauKH(maTK, matKhauCu, matKhauMoi);
                TempData["Success"] = "Đổi mật khẩu thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("ThongTin");
        }
    }
}

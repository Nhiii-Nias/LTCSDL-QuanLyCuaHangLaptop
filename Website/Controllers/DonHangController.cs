using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BUS_HTQLCuaHangLaptop;

namespace Website.Controllers
{
    public class DonHangController : Controller
    {
        private readonly BUS_DonHang _busDH = new BUS_DonHang();

        // Kiểm tra đăng nhập
        private string? GetMaKH() => HttpContext?.Session?.GetString("MaKH");

        // GET /DonHang/Index — Lịch sử đơn hàng
        public IActionResult Index()
        {
            var maKH = GetMaKH();
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/DonHang/Index" });

            try
            {
                ViewBag.DanhSachDonHang = _busDH.LayTheoKhachHang(maKH);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải danh sách đơn hàng: " + ex.Message;
                ViewBag.DanhSachDonHang = null;
            }

            return View();
        }

        // GET /DonHang/ChiTiet/{maDH}
        public IActionResult ChiTiet(string maDH)
        {
            var maKH = GetMaKH();
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan");

            if (string.IsNullOrWhiteSpace(maDH))
                return RedirectToAction("Index");

            try
            {
                var donHang = _busDH.LayTheoMa(maDH);
                if (donHang == null || donHang.MaKH != maKH)
                {
                    TempData["Error"] = "Đơn hàng không tồn tại hoặc không thuộc về bạn.";
                    return RedirectToAction("Index");
                }

                ViewBag.DonHang = donHang;
                ViewBag.ChiTiet = _busDH.LayChiTietDonHang(maDH);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải chi tiết đơn hàng: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST /DonHang/Huy/{maDH}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Huy(string maDH)
        {
            var maKH = GetMaKH();
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan");

            try
            {
                var donHang = _busDH.LayTheoMa(maDH);
                if (donHang == null || donHang.MaKH != maKH)
                {
                    TempData["Error"] = "Đơn hàng không tồn tại hoặc không thuộc về bạn.";
                    return RedirectToAction("Index");
                }

                _busDH.HuyDonHang(maDH);
                TempData["Success"] = $"Đã hủy đơn hàng {maDH} thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}

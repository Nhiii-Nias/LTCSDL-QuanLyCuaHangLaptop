using Microsoft.AspNetCore.Mvc;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Website.Controllers
{
    public class KhieuNaiController : Controller
    {
        private readonly BUS_HauMai _busHM = new BUS_HauMai();
        private readonly BUS_SanPham _busSP = new BUS_SanPham();
        private readonly BUS_DonHang _busDH = new BUS_DonHang();

        // GET /KhieuNai/DanhSach — Danh sách khiếu nại của khách hàng (yêu cầu đăng nhập)
        public IActionResult DanhSach()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/KhieuNai/DanhSach" });

            try
            {
                // Lấy toàn bộ đơn khiếu nại từ hệ thống và lọc theo MaKH
                var dtAll = _busHM.LayDanhSachKhieuNai();
                var dtMyKN = dtAll.Clone();
                foreach (System.Data.DataRow row in dtAll.Rows)
                {
                    if (row["MaKH"].ToString()!.Trim().Equals(maKH, StringComparison.OrdinalIgnoreCase))
                    {
                        dtMyKN.ImportRow(row);
                    }
                }
                ViewBag.DsKhieuNai = dtMyKN;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải danh sách khiếu nại: " + ex.Message;
            }

            return View();
        }

        // GET /KhieuNai/YeuCau — Form gửi khiếu nại mới
        [HttpGet]
        public IActionResult YeuCau()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/KhieuNai/YeuCau" });

            var listSerials = new List<KeyValuePair<string, string>>();
            try
            {
                var dtDH = _busDH.LayTheoKhachHang(maKH);
                foreach (System.Data.DataRow rowDH in dtDH.Rows)
                {
                    string maDH = rowDH["MaDH"].ToString()!.Trim();
                    string trangThaiDH = rowDH["TrangThai"].ToString()!;

                    // Ràng buộc chỉ khiếu nại nếu đơn hàng ở trạng thái "Hoàn Thành"
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
                TempData["Error"] = "Lỗi khi lấy danh sách sản phẩm khiếu nại: " + ex.Message;
            }

            ViewBag.Serials = listSerials;
            return View();
        }

        // POST /KhieuNai/YeuCau — Xác nhận tạo đơn khiếu nại
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YeuCau(string maSerial, string noiDung)
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan");

            if (string.IsNullOrWhiteSpace(maSerial) || string.IsNullOrWhiteSpace(noiDung))
            {
                TempData["Error"] = "Vui lòng chọn sản phẩm và nhập nội dung khiếu nại.";
                return RedirectToAction("YeuCau");
            }

            try
            {
                maSerial = maSerial.Trim();
                noiDung = noiDung.Trim();

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

                // 2. Tạo đơn khiếu nại mới
                string maDonKN = TaoMaDKNMoi();
                var dkn = new DTO_DonKhieuNai
                {
                    MaDonKN = maDonKN,
                    MaDH = dh.MaDH,
                    MaKH = maKH,
                    NoiDung = noiDung,
                    NgayGui = DateTime.Today,
                    TrangThai = "Đang Xử Lý",
                    NgayTao = DateTime.Now
                };

                bool ok = _busHM.TaoDonKhieuNai(dkn);
                if (ok)
                {
                    TempData["Success"] = $"Gửi đơn khiếu nại thành công! Mã đơn: {maDonKN}";
                    return RedirectToAction("DanhSach");
                }
                else
                {
                    TempData["Error"] = "Gửi đơn khiếu nại thất bại.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi xử lý gửi khiếu nại: " + ex.Message;
            }

            return RedirectToAction("YeuCau");
        }

        private string TaoMaDKNMoi()
        {
            try
            {
                var dt = _busHM.LayDanhSachKhieuNai();
                int soLon = 0;
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string ma = row["MaDonKN"]?.ToString()?.Trim() ?? "";
                    if (ma.StartsWith("DKN") && ma.Length == 10)
                    {
                        if (int.TryParse(ma.Substring(3), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "DKN" + (soLon + 1).ToString().PadLeft(7, '0');
            }
            catch
            {
                return "DKN0000001";
            }
        }
    }
}

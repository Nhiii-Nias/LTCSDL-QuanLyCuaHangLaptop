using Microsoft.AspNetCore.Mvc;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Website.Controllers
{
    public class DoiTraController : Controller
    {
        private readonly BUS_HauMai _busHM = new BUS_HauMai();
        private readonly BUS_SanPham _busSP = new BUS_SanPham();
        private readonly BUS_DonHang _busDH = new BUS_DonHang();

        // GET /DoiTra/DanhSach — Lịch sử đổi trả của khách hàng (yêu cầu đăng nhập)
        public IActionResult DanhSach()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/DoiTra/DanhSach" });

            try
            {
                ViewBag.DsDoiTra = _busHM.LayDanhSachDoiTraTheoKH(maKH);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải dữ liệu đổi trả: " + ex.Message;
            }

            return View();
        }

        // GET /DoiTra/YeuCau — Form tạo yêu cầu đổi trả mới
        [HttpGet]
        public IActionResult YeuCau()
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan", new { returnUrl = "/DoiTra/YeuCau" });

            var listSerials = new List<KeyValuePair<string, string>>();
            try
            {
                var dtDH = _busDH.LayTheoKhachHang(maKH);
                foreach (System.Data.DataRow rowDH in dtDH.Rows)
                {
                    string maDH = rowDH["MaDH"].ToString()!.Trim();
                    string trangThaiDH = rowDH["TrangThai"].ToString()!;

                    // Ràng buộc chỉ đổi trả sản phẩm nếu đơn hàng ở trạng thái "Hoàn Thành"
                    if (trangThaiDH.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase))
                    {
                        DateTime ngayDat = Convert.ToDateTime(rowDH["NgayDat"]);
                        var dtCT = _busDH.LayChiTietDonHang(maDH);
                        foreach (System.Data.DataRow rowCT in dtCT.Rows)
                        {
                            string serial = rowCT["MaSerialSP"].ToString()!.Trim();
                            string tenLoai = rowCT["TenLoai"].ToString()!;
                            string danhMuc = rowCT["DanhMuc"].ToString()!;

                            // Kiểm tra điều kiện đổi trả: trong vòng 30 ngày và chưa từng đổi trả trước đó
                            var (hopLe, _) = _busHM.KiemTraDieuKienDoiTra(serial, ngayDat);
                            if (hopLe)
                            {
                                string icon = danhMuc switch { "Laptop" => "💻", "Chuột" => "🖱️", "Bàn Phím" => "⌨️", _ => "📦" };
                                listSerials.Add(new KeyValuePair<string, string>(serial, $"{icon} {serial} - {tenLoai}"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi lấy danh sách sản phẩm đổi trả: " + ex.Message;
            }

            ViewBag.Serials = listSerials;
            return View();
        }

        // POST /DoiTra/YeuCau — Xác nhận tạo yêu cầu đổi trả
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YeuCau(string maSerial, string lyDo, string loaiXuLy)
        {
            var maKH = HttpContext.Session.GetString("MaKH");
            if (string.IsNullOrEmpty(maKH))
                return RedirectToAction("DangNhap", "TaiKhoan");

            if (string.IsNullOrWhiteSpace(maSerial) || string.IsNullOrWhiteSpace(lyDo) || string.IsNullOrWhiteSpace(loaiXuLy))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin: chọn sản phẩm, lý do và hình thức xử lý.";
                return RedirectToAction("YeuCau");
            }

            try
            {
                maSerial = maSerial.Trim();
                lyDo = lyDo.Trim();
                loaiXuLy = loaiXuLy.Trim();

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

                // 2. Kiểm tra điều kiện đổi trả từ BUS
                var (hopLe, lyDoTuChoi) = _busHM.KiemTraDieuKienDoiTra(maSerial, dh.NgayDat);
                if (!hopLe)
                {
                    TempData["Error"] = "Không đủ điều kiện đổi trả: " + lyDoTuChoi;
                    return RedirectToAction("YeuCau");
                }

                // 3. Tạo phiếu đổi trả mới
                string maPhieuDT = TaoMaPDTMoi();
                var pdt = new DTO_PhieuDoiTra
                {
                    MaPhieuDT = maPhieuDT,
                    MaDH = dh.MaDH,
                    MaSerialSP = maSerial,
                    MaKH = maKH,
                    NgayYeuCau = DateTime.Today,
                    LyDo = lyDo,
                    LoaiXuLy = loaiXuLy,
                    TrangThai = "Đang Xử Lý",
                    NgayTao = DateTime.Now
                };

                bool ok = _busHM.TaoPhieuDoiTra(pdt);
                if (ok)
                {
                    TempData["Success"] = $"Gửi yêu cầu đổi trả thành công! Mã phiếu: {maPhieuDT}";
                    return RedirectToAction("DanhSach");
                }
                else
                {
                    TempData["Error"] = "Gửi yêu cầu đổi trả thất bại.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi xử lý yêu cầu đổi trả: " + ex.Message;
            }

            return RedirectToAction("YeuCau");
        }

        private string TaoMaPDTMoi()
        {
            try
            {
                var dt = _busHM.LayDanhSachDoiTra();
                int soLon = 0;
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string ma = row["MaPhieuDT"]?.ToString()?.Trim() ?? "";
                    if (ma.StartsWith("PDT") && ma.Length == 10)
                    {
                        if (int.TryParse(ma.Substring(3), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "PDT" + (soLon + 1).ToString().PadLeft(7, '0');
            }
            catch
            {
                return "PDT0000001";
            }
        }
    }
}

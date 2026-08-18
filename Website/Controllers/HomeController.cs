using Microsoft.AspNetCore.Mvc;
using BUS_HTQLCuaHangLaptop;
using System.Diagnostics;
using Website.Models;
using System.Data;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly BUS_SanPham   _busSP = new BUS_SanPham();
        private readonly BUS_KhuyenMai _busKM = new BUS_KhuyenMai();

        // GET /
        public IActionResult Index()
        {
            try
            {
                ViewBag.DanhSachLoaiSP  = _busSP.LayDanhSachLoaiSP();
                ViewBag.DanhSachLaptop  = _busSP.LayLoaiSPTheoDanhMuc("Laptop");
                ViewBag.DanhSachChuot   = _busSP.LayLoaiSPTheoDanhMuc("Chuột");
                ViewBag.DanhSachBanPhim = _busSP.LayLoaiSPTheoDanhMuc("Bàn Phím");
                var dtKM = _busKM.LayKhuyenMaiHieuLuc(DateTime.Today);
                if (dtKM != null)
                {
                    DataView dv = dtKM.DefaultView;
                    if (dtKM.Columns.Contains("isHienThi"))
                    {
                        dv.RowFilter = "isHienThi = 1";
                    }
                    ViewBag.KhuyenMai = dv.ToTable();
                }
                else
                {
                    ViewBag.KhuyenMai = null;
                }
                ViewBag.DanhSachHSX     = _busSP.LayDanhSachHSX();
            }
            catch (Exception ex)
            {
                // Nếu chưa có DB, trang chủ vẫn load (hiển thị giao diện trống)
                ViewBag.DbError = ex.Message;
                ViewBag.DanhSachLoaiSP  = null;
                ViewBag.DanhSachLaptop  = null;
                ViewBag.DanhSachChuot   = null;
                ViewBag.DanhSachBanPhim = null;
                ViewBag.KhuyenMai       = null;
                ViewBag.DanhSachHSX     = null;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

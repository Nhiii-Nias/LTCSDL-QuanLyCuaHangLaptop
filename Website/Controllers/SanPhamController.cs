using Microsoft.AspNetCore.Mvc;
using BUS_HTQLCuaHangLaptop;
using System.Data;

namespace Website.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly BUS_SanPham _busSP = new BUS_SanPham();
        private const int TRANG_KICH_THUOC = 12;

        // GET /SanPham/Index?danhMuc=Laptop&maHang=HSX001&q=asus&trang=1
        public IActionResult Index(string? danhMuc, string? maHang, string? q, int trang = 1)
        {
            try
            {
                DataTable dtSP;

                // Ưu tiên lọc theo danhMuc hoặc maHang
                if (!string.IsNullOrWhiteSpace(danhMuc))
                    dtSP = _busSP.LayLoaiSPTheoDanhMuc(danhMuc);
                else if (!string.IsNullOrWhiteSpace(maHang))
                    dtSP = _busSP.LayLoaiSPTheoHang(maHang);
                else
                    dtSP = _busSP.LayDanhSachLoaiSP();

                // Lọc theo từ khoá (tìm kiếm trong TenLoai)
                if (!string.IsNullOrWhiteSpace(q))
                {
                    string keyword = q.Trim().ToLower();
                    var rows = dtSP.AsEnumerable()
                        .Where(r => r["TenLoai"].ToString()!.ToLower().Contains(keyword) ||
                                    r["TenHang"].ToString()!.ToLower().Contains(keyword))
                        .ToList();
                    dtSP = rows.Count > 0 ? rows.CopyToDataTable() : dtSP.Clone();
                }

                // Phân trang
                int tongSP = dtSP.Rows.Count;
                int tongTrang = (int)Math.Ceiling((double)tongSP / TRANG_KICH_THUOC);
                if (trang < 1) trang = 1;
                if (trang > tongTrang && tongTrang > 0) trang = tongTrang;

                DataTable dtHienThi = dtSP.Clone();
                int batDau = (trang - 1) * TRANG_KICH_THUOC;
                for (int i = batDau; i < Math.Min(batDau + TRANG_KICH_THUOC, tongSP); i++)
                    dtHienThi.ImportRow(dtSP.Rows[i]);

                // Danh sách hãng để filter sidebar
                ViewBag.DanhSachHSX = _busSP.LayDanhSachHSX();
                ViewBag.DanhSachLoaiSP = dtHienThi;
                ViewBag.TongSP = tongSP;
                ViewBag.TrangHienTai = trang;
                ViewBag.TongTrang = tongTrang;
                ViewBag.DanhMuc = danhMuc;
                ViewBag.MaHang = maHang;
                ViewBag.SearchQuery = q;
                ViewData["SearchQuery"] = q;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải danh sách sản phẩm: " + ex.Message;
                ViewBag.DanhSachLoaiSP = null;
                ViewBag.DanhSachHSX = null;
                ViewBag.TongSP = 0;
                ViewBag.TrangHienTai = 1;
                ViewBag.TongTrang = 0;
            }

            return View();
        }

        // GET /SanPham/ChiTiet/{maLoaiSP}
        public IActionResult ChiTiet(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                return RedirectToAction("Index");

            try
            {
                var loaiSP = _busSP.LayLoaiSPTheoMa(maLoaiSP);
                if (loaiSP == null)
                {
                    TempData["Error"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("Index");
                }

                var cauHinh = _busSP.LayCauHinhTheoLoaiSP(maLoaiSP);
                var hsx = _busSP.LayHSXTheoMa(loaiSP.MaHang);

                // Đếm số lượng còn trong kho
                var dsSP = _busSP.LayDanhSachTheoLoaiSP(maLoaiSP);
                int soLuongTonKho = dsSP.AsEnumerable()
                    .Count(r => r["TrangThai"].ToString() == "Trong Kho" &&
                                !Convert.ToBoolean(r["IsDeleted"]));

                ViewBag.LoaiSanPham = loaiSP;
                ViewBag.CauHinh = cauHinh;
                ViewBag.HangSanXuat = hsx;
                ViewBag.SoLuongTonKho = soLuongTonKho;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi tải thông tin sản phẩm: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}

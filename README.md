# 🖥️ Hệ Thống Quản Lý Cửa Hàng Phân Phối Laptop

<div align="center">

![C#](https://img.shields.io/badge/C%23-.NET%206-239120?style=for-the-badge&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-WinForms-0078D4?style=for-the-badge&logo=windows&logoColor=white)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

**Tiểu luận môn Lập Trình Cơ Sở Dữ Liệu**

Sinh viên: **Nguyễn Yến Nhi** — MSSV: **DH23IM01**  
Giảng viên hướng dẫn: **ThS. Phạm Chí Công**

</div>

---

## 📋 Tổng Quan

Ứng dụng quản lý toàn bộ hoạt động của một **cửa hàng phân phối laptop**, bao gồm hai giao diện:
- **Windows Forms** — Dành cho nhân viên (quản lý nội bộ)
- **ASP.NET MVC Website** — Dành cho khách hàng (đặt hàng online)

### Tính Năng Chính

| Module | Mô tả |
|--------|-------|
| 📦 Sản phẩm | Quản lý serial, loại SP, hãng sản xuất, cấu hình |
| 🏭 Kho hàng | Phiếu nhập, nhà cung cấp, tồn kho theo serial |
| 🛒 Bán hàng | Đơn hàng lẻ, hợp đồng sỉ, giỏ hàng online |
| 🎁 Khuyến mãi | 4 chương trình KM (Back To School, Black Friday, ...) |
| 👥 Khách hàng | Khách lẻ, khách sỉ, doanh nghiệp |
| 🔧 Hậu mãi | Bảo hành, đổi trả, khiếu nại |
| 👤 Nhân sự | Quản lý nhân viên, tài khoản, phân quyền 5 vai trò |
| 📊 Báo cáo | Tồn kho, nhập hàng, doanh thu |

---

## 🏗️ Kiến Trúc N-Layer

```
GUI_WinForm  ──┐
               ├──▶  BUS  ──▶  DAL  ──▶  DTO  ──▶  SQL Server
Web_MVC      ──┘
```

| Project | Loại | Mô tả |
|---------|------|-------|
| `DTO_HTQLCuaHangLaptop` | Class Library | Data Transfer Objects — 22 bảng |
| `DAL_HTQLCuaHangLaptop` | Class Library | Data Access Layer — ADO.NET thuần |
| `BUS_HTQLCuaHangLaptop` | Class Library | Business Logic — validate, nghiệp vụ |
| `GUI_HTQLCuaHangLaptop` | Windows Forms | Giao diện nhân viên |
| `Website` | ASP.NET Core MVC | Giao diện khách hàng online |

> **Quy tắc phụ thuộc:** GUI và Web **tuyệt đối không** gọi DAL trực tiếp — mọi logic phải đi qua BUS.

---

## 🛠️ Công Nghệ Sử Dụng

| Thành phần | Công nghệ |
|-----------|-----------|
| Ngôn ngữ | C# (.NET 6+) |
| Giao diện nhân viên | Windows Forms |
| Giao diện khách hàng | ASP.NET Core MVC |
| Cơ sở dữ liệu | SQL Server (Collation: `Vietnamese_CI_AS`) |
| Truy xuất DB | ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataAdapter`) |
| Bảo mật | SHA-256 password hashing, `SqlParameter` (chống SQL Injection) |
| Session | ASP.NET Core Session (timeout 30 phút) |

---

## ⚙️ Yêu Cầu Hệ Thống

Trước khi cài đặt, hãy đảm bảo đã có:

- ✅ **Visual Studio 2022** (hoặc mới hơn)
- ✅ **.NET 6 SDK** trở lên
- ✅ **SQL Server** (Express / Developer / Standard Edition)
- ✅ **SQL Server Management Studio (SSMS)** để import database

---

## 🚀 Hướng Dẫn Cài Đặt

### Bước 1 — Clone Repository

```bash
git clone https://github.com/Nhiii-Nias/LTCSDL-QuanLyCuaHangLaptop.git
cd LTCSDL-QuanLyCuaHangLaptop
```

### Bước 2 — Khởi Tạo Database

1. Mở **SQL Server Management Studio (SSMS)**
2. Kết nối đến SQL Server instance của bạn
3. Mở file `QuanLyCuaHangLaptop.sql` trong SSMS
4. Nhấn **F5** (hoặc nút Execute) để chạy toàn bộ script
5. Kiểm tra database `QuanLyCuaHangLaptop` đã được tạo thành công

### Bước 3 — Cấu Hình Connection String

> ⚠️ Thay `.\SQLEXPRESS` bằng tên SQL Server instance thực tế trên máy bạn.  
> Ví dụ: `localhost`, `.\MSSQLSERVER`, `PCNAME\SQLEXPRESS`, ...

**WinForms** — Sửa file `GUI_HTQLCuaHangLaptop/App.config`:

```xml
<connectionStrings>
  <add name="QuanLyCuaHangLaptop"
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCuaHangLaptop;Integrated Security=True;TrustServerCertificate=True"
       providerName="Microsoft.Data.SqlClient" />
</connectionStrings>
```

**Website** — Sửa file `Website/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "QuanLyCuaHangLaptop": "Data Source=.\\SQLEXPRESS;Initial Catalog=QuanLyCuaHangLaptop;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

### Bước 4 — Build & Chạy

```
1. Mở file LTCSDL_HTQLCuaHangPhanPhoiLaptop.slnx trong Visual Studio
2. Nhấn Ctrl + Shift + B để build toàn bộ solution
3. Chọn Startup Project:
   - GUI_HTQLCuaHangLaptop  →  chạy ứng dụng WinForm (nhân viên)
   - Website                →  chạy website MVC (khách hàng)
4. Nhấn F5 để chạy
```

---

## 👥 Tài Khoản Mặc Định (Sau Khi Import Script)

| Vai trò | Quyền hạn |
|---------|----------|
| Quản trị hệ thống | Toàn quyền |
| Nhân viên bán hàng | Đơn hàng, hợp đồng, khuyến mãi |
| Nhân viên kho | Nhập/xuất kho, tồn kho, NCC |
| Nhân viên CSKH | Bảo hành, đổi trả, khiếu nại |
| Quản lý / Giám đốc | Chỉ xem báo cáo |

> 💡 Xem thông tin tài khoản mặc định trong file `QuanLyCuaHangLaptop.sql` phần `INSERT TaiKhoanNV`.

---

## 📁 Cấu Trúc Thư Mục

```
📦 LTCSDL_HTQLCuaHangPhanPhoiLaptop/
 ├── 📂 DTO_HTQLCuaHangLaptop/       # Data Transfer Objects (22 class)
 │    ├── DTO_SanPham.cs
 │    ├── DTO_KhachHang.cs
 │    └── ...
 ├── 📂 DAL_HTQLCuaHangLaptop/       # Data Access Layer — ADO.NET
 │    ├── DBConnect.cs
 │    ├── DAL_SanPham.cs
 │    └── ...
 ├── 📂 BUS_HTQLCuaHangLaptop/       # Business Logic Layer
 │    ├── BUS_SanPham.cs
 │    ├── BUS_KhuyenMai.cs
 │    └── ...
 ├── 📂 GUI_HTQLCuaHangLaptop/       # Windows Forms — giao diện nhân viên
 │    ├── FormDangNhap.cs
 │    ├── FormMain.cs
 │    ├── FormQuanLyDonHang.cs
 │    └── ...
 ├── 📂 Website/                     # ASP.NET Core MVC — giao diện KH
 │    ├── Controllers/
 │    ├── Models/
 │    ├── Views/
 │    └── wwwroot/
 ├── 📄 QuanLyCuaHangLaptop.sql      # Script khởi tạo database đầy đủ
 ├── 📄 LTCSDL_HTQLCuaHangPhanPhoiLaptop.slnx
 └── 📄 README.md
```

---

## 🗄️ Sơ Đồ Cơ Sở Dữ Liệu

**22 bảng** chia theo nhóm nghiệp vụ:

| Nhóm | Bảng |
|------|------|
| Tài khoản & Phân quyền | `VaiTro`, `TaiKhoanNV`, `TaiKhoanKH`, `LichSuDangNhap` |
| Nhân sự | `NhanVien` |
| Khách hàng | `KhachHang`, `KhachHangLe`, `KhachHangSi` |
| Sản phẩm | `HangSanXuat`, `LoaiSanPham`, `CauHinh`, `SanPham` |
| Kho hàng | `NhaCungCap`, `PhieuNhap`, `ChiTietPhieuNhap` |
| Bán hàng | `KhuyenMai`, `HopDong`, `DonHang`, `ChiTietDonHang` |
| Hậu mãi | `DonKhieuNai`, `PhieuBaoHanh`, `PhieuDoiTra` |

**Đặc điểm nổi bật:**
- `SanPham.MaSerialSP` — PRIMARY KEY dạng serial vật lý, mỗi máy có 1 serial duy nhất
- `ChiTietDonHang.MaSerialSP` — UNIQUE constraint, mỗi serial chỉ được bán 1 lần
- Xóa mềm (`IsDeleted = 1`) cho bảng master — không xóa vật lý dữ liệu lịch sử
- Collation `Vietnamese_CI_AS` — hỗ trợ đầy đủ tiếng Việt có dấu

---

## 🎁 Nghiệp Vụ Khuyến Mãi

4 chương trình khuyến mãi, không áp dụng đồng thời:

| Mã | Tên | Điều kiện | Ưu đãi |
|----|-----|-----------|--------|
| KM0001 | Back To School | HSSV, 15/08–15/09 | Laptop -10%, khác -15% |
| KM0002 | Black Friday | Tất cả, 25/11–30/11 | Laptop -5%, khác -15% |
| KM0003 | 10 Laptop 10% | DN, mua ≥10 laptop | -5% tổng hợp đồng |
| KM0004 | 30 Laptop 10% | DN, mua ≥30 laptop | -10% (hoặc -20% nếu thêm phụ kiện) |

---

## 📜 License

Dự án được xây dựng phục vụ mục đích **học thuật** — không sử dụng cho mục đích thương mại.

---

<div align="center">

*© 2026 — Nguyễn Yến Nhi — DH23IM01*  
*Môn: Lập Trình Cơ Sở Dữ Liệu — GVHD: ThS. Phạm Chí Công*

</div>

# 🖥️ Hệ Thống Quản Lý Cửa Hàng Phân Phối Laptop

<div align="center">

[![C#](https://img.shields.io/badge/C%23-.NET%206+-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-WinForms-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/winforms/)
[![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/mvc)

**Tiểu luận môn Lập Trình Cơ Sở Dữ Liệu**

👨‍🎓 Sinh viên: **Nguyễn Yến Nhi** 

</div>

---

## 📑 Mục Lục

1. [📋 Tổng Quan](#-tổng-quan)
2. [✨ Tính Năng Chính](#-tính-năng-chính)
3. [🏗️ Kiến Trúc Hệ Thống](#️-kiến-trúc-hệ-thống)
4. [🛠️ Công Nghệ Sử Dụng](#️-công-nghệ-sử-dụng)
5. [⚙️ Yêu Cầu Hệ Thống](#️-yêu-cầu-hệ-thống)
6. [🚀 Hướng Dẫn Cài Đặt](#-hướng-dẫn-cài-đặt)
7. [👥 Tài Khoản Mặc Định](#-tài-khoản-mặc-định)
8. [📁 Cấu Trúc Thư Mục](#-cấu-trúc-thư-mục)
9. [🗄️ Sơ Đồ Cơ Sở Dữ Liệu](#️-sơ-đồ-cơ-sở-dữ-liệu)
10. [🎁 Nghiệp Vụ Khuyến Mãi](#-nghiệp-vụ-khuyến-mãi)
11. [❓ Xử Lý Sự Cố](#-xử-lý-sự-cố)
12. [📝 Hướng Dẫn Đóng Góp](#-hướng-dẫn-đóng-góp)
13. [📜 License](#-license)

---

## 📋 Tổng Quan

Ứng dụng quản lý toàn bộ hoạt động của một **cửa hàng phân phối laptop**, bao gồm hai giao diện riêng biệt:

| Giao diện | Người dùng | Chức năng |
|-----------|-----------|----------|
| 🖥️ **Windows Forms** | Nhân viên | Quản lý nội bộ (kho, bán hàng, báo cáo) |
| 🌐 **ASP.NET MVC Website** | Khách hàng | Duyệt sản phẩm, đặt hàng online, theo dõi đơn |

**Điểm nổi bật:**
- ✅ Hỗ trợ **xuyên phương** quản lý: serial vật lý, loại, hãng sản xuất
- ✅ **Khuyến mãi thông minh** — tự động áp dụng theo nhóm khách hàng & thời gian
- ✅ **Hậu mãi toàn diện** — bảo hành, đổi trả, xử lý khiếu nại
- ✅ **Bảo mật cao** — mã hóa SHA-256, phân quyền 5 vai trò, chống SQL Injection

---

## ✨ Tính Năng Chính

### 📦 Quản Lý Sản Phẩm
- Quản lý **mã serial** vật lý (mỗi máy là 1 record duy nhất)
- Phân loại theo **hãng**, **dòng**, **cấu hình**
- Theo dõi **trạng thái** (có sẵn, đã bán, hỏng hóc, ...)

### 🏭 Quản Lý Kho Hàng
- Phiếu nhập từ **nhà cung cấp**
- Quản lý **tồn kho** theo serial
- Báo cáo **hàng sắp hết** & **hàng lỏi**

### 🛒 Bán Hàng & Đơn Hàng
- **Đơn lẻ** — bán rời lẻ cho khách hàng cá nhân
- **Hợp đồng sỉ** — bán buôn cho doanh nghiệp với điều khoản thanh toán
- **Giỏ hàng online** — khách tự đặt qua website

### 🎁 Khuyến Mãi Thông Minh
- 4 chương trình KM không trùng lặp
- Tự động áp dụng theo **nhóm KH** (HSSV, DN, ...)
- Tính toán **chiết khấu** theo sản phẩm

### 👥 Quản Lý Khách Hàng
- Phân loại: **Khách lẻ** | **Khách sỉ** | **Doanh nghiệp**
- Theo dõi **lịch sử giao dịch** và **nợ tiền**

### 🔧 Hậu Mãi & Dịch Vụ
- 📋 **Bảo hành** — theo dõi thời hạn & trạng thái
- 🔄 **Đổi trả** — yêu cầu & phê duyệt
- 📞 **Khiếu nại** — ghi nhận & xử lý

### 👤 Quản Lý Nhân Sự & Phân Quyền
- Quản lý **tài khoản nhân viên**
- Phân quyền **5 vai trò** (Quản trị, Bán hàng, Kho, CSKH, Quản lý)
- Lịch sử **đăng nhập**

### 📊 Báo Cáo & Thống Kê
- 📈 Doanh thu theo **kỳ, loại sản phẩm**
- 📦 Tồn kho và **hàng lỏi**
- 💰 Nợ tiền & **tình hình thanh toán**

---

## 🏗️ Kiến Trúc Hệ Thống

### Sơ Đồ Kiến Trúc N-Layer

```
┌──────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                │
├──────────────┬──────────────────┬────────────────────┤
│  Windows     │   ASP.NET Core   │   Mobile (nếu có)  │
│  Forms GUI   │   MVC Website    │                    │
└──────────┬───┴────────┬─────────┴────────────────┬───┘
           │            │                         │
           └────────────┼─────────────────────────┘
                        │
           ┌────────────▼─────────────┐
           │   BUSINESS LOGIC LAYER   │
           │   (BUS - Validation,     │
           │    Business Rules)       │
           └────────────┬─────────────┘
                        │
           ┌────────────▼─────────────┐
           │  DATA ACCESS LAYER       │
           │  (DAL - ADO.NET, SQL)    │
           └────────────┬─────────────┘
                        │
           ┌────────────▼─────────────┐
           │   DATA TRANSFER OBJECTS  │
           │   (DTO - Model Classes)  │
           └────────────┬─────────────┘
                        │
           ┌────────────▼─────────────┐
           │    DATABASE LAYER        │
           │  (SQL Server 2019+)      │
           └──────────────────────────┘
```

### Chi Tiết Các Tầng

| Project | Loại | Lớp | Mô Tả |
|---------|------|-----|-------|
| `DTO_HTQLCuaHangLaptop` | Class Library | Data Transfer Objects | 22 DTO class tương ứng 22 bảng DB |
| `DAL_HTQLCuaHangLaptop` | Class Library | Data Access | ADO.NET thuần (SqlConnection, SqlCommand) |
| `BUS_HTQLCuaHangLaptop` | Class Library | Business Logic | Validate input, kiểm tra business rules |
| `GUI_HTQLCuaHangLaptop` | Windows Forms | Presentation | Giao diện quản lý cho nhân viên |
| `Website` | ASP.NET Core MVC | Presentation | Website khách hàng & admin panel |

> ⚠️ **Quy tắc kiến trúc:** GUI/Web **KHÔNG** gọi DAL trực tiếp — tất cả logic phải qua BUS layer để đảm bảo tính nhất quán.

---

## 🛠️ Công Nghệ Sử Dụng

| Công Nghệ | Phiên Bản | Mục Đích |
|-----------|-----------|---------|
| **C#** | C# 10+ | Ngôn ngữ lập trình chính |
| **.NET** | 6.0 LTS trở lên | Framework nền tảng |
| **Windows Forms** | .NET 6+ | Giao diện desktop (WinForms) |
| **ASP.NET Core MVC** | 6.0+ | Framework web backend |
| **SQL Server** | 2019 Express+  | Hệ quản trị CSDL |
| **ADO.NET** | Tích hợp sẵn | Kết nối & truy vấn DB |
| **SHA-256** | Tích hợp sẵn | Mã hóa mật khẩu |
| **SqlParameter** | ADO.NET | Chống SQL Injection |
| **Session** | ASP.NET Core | Quản lý phiên (30 phút timeout) |

---

## ⚙️ Yêu Cầu Hệ Thống

### Phần Cứng (Tối Thiểu)
- CPU: 2 cores, 2 GHz
- RAM: 4 GB (8 GB khuyến nghị)
- Disk: 2 GB free space

### Phần Mềm (Bắt Buộc)
- ✅ **Windows** 10/11 hoặc **Server 2016+**
- ✅ **Visual Studio 2022** (Community/Professional/Enterprise)
- ✅ **.NET 6 SDK** trở lên ([tải tại đây](https://dotnet.microsoft.com/download))
- ✅ **SQL Server Express/Developer** 2019+
- ✅ **SQL Server Management Studio (SSMS)** 18.12+

### Kiểm Tra Cài Đặt

Mở **Command Prompt / PowerShell** và chạy:

```bash
# Kiểm tra phiên bản .NET
dotnet --version

# Kiểm tra Visual Studio
"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" --version
```

---

## 🚀 Hướng Dẫn Cài Đặt

### ⏱️ Thời gian dự kiến: **15-20 phút**

### **Bước 1️⃣ — Clone Repository**

```bash
git clone https://github.com/Nhiii-Nias/LTCSDL-QuanLyCuaHangLaptop.git
cd LTCSDL-QuanLyCuaHangLaptop
```

### **Bước 2️⃣ — Khởi Tạo Database**

1. Mở **SQL Server Management Studio (SSMS)**
2. Kết nối tới SQL Server của bạn
   - Server name: `.\SQLEXPRESS` (hoặc tên server thực tế)
   - Authentication: **Windows Authentication**
3. Nhấp chuột phải → **New Query** (Ctrl + N)
4. Mở file `QuanLyCuaHangLaptop.sql` từ repository
5. Nhấn **F5** hoặc nút **Execute** để chạy toàn bộ script
6. Đợi 2-3 giây, kiểm tra message "Completed successfully"

✅ Database `QuanLyCuaHangLaptop` đã tạo xong!

### **Bước 3️⃣ — Cấu Hình Connection String**

**Connection string mẫu:**
```
Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCuaHangLaptop;Integrated Security=True;TrustServerCertificate=True
```

**Thay đổi `.\SQLEXPRESS` thành:**
- `localhost` — máy cục bộ, SQL Server default
- `PCNAME\SQLEXPRESS` — tên máy + instance name (nếu có)
- `SERVER_IP,1433` — kết nối qua mạng

#### **Với Windows Forms** → Sửa `GUI_HTQLCuaHangLaptop/App.config`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <connectionStrings>
    <add name="QuanLyCuaHangLaptop"
         connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCuaHangLaptop;Integrated Security=True;TrustServerCertificate=True"
         providerName="Microsoft.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

#### **Với Website** → Sửa `Website/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "QuanLyCuaHangLaptop": "Data Source=.\\SQLEXPRESS;Initial Catalog=QuanLyCuaHangLaptop;Integrated Security=True;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

> 💡 **Tip:** Dùng **SSMS** → Connect → xem tên server chính xác ở đầu cửa sổ

### **Bước 4️⃣ — Build & Chạy**

1. Mở file `LTCSDL_HTQLCuaHangPhanPhoiLaptop.slnx` trong **Visual Studio 2022**

2. **Build Solution** → Ctrl + Shift + B (chờ 30-60 giây)

3. **Chọn Startup Project** trong Solution Explorer:
   - Nhấp chuột phải trên project
   - Chọn **"Set as Startup Project"**

4. **Chạy ứng dụng:**
   - 🖥️ **Windows Forms**: Chọn `GUI_HTQLCuaHangLaptop` → Nhấn F5
   - 🌐 **Website**: Chọn `Website` → Nhấn F5

5. ✅ Ứng dụng khởi động! Đăng nhập bằng tài khoản default (xem mục tiếp theo)

---

## 👥 Tài Khoản Mặc Định

Sau khi chạy script `QuanLyCuaHangLaptop.sql`, hệ thống tạo sẵn các tài khoản sau:

| Vai Trò | Tên Đăng Nhập | Mật Khẩu | Quyền Hạn |
|---------|--------------|---------|----------|
| 🔐 **Admin** | `admin` | `admin123` | Toàn quyền hệ thống |
| 💼 **Nhân viên Bán Hàng** | `nhanvienbh` | `bh123` | Đơn hàng, hợp đồng, KM |
| 📦 **Nhân viên Kho** | `nhanvienkho` | `kho123` | Nhập/xuất kho, NCC |
| 📞 **Nhân viên CSKH** | `nhanviencskh` | `cskh123` | Bảo hành, đổi trả, khiếu nại |
| 📊 **Quản Lý** | `quanly` | `ql123` | Xem báo cáo & thống kê |

> ⚠️ **Bảo mật:** Thay đổi mật khẩu ngay lần đăng nhập đầu tiên trong ứng dụng!

---

## 📁 Cấu Trúc Thư Mục

```
📦 LTCSDL_HTQLCuaHangPhanPhoiLaptop/
│
├── 📂 DTO_HTQLCuaHangLaptop/           # 📋 Data Transfer Objects
│   ├── DTO_SanPham.cs
│   ├── DTO_KhachHang.cs
│   ├── DTO_TaiKhoanNV.cs
│   ├── DTO_NhanVien.cs
│   ├── DTO_KhuyenMai.cs
│   ├── DTO_DonHang.cs
│   ├── DTO_PhieuNhap.cs
│   ├── DTO_HopDong.cs
│   └── ... (16 file DTO khác)
│
├── 📂 DAL_HTQLCuaHangLaptop/           # 🔌 Data Access Layer (ADO.NET)
│   ├── DBConnect.cs                    # Kết nối DB
│   ├── DAL_SanPham.cs                  # CRUD Sản phẩm
│   ├── DAL_KhachHang.cs
│   ├── DAL_KhuyenMai.cs
│   ├── DAL_DonHang.cs
│   ├── DAL_BaoHanh.cs
│   └── ... (các file DAL khác)
│
├── 📂 BUS_HTQLCuaHangLaptop/           # ⚙️ Business Logic Layer
│   ├── BUS_SanPham.cs                  # Validate, tính toán
│   ├── BUS_KhuyenMai.cs                # Logic KM thông minh
│   ├── BUS_DonHang.cs
│   ├── BUS_KhachHang.cs
│   ├── BUS_TaiKhoan.cs
│   ├── BUS_BaoCao.cs
│   └── ... (các file BUS khác)
│
├── 📂 GUI_HTQLCuaHangLaptop/           # 🖥️ Windows Forms (Giao diện nhân viên)
│   ├── FormDangNhap.cs                 # Màn hình đăng nhập
│   ├── FormMain.cs                     # Màn hình chính
│   ├── FormQuanLyDonHang.cs
│   ├── FormQuanLyKho.cs
│   ├── FormBaoCao.cs
│   ├── FormKhuyenMai.cs
│   ├── Resources/                      # Ảnh, icon
│   └── App.config                      # Connection string
│
├── 📂 Website/                         # 🌐 ASP.NET Core MVC (Giao diện khách)
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   ├── SanPhamController.cs
│   │   ├── GioHangController.cs
│   │   ├── DonHangController.cs
│   │   └── TaiKhoanController.cs
│   ├── Models/                         # View Models
│   ├── Views/                          # Razor Pages (.cshtml)
│   │   ├── Home/
│   │   ├── SanPham/
│   │   ├── GioHang/
│   │   └── Shared/
│   ├── wwwroot/                        # CSS, JS, hình ảnh
│   │   ├── css/
│   │   ├── js/
│   │   └── images/
│   ├── appsettings.json                # Cấu hình (connection string)
│   └── Program.cs                      # Startup
│
├── 📄 QuanLyCuaHangLaptop.sql          # 📊 SQL Script tạo DB
├── 📄 LTCSDL_HTQLCuaHangPhanPhoiLaptop.slnx  # Visual Studio Solution
├── 📄 README.md                        # Tài liệu này
├── 📄 AGENTS.md                        # Danh sách thành viên
├── 📄 GiaiThichCode_HTQLCuaHangLaptop.docx   # Giải thích code
├── 📄 GiaiThichCode_script.py          # Script generate doc
└── 📄 LTCSLD_YeuCauHeThong.docx        # Yêu cầu hệ thống

```

---

## 🗄️ Sơ Đồ Cơ Sở Dữ Liệu

Hệ thống sử dụng **22 bảng** được thiết kế theo **mô hình quan hệ chuẩn hóa bậc 3 (3NF)**:

### Nhóm 1: Tài Khoản & Phân Quyền (4 bảng)
```
VaiTro (1) ──── (nhiều) TaiKhoanNV
           ──── (nhiều) TaiKhoanKH
           ──── (nhiều) LichSuDangNhap
```
- **VaiTro**: Quản trị, Bán hàng, Kho, CSKH, Quản lý
- **TaiKhoanNV**: Tài khoản nhân viên (SHA-256 hashed)
- **TaiKhoanKH**: Tài khoản khách online
- **LichSuDangNhap**: Audit log

### Nhóm 2: Nhân Sự (1 bảng)
```
NhanVien (link với TaiKhoanNV)
```

### Nhóm 3: Khách Hàng (3 bảng)
```
KhachHang (1) ──── (nhiều) KhachHangLe
           ──── (nhiều) KhachHangSi
```

### Nhóm 4: Sản Phẩm (4 bảng)
```
HangSanXuat (1) ──── (nhiều) SanPham
LoaiSanPham (1) ──── (nhiều) SanPham
CauHinh (1) ──── (nhiều) SanPham
```
- **SanPham**: PRIMARY KEY = `MaSerialSP` (mã serial vật lý duy nhất)

### Nhóm 5: Kho Hàng (3 bảng)
```
NhaCungCap (1) ──── (nhiều) PhieuNhap
PhieuNhap (1) ──── (nhiều) ChiTietPhieuNhap
```

### Nhóm 6: Bán Hàng (4 bảng)
```
KhuyenMai (1) ──── (nhiều) DonHang
DonHang (1) ──── (nhiều) ChiTietDonHang
HopDong (1) ──── (nhiều) ChiTietDonHang
```
- **ChiTietDonHang.MaSerialSP**: UNIQUE (mỗi serial bán 1 lần)

### Nhóm 7: Hậu Mãi (3 bảng)
```
DonKhieuNai (tham chiếu DonHang)
PhieuBaoHanh (tham chiếu ChiTietDonHang)
PhieuDoiTra (tham chiếu ChiTietDonHang)
```

**Đặc điểm nổi bật:**
- ✅ Xóa mềm (`IsDeleted = 1`) — giữ lịch sử lâu dài
- ✅ Collation `Vietnamese_CI_AS` — hỗ trợ Tiếng Việt có dấu
- ✅ Foreign Key constraints — đảm bảo toàn vẹn dữ liệu
- ✅ Indexed fields — tối ưu truy vấn

---

## 🎁 Nghiệp Vụ Khuyến Mãi

Hệ thống hỗ trợ **4 chương trình khuyến mãi không trùng lặp**, tự động áp dụng dựa vào **ngày tháng** và **loại khách hàng**:

| Mã | Tên Chương Trình | Đối Tượng | Thời Gian | Chiết Khấu |
|---|---|---|---|---|
| KM0001 | **Back To School** | HSSV, Sinh viên | 15/08–15/09 | Laptop -10%, Phụ kiện -15% |
| KM0002 | **Black Friday** | Tất cả | 25/11–30/11 | Laptop -5%, Phụ kiện -15% |
| KM0003 | **Sỉ 10 Laptop** | Doanh nghiệp | Cả năm | Mua ≥10 laptop: Hợp đồng -5% |
| KM0004 | **Sỉ 30 Laptop** | Doanh nghiệp | Cả năm | Mua ≥30 laptop: -10% (hoặc -20% + phụ kiện) |

### Logic Tính Toán
1. Kiểm tra **ngày hiện tại** & **loại khách**
2. Xác định KM **phù hợp nhất**
3. Tính chiết khấu từng **line item**
4. Áp dụng vào **tổng hợp đơng hoặc hợp đồng**

---

## ❓ Xử Lý Sự Cố

### 🔴 Lỗi: Connection Failed — "Server name = .\SQLEXPRESS is not valid"

**Nguyên nhân:** Sai tên SQL Server instance  
**Cách khắc phục:**
1. Mở **SQL Server Management Studio**
2. Xem tên server ở ô **Server name** khi đăng nhập
3. Sao chép tên chính xác, thay vào `appsettings.json` hoặc `App.config`
4. Rebuild solution (Ctrl + Shift + B)

---

### 🔴 Lỗi: Database doesn't exist — "Cannot open database 'QuanLyCuaHangLaptop'"

**Nguyên nhân:** Script SQL chưa chạy hoặc chạy không thành công  
**Cách khắc phục:**
1. Mở **SSMS** → **File** → **Open** → Chọn `QuanLyCuaHangLaptop.sql`
2. Kiểm tra kết nối (Ctrl + Shift + C)
3. Chạy từng part nhỏ (Highlight → Ctrl + Shift + E)
4. Xem **Messages** tab ở dưới để tìm lỗi cụ thể

---

### 🔴 Lỗi: Visual Studio không tìm thấy .NET 6 SDK

**Nguyên nhân:** .NET SDK chưa cài hoặc cài sai version  
**Cách khắc phục:**
1. Tải **.NET 6 SDK** từ https://dotnet.microsoft.com/download
2. Cài đặt (chọn "Desktop Runtime" + "ASP.NET Runtime")
3. Restart Visual Studio
4. Chạy lệnh kiểm tra: `dotnet --version`

---

### 🔴 Lỗi: Unauthorized — "Login failed for user 'sa'"

**Nguyên nhân:** SQL Server Authentication bị khóa hoặc sa user sai mật khẩu  
**Cách khắc phục:**
1. Mở **SQL Server Configuration Manager**
2. Đảm bảo **SQL Server Browser** đang chạy
3. Kiểm tra **Authentication mode** = "Mixed Mode"
4. Reset mật khẩu `sa` nếu quên (cần quyền Admin)

---

### 🔴 Build Error: "The project file '...' cannot be opened."

**Nguyên nhân:** Tệp .csproj hoặc .slnx bị hỏng  
**Cách khắc phục:**
1. **Clean Solution** → Build → Rebuild
2. Xóa thư mục `bin` & `obj` ở tất cả projects
3. Xóa file `.vs` (hidden folder)
4. Đóng & mở lại Visual Studio

---

### 💡 Debug Tips
- **F5** — Chạy với debugger (chậm hơn)
- **Ctrl + F5** — Chạy không debugger (nhanh hơn)
- **Debug** → **Windows** → **Output** — xem log chi tiết
- **Debug** → **Break All** (Ctrl + Alt + Break) — tạm dừng khi lỗi

---

## 📝 Hướng Dẫn Đóng Góp

Bạn muốn cải tiến dự án? Chào mừng! Hãy làm theo các bước sau:

### 1️⃣ Fork & Clone
```bash
# Fork repo trên GitHub
# Sau đó clone fork của bạn
git clone https://github.com/YOUR_USERNAME/LTCSDL-QuanLyCuaHangLaptop.git
cd LTCSDL-QuanLyCuaHangLaptop
git remote add upstream https://github.com/Nhiii-Nias/LTCSDL-QuanLyCuaHangLaptop.git
```

### 2️⃣ Tạo Feature Branch
```bash
git checkout -b feature/ten-tinh-nang
# Ví dụ: feature/add-export-excel
```

### 3️⃣ Commit & Push
```bash
git add .
git commit -m "Add: Mô tả tính năng"
git push origin feature/ten-tinh-nang
```

### 4️⃣ Tạo Pull Request
- Vào GitHub → **Compare & pull request**
- Mô tả chi tiết thay đổi
- Chờ review từ maintainer

### 📋 Guideline Commit Message
```
[TYPE]: Brief description

Detailed explanation if needed
- Bullet point 1
- Bullet point 2

Fixes #ISSUE_NUMBER
```

**Types:**
- `Add` — Thêm tính năng mới
- `Fix` — Sửa bug
- `Refactor` — Cải thiện code
- `Docs` — Cập nhật tài liệu

---

## 📜 License

Dự án được xây dựng phục vụ mục đích **học thuật & nghiên cứu**.

📌 **Không được sử dụng cho mục đích thương mại hoặc phân phối mà không xin phép.**

---

## 👏 Ghi Nhận

Cảm ơn các thư viện & công nghệ mã nguồn mở:
- **.NET Foundation** — .NET & ASP.NET
- **Microsoft** — SQL Server, Visual Studio
- **Community** — Nhà phát triển và tester

---

## 📧 Liên Hệ

**Câu hỏi hoặc góp ý?**
- 👨‍💻 Nguyễn Yến Nhi — [GitHub](https://github.com/Nhiii-Nias)
- 📧 Email: nnynnias@gmail.com

---

<div align="center">

### © 2026 — Tiểu Luận LTCSDL — ĐH Mở

![GitHub last commit](https://img.shields.io/github/last-commit/Nhiii-Nias/LTCSDL-QuanLyCuaHangLaptop?style=flat-square)
![GitHub repo size](https://img.shields.io/github/repo-size/Nhiii-Nias/LTCSDL-QuanLyCuaHangLaptop?style=flat-square)
![GitHub followers](https://img.shields.io/github/followers/Nhiii-Nias?style=social)

⭐ **Nếu dự án hữu ích, hãy cho một sao!** ⭐

</div>

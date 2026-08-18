# AGENTS.md — Hệ Thống Quản Lý Cửa Hàng Phân Phối Máy Tính

> Tài liệu này là hướng dẫn bắt buộc cho AI agent (Antigravity / Copilot / Claude Code) khi làm việc trong repository này. Đọc toàn bộ trước khi thực hiện bất kỳ thay đổi nào.

---

## 1. Tổng Quan Dự Án

| Mục | Nội dung |
|-----|----------|
| **Tên đề tài** | Hệ Thống Quản Lý Cửa Hàng Phân Phối Máy Tính |
| **Môn học** | Lập Trình Cơ Sở Dữ Liệu |
| **Ngôn ngữ** | C# (.NET Framework / .NET 6+) |
| **Giao diện NV** | Windows Forms (WinForm) |
| **Giao diện KH** | ASP.NET MVC (Website) |
| **Cơ sở dữ liệu** | SQL Server — dùng chung 1 CSDL cho cả 2 giao diện |
| **Truy xuất dữ liệu** | ADO.NET (SqlConnection, SqlCommand, SqlDataAdapter) |
| **Kiến trúc** | N-Layer: DTO → DAL → BUS → GUI/Web |

---

## 2. Kiến Trúc N-Layer (Bắt Buộc)

Hệ thống chia thành **4 project riêng biệt** trong cùng một Solution. Luồng phụ thuộc **chỉ được đi một chiều**:

```
GUI_WinForm  ──┐
               ├──▶  BUS  ──▶  DAL  ──▶  DTO
Web_MVC      ──┘
```

### 2.1 Cấu Trúc Solution

```
LTCSDL_HTQLCuaHangPhanPhoiLaptop.sln
│
├── DTO_HTQLCuaHangLaptop/          ← Class Library — Data Transfer Objects
├── DAL_HTQLCuaHangLaptop/          ← Class Library — Data Access Layer
├── BUS_HTQLCuaHangLaptop/          ← Class Library — Business Logic Layer
├── GUI_HTQLCuaHangLaptop/          ← Windows Forms App — Giao diện nhân viên
└── Web_HTQLCuaHangLaptop/          ← ASP.NET MVC App — Giao diện khách hàng
```

### 2.2 Quy Tắc Phụ Thuộc (Cứng — Không Được Vi Phạm)

| Project | Được phép tham chiếu | Không được tham chiếu |
|---------|----------------------|----------------------|
| `DTO` | *(không ai)* | DAL, BUS, GUI, Web |
| `DAL` | `DTO` | BUS, GUI, Web |
| `BUS` | `DAL`, `DTO` | GUI, Web |
| `GUI_WinForm` | `BUS`, `DTO` | DAL trực tiếp |
| `Web_MVC` | `BUS`, `DTO` | DAL trực tiếp |

> **Lý do:** GUI và Web tuyệt đối không được gọi DAL trực tiếp. Mọi logic nghiệp vụ phải đi qua BUS.

---

## 3. Quy Ước Đặt Tên

### 3.1 File & Class

| Lớp | Prefix | Ví dụ |
|-----|--------|-------|
| DTO | `DTO_` | `DTO_SanPham.cs`, `DTO_KhachHang.cs` |
| DAL | `DAL_` | `DAL_SanPham.cs`, `DAL_DonHang.cs` |
| BUS | `BUS_` | `BUS_SanPham.cs`, `BUS_KhuyenMai.cs` |
| WinForm | `frm` | `frmDangNhap.cs`, `frmQuanLySanPham.cs` |
| MVC Controller | `Controller` suffix | `SanPhamController.cs` |
| MVC View | Tên action | `Index.cshtml`, `ChiTiet.cshtml` |

### 3.2 Method Trong DAL / BUS

Dùng tiếng Việt không dấu hoặc tiếng Anh nhất quán trong cùng một class (Ưu tiên dùng tiếng Việt không dấu):

```csharp
// DAL — thao tác thô với DB
GetAll()          // SELECT *
GetById(string id)
Insert(DTO_X obj)
Update(DTO_X obj)
Delete(string id)
GetByCondition(...)

// BUS — nghiệp vụ
LayDanhSach()
LayTheoMa(string ma)
Them(DTO_X obj)
Sua(DTO_X obj)
Xoa(string ma)
KiemTraHopLe(DTO_X obj)   // validate trước khi gọi DAL
```

### 3.3 Connection String

Đặt trong `App.config` (WinForm) và `Web.config` (MVC). **Không hardcode** trong class DAL.

```xml
<connectionStrings>
  <add name="HTQLCuaHangLaptopDB"
       providerName="System.Data.SqlClient"
       connectionString="Data Source=.\SQLEXPRESS;
         Initial Catalog=HTQLCuaHangLaptopDB;
         Integrated Security=True" />
</connectionStrings>
```

Class `DBConnect` trong DAL đọc connection string qua `ConfigurationManager`.

---

## 4. Cơ Sở Dữ Liệu

### 4.1 Tên CSDL & Collation

**Tên CSDL:** `QuanLyCuaHangLaptop`  
**Collation:** `Vietnamese_CI_AS` (hỗ trợ tiếng Việt) 
**File SQL khởi tạo:** `QuanLyCuaHangLaptop.sql`
### 4.2 Danh Sách Bảng (22 bảng)

| Nhóm | Bảng |
|------|------|
| Tài khoản & Phân quyền | `VaiTro`, `TaiKhoanNV`, `TaiKhoanKH`, `LichSuDangNhap` |
| Nhân sự | `NhanVien` |
| Khách hàng | `KhachHang`, `KhachHangLe`, `KhachHangSi` |
| Sản phẩm | `HangSanXuat`, `LoaiSanPham`, `CauHinh`, `SanPham` |
| Kho hàng | `NhaCungCap`, `PhieuNhap`, `ChiTietPhieuNhap` |
| Bán hàng | `KhuyenMai`, `HopDong`, `DonHang`, `ChiTietDonHang` |
| Hậu mãi | `DonKhieuNai`, `PhieuBaoHanh`, `PhieuDoiTra` |

### 4.3 Ràng Buộc Quan Trọng

- `SanPham.MaSerialSP` là VARCHAR(50), PRIMARY KEY — mỗi máy vật lý có 1 serial duy nhất.
- `ChiTietDonHang.MaSerialSP` có ràng buộc UNIQUE — mỗi serial chỉ được bán đúng 1 lần.
- `DonHang.MaKM` và `DonHang.MaHD` là nullable (NULL nếu không áp dụng).
- Các bảng giao dịch (`DonHang`, `PhieuNhap`, `HopDong`) dùng cột `TrangThai` thay cho xóa mềm.
- Các bảng master dùng cột `IsDeleted BIT` để xóa mềm (không xóa vật lý):
  - `NhanVien` — liên kết lịch sử với đơn hàng, phiếu nhập.
  - `KhachHang` — lịch sử mua hàng, bảo hành, đổi trả vẫn tồn tại.
  - `SanPham` — serial liên kết với đơn hàng, phiếu bảo hành.
  - `HangSanXuat` — liên kết với `LoaiSanPham` đang còn hoạt động.
  - `LoaiSanPham` — liên kết với `SanPham`, `ChiTietPhieuNhap`, `CauHinh`.
  - `NhaCungCap` — liên kết với `PhieuNhap` lịch sử.
- Các bảng tài khoản (`TaiKhoanNV`, `TaiKhoanKH`) không dùng `IsDeleted` — vô hiệu hóa bằng cột `TrangThai = 'Khóa'` đã có sẵn.

### 4.4 Audit Columns (Thêm Vào Mọi Bảng Nghiệp Vụ)

```sql
NgayTao       DATETIME DEFAULT GETDATE(),
NgayCapNhat   DATETIME,
NguoiTao      CHAR(10),   -- FK → TaiKhoanNV hoặc TaiKhoanKH
NguoiCapNhat  CHAR(10)
```

---

## 5. Lớp DTO

Mỗi bảng trong CSDL tương ứng với một class DTO. Class chỉ chứa **properties**, không chứa logic.

```csharp
// DTO_HTQLCuaHangLaptop/DTO_SanPham.cs
namespace DTO_HTQLCuaHangLaptop
{
    public class DTO_SanPham
    {
        public string MaSerialSP { get; set; }
        public string MaPhieuNhap { get; set; }
        public string MaLoaiSP { get; set; }
        public DateTime NgayNhap { get; set; }
        public DateTime? NgaySX { get; set; }
        public string TrangThai { get; set; }  // Trong Kho | Đã Bán | Bảo Hành | Lỗi | Đổi Trả
    }
}
```

**Quy tắc DTO:**
- Không import namespace của DAL hay BUS.
- Nullable (`?`) cho các cột NOT NULL = NO trong CSDL.
- Dùng `decimal` cho tiền tệ (không dùng `float` hay `double`).
- Dùng `DateTime?` cho date nullable.

---

## 6. Lớp DAL

### 6.1 Class DBConnect (Base)

```csharp
// DAL_HTQLCuaHangLaptop/DBConnect.cs
using System.Configuration;
using System.Data.SqlClient;

namespace DAL_HTQLCuaHangLaptop
{
    public class DBConnect
    {
        protected SqlConnection _conn;

        public DBConnect()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["HTQLCuaHangLaptopDB"].ConnectionString;
            _conn = new SqlConnection(connStr);
        }
    }
}
```

### 6.2 Cấu Trúc DAL Điển Hình

```csharp
// DAL_HTQLCuaHangLaptop/DAL_SanPham.cs
public class DAL_SanPham : DBConnect
{
    public DataTable GetAll() { ... }
    public DataTable GetByTrangThai(string trangThai) { ... }
    public DTO_SanPham GetById(string maSerial) { ... }
    public bool Insert(DTO_SanPham sp) { ... }
    public bool Update(DTO_SanPham sp) { ... }
    public bool SoftDelete(string maSerial) { ... }  // IsDeleted = 1
}
```

**Quy tắc DAL:**
- Luôn `Open()` trước và `Close()` trong `finally`.
- Dùng `SqlParameter` — **không** ghép chuỗi SQL trực tiếp (SQL Injection).
- Trả về `DataTable` cho danh sách, `DTO_X` cho đơn lẻ, `bool` cho CUD.
- Không chứa logic nghiệp vụ (validate, tính toán) — chỉ CRUD thuần.

---

## 7. Lớp BUS

```csharp
// BUS_HTQLCuaHangLaptop/BUS_SanPham.cs
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

public class BUS_SanPham
{
    private readonly DAL_SanPham _dal = new DAL_SanPham();

    public DataTable LayDanhSachTonKho()
    {
        return _dal.GetByTrangThai("Trong Kho");
    }

    public bool ThemSanPham(DTO_SanPham sp)
    {
        // Validate trước
        if (string.IsNullOrWhiteSpace(sp.MaSerialSP)) return false;
        if (string.IsNullOrWhiteSpace(sp.MaLoaiSP)) return false;
        return _dal.Insert(sp);
    }
}
```

**Quy tắc BUS:**
- Validate input trước khi gọi DAL.
- Đặt logic nghiệp vụ đặc thù tại đây (tính giảm giá, kiểm tra bảo hành, xử lý đổi trả).
- Không import `System.Data.SqlClient` — không có SQL trong BUS.

---

## 8. Nghiệp Vụ Đặc Thù Cần Chú Ý

### 8.1 Khuyến Mãi (4 chương trình — không áp dụng đồng thời)

| Mã | Tên | Điều kiện | Ưu đãi |
|----|-----|-----------|--------|
| KM0001 | Back To School | HSSV, 15/08–15/09 | Laptop -10%, khác -15% trên SP |
| KM0002 | Black Friday | Tất cả, 25/11–30/11 | Laptop -5%, khác -15% trên SP |
| KM0003 | 10 Laptop 10% | DN, mua ≥10 laptop | -5% tổng HĐ |
| KM0004 | 30 Laptop 10% | DN, mua ≥30 laptop | -10% tổng HĐ; thêm 10 chuột/bàn phím → -20% |

Logic chọn khuyến mãi nằm trong `BUS_KhuyenMai.TinhKhuyenMai(DTO_DonHang dh, List<DTO_ChiTietDonHang> chiTiet)`.

### 8.2 Bảo Hành

- **Khách lẻ:** bảo hành tại cửa hàng, tính từ ngày mua trên hóa đơn, thời hạn theo `LoaiSanPham.ThoiGianBaoHanh`.
- **Khách sỉ:** bảo hành tại hãng. Nếu ≥10 máy lỗi do NSX trong 30 ngày → cửa hàng đổi 1:1, thu hồi gửi NCC.

### 8.3 Đổi Trả

- Áp dụng tất cả sản phẩm, trong vòng 30 ngày kể từ ngày mua.
- Chỉ chấp nhận lỗi do nhà sản xuất.
- Logic: `BUS_PhieuDoiTra.KiemTraDieuKienDoiTra(string maSerial, DateTime ngayMua)`.

### 8.4 Phân Quyền (5 Vai Trò)

| Mã | Vai Trò | Phạm vi |
|----|---------|---------|
| VT001 | Quản trị hệ thống | Toàn quyền |
| VT002 | Nhân viên bán hàng | Đơn hàng, hợp đồng, khuyến mãi |
| VT003 | Nhân viên kho | Nhập/xuất, tồn kho, serial, NCC |
| VT004 | Nhân viên CSKH | Bảo hành, đổi trả, khiếu nại |
| VT005 | Quản lý/Giám đốc | Chỉ xem báo cáo |

Kiểm tra quyền trong WinForm tại sự kiện `Form_Load` và trước mỗi thao tác CUD:

```csharp
if (currentUser.MaVaiTro != "VT001" && currentUser.MaVaiTro != "VT002")
{
    MessageBox.Show("Bạn không có quyền thực hiện thao tác này.");
    return;
}
```

---

## 9. Giao Diện WinForm (GUI_HTQLCuaHangLaptop)

### 9.1 Form Chính

```
frmMain.cs             ← MDI Container chính
frmDangNhap.cs         ← Màn hình đăng nhập (mở đầu tiên)
frmDoiMatKhau.cs
```

### 9.2 Form Theo Module

```
Quan ly he thong:   frmNhanVien, frmTaiKhoanNV, frmPhanQuyen
Ban hang:           frmDonHang, frmHopDong, frmKhuyenMai
Kho hang:           frmPhieuNhap, frmTonKho, frmNhaCungCap, frmDoiTraNCC
Cham soc KH:        frmKhachHang, frmBaoHanh, frmDoiTraSP, frmKhieuNai
Danh muc SP:        frmHangSanXuat, frmLoaiSanPham, frmCauHinh, frmSerial
Bao cao:            frmBaoCaoTonKho, frmBaoCaoNhap, frmBaoCaoDoanhThu
```

### 9.3 Pattern Chuẩn Cho Form Quản Lý

Mỗi form quản lý danh sách gồm: `DataGridView` hiển thị danh sách + `Panel` nhập liệu + 4 nút (Thêm / Sửa / Xóa / Làm mới).

```csharp
public partial class frmLoaiSanPham : Form
{
    private BUS_LoaiSanPham _bus = new BUS_LoaiSanPham();

    private void Form_Load(object sender, EventArgs e) => LoadData();

    private void LoadData()
    {
        dgvData.DataSource = _bus.LayDanhSach();
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        var dto = ReadFormData();
        if (_bus.Them(dto)) { MessageBox.Show("Thêm thành công!"); LoadData(); }
    }
    // ...
}
```

---

## 10. Giao Diện Website MVC (Web_HTQLCuaHangLaptop)

### 10.1 Cấu Trúc Controllers

```
Controllers/
  HomeController.cs          ← Trang chủ, danh sách SP
  SanPhamController.cs       ← Xem SP, chi tiết SP
  TaiKhoanController.cs      ← Đăng ký, đăng nhập, đăng xuất KH
  DonHangController.cs       ← Đặt hàng online, xem trạng thái
  GioHangController.cs       ← Giỏ hàng (Session)
  BaoHanhController.cs       ← Yêu cầu bảo hành
```

### 10.2 Lưu Thông Tin Đăng Nhập

Dùng `Session`, không dùng cookie trực tiếp:

```csharp
Session["KhachHang"] = dto_kh;   // Lưu DTO_KhachHang sau đăng nhập
Session["GioHang"]   = listGioHang;
```

### 10.3 Truyền Dữ Liệu Controller → View

Ưu tiên dùng **strongly-typed Model** (truyền DTO hoặc ViewModel). Chỉ dùng `ViewBag` cho dữ liệu phụ (SelectList dropdown, thông báo flash).

```csharp
// Tốt
return View(listSanPham);   // View nhận IEnumerable<DTO_SanPham>

// Chỉ dùng ViewBag cho phụ
ViewBag.DanhMucList = new SelectList(danhMuc, "MaLoaiSP", "TenLoai");
```

---

## 11. Bảo Mật

- **Mật khẩu:** lưu dạng hash SHA-256 hoặc BCrypt — không lưu plaintext.
- **SQL Injection:** luôn dùng `SqlParameter`, không ghép chuỗi.
- **Session timeout:** website đặt 30 phút.
- **Ghi log đăng nhập:** mọi lần đăng nhập (thành công/thất bại) → ghi vào bảng `LichSuDangNhap`.
- **Xóa mềm:** không xóa vật lý bản ghi master — đặt `IsDeleted = 1`. Áp dụng cho: `KhachHang`, `NhanVien`, `SanPham`, `HangSanXuat`, `LoaiSanPham`, `NhaCungCap`.

---

## 12. Những Điều Agent Tuyệt Đối Không Được Làm

1. **Gọi DAL trực tiếp từ GUI hoặc Controller** — vi phạm kiến trúc N-Layer.
2. **Ghép chuỗi SQL** (`"SELECT ... WHERE Id = " + id`) — lỗ hổng SQL Injection.
3. **Để connection string hardcode** trong code C# thay vì `App.config`/`Web.config`.
4. **Thêm logic nghiệp vụ vào DAL** (validate, tính toán) — DAL chỉ làm CRUD thuần.
5. **Thêm truy vấn SQL vào BUS hoặc GUI** — SQL chỉ nằm trong DAL.
6. **Xóa vật lý** bản ghi master (`KhachHang`, `NhanVien`, `SanPham`, `HangSanXuat`, `LoaiSanPham`, `NhaCungCap`) — phải xóa mềm bằng `IsDeleted = 1`.
7. **Bỏ qua kiểm tra quyền** trước thao tác CUD trong WinForm.
8. **Lưu mật khẩu plaintext** vào CSDL.
9. **Tạo thêm project mới** ngoài cấu trúc 5 project đã định nghĩa mà không có sự đồng ý.
10. **Sửa file AGENTS.md** mà không có yêu cầu rõ ràng từ người dùng.

---

## 13. Thứ Tự Xây Dựng Được Khuyến Nghị

```
Bước 1: Tạo CSDL SQL Server (script CREATE TABLE đầy đủ) - đã có file: QuanLyCuaHangLaptop.sql
Bước 2: Tạo Solution với 5 project, thiết lập References
Bước 3: Xây dựng toàn bộ DTO (1 class/bảng)
Bước 4: Xây dựng DBConnect + DAL cho từng bảng
Bước 5: Xây dựng BUS (bắt đầu từ module đơn giản: HangSanXuat, LoaiSanPham)
Bước 6: WinForm — frmDangNhap trước, sau đó từng module theo BPC
Bước 7: Website MVC — HomeController + SanPhamController trước
Bước 8: Tích hợp, kiểm thử từng luồng nghiệp vụ
```

---

*Cập nhật lần cuối: 2026 — Nguyễn Yến Nhi – DH23IM01 – GVHD: ThS. Phạm Chí Công*

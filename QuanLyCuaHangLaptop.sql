-- ============================================================
-- HỆ THỐNG QUẢN LÝ CỬA HÀNG PHÂN PHỐI MÁY TÍNH
-- Môn học: Lập trình Cơ sở dữ liệu
-- Sinh viên: Nguyễn Yến Nhi – 2354050086
-- ============================================================

-- ============================================================
-- Tài khoản admin để test: admin.nhi (Mật khẩu: admin)
-- ============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyCuaHangLaptop')
    DROP DATABASE QuanLyCuaHangLaptop;
GO
 --Tạo CSDL cấu hình tiếng Việt chuẩn, tìm kiếm không phân biệt hoa/thường nhưng phân biệt rõ ràng các từ có dấu khác nhau.
CREATE DATABASE QuanLyCuaHangLaptop
    COLLATE Vietnamese_CI_AS;
GO

USE QuanLyCuaHangLaptop;
GO

-- ============================================================
-- PHẦN 1: TẠO CÁC BẢNG
-- ============================================================

-- ----------------------------------------------------------
-- 1. VaiTro
-- ----------------------------------------------------------
CREATE TABLE VaiTro (
    MaVaiTro   CHAR(10)       NOT NULL,
    TenVaiTro  NVARCHAR(50)   NOT NULL,
    MoTaQuyen  NVARCHAR(500)  NULL,
    CONSTRAINT PK_VaiTro PRIMARY KEY (MaVaiTro)
);

-- ----------------------------------------------------------
-- 2. NhanVien
-- ----------------------------------------------------------
CREATE TABLE NhanVien (
    MaNV        CHAR(10)        NOT NULL,
    TenNV       NVARCHAR(50)    NOT NULL,
    GioiTinh    NVARCHAR(10)    NULL,
    SinhNhat    DATE            NOT NULL,
    SDT         VARCHAR(10)     NOT NULL,
    DiaChi      NVARCHAR(300)   NOT NULL,
    Email       VARCHAR(100)    NULL,
    NgayVaoLam  DATE            NOT NULL,
    Luong       DECIMAL(15,2)   NOT NULL,
    ChucVu      NVARCHAR(100)   NOT NULL,
    NgayTao     DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME        NULL,
    NguoiTao    CHAR(10)        NULL,
    NguoiCapNhat CHAR(10)       NULL,
    IsDeleted   BIT             NOT NULL DEFAULT 0,
    CONSTRAINT PK_NhanVien PRIMARY KEY (MaNV),
    CONSTRAINT CK_NhanVien_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ') OR GioiTinh IS NULL),
    CONSTRAINT CK_NhanVien_Luong    CHECK (Luong >= 0),
    CONSTRAINT CK_NhanVien_SDT      CHECK (SDT NOT LIKE '%[^0-9]%')
);

-- ----------------------------------------------------------
-- 3. TaiKhoanNV
-- ----------------------------------------------------------
CREATE TABLE TaiKhoanNV (
    MaTK         CHAR(10)      NOT NULL,
    MaNV         CHAR(10)      NOT NULL,
    MaVaiTro     CHAR(10)      NOT NULL,
    TenDangNhap  VARCHAR(50)   NOT NULL,
    MatKhau      VARCHAR(255)  NOT NULL,
    TrangThai    NVARCHAR(20)  NOT NULL,
    NgayTao      DATETIME      NOT NULL DEFAULT GETDATE(),
    NgayCapNhat  DATETIME      NULL,
    CONSTRAINT PK_TaiKhoanNV        PRIMARY KEY (MaTK),
    CONSTRAINT FK_TaiKhoanNV_NV     FOREIGN KEY (MaNV)     REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_TaiKhoanNV_VT     FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
    CONSTRAINT UQ_TaiKhoanNV_MaNV   UNIQUE (MaNV),
    CONSTRAINT UQ_TaiKhoanNV_Login  UNIQUE (TenDangNhap),
    CONSTRAINT CK_TaiKhoanNV_TT     CHECK (TrangThai IN (N'Hoạt Động', N'Khóa'))
);

-- ----------------------------------------------------------
-- 4. KhachHang
-- ----------------------------------------------------------
CREATE TABLE KhachHang (
    MaKH        CHAR(10)      NOT NULL,
    TenKH       NVARCHAR(50)  NOT NULL,
    Email       VARCHAR(100)  NULL,
    SDT         VARCHAR(10)   NULL,
    DiaChi      NVARCHAR(200) NULL,
    LoaiKH      NVARCHAR(10)  NOT NULL,
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME      NULL,
    NguoiTao    CHAR(10)      NULL,
    IsDeleted   BIT           NOT NULL DEFAULT 0,
    CONSTRAINT PK_KhachHang    PRIMARY KEY (MaKH),
    CONSTRAINT CK_KhachHang_LK CHECK (LoaiKH IN (N'Lẻ', N'Sỉ')),
    CONSTRAINT CK_KhachHang_SDT CHECK (SDT NOT LIKE '%[^0-9]%' OR SDT IS NULL)
);

-- ----------------------------------------------------------
-- 5. KhachHangLe
-- ----------------------------------------------------------
CREATE TABLE KhachHangLe (
    MaKHLe   CHAR(10) NOT NULL,
    LaHSSV   BIT      NOT NULL,
    SinhNhat DATE     NULL,
    CONSTRAINT PK_KhachHangLe   PRIMARY KEY (MaKHLe),
    CONSTRAINT FK_KhachHangLe   FOREIGN KEY (MaKHLe) REFERENCES KhachHang(MaKH),
    CONSTRAINT CK_KhachHangLe_HSSV CHECK (LaHSSV IN (0, 1))
);

-- ----------------------------------------------------------
-- 6. KhachHangSi
-- ----------------------------------------------------------
CREATE TABLE KhachHangSi (
    MaKHSi CHAR(10) NOT NULL,
    CONSTRAINT PK_KhachHangSi PRIMARY KEY (MaKHSi),
    CONSTRAINT FK_KhachHangSi  FOREIGN KEY (MaKHSi) REFERENCES KhachHang(MaKH)
);

-- ----------------------------------------------------------
-- 7. TaiKhoanKH
-- ----------------------------------------------------------
CREATE TABLE TaiKhoanKH (
    MaTK        CHAR(10)      NOT NULL,
    MaKH        CHAR(10)      NOT NULL,
    TenDangNhap VARCHAR(50)   NOT NULL,
    MatKhau     VARCHAR(255)  NOT NULL,
    NgayTao     DATE          NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    TrangThai   NVARCHAR(20)  NOT NULL,
    CONSTRAINT PK_TaiKhoanKH       PRIMARY KEY (MaTK),
    CONSTRAINT FK_TaiKhoanKH_KH    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    CONSTRAINT UQ_TaiKhoanKH_MaKH  UNIQUE (MaKH),
    CONSTRAINT UQ_TaiKhoanKH_Login UNIQUE (TenDangNhap),
    CONSTRAINT CK_TaiKhoanKH_TT    CHECK (TrangThai IN (N'Hoạt Động', N'Khóa'))
);

-- ----------------------------------------------------------
-- 8. LichSuDangNhap
-- ----------------------------------------------------------
CREATE TABLE LichSuDangNhap (
    MaLSDN    CHAR(10)      NOT NULL,
    MaTK      CHAR(10)      NOT NULL,  -- Chỉ dành cho TaiKhoanNV
    ThoiGian  DATETIME      NOT NULL DEFAULT GETDATE(),
    DiaChiIP  VARCHAR(45)   NULL,
    TrangThai NVARCHAR(20)  NOT NULL,
    CONSTRAINT PK_LichSuDangNhap    PRIMARY KEY (MaLSDN),
    CONSTRAINT FK_LSDN_TaiKhoanNV   FOREIGN KEY (MaTK)
        REFERENCES TaiKhoanNV(MaTK),
    CONSTRAINT CK_LichSuDangNhap_TT CHECK (TrangThai IN (N'Thành Công', N'Thất Bại'))
);

-- ----------------------------------------------------------
-- 9. HangSanXuat
-- ----------------------------------------------------------
CREATE TABLE HangSanXuat (
    MaHang      CHAR(10)       NOT NULL,
    TenHang     NVARCHAR(100)  NOT NULL,
    QuocGia     NVARCHAR(100)  NULL,
    NgayTao     DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME       NULL,
    NguoiTao    CHAR(10)       NULL,
    IsDeleted   BIT            NOT NULL DEFAULT 0,
    CONSTRAINT PK_HangSanXuat    PRIMARY KEY (MaHang),
    CONSTRAINT UQ_HangSanXuat_TH UNIQUE (TenHang)
);

-- ----------------------------------------------------------
-- 10. LoaiSanPham
-- ----------------------------------------------------------
CREATE TABLE LoaiSanPham (
    MaLoaiSP         CHAR(10)       NOT NULL,
    MaHang           CHAR(10)       NOT NULL,
    TenLoai          NVARCHAR(200)  NOT NULL,
    DanhMuc          NVARCHAR(50)   NOT NULL,
    ThoiGianBaoHanh  INT            NOT NULL,
    GiaBanGoc        DECIMAL(15,2)  NOT NULL,
    NgayTao          DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat      DATETIME       NULL,
    NguoiTao         CHAR(10)       NULL,
    IsDeleted        BIT            NOT NULL DEFAULT 0,
    CONSTRAINT PK_LoaiSanPham     PRIMARY KEY (MaLoaiSP),
    CONSTRAINT FK_LoaiSanPham_HSX FOREIGN KEY (MaHang) REFERENCES HangSanXuat(MaHang),
    CONSTRAINT CK_LoaiSanPham_DM  CHECK (DanhMuc IN (N'Laptop', N'Chuột', N'Bàn Phím')),
    CONSTRAINT CK_LoaiSanPham_BH  CHECK (ThoiGianBaoHanh > 0),
    CONSTRAINT CK_LoaiSanPham_Gia CHECK (GiaBanGoc >= 0)
);

-- ----------------------------------------------------------
-- 11. CauHinh
-- ----------------------------------------------------------
CREATE TABLE CauHinh (
    MaCauHinh     CHAR(10)       NOT NULL,
    MaLoaiSP      CHAR(10)       NOT NULL,
    TenThuocTinh  NVARCHAR(150)  NOT NULL,
    CONSTRAINT PK_CauHinh      PRIMARY KEY (MaCauHinh),
    CONSTRAINT FK_CauHinh_LLSP FOREIGN KEY (MaLoaiSP) REFERENCES LoaiSanPham(MaLoaiSP)
);

-- ----------------------------------------------------------
-- 12. NhaCungCap
-- ----------------------------------------------------------
CREATE TABLE NhaCungCap (
    MaNCC       CHAR(10)       NOT NULL,
    TenNCC      NVARCHAR(200)  NOT NULL,
    Email       VARCHAR(150)   NULL,
    SDT         VARCHAR(10)    NULL,
    DiaChi      NVARCHAR(300)  NULL,
    NgayTao     DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME       NULL,
    NguoiTao    CHAR(10)       NULL,
    IsDeleted   BIT            NOT NULL DEFAULT 0,
    CONSTRAINT PK_NhaCungCap     PRIMARY KEY (MaNCC),
    CONSTRAINT CK_NhaCungCap_SDT CHECK (SDT NOT LIKE '%[^0-9]%' OR SDT IS NULL)
);

-- ----------------------------------------------------------
-- 13. PhieuNhap
-- ----------------------------------------------------------
CREATE TABLE PhieuNhap (
    MaPhieuNhap  CHAR(10)       NOT NULL,
    MaNV         CHAR(10)       NOT NULL,
    MaNCC        CHAR(10)       NOT NULL,
    NgayNhap     DATE           NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    TongTien     DECIMAL(15,2)  NOT NULL,
    TrangThai    NVARCHAR(50)   NOT NULL,
    NgayTao      DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat  DATETIME       NULL,
    NguoiTao     CHAR(10)       NULL,
    CONSTRAINT PK_PhieuNhap       PRIMARY KEY (MaPhieuNhap),
    CONSTRAINT FK_PhieuNhap_NV    FOREIGN KEY (MaNV)  REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_PhieuNhap_NCC   FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT CK_PhieuNhap_TT    CHECK (TrangThai IN (N'Chờ Xác Nhận', N'Đã Nhập', N'Huỷ')),
    CONSTRAINT CK_PhieuNhap_Tien  CHECK (TongTien >= 0)
);

-- ----------------------------------------------------------
-- 14. SanPham  (phụ thuộc PhieuNhap & LoaiSanPham)
-- ----------------------------------------------------------
CREATE TABLE SanPham (
    MaSerialSP   VARCHAR(50)   NOT NULL,
    MaPhieuNhap  CHAR(10)      NOT NULL,
    MaLoaiSP     CHAR(10)      NOT NULL,
    NgayNhap     DATE          NOT NULL,
    NgaySX       DATE          NULL,
    TrangThai    NVARCHAR(50)  NOT NULL,
    NgayTao      DATETIME      NOT NULL DEFAULT GETDATE(),
    NgayCapNhat  DATETIME      NULL,
    IsDeleted    BIT           NOT NULL DEFAULT 0,
    CONSTRAINT PK_SanPham       PRIMARY KEY (MaSerialSP),
    CONSTRAINT FK_SanPham_PN    FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT FK_SanPham_LLSP  FOREIGN KEY (MaLoaiSP)    REFERENCES LoaiSanPham(MaLoaiSP),
    CONSTRAINT CK_SanPham_TT    CHECK (TrangThai IN (N'Trong Kho', N'Đã Bán', N'Bảo Hành', N'Lỗi', N'Đổi Trả'))
);

-- ----------------------------------------------------------
-- 15. ChiTietPhieuNhap
-- ----------------------------------------------------------
CREATE TABLE ChiTietPhieuNhap (
    MaLoaiSP    CHAR(10)       NOT NULL,
    MaPhieuNhap CHAR(10)       NOT NULL,
    SoLuong     INT            NOT NULL,
    GiaNhap     DECIMAL(15,2)  NOT NULL,
    CONSTRAINT PK_ChiTietPhieuNhap      PRIMARY KEY (MaLoaiSP, MaPhieuNhap),
    CONSTRAINT FK_CTPN_LoaiSP           FOREIGN KEY (MaLoaiSP)    REFERENCES LoaiSanPham(MaLoaiSP),
    CONSTRAINT FK_CTPN_PhieuNhap        FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT CK_CTPN_SoLuong          CHECK (SoLuong > 0),
    CONSTRAINT CK_CTPN_GiaNhap          CHECK (GiaNhap >= 0)
);

-- ----------------------------------------------------------
-- 16. KhuyenMai
-- ----------------------------------------------------------
CREATE TABLE KhuyenMai (
    MaKM         CHAR(10)       NOT NULL,
    TenKM        NVARCHAR(200)  NOT NULL,
    DoiTuong     NVARCHAR(100)  NOT NULL,
    DieuKien     NVARCHAR(500)  NULL,
    NgayBatDau   DATE           NOT NULL,
    NgayKetThuc  DATE           NOT NULL,
    MoTa         NVARCHAR(500)  NULL,
    MucGiamSP    DECIMAL(5,2)   NULL,
    MucGiamDH    DECIMAL(5,2)   NULL,
    SLToiThieu   INT            NULL,
    NgayTao      DATETIME       NOT NULL DEFAULT GETDATE(),
    isHienThi    BIT            NOT NULL DEFAULT 1,
    CONSTRAINT PK_KhuyenMai      PRIMARY KEY (MaKM),
    CONSTRAINT CK_KM_DoiTuong    CHECK (DoiTuong IN (N'Tất Cả', N'HSSV', N'Doanh Nghiệp')),
    CONSTRAINT CK_KM_NgayHetHan  CHECK (NgayKetThuc >= NgayBatDau)
);

-- ----------------------------------------------------------
-- 17. HopDong
-- ----------------------------------------------------------
CREATE TABLE HopDong (
    MaHD         CHAR(10)       NOT NULL,
    MaNV         CHAR(10)       NOT NULL,
    MaKH         CHAR(10)       NOT NULL,
    NgayKy       DATE           NOT NULL,
    GiaTriHD     DECIMAL(15,2)  NOT NULL,
    NgayHieuLuc  DATE           NOT NULL,
    NgayHetHan   DATE           NOT NULL,
    TrangThai    NVARCHAR(50)   NOT NULL,
    NgayTao      DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat  DATETIME       NULL,
    NguoiTao     CHAR(10)       NULL,
    CONSTRAINT PK_HopDong        PRIMARY KEY (MaHD),
    CONSTRAINT FK_HopDong_NV     FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_HopDong_KH     FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    CONSTRAINT CK_HopDong_TT     CHECK (TrangThai IN (N'Hiệu Lực', N'Hết Hạn', N'Huỷ')),
    CONSTRAINT CK_HopDong_GiaTri CHECK (GiaTriHD >= 0),
    CONSTRAINT CK_HopDong_NgayHH CHECK (NgayHetHan > NgayHieuLuc)
);

-- ----------------------------------------------------------
-- 18. DonHang
-- ----------------------------------------------------------
CREATE TABLE DonHang (
    MaDH                   CHAR(10)       NOT NULL,
    MaNV                   CHAR(10)       NOT NULL,
    MaKH                   CHAR(10)       NOT NULL,
    MaKM                   CHAR(10)       NULL,
    MaHD                   CHAR(10)       NULL,
    NgayDat                DATETIME       NOT NULL DEFAULT GETDATE(),
    TongTien               DECIMAL(15,2)  NOT NULL,
    TienSauGiam            DECIMAL(15,2)  NULL,
    PhuongThucThanhToan    NVARCHAR(100)  NOT NULL,
    TrangThai              NVARCHAR(50)   NOT NULL,
    NgayTao                DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat            DATETIME       NULL,
    NguoiTao               CHAR(10)       NULL,
    CONSTRAINT PK_DonHang        PRIMARY KEY (MaDH),
    CONSTRAINT FK_DonHang_NV     FOREIGN KEY (MaNV)  REFERENCES NhanVien(MaNV),
    CONSTRAINT FK_DonHang_KH     FOREIGN KEY (MaKH)  REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_DonHang_KM     FOREIGN KEY (MaKM)  REFERENCES KhuyenMai(MaKM),
    CONSTRAINT FK_DonHang_HD     FOREIGN KEY (MaHD)  REFERENCES HopDong(MaHD),
    CONSTRAINT CK_DonHang_TT     CHECK (TrangThai IN (N'Chờ Xử Lý', N'Đang Giao', N'Hoàn Thành', N'Huỷ')),
    CONSTRAINT CK_DonHang_PTTT   CHECK (PhuongThucThanhToan IN (N'Tiền Mặt', N'Chuyển Khoản', N'Thẻ')),
    CONSTRAINT CK_DonHang_TongTien CHECK (TongTien >= 0),
    CONSTRAINT CK_DonHang_TienSG  CHECK (TienSauGiam >= 0 OR TienSauGiam IS NULL)
);

-- ----------------------------------------------------------
-- 19. ChiTietDonHang
-- ----------------------------------------------------------
CREATE TABLE ChiTietDonHang (
    MaDH          CHAR(10)       NOT NULL,
    MaSerialSP    VARCHAR(50)    NOT NULL,
    GiaBan        DECIMAL(15,2)  NOT NULL,
    PhanTramGiam  DECIMAL(5,2)   NULL,
    CONSTRAINT PK_ChiTietDonHang     PRIMARY KEY (MaDH, MaSerialSP),
    CONSTRAINT FK_CTDH_DonHang       FOREIGN KEY (MaDH)       REFERENCES DonHang(MaDH),
    CONSTRAINT FK_CTDH_SanPham       FOREIGN KEY (MaSerialSP) REFERENCES SanPham(MaSerialSP),
    CONSTRAINT UQ_CTDH_Serial        UNIQUE (MaSerialSP),
    CONSTRAINT CK_CTDH_GiaBan        CHECK (GiaBan >= 0),
    CONSTRAINT CK_CTDH_PhanTramGiam  CHECK (PhanTramGiam >= 0 AND PhanTramGiam <= 100 OR PhanTramGiam IS NULL)
);

-- ----------------------------------------------------------
-- 20. DonKhieuNai
-- ----------------------------------------------------------
CREATE TABLE DonKhieuNai (
    MaDonKN   CHAR(10)        NOT NULL,
    MaDH      CHAR(10)        NOT NULL,
    MaKH      CHAR(10)        NOT NULL,
    NoiDung   NVARCHAR(1000)  NOT NULL,
    NgayGui   DATE            NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    TrangThai NVARCHAR(50)    NOT NULL,
    KetQua    NVARCHAR(500)   NULL,
    NgayTao   DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_DonKhieuNai     PRIMARY KEY (MaDonKN),
    CONSTRAINT FK_DKN_DonHang     FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    CONSTRAINT FK_DKN_KhachHang   FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    CONSTRAINT CK_DKN_TrangThai   CHECK (TrangThai IN (N'Đang Xử Lý', N'Đã Giải Quyết', N'Từ Chối'))
);

-- ----------------------------------------------------------
-- 21. PhieuBaoHanh
-- ----------------------------------------------------------
CREATE TABLE PhieuBaoHanh (
    MaPBH        CHAR(10)      NOT NULL,
    MaDH         CHAR(10)      NOT NULL,
    MaKH         CHAR(10)      NOT NULL,
    MaSerialSP   VARCHAR(50)   NOT NULL,
    LoaiBH       NVARCHAR(50)  NOT NULL,
    TrangThai    NVARCHAR(50)  NOT NULL,
    NgayBatDau   DATE          NOT NULL,
    NgayKetThuc  DATE          NOT NULL,
    LyDoLoi      NVARCHAR(500) NULL,
    KetQua       NVARCHAR(500) NULL,
    NgayTao      DATETIME      NOT NULL DEFAULT GETDATE(),
    NgayCapNhat  DATETIME      NULL,
    CONSTRAINT PK_PhieuBaoHanh     PRIMARY KEY (MaPBH),
    CONSTRAINT FK_PBH_DonHang      FOREIGN KEY (MaDH)       REFERENCES DonHang(MaDH),
    CONSTRAINT FK_PBH_KhachHang    FOREIGN KEY (MaKH)       REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_PBH_SanPham      FOREIGN KEY (MaSerialSP) REFERENCES SanPham(MaSerialSP),
    CONSTRAINT CK_PBH_LoaiBH       CHECK (LoaiBH    IN (N'Cửa Hàng', N'Hãng')),
    CONSTRAINT CK_PBH_TrangThai    CHECK (TrangThai IN (N'Đang Xử Lý', N'Hoàn Thành', N'Từ Chối')),
    CONSTRAINT CK_PBH_NgayKetThuc  CHECK (NgayKetThuc > NgayBatDau)
);

-- ----------------------------------------------------------
-- 22. PhieuDoiTra
-- ----------------------------------------------------------
CREATE TABLE PhieuDoiTra (
    MaPhieuDT  CHAR(10)       NOT NULL,
    MaDH       CHAR(10)       NOT NULL,
    MaSerialSP VARCHAR(50)    NOT NULL,
    MaKH       CHAR(10)       NOT NULL,
    NgayYeuCau DATE           NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    LyDo       NVARCHAR(500)  NOT NULL,
    LoaiXuLy   NVARCHAR(50)   NOT NULL,
    TrangThai  NVARCHAR(50)   NOT NULL,
    NgayTao    DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME      NULL,
    CONSTRAINT PK_PhieuDoiTra     PRIMARY KEY (MaPhieuDT),
    CONSTRAINT FK_PDT_DonHang     FOREIGN KEY (MaDH)       REFERENCES DonHang(MaDH),
    CONSTRAINT FK_PDT_SanPham     FOREIGN KEY (MaSerialSP) REFERENCES SanPham(MaSerialSP),
    CONSTRAINT FK_PDT_KhachHang   FOREIGN KEY (MaKH)       REFERENCES KhachHang(MaKH),
    CONSTRAINT UQ_PDT_Serial      UNIQUE (MaSerialSP),
    CONSTRAINT CK_PDT_LoaiXuLy   CHECK (LoaiXuLy  IN (N'Đổi Máy', N'Hoàn Tiền', N'Từ Chối')),
    CONSTRAINT CK_PDT_TrangThai   CHECK (TrangThai IN (N'Đang Xử Lý', N'Hoàn Thành', N'Từ Chối'))
);

GO

-- ----------------------------------------------------------
-- Triggers Validation cho PhieuBaoHanh & PhieuDoiTra
-- ----------------------------------------------------------
CREATE TRIGGER trg_PhieuBaoHanh_Validation
ON PhieuBaoHanh
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Sản phẩm chỉ có thể bảo hành nếu đơn hàng đã ở trạng thái 'Hoàn Thành'
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN DonHang dh ON i.MaDH = dh.MaDH
        WHERE dh.TrangThai <> N'Hoàn Thành'
    )
    BEGIN
        RAISERROR (N'Lỗi: Sản phẩm chỉ có thể bảo hành nếu đơn hàng đã ở trạng thái Hoàn Thành.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Nếu sản phẩm đang có phiếu đổi trả trong trạng thái đang xử lý thì không có phiếu bảo hành
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN PhieuDoiTra pdt ON i.MaSerialSP = pdt.MaSerialSP
        WHERE i.TrangThai = N'Đang Xử Lý' AND pdt.TrangThai = N'Đang Xử Lý'
    )
    BEGIN
        RAISERROR (N'Lỗi: Sản phẩm đang có phiếu đổi trả ở trạng thái Đang Xử Lý, không thể tạo/sửa phiếu bảo hành.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END;
GO

CREATE TRIGGER trg_PhieuDoiTra_Validation
ON PhieuDoiTra
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Sản phẩm chỉ có thể đổi trả nếu đơn hàng đã ở trạng thái 'Hoàn Thành'
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN DonHang dh ON i.MaDH = dh.MaDH
        WHERE dh.TrangThai <> N'Hoàn Thành'
    )
    BEGIN
        RAISERROR (N'Lỗi: Sản phẩm chỉ có thể đổi trả nếu đơn hàng đã ở trạng thái Hoàn Thành.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    -- Nếu sản phẩm đang có phiếu bảo hành trong trạng thái đang xử lý thì không có phiếu đổi trả
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN PhieuBaoHanh pbh ON i.MaSerialSP = pbh.MaSerialSP
        WHERE i.TrangThai = N'Đang Xử Lý' AND pbh.TrangThai = N'Đang Xử Lý'
    )
    BEGIN
        RAISERROR (N'Lỗi: Sản phẩm đang có phiếu bảo hành ở trạng thái Đang Xử Lý, không thể tạo/sửa phiếu đổi trả.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END;
GO

-- ============================================================
-- PHẦN 2: THÊM DỮ LIỆU MẪU
-- ============================================================

-- ----------------------------------------------------------
-- VaiTro
-- ----------------------------------------------------------
INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTaQuyen) VALUES
('VT00000001', N'Quản trị hệ thống',  N'Toàn quyền, quản lý tài khoản và phân quyền'),
('VT00000002', N'Nhân viên bán hàng', N'Tạo đơn hàng, hợp đồng, áp dụng khuyến mãi'),
('VT00000003', N'Nhân viên kho',      N'Quản lý nhập/xuất, tồn kho, đổi trả NCC, quản lý sản phẩm'),
('VT00000004', N'Nhân viên CSKH',     N'Xử lý bảo hành, đổi trả, khiếu nại'),
('VT00000005', N'Quản lý / Giám đốc', N'Xem báo cáo thống kê, không chỉnh sửa dữ liệu');

-- ----------------------------------------------------------
-- NhanVien
-- ----------------------------------------------------------
INSERT INTO NhanVien (MaNV, TenNV, GioiTinh, SinhNhat, SDT, DiaChi, Email, NgayVaoLam, Luong, ChucVu) VALUES
('NV00000001', N'Nguyễn Yến Nhi',   N'Nữ', '2005-03-25', '0901234567', N'12 Lê Lợi, Q.1, TP.HCM',         'nhi@laptopsore.vn',   '2024-01-15', 18000000, N'Quản trị viên hệ thống'),
('NV00000002', N'Nguyễn Thị Lan',   N'Nữ',  '1995-08-22', '0912345678', N'34 Nguyễn Huệ, Q.1, TP.HCM',     'lan.nguyen@laptopstore.vn',  '2020-03-01', 12000000, N'Nhân viên bán hàng'),
('NV00000003', N'Lê Quốc Hùng',     N'Nam', '1993-11-05', '0923456789', N'56 Trần Hưng Đạo, Q.5, TP.HCM',  'hung.le@laptopstore.vn',     '2019-07-10', 11000000, N'Nhân viên bán hàng'),
('NV00000004', N'Phạm Thị Hương',   N'Nữ',  '1997-03-18', '0934567890', N'78 Cách Mạng Tháng 8, Q.3',      'huong.pham@laptopstore.vn',  '2021-09-01', 10500000, N'Nhân viên kho'),
('NV00000005', N'Võ Thanh Tùng',    N'Nam', '1992-07-30', '0945678901', N'90 Đinh Tiên Hoàng, Bình Thạnh', 'tung.vo@laptopstore.vn',     '2020-11-15', 10500000, N'Nhân viên kho'),
('NV00000006', N'Đặng Ngọc Mai',    N'Nữ',  '1998-12-01', '0956789012', N'102 Hoàng Văn Thụ, Phú Nhuận',   'mai.dang@laptopstore.vn',    '2022-02-20', 10000000, N'Nhân viên CSKH'),
('NV00000007', N'Bùi Văn Thắng',    N'Nam', '1985-04-25', '0967890123', N'15 Pasteur, Q.3, TP.HCM',         'thang.bui@laptopstore.vn',   '2015-06-01', 25000000, N'Giám đốc kinh doanh');

-- ----------------------------------------------------------
-- TaiKhoanNV
-- ----------------------------------------------------------
INSERT INTO TaiKhoanNV (MaTK, MaNV, MaVaiTro, TenDangNhap, MatKhau, TrangThai) VALUES
('TKNV000001', 'NV00000001', 'VT00000001', 'admin.nhi',    'admin',   N'Hoạt Động'),
('TKNV000002', 'NV00000002', 'VT00000002', 'sale.lan',       'lan',     N'Hoạt Động'),
('TKNV000003', 'NV00000003', 'VT00000002', 'sale.hung',      'hung',    N'Hoạt Động'),
('TKNV000004', 'NV00000004', 'VT00000003', 'kho.huong',      'huong',   N'Hoạt Động'),
('TKNV000005', 'NV00000005', 'VT00000003', 'kho.tung',       'tung',    N'Hoạt Động'),
('TKNV000006', 'NV00000006', 'VT00000004', 'cskh.mai',       'mai',     N'Hoạt Động'),
('TKNV000007', 'NV00000007', 'VT00000005', 'giamdoc.thang',  'thang',   N'Hoạt Động');

-- ----------------------------------------------------------
-- HangSanXuat
-- ----------------------------------------------------------
INSERT INTO HangSanXuat (MaHang, TenHang, QuocGia) VALUES
('HANG000001', 'ASUS',    N'Đài Loan'),
('HANG000002', 'Dell',    N'Hoa Kỳ'),
('HANG000003', 'HP',      N'Hoa Kỳ'),
('HANG000004', 'Lenovo',  N'Trung Quốc'),
('HANG000005', 'Apple',   N'Hoa Kỳ'),
('HANG000006', 'Logitech',N'Thụy Sĩ');

-- ----------------------------------------------------------
-- LoaiSanPham
-- ----------------------------------------------------------
INSERT INTO LoaiSanPham (MaLoaiSP, MaHang, TenLoai, DanhMuc, ThoiGianBaoHanh, GiaBanGoc) VALUES
-- ASUS
('LSP0000001', 'HANG000001', N'ASUS VivoBook 15 X1504VA i5-1335U',  N'Laptop',   24, 16990000),
('LSP0000002', 'HANG000001', N'ASUS ROG Strix G15 Ryzen 9 7945HX',  N'Laptop',   24, 42990000),
('LSP0000003', 'HANG000001', N'ASUS ROG Strix Impact III Gaming',    N'Chuột',    12,  1290000),
-- Dell
('LSP0000004', 'HANG000002', N'Dell Inspiron 15 3535 Ryzen 5 7530U', N'Laptop',  24, 17990000),
('LSP0000005', 'HANG000002', N'Dell XPS 13 9340 Core Ultra 7',       N'Laptop',  24, 38990000),
('LSP0000006', 'HANG000002', N'Dell MS3320W Wireless Mouse',          N'Chuột',  12,    890000),
-- HP
('LSP0000007', 'HANG000003', N'HP Pavilion 15-eg3036TX Core i5-1335U', N'Laptop', 12, 17490000),
('LSP0000008', 'HANG000003', N'HP Wireless Keyboard & Mouse KM100',    N'Bàn Phím',12,   490000),
-- Lenovo
('LSP0000009', 'HANG000004', N'Lenovo IdeaPad Slim 3 15IRH8 i5-12450H', N'Laptop',24, 16490000),
('LSP0000010', 'HANG000004', N'Lenovo ThinkPad E14 Gen 5 Core i7-1355U', N'Laptop',36, 28990000),
-- Apple
('LSP0000011', 'HANG000005', N'Apple MacBook Air M2 8GB 256GB',      N'Laptop',   12, 28990000),
('LSP0000012', 'HANG000005', N'Apple Magic Mouse - Space Grey',       N'Chuột',   12,  2490000),
-- Logitech
('LSP0000013', 'HANG000006', N'Logitech MX Master 3S Wireless Mouse', N'Chuột',  24,  2390000),
('LSP0000014', 'HANG000006', N'Logitech MX Keys Mini Wireless KB',   N'Bàn Phím',24,  2890000);

-- ----------------------------------------------------------
-- CauHinh
-- ----------------------------------------------------------
INSERT INTO CauHinh (MaCauHinh, MaLoaiSP, TenThuocTinh) VALUES
-- LSP001 ASUS VivoBook 15
-- ----------------------------------------------------------
-- 1. LSP0000001: ASUS VivoBook 15 X1504VA i5-1335U (Laptop)
-- ----------------------------------------------------------
('CH00000001', 'LSP0000001', N'Công nghệ CPU: Intel Core i5 Raptor Lake (1335U)'),
('CH00000002', 'LSP0000001', N'Tốc độ CPU: 1.30 GHz (Turbo Boost 4.6 GHz)'),
('CH00000003', 'LSP0000001', N'RAM: 8 GB DDR4 3200 MHz'),
('CH00000004', 'LSP0000001', N'Dung lượng ổ cứng: 512 GB SSD PCIe NVMe M.2'),
('CH00000005', 'LSP0000001', N'Kích thước màn hình: 15.6 inch'),
('CH00000006', 'LSP0000001', N'Độ phân giải: Full HD (1920 x 1080)'),
('CH00000007', 'LSP0000001', N'Card đồ hoạ: Intel Iris Xe Graphics'),

-- ----------------------------------------------------------
-- 2. LSP0000002: ASUS ROG Strix G15 Ryzen 9 7945HX (Laptop)
-- ----------------------------------------------------------
('CH00000008', 'LSP0000002', N'Công nghệ CPU: AMD Ryzen 9 (7945HX)'),
('CH00000009', 'LSP0000002', N'Tốc độ CPU: 2.50 GHz (Turbo Boost 5.4 GHz)'),
('CH00000010', 'LSP0000002', N'RAM: 16 GB DDR5 4800 MHz'),
('CH00000011', 'LSP0000002', N'Dung lượng ổ cứng: 1 TB SSD PCIe NVMe M.2'),
('CH00000012', 'LSP0000002', N'Kích thước màn hình: 15.6 inch (360Hz)'),
('CH00000013', 'LSP0000002', N'Độ phân giải: Full HD (1920 x 1080)'),
('CH00000014', 'LSP0000002', N'Card đồ hoạ: NVIDIA GeForce RTX 4070 8GB'),

-- ----------------------------------------------------------
-- 3. LSP0000003: ASUS ROG Strix Impact III Gaming (Chuột)
-- ----------------------------------------------------------
('CH00000015', 'LSP0000003', N'Kết nối: Có dây USB 2.0'),
('CH00000016', 'LSP0000003', N'Đèn led: RGB ASUS Aura Sync'),
('CH00000017', 'LSP0000003', N'Màu sắc chuột: Đen'),

-- ----------------------------------------------------------
-- 4. LSP0000004: Dell Inspiron 15 3535 Ryzen 5 7530U (Laptop)
-- ----------------------------------------------------------
('CH00000018', 'LSP0000004', N'Công nghệ CPU: AMD Ryzen 5 (7530U)'),
('CH00000019', 'LSP0000004', N'Tốc độ CPU: 2.00 GHz (Turbo Boost 4.5 GHz)'),
('CH00000020', 'LSP0000004', N'RAM: 8 GB DDR4 3200 MHz'),
('CH00000021', 'LSP0000004', N'Dung lượng ổ cứng: 512 GB SSD PCIe NVMe'),
('CH00000022', 'LSP0000004', N'Kích thước màn hình: 15.6 inch (120Hz)'),
('CH00000023', 'LSP0000004', N'Độ phân giải: Full HD (1920 x 1080)'),
('CH00000024', 'LSP0000004', N'Card đồ hoạ: AMD Radeon Graphics'),

-- ----------------------------------------------------------
-- 5. LSP0000005: Dell XPS 13 9340 Core Ultra 7 (Laptop)
-- ----------------------------------------------------------
('CH00000025', 'LSP0000005', N'Công nghệ CPU: Intel Core Ultra 7 (155H)'),
('CH00000026', 'LSP0000005', N'Tốc độ CPU: 1.40 GHz (Turbo Boost 4.8 GHz)'),
('CH00000027', 'LSP0000005', N'RAM: 16 GB LPDDR5X 7467 MHz'),
('CH00000028', 'LSP0000005', N'Dung lượng ổ cứng: 512 GB SSD PCIe NVMe M.2 Gen 4'),
('CH00000029', 'LSP0000005', N'Kích thước màn hình: 13.4 inch'),
('CH00000030', 'LSP0000005', N'Độ phân giải: FHD+ (1920 x 1200) InfinityEdge'),
('CH00000031', 'LSP0000005', N'Card đồ hoạ: Intel Arc Graphics'),

-- ----------------------------------------------------------
-- 6. LSP0000006: Dell MS3320W Wireless Mouse (Chuột)
-- ----------------------------------------------------------
('CH00000032', 'LSP0000006', N'Kết nối: Không dây (2.4GHz Wireless & Bluetooth 5.0)'),
('CH00000033', 'LSP0000006', N'Đèn led: Không hỗ trợ'),
('CH00000034', 'LSP0000006', N'Màu sắc chuột: Xám đen (Titan Gray)'),

-- ----------------------------------------------------------
-- 7. LSP0000007: HP Pavilion 15-eg3036TX Core i5-1335U (Laptop)
-- ----------------------------------------------------------
('CH00000035', 'LSP0000007', N'Công nghệ CPU: Intel Core i5 Raptor Lake (1335U)'),
('CH00000036', 'LSP0000007', N'Tốc độ CPU: 1.30 GHz (Turbo Boost 4.6 GHz)'),
('CH00000037', 'LSP0000007', N'RAM: 8 GB DDR4 3200 MHz'),
('CH00000038', 'LSP0000007', N'Dung lượng ổ cứng: 512 GB SSD PCIe NVMe M.2'),
('CH00000039', 'LSP0000007', N'Kích thước màn hình: 15.6 inch'),
('CH00000040', 'LSP0000007', N'Độ phân giải: Full HD (1920 x 1080) IPS'),
('CH00000041', 'LSP0000007', N'Card đồ hoạ: NVIDIA GeForce MX550 2GB'),

-- ----------------------------------------------------------
-- 8. LSP0000008: HP Wireless Keyboard & Mouse KM100 (Bàn Phím combo - Lấy cấu hình phím)
-- ----------------------------------------------------------
('CH00000042', 'LSP0000008', N'KeyCap: Nhựa ABS độ bền cao, in Laser chống bay chữ'),
('CH00000043', 'LSP0000008', N'Kết nối: Không dây qua đầu thu USB 2.4GHz'),
('CH00000044', 'LSP0000008', N'Kích thước: Full-size (Có cụm phím số)'),

-- ----------------------------------------------------------
-- 9. LSP0000009: Lenovo IdeaPad Slim 3 15IRH8 i5-12450H (Laptop)
-- ----------------------------------------------------------
('CH00000045', 'LSP0000009', N'Công nghệ CPU: Intel Core i5 Alder Lake (12450H)'),
('CH00000046', 'LSP0000009', N'Tốc độ CPU: 2.00 GHz (Turbo Boost 4.4 GHz)'),
('CH00000047', 'LSP0000009', N'RAM: 8 GB LPDDR5 4800 MHz'),
('CH00000048', 'LSP0000009', N'Dung lượng ổ cứng: 512 GB SSD PCIe NVMe M.2 Gen 4'),
('CH00000049', 'LSP0000009', N'Kích thước màn hình: 15.6 inch'),
('CH00000050', 'LSP0000009', N'Độ phân giải: Full HD (1920 x 1080)'),
('CH00000051', 'LSP0000009', N'Card đồ hoạ: Intel UHD Graphics'),

-- ----------------------------------------------------------
-- 10. LSP0000010: Lenovo ThinkPad E14 Gen 5 Core i7-1355U (Laptop)
-- ----------------------------------------------------------
('CH00000052', 'LSP0000010', N'Công nghệ CPU: Intel Core i7 Raptor Lake (1355U)'),
('CH00000053', 'LSP0000010', N'Tốc độ CPU: 1.70 GHz (Turbo Boost 5.0 GHz)'),
('CH00000054', 'LSP0000010', N'RAM: 16 GB DDR4 3200 MHz'),
('CH00000055', 'LSP0000010', N'Dung lượng ổ cứng: 512 GB SSD PCIe NVMe M.2'),
('CH00000056', 'LSP0000010', N'Kích thước màn hình: 14.0 inch'),
('CH00000057', 'LSP0000010', N'Độ phân giải: WUXGA (1920 x 1200) IPS'),
('CH00000058', 'LSP0000010', N'Card đồ hoạ: Intel Iris Xe Graphics'),

-- ----------------------------------------------------------
-- 11. LSP0000011: Apple MacBook Air M2 8GB 256GB (Laptop)
-- ----------------------------------------------------------
('CH00000059', 'LSP0000011', N'Công nghệ CPU: Apple M2 (8 nhân CPU / 8 nhân GPU)'),
('CH00000060', 'LSP0000011', N'Tốc độ CPU: 100 GB/s bộ nhớ băng thông'),
('CH00000061', 'LSP0000011', N'RAM: 8 GB Unified Memory'),
('CH00000062', 'LSP0000011', N'Dung lượng ổ cứng: 256 GB SSD cao cấp'),
('CH00000063', 'LSP0000011', N'Kích thước màn hình: 13.6 inch Liquid Retina'),
('CH00000064', 'LSP0000011', N'Độ phân giải: Liquid Retina (2560 x 1664)'),
('CH00000065', 'LSP0000011', N'Card đồ hoạ: Apple GPU 8 nhân tích hợp'),

-- ----------------------------------------------------------
-- 12. LSP0000012: Apple Magic Mouse - Space Grey (Chuột)
-- ----------------------------------------------------------
('CH00000066', 'LSP0000012', N'Kết nối: Không dây Bluetooth, Cổng Lightning'),
('CH00000067', 'LSP0000012', N'Đèn led: Không hỗ trợ'),
('CH00000068', 'LSP0000012', N'Màu sắc chuột: Xám không gian (Space Grey)'),

-- ----------------------------------------------------------
-- 13. LSP0000013: Logitech MX Master 3S Wireless Mouse (Chuột)
-- ----------------------------------------------------------
('CH00000069', 'LSP0000013', N'Kết nối: Không dây Bluetooth & Logi Bolt USB Receiver'),
('CH00000070', 'LSP0000013', N'Đèn led: Đèn hiển thị trạng thái pin LED đơn sắc'),
('CH00000071', 'LSP0000013', N'Màu sắc chuột: Đen graphite'),

-- ----------------------------------------------------------
-- 14. LSP0000014: Logitech MX Keys Mini Wireless KB (Bàn Phím)
-- ----------------------------------------------------------
('CH00000072', 'LSP0000014', N'KeyCap: Thiết kế lõm theo đầu ngón tay (Spherically-dished)'),
('CH00000073', 'LSP0000014', N'Kết nối: Không dây Bluetooth Low Energy / Logi Bolt'),
('CH00000074', 'LSP0000014', N'Kích thước: Tenkeyless Mini (75% - Không cụm phím số)');
GO

-- ----------------------------------------------------------
-- NhaCungCap
-- ----------------------------------------------------------
INSERT INTO NhaCungCap (MaNCC, TenNCC, Email, SDT, DiaChi) VALUES
('NCC0000001', N'Công ty TNHH Phân Phối ASUS Việt Nam',   'asus.vn@asus-dist.com',  '0281234567', N'123 Nguyễn Văn Cừ, Q.5, TP.HCM'),
('NCC0000002', N'Dell Technologies Vietnam',               'order@dell.com.vn',      '0289876543', N'Tòa nhà Bitexco, Q.1, TP.HCM'),
('NCC0000003', N'HP Vietnam Distribution Co.',             'hp.supply@hp.vn',        '0283456789', N'115 Đinh Bộ Lĩnh, Bình Thạnh'),
('NCC0000004', N'Lenovo Vietnam Official',                 'lenovo@lenovovn.com',    '0284567890', N'22 Võ Văn Tần, Q.3, TP.HCM'),
('NCC0000005', N'Apple Authorized Distributor VN',         'apple.dist@fpt.vn',      '0285678901', N'FPT Tower, Q.7, TP.HCM'),
('NCC0000006', N'Logitech SEA Distribution',               'logitech.sea@logi.com',  '0286789012', N'35 Hoàng Diệu 2, Thủ Đức');

-- ----------------------------------------------------------
-- PhieuNhap
-- ----------------------------------------------------------
INSERT INTO PhieuNhap (MaPhieuNhap, MaNV, MaNCC, NgayNhap, TongTien, TrangThai) VALUES
('PN00000001', 'NV00000004', 'NCC0000001', '2025-11-01', 284280000, N'Đã Nhập'),
('PN00000002', 'NV00000004', 'NCC0000002', '2025-11-05', 227570000, N'Đã Nhập'),
('PN00000003', 'NV00000005', 'NCC0000004', '2025-11-10', 204370000, N'Đã Nhập'),
('PN00000004', 'NV00000005', 'NCC0000005', '2025-12-01', 290850000, N'Đã Nhập'),
('PN00000005', 'NV00000004', 'NCC0000001', '2026-01-15',  64960000, N'Đã Nhập'),
('PN00000006', 'NV00000005', 'NCC0000003', '2026-02-01',  91400000, N'Đã Nhập'),
('PN00000007', 'NV00000004', 'NCC0000006', '2026-03-10',  82490000, N'Đã Nhập'),
('PN00000008', 'NV00000005', 'NCC0000002', '2026-04-01', 194950000, N'Chờ Xác Nhận');

-- ----------------------------------------------------------
-- ChiTietPhieuNhap
-- ----------------------------------------------------------
INSERT INTO ChiTietPhieuNhap (MaLoaiSP, MaPhieuNhap, SoLuong, GiaNhap) VALUES
('LSP0000001', 'PN00000001', 13,  14990000),
('LSP0000002', 'PN00000001', 2,  37990000),
('LSP0000003', 'PN00000001', 10,   1090000),
('LSP0000004', 'PN00000002', 10,  15990000),
('LSP0000005', 'PN00000002', 2,  35990000),
('LSP0000006', 'PN00000002', 13,   740000),
('LSP0000009', 'PN00000003', 10,  14490000),
('LSP0000010', 'PN00000003', 3,  25990000),
('LSP0000014', 'PN00000003', 5,   2490000),
('LSP0000011', 'PN00000004', 10,  26990000),
('LSP0000012', 'PN00000004', 13,   2190000),
('LSP0000001', 'PN00000005', 4,  14990000),
('LSP0000003', 'PN00000005', 4,   1090000),
('LSP0000007', 'PN00000006', 5,  15490000),
('LSP0000008', 'PN00000006', 18,    395000),
('LSP0000013', 'PN00000007', 13,   1990000),
('LSP0000014', 'PN00000007', 18,   2490000),
('LSP0000005', 'PN00000008', 5,  35990000);

-- ----------------------------------------------------------
-- SanPham (Serial riêng cho từng máy)
-- ----------------------------------------------------------
-- PN00000001: 8× LSP0000001, 2× LSP0000002, 5× LSP0000003
INSERT INTO SanPham (MaSerialSP, MaPhieuNhap, MaLoaiSP, NgayNhap, NgaySX, TrangThai) VALUES
-- ASUS VivoBook (LSP0000001) - PN00000001
('ASUS-VB-001', 'PN00000001','LSP0000001','2025-11-01','2025-09-01', N'Đã Bán'),
('ASUS-VB-002', 'PN00000001','LSP0000001','2025-11-01','2025-09-01', N'Đã Bán'),
('ASUS-VB-003', 'PN00000001','LSP0000001','2025-11-01','2025-09-05', N'Đã Bán'),
('ASUS-VB-004', 'PN00000001','LSP0000001','2025-11-01','2025-09-05', N'Đã Bán'),
('ASUS-VB-005', 'PN00000001','LSP0000001','2025-11-01','2025-09-10', N'Đã Bán'),
('ASUS-VB-006', 'PN00000001','LSP0000001','2025-11-01','2025-09-10', N'Trong Kho'),
('ASUS-VB-007', 'PN00000001','LSP0000001','2025-11-01','2025-09-15', N'Trong Kho'),
('ASUS-VB-008', 'PN00000001','LSP0000001','2025-11-01','2025-09-15', N'Trong Kho'),
-- ASUS ROG (LSP0000002) - PN00000001
('ASUS-ROG-001','PN00000001','LSP0000002','2025-11-01','2025-08-20', N'Đã Bán'),
('ASUS-ROG-002','PN00000001','LSP0000002','2025-11-01','2025-08-20', N'Trong Kho'),
-- ASUS Chuột ROG (LSP0000003) - PN00000001
('ASUS-MS-001', 'PN00000001','LSP0000003','2025-11-01','2025-07-01', N'Đã Bán'),
('ASUS-MS-002', 'PN00000001','LSP0000003','2025-11-01','2025-07-01', N'Đã Bán'),
('ASUS-MS-003', 'PN00000001','LSP0000003','2025-11-01','2025-07-01', N'Trong Kho'),
('ASUS-MS-004', 'PN00000001','LSP0000003','2025-11-01','2025-07-15', N'Trong Kho'),
('ASUS-MS-005', 'PN00000001','LSP0000003','2025-11-01','2025-07-15', N'Trong Kho'),
-- Dell Inspiron (LSP0000004) - PN00000002
('DELL-INS-001','PN00000002','LSP0000004','2025-11-05','2025-09-01', N'Đã Bán'),
('DELL-INS-002','PN00000002','LSP0000004','2025-11-05','2025-09-01', N'Đã Bán'),
('DELL-INS-003','PN00000002','LSP0000004','2025-11-05','2025-09-10', N'Đã Bán'),
('DELL-INS-004','PN00000002','LSP0000004','2025-11-05','2025-09-10', N'Trong Kho'),
('DELL-INS-005','PN00000002','LSP0000004','2025-11-05','2025-09-15', N'Trong Kho'),
-- Dell XPS (LSP0000005) - PN00000002
('DELL-XPS-001','PN00000002','LSP0000005','2025-11-05','2025-08-01', N'Đã Bán'),
('DELL-XPS-002','PN00000002','LSP0000005','2025-11-05','2025-08-01', N'Trong Kho'),
-- Dell Chuột (LSP0000006) - PN00000002
('DELL-MS-001', 'PN00000002','LSP0000006','2025-11-05','2025-06-01', N'Đã Bán'),
('DELL-MS-002', 'PN00000002','LSP0000006','2025-11-05','2025-06-01', N'Đã Bán'),
('DELL-MS-003', 'PN00000002','LSP0000006','2025-11-05','2025-06-01', N'Trong Kho'),
('DELL-MS-004', 'PN00000002','LSP0000006','2025-11-05','2025-06-15', N'Trong Kho'),
('DELL-MS-005', 'PN00000002','LSP0000006','2025-11-05','2025-06-15', N'Trong Kho'),
('DELL-MS-006', 'PN00000002','LSP0000006','2025-11-05','2025-06-15', N'Trong Kho'),
('DELL-MS-007', 'PN00000002','LSP0000006','2025-11-05','2025-06-20', N'Trong Kho'),
('DELL-MS-008', 'PN00000002','LSP0000006','2025-11-05','2025-06-20', N'Trong Kho'),
-- Lenovo IdeaPad (LSP0000009) - PN00000003
('LNVO-IP-001', 'PN00000003','LSP0000009','2025-11-10','2025-09-05', N'Đã Bán'),
('LNVO-IP-002', 'PN00000003','LSP0000009','2025-11-10','2025-09-05', N'Đã Bán'),
('LNVO-IP-003', 'PN00000003','LSP0000009','2025-11-10','2025-09-10', N'Đã Bán'),
('LNVO-IP-004', 'PN00000003','LSP0000009','2025-11-10','2025-09-10', N'Trong Kho'),
('LNVO-IP-005', 'PN00000003','LSP0000009','2025-11-10','2025-09-15', N'Trong Kho'),
-- Lenovo ThinkPad (LSP0000010) - PN00000003
('LNVO-TP-001', 'PN00000003','LSP0000010','2025-11-10','2025-08-10', N'Đã Bán'),
('LNVO-TP-002', 'PN00000003','LSP0000010','2025-11-10','2025-08-10', N'Đã Bán'),
('LNVO-TP-003', 'PN00000003','LSP0000010','2025-11-10','2025-08-15', N'Trong Kho'),
-- Logitech KB (LSP0000014) - PN00000003
('LOGI-KB-001', 'PN00000003','LSP0000014','2025-11-10','2025-07-01', N'Đã Bán'),
('LOGI-KB-002', 'PN00000003','LSP0000014','2025-11-10','2025-07-01', N'Đã Bán'),
('LOGI-KB-003', 'PN00000003','LSP0000014','2025-11-10','2025-07-05', N'Trong Kho'),
('LOGI-KB-004', 'PN00000003','LSP0000014','2025-11-10','2025-07-05', N'Trong Kho'),
('LOGI-KB-005', 'PN00000003','LSP0000014','2025-11-10','2025-07-10', N'Trong Kho'),
-- MacBook Air (LSP0000011) - PN00000004
('APPL-MBA-001','PN00000004','LSP0000011','2025-12-01','2025-10-01', N'Đã Bán'),
('APPL-MBA-002','PN00000004','LSP0000011','2025-12-01','2025-10-01', N'Đã Bán'),
('APPL-MBA-003','PN00000004','LSP0000011','2025-12-01','2025-10-05', N'Đã Bán'),
('APPL-MBA-004','PN00000004','LSP0000011','2025-12-01','2025-10-05', N'Trong Kho'),
('APPL-MBA-005','PN00000004','LSP0000011','2025-12-01','2025-10-10', N'Trong Kho'),
-- Apple Magic Mouse (LSP0000012) - PN00000004
('APPL-MM-001', 'PN00000004','LSP0000012','2025-12-01','2025-09-01', N'Đã Bán'),
('APPL-MM-002', 'PN00000004','LSP0000012','2025-12-01','2025-09-01', N'Đã Bán'),
('APPL-MM-003', 'PN00000004','LSP0000012','2025-12-01','2025-09-05', N'Trong Kho'),
('APPL-MM-004', 'PN00000004','LSP0000012','2025-12-01','2025-09-05', N'Trong Kho'),
('APPL-MM-005', 'PN00000004','LSP0000012','2025-12-01','2025-09-10', N'Trong Kho'),
('APPL-MM-006', 'PN00000004','LSP0000012','2025-12-01','2025-09-10', N'Trong Kho'),
('APPL-MM-007', 'PN00000004','LSP0000012','2025-12-01','2025-09-15', N'Trong Kho'),
('APPL-MM-008', 'PN00000004','LSP0000012','2025-12-01','2025-09-15', N'Trong Kho'),
-- ASUS VivoBook (LSP0000001) - PN00000005
('ASUS-VB-009', 'PN00000005','LSP0000001','2026-01-15','2025-11-01', N'Trong Kho'),
('ASUS-VB-010', 'PN00000005','LSP0000001','2026-01-15','2025-11-01', N'Trong Kho'),
('ASUS-VB-011', 'PN00000005','LSP0000001','2026-01-15','2025-11-05', N'Trong Kho'),
('ASUS-VB-012', 'PN00000005','LSP0000001','2026-01-15','2025-11-05', N'Trong Kho'),
-- ASUS Chuột ROG (LSP0000003) - PN00000005
('ASUS-MS-006', 'PN00000005','LSP0000003','2026-01-15','2025-10-01', N'Trong Kho'),
('ASUS-MS-007', 'PN00000005','LSP0000003','2026-01-15','2025-10-01', N'Trong Kho'),
('ASUS-MS-008', 'PN00000005','LSP0000003','2026-01-15','2025-10-05', N'Trong Kho'),
('ASUS-MS-009', 'PN00000005','LSP0000003','2026-01-15','2025-10-05', N'Trong Kho'),
-- HP Pavilion (LSP0000007) - PN00000006
('HP-PAV-001',  'PN00000006','LSP0000007','2026-02-01','2025-12-01', N'Đã Bán'),
('HP-PAV-002',  'PN00000006','LSP0000007','2026-02-01','2025-12-01', N'Đã Bán'),
('HP-PAV-003',  'PN00000006','LSP0000007','2026-02-01','2025-12-05', N'Bảo Hành'),
('HP-PAV-004',  'PN00000006','LSP0000007','2026-02-01','2025-12-05', N'Trong Kho'),
('HP-PAV-005',  'PN00000006','LSP0000007','2026-02-01','2025-12-10', N'Trong Kho'),
-- HP Bàn phím (LSP0000008) - PN00000006
('HP-KB-001',   'PN00000006','LSP0000008','2026-02-01','2025-11-01', N'Đã Bán'),
('HP-KB-002',   'PN00000006','LSP0000008','2026-02-01','2025-11-01', N'Đã Bán'),
('HP-KB-003',   'PN00000006','LSP0000008','2026-02-01','2025-11-05', N'Trong Kho'),
('HP-KB-004',   'PN00000006','LSP0000008','2026-02-01','2025-11-05', N'Trong Kho'),
('HP-KB-005',   'PN00000006','LSP0000008','2026-02-01','2025-11-05', N'Trong Kho'),
('HP-KB-006',   'PN00000006','LSP0000008','2026-02-01','2025-11-10', N'Trong Kho'),
('HP-KB-007',   'PN00000006','LSP0000008','2026-02-01','2025-11-10', N'Trong Kho'),
('HP-KB-008',   'PN00000006','LSP0000008','2026-02-01','2025-11-10', N'Trong Kho'),
-- Logitech Chuột MX (LSP0000013) - PN00000007
('LOGI-MX-001', 'PN00000007','LSP0000013','2026-03-10','2026-01-01', N'Trong Kho'),
('LOGI-MX-002', 'PN00000007','LSP0000013','2026-03-10','2026-01-01', N'Trong Kho'),
('LOGI-MX-003', 'PN00000007','LSP0000013','2026-03-10','2026-01-05', N'Trong Kho'),
('LOGI-MX-004', 'PN00000007','LSP0000013','2026-03-10','2026-01-05', N'Trong Kho'),
('LOGI-MX-005', 'PN00000007','LSP0000013','2026-03-10','2026-01-10', N'Trong Kho'),
('LOGI-MX-006', 'PN00000007','LSP0000013','2026-03-10','2026-01-10', N'Trong Kho'),
('LOGI-MX-007', 'PN00000007','LSP0000013','2026-03-10','2026-01-15', N'Trong Kho'),
('LOGI-MX-008', 'PN00000007','LSP0000013','2026-03-10','2026-01-15', N'Trong Kho'),
-- Logitech KB MX Keys (LSP0000014) - PN00000007
('LOGI-KB-006', 'PN00000007','LSP0000014','2026-03-10','2026-01-01', N'Trong Kho'),
('LOGI-KB-007', 'PN00000007','LSP0000014','2026-03-10','2026-01-01', N'Trong Kho'),
('LOGI-KB-008', 'PN00000007','LSP0000014','2026-03-10','2026-01-05', N'Trong Kho'),
('LOGI-KB-009', 'PN00000007','LSP0000014','2026-03-10','2026-01-05', N'Trong Kho'),
('LOGI-KB-010', 'PN00000007','LSP0000014','2026-03-10','2026-01-10', N'Trong Kho'),
('LOGI-KB-011', 'PN00000007','LSP0000014','2026-03-10','2026-01-10', N'Trong Kho'),
('LOGI-KB-012', 'PN00000007','LSP0000014','2026-03-10','2026-01-15', N'Trong Kho'),
('LOGI-KB-013', 'PN00000007','LSP0000014','2026-03-10','2026-01-15', N'Trong Kho'),
-- Bổ sung 20 sản phẩm Laptop
('ASUS-VB-013', 'PN00000001', 'LSP0000001', '2025-11-01', '2025-09-15', N'Trong Kho'),
('ASUS-VB-014', 'PN00000001', 'LSP0000001', '2025-11-01', '2025-09-15', N'Trong Kho'),
('ASUS-VB-015', 'PN00000001', 'LSP0000001', '2025-11-01', '2025-09-15', N'Trong Kho'),
('ASUS-VB-016', 'PN00000001', 'LSP0000001', '2025-11-01', '2025-09-15', N'Trong Kho'),
('ASUS-VB-017', 'PN00000001', 'LSP0000001', '2025-11-01', '2025-09-15', N'Trong Kho'),
('DELL-INS-006', 'PN00000002', 'LSP0000004', '2025-11-05', '2025-09-15', N'Trong Kho'),
('DELL-INS-007', 'PN00000002', 'LSP0000004', '2025-11-05', '2025-09-15', N'Trong Kho'),
('DELL-INS-008', 'PN00000002', 'LSP0000004', '2025-11-05', '2025-09-15', N'Trong Kho'),
('DELL-INS-009', 'PN00000002', 'LSP0000004', '2025-11-05', '2025-09-15', N'Trong Kho'),
('DELL-INS-010', 'PN00000002', 'LSP0000004', '2025-11-05', '2025-09-15', N'Trong Kho'),
('LNVO-IP-006', 'PN00000003', 'LSP0000009', '2025-11-10', '2025-09-15', N'Trong Kho'),
('LNVO-IP-007', 'PN00000003', 'LSP0000009', '2025-11-10', '2025-09-15', N'Trong Kho'),
('LNVO-IP-008', 'PN00000003', 'LSP0000009', '2025-11-10', '2025-09-15', N'Trong Kho'),
('LNVO-IP-009', 'PN00000003', 'LSP0000009', '2025-11-10', '2025-09-15', N'Trong Kho'),
('LNVO-IP-010', 'PN00000003', 'LSP0000009', '2025-11-10', '2025-09-15', N'Trong Kho'),
('APPL-MBA-006', 'PN00000004', 'LSP0000011', '2025-12-01', '2025-10-10', N'Trong Kho'),
('APPL-MBA-007', 'PN00000004', 'LSP0000011', '2025-12-01', '2025-10-10', N'Trong Kho'),
('APPL-MBA-008', 'PN00000004', 'LSP0000011', '2025-12-01', '2025-10-10', N'Trong Kho'),
('APPL-MBA-009', 'PN00000004', 'LSP0000011', '2025-12-01', '2025-10-10', N'Trong Kho'),
('APPL-MBA-010', 'PN00000004', 'LSP0000011', '2025-12-01', '2025-10-10', N'Trong Kho'),
-- Bổ sung 20 sản phẩm Chuột
('ASUS-MS-010', 'PN00000001', 'LSP0000003', '2025-11-01', '2025-07-15', N'Trong Kho'),
('ASUS-MS-011', 'PN00000001', 'LSP0000003', '2025-11-01', '2025-07-15', N'Trong Kho'),
('ASUS-MS-012', 'PN00000001', 'LSP0000003', '2025-11-01', '2025-07-15', N'Trong Kho'),
('ASUS-MS-013', 'PN00000001', 'LSP0000003', '2025-11-01', '2025-07-15', N'Trong Kho'),
('ASUS-MS-014', 'PN00000001', 'LSP0000003', '2025-11-01', '2025-07-15', N'Trong Kho'),
('DELL-MS-009', 'PN00000002', 'LSP0000006', '2025-11-05', '2025-06-20', N'Trong Kho'),
('DELL-MS-010', 'PN00000002', 'LSP0000006', '2025-11-05', '2025-06-20', N'Trong Kho'),
('DELL-MS-011', 'PN00000002', 'LSP0000006', '2025-11-05', '2025-06-20', N'Trong Kho'),
('DELL-MS-012', 'PN00000002', 'LSP0000006', '2025-11-05', '2025-06-20', N'Trong Kho'),
('DELL-MS-013', 'PN00000002', 'LSP0000006', '2025-11-05', '2025-06-20', N'Trong Kho'),
('APPL-MM-009', 'PN00000004', 'LSP0000012', '2025-12-01', '2025-09-15', N'Trong Kho'),
('APPL-MM-010', 'PN00000004', 'LSP0000012', '2025-12-01', '2025-09-15', N'Trong Kho'),
('APPL-MM-011', 'PN00000004', 'LSP0000012', '2025-12-01', '2025-09-15', N'Trong Kho'),
('APPL-MM-012', 'PN00000004', 'LSP0000012', '2025-12-01', '2025-09-15', N'Trong Kho'),
('APPL-MM-013', 'PN00000004', 'LSP0000012', '2025-12-01', '2025-09-15', N'Trong Kho'),
('LOGI-MX-009', 'PN00000007', 'LSP0000013', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-MX-010', 'PN00000007', 'LSP0000013', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-MX-011', 'PN00000007', 'LSP0000013', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-MX-012', 'PN00000007', 'LSP0000013', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-MX-013', 'PN00000007', 'LSP0000013', '2026-03-10', '2026-01-15', N'Trong Kho'),
-- Bổ sung 20 sản phẩm Bàn Phím
('HP-KB-009', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-010', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-011', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-012', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-013', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-014', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-015', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-016', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-017', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('HP-KB-018', 'PN00000006', 'LSP0000008', '2026-02-01', '2025-11-10', N'Trong Kho'),
('LOGI-KB-014', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-015', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-016', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-017', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-018', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-019', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-020', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-021', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-022', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho'),
('LOGI-KB-023', 'PN00000007', 'LSP0000014', '2026-03-10', '2026-01-15', N'Trong Kho');

-- ----------------------------------------------------------
-- KhachHang
-- ----------------------------------------------------------
INSERT INTO KhachHang (MaKH, TenKH, Email, SDT, DiaChi, LoaiKH) VALUES
-- Khách lẻ
('KH00000001', N'Nguyễn Văn An',       'an.nguyen@gmail.com',      '0901111111', N'45 Lê Văn Sỹ, Q.3, TP.HCM',         N'Lẻ'),
('KH00000002', N'Trần Thị Bích',       'bich.tran@yahoo.com',      '0912222222', N'78 Cô Giang, Q.1, TP.HCM',           N'Lẻ'),
('KH00000003', N'Lê Minh Châu',        'chau.le@outlook.com',      '0923333333', N'12 Nguyễn Thị Minh Khai, Q.1',       N'Lẻ'),
('KH00000004', N'Phạm Thị Dung',       'dung.pham@gmail.com',      '0934444444', N'33 Đinh Công Tráng, Q.1, TP.HCM',    N'Lẻ'),
('KH00000005', N'Hoàng Văn Em',        'em.hoang@gmail.com',       '0945555555', N'56 Trần Quốc Thảo, Q.3, TP.HCM',    N'Lẻ'),
('KH00000006', N'Võ Thị Phương',       'phuong.vo@gmail.com',      '0956666666', N'90 Bùi Thị Xuân, Q.1, TP.HCM',      N'Lẻ'),
-- Khách sỉ (doanh nghiệp)
('DN00000007', N'Công ty CP Giáo Dục Tương Lai', 'procurement@tuonglai.edu.vn', '0281112233', N'100 Nguyễn Đình Chiểu, Q.3', N'Sỉ'),
('DN00000008', N'Công ty TNHH Thiết Kế ABC',     'purchase@abcdesign.vn',       '0282223344', N'200 Võ Thị Sáu, Q.1, TP.HCM', N'Sỉ'),
('DN00000009', N'Tập đoàn Công Nghệ XYZ',        'it.dept@xyz-tech.vn',         '0283334455', N'Toà nhà M Plaza, Q.1, TP.HCM', N'Sỉ');

-- ----------------------------------------------------------
-- KhachHangLe
-- ----------------------------------------------------------
INSERT INTO KhachHangLe (MaKHLe, LaHSSV, SinhNhat) VALUES
('KH00000001', 0, '1990-03-15'),
('KH00000002', 0, '1988-07-22'),
('KH00000003', 1, '2002-11-08'),
('KH00000004', 1, '2003-05-30'),
('KH00000005', 0, '1985-01-20'),
('KH00000006', 1, '2001-09-12');

-- ----------------------------------------------------------
-- KhachHangSi
-- ----------------------------------------------------------
INSERT INTO KhachHangSi (MaKHSi) VALUES
('DN00000007'),('DN00000008'),('DN00000009');

-- ----------------------------------------------------------
-- TaiKhoanKH
-- ----------------------------------------------------------
INSERT INTO TaiKhoanKH (MaTK, MaKH, TenDangNhap, MatKhau, NgayTao, TrangThai) VALUES
('TKKH000001', 'KH00000001', 'an.nguyen',      '$2b$12$hash_an',      '2025-08-01', N'Hoạt Động'),
('TKKH000002', 'KH00000002', 'bich.tran',      '$2b$12$hash_bich',    '2025-09-10', N'Hoạt Động'),
('TKKH000003', 'KH00000003', 'chau.le.hssv',   '$2b$12$hash_chau',    '2025-08-15', N'Hoạt Động'),
('TKKH000004', 'KH00000004', 'dung.pham.sv',   '$2b$12$hash_dung',    '2025-09-01', N'Hoạt Động'),
('TKKH000005', 'KH00000005', 'em.hoang',       '$2b$12$hash_em',      '2025-10-20', N'Hoạt Động'),
('TKKH000006', 'KH00000006', 'phuong.vo.sv',   '$2b$12$hash_phuong',  '2025-11-05', N'Hoạt Động');

-- ----------------------------------------------------------
-- LichSuDangNhap
-- ----------------------------------------------------------
INSERT INTO LichSuDangNhap (MaLSDN, MaTK, ThoiGian, DiaChiIP, TrangThai) VALUES
('LSDN000001', 'TKNV000001', '2026-01-10 08:00:00', '192.168.1.10', N'Thành Công'),
('LSDN000002', 'TKNV000002', '2026-01-10 08:05:00', '192.168.1.11', N'Thành Công'),
('LSDN000003', 'TKNV000003', '2026-02-01 08:30:00', '192.168.1.12', N'Thành Công');

-- ----------------------------------------------------------
-- KhuyenMai 
-- ----------------------------------------------------------
INSERT INTO KhuyenMai (MaKM, TenKM, DoiTuong, DieuKien, NgayBatDau, NgayKetThuc, MoTa, MucGiamSP, MucGiamDH, SLToiThieu, isHienThi) VALUES
('KM00000001', N'Back To School',
    N'HSSV',
    NULL,
    '2026-01-01', '2026-12-31',
    N'Khách hàng phải là học sinh hoặc sinh viên (LaHSSV = 1)',
    10.00, NULL, NULL, 1),
('KM00000002', N'Black Friday',
    N'Tất Cả',
    NULL,
    '2025-11-25', '2025-11-30',
    N'Áp dụng cho tất cả khách hàng trong thời gian diễn ra',
    5.00, NULL, NULL, 1),
('KM00000003', N'10 Laptop 10%',
    N'Doanh Nghiệp',
    N'Laptop',
    '2025-01-01', '2026-12-31',
    N'Mua từ 10 laptop trở lên trong cùng một đơn hàng',
    NULL, 5.00, 10, 0),
('KM00000004', N'30 Laptop 10%',
    N'Doanh Nghiệp',
    N'Laptop',
    '2025-01-01', '2026-12-31',
    N'Mua từ 30 laptop',
    NULL, 10.00, 30, 0),
('KM00000005', N'30 Laptop 20% (10+ chuột/ bàn phím)',
    N'Doanh Nghiệp',
    N'Laptop',
    '2025-01-01', '2026-12-31',
    N'Mua từ 30 laptop; giảm thêm nếu kèm 10+ chuột hoặc 10+ bàn phím',
    NULL, 20.00, 30, 0);

-- ----------------------------------------------------------
-- HopDong  (khách hàng sỉ)
-- ----------------------------------------------------------
INSERT INTO HopDong (MaHD, MaNV, MaKH, NgayKy, GiaTriHD, NgayHieuLuc, NgayHetHan, TrangThai) VALUES
('HD00000001', 'NV00000002', 'DN00000007', '2025-10-01',  500000000, '2025-10-01', '2026-09-30', N'Hiệu Lực'),
('HD00000002', 'NV00000003', 'DN00000008', '2025-11-01',  300000000, '2025-11-01', '2026-10-31', N'Hiệu Lực'),
('HD00000003', 'NV00000002', 'DN00000009', '2026-01-15', 1000000000, '2026-01-15', '2027-01-14', N'Hiệu Lực');

-- ----------------------------------------------------------
-- DonHang
-- ----------------------------------------------------------
-- DH00000001: KH00000001 mua lẻ - ASUS VB (không khuyến mãi)
INSERT INTO DonHang (MaDH, MaNV, MaKH, MaKM, MaHD, NgayDat, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai) VALUES
('DH00000001','NV00000002','KH00000001', NULL, NULL, '2025-11-20 10:30:00', 16990000, 16990000, N'Tiền Mặt',       N'Hoàn Thành'),
-- DH00000002: KH00000003 (HSSV) mua Black Friday  -- Note: BTS đã hết, dùng BlackFriday
('DH00000002','NV00000002','KH00000003', 'KM00000002', NULL, '2025-11-26 14:00:00', 17490000, 16615500, N'Chuyển Khoản', N'Hoàn Thành'),
-- DH00000003: KH00000002 mua lẻ - Dell + Chuột
('DH00000003','NV00000003','KH00000002', NULL, NULL, '2025-12-05 09:00:00', 17990000, 17990000, N'Thẻ',            N'Hoàn Thành'),
-- DH00000004: KH00000007 (sỉ) theo hợp đồng HD00000001 - 3 Lenovo ThinkPad
('DH00000004','NV00000002','DN00000007', NULL, 'HD00000001', '2025-12-10 11:00:00', 86970000, 86970000, N'Chuyển Khoản', N'Hoàn Thành'),
-- DH00000005: KH00000003 (HSSV) Back To School 2025
('DH00000005','NV00000002','KH00000003', 'KM00000001', NULL, '2025-08-25 15:30:00', 16990000, 15291000, N'Chuyển Khoản', N'Hoàn Thành'),
-- DH00000006: KH00000004 (HSSV) mua online - MacBook Air
('DH00000006','NV00000002','KH00000004', NULL, NULL, '2025-12-20 16:00:00', 28990000, 28990000, N'Chuyển Khoản',   N'Hoàn Thành'),
-- DH00000007: KH00000008 (sỉ) HD00000002 - 2 Dell XPS
('DH00000007','NV00000003','DN00000008', NULL, 'HD00000002', '2026-01-05 10:00:00', 77980000, 77980000, N'Chuyển Khoản', N'Hoàn Thành'),
-- DH00000008: KH00000005 mua lẻ - ASUS ROG
('DH00000008','NV00000002','KH00000005', NULL, NULL, '2026-01-15 13:00:00', 42990000, 42990000, N'Tiền Mặt',       N'Hoàn Thành'),
-- DH00000009: KH00000006 (HSSV) mua lẻ - HP Pavilion + KB
('DH00000009','NV00000003','KH00000006', NULL, NULL, '2026-02-10 11:00:00', 17980000, 17980000, N'Thẻ',            N'Hoàn Thành'),
-- DH00000010: KH00000009 (sỉ) HD00000003 - 3 MacBook Air + 2 Magic Mouse
('DH00000010','NV00000002','DN00000009', 'KM00000003', 'HD00000003', '2026-03-01 09:00:00', 91940000, 87343000, N'Chuyển Khoản', N'Hoàn Thành'),
-- DH00000011: KH00000001 đơn đang xử lý
('DH00000011','NV00000003','KH00000001', NULL, NULL, '2026-05-20 10:00:00', 16990000, 16990000, N'Tiền Mặt',        N'Chờ Xử Lý'),
-- DH00000012: KH00000002 đơn đang giao
('DH00000012','NV00000002','KH00000002', NULL, NULL, '2026-05-18 14:00:00', 2390000,  2390000,  N'Chuyển Khoản',    N'Đang Giao');

-- ----------------------------------------------------------
-- ChiTietDonHang
-- ----------------------------------------------------------
INSERT INTO ChiTietDonHang (MaDH, MaSerialSP, GiaBan, PhanTramGiam) VALUES
-- DH00000001
('DH00000001','ASUS-VB-001', 16990000, NULL),
-- DH00000002 (Black Friday -5% laptop)
('DH00000002','HP-PAV-001',  17490000, 5.00),
-- DH00000003
('DH00000003','DELL-INS-001',17990000, NULL),
-- DH00000004 (3 ThinkPad)
('DH00000004','LNVO-TP-001', 28990000, NULL),
('DH00000004','LNVO-TP-002', 28990000, NULL),
-- DH00000005 (BTS -10% laptop HSSV)
('DH00000005','ASUS-VB-002', 16990000, 10.00),
-- DH00000006
('DH00000006','APPL-MBA-001',28990000, NULL),
-- DH00000007 (2 Dell XPS)
('DH00000007','DELL-XPS-001',38990000, NULL),
-- DH00000008
('DH00000008','ASUS-ROG-001',42990000, NULL),
-- DH00000009 (HP Pavilion + HP KB)
('DH00000009','HP-PAV-003',  17490000, NULL), -- HP-PAV-003 thay cho HP-PAV-002 để khớp với PhieuBaoHanh
('DH00000009','HP-KB-001',     490000, NULL),
-- DH00000010 (3 MBA + 2 Magic Mouse, KM0003 -5% trên DH)
('DH00000010','APPL-MBA-002',28990000, NULL),
('DH00000010','APPL-MBA-003',28990000, NULL),
('DH00000010','APPL-MM-001',  2490000, NULL),
('DH00000010','APPL-MM-002',  2490000, NULL),
-- DH00000011
('DH00000011','ASUS-VB-003', 16990000, NULL),
-- DH00000012
('DH00000012','LOGI-MX-001',  2390000, NULL);

-- ----------------------------------------------------------
-- DonKhieuNai
-- ----------------------------------------------------------
INSERT INTO DonKhieuNai (MaDonKN, MaDH, MaKH, NoiDung, NgayGui, TrangThai, KetQua) VALUES
('KN00000001', 'DH00000003', 'KH00000002',
    N'Laptop nhận được bị trầy xước mặt trước, không phải lỗi sử dụng.',
    '2025-12-08', N'Đã Giải Quyết',
    N'Đã kiểm tra xác nhận lỗi từ vận chuyển. Hỗ trợ giảm 200,000 VNĐ cho đơn tiếp theo.'),
('KN00000002', 'DH00000009', 'KH00000006',
    N'Bàn phím mua về không nhận được kết nối bluetooth với laptop HP.',
    '2026-02-15', N'Đang Xử Lý', NULL);

-- ----------------------------------------------------------
-- PhieuBaoHanh
-- ----------------------------------------------------------
INSERT INTO PhieuBaoHanh (MaPBH, MaDH, MaKH, MaSerialSP, LoaiBH, TrangThai, NgayBatDau, NgayKetThuc, LyDoLoi, KetQua) VALUES
-- KH00000003 bảo hành laptop mua qua DH00000005 (BTS)
('PBH0000001','DH00000005','KH00000003','ASUS-VB-002', N'Cửa Hàng', N'Hoàn Thành',
    '2026-01-10','2026-02-10', N'Máy quá nóng, quạt kêu to',
    N'Đã thay thế quạt tản nhiệt. Máy hoạt động bình thường.'),
-- KH00000002 bảo hành HP Pavilion (DH00000009 - HP-PAV-002 -> Ghi nhầm, thực tế DH00000009 KH00000006)
-- Dùng HP-PAV-003 hiện trạng thái Bảo Hành
('PBH0000002','DH00000009','KH00000006','HP-PAV-003', N'Cửa Hàng', N'Đang Xử Lý',
    '2026-04-01','2027-04-01', N'Không lên nguồn', NULL),
-- KH00000004 MacBook Air bảo hành tại hãng (khách lẻ mua MacBook - bảo hành cửa hàng)
('PBH0000003','DH00000006','KH00000004','APPL-MBA-001', N'Cửa Hàng', N'Đang Xử Lý',
    '2026-05-01','2027-05-01', N'Màn hình chập chờn', NULL);

-- Cập nhật trạng thái HP-PAV-003 đang bảo hành (đã set sẵn trong SanPham)

-- ----------------------------------------------------------
-- PhieuDoiTra
-- ----------------------------------------------------------
INSERT INTO PhieuDoiTra (MaPhieuDT, MaDH, MaSerialSP, MaKH, NgayYeuCau, LyDo, LoaiXuLy, TrangThai) VALUES
('PDT0000001','DH00000001','ASUS-VB-001','KH00000001',
    '2025-12-05',
    N'Màn hình laptop bị đốm đen sau 15 ngày sử dụng - lỗi nhà sản xuất.',
    N'Đổi Máy', N'Hoàn Thành'),
('PDT0000002','DH00000008','ASUS-ROG-001','KH00000005',
    '2026-02-01',
    N'Bàn phím laptop bị liệt một số phím sau 17 ngày - lỗi nhà sản xuất.',
    N'Đổi Máy', N'Đang Xử Lý');

-- Cập nhật serial đã đổi trả thành trạng thái Đổi Trả
UPDATE SanPham SET TrangThai = N'Đổi Trả' WHERE MaSerialSP = 'ASUS-VB-001';
UPDATE SanPham SET TrangThai = N'Đổi Trả' WHERE MaSerialSP = 'ASUS-ROG-001';

GO

-- ============================================================
-- PHẦN 3: CÁC VIEW HỮU ÍCH
-- ============================================================

-- ----------------------------------------------------------
-- View: Tồn kho theo loại sản phẩm
-- ----------------------------------------------------------
CREATE VIEW vw_TonKho AS
SELECT
    lsp.MaLoaiSP,
    lsp.TenLoai,
    lsp.DanhMuc,
    h.TenHang,
    lsp.GiaBanGoc,
    COUNT(sp.MaSerialSP) AS TongSoLuong,
    SUM(CASE WHEN sp.TrangThai = N'Trong Kho' THEN 1 ELSE 0 END) AS SoLuongTonKho,
    SUM(CASE WHEN sp.TrangThai = N'Đã Bán'    THEN 1 ELSE 0 END) AS SoLuongDaBan,
    SUM(CASE WHEN sp.TrangThai = N'Bảo Hành'  THEN 1 ELSE 0 END) AS SoLuongBaoHanh
FROM LoaiSanPham lsp
JOIN HangSanXuat h  ON h.MaHang    = lsp.MaHang
LEFT JOIN SanPham sp ON sp.MaLoaiSP = lsp.MaLoaiSP
GROUP BY lsp.MaLoaiSP, lsp.TenLoai, lsp.DanhMuc, h.TenHang, lsp.GiaBanGoc;
GO

-- ----------------------------------------------------------
-- View: Doanh thu theo tháng
-- ----------------------------------------------------------
CREATE VIEW vw_DoanhThuTheoThang AS
SELECT
    YEAR(NgayDat)  AS Nam,
    MONTH(NgayDat) AS Thang,
    COUNT(MaDH)    AS SoDonHang,
    SUM(TongTien)  AS TongDoanhThu,
    SUM(ISNULL(TienSauGiam, TongTien)) AS DoanhThuSauGiam
FROM DonHang
WHERE TrangThai = N'Hoàn Thành'
GROUP BY YEAR(NgayDat), MONTH(NgayDat);
GO

-- ----------------------------------------------------------
-- View: Chi tiết đơn hàng đầy đủ
-- ----------------------------------------------------------
CREATE VIEW vw_ChiTietDonHangDayDu AS
SELECT
    dh.MaDH,
    dh.NgayDat,
    kh.TenKH,
    kh.LoaiKH,
    nv.TenNV    AS NhanVienPhuTrach,
    lsp.TenLoai AS TenSanPham,
    lsp.DanhMuc,
    h.TenHang,
    ctdh.MaSerialSP,
    ctdh.GiaBan,
    ISNULL(ctdh.PhanTramGiam, 0)          AS PhanTramGiam,
    ctdh.GiaBan * (1 - ISNULL(ctdh.PhanTramGiam,0)/100.0) AS ThanhTien,
    dh.TrangThai
FROM DonHang dh
JOIN KhachHang       kh   ON kh.MaKH       = dh.MaKH
JOIN NhanVien        nv   ON nv.MaNV        = dh.MaNV
JOIN ChiTietDonHang  ctdh ON ctdh.MaDH      = dh.MaDH
JOIN SanPham         sp   ON sp.MaSerialSP  = ctdh.MaSerialSP
JOIN LoaiSanPham     lsp  ON lsp.MaLoaiSP   = sp.MaLoaiSP
JOIN HangSanXuat     h    ON h.MaHang       = lsp.MaHang;
GO

-- ============================================================
-- PHẦN 4: STORED PROCEDURES
-- ============================================================

-- SP: Xem tồn kho hiện tại
CREATE PROCEDURE sp_BaoCaoTonKho
    @DanhMuc NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM vw_TonKho
    WHERE (@DanhMuc IS NULL OR DanhMuc = @DanhMuc)
    ORDER BY TenHang, DanhMuc, TenLoai;
END;
GO

-- SP: Báo cáo doanh thu theo khoảng thời gian
CREATE PROCEDURE sp_BaoCaoDoanhThu
    @TuNgay DATE,
    @DenNgay DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        dh.MaDH,
        dh.NgayDat,
        kh.TenKH,
        kh.LoaiKH,
        dh.TongTien,
        ISNULL(dh.TienSauGiam, dh.TongTien) AS ThucThu,
        km.TenKM AS KhuyenMaiApDung,
        dh.PhuongThucThanhToan,
        dh.TrangThai
    FROM DonHang dh
    JOIN KhachHang kh    ON kh.MaKH = dh.MaKH
    LEFT JOIN KhuyenMai km ON km.MaKM = dh.MaKM
    WHERE CAST(dh.NgayDat AS DATE) BETWEEN @TuNgay AND @DenNgay
      AND dh.TrangThai = N'Hoàn Thành'
    ORDER BY dh.NgayDat;
END;
GO

-- SP: Tạo đơn hàng lẻ
CREATE PROCEDURE sp_TaoDonHangLe
    @MaDH     CHAR(10),
    @MaNV     CHAR(10),
    @MaKH     CHAR(10),
    @MaKM     CHAR(10) = NULL,
    @PTTT     NVARCHAR(100),
    @Serials  NVARCHAR(MAX)   -- Danh sách serial cách nhau dấu phẩy
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Tính tổng tiền
        DECLARE @TongTien DECIMAL(15,2) = 0;

        SELECT @TongTien = SUM(lsp.GiaBanGoc)
        FROM SanPham sp
        JOIN LoaiSanPham lsp ON lsp.MaLoaiSP = sp.MaLoaiSP
        WHERE CHARINDEX(sp.MaSerialSP, @Serials) > 0
          AND sp.TrangThai = N'Trong Kho';

        INSERT INTO DonHang (MaDH, MaNV, MaKH, MaKM, NgayDat, TongTien, TienSauGiam, PhuongThucThanhToan, TrangThai)
        VALUES (@MaDH, @MaNV, @MaKH, @MaKM, GETDATE(), @TongTien, @TongTien, @PTTT, N'Chờ Xử Lý');

        -- Cập nhật trạng thái sản phẩm
        UPDATE SanPham
        SET TrangThai = N'Đã Bán'
        WHERE CHARINDEX(MaSerialSP, @Serials) > 0
          AND TrangThai = N'Trong Kho';

        COMMIT TRANSACTION;
        SELECT N'Tạo đơn hàng thành công' AS KetQua, @MaDH AS MaDH, @TongTien AS TongTien;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT ERROR_MESSAGE() AS LoiXayRa;
    END CATCH;
END;
GO

-- ============================================================
-- PHẦN 5: KIỂM TRA DỮ LIỆU
-- ============================================================

SELECT N'=== THỐNG KÊ DỮ LIỆU ===' AS ThongBao;

SELECT 'VaiTro'           AS TenBang, COUNT(*) AS SoBanGhi FROM VaiTro           UNION ALL
SELECT 'NhanVien'         ,           COUNT(*)              FROM NhanVien         UNION ALL
SELECT 'TaiKhoanNV'       ,           COUNT(*)              FROM TaiKhoanNV       UNION ALL
SELECT 'KhachHang'        ,           COUNT(*)              FROM KhachHang        UNION ALL
SELECT 'HangSanXuat'      ,           COUNT(*)              FROM HangSanXuat      UNION ALL
SELECT 'LoaiSanPham'      ,           COUNT(*)              FROM LoaiSanPham      UNION ALL
SELECT 'CauHinh'          ,           COUNT(*)              FROM CauHinh          UNION ALL
SELECT 'NhaCungCap'       ,           COUNT(*)              FROM NhaCungCap       UNION ALL
SELECT 'PhieuNhap'        ,           COUNT(*)              FROM PhieuNhap        UNION ALL
SELECT 'ChiTietPhieuNhap' ,           COUNT(*)              FROM ChiTietPhieuNhap UNION ALL
SELECT 'SanPham'          ,           COUNT(*)              FROM SanPham          UNION ALL
SELECT 'KhuyenMai'        ,           COUNT(*)              FROM KhuyenMai        UNION ALL
SELECT 'HopDong'          ,           COUNT(*)              FROM HopDong          UNION ALL
SELECT 'DonHang'          ,           COUNT(*)              FROM DonHang          UNION ALL
SELECT 'ChiTietDonHang'   ,           COUNT(*)              FROM ChiTietDonHang   UNION ALL
SELECT 'DonKhieuNai'      ,           COUNT(*)              FROM DonKhieuNai      UNION ALL
SELECT 'PhieuBaoHanh'     ,           COUNT(*)              FROM PhieuBaoHanh     UNION ALL
SELECT 'PhieuDoiTra'      ,           COUNT(*)              FROM PhieuDoiTra;

-- Kiểm tra tồn kho
SELECT N'=== TỒN KHO HIỆN TẠI ===' AS ThongBao;
EXEC sp_BaoCaoTonKho;

-- Doanh thu từ đầu năm 2026
SELECT N'=== DOANH THU 2026 ===' AS ThongBao;
EXEC sp_BaoCaoDoanhThu '2026-01-01', '2026-12-31';

GO
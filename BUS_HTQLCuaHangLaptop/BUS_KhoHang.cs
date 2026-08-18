using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using DAL_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace BUS_HTQLCuaHangLaptop
{
    /// Lớp nghiệp vụ quản lý kho hàng: NhaCungCap, PhieuNhap, ChiTietPhieuNhap, SanPham (serial).
    /// Chịu trách nhiệm: nhập kho (transaction 3 bảng), xác nhận / hủy phiếu nhập,
    /// quản lý nhà cung cấp, và báo cáo tồn kho theo loại sản phẩm.
    public class BUS_KhoHang
    {
        // Khai báo các DAL liên quan
        private readonly DAL_NhaCungCap       _dalNCC   = new DAL_NhaCungCap();
        private readonly DAL_PhieuNhap        _dalPN    = new DAL_PhieuNhap();
        private readonly DAL_ChiTietPhieuNhap _dalCTPN  = new DAL_ChiTietPhieuNhap();
        private readonly DAL_SanPham          _dalSP    = new DAL_SanPham();

        // Trạng thái phiếu nhập hợp lệ
        private static readonly string[] TRANG_THAI_PHIEU_NHAP = { "Chờ Xác Nhận", "Đã Nhập", "Huỷ" };


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 1: NHÀ CUNG CẤP (NhaCungCap)
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ nhà cung cấp chưa bị xóa mềm.
        /// Trả về DataTable chứa danh sách nhà cung cấp.
        public DataTable LayDanhSachNCC()
        {
            try
            {
                return _dalNCC.DSTatCaNCC();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách nhà cung cấp: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin nhà cung cấp theo mã.
        /// <param name="maNCC">Mã nhà cung cấp.
        /// Trả về DTO_NhaCungCap nếu tìm thấy, null nếu không tồn tại.
        public DTO_NhaCungCap? LayNCCTheoMa(string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maNCC))
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");
            try
            {
                return _dalNCC.DSTheoMaNCC(maNCC.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin nhà cung cấp: {ex.Message}", ex);
            }
        }

        /// Thêm nhà cung cấp mới.
        /// <param name="ncc">DTO chứa thông tin nhà cung cấp.
        /// Trả về True nếu thêm thành công.
        public bool ThemNCC(DTO_NhaCungCap ncc)
        {
            KiemTraHopLeNCC(ncc);
            ncc.IsDeleted = false;
            try
            {
                return _dalNCC.ThemNCC(ncc);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm nhà cung cấp: {ex.Message}", ex);
            }
        }

        /// Cập nhật thông tin nhà cung cấp (TenNCC, Email, SDT, DiaChi).
        /// <param name="ncc">DTO chứa thông tin cần cập nhật. MaNCC bắt buộc.
        /// Trả về True nếu cập nhật thành công.
        public bool CapNhatNCC(DTO_NhaCungCap ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.MaNCC))
                throw new ArgumentException("Mã nhà cung cấp không được để trống khi cập nhật.");

            var existing = _dalNCC.DSTheoMaNCC(ncc.MaNCC.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Nhà cung cấp '{ncc.MaNCC}' không tồn tại hoặc đã bị xóa.");

            KiemTraHopLeNCC(ncc);
            ncc.NgayCapNhat = DateTime.Now;
            try
            {
                return _dalNCC.CapNhatNCC(ncc);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật nhà cung cấp: {ex.Message}", ex);
            }
        }

        /// Xóa mềm nhà cung cấp (IsDeleted = 1).
        /// Không được xóa nếu còn phiếu nhập đang 'Chờ Xác Nhận' từ NCC đó.
        /// <param name="maNCC">Mã nhà cung cấp cần xóa mềm.
        /// Trả về True nếu xóa mềm thành công.
        public bool XoaNCC(string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maNCC))
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");

            var existing = _dalNCC.DSTheoMaNCC(maNCC.Trim());
            if (existing == null)
                throw new InvalidOperationException($"Nhà cung cấp '{maNCC}' không tồn tại hoặc đã bị xóa trước đó.");

            // Ràng buộc: còn phiếu nhập đang 'Chờ Xác Nhận' → không cho xóa
            var dsPN = _dalPN.DSTheoNhaCungCap(maNCC.Trim());
            int soPhieuChoXacNhan = 0;
            foreach (DataRow row in dsPN.Rows)
            {
                if (row["TrangThai"].ToString() == "Chờ Xác Nhận")
                    soPhieuChoXacNhan++;
            }
            if (soPhieuChoXacNhan > 0)
                throw new InvalidOperationException(
                    $"Không thể xóa nhà cung cấp '{existing.TenNCC}' vì còn {soPhieuChoXacNhan} phiếu nhập " +
                    "đang ở trạng thái 'Chờ Xác Nhận'. Hãy xử lý hết phiếu trước.");

            try
            {
                return _dalNCC.XoaMemNCC(maNCC.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa mềm nhà cung cấp: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 2: PHIẾU NHẬP — TẠO MỚI (TRANSACTION 3 BẢNG)
        // ══════════════════════════════════════════════════════════════════

        /// Tạo phiếu nhập mới — INSERT PhieuNhap + ChiTietPhieuNhap + SanPham (serials) trong 1 transaction.
        /// TongTien được tính tự động = SUM(SoLuong × GiaNhap) — không nhận từ caller.
        /// TrangThai PhieuNhap mặc định = 'Chờ Xác Nhận'.
        /// TrangThai mỗi SanPham (serial) = 'Trong Kho' khi phiếu đã ở trạng thái 'Đã Nhập',
        /// nhưng khi mới tạo ('Chờ Xác Nhận') thì chưa thêm serial vào bảng SanPham.
        ///
        /// <param name="pn">DTO_PhieuNhap (MaPhieuNhap, MaNV, MaNCC). TongTien sẽ tự tính.
        /// <param name="danhSachCTPN">Danh sách chi tiết (MaLoaiSP, SoLuong, GiaNhap).
        /// <param name="danhSachSerial">Danh sách DTO_SanPham tương ứng để insert vào bảng SanPham.
        /// <param name="maTKNguoiTao">Mã tài khoản nhân viên tạo phiếu.
        /// Trả về True nếu tạo thành công.
        public bool TaoPhieuNhap(
            DTO_PhieuNhap pn,
            List<DTO_ChiTietPhieuNhap> danhSachCTPN,
            List<DTO_SanPham> danhSachSerial,
            string? maTKNguoiTao = null)
        {
            // Validate phiếu nhập
            if (string.IsNullOrWhiteSpace(pn.MaPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");
            if (string.IsNullOrWhiteSpace(pn.MaNV))
                throw new ArgumentException("Mã nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(pn.MaNCC))
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");

            // Kiểm tra NCC tồn tại
            var ncc = _dalNCC.DSTheoMaNCC(pn.MaNCC.Trim());
            if (ncc == null)
                throw new InvalidOperationException($"Nhà cung cấp '{pn.MaNCC}' không tồn tại hoặc đã bị xóa.");

            // Validate danh sách chi tiết
            if (danhSachCTPN == null || danhSachCTPN.Count == 0)
                throw new ArgumentException("Phiếu nhập phải có ít nhất một dòng chi tiết.");

            // Validate từng dòng CTPN
            foreach (var ctpn in danhSachCTPN)
                KiemTraHopLeCTPN(ctpn);

            // Validate danh sách serial
            if (danhSachSerial == null || danhSachSerial.Count == 0)
                throw new ArgumentException("Danh sách serial sản phẩm không được rỗng.");

            foreach (var sp in danhSachSerial)
            {
                if (string.IsNullOrWhiteSpace(sp.MaSerialSP))
                    throw new ArgumentException("Serial sản phẩm không được để trống.");
                if (sp.MaSerialSP.Length > 50)
                    throw new ArgumentException($"Serial '{sp.MaSerialSP}' không được vượt quá 50 ký tự.");
                // Serial không được trùng nhau trong chính danh sách này
            }

            // Tính TongTien = SUM(SoLuong × GiaNhap)
            decimal tongTien = 0;
            foreach (var ctpn in danhSachCTPN)
                tongTien += ctpn.SoLuong * ctpn.GiaNhap;

            if (tongTien < 0)
                throw new ArgumentException("Tổng tiền phiếu nhập không được âm.");

            // Gán thông tin phiếu
            pn.TongTien   = tongTien;
            pn.TrangThai  = "Chờ Xác Nhận";
            pn.NgayTao    = DateTime.Now;
            pn.NguoiTao   = maTKNguoiTao?.Trim();
            pn.MaPhieuNhap = pn.MaPhieuNhap.Trim();

            // Thực thi transaction
            string connStr = ConfigurationManager
                .ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;

            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Bước 1: INSERT PhieuNhap
                string sqlPN = "INSERT INTO PhieuNhap (MaPhieuNhap, MaNV, MaNCC, TongTien, TrangThai, NgayCapNhat, NguoiTao) " +
                               "VALUES (@MaPhieuNhap, @MaNV, @MaNCC, @TongTien, @TrangThai, @NgayCapNhat, @NguoiTao)";
                using (SqlCommand cmd = new SqlCommand(sqlPN, conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10)    { Value = pn.MaPhieuNhap });
                    cmd.Parameters.Add(new SqlParameter("@MaNV",        SqlDbType.Char, 10)    { Value = pn.MaNV });
                    cmd.Parameters.Add(new SqlParameter("@MaNCC",       SqlDbType.Char, 10)    { Value = pn.MaNCC });
                    cmd.Parameters.Add(new SqlParameter("@TongTien",    SqlDbType.Decimal)     { Value = pn.TongTien, Precision = 15, Scale = 2 });
                    cmd.Parameters.Add(new SqlParameter("@TrangThai",   SqlDbType.NVarChar, 50){ Value = pn.TrangThai });
                    cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime)    { Value = (object?)pn.NgayCapNhat ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@NguoiTao",    SqlDbType.Char, 10)    { Value = string.IsNullOrEmpty(pn.NguoiTao) ? (object)DBNull.Value : pn.NguoiTao });
                    cmd.ExecuteNonQuery();
                }

                // Bước 2: INSERT từng dòng ChiTietPhieuNhap
                foreach (var ctpn in danhSachCTPN)
                {
                    string sqlCTPN = "INSERT INTO ChiTietPhieuNhap (MaLoaiSP, MaPhieuNhap, SoLuong, GiaNhap) " +
                                     "VALUES (@MaLoaiSP, @MaPhieuNhap, @SoLuong, @GiaNhap)";
                    using SqlCommand cmd = new SqlCommand(sqlCTPN, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaLoaiSP",    SqlDbType.Char, 10) { Value = ctpn.MaLoaiSP });
                    cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = pn.MaPhieuNhap });
                    cmd.Parameters.Add(new SqlParameter("@SoLuong",     SqlDbType.Int)      { Value = ctpn.SoLuong });
                    cmd.Parameters.Add(new SqlParameter("@GiaNhap",     SqlDbType.Decimal)  { Value = ctpn.GiaNhap, Precision = 15, Scale = 2 });
                    cmd.ExecuteNonQuery();
                }

                // Bước 3: INSERT từng serial vào SanPham (TrangThai = 'Trong Kho')
                foreach (var sp in danhSachSerial)
                {
                    string sqlSP = "INSERT INTO SanPham (MaSerialSP, MaPhieuNhap, MaLoaiSP, NgayNhap, NgaySX, TrangThai, NgayTao, NgayCapNhat, IsDeleted) " +
                                   "VALUES (@MaSerialSP, @MaPhieuNhap, @MaLoaiSP, @NgayNhap, @NgaySX, N'Trong Kho', @NgayTao, @NgayCapNhat, 1)";
                    using SqlCommand cmd = new SqlCommand(sqlSP, conn, tran);
                    cmd.Parameters.Add(new SqlParameter("@MaSerialSP",  SqlDbType.VarChar, 50) { Value = sp.MaSerialSP.Trim() });
                    cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10)    { Value = pn.MaPhieuNhap });
                    cmd.Parameters.Add(new SqlParameter("@MaLoaiSP",    SqlDbType.Char, 10)    { Value = sp.MaLoaiSP });
                    cmd.Parameters.Add(new SqlParameter("@NgayNhap",    SqlDbType.Date)        { Value = sp.NgayNhap == default(DateTime) ? DateTime.Today : sp.NgayNhap });
                    cmd.Parameters.Add(new SqlParameter("@NgaySX",      SqlDbType.Date)        { Value = sp.NgaySX.HasValue ? (object)sp.NgaySX.Value : DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@NgayTao",     SqlDbType.DateTime)    { Value = DateTime.Now });
                    cmd.Parameters.Add(new SqlParameter("@NgayCapNhat", SqlDbType.DateTime)    { Value = DBNull.Value });
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 3: PHIẾU NHẬP — XÁC NHẬN / HỦY
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách toàn bộ phiếu nhập.
        /// Trả về DataTable chứa danh sách phiếu nhập.
        public DataTable LayDanhSachPhieuNhap()
        {
            try
            {
                return _dalPN.DSTatCaPhieuNhap();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách phiếu nhập: {ex.Message}", ex);
            }
        }

        /// Lấy thông tin phiếu nhập theo mã.
        /// <param name="maPhieuNhap">Mã phiếu nhập.
        /// Trả về DTO_PhieuNhap nếu tìm thấy, null nếu không tồn tại.
        public DTO_PhieuNhap? LayPhieuNhapTheoMa(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");
            try
            {
                return _dalPN.DSTheoMaPhieuNhap(maPhieuNhap.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin phiếu nhập: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách chi tiết phiếu nhập (kèm TenLoai, DanhMuc, TenHang, ThanhTien).
        /// <param name="maPhieuNhap">Mã phiếu nhập.
        /// Trả về DataTable chứa chi tiết phiếu nhập.
        public DataTable LayChiTietPhieuNhap(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");
            try
            {
                return _dalCTPN.DSChiTietCoThongTinSanPham(maPhieuNhap.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách serial thuộc phiếu nhập.
        public DataTable LayDanhSachSerialTheoPhieuNhap(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");
            try
            {
                return _dalSP.DSTheoPhieuNhap(maPhieuNhap.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách serial theo phiếu nhập: {ex.Message}", ex);
            }
        }

        /// Xác nhận phiếu nhập — chuyển TrangThai từ 'Chờ Xác Nhận' sang 'Đã Nhập'.
        /// Khi xác nhận: cập nhật TrangThai các SanPham thuộc phiếu thành 'Trong Kho'.
        /// <param name="maPhieuNhap">Mã phiếu nhập cần xác nhận.
        /// Trả về True nếu xác nhận thành công.
        public bool XacNhanPhieuNhap(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");

            var pn = _dalPN.DSTheoMaPhieuNhap(maPhieuNhap.Trim());
            if (pn == null)
                throw new InvalidOperationException($"Phiếu nhập '{maPhieuNhap}' không tồn tại.");
            if (pn.TrangThai != "Chờ Xác Nhận")
                throw new InvalidOperationException(
                    $"Phiếu nhập '{maPhieuNhap}' đang ở trạng thái '{pn.TrangThai}'. " +
                    "Chỉ có thể xác nhận phiếu đang ở 'Chờ Xác Nhận'.");

            // Lấy danh sách serial thuộc phiếu này
            var dsSP = _dalSP.DSTheoPhieuNhap(maPhieuNhap.Trim());

            string connStr = ConfigurationManager
                .ConnectionStrings["QuanLyCuaHangLaptop"].ConnectionString;

            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Bước 1: Cập nhật TrangThai PhieuNhap → 'Đã Nhập'
                string sqlPN = "UPDATE PhieuNhap SET TrangThai = N'Đã Nhập', NgayCapNhat = GETDATE() WHERE MaPhieuNhap = @MaPhieuNhap";
                using (SqlCommand cmd = new SqlCommand(sqlPN, conn, tran))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaPhieuNhap", SqlDbType.Char, 10) { Value = maPhieuNhap.Trim() });
                    cmd.ExecuteNonQuery();
                }

                // Bước 2: Cập nhật TrangThai các SanPham → 'Trong Kho' hoặc swap nếu có đơn đặt hàng trước (virtual serial)
                foreach (DataRow row in dsSP.Rows)
                {
                    string maSerial = row["MaSerialSP"].ToString()!.Trim();
                    string maLoaiSP = row["MaLoaiSP"].ToString()!.Trim();

                    // Check if there is a pending virtual serial for this MaLoaiSP
                    string sqlFindVirtual = "SELECT TOP 1 ct.MaSerialSP, ct.MaDH FROM ChiTietDonHang ct " +
                                            "INNER JOIN SanPham sp ON ct.MaSerialSP = sp.MaSerialSP " +
                                            "WHERE sp.MaLoaiSP = @MaLoaiSP AND ct.MaSerialSP LIKE 'x-%'";
                    string? virtualSerial = null;
                    string? maDH = null;
                    using (SqlCommand cmdFind = new SqlCommand(sqlFindVirtual, conn, tran))
                    {
                        cmdFind.Parameters.Add(new SqlParameter("@MaLoaiSP", SqlDbType.Char, 10) { Value = maLoaiSP });
                        using (SqlDataReader reader = cmdFind.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                virtualSerial = reader["MaSerialSP"].ToString()!.Trim();
                                maDH = reader["MaDH"].ToString()!.Trim();
                            }
                        }
                    }

                    if (virtualSerial != null && maDH != null)
                    {
                        // 1. UPDATE TrangThai of the actual product to N'Đã Bán' and set IsDeleted = 0
                        string sqlUpdateActual = "UPDATE SanPham SET TrangThai = N'Đã Bán', IsDeleted = 0, NgayCapNhat = GETDATE() " +
                                                 "WHERE MaSerialSP = @MaSerialSP";
                        using (SqlCommand cmdAct = new SqlCommand(sqlUpdateActual, conn, tran))
                        {
                            cmdAct.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });
                            cmdAct.ExecuteNonQuery();
                        }

                        // 2. UPDATE ChiTietDonHang.MaSerialSP to the new actual serial
                        string sqlUpdateCT = "UPDATE ChiTietDonHang SET MaSerialSP = @MaSerialSP " +
                                             "WHERE MaDH = @MaDH AND MaSerialSP = @VirtualSerial";
                        using (SqlCommand cmdCT = new SqlCommand(sqlUpdateCT, conn, tran))
                        {
                            cmdCT.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });
                            cmdCT.Parameters.Add(new SqlParameter("@MaDH", SqlDbType.Char, 10) { Value = maDH });
                            cmdCT.Parameters.Add(new SqlParameter("@VirtualSerial", SqlDbType.VarChar, 50) { Value = virtualSerial });
                            cmdCT.ExecuteNonQuery();
                        }

                        // 3. DELETE the temporary virtual serial from SanPham table
                        string sqlDeleteVirtual = "DELETE FROM SanPham WHERE MaSerialSP = @VirtualSerial";
                        using (SqlCommand cmdDel = new SqlCommand(sqlDeleteVirtual, conn, tran))
                        {
                            cmdDel.Parameters.Add(new SqlParameter("@VirtualSerial", SqlDbType.VarChar, 50) { Value = virtualSerial });
                            cmdDel.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // No virtual serial pending, set status to N'Trong Kho' and set IsDeleted = 0
                        string sqlSP = "UPDATE SanPham SET TrangThai = N'Trong Kho', IsDeleted = 0, NgayCapNhat = GETDATE() " +
                                       "WHERE MaSerialSP = @MaSerialSP";
                        using (SqlCommand cmd = new SqlCommand(sqlSP, conn, tran))
                        {
                            cmd.Parameters.Add(new SqlParameter("@MaSerialSP", SqlDbType.VarChar, 50) { Value = maSerial });
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        /// Hủy phiếu nhập — chuyển TrangThai sang 'Huỷ'.
        /// Chỉ hủy được khi phiếu đang ở 'Chờ Xác Nhận'. Không hủy khi đã 'Đã Nhập'.
        /// <param name="maPhieuNhap">Mã phiếu nhập cần hủy.
        /// Trả về True nếu hủy thành công.
        public bool HuyPhieuNhap(string maPhieuNhap)
        {
            if (string.IsNullOrWhiteSpace(maPhieuNhap))
                throw new ArgumentException("Mã phiếu nhập không được để trống.");

            var pn = _dalPN.DSTheoMaPhieuNhap(maPhieuNhap.Trim());
            if (pn == null)
                throw new InvalidOperationException($"Phiếu nhập '{maPhieuNhap}' không tồn tại.");

            if (pn.TrangThai == "Đã Nhập")
                throw new InvalidOperationException(
                    $"Phiếu nhập '{maPhieuNhap}' đã được xác nhận ('Đã Nhập'). Không thể hủy phiếu đã nhập kho.");
            if (pn.TrangThai == "Huỷ")
                throw new InvalidOperationException($"Phiếu nhập '{maPhieuNhap}' đã bị hủy trước đó.");

            try
            {
                return _dalPN.CapNhatTrangThai(maPhieuNhap.Trim(), "Huỷ");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi hủy phiếu nhập: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 4: BÁO CÁO TỒN KHO
        // ══════════════════════════════════════════════════════════════════

        /// Lấy danh sách tất cả serial đang tồn kho (TrangThai = 'Trong Kho').
        /// Trả về DataTable chứa danh sách serial tồn kho.
        public DataTable BaoCaoTonKho()
        {
            try
            {
                return _dalSP.DSTheoTrangThai("Trong Kho");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy báo cáo tồn kho: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách sản phẩm tồn kho của một loại sản phẩm cụ thể.
        /// <param name="maLoaiSP">Mã loại sản phẩm cần xem tồn kho.
        /// Trả về DataTable chứa serial còn trong kho.
        public DataTable TonKhoTheoLoaiSP(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm không được để trống.");
            try
            {
                // Lấy tất cả serial theo loại SP, sau đó lọc chỉ lấy Trong Kho
                var dsSP = _dalSP.DSTheoLoaiSP(maLoaiSP.Trim());
                var dt   = dsSP.Clone(); // giữ cấu trúc cột
                foreach (DataRow row in dsSP.Rows)
                    if (row["TrangThai"].ToString() == "Trong Kho")
                        dt.ImportRow(row);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tồn kho theo loại sản phẩm: {ex.Message}", ex);
            }
        }

        /// Thống kê số lượng sản phẩm theo trạng thái cho từng loại sản phẩm.
        /// Trả về DataTable với cột: MaLoaiSP, TrangThai, SoLuong.
        /// Dùng để hiển thị bảng báo cáo tổng hợp tồn kho trên WinForm/MVC.
        public DataTable ThongKeTonKhoTheoLoaiSP()
        {
            try
            {
                // Lấy toàn bộ sản phẩm chưa bị xóa
                var dsTatCa = _dalSP.DSTatCaSanPham();

                // Tổng hợp thủ công theo (MaLoaiSP, TrangThai)
                var result = new DataTable();
                result.Columns.Add("MaLoaiSP", typeof(string));
                result.Columns.Add("TrangThai", typeof(string));
                result.Columns.Add("SoLuong",  typeof(int));

                var dict = new Dictionary<string, int>();
                foreach (DataRow row in dsTatCa.Rows)
                {
                    string maLoai   = row["MaLoaiSP"].ToString()!.Trim();
                    string trangThai = row["TrangThai"].ToString()!;
                    string key      = $"{maLoai}|{trangThai}";

                    if (!dict.ContainsKey(key))
                        dict[key] = 0;
                    dict[key]++;
                }

                foreach (var kv in dict)
                {
                    var parts = kv.Key.Split('|');
                    result.Rows.Add(parts[0], parts[1], kv.Value);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thống kê tồn kho: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách phiếu nhập theo nhà cung cấp.
        /// <param name="maNCC">Mã nhà cung cấp.
        /// Trả về DataTable chứa danh sách phiếu nhập.
        public DataTable LayPhieuNhapTheoNCC(string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maNCC))
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");
            try
            {
                return _dalPN.DSTheoNhaCungCap(maNCC.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy phiếu nhập theo nhà cung cấp: {ex.Message}", ex);
            }
        }


        // ══════════════════════════════════════════════════════════════════
        // PHẦN 5: KIỂM TRA DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════

        /// Kiểm tra thông tin nhà cung cấp hợp lệ.
        public void KiemTraHopLeNCC(DTO_NhaCungCap ncc)
        {
            if (ncc == null)
                throw new ArgumentNullException(nameof(ncc), "Thông tin nhà cung cấp không được null.");

            if (string.IsNullOrWhiteSpace(ncc.TenNCC))
                throw new ArgumentException("Tên nhà cung cấp không được để trống.");
            if (ncc.TenNCC.Length > 200)
                throw new ArgumentException("Tên nhà cung cấp không được vượt quá 200 ký tự.");

            if (!string.IsNullOrEmpty(ncc.Email) && !Regex.IsMatch(ncc.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Địa chỉ Email nhà cung cấp không đúng định dạng.");

            if (!string.IsNullOrEmpty(ncc.SDT) && !Regex.IsMatch(ncc.SDT, @"^\d{10}$"))
                throw new ArgumentException("Số điện thoại nhà cung cấp phải gồm đúng 10 chữ số.");
        }

        /// Kiểm tra thông tin dòng chi tiết phiếu nhập hợp lệ.
        public void KiemTraHopLeCTPN(DTO_ChiTietPhieuNhap ctpn)
        {
            if (ctpn == null)
                throw new ArgumentNullException(nameof(ctpn), "Chi tiết phiếu nhập không được null.");
            if (string.IsNullOrWhiteSpace(ctpn.MaLoaiSP))
                throw new ArgumentException("Mã loại sản phẩm trong chi tiết phiếu nhập không được để trống.");
            if (ctpn.SoLuong <= 0)
                throw new ArgumentException($"Số lượng sản phẩm '{ctpn.MaLoaiSP}' phải lớn hơn 0.");
            if (ctpn.GiaNhap < 0)
                throw new ArgumentException($"Giá nhập sản phẩm '{ctpn.MaLoaiSP}' không được âm.");
        }
    }
}

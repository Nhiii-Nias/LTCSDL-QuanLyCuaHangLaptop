using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyNhapHang : Form
    {
        private readonly DTO_TaiKhoanNV _taiKhoanHienTai;
        private readonly BUS_KhoHang _busKhoHang = new BUS_KhoHang();
        private readonly BUS_SanPham _busSanPham = new BUS_SanPham();
        
        private string? _maPhieuNhapDangChon = null;
        private bool _dangXemChiTiet = false;

        private bool IsVT004(string? maVaiTro)
        {
            if (string.IsNullOrWhiteSpace(maVaiTro)) return false;
            string value = maVaiTro.Trim();
            if (value == "VT004" || value == "VT00000004") return true;
            if (value.StartsWith("VT") && value.Length == 10)
            {
                if (int.TryParse(value.Substring(2), out int num))
                {
                    return num == 4;
                }
            }
            return false;
        }

        public FormQuanLyNhapHang(DTO_TaiKhoanNV taiKhoan)
        {
            _taiKhoanHienTai = taiKhoan;
            InitializeComponent();

            this.Load += FormQuanLyNhapHang_Load;
            dataGridViewDSPhieuNhap.CellClick += dataGridViewDSPhieuNhap_CellClick;
            
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnTim.Click += btnTim_Click;

            // Thiết lập thuộc tính readonly cho một số ô nhập liệu
            txtTenNhanVien.ReadOnly = true;
            txtMaKhuyenMai.ReadOnly = true; // Tổng tiền
        }

        private void FormQuanLyNhapHang_Load(object sender, EventArgs e)
        {
            txtTenNhanVien.Text = _taiKhoanHienTai.MaNV.Trim();
            txtMaKhuyenMai.Text = "0";

            // Load Combobox Loại Sản Phẩm
            try
            {
                DataTable dtLoaiSP = _busSanPham.LayDanhSachLoaiSP();
                comboBoxChonLoaiSanPham.DataSource = dtLoaiSP;
                comboBoxChonLoaiSanPham.DisplayMember = "TenLoai";
                comboBoxChonLoaiSanPham.ValueMember = "MaLoaiSP";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục loại sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Thiết lập Combobox trạng thái
            comboBoxMaHopDong.Items.Clear();
            comboBoxMaHopDong.Items.AddRange(new object[] { "Tất cả", "Chờ xác nhận", "Đã nhập", "Huỷ" });
            comboBoxMaHopDong.SelectedIndex = 0;

            NapComboBoxNhaCungCap();
            LoadPhieuNhapList();
        }

        private void NapComboBoxNhaCungCap()
        {
            try
            {
                DataTable dtNCC = _busKhoHang.LayDanhSachNCC();
                DataTable dtActive = dtNCC.Clone();
                foreach (DataRow row in dtNCC.Rows)
                {
                    bool isDeleted = Convert.ToBoolean(row["IsDeleted"] ?? false);
                    if (!isDeleted)
                    {
                        dtActive.ImportRow(row);
                    }
                }
                comboBoxMaNhaCungCap.DataSource = dtActive;
                comboBoxMaNhaCungCap.DisplayMember = "TenNCC";
                comboBoxMaNhaCungCap.ValueMember = "MaNCC";
                comboBoxMaNhaCungCap.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPhieuNhapList(string maNCC = "", string trangThai = "")
        {
            try
            {
                _dangXemChiTiet = false;
                _maPhieuNhapDangChon = null;

                DataTable dt = _busKhoHang.LayDanhSachPhieuNhap();

                List<string> filters = new List<string>();
                if (!string.IsNullOrWhiteSpace(maNCC))
                {
                    filters.Add($"(MaNCC = '{maNCC.Replace("'", "''")}')");
                }
                if (!string.IsNullOrWhiteSpace(trangThai))
                {
                    string dbTrangThai = trangThai;
                    if (trangThai == "Chờ xác nhận") dbTrangThai = "Chờ Xác Nhận";
                    else if (trangThai == "Đã nhập") dbTrangThai = "Đã Nhập";
                    else if (trangThai == "Huỷ") dbTrangThai = "Huỷ";
                    filters.Add($"(TrangThai = '{dbTrangThai.Replace("'", "''")}')");
                }

                if (filters.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Join(" AND ", filters);
                    dt = dv.ToTable();
                }

                dataGridViewDSPhieuNhap.DataSource = dt;

                // Cấu hình các Header
                if (dataGridViewDSPhieuNhap.Columns["MaPhieuNhap"] != null)
                    dataGridViewDSPhieuNhap.Columns["MaPhieuNhap"].HeaderText = "Mã phiếu nhập";
                if (dataGridViewDSPhieuNhap.Columns["MaNV"] != null)
                    dataGridViewDSPhieuNhap.Columns["MaNV"].HeaderText = "Nhân viên nhập";
                if (dataGridViewDSPhieuNhap.Columns["MaNCC"] != null)
                    dataGridViewDSPhieuNhap.Columns["MaNCC"].HeaderText = "Mã nhà cung cấp";
                if (dataGridViewDSPhieuNhap.Columns["NgayNhap"] != null)
                    dataGridViewDSPhieuNhap.Columns["NgayNhap"].HeaderText = "Ngày nhập";
                if (dataGridViewDSPhieuNhap.Columns["TongTien"] != null)
                {
                    dataGridViewDSPhieuNhap.Columns["TongTien"].HeaderText = "Tổng tiền";
                    dataGridViewDSPhieuNhap.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                }
                if (dataGridViewDSPhieuNhap.Columns["TrangThai"] != null)
                    dataGridViewDSPhieuNhap.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewDSPhieuNhap.Columns["NgayTao"] != null)
                    dataGridViewDSPhieuNhap.Columns["NgayTao"].Visible = false;
                if (dataGridViewDSPhieuNhap.Columns["NgayCapNhat"] != null)
                    dataGridViewDSPhieuNhap.Columns["NgayCapNhat"].Visible = false;
                if (dataGridViewDSPhieuNhap.Columns["NguoiTao"] != null)
                    dataGridViewDSPhieuNhap.Columns["NguoiTao"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (!_dangXemChiTiet)
            {
                DataGridViewRow row = dataGridViewDSPhieuNhap.Rows[e.RowIndex];
                _maPhieuNhapDangChon = row.Cells["MaPhieuNhap"]?.Value?.ToString()?.Trim();
                
                comboBoxMaNhaCungCap.SelectedValue = row.Cells["MaNCC"]?.Value?.ToString()?.Trim();
                txtTenNhanVien.Text = row.Cells["MaNV"]?.Value?.ToString()?.Trim();
                if (row.Cells["NgayNhap"]?.Value != DBNull.Value)
                {
                    dateTimePickerNgayBatDau.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value);
                }
                txtMaKhuyenMai.Text = Convert.ToDecimal(row.Cells["TongTien"]?.Value).ToString("N0");
                comboBoxMaHopDong.Text = row.Cells["TrangThai"]?.Value?.ToString()?.Trim();

                // Hiển thị chi tiết (serial) lên Grid
                if (!string.IsNullOrEmpty(_maPhieuNhapDangChon))
                {
                    HienshiChiTietPhieuNhap(_maPhieuNhapDangChon);
                }
            }
            else
            {
                // Nếu đang xem chi tiết, click dòng không làm gì vì txtMaDonHang đã bị xóa
            }
        }

        private void HienshiChiTietPhieuNhap(string maPN)
        {
            try
            {
                _dangXemChiTiet = true;
                DataTable dtSerials = _busKhoHang.LayDanhSachSerialTheoPhieuNhap(maPN);
                dataGridViewDSPhieuNhap.DataSource = dtSerials;

                if (dataGridViewDSPhieuNhap.Columns["MaSerialSP"] != null)
                    dataGridViewDSPhieuNhap.Columns["MaSerialSP"].HeaderText = "Mã Serial";
                if (dataGridViewDSPhieuNhap.Columns["MaPhieuNhap"] != null)
                    dataGridViewDSPhieuNhap.Columns["MaPhieuNhap"].HeaderText = "Mã phiếu";
                if (dataGridViewDSPhieuNhap.Columns["MaLoaiSP"] != null)
                    dataGridViewDSPhieuNhap.Columns["MaLoaiSP"].HeaderText = "Mã Loại SP";
                if (dataGridViewDSPhieuNhap.Columns["NgayNhap"] != null)
                    dataGridViewDSPhieuNhap.Columns["NgayNhap"].HeaderText = "Ngày nhập";
                if (dataGridViewDSPhieuNhap.Columns["NgaySX"] != null)
                    dataGridViewDSPhieuNhap.Columns["NgaySX"].HeaderText = "Ngày sản xuất";
                if (dataGridViewDSPhieuNhap.Columns["TrangThai"] != null)
                    dataGridViewDSPhieuNhap.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewDSPhieuNhap.Columns["IsDeleted"] != null)
                    dataGridViewDSPhieuNhap.Columns["IsDeleted"].Visible = false;
                if (dataGridViewDSPhieuNhap.Columns["NgayTao"] != null)
                    dataGridViewDSPhieuNhap.Columns["NgayTao"].Visible = false;
                if (dataGridViewDSPhieuNhap.Columns["NgayCapNhat"] != null)
                    dataGridViewDSPhieuNhap.Columns["NgayCapNhat"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsVT004(_taiKhoanHienTai.MaVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (comboBoxMaNhaCungCap.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Nhà Cung Cấp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string maNCC = comboBoxMaNhaCungCap.SelectedValue.ToString()!.Trim();

                // Kiểm tra NCC tồn tại
                var ncc = _busKhoHang.LayNCCTheoMa(maNCC);
                if (ncc == null)
                {
                    MessageBox.Show("Nhà cung cấp không tồn tại trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (comboBoxChonLoaiSanPham.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm cần nhập.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string maLoaiSP = comboBoxChonLoaiSanPham.SelectedValue.ToString()!;

                if (!int.TryParse(textBox1.Text.Trim(), out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng nhập phải là số nguyên dương lớn hơn 0.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(textBox2.Text.Trim(), out decimal giaNhap) || giaNhap < 0)
                {
                    MessageBox.Show("Đơn giá nhập không được âm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yêu cầu nhập/quét Serial
                List<string> listSerials = new List<string>();
                using (Form prompt = new Form())
                {
                    prompt.Width = 450;
                    prompt.Height = 400;
                    prompt.Text = "Nhập danh sách mã Serial sản phẩm";
                    prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                    prompt.MaximizeBox = false;
                    prompt.MinimizeBox = false;
                    prompt.StartPosition = FormStartPosition.CenterParent;

                    Label textLabel = new Label() { Left = 20, Top = 15, Width = 400, Text = $"Vui lòng nhập đúng {soLuong} mã serial (mỗi mã trên 1 dòng):", Font = new Font("Segoe UI", 10, FontStyle.Bold) };
                    TextBox textBoxSerials = new TextBox() { Left = 20, Top = 45, Width = 390, Height = 230, Multiline = true, AcceptsReturn = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10) };
                    Button btnConfirm = new Button() { Text = "Xác nhận", Left = 310, Width = 100, Top = 295, Height = 35, DialogResult = DialogResult.OK, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

                    prompt.Controls.Add(textLabel);
                    prompt.Controls.Add(textBoxSerials);
                    prompt.Controls.Add(btnConfirm);

                    if (prompt.ShowDialog() == DialogResult.OK)
                    {
                        string[] lines = textBoxSerials.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var l in lines)
                        {
                            string sClean = l.Trim();
                            if (!string.IsNullOrEmpty(sClean))
                            {
                                listSerials.Add(sClean);
                            }
                        }
                    }
                    else
                    {
                        return; // Người dùng hủy nhập
                    }
                }

                if (listSerials.Count != soLuong)
                {
                    MessageBox.Show($"Số lượng mã serial đã nhập ({listSerials.Count}) không khớp với số lượng nhập ({soLuong}).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra trùng lắp serial đầu vào
                HashSet<string> checkDuplicate = new HashSet<string>();
                foreach (var s in listSerials)
                {
                    if (!checkDuplicate.Add(s))
                    {
                        MessageBox.Show($"Mã serial '{s}' bị nhập trùng lặp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra xem serial đã tồn tại dưới DB chưa
                    var spExisting = _busSanPham.LayTheoSerial(s);
                    if (spExisting != null)
                    {
                        MessageBox.Show($"Mã serial '{s}' đã tồn tại trong cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Tạo đối tượng PhieuNhap
                string maPN = TaoMaPhieuNhapMoi();
                DTO_PhieuNhap pn = new DTO_PhieuNhap
                {
                    MaPhieuNhap = maPN,
                    MaNV = _taiKhoanHienTai.MaNV,
                    MaNCC = maNCC,
                    NgayNhap = dateTimePickerNgayBatDau.Value,
                    TrangThai = "Chờ Xác Nhận"
                };

                // Tạo chi tiết phiếu nhập
                DTO_ChiTietPhieuNhap ctpn = new DTO_ChiTietPhieuNhap
                {
                    MaPhieuNhap = maPN,
                    MaLoaiSP = maLoaiSP,
                    SoLuong = soLuong,
                    GiaNhap = giaNhap
                };

                // Tạo danh sách sản phẩm
                List<DTO_SanPham> listSp = new List<DTO_SanPham>();
                foreach (var s in listSerials)
                {
                    listSp.Add(new DTO_SanPham
                    {
                        MaSerialSP = s,
                        MaPhieuNhap = maPN,
                        MaLoaiSP = maLoaiSP,
                        NgayNhap = pn.NgayNhap,
                        TrangThai = "Trong Kho",
                        IsDeleted = false
                    });
                }

                if (_busKhoHang.TaoPhieuNhap(pn, new List<DTO_ChiTietPhieuNhap> { ctpn }, listSp, _taiKhoanHienTai.MaTK))
                {
                    MessageBox.Show($"Tạo phiếu nhập {maPN} ở trạng thái 'Chờ Xác Nhận' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPhieuNhapList();
                }
                else
                {
                    MessageBox.Show("Tạo phiếu nhập thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (IsVT004(_taiKhoanHienTai.MaVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận nhập kho
            if (string.IsNullOrEmpty(_maPhieuNhapDangChon))
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập từ danh sách để xác nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var pn = _busKhoHang.LayPhieuNhapTheoMa(_maPhieuNhapDangChon);
                if (pn == null)
                {
                    MessageBox.Show("Phiếu nhập không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (pn.TrangThai != "Chờ Xác Nhận")
                {
                    MessageBox.Show($"Phiếu nhập đang ở trạng thái '{pn.TrangThai}'. Chỉ có thể xác nhận phiếu nhập 'Chờ Xác Nhận'.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi trạng thái phiếu nhập này thành 'Đã Nhập' không?\nHành động này sẽ đưa các sản phẩm thuộc phiếu nhập vào kho hàng.",
                    "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    if (_busKhoHang.XacNhanPhieuNhap(_maPhieuNhapDangChon))
                    {
                        MessageBox.Show("Xác nhận nhập kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPhieuNhapList();
                    }
                    else
                    {
                        MessageBox.Show("Xác nhận nhập kho thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xác nhận phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (IsVT004(_taiKhoanHienTai.MaVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hủy phiếu nhập
            if (string.IsNullOrEmpty(_maPhieuNhapDangChon))
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập từ danh sách để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var pn = _busKhoHang.LayPhieuNhapTheoMa(_maPhieuNhapDangChon);
                if (pn == null)
                {
                    MessageBox.Show("Phiếu nhập không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (pn.TrangThai != "Chờ Xác Nhận")
                {
                    MessageBox.Show($"Phiếu nhập đang ở trạng thái '{pn.TrangThai}'. Chỉ có thể hủy phiếu nhập 'Chờ Xác Nhận'.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi hủy phiếu nhập này không?",
                    "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    if (_busKhoHang.HuyPhieuNhap(_maPhieuNhapDangChon))
                    {
                        MessageBox.Show("Hủy phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPhieuNhapList();
                    }
                    else
                    {
                        MessageBox.Show("Hủy phiếu nhập thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hủy phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string maNCC = comboBoxMaNhaCungCap.SelectedValue?.ToString()?.Trim() ?? "";
            string trangThai = comboBoxMaHopDong.SelectedItem?.ToString()?.Trim() ?? "";
            if (trangThai == "Tất cả") trangThai = "";

            LoadPhieuNhapList(maNCC, trangThai);
        }

        private string TaoMaPhieuNhapMoi()
        {
            try
            {
                DataTable dt = _busKhoHang.LayDanhSachPhieuNhap();
                int soLon = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string maPN = row["MaPhieuNhap"]?.ToString()?.Trim() ?? "";
                    if (maPN.StartsWith("PN") && maPN.Length == 10)
                    {
                        if (int.TryParse(maPN.Substring(2), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "PN" + (soLon + 1).ToString().PadLeft(8, '0');
            }
            catch
            {
                return "PN00000001";
            }
        }
    }
}

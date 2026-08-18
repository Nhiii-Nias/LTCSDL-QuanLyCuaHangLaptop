using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormDoiTraNCC : Form
    {
        private readonly BUS_SanPham _busSanPham = new BUS_SanPham();
        private readonly BUS_KhoHang _busKhoHang = new BUS_KhoHang();
        private string? _maSerialDangChon = null;
        private readonly string? _maVaiTro;

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

        public FormDoiTraNCC(string? maVaiTro = null)
        {
            _maVaiTro = maVaiTro;
            InitializeComponent();

            this.Load += FormDoiTraNCC_Load;
            dataGridViewDSDoiTraNCC.CellClick += dataGridViewDSDoiTraNCC_CellClick;

            btnThemKhachHang.Click += btnThemKhachHang_Click; // Thực hiện trả
            btnTim.Click += btnTim_Click;

            // Thiết lập chỉ đọc cho một số trường hiển thị
            txtNhaCungCap.ReadOnly = true;
        }

        private void FormDoiTraNCC_Load(object sender, EventArgs e)
        {
            NapComboBoxSerialLoi();
            LoadData();
        }

        private void NapComboBoxSerialLoi()
        {
            try
            {
                DataTable dt = _busSanPham.LayDanhSachSanPham();
                DataTable dtLoiOnly = dt.Clone();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["TrangThai"]?.ToString()?.Trim() == "Lỗi")
                    {
                        dtLoiOnly.ImportRow(row);
                    }
                }
                comboBoxSerialSPLoi.DataSource = dtLoiOnly;
                comboBoxSerialSPLoi.DisplayMember = "MaSerialSP";
                comboBoxSerialSPLoi.ValueMember = "MaSerialSP";
                comboBoxSerialSPLoi.SelectedIndex = -1;
                comboBoxSerialSPLoi.SelectedIndexChanged += comboBoxSerialSPLoi_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách serial lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxSerialSPLoi_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxSerialSPLoi.SelectedValue == null) return;
            string serial = comboBoxSerialSPLoi.SelectedValue.ToString()!.Trim();
            _maSerialDangChon = serial;

            // Autofill supplier name
            try
            {
                var sp = _busSanPham.LayTheoSerial(serial);
                if (sp != null && !string.IsNullOrEmpty(sp.MaPhieuNhap))
                {
                    var pn = _busKhoHang.LayPhieuNhapTheoMa(sp.MaPhieuNhap);
                    if (pn != null)
                    {
                        var ncc = _busKhoHang.LayNCCTheoMa(pn.MaNCC);
                        if (ncc != null)
                        {
                            txtNhaCungCap.Text = ncc.TenNCC.Trim();
                            return;
                        }
                    }
                }
                txtNhaCungCap.Text = "Không xác định";
            }
            catch
            {
                txtNhaCungCap.Text = "Không xác định";
            }
        }

        private void LoadData(string searchSerial = "", string searchNCC = "")
        {
            try
            {
                DataTable dt = _busSanPham.LayDanhSachSanPham();
                DataTable dtLoi = dt.Clone();
                if (!dtLoi.Columns.Contains("TenNCC"))
                {
                    dtLoi.Columns.Add("TenNCC", typeof(string));
                }

                foreach (DataRow row in dt.Rows)
                {
                    string tt = row["TrangThai"]?.ToString()?.Trim() ?? "";
                    if (tt == "Lỗi" || tt == "Đổi Trả")
                    {
                        string serial = row["MaSerialSP"]?.ToString()?.Trim() ?? "";
                        
                        // Get Supplier Name
                        string tenNCC = "";
                        string? maPN = row["MaPhieuNhap"]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(maPN))
                        {
                            var pn = _busKhoHang.LayPhieuNhapTheoMa(maPN);
                            if (pn != null)
                            {
                                var ncc = _busKhoHang.LayNCCTheoMa(pn.MaNCC);
                                if (ncc != null)
                                {
                                    tenNCC = ncc.TenNCC.Trim();
                                }
                            }
                        }

                        // Apply filters
                        bool matchSerial = string.IsNullOrWhiteSpace(searchSerial) || serial.Contains(searchSerial);
                        bool matchNCC = string.IsNullOrWhiteSpace(searchNCC) || tenNCC.ToLower().Contains(searchNCC.ToLower());

                        if (matchSerial && matchNCC)
                        {
                            DataRow newRow = dtLoi.NewRow();
                            foreach (DataColumn col in dt.Columns)
                            {
                                newRow[col.ColumnName] = row[col.ColumnName];
                            }
                            newRow["TenNCC"] = tenNCC;
                            dtLoi.Rows.Add(newRow);
                        }
                    }
                }

                dataGridViewDSDoiTraNCC.DataSource = dtLoi;

                // Định dạng hiển thị GridView
                if (dataGridViewDSDoiTraNCC.Columns["MaSerialSP"] != null)
                    dataGridViewDSDoiTraNCC.Columns["MaSerialSP"].HeaderText = "Số Serial Lỗi";
                if (dataGridViewDSDoiTraNCC.Columns["MaPhieuNhap"] != null)
                    dataGridViewDSDoiTraNCC.Columns["MaPhieuNhap"].HeaderText = "Mã phiếu nhập";
                if (dataGridViewDSDoiTraNCC.Columns["MaLoaiSP"] != null)
                    dataGridViewDSDoiTraNCC.Columns["MaLoaiSP"].HeaderText = "Mã loại sản phẩm";
                if (dataGridViewDSDoiTraNCC.Columns["NgayNhap"] != null)
                    dataGridViewDSDoiTraNCC.Columns["NgayNhap"].HeaderText = "Ngày nhập";
                if (dataGridViewDSDoiTraNCC.Columns["NgaySX"] != null)
                    dataGridViewDSDoiTraNCC.Columns["NgaySX"].HeaderText = "Ngày sản xuất";
                if (dataGridViewDSDoiTraNCC.Columns["TrangThai"] != null)
                    dataGridViewDSDoiTraNCC.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewDSDoiTraNCC.Columns["TenNCC"] != null)
                    dataGridViewDSDoiTraNCC.Columns["TenNCC"].HeaderText = "Nhà cung cấp";
                if (dataGridViewDSDoiTraNCC.Columns["IsDeleted"] != null)
                    dataGridViewDSDoiTraNCC.Columns["IsDeleted"].Visible = false;
                if (dataGridViewDSDoiTraNCC.Columns["NgayTao"] != null)
                    dataGridViewDSDoiTraNCC.Columns["NgayTao"].Visible = false;
                if (dataGridViewDSDoiTraNCC.Columns["NgayCapNhat"] != null)
                    dataGridViewDSDoiTraNCC.Columns["NgayCapNhat"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách sản phẩm lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSDoiTraNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSDoiTraNCC.Rows[e.RowIndex];

            _maSerialDangChon = row.Cells["MaSerialSP"]?.Value?.ToString()?.Trim();
            comboBoxSerialSPLoi.SelectedValue = _maSerialDangChon;
            txtLyDoTra.Clear();

            // Tìm nhà cung cấp tương ứng
            try
            {
                string? maPN = row.Cells["MaPhieuNhap"]?.Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(maPN))
                {
                    var pn = _busKhoHang.LayPhieuNhapTheoMa(maPN);
                    if (pn != null)
                    {
                        var ncc = _busKhoHang.LayNCCTheoMa(pn.MaNCC);
                        if (ncc != null)
                        {
                            txtNhaCungCap.Text = ncc.TenNCC.Trim();
                            return;
                        }
                    }
                }
                txtNhaCungCap.Text = "Không xác định";
            }
            catch
            {
                txtNhaCungCap.Text = "Không xác định";
            }
        }

        private void btnThemKhachHang_Click(object sender, EventArgs e)
        {
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thực hiện trả NCC
            if (string.IsNullOrEmpty(_maSerialDangChon))
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm lỗi trong danh sách để trả cho nhà cung cấp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string lyDo = txtLyDoTra.Text.Trim();
            if (string.IsNullOrWhiteSpace(lyDo))
            {
                MessageBox.Show("Vui lòng nhập Lý do trả sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi hay không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                // Thực hiện trả NCC bằng cách chuyển sản phẩm về 'Trong Kho' tạm thời và gọi Xoa(maSerial) để xóa mềm (IsDeleted = 1, TrangThai = 'Lỗi')
                // Việc chuyển về 'Trong Kho' là cần thiết để vượt qua các điều kiện kiểm tra trong BUS_SanPham.Xoa()
                if (_busSanPham.CapNhatTrangThaiSerial(_maSerialDangChon, "Trong Kho") && 
                    _busSanPham.Xoa(_maSerialDangChon))
                {
                    MessageBox.Show($"Đã trả sản phẩm lỗi '{_maSerialDangChon}' cho nhà cung cấp '{txtNhaCungCap.Text}' thành công!\nLý do: {lyDo}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Thực hiện trả hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực hiện trả hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string searchSerial = comboBoxSerialSPLoi.SelectedValue?.ToString()?.Trim() ?? comboBoxSerialSPLoi.Text.Trim();
            string searchNCC = txtNhaCungCap.Text.Trim();
            LoadData(searchSerial, searchNCC);
        }

        private void LamMoiForm()
        {
            _maSerialDangChon = null;
            comboBoxSerialSPLoi.SelectedIndex = -1;
            txtNhaCungCap.Clear();
            txtLyDoTra.Clear();
            dateTimePickerNgayTra.Value = DateTime.Today;
        }
    }
}

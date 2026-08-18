using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormTonKho : Form
    {
        private readonly BUS_SanPham _busSanPham = new BUS_SanPham();
        private readonly BUS_KhoHang _busKhoHang = new BUS_KhoHang();
        private string? _maSerialDangChon = null;
        private readonly string? _maVaiTro;

        private bool CoQuyenGhi(string? maVaiTro)
        {
            if (string.IsNullOrWhiteSpace(maVaiTro)) return false;
            string value = maVaiTro.Trim();
            if (value == "VT001" || value == "VT00000001") return true;
            if (value == "VT003" || value == "VT00000003") return true;
            if (value.StartsWith("VT") && value.Length == 10)
            {
                if (int.TryParse(value.Substring(2), out int num))
                {
                    return num == 1 || num == 3;
                }
            }
            return false;
        }

        public FormTonKho(string? maVaiTro = null)
        {
            _maVaiTro = maVaiTro ?? FormMain.TaiKhoanDangNhap?.MaVaiTro;
            InitializeComponent();

            this.Load += FormTonKho_Load;
            dataGridViewDSSanPham.CellClick += dataGridViewDSSanPham_CellClick;
            dataGridViewDSSanPham.CellEndEdit += dataGridViewDSSanPham_CellEndEdit;

            btnSua.Click += btnSua_Click;
            btnTim.Click += btnTim_Click;

            // Đăng ký sự kiện thay đổi lựa chọn lọc
            comboBoxChonLoaiSanPham.SelectedIndexChanged += (s, e) => LoadData();
            comboBoxChonNhaSanXuat.SelectedIndexChanged += (s, e) => LoadData();
            comboBoxTrangThai.SelectedIndexChanged += (s, e) => LoadData();
        }

        private void FormTonKho_Load(object sender, EventArgs e)
        {
            // Thiết lập ComboBox trạng thái nhập liệu
            comboBoxTrangThai.Items.Clear();
            comboBoxTrangThai.Items.AddRange(new object[] { "Tất cả", "Trong Kho", "Đã Bán", "Bảo Hành", "Lỗi", "Đổi Trả" });
            comboBoxTrangThai.SelectedIndex = 0;

            // Kích hoạt ShowCheckBox = true và đặt Checked = false cho DateTimePickers
            dateTimePickerNgayNhapKho.ShowCheckBox = true;
            dateTimePickerNgayNhapKho.Checked = false;
            dateTimePickerNgaySanXuat.ShowCheckBox = true;
            dateTimePickerNgaySanXuat.Checked = false;

            // Tải dữ liệu bộ lọc
            try
            {
                // Bộ lọc Loại SP tĩnh
                comboBoxChonLoaiSanPham.DataSource = null;
                comboBoxChonLoaiSanPham.Items.Clear();
                comboBoxChonLoaiSanPham.Items.AddRange(new object[] { "Tất cả", "Laptop", "Chuột", "Bàn phím" });
                comboBoxChonLoaiSanPham.SelectedIndex = 0;

                // Bộ lọc Hãng SX
                DataTable dtHang = _busSanPham.LayDanhSachHSX();
                DataRow drHang = dtHang.NewRow();
                drHang["MaHang"] = "ALL";
                drHang["TenHang"] = "Tất cả hãng";
                dtHang.Rows.InsertAt(drHang, 0);
                comboBoxChonNhaSanXuat.DataSource = dtHang;
                comboBoxChonNhaSanXuat.DisplayMember = "TenHang";
                comboBoxChonNhaSanXuat.ValueMember = "MaHang";
                comboBoxChonNhaSanXuat.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bộ lọc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadData();

            if (!CoQuyenGhi(_maVaiTro))
            {
                dataGridViewDSSanPham.ReadOnly = true;
            }
            else
            {
                dataGridViewDSSanPham.ReadOnly = false;
                foreach (DataGridViewColumn col in dataGridViewDSSanPham.Columns)
                {
                    if (col.Name == "TrangThai" || col.Name == "MaLoaiSP" || col.Name == "MaPhieuNhap" || col.Name == "NgayNhap" || col.Name == "NgaySX" || col.Name == "IsDeleted")
                    {
                        col.ReadOnly = false;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                dataGridViewDSSanPham.CellValueChanged += dataGridViewDSSanPham_CellValueChanged;
            }
        }

        private void dataGridViewDSSanPham_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSSanPham.Rows[e.RowIndex];
            string colName = dataGridViewDSSanPham.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "TrangThai")
            {
                comboBoxTrangThai.Text = val?.ToString()?.Trim();
            }
            else if (colName == "MaLoaiSP")
            {
                txtTenSanPham.Text = val?.ToString()?.Trim();
            }
            else if (colName == "MaPhieuNhap")
            {
                txtMaPhieuNhapHang.Text = val?.ToString()?.Trim();
            }
            else if (colName == "NgayNhap")
            {
                if (DateTime.TryParse(val?.ToString(), out DateTime dt))
                    dateTimePickerNgayNhapKho.Value = dt;
            }
            else if (colName == "NgaySX")
            {
                if (DateTime.TryParse(val?.ToString(), out DateTime dt))
                    dateTimePickerNgaySanXuat.Value = dt;
            }
        }

        private void LoadData(bool apDungBoLoc = false)
        {
            try
            {
                DataTable dt = _busSanPham.LayDanhSachSanPham();
                var rows = dt.AsEnumerable();

                // 1. Lọc theo loại sản phẩm (ComboBox tĩnh)
                if (comboBoxChonLoaiSanPham.SelectedIndex > 0)
                {
                    string category = comboBoxChonLoaiSanPham.Text.Trim().ToLower();
                    rows = rows.Where(r => r.Field<string>("DanhMuc") != null && r.Field<string>("DanhMuc").Trim().ToLower() == category);
                }

                // 2. Lọc theo Hãng sản xuất (ComboBox)
                if (comboBoxChonNhaSanXuat.SelectedValue != null)
                {
                    string maHang = comboBoxChonNhaSanXuat.SelectedValue.ToString()!;
                    if (maHang != "ALL")
                    {
                        DataTable dtLoaiSP = _busSanPham.LayDanhSachLoaiSP();
                        List<string> listMaLoai = new List<string>();
                        foreach (DataRow row in dtLoaiSP.Rows)
                        {
                            if (row["MaHang"]?.ToString()?.Trim() == maHang)
                            {
                                listMaLoai.Add(row["MaLoaiSP"]?.ToString()?.Trim() ?? "");
                            }
                        }

                        if (listMaLoai.Count > 0)
                        {
                            rows = rows.Where(r => r.Field<string>("MaLoaiSP") != null && listMaLoai.Contains(r.Field<string>("MaLoaiSP").Trim()));
                        }
                        else
                        {
                            rows = rows.Where(r => false);
                        }
                    }
                }

                // Lọc theo trạng thái
                if (comboBoxTrangThai.SelectedIndex > 0 && comboBoxTrangThai.Text != "Tất cả")
                {
                    string status = comboBoxTrangThai.Text.Trim();
                    rows = rows.Where(r => r.Field<string>("TrangThai") != null && r.Field<string>("TrangThai").Trim() == status);
                }

                // Lọc theo các điều kiện tìm kiếm nâng cao (khi nhấn nút Tìm)
                if (apDungBoLoc)
                {
                    // Lọc theo Serial
                    string serial = txtMaSerialSanPham.Text.Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(serial))
                    {
                        rows = rows.Where(r => r.Field<string>("MaSerialSP") != null && r.Field<string>("MaSerialSP").ToLower().Contains(serial));
                    }

                    // Lọc đối chiếu Tên sản phẩm / Loại sản phẩm
                    string searchName = txtTenSanPham.Text.Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(searchName))
                    {
                        rows = rows.Where(r => {
                            string tenLoai = r.Table.Columns.Contains("TenLoai") ? r.Field<string>("TenLoai")?.ToString() : null;
                            string maLoai = r.Field<string>("MaLoaiSP");
                            return (tenLoai != null && tenLoai.ToLower().Contains(searchName)) || (maLoai != null && maLoai.ToLower().Contains(searchName));
                        });
                    }

                    // Lọc theo Mã phiếu nhập
                    string maPN = txtMaPhieuNhapHang.Text.Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(maPN))
                    {
                        rows = rows.Where(r => r.Field<string>("MaPhieuNhap") != null && r.Field<string>("MaPhieuNhap").ToLower().Contains(maPN));
                    }

                    // Lọc theo Ngày nhập kho
                    if (dateTimePickerNgayNhapKho.Checked)
                    {
                        DateTime ngayNhap = dateTimePickerNgayNhapKho.Value.Date;
                        rows = rows.Where(r => r.Field<DateTime?>("NgayNhap") != null && r.Field<DateTime>("NgayNhap").Date == ngayNhap);
                    }

                    // Lọc theo Ngày sản xuất
                    if (dateTimePickerNgaySanXuat.Checked)
                    {
                        DateTime ngaySX = dateTimePickerNgaySanXuat.Value.Date;
                        rows = rows.Where(r => r.Field<DateTime?>("NgaySX") != null && r.Field<DateTime?>("NgaySX").Value.Date == ngaySX);
                    }
                }

                DataTable dtResult = rows.Any() ? rows.CopyToDataTable() : dt.Clone();
                dataGridViewDSSanPham.DataSource = dtResult;

                // Định dạng Header
                if (dataGridViewDSSanPham.Columns["MaSerialSP"] != null)
                    dataGridViewDSSanPham.Columns["MaSerialSP"].HeaderText = "Số Serial";
                if (dataGridViewDSSanPham.Columns["MaPhieuNhap"] != null)
                    dataGridViewDSSanPham.Columns["MaPhieuNhap"].HeaderText = "Mã phiếu nhập";
                if (dataGridViewDSSanPham.Columns["MaLoaiSP"] != null)
                    dataGridViewDSSanPham.Columns["MaLoaiSP"].HeaderText = "Mã loại sản phẩm";
                if (dataGridViewDSSanPham.Columns["TenLoai"] != null)
                    dataGridViewDSSanPham.Columns["TenLoai"].HeaderText = "Tên loại sản phẩm";
                if (dataGridViewDSSanPham.Columns["NgayNhap"] != null)
                    dataGridViewDSSanPham.Columns["NgayNhap"].HeaderText = "Ngày nhập kho";
                if (dataGridViewDSSanPham.Columns["NgaySX"] != null)
                    dataGridViewDSSanPham.Columns["NgaySX"].HeaderText = "Ngày sản xuất";
                if (dataGridViewDSSanPham.Columns["TrangThai"] != null)
                    dataGridViewDSSanPham.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewDSSanPham.Columns["NgayTao"] != null)
                    dataGridViewDSSanPham.Columns["NgayTao"].Visible = false;
                if (dataGridViewDSSanPham.Columns["NgayCapNhat"] != null)
                    dataGridViewDSSanPham.Columns["NgayCapNhat"].Visible = false;
                if (dataGridViewDSSanPham.Columns["IsDeleted"] != null)
                {
                    dataGridViewDSSanPham.Columns["IsDeleted"].Visible = true;
                    dataGridViewDSSanPham.Columns["IsDeleted"].HeaderText = "Đã Xóa";
                }

                // Tính tổng số lượng hàng trong kho hiển thị lên label
                int tongTon = 0;
                foreach (DataRow row in dtResult.Rows)
                {
                    if (row["TrangThai"]?.ToString()?.Trim() == "Trong Kho")
                    {
                        tongTon++;
                    }
                }
                labelKetQuaTongSLLoaiSanPham.Text = tongTon.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị dữ liệu tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSSanPham.Rows[e.RowIndex];

            _maSerialDangChon = row.Cells["MaSerialSP"]?.Value?.ToString()?.Trim();
            txtMaSerialSanPham.Text = _maSerialDangChon;
            txtTenSanPham.Text = row.Cells["MaLoaiSP"]?.Value?.ToString()?.Trim(); // Mã loại SP
            txtMaPhieuNhapHang.Text = row.Cells["MaPhieuNhap"]?.Value?.ToString()?.Trim();
            
            if (row.Cells["NgayNhap"]?.Value != DBNull.Value)
                dateTimePickerNgayNhapKho.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value);
            
            if (row.Cells["NgaySX"]?.Value != DBNull.Value)
                dateTimePickerNgaySanXuat.Value = Convert.ToDateTime(row.Cells["NgaySX"].Value);
            else
                dateTimePickerNgaySanXuat.Value = DateTime.Today;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!CoQuyenGhi(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_maSerialDangChon))
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DTO_SanPham? original = _busSanPham.LayTheoSerial(_maSerialDangChon);
            if (original == null)
            {
                MessageBox.Show("Sản phẩm không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maLoaiSP = txtTenSanPham.Text.Trim();
            string maPN = txtMaPhieuNhapHang.Text.Trim();
            DateTime ngayNhap = dateTimePickerNgayNhapKho.Value;
            DateTime? ngaySX = dateTimePickerNgaySanXuat.Value;

            // Chức năng sửa chỉ được phép sửa ngày sản xuất
            if (maLoaiSP != original.MaLoaiSP || maPN != original.MaPhieuNhap || ngayNhap.Date != original.NgayNhap.Date)
            {
                MessageBox.Show("Chỉ được phép sửa ngày sản xuất của sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi hay không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                DTO_SanPham sp = new DTO_SanPham
                {
                    MaSerialSP = _maSerialDangChon,
                    MaLoaiSP = maLoaiSP,
                    MaPhieuNhap = maPN,
                    NgayNhap = ngayNhap,
                    NgaySX = ngaySX,
                    TrangThai = original.TrangThai, // Giữ nguyên trạng thái cũ
                    IsDeleted = original.IsDeleted
                };

                if (_busSanPham.CapNhat(sp))
                {
                    MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData(true);
        }

        private void dataGridViewDSSanPham_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSSanPham.Rows[e.RowIndex];
            
            try
            {
                string serial = row.Cells["MaSerialSP"]?.Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrEmpty(serial)) return;

                string maLoaiSP = row.Cells["MaLoaiSP"]?.Value?.ToString()?.Trim() ?? "";
                string maPN = row.Cells["MaPhieuNhap"]?.Value?.ToString()?.Trim() ?? "";
                string trangThai = row.Cells["TrangThai"]?.Value?.ToString()?.Trim() ?? "";
                
                DateTime ngayNhap = DateTime.Today;
                if (row.Cells["NgayNhap"]?.Value != DBNull.Value)
                    ngayNhap = Convert.ToDateTime(row.Cells["NgayNhap"].Value);

                DateTime? ngaySX = null;
                if (row.Cells["NgaySX"]?.Value != DBNull.Value && row.Cells["NgaySX"]?.Value != null)
                    ngaySX = Convert.ToDateTime(row.Cells["NgaySX"].Value);

                bool isDeleted = Convert.ToBoolean(row.Cells["IsDeleted"]?.Value ?? false);

                DTO_SanPham? original = _busSanPham.LayTheoSerial(serial);
                if (original != null)
                {
                    // Kiểm tra xem isDeleted có thay đổi sang true nhưng sản phẩm đã bán không
                    if (isDeleted && !original.IsDeleted && original.TrangThai == "Đã Bán")
                    {
                        MessageBox.Show("Sản phẩm đã được bán, không được phép xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadData();
                        return;
                    }

                    // Chỉ được phép sửa ngày sản xuất và isDeleted
                    if (maLoaiSP != original.MaLoaiSP || maPN != original.MaPhieuNhap || ngayNhap.Date != original.NgayNhap.Date || trangThai != original.TrangThai)
                    {
                        MessageBox.Show("Chỉ được phép sửa ngày sản xuất của sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoadData();
                        return;
                    }
                }

                // Validate
                if (_busSanPham.LayLoaiSPTheoMa(maLoaiSP) == null)
                {
                    MessageBox.Show("Mã loại sản phẩm không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                if (_busKhoHang.LayPhieuNhapTheoMa(maPN) == null)
                {
                    MessageBox.Show("Mã phiếu nhập không tồn tại trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                DTO_SanPham sp = new DTO_SanPham
                {
                    MaSerialSP = serial,
                    MaLoaiSP = maLoaiSP,
                    MaPhieuNhap = maPN,
                    NgayNhap = ngayNhap,
                    NgaySX = ngaySX,
                    TrangThai = trangThai,
                    IsDeleted = isDeleted
                };

                if (_busSanPham.CapNhat(sp))
                {
                    if (serial == _maSerialDangChon)
                    {
                        txtTenSanPham.Text = maLoaiSP;
                        txtMaPhieuNhapHang.Text = maPN;
                        dateTimePickerNgayNhapKho.Value = ngayNhap;
                        if (ngaySX.HasValue)
                            dateTimePickerNgaySanXuat.Value = ngaySX.Value;
                        comboBoxTrangThai.Text = trangThai;
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật thông tin sản phẩm trực tiếp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dòng trực tiếp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
            }
        }

        private void LamMoiForm()
        {
            _maSerialDangChon = null;
            txtMaSerialSanPham.Clear();
            txtTenSanPham.Clear();
            txtMaPhieuNhapHang.Clear();
            dateTimePickerNgayNhapKho.Value = DateTime.Today;
            dateTimePickerNgayNhapKho.Checked = false;
            dateTimePickerNgaySanXuat.Value = DateTime.Today;
            dateTimePickerNgaySanXuat.Checked = false;
            comboBoxTrangThai.SelectedIndex = 0;
        }
    }
}

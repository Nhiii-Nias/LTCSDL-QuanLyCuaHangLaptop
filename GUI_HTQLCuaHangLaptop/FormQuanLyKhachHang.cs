using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyKhachHang : Form
    {
        private readonly BUS_KhachHang _busKH = new BUS_KhachHang();
        private readonly BUS_TaiKhoan _busTK = new BUS_TaiKhoan();
        private string? _maKHDangChon = null;
        private string? _maTKDangChon = null;

        private readonly string _maNV;
        private readonly string? _maVaiTro;

        private bool CoQuyenGhi(string? maVaiTro)
        {
            if (string.IsNullOrWhiteSpace(maVaiTro)) return false;
            string value = maVaiTro.Trim();
            if (value == "VT001" || value == "VT00000001") return true;
            if (value == "VT002" || value == "VT00000002") return true;
            if (value.StartsWith("VT") && value.Length == 10)
            {
                if (int.TryParse(value.Substring(2), out int num))
                {
                    return num == 1 || num == 2;
                }
            }
            return false;
        }

        public FormQuanLyKhachHang(string maNV = "NV00000001")
        {
            _maNV = maNV;
            _maVaiTro = FormMain.TaiKhoanDangNhap?.MaVaiTro;
            InitializeComponent();

            this.Load += FormQuanLyKhachHang_Load;
            btnThemKhachHang.Click += btnThem_Click;
            btnSuaKhachHang.Click += btnSua_Click;
            btnXoaKhachHang.Click += btnXoa_Click;
            btnTim.Click += btnTim_Click;
            dataGridViewDSKhachHang.CellClick += dataGridViewDSKhachHang_CellClick;
            dataGridViewDSKhachHang.CellEndEdit += dataGridViewDSKhachHang_CellEndEdit;

            comboBoxLoaiKhachHang.SelectedIndexChanged += comboBoxLoaiKhachHang_SelectedIndexChanged;
            comboBoxLoaiBang.SelectedIndexChanged += comboBoxLoaiBang_SelectedIndexChanged;

            txtMaKH.ReadOnly = false;
        }

        private void SetGridColumnsMode(bool isAccountMode)
        {
            dataGridViewDSKhachHang.Columns.Clear();
            dataGridViewDSKhachHang.AutoGenerateColumns = false;

            if (!isAccountMode)
            {
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaKH", HeaderText = "Mã KH", DataPropertyName = "MaKH", ReadOnly = true });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTenKhachHang", HeaderText = "Tên Khách Hàng", DataPropertyName = "TenKH", ReadOnly = false });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLoaiKhachHang", HeaderText = "Loại KH", DataPropertyName = "LoaiKH", ReadOnly = true });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSDT", HeaderText = "Số Điện Thoại", DataPropertyName = "SDT", ReadOnly = false });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEmail", HeaderText = "Email", DataPropertyName = "Email", ReadOnly = false });
                
                DataGridViewCheckBoxColumn colIsDeleted = new DataGridViewCheckBoxColumn();
                colIsDeleted.Name = "colIsDeleted";
                colIsDeleted.HeaderText = "Đã Xóa";
                colIsDeleted.DataPropertyName = "IsDeleted";
                colIsDeleted.ReadOnly = true;
                dataGridViewDSKhachHang.Columns.Add(colIsDeleted);
            }
            else
            {
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaTK", HeaderText = "Mã TK", DataPropertyName = "MaTK", ReadOnly = true });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaKH", HeaderText = "Mã KH", DataPropertyName = "MaKH", ReadOnly = true });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTenDangNhap", HeaderText = "Tên Đăng Nhập", DataPropertyName = "TenDangNhap", ReadOnly = false });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMatKhau", HeaderText = "Mật Khẩu", DataPropertyName = "MatKhau", ReadOnly = false });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNgayTao", HeaderText = "Ngày Tạo", DataPropertyName = "NgayTao", ReadOnly = true });
                dataGridViewDSKhachHang.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTrangThai", HeaderText = "Trạng Thái", DataPropertyName = "TrangThai", ReadOnly = false });
            }
        }

        private void ApplyReadOnlyStates()
        {
            bool coQuyen = CoQuyenGhi(_maVaiTro);
            if (!coQuyen)
            {
                dataGridViewDSKhachHang.ReadOnly = true;
            }
            else
            {
                dataGridViewDSKhachHang.ReadOnly = false;
                bool isAccountMode = (comboBoxLoaiBang.SelectedIndex == 1);
                foreach (DataGridViewColumn col in dataGridViewDSKhachHang.Columns)
                {
                    if (isAccountMode)
                    {
                        if (col.Name == "colTenDangNhap" || col.Name == "colMatKhau" || col.Name == "colTrangThai")
                        {
                            col.ReadOnly = false;
                        }
                        else
                        {
                            col.ReadOnly = true;
                        }
                    }
                    else
                    {
                        if (col.Name == "colTenKhachHang" || col.Name == "colSDT" || col.Name == "colEmail" || col.Name == "colIsDeleted")
                        {
                            col.ReadOnly = false;
                        }
                        else
                        {
                            col.ReadOnly = true;
                        }
                    }
                }
            }
        }

        private void FormQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            dateTimePickerNgaySinh.ShowCheckBox = true;
            dateTimePickerNgaySinh.Checked = false;

            comboBoxLoaiBang.Items.Clear();
            comboBoxLoaiBang.Items.AddRange(new object[] { "Bảng khách hàng", "Bảng tài khoản khách hàng" });

            comboBoxLoaiBang.SelectedIndex = 0; // Default: Bảng khách hàng

            comboBoxLoaiKhachHang.Items.Clear();
            comboBoxLoaiKhachHang.Items.AddRange(new object[] { "Tất cả", "Khách lẻ", "Khách sỉ" });

            SetGridColumnsMode(false);
            LoadData();
            LamMoiForm();
            ApplyReadOnlyStates();

            if (CoQuyenGhi(_maVaiTro))
            {
                dataGridViewDSKhachHang.CellValueChanged += dataGridViewDSKhachHang_CellValueChanged;
            }
        }

        private void comboBoxLoaiBang_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LamMoiForm();
            bool isAccountMode = (comboBoxLoaiBang.SelectedIndex == 1);

            txtTenTaiKhoan.Enabled = isAccountMode;
            txtMatKhau.Enabled = isAccountMode;
            comboBoxTrangThai.Enabled = isAccountMode;

            txtTenKhachHang.Enabled = !isAccountMode;
            txtSDT.Enabled = !isAccountMode;
            txtEmail.Enabled = !isAccountMode;
            txtDiaChi.Enabled = !isAccountMode;
            comboBoxLoaiKhachHang.Enabled = !isAccountMode;
            dateTimePickerNgaySinh.Enabled = !isAccountMode;
            comboBoxIsHSSV.Enabled = !isAccountMode;

            txtMaKH.Enabled = true;

            btnThemKhachHang.Visible = true;
            btnSuaKhachHang.Visible = true;
            btnXoaKhachHang.Visible = true;
            btnXoaKhachHang.Enabled = !isAccountMode;
            btnTim.Visible = true;

            SetGridColumnsMode(isAccountMode);
            ApplyReadOnlyStates();
            LoadData();
        }

        private void comboBoxLoaiKhachHang_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Nếu người dùng đang chọn thêm mới (chưa chọn dòng nào dưới Grid)
            if (string.IsNullOrWhiteSpace(_maKHDangChon))
            {
                if (comboBoxLoaiKhachHang.SelectedIndex == 1) // Khách lẻ
                {
                    txtMaKH.Text = _busTK.TaoMaKHLeMoi();
                    dateTimePickerNgaySinh.Enabled = true;
                    comboBoxIsHSSV.Enabled = true;
                }
                else if (comboBoxLoaiKhachHang.SelectedIndex == 2) // Khách sỉ
                {
                    txtMaKH.Text = _busTK.TaoMaKHSiMoi();
                    dateTimePickerNgaySinh.Checked = false;
                    dateTimePickerNgaySinh.Enabled = false;
                    comboBoxIsHSSV.SelectedIndex = -1;
                    comboBoxIsHSSV.Enabled = false;
                }
                else
                {
                    txtMaKH.Text = "";
                }
            }
        }

        private void LoadData(bool apDungBoLoc = false)
        {
            try
            {
                bool isAccountMode = (comboBoxLoaiBang.SelectedIndex == 1);
                DataTable dt = isAccountMode ? _busTK.LayDanhSachTaiKhoanKH() : _busKH.LayDanhSach();

                if (apDungBoLoc)
                {
                    var rows = dt.AsEnumerable();

                    if (isAccountMode)
                    {
                        if (!string.IsNullOrWhiteSpace(txtMaKH.Text))
                        {
                            string term = txtMaKH.Text.Trim().ToLower();
                            rows = rows.Where(r => r.Field<string>("MaKH") != null && r.Field<string>("MaKH").ToLower().Contains(term));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(txtMaKH.Text))
                        {
                            string term = txtMaKH.Text.Trim().ToLower();
                            rows = rows.Where(r => r.Field<string>("MaKH") != null && r.Field<string>("MaKH").ToLower().Contains(term));
                        }
                        if (!string.IsNullOrWhiteSpace(txtTenKhachHang.Text))
                        {
                            string term = txtTenKhachHang.Text.Trim().ToLower();
                            rows = rows.Where(r => r.Field<string>("TenKH") != null && r.Field<string>("TenKH").ToLower().Contains(term));
                        }
                        if (!string.IsNullOrWhiteSpace(txtSDT.Text))
                        {
                            string term = txtSDT.Text.Trim();
                            rows = rows.Where(r => r.Field<string>("SDT") != null && r.Field<string>("SDT").Contains(term));
                        }
                        if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                        {
                            string term = txtEmail.Text.Trim().ToLower();
                            rows = rows.Where(r => r.Field<string>("Email") != null && r.Field<string>("Email").ToLower().Contains(term));
                        }
                        if (!string.IsNullOrWhiteSpace(txtDiaChi.Text))
                        {
                            string term = txtDiaChi.Text.Trim().ToLower();
                            rows = rows.Where(r => r.Field<string>("DiaChi") != null && r.Field<string>("DiaChi").ToLower().Contains(term));
                        }
                        if (comboBoxLoaiKhachHang.SelectedIndex > 0) // Index 0 is "Tất cả"
                        {
                            string term = comboBoxLoaiKhachHang.SelectedIndex == 1 ? "Lẻ" : "Sỉ";
                            rows = rows.Where(r => r.Field<string>("LoaiKH") != null && r.Field<string>("LoaiKH").Trim() == term);
                        }
                        if (dateTimePickerNgaySinh.Checked)
                        {
                            DateTime birthDate = dateTimePickerNgaySinh.Value.Date;
                            rows = rows.Where(r => r.Field<DateTime?>("SinhNhat") != null && r.Field<DateTime?>("SinhNhat").Value.Date == birthDate);
                        }
                        if (comboBoxIsHSSV.SelectedIndex >= 0)
                        {
                            bool laHSSV = comboBoxIsHSSV.Text == "Là HSSV";
                            rows = rows.Where(r => r.Field<bool?>("LaHSSV") != null && r.Field<bool?>("LaHSSV").Value == laHSSV);
                        }
                    }

                    dt = rows.Any() ? rows.CopyToDataTable() : dt.Clone();
                }

                dataGridViewDSKhachHang.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSKhachHang.Rows[e.RowIndex];

            if (comboBoxLoaiBang.SelectedIndex == 1) // Account mode
            {
                _maTKDangChon = row.Cells["colMaTK"]?.Value?.ToString()?.Trim();
                txtMaKH.Text = row.Cells["colMaKH"]?.Value?.ToString()?.Trim();
                txtTenTaiKhoan.Text = row.Cells["colTenDangNhap"]?.Value?.ToString()?.Trim();
                txtMatKhau.Text = row.Cells["colMatKhau"]?.Value?.ToString()?.Trim();
                string trangThai = row.Cells["colTrangThai"]?.Value?.ToString()?.Trim() ?? "Hoạt Động";
                comboBoxTrangThai.Text = trangThai;
            }
            else
            {
                _maKHDangChon = row.Cells["colMaKH"]?.Value?.ToString()?.Trim();
                txtMaKH.Text = _maKHDangChon;
                txtTenKhachHang.Text = row.Cells["colTenKhachHang"]?.Value?.ToString()?.Trim();
                txtSDT.Text = row.Cells["colSDT"]?.Value?.ToString()?.Trim();
                txtEmail.Text = row.Cells["colEmail"]?.Value?.ToString()?.Trim();

                // Đổ thêm thông tin địa chỉ từ CSDL (vì GridView không hiện địa chỉ nhưng DTO có)
                if (!string.IsNullOrWhiteSpace(_maKHDangChon))
                {
                    DTO_KhachHang? kh = _busKH.LayTheoMa(_maKHDangChon);
                    if (kh != null)
                    {
                        txtDiaChi.Text = kh.DiaChi ?? "";
                        // Lấy loại khách hàng
                        string loaiKH = kh.LoaiKH.Trim();
                        if (loaiKH == "Lẻ")
                        {
                            comboBoxLoaiKhachHang.SelectedIndex = 1;
                            dateTimePickerNgaySinh.Enabled = true;
                            comboBoxIsHSSV.Enabled = true;

                            DTO_KhachHangLe? khLe = _busKH.LayThongTinLe(_maKHDangChon);
                            if (khLe != null)
                            {
                                if (khLe.SinhNhat.HasValue)
                                {
                                    dateTimePickerNgaySinh.Value = khLe.SinhNhat.Value;
                                    dateTimePickerNgaySinh.Checked = true;
                                }
                                else
                                {
                                    dateTimePickerNgaySinh.Checked = false;
                                }
                                comboBoxIsHSSV.Text = khLe.LaHSSV ? "Là HSSV" : "Không phải";
                            }
                        }
                        else
                        {
                            comboBoxLoaiKhachHang.SelectedIndex = 2;
                            dateTimePickerNgaySinh.Checked = false;
                            dateTimePickerNgaySinh.Enabled = false;
                            comboBoxIsHSSV.SelectedIndex = -1;
                            comboBoxIsHSSV.Enabled = false;
                        }
                    }
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            _maKHDangChon = null;
            _maTKDangChon = null;

            LoadData(true);
        }

        private void dataGridViewDSKhachHang_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSKhachHang.Rows[e.RowIndex];
            string colName = dataGridViewDSKhachHang.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "colTenKhachHang")
            {
                txtTenKhachHang.Text = val?.ToString()?.Trim();
            }
            else if (colName == "colSDT")
            {
                txtSDT.Text = val?.ToString()?.Trim();
            }
            else if (colName == "colEmail")
            {
                txtEmail.Text = val?.ToString()?.Trim();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!CoQuyenGhi(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxLoaiBang.SelectedIndex == 1) // Account mode
            {
                try
                {
                    string maKH = txtMaKH.Text.Trim();
                    string tenDangNhap = txtTenTaiKhoan.Text.Trim();
                    string matKhau = txtMatKhau.Text.Trim();
                    string trangThai = comboBoxTrangThai.Text.Trim();

                    if (string.IsNullOrWhiteSpace(maKH))
                    {
                        MessageBox.Show("Mã khách hàng không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (maKH.StartsWith("DN", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Không được phép tạo tài khoản cho khách hàng doanh nghiệp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(tenDangNhap))
                    {
                        MessageBox.Show("Tên đăng nhập không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(matKhau))
                    {
                        MessageBox.Show("Mật khẩu không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var tk = new DTO_TaiKhoanKH
                    {
                        MaKH = maKH,
                        TenDangNhap = tenDangNhap,
                        MatKhau = matKhau,
                        TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "Hoạt Động" : trangThai
                    };

                    if (_busTK.DangKyTaiKhoanKH(tk))
                    {
                        MessageBox.Show("Thêm tài khoản khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        LamMoiForm();
                    }
                    else
                    {
                        MessageBox.Show("Thêm tài khoản thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            try
            {
                string tenKH = txtTenKhachHang.Text.Trim();
                string sdt = txtSDT.Text.Trim();
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenKH))
                {
                    MessageBox.Show("Tên khách hàng không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBoxLoaiKhachHang.SelectedIndex <= 0)
                {
                    MessageBox.Show("Vui lòng chọn loại khách hàng hợp lệ (Khách lẻ hoặc Khách sỉ). Không được chọn 'Tất cả' khi thêm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var kh = new DTO_KhachHang
                {
                    TenKH = tenKH,
                    SDT = string.IsNullOrWhiteSpace(sdt) ? null : sdt,
                    Email = string.IsNullOrWhiteSpace(email) ? null : email,
                    DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim(),
                    LoaiKH = comboBoxLoaiKhachHang.SelectedIndex == 1 ? "Lẻ" : "Sỉ"
                };

                if (kh.LoaiKH == "Lẻ")
                {
                    // Đọc thông tin khách lẻ
                    DateTime? sinhNhat = dateTimePickerNgaySinh.Checked ? dateTimePickerNgaySinh.Value.Date : (DateTime?)null;

                    bool laHSSV = comboBoxIsHSSV.Text == "Là HSSV";

                    var khLe = new DTO_KhachHangLe
                    {
                        LaHSSV = laHSSV,
                        SinhNhat = sinhNhat
                    };

                    if (_busKH.ThemKhachHangLe(kh, khLe, _maNV))
                    {
                        MessageBox.Show("Thêm khách hàng lẻ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        LamMoiForm();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Thêm khách sỉ
                    if (_busKH.ThemKhachHangSi(kh, _maNV))
                    {
                        MessageBox.Show("Thêm khách hàng sỉ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        LamMoiForm();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!CoQuyenGhi(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxLoaiBang.SelectedIndex == 1) // Account mode
            {
                if (string.IsNullOrWhiteSpace(_maTKDangChon))
                {
                    MessageBox.Show("Vui lòng chọn một tài khoản trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmAcc = MessageBox.Show("Bạn có chắc chắn muốn thay đổi thông tin tài khoản này không?",
                    "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmAcc != DialogResult.Yes) return;

                try
                {
                    string maKH = txtMaKH.Text.Trim();
                    string tenDangNhap = txtTenTaiKhoan.Text.Trim();
                    string matKhau = txtMatKhau.Text.Trim();
                    string trangThai = comboBoxTrangThai.Text.Trim();

                    if (string.IsNullOrWhiteSpace(maKH))
                    {
                        MessageBox.Show("Mã khách hàng không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(tenDangNhap))
                    {
                        MessageBox.Show("Tên đăng nhập không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(matKhau))
                    {
                        MessageBox.Show("Mật khẩu không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DTO_TaiKhoanKH? existing = _busTK.LayTaiKhoanKHTheoMaTK(_maTKDangChon);
                    if (existing != null && existing.MatKhau != matKhau)
                    {
                        if (matKhau.Length < 6)
                        {
                            MessageBox.Show("Mật khẩu phải từ 6 kí tự trở lên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    var tk = new DTO_TaiKhoanKH
                    {
                        MaTK = _maTKDangChon,
                        MaKH = maKH,
                        TenDangNhap = tenDangNhap,
                        MatKhau = matKhau,
                        TrangThai = string.IsNullOrWhiteSpace(trangThai) ? "Hoạt Động" : trangThai
                    };

                    if (_busTK.CapNhatTaiKhoanKH(tk))
                    {
                        MessageBox.Show("Cập nhật tài khoản khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        LamMoiForm();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật tài khoản thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(_maKHDangChon))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi thông tin khách hàng này không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string tenKH = txtTenKhachHang.Text.Trim();
                string sdt = txtSDT.Text.Trim();
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenKH))
                {
                    MessageBox.Show("Tên khách hàng không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBoxLoaiKhachHang.SelectedIndex <= 0)
                {
                    MessageBox.Show("Vui lòng chọn loại khách hàng hợp lệ (Khách lẻ hoặc Khách sỉ). Không được chọn 'Tất cả' khi sửa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var kh = new DTO_KhachHang
                {
                    MaKH = _maKHDangChon,
                    TenKH = tenKH,
                    SDT = string.IsNullOrWhiteSpace(sdt) ? null : sdt,
                    Email = string.IsNullOrWhiteSpace(email) ? null : email,
                    DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim(),
                    LoaiKH = comboBoxLoaiKhachHang.SelectedIndex == 1 ? "Lẻ" : "Sỉ"
                };

                // Cập nhật thông tin chung
                if (_busKH.CapNhat(kh))
                {
                    if (kh.LoaiKH == "Lẻ")
                    {
                        DateTime? sinhNhat = dateTimePickerNgaySinh.Checked ? dateTimePickerNgaySinh.Value.Date : (DateTime?)null;

                        bool laHSSV = comboBoxIsHSSV.Text == "Là HSSV";

                        var khLe = new DTO_KhachHangLe
                        {
                            MaKHLe = _maKHDangChon,
                            LaHSSV = laHSSV,
                            SinhNhat = sinhNhat
                        };

                        _busKH.CapNhatThongTinLe(khLe);
                    }

                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (comboBoxLoaiBang.SelectedIndex == 1)
            {
                MessageBox.Show("Không cho phép xóa tài khoản ở giao diện này.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CoQuyenGhi(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_maKHDangChon))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{_maKHDangChon}' không?\n(Dữ liệu sẽ bị ẩn, không xóa vật lý)",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (_busKH.Xoa(_maKHDangChon))
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewDSKhachHang_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (comboBoxLoaiBang.SelectedIndex == 2) return;
            DataGridViewRow row = dataGridViewDSKhachHang.Rows[e.RowIndex];
            
            try
            {
                if (comboBoxLoaiBang.SelectedIndex == 1) // Account mode
                {
                    string maTK = row.Cells["colMaTK"]?.Value?.ToString()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(maTK)) return;

                    string maKH = row.Cells["colMaKH"]?.Value?.ToString()?.Trim() ?? "";
                    string tenDangNhap = row.Cells["colTenDangNhap"]?.Value?.ToString()?.Trim() ?? "";
                    string matKhau = row.Cells["colMatKhau"]?.Value?.ToString()?.Trim() ?? "";
                    DTO_TaiKhoanKH? existing = _busTK.LayTaiKhoanKHTheoMaTK(maTK);
                    if (existing != null && existing.MatKhau != matKhau)
                    {
                        if (matKhau.Length < 6)
                        {
                            MessageBox.Show("Mật khẩu phải từ 6 kí tự trở lên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LoadData();
                            return;
                        }
                    }
                    string trangThai = row.Cells["colTrangThai"]?.Value?.ToString()?.Trim() ?? "Hoạt Động";

                    DTO_TaiKhoanKH tk = new DTO_TaiKhoanKH
                    {
                        MaTK = maTK,
                        MaKH = maKH,
                        TenDangNhap = tenDangNhap,
                        MatKhau = matKhau,
                        TrangThai = trangThai
                    };

                    if (_busTK.CapNhatTaiKhoanKH(tk))
                    {
                        if (maTK == _maTKDangChon)
                        {
                            txtTenTaiKhoan.Text = tenDangNhap;
                            txtMatKhau.Text = matKhau;
                            comboBoxTrangThai.Text = trangThai;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật tài khoản khách hàng trực tiếp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoadData();
                    }
                }
                else
                {
                    string maKH = row.Cells["colMaKH"]?.Value?.ToString()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(maKH)) return;

                    string tenKH = row.Cells["colTenKhachHang"]?.Value?.ToString()?.Trim() ?? "";
                    string sdt = row.Cells["colSDT"]?.Value?.ToString()?.Trim() ?? "";
                    string email = row.Cells["colEmail"]?.Value?.ToString()?.Trim() ?? "";
                    
                    // Get additional info if exist
                    DTO_KhachHang? existingKH = _busKH.LayTheoMa(maKH);
                    string diaChi = existingKH?.DiaChi ?? "";
                    string loaiKH = existingKH?.LoaiKH ?? "Lẻ";
                    bool isDeleted = Convert.ToBoolean(row.Cells["colIsDeleted"]?.Value ?? false);

                    DTO_KhachHang kh = new DTO_KhachHang
                    {
                        MaKH = maKH,
                        TenKH = tenKH,
                        SDT = string.IsNullOrWhiteSpace(sdt) ? null : sdt,
                        Email = string.IsNullOrWhiteSpace(email) ? null : email,
                        DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi,
                        LoaiKH = loaiKH,
                        IsDeleted = isDeleted
                    };

                    if (_busKH.CapNhat(kh))
                    {
                        if (maKH == _maKHDangChon)
                        {
                            txtTenKhachHang.Text = tenKH;
                            txtSDT.Text = sdt;
                            txtEmail.Text = email;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật khách hàng trực tiếp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoadData();
                    }
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
            _maKHDangChon = null;
            _maTKDangChon = null;
            
            txtMaKH.Clear();
            txtTenKhachHang.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            dateTimePickerNgaySinh.Checked = false;
            dateTimePickerNgaySinh.Enabled = (comboBoxLoaiBang.SelectedIndex == 0);
            comboBoxIsHSSV.SelectedIndex = -1;
            comboBoxIsHSSV.Enabled = (comboBoxLoaiBang.SelectedIndex == 0);

            txtTenTaiKhoan.Clear();
            txtMatKhau.Clear();
            comboBoxTrangThai.SelectedIndex = 0; // Default: Hoạt Động

            if (comboBoxLoaiBang.SelectedIndex == 0) // Client mode
            {
                comboBoxLoaiKhachHang.SelectedIndex = 0; // "Tất cả"
            }
            else
            {
                comboBoxLoaiKhachHang.SelectedIndex = -1;
            }
        }
    }
}

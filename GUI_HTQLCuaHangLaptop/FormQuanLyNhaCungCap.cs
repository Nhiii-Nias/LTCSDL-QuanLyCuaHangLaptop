using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyNhaCungCap : Form
    {
        private readonly BUS_KhoHang _busKhoHang = new BUS_KhoHang();
        private string? _maNCCDangChon = null;
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

        public FormQuanLyNhaCungCap(string? maVaiTro = null)
        {
            _maVaiTro = maVaiTro ?? FormMain.TaiKhoanDangNhap?.MaVaiTro;
            InitializeComponent();

            this.Load += FormQuanLyNhaCungCap_Load;
            btnThemKhachHang.Click += btnThem_Click;
            btnSuaKhachHang.Click += btnSua_Click;
            btnXoaKhachHang.Click += btnXoa_Click;
            
            // Thiết lập sự kiện CellClick
            dataGridViewDSNhaCungCap.CellClick += dataGridViewDSNhaCungCap_CellClick;
            dataGridViewDSNhaCungCap.CellEndEdit += dataGridViewDSNhaCungCap_CellEndEdit;
        }

        private void FormQuanLyNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadData();
            LamMoiForm();

            if (!CoQuyenGhi(_maVaiTro))
            {
                dataGridViewDSNhaCungCap.ReadOnly = true;
            }
            else
            {
                dataGridViewDSNhaCungCap.ReadOnly = false;
                foreach (DataGridViewColumn col in dataGridViewDSNhaCungCap.Columns)
                {
                    if (col.Name == "TenNCC" || col.Name == "SDT" || col.Name == "Email" || col.Name == "DiaChi" || col.Name == "IsDeleted")
                    {
                        col.ReadOnly = false;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                dataGridViewDSNhaCungCap.CellValueChanged += dataGridViewDSNhaCungCap_CellValueChanged;
            }
        }

        private void LoadData(string ten = "", string sdt = "", string email = "", string diaChi = "")
        {
            try
            {
                DataTable dt = _busKhoHang.LayDanhSachNCC();

                List<string> filters = new List<string>();
                if (!string.IsNullOrWhiteSpace(ten))
                {
                    filters.Add($"(TenNCC LIKE '%{ten.Replace("'", "''")}%' OR MaNCC LIKE '%{ten.Replace("'", "''")}%')");
                }
                if (!string.IsNullOrWhiteSpace(sdt))
                {
                    filters.Add($"(SDT LIKE '%{sdt.Replace("'", "''")}%')");
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    filters.Add($"(Email LIKE '%{email.Replace("'", "''")}%')");
                }
                if (!string.IsNullOrWhiteSpace(diaChi))
                {
                    filters.Add($"(DiaChi LIKE '%{diaChi.Replace("'", "''")}%')");
                }

                if (filters.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Join(" AND ", filters);
                    dt = dv.ToTable();
                }

                dataGridViewDSNhaCungCap.DataSource = dt;

                // Định cấu hình cột hiển thị
                if (dataGridViewDSNhaCungCap.Columns["MaNCC"] != null)
                    dataGridViewDSNhaCungCap.Columns["MaNCC"].HeaderText = "Mã NCC";
                if (dataGridViewDSNhaCungCap.Columns["TenNCC"] != null)
                    dataGridViewDSNhaCungCap.Columns["TenNCC"].HeaderText = "Tên nhà cung cấp";
                if (dataGridViewDSNhaCungCap.Columns["SDT"] != null)
                    dataGridViewDSNhaCungCap.Columns["SDT"].HeaderText = "Số điện thoại";
                if (dataGridViewDSNhaCungCap.Columns["Email"] != null)
                    dataGridViewDSNhaCungCap.Columns["Email"].HeaderText = "Email";
                if (dataGridViewDSNhaCungCap.Columns["DiaChi"] != null)
                    dataGridViewDSNhaCungCap.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if (dataGridViewDSNhaCungCap.Columns["NgayTao"] != null)
                    dataGridViewDSNhaCungCap.Columns["NgayTao"].HeaderText = "Ngày tạo";
                if (dataGridViewDSNhaCungCap.Columns["NgayCapNhat"] != null)
                    dataGridViewDSNhaCungCap.Columns["NgayCapNhat"].HeaderText = "Ngày cập nhật";
                if (dataGridViewDSNhaCungCap.Columns["IsDeleted"] != null)
                {
                    dataGridViewDSNhaCungCap.Columns["IsDeleted"].Visible = true;
                    dataGridViewDSNhaCungCap.Columns["IsDeleted"].HeaderText = "Đã Xóa";
                }
                if (dataGridViewDSNhaCungCap.Columns["NguoiTao"] != null)
                    dataGridViewDSNhaCungCap.Columns["NguoiTao"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSNhaCungCap.Rows[e.RowIndex];

            _maNCCDangChon = row.Cells["MaNCC"]?.Value?.ToString()?.Trim();
            txtTenNhaCungCap.Text = row.Cells["TenNCC"]?.Value?.ToString()?.Trim();
            txtSDT.Text = row.Cells["SDT"]?.Value?.ToString()?.Trim();
            txtEmail.Text = row.Cells["Email"]?.Value?.ToString()?.Trim();
            txtDiaChi.Text = row.Cells["DiaChi"]?.Value?.ToString()?.Trim();
        }

        private void dataGridViewDSNhaCungCap_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSNhaCungCap.Rows[e.RowIndex];
            string colName = dataGridViewDSNhaCungCap.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "TenNCC")
            {
                txtTenNhaCungCap.Text = val?.ToString()?.Trim();
            }
            else if (colName == "SDT")
            {
                txtSDT.Text = val?.ToString()?.Trim();
            }
            else if (colName == "Email")
            {
                txtEmail.Text = val?.ToString()?.Trim();
            }
            else if (colName == "DiaChi")
            {
                txtDiaChi.Text = val?.ToString()?.Trim();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData(txtTenNhaCungCap.Text.Trim(), txtSDT.Text.Trim(), txtEmail.Text.Trim(), txtDiaChi.Text.Trim());
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!CoQuyenGhi(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string tenNCC = txtTenNhaCungCap.Text.Trim();
                string sdt = txtSDT.Text.Trim();
                string email = txtEmail.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenNCC))
                {
                    MessageBox.Show("Tên nhà cung cấp không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate thông tin NCC
                if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Địa chỉ Email không đúng định dạng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(sdt) && !Regex.IsMatch(sdt, @"^\d{10}$"))
                {
                    MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var ncc = new DTO_NhaCungCap
                {
                    MaNCC = TaoMaNCCMoi(),
                    TenNCC = tenNCC,
                    SDT = string.IsNullOrWhiteSpace(sdt) ? null : sdt,
                    Email = string.IsNullOrWhiteSpace(email) ? null : email,
                    DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi
                };

                if (_busKhoHang.ThemNCC(ncc))
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Thêm nhà cung cấp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (string.IsNullOrWhiteSpace(_maNCCDangChon))
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi thông tin nhà cung cấp này không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string tenNCC = txtTenNhaCungCap.Text.Trim();
                string sdt = txtSDT.Text.Trim();
                string email = txtEmail.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenNCC))
                {
                    MessageBox.Show("Tên nhà cung cấp không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate
                if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Địa chỉ Email không đúng định dạng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(sdt) && !Regex.IsMatch(sdt, @"^\d{10}$"))
                {
                    MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var ncc = new DTO_NhaCungCap
                {
                    MaNCC = _maNCCDangChon,
                    TenNCC = tenNCC,
                    SDT = string.IsNullOrWhiteSpace(sdt) ? null : sdt,
                    Email = string.IsNullOrWhiteSpace(email) ? null : email,
                    DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi
                };

                if (_busKhoHang.CapNhatNCC(ncc))
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!CoQuyenGhi(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_maNCCDangChon))
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp '{_maNCCDangChon}' không?\n(Dữ liệu sẽ bị ẩn, không xóa vật lý)",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (_busKhoHang.XoaNCC(_maNCCDangChon))
                {
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridViewDSNhaCungCap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSNhaCungCap.Rows[e.RowIndex];
            
            try
            {
                string maNCC = row.Cells["MaNCC"]?.Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrEmpty(maNCC)) return;

                string tenNCC = row.Cells["TenNCC"]?.Value?.ToString()?.Trim() ?? "";
                string sdt = row.Cells["SDT"]?.Value?.ToString()?.Trim() ?? "";
                string email = row.Cells["Email"]?.Value?.ToString()?.Trim() ?? "";
                string diaChi = row.Cells["DiaChi"]?.Value?.ToString()?.Trim() ?? "";
                bool isDeleted = Convert.ToBoolean(row.Cells["IsDeleted"]?.Value ?? false);

                // Validate
                if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Địa chỉ Email không đúng định dạng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadData();
                    return;
                }
                if (!string.IsNullOrEmpty(sdt) && !Regex.IsMatch(sdt, @"^\d{10}$"))
                {
                    MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadData();
                    return;
                }

                DTO_NhaCungCap ncc = new DTO_NhaCungCap
                {
                    MaNCC = maNCC,
                    TenNCC = tenNCC,
                    SDT = string.IsNullOrWhiteSpace(sdt) ? null : sdt,
                    Email = string.IsNullOrWhiteSpace(email) ? null : email,
                    DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi,
                    IsDeleted = isDeleted
                };

                if (_busKhoHang.CapNhatNCC(ncc))
                {
                    if (maNCC == _maNCCDangChon)
                    {
                        txtTenNhaCungCap.Text = tenNCC;
                        txtSDT.Text = sdt;
                        txtEmail.Text = email;
                        txtDiaChi.Text = diaChi;
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật nhà cung cấp trực tiếp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            _maNCCDangChon = null;
            txtTenNhaCungCap.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
        }

        private string TaoMaNCCMoi()
        {
            try
            {
                DataTable dt = _busKhoHang.LayDanhSachNCC();
                int soLon = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string maNCC = row["MaNCC"]?.ToString()?.Trim() ?? "";
                    if (maNCC.StartsWith("NCC") && maNCC.Length == 10)
                    {
                        if (int.TryParse(maNCC.Substring(3), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "NCC" + (soLon + 1).ToString().PadLeft(7, '0');
            }
            catch
            {
                return "NCC0000001";
            }
        }
    }
}

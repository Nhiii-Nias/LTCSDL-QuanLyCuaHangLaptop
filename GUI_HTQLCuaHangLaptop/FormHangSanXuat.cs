using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormHangSanXuat : Form
    {
        private readonly BUS_SanPham _busSP = new BUS_SanPham();
        private string? _maHangDangChon = null;
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

        public FormHangSanXuat(string? maVaiTro = null)
        {
            _maVaiTro = maVaiTro ?? FormMain.TaiKhoanDangNhap?.MaVaiTro;
            InitializeComponent();

            this.Load += FormHangSanXuat_Load;
            btnThemKhachHang.Click += btnThem_Click;
            btnSuaKhachHang.Click += btnSua_Click;
            btnXoaKhachHang.Click += btnXoa_Click;
            btnTim.Click += btnTim_Click;
            dataGridViewDSHangSanXuat.CellClick += dataGridViewDSHangSanXuat_CellClick;
            dataGridViewDSHangSanXuat.CellEndEdit += dataGridViewDSHangSanXuat_CellEndEdit;

            txtMaHangSanXuat.ReadOnly = false;
        }

        private void FormHangSanXuat_Load(object sender, EventArgs e)
        {
            LoadData();
            LamMoiForm();

            if (!CoQuyenGhi(_maVaiTro))
            {
                dataGridViewDSHangSanXuat.ReadOnly = true;
            }
            else
            {
                dataGridViewDSHangSanXuat.ReadOnly = false;
                foreach (DataGridViewColumn col in dataGridViewDSHangSanXuat.Columns)
                {
                    if (col.Name == "TenHang" || col.Name == "QuocGia" || col.Name == "IsDeleted")
                    {
                        col.ReadOnly = false;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                dataGridViewDSHangSanXuat.CellValueChanged += dataGridViewDSHangSanXuat_CellValueChanged;
            }
        }

        private void LoadData(string maHang = "", string tenHang = "", string quocGia = "")
        {
            try
            {
                DataTable dt = _busSP.LayDanhSachHSX();

                string filter = "";
                if (!string.IsNullOrWhiteSpace(maHang))
                {
                    filter += $"(MaHang LIKE '%{maHang.Replace("'", "''")}%')";
                }
                if (!string.IsNullOrWhiteSpace(tenHang))
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(TenHang LIKE '%{tenHang.Replace("'", "''")}%')";
                }
                if (!string.IsNullOrWhiteSpace(quocGia))
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(QuocGia LIKE '%{quocGia.Replace("'", "''")}%')";
                }

                if (filter != "")
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = filter;
                    dt = dv.ToTable();
                }

                dataGridViewDSHangSanXuat.DataSource = dt;

                // Cấu hình Header hiển thị
                if (dataGridViewDSHangSanXuat.Columns["MaHang"] != null)
                    dataGridViewDSHangSanXuat.Columns["MaHang"].HeaderText = "Mã hãng";
                if (dataGridViewDSHangSanXuat.Columns["TenHang"] != null)
                    dataGridViewDSHangSanXuat.Columns["TenHang"].HeaderText = "Tên hãng";
                if (dataGridViewDSHangSanXuat.Columns["QuocGia"] != null)
                    dataGridViewDSHangSanXuat.Columns["QuocGia"].HeaderText = "Quốc gia";
                if (dataGridViewDSHangSanXuat.Columns["NgayTao"] != null)
                    dataGridViewDSHangSanXuat.Columns["NgayTao"].HeaderText = "Ngày tạo";
                if (dataGridViewDSHangSanXuat.Columns["NgayCapNhat"] != null)
                    dataGridViewDSHangSanXuat.Columns["NgayCapNhat"].HeaderText = "Ngày cập nhật";
                if (dataGridViewDSHangSanXuat.Columns["IsDeleted"] != null)
                {
                    dataGridViewDSHangSanXuat.Columns["IsDeleted"].Visible = true;
                    dataGridViewDSHangSanXuat.Columns["IsDeleted"].HeaderText = "Đã Xóa";
                }
                if (dataGridViewDSHangSanXuat.Columns["NguoiTao"] != null)
                    dataGridViewDSHangSanXuat.Columns["NguoiTao"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hãng sản xuất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSHangSanXuat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSHangSanXuat.Rows[e.RowIndex];

            _maHangDangChon = row.Cells["MaHang"]?.Value?.ToString()?.Trim();
            txtMaHangSanXuat.Text = _maHangDangChon;
            txtTenHangSanXuat.Text = row.Cells["TenHang"]?.Value?.ToString()?.Trim();
            txtQuocGia.Text = row.Cells["QuocGia"]?.Value?.ToString()?.Trim();
        }

        private void dataGridViewDSHangSanXuat_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSHangSanXuat.Rows[e.RowIndex];
            string colName = dataGridViewDSHangSanXuat.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "TenHang")
            {
                txtTenHangSanXuat.Text = val?.ToString()?.Trim();
            }
            else if (colName == "QuocGia")
            {
                txtQuocGia.Text = val?.ToString()?.Trim();
            }
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
                string tenHang = txtTenHangSanXuat.Text.Trim();
                string quocGia = txtQuocGia.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenHang))
                {
                    MessageBox.Show("Tên hãng sản xuất không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var hsx = new DTO_HangSanXuat
                {
                    MaHang = TaoMaHangMoi(),
                    TenHang = tenHang,
                    QuocGia = string.IsNullOrWhiteSpace(quocGia) ? null : quocGia
                };

                if (_busSP.ThemHSX(hsx))
                {
                    MessageBox.Show("Thêm hãng sản xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (string.IsNullOrWhiteSpace(_maHangDangChon))
            {
                MessageBox.Show("Vui lòng chọn một hãng sản xuất trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaHangSanXuat.Text.Trim() != _maHangDangChon)
            {
                MessageBox.Show("Mã hãng sản xuất không cho phép sửa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi thông tin hãng sản xuất này không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string tenHang = txtTenHangSanXuat.Text.Trim();
                string quocGia = txtQuocGia.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenHang))
                {
                    MessageBox.Show("Tên hãng sản xuất không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var hsx = new DTO_HangSanXuat
                {
                    MaHang = _maHangDangChon,
                    TenHang = tenHang,
                    QuocGia = string.IsNullOrWhiteSpace(quocGia) ? null : quocGia
                };

                if (_busSP.CapNhatHSX(hsx))
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!CoQuyenGhi(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(_maHangDangChon))
            {
                MessageBox.Show("Vui lòng chọn một hãng sản xuất để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa hãng sản xuất '{_maHangDangChon}' không?\n(Dữ liệu sẽ bị xóa mềm, không xóa vật lý)",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (_busSP.XoaHSX(_maHangDangChon))
                {
                    MessageBox.Show("Xóa hãng sản xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridViewDSHangSanXuat_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSHangSanXuat.Rows[e.RowIndex];
            
            try
            {
                string maHang = row.Cells["MaHang"]?.Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrEmpty(maHang)) return;

                string tenHang = row.Cells["TenHang"]?.Value?.ToString()?.Trim() ?? "";
                string quocGia = row.Cells["QuocGia"]?.Value?.ToString()?.Trim() ?? "";
                bool isDeleted = Convert.ToBoolean(row.Cells["IsDeleted"]?.Value ?? false);

                DTO_HangSanXuat hsx = new DTO_HangSanXuat
                {
                    MaHang = maHang,
                    TenHang = tenHang,
                    QuocGia = string.IsNullOrWhiteSpace(quocGia) ? null : quocGia,
                    IsDeleted = isDeleted
                };

                if (_busSP.CapNhatHSX(hsx))
                {
                    if (maHang == _maHangDangChon)
                    {
                        txtTenHangSanXuat.Text = tenHang;
                        txtQuocGia.Text = quocGia;
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật hãng sản xuất trực tiếp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dòng trực tiếp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string maHang = txtMaHangSanXuat.Text.Trim();
            string tenHang = txtTenHangSanXuat.Text.Trim();
            string quocGia = txtQuocGia.Text.Trim();
            LoadData(maHang, tenHang, quocGia);
        }

        private void LamMoiForm()
        {
            _maHangDangChon = null;
            txtMaHangSanXuat.Text = TaoMaHangMoi();
            txtTenHangSanXuat.Clear();
            txtQuocGia.Clear();
        }

        private string TaoMaHangMoi()
        {
            try
            {
                DataTable dt = _busSP.LayDanhSachHSX();
                int soLon = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string maHang = row["MaHang"]?.ToString()?.Trim() ?? "";
                    if (maHang.StartsWith("HANG") && maHang.Length == 10)
                    {
                        if (int.TryParse(maHang.Substring(4), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "HANG" + (soLon + 1).ToString().PadLeft(6, '0');
            }
            catch
            {
                return "HANG000001";
            }
        }
    }
}

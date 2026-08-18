using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormLoaiSanPham : Form
    {
        private readonly BUS_SanPham _busSP = new BUS_SanPham();
        private string? _maLoaiDangChon = null;
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

        public FormLoaiSanPham(string? maVaiTro = null)
        {
            _maVaiTro = maVaiTro ?? FormMain.TaiKhoanDangNhap?.MaVaiTro;
            InitializeComponent();

            this.Load += FormLoaiSanPham_Load;
            btnThemKhachHang.Click += btnThem_Click;
            btnSuaKhachHang.Click += btnSua_Click;
            btnXoaKhachHang.Click += btnXoa_Click;
            btnTim.Click += btnTim_Click;
            dataGridViewDSLoaiSanPham.CellClick += dataGridViewDSLoaiSanPham_CellClick;
            dataGridViewDSLoaiSanPham.CellEndEdit += dataGridViewDSLoaiSanPham_CellEndEdit;

            txtMaLoaiSanPham.ReadOnly = false;
        }

        private void FormLoaiSanPham_Load(object sender, EventArgs e)
        {
            NapComboBoxHangSanXuat();
            LoadData();
            LamMoiForm();

            if (IsVT004(_maVaiTro))
            {
                dataGridViewDSLoaiSanPham.ReadOnly = true;
            }
            else
            {
                dataGridViewDSLoaiSanPham.ReadOnly = false;
                foreach (DataGridViewColumn col in dataGridViewDSLoaiSanPham.Columns)
                {
                    if (col.Name == "TenLoai" || col.Name == "ThoiGianBaoHanh" || col.Name == "GiaBanGoc" || col.Name == "DanhMuc" || col.Name == "MaHang" || col.Name == "IsDeleted")
                    {
                        col.ReadOnly = false;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                dataGridViewDSLoaiSanPham.CellValueChanged += dataGridViewDSLoaiSanPham_CellValueChanged;
            }
        }

        private void NapComboBoxHangSanXuat()
        {
            try
            {
                DataTable dt = _busSP.LayDanhSachHSX();
                comboBoxHangSanXuat.DataSource = dt;
                comboBoxHangSanXuat.DisplayMember = "TenHang";
                comboBoxHangSanXuat.ValueMember = "MaHang";
                comboBoxHangSanXuat.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hãng sản xuất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData(string maLoai = "", string tenLoai = "", string maHang = "", string danhMuc = "", string thoiGianBH = "", string giaBanGoc = "")
        {
            try
            {
                DataTable dt = _busSP.LayDanhSachLoaiSP();

                string filter = "";
                if (!string.IsNullOrWhiteSpace(maLoai))
                {
                    filter += $"(MaLoaiSP LIKE '%{maLoai.Replace("'", "''")}%')";
                }
                if (!string.IsNullOrWhiteSpace(tenLoai))
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(TenLoai LIKE '%{tenLoai.Replace("'", "''")}%')";
                }
                if (!string.IsNullOrWhiteSpace(maHang))
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(MaHang = '{maHang.Replace("'", "''")}')";
                }
                if (!string.IsNullOrWhiteSpace(danhMuc))
                {
                    if (filter != "") filter += " AND ";
                    string normalizedDanhMuc = danhMuc;
                    if (danhMuc.Equals("Bàn phím", StringComparison.OrdinalIgnoreCase))
                        normalizedDanhMuc = "Bàn Phím";
                    filter += $"(DanhMuc LIKE '%{normalizedDanhMuc.Replace("'", "''")}%')";
                }
                if (!string.IsNullOrWhiteSpace(thoiGianBH))
                {
                    if (int.TryParse(thoiGianBH, out int bh))
                    {
                        if (filter != "") filter += " AND ";
                        filter += $"(ThoiGianBaoHanh = {bh})";
                    }
                }
                if (!string.IsNullOrWhiteSpace(giaBanGoc))
                {
                    if (decimal.TryParse(giaBanGoc, out decimal gia))
                    {
                        if (filter != "") filter += " AND ";
                        filter += $"(GiaBanGoc = {gia})";
                    }
                }

                if (filter != "")
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = filter;
                    dt = dv.ToTable();
                }

                dataGridViewDSLoaiSanPham.DataSource = dt;

                // Định cấu hình các cột hiển thị
                if (dataGridViewDSLoaiSanPham.Columns["MaLoaiSP"] != null)
                    dataGridViewDSLoaiSanPham.Columns["MaLoaiSP"].HeaderText = "Mã loại";
                if (dataGridViewDSLoaiSanPham.Columns["MaHang"] != null)
                    dataGridViewDSLoaiSanPham.Columns["MaHang"].HeaderText = "Mã hãng";
                if (dataGridViewDSLoaiSanPham.Columns["TenLoai"] != null)
                    dataGridViewDSLoaiSanPham.Columns["TenLoai"].HeaderText = "Tên loại sản phẩm";
                if (dataGridViewDSLoaiSanPham.Columns["DanhMuc"] != null)
                    dataGridViewDSLoaiSanPham.Columns["DanhMuc"].HeaderText = "Danh mục";
                if (dataGridViewDSLoaiSanPham.Columns["ThoiGianBaoHanh"] != null)
                    dataGridViewDSLoaiSanPham.Columns["ThoiGianBaoHanh"].HeaderText = "Bảo hành (tháng)";
                if (dataGridViewDSLoaiSanPham.Columns["GiaBanGoc"] != null)
                    dataGridViewDSLoaiSanPham.Columns["GiaBanGoc"].HeaderText = "Giá bán gốc";
                if (dataGridViewDSLoaiSanPham.Columns["NgayTao"] != null)
                    dataGridViewDSLoaiSanPham.Columns["NgayTao"].HeaderText = "Ngày tạo";
                if (dataGridViewDSLoaiSanPham.Columns["NgayCapNhat"] != null)
                    dataGridViewDSLoaiSanPham.Columns["NgayCapNhat"].HeaderText = "Ngày cập nhật";
                if (dataGridViewDSLoaiSanPham.Columns["IsDeleted"] != null)
                {
                    dataGridViewDSLoaiSanPham.Columns["IsDeleted"].Visible = true;
                    dataGridViewDSLoaiSanPham.Columns["IsDeleted"].HeaderText = "Đã Xóa";
                }
                if (dataGridViewDSLoaiSanPham.Columns["NguoiTao"] != null)
                    dataGridViewDSLoaiSanPham.Columns["NguoiTao"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách loại sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSLoaiSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSLoaiSanPham.Rows[e.RowIndex];

            _maLoaiDangChon = row.Cells["MaLoaiSP"]?.Value?.ToString()?.Trim();
            txtMaLoaiSanPham.Text = _maLoaiDangChon;
            txtTenLoaiSanPham.Text = row.Cells["TenLoai"]?.Value?.ToString()?.Trim();
            
            string maHang = row.Cells["MaHang"]?.Value?.ToString()?.Trim() ?? "";
            comboBoxHangSanXuat.SelectedValue = maHang;

            string danhMuc = row.Cells["DanhMuc"]?.Value?.ToString()?.Trim() ?? "";
            // Chuẩn hóa hiển thị danh mục
            if (danhMuc.Equals("Bàn Phím", StringComparison.OrdinalIgnoreCase))
                comboBoxDanhMucSanPham.Text = "Bàn phím";
            else
                comboBoxDanhMucSanPham.Text = danhMuc;

            textBox2.Text = row.Cells["ThoiGianBaoHanh"]?.Value?.ToString()?.Trim();
            textBox3.Text = row.Cells["GiaBanGoc"]?.Value?.ToString()?.Trim();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string maLoai = txtMaLoaiSanPham.Text.Trim();
            string tenLoai = txtTenLoaiSanPham.Text.Trim();
            string maHang = comboBoxHangSanXuat.SelectedValue?.ToString() ?? "";
            string danhMuc = comboBoxDanhMucSanPham.Text.Trim();
            string thoiGianBH = textBox2.Text.Trim();
            string giaBanGoc = textBox3.Text.Trim();

            LoadData(maLoai, tenLoai, maHang, danhMuc, thoiGianBH, giaBanGoc);
        }

        private void dataGridViewDSLoaiSanPham_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSLoaiSanPham.Rows[e.RowIndex];
            string colName = dataGridViewDSLoaiSanPham.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "TenLoai")
            {
                txtTenLoaiSanPham.Text = val?.ToString()?.Trim();
            }
            else if (colName == "ThoiGianBaoHanh")
            {
                textBox2.Text = val?.ToString()?.Trim();
            }
            else if (colName == "GiaBanGoc")
            {
                textBox3.Text = val?.ToString()?.Trim();
            }
            else if (colName == "DanhMuc")
            {
                string dm = val?.ToString()?.Trim() ?? "";
                if (dm.Equals("Bàn Phím", StringComparison.OrdinalIgnoreCase))
                    comboBoxDanhMucSanPham.Text = "Bàn phím";
                else
                    comboBoxDanhMucSanPham.Text = dm;
            }
            else if (colName == "MaHang")
            {
                comboBoxHangSanXuat.SelectedValue = val?.ToString()?.Trim();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var lsp = DocDuLieuForm();
                lsp.MaLoaiSP = TaoMaLoaiSPMoi();

                if (_busSP.ThemLoaiSP(lsp))
                {
                    MessageBox.Show("Thêm loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_maLoaiDangChon))
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaLoaiSanPham.Text.Trim() != _maLoaiDangChon)
            {
                MessageBox.Show("Không được phép sửa mã loại sản phẩm", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi thông tin loại sản phẩm này không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                var lsp = DocDuLieuForm();
                lsp.MaLoaiSP = _maLoaiDangChon;

                if (_busSP.CapNhatLoaiSP(lsp))
                {
                    MessageBox.Show("Cập nhật loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_maLoaiDangChon))
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa loại sản phẩm '{_maLoaiDangChon}' không?\n(Dữ liệu sẽ bị xóa mềm, không xóa vật lý)",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (_busSP.XoaLoaiSP(_maLoaiDangChon))
                {
                    MessageBox.Show("Xóa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private DTO_LoaiSanPham DocDuLieuForm()
        {
            string tenLoai = txtTenLoaiSanPham.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenLoai))
                throw new ArgumentException("Tên loại sản phẩm không được để trống.");

            if (comboBoxHangSanXuat.SelectedValue == null)
                throw new ArgumentException("Vui lòng chọn hãng sản xuất.");
            string maHang = comboBoxHangSanXuat.SelectedValue.ToString()!.Trim();

            string danhMucSelected = comboBoxDanhMucSanPham.Text.Trim();
            if (string.IsNullOrWhiteSpace(danhMucSelected))
                throw new ArgumentException("Vui lòng chọn hoặc nhập danh mục.");
            
            // Chuẩn hóa danh mục theo quy định database ("Laptop", "Chuột", "Bàn Phím")
            string danhMuc = danhMucSelected;
            if (danhMucSelected.Equals("Bàn phím", StringComparison.OrdinalIgnoreCase))
                danhMuc = "Bàn Phím";
            else if (danhMucSelected.Equals("Laptop", StringComparison.OrdinalIgnoreCase))
                danhMuc = "Laptop";
            else if (danhMucSelected.Equals("Chuột", StringComparison.OrdinalIgnoreCase))
                danhMuc = "Chuột";

            string thoiGianBHStr = textBox2.Text.Trim();
            if (!int.TryParse(thoiGianBHStr, out int thoiGianBH) || thoiGianBH <= 0)
                throw new ArgumentException("Thời gian bảo hành phải là số nguyên dương (tháng).");

            string giaBanGocStr = textBox3.Text.Trim();
            if (!decimal.TryParse(giaBanGocStr, out decimal giaBanGoc) || giaBanGoc < 0)
                throw new ArgumentException("Giá bán gốc phải là số lớn hơn hoặc bằng 0.");

            return new DTO_LoaiSanPham
            {
                MaHang = maHang,
                TenLoai = tenLoai,
                DanhMuc = danhMuc,
                ThoiGianBaoHanh = thoiGianBH,
                GiaBanGoc = giaBanGoc
            };
        }

        private void LamMoiForm()
        {
            _maLoaiDangChon = null;
            txtMaLoaiSanPham.Clear();
            txtTenLoaiSanPham.Clear();
            comboBoxHangSanXuat.SelectedIndex = -1;
            comboBoxDanhMucSanPham.SelectedIndex = -1;
            textBox2.Clear();
            textBox3.Clear();
        }

        private void dataGridViewDSLoaiSanPham_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSLoaiSanPham.Rows[e.RowIndex];
            
            try
            {
                string maLoai = row.Cells["MaLoaiSP"]?.Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrEmpty(maLoai)) return;

                string maHang = row.Cells["MaHang"]?.Value?.ToString()?.Trim() ?? "";
                string tenLoai = row.Cells["TenLoai"]?.Value?.ToString()?.Trim() ?? "";
                string danhMuc = row.Cells["DanhMuc"]?.Value?.ToString()?.Trim() ?? "";
                int thoiGianBH = Convert.ToInt32(row.Cells["ThoiGianBaoHanh"]?.Value ?? 0);
                decimal giaBanGoc = Convert.ToDecimal(row.Cells["GiaBanGoc"]?.Value ?? 0);
                bool isDeleted = Convert.ToBoolean(row.Cells["IsDeleted"]?.Value ?? false);

                if (isDeleted)
                {
                    DataTable dsSP = _busSP.LayDanhSachTheoLoaiSP(maLoai);
                    int soConLai = 0;
                    foreach (DataRow r in dsSP.Rows)
                    {
                        string tt = r["TrangThai"]?.ToString()?.Trim() ?? "";
                        if (tt != "Đã Bán")
                            soConLai++;
                    }
                    if (soConLai > 0)
                    {
                        MessageBox.Show($"Không thể xóa loại sản phẩm '{maLoai}' vì còn {soConLai} sản phẩm chưa được bán (TrangThai khác 'Đã Bán'). Hãy xử lý hết hàng tồn kho trước.", "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadData();
                        return;
                    }
                }

                DTO_LoaiSanPham lsp = new DTO_LoaiSanPham
                {
                    MaLoaiSP = maLoai,
                    MaHang = maHang,
                    TenLoai = tenLoai,
                    DanhMuc = danhMuc,
                    ThoiGianBaoHanh = thoiGianBH,
                    GiaBanGoc = giaBanGoc,
                    IsDeleted = isDeleted,
                    NgayCapNhat = DateTime.Now
                };

                if (_busSP.CapNhatLoaiSP(lsp))
                {
                    if (maLoai == _maLoaiDangChon)
                    {
                        txtTenLoaiSanPham.Text = tenLoai;
                        comboBoxHangSanXuat.SelectedValue = maHang;
                        comboBoxDanhMucSanPham.Text = danhMuc;
                        textBox2.Text = thoiGianBH.ToString();
                        textBox3.Text = giaBanGoc.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật loại sản phẩm trực tiếp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dòng trực tiếp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
            }
        }

        private string TaoMaLoaiSPMoi()
        {
            try
            {
                DataTable dt = _busSP.LayDanhSachLoaiSP();
                int soLon = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string maLoai = row["MaLoaiSP"]?.ToString()?.Trim() ?? "";
                    if (maLoai.StartsWith("LSP") && maLoai.Length == 10)
                    {
                        if (int.TryParse(maLoai.Substring(3), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "LSP" + (soLon + 1).ToString().PadLeft(7, '0');
            }
            catch
            {
                return "LSP0000001";
            }
        }
    }
}

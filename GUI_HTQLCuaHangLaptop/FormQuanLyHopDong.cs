using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyHopDong : Form
    {
        // ── BUS ─────────────────────────────────────────────────────────
        private readonly BUS_HopDong _busHD = new BUS_HopDong();

        // ── Mã nhân viên đang đăng nhập ──────────────────────────────
        private readonly string _maNV;
        private bool _isLoading = false;

        public FormQuanLyHopDong(string maNV = "NV00000001")
        {
            InitializeComponent();
            _maNV = maNV;

            // Đăng ký sự kiện (btnTim.Click đã được đăng ký trong Designer)
            this.Load += FormQuanLyHopDong_Load;
            btnThem.Click += BtnThem_Click;
            btnSua.Click  += BtnSua_Click;
            btnXoa.Click  += BtnXoa_Click;
            dataGridViewDSHopDong.CellClick += DgvHopDong_CellClick;
        }

        // ══════════════════════════════════════════════════════════════════
        // FORM LOAD
        // ══════════════════════════════════════════════════════════════════
        private void FormQuanLyHopDong_Load(object sender, EventArgs e)
        {
            _isLoading = true;

            // Cấu hình DateTimePickers
            dateTimePickerNgayKy.ShowCheckBox = true;
            dateTimePickerNgayKy.Checked = false;
            dateTimePickerNgayHieuLuc.ShowCheckBox = true;
            dateTimePickerNgayHieuLuc.Checked = false;
            dateTimePickerNgayHetHan.ShowCheckBox = true;
            dateTimePickerNgayHetHan.Checked = false;

            // Thiết lập ComboBox trạng thái
            comboBoxTrangThai.Items.Clear();
            comboBoxTrangThai.Items.AddRange(new object[] { "Tất cả", "Hiệu Lực", "Hết Hạn", "Huỷ" });
            comboBoxTrangThai.SelectedIndex = -1;

            // Load danh sách khách hàng
            NapComboBoxMaKhachHang();

            // Load danh sách hợp đồng
            LoadDanhSachHopDong();

            // Set grid to be editable except MaHD
            dataGridViewDSHopDong.ReadOnly = false;
            foreach (DataGridViewColumn col in dataGridViewDSHopDong.Columns)
            {
                if (col.Name == "MaKH" || col.Name == "GiaTriHD" || col.Name == "NgayKy" || col.Name == "NgayHieuLuc" || col.Name == "NgayHetHan" || col.Name == "TrangThai")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }
            dataGridViewDSHopDong.CellValueChanged += dataGridViewDSHopDong_CellValueChanged;

            _isLoading = false;
            LamMoiForm();
        }

        private void LamMoiForm()
        {
            comboBoxMaKhachHang.SelectedIndex = -1;
            txtGiaTriHopDong.Clear();
            dateTimePickerNgayKy.Checked = false;
            dateTimePickerNgayHieuLuc.Checked = false;
            dateTimePickerNgayHetHan.Checked = false;
            comboBoxTrangThai.SelectedIndex = -1;
        }

        // ══════════════════════════════════════════════════════════════════
        // TẢI DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════
        private void LoadDanhSachHopDong()
        {
            try
            {
                dataGridViewDSHopDong.DataSource = _busHD.LayDanhSachHopDong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách hợp đồng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CHỌN DÒNG → ĐỔ DỮ LIỆU LÊN FORM
        private void DgvHopDong_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridViewDSHopDong.Rows[e.RowIndex];

            try
            {
                comboBoxMaKhachHang.Text = row.Cells["MaKH"].Value?.ToString() ?? "";
                txtGiaTriHopDong.Text  = row.Cells["GiaTriHD"].Value?.ToString() ?? "";

                if (DateTime.TryParse(row.Cells["NgayKy"].Value?.ToString(), out DateTime ngayKy))
                {
                    dateTimePickerNgayKy.Value = ngayKy;
                    dateTimePickerNgayKy.Checked = true;
                }
                else
                {
                    dateTimePickerNgayKy.Checked = false;
                }

                if (DateTime.TryParse(row.Cells["NgayHieuLuc"].Value?.ToString(), out DateTime ngayHL))
                {
                    dateTimePickerNgayHieuLuc.Value = ngayHL;
                    dateTimePickerNgayHieuLuc.Checked = true;
                }
                else
                {
                    dateTimePickerNgayHieuLuc.Checked = false;
                }

                if (DateTime.TryParse(row.Cells["NgayHetHan"].Value?.ToString(), out DateTime ngayHH))
                {
                    dateTimePickerNgayHetHan.Value = ngayHH;
                    dateTimePickerNgayHetHan.Checked = true;
                }
                else
                {
                    dateTimePickerNgayHetHan.Checked = false;
                }

                string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "Hiệu Lực";
                int idx = comboBoxTrangThai.Items.IndexOf(trangThai);
                if (idx >= 0) 
                    comboBoxTrangThai.SelectedIndex = idx;
                else
                    comboBoxTrangThai.Text = trangThai;
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN THÊM — Thêm hợp đồng mới
        // ══════════════════════════════════════════════════════════════════
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(comboBoxMaKhachHang.Text) || comboBoxMaKhachHang.Text == "Tất cả" || comboBoxMaKhachHang.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập một mã khách hàng sỉ cụ thể. Không chọn 'Tất cả'.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtGiaTriHopDong.Text.Trim(), out decimal giaTriHD) || giaTriHD < 0)
            {
                MessageBox.Show("Giá trị hợp đồng phải là số không âm.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dateTimePickerNgayHetHan.Value.Date <= dateTimePickerNgayHieuLuc.Value.Date)
            {
                MessageBox.Show("Ngày hết hạn phải sau ngày hiệu lực.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Sinh mã HD mới
                string maHDMoi = SinhMaHDMoi();

                // Mã NV từ session đăng nhập
                string maNV = _maNV;

                var hd = new DTO_HopDong
                {
                    MaHD         = maHDMoi,
                    MaNV         = maNV,
                    MaKH         = comboBoxMaKhachHang.Text.Trim(),
                    NgayKy       = dateTimePickerNgayKy.Value.Date,
                    GiaTriHD     = giaTriHD,
                    NgayHieuLuc  = dateTimePickerNgayHieuLuc.Value.Date,
                    NgayHetHan   = dateTimePickerNgayHetHan.Value.Date,
                    TrangThai    = "Hiệu Lực",
                };

                bool ok = _busHD.ThemHopDong(hd);
                if (ok)
                {
                    MessageBox.Show($"✅ Thêm hợp đồng '{maHDMoi}' thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachHopDong();
                    LamMoiForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm hợp đồng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN SỬA — Cập nhật hợp đồng (chỉ khi Hiệu Lực)
        // ══════════════════════════════════════════════════════════════════
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (dataGridViewDSHopDong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hợp đồng cần sửa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHD = dataGridViewDSHopDong.CurrentRow.Cells["MaHD"].Value?.ToString()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(maHD)) return;

            var existingHD = _busHD.LayTheoMa(maHD);
            if (existingHD != null && existingHD.MaKH.Trim() != comboBoxMaKhachHang.Text.Trim())
            {
                MessageBox.Show("Không được phép sửa mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(comboBoxMaKhachHang.Text) || comboBoxMaKhachHang.Text == "Tất cả" || comboBoxMaKhachHang.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập một mã khách hàng sỉ cụ thể. Không chọn 'Tất cả'.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn thay đổi hợp đồng '{maHD}' không?",
                "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            if (!decimal.TryParse(txtGiaTriHopDong.Text.Trim(), out decimal giaTriHD) || giaTriHD < 0)
            {
                MessageBox.Show("Giá trị hợp đồng phải là số không âm.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dateTimePickerNgayHetHan.Value.Date <= dateTimePickerNgayHieuLuc.Value.Date)
            {
                MessageBox.Show("Ngày hết hạn phải sau ngày hiệu lực.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var hd = new DTO_HopDong
                {
                    MaHD        = maHD,
                    MaNV        = _maNV,
                    MaKH        = comboBoxMaKhachHang.Text.Trim(),
                    NgayKy      = dateTimePickerNgayKy.Value.Date,
                    GiaTriHD    = giaTriHD,
                    NgayHieuLuc = dateTimePickerNgayHieuLuc.Value.Date,
                    NgayHetHan  = dateTimePickerNgayHetHan.Value.Date,
                    TrangThai   = comboBoxTrangThai.Text.Trim(),
                };

                bool ok = _busHD.CapNhat(hd);
                if (ok)
                {
                    MessageBox.Show("✅ Cập nhật hợp đồng thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachHopDong();
                    LamMoiForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sửa hợp đồng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN XÓA — Đặt trạng thái hợp đồng thành 'Huỷ' (không xóa vật lý)
        // ══════════════════════════════════════════════════════════════════
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (dataGridViewDSHopDong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hợp đồng cần hủy.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maHD = dataGridViewDSHopDong.CurrentRow.Cells["MaHD"].Value?.ToString()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(maHD)) return;

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn hủy hợp đồng '{maHD}' không?\n" +
                "Hợp đồng sẽ được đặt sang trạng thái 'Huỷ' và không thể dùng để tạo đơn hàng mới.",
                "Xác nhận hủy hợp đồng", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ok = _busHD.HuyHopDong(maHD);
                if (ok)
                {
                    MessageBox.Show($"✅ Đã hủy hợp đồng '{maHD}' thành công.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachHopDong();
                    LamMoiForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hủy hợp đồng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN TÌM — Tìm kiếm hợp đồng theo nhiều tiêu chí kết hợp
        // ══════════════════════════════════════════════════════════════════
        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = _busHD.LayDanhSachHopDong();
                var rows = dt.AsEnumerable();

                // 1. Lọc theo mã khách hàng
                string textMaKH = comboBoxMaKhachHang.Text.Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(textMaKH) && textMaKH != "tất cả")
                {
                    rows = rows.Where(r => r.Field<string>("MaKH") != null && r.Field<string>("MaKH").ToLower().Contains(textMaKH));
                }

                // 2. Lọc theo giá trị hợp đồng
                if (!string.IsNullOrWhiteSpace(txtGiaTriHopDong.Text))
                {
                    if (decimal.TryParse(txtGiaTriHopDong.Text.Trim(), out decimal giaTri))
                    {
                        rows = rows.Where(r => r.Field<decimal>("GiaTriHD") == giaTri);
                    }
                }

                // 3. Lọc theo ngày ký
                if (dateTimePickerNgayKy.Checked)
                {
                    DateTime ngayKy = dateTimePickerNgayKy.Value.Date;
                    rows = rows.Where(r => r.Field<DateTime>("NgayKy").Date == ngayKy);
                }

                // 4. Lọc theo ngày hiệu lực
                if (dateTimePickerNgayHieuLuc.Checked)
                {
                    DateTime ngayHL = dateTimePickerNgayHieuLuc.Value.Date;
                    rows = rows.Where(r => r.Field<DateTime>("NgayHieuLuc").Date == ngayHL);
                }

                // 5. Lọc theo ngày hết hạn
                if (dateTimePickerNgayHetHan.Checked)
                {
                    DateTime ngayHH = dateTimePickerNgayHetHan.Value.Date;
                    rows = rows.Where(r => r.Field<DateTime>("NgayHetHan").Date == ngayHH);
                }

                // 6. Lọc theo trạng thái
                if (comboBoxTrangThai.SelectedIndex >= 0 && comboBoxTrangThai.Text != "Tất cả")
                {
                    string trangThai = comboBoxTrangThai.Text.Trim();
                    rows = rows.Where(r => r.Field<string>("TrangThai") != null && r.Field<string>("TrangThai").Trim() == trangThai);
                }

                DataTable filteredDt = rows.Any() ? rows.CopyToDataTable() : dt.Clone();
                dataGridViewDSHopDong.DataSource = filteredDt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // HELPER — Sinh mã hợp đồng mới tự động
        // ══════════════════════════════════════════════════════════════════
        private string SinhMaHDMoi()
        {
            try
            {
                var dt = _busHD.LayDanhSachHopDong();
                int maxSo = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string ma = row["MaHD"].ToString()!.Trim();
                    if (ma.StartsWith("HD") && ma.Length == 10)
                    {
                        if (int.TryParse(ma.Substring(2), out int so) && so > maxSo)
                            maxSo = so;
                    }
                }
                return "HD" + (maxSo + 1).ToString().PadLeft(8, '0');
            }
            catch
            {
                return "HD" + DateTime.Now.Ticks.ToString().Substring(0, 8);
            }
        }

        private void dataGridViewDSHopDong_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSHopDong.Rows[e.RowIndex];
            string colName = dataGridViewDSHopDong.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "MaKH")
            {
                comboBoxMaKhachHang.Text = val?.ToString()?.Trim();
            }
            else if (colName == "GiaTriHD")
            {
                txtGiaTriHopDong.Text = val?.ToString()?.Trim();
            }
            else if (colName == "NgayKy")
            {
                if (DateTime.TryParse(val?.ToString(), out DateTime dt))
                    dateTimePickerNgayKy.Value = dt;
            }
            else if (colName == "NgayHieuLuc")
            {
                if (DateTime.TryParse(val?.ToString(), out DateTime dt))
                    dateTimePickerNgayHieuLuc.Value = dt;
            }
            else if (colName == "NgayHetHan")
            {
                if (DateTime.TryParse(val?.ToString(), out DateTime dt))
                    dateTimePickerNgayHetHan.Value = dt;
            }
            else if (colName == "TrangThai")
            {
                string status = val?.ToString()?.Trim() ?? "Hiệu Lực";
                int idx = comboBoxTrangThai.Items.IndexOf(status);
                if (idx >= 0) comboBoxTrangThai.SelectedIndex = idx;
            }
        }

        private void NapComboBoxMaKhachHang()
        {
            try
            {
                BUS_KhachHang busKH = new BUS_KhachHang();
                DataTable dt = busKH.LayDanhSach();

                var filtered = dt.AsEnumerable()
                    .Where(r => r.Field<string>("LoaiKH") != null && r.Field<string>("LoaiKH").Trim() == "Sỉ"
                             && (r.Field<bool?>("IsDeleted") == null || r.Field<bool?>("IsDeleted") == false));

                DataTable dtFiltered = filtered.Any() ? filtered.CopyToDataTable() : dt.Clone();

                DataRow newRow = dtFiltered.NewRow();
                newRow["MaKH"] = "Tất cả";
                dtFiltered.Rows.InsertAt(newRow, 0);

                comboBoxMaKhachHang.DataSource = dtFiltered;
                comboBoxMaKhachHang.DisplayMember = "MaKH";
                comboBoxMaKhachHang.ValueMember = "MaKH";
                comboBoxMaKhachHang.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

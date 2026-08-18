using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyKhuyenMai : Form
    {
        // ── BUS ─────────────────────────────────────────────────────────
        private readonly BUS_KhuyenMai _busKM = new BUS_KhuyenMai();

        public FormQuanLyKhuyenMai()
        {
            InitializeComponent();

            // Đăng ký sự kiện
            this.Load += FormQuanLyKhuyenMai_Load;
            btnThem.Click += BtnThem_Click;
            btnSua.Click  += BtnSua_Click;
            btnTim.Click  += BtnTim_Click;
            dataGridViewDSKhuyenMai.SelectionChanged += DgvKM_SelectionChanged;
            dataGridViewDSKhuyenMai.CellEndEdit += dataGridViewDSKhuyenMai_CellEndEdit;
        }

        // ══════════════════════════════════════════════════════════════════
        // FORM LOAD
        // ══════════════════════════════════════════════════════════════════
        private void FormQuanLyKhuyenMai_Load(object sender, EventArgs e)
        {
            // ComboBox đối tượng áp dụng đã được thiết kế sẵn
            // Đảm bảo đúng giá trị
            comboBoxDoiTuongApDung.Items.Clear();
            comboBoxDoiTuongApDung.Items.AddRange(new object[] { "Tất Cả", "HSSV", "Doanh Nghiệp", "Null" });
            comboBoxDoiTuongApDung.SelectedIndex = 0;

            // Cấu hình DateTimePickers
            dateTimePickerNgayBatDau.ShowCheckBox = true;
            dateTimePickerNgayBatDau.Checked = false;
            dateTimePickerNgayKetThuc.ShowCheckBox = true;
            dateTimePickerNgayKetThuc.Checked = false;

            // Tạm thời gỡ bỏ sự kiện SelectionChanged để tránh ghi đè dữ liệu đầu vào khi load dữ liệu dòng đầu
            dataGridViewDSKhuyenMai.SelectionChanged -= DgvKM_SelectionChanged;

            LoadDanhSachKhuyenMai();

            dataGridViewDSKhuyenMai.ClearSelection();

            // Reset các ô nhập liệu
            txtTenChuongTrinh.Clear();
            txtDieuKien.Clear();
            txtGiamTheoSanPham.Clear();
            txtGiamTheoDonHang.Clear();
            txtSoLuongToiThieu.Clear();
            comboBoxDoiTuongApDung.SelectedIndex = 0; // "Tất Cả"
            dateTimePickerNgayBatDau.Checked = false;
            dateTimePickerNgayKetThuc.Checked = false;

            // Đăng ký lại sự kiện
            dataGridViewDSKhuyenMai.SelectionChanged += DgvKM_SelectionChanged;

            // Set grid to be editable except MaKM
            dataGridViewDSKhuyenMai.ReadOnly = false;
            foreach (DataGridViewColumn col in dataGridViewDSKhuyenMai.Columns)
            {
                if (col.Name == "TenKM" || col.Name == "DieuKien" || col.Name == "DoiTuong" || col.Name == "NgayBatDau" || col.Name == "NgayKetThuc" || col.Name == "MucGiamSP" || col.Name == "MucGiamDH" || col.Name == "SLToiThieu" || col.Name == "isHienThi" || col.Name == "IsHienThi")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }
            dataGridViewDSKhuyenMai.CellValueChanged += dataGridViewDSKhuyenMai_CellValueChanged;
        }

        // ══════════════════════════════════════════════════════════════════
        // TẢI DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════
        private void LoadDanhSachKhuyenMai()
        {
            try
            {
                dataGridViewDSKhuyenMai.DataSource = _busKM.LayDanhSachKhuyenMai();
                if (dataGridViewDSKhuyenMai.Columns["isHienThi"] != null)
                    dataGridViewDSKhuyenMai.Columns["isHienThi"].HeaderText = "Hiển thị Web";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách khuyến mãi: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CHỌN DÒNG → ĐỔ DỮ LIỆU LÊN FORM
        // ══════════════════════════════════════════════════════════════════
        private void DgvKM_SelectionChanged(object? sender, EventArgs e)
        {
            if (dataGridViewDSKhuyenMai.CurrentRow == null || dataGridViewDSKhuyenMai.SelectedRows.Count == 0) return;
            var row = dataGridViewDSKhuyenMai.CurrentRow;

            try
            {
                txtTenChuongTrinh.Text  = row.Cells["TenKM"].Value?.ToString()      ?? "";
                txtDieuKien.Text        = row.Cells["DieuKien"].Value?.ToString()   ?? "";

                string doiTuong = row.Cells["DoiTuong"].Value?.ToString() ?? "Tất Cả";
                if (string.IsNullOrWhiteSpace(doiTuong)) doiTuong = "Null";
                int idx = comboBoxDoiTuongApDung.Items.IndexOf(doiTuong);
                if (idx >= 0) comboBoxDoiTuongApDung.SelectedIndex = idx;

                if (DateTime.TryParse(row.Cells["NgayBatDau"].Value?.ToString(), out DateTime ngayBD))
                    dateTimePickerNgayBatDau.Value = ngayBD;
                if (DateTime.TryParse(row.Cells["NgayKetThuc"].Value?.ToString(), out DateTime ngayKT))
                    dateTimePickerNgayKetThuc.Value = ngayKT;

                // MucGiamSP và MucGiamDH
                object mucSP = row.Cells["MucGiamSP"].Value;
                object mucDH = row.Cells["MucGiamDH"].Value;
                txtGiamTheoSanPham.Text  = (mucSP == null || mucSP == DBNull.Value) ? "" : mucSP.ToString();
                txtGiamTheoDonHang.Text  = (mucDH == null || mucDH == DBNull.Value) ? "" : mucDH.ToString();

                object sl = row.Cells["SLToiThieu"].Value;
                txtSoLuongToiThieu.Text = (sl == null || sl == DBNull.Value) ? "" : sl.ToString();
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN THÊM — Thêm chương trình khuyến mãi mới
        // ══════════════════════════════════════════════════════════════════
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenChuongTrinh.Text))
            {
                MessageBox.Show("Vui lòng nhập tên chương trình khuyến mãi.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dateTimePickerNgayKetThuc.Value.Date < dateTimePickerNgayBatDau.Value.Date)
            {
                MessageBox.Show("Ngày kết thúc phải >= ngày bắt đầu.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate mức giảm
            decimal? mucGiamSP = null, mucGiamDH = null;
            if (!string.IsNullOrWhiteSpace(txtGiamTheoSanPham.Text))
            {
                if (!decimal.TryParse(txtGiamTheoSanPham.Text.Trim(), out decimal val) || val < 0 || val > 100)
                {
                    MessageBox.Show("Mức giảm theo sản phẩm phải trong khoảng 0–100.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                mucGiamSP = val;
            }
            if (!string.IsNullOrWhiteSpace(txtGiamTheoDonHang.Text))
            {
                if (!decimal.TryParse(txtGiamTheoDonHang.Text.Trim(), out decimal val) || val < 0 || val > 100)
                {
                    MessageBox.Show("Mức giảm theo đơn hàng phải trong khoảng 0–100.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                mucGiamDH = val;
            }
            if (mucGiamSP.HasValue && mucGiamDH.HasValue)
            {
                MessageBox.Show("Chỉ được nhập MỘT trong hai: Giảm theo sản phẩm HOẶC Giảm theo đơn hàng.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!mucGiamSP.HasValue && !mucGiamDH.HasValue)
            {
                MessageBox.Show("Phải nhập ít nhất một mức giảm (theo sản phẩm hoặc theo đơn hàng).",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? slToiThieu = null;
            if (!string.IsNullOrWhiteSpace(txtSoLuongToiThieu.Text))
            {
                if (!int.TryParse(txtSoLuongToiThieu.Text.Trim(), out int sl) || sl < 0)
                {
                    MessageBox.Show("Số lượng tối thiểu phải là số nguyên không âm.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                slToiThieu = sl;
            }

            try
            {
                string maKMMoi = SinhMaKMMoi();

                var km = new DTO_KhuyenMai
                {
                    MaKM         = maKMMoi,
                    TenKM        = txtTenChuongTrinh.Text.Trim(),
                    DoiTuong     = (string.IsNullOrEmpty(comboBoxDoiTuongApDung.Text) || comboBoxDoiTuongApDung.Text.Trim().Equals("Null", StringComparison.OrdinalIgnoreCase)) ? "Tất Cả" : comboBoxDoiTuongApDung.Text.Trim(),
                    DieuKien     = string.IsNullOrWhiteSpace(txtDieuKien.Text) ? null! : txtDieuKien.Text.Trim(),
                    NgayBatDau   = dateTimePickerNgayBatDau.Value.Date,
                    NgayKetThuc  = dateTimePickerNgayKetThuc.Value.Date,
                    MucGiamSP    = mucGiamSP,
                    MucGiamDH    = mucGiamDH,
                    SLToiThieu   = slToiThieu,
                    IsHienThi    = true,
                    NgayTao      = DateTime.Now,
                };

                bool ok = _busKM.ThemKhuyenMai(km);
                if (ok)
                {
                    MessageBox.Show($"✅ Thêm chương trình khuyến mãi '{maKMMoi}' thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachKhuyenMai();
                    XoaFormNhapLieu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm khuyến mãi: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN SỬA — Cập nhật chương trình khuyến mãi
        // (Để hủy KM: chỉ cần đặt NgayKetThuc về quá khứ)
        // ══════════════════════════════════════════════════════════════════
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (dataGridViewDSKhuyenMai.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn chương trình khuyến mãi cần sửa.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKM = dataGridViewDSKhuyenMai.CurrentRow.Cells["MaKM"].Value?.ToString()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(maKM)) return;

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn thay đổi chương trình khuyến mãi '{maKM}' không?\n\n" +
                "💡 Gợi ý: Để hủy khuyến mãi, hãy đặt ngày kết thúc về quá khứ.",
                "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            if (string.IsNullOrWhiteSpace(txtTenChuongTrinh.Text))
            {
                MessageBox.Show("Vui lòng nhập tên chương trình khuyến mãi.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dateTimePickerNgayKetThuc.Value.Date < dateTimePickerNgayBatDau.Value.Date)
            {
                MessageBox.Show("Ngày kết thúc phải >= ngày bắt đầu.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal? mucGiamSP = null, mucGiamDH = null;
            if (!string.IsNullOrWhiteSpace(txtGiamTheoSanPham.Text))
            {
                if (!decimal.TryParse(txtGiamTheoSanPham.Text.Trim(), out decimal val) || val < 0 || val > 100)
                {
                    MessageBox.Show("Mức giảm theo sản phẩm phải trong khoảng 0–100.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                mucGiamSP = val;
            }
            if (!string.IsNullOrWhiteSpace(txtGiamTheoDonHang.Text))
            {
                if (!decimal.TryParse(txtGiamTheoDonHang.Text.Trim(), out decimal val) || val < 0 || val > 100)
                {
                    MessageBox.Show("Mức giảm theo đơn hàng phải trong khoảng 0–100.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                mucGiamDH = val;
            }
            if (mucGiamSP.HasValue && mucGiamDH.HasValue)
            {
                MessageBox.Show("Chỉ được nhập MỘT trong hai: Giảm theo sản phẩm HOẶC Giảm theo đơn hàng.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!mucGiamSP.HasValue && !mucGiamDH.HasValue)
            {
                MessageBox.Show("Phải nhập ít nhất một mức giảm.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? slToiThieu = null;
            if (!string.IsNullOrWhiteSpace(txtSoLuongToiThieu.Text))
            {
                if (!int.TryParse(txtSoLuongToiThieu.Text.Trim(), out int sl) || sl < 0)
                {
                    MessageBox.Show("Số lượng tối thiểu phải là số nguyên không âm.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                slToiThieu = sl;
            }

            try
            {
                var km = new DTO_KhuyenMai
                {
                    MaKM        = maKM,
                    TenKM       = txtTenChuongTrinh.Text.Trim(),
                    DoiTuong    = (string.IsNullOrEmpty(comboBoxDoiTuongApDung.Text) || comboBoxDoiTuongApDung.Text.Trim().Equals("Null", StringComparison.OrdinalIgnoreCase)) ? "Tất Cả" : comboBoxDoiTuongApDung.Text.Trim(),
                    DieuKien    = string.IsNullOrWhiteSpace(txtDieuKien.Text) ? null! : txtDieuKien.Text.Trim(),
                    NgayBatDau  = dateTimePickerNgayBatDau.Value.Date,
                    NgayKetThuc = dateTimePickerNgayKetThuc.Value.Date,
                    MucGiamSP   = mucGiamSP,
                    MucGiamDH   = mucGiamDH,
                    SLToiThieu  = slToiThieu,
                    IsHienThi   = dataGridViewDSKhuyenMai.CurrentRow.Cells["isHienThi"].Value == DBNull.Value ? true : Convert.ToBoolean(dataGridViewDSKhuyenMai.CurrentRow.Cells["isHienThi"].Value),
                };

                bool ok = _busKM.CapNhatKhuyenMai(km);
                if (ok)
                {
                    // Kiểm tra xem người dùng có vừa đặt về quá khứ không
                    bool daQua = km.NgayKetThuc.Date < DateTime.Today;
                    string thongBao = $"✅ Cập nhật chương trình '{maKM}' thành công!";
                    if (daQua)
                        thongBao += "\n⚠️ Lưu ý: Khuyến mãi đã hết hạn (ngày kết thúc trong quá khứ). Khuyến mãi này sẽ không được áp dụng.";

                    MessageBox.Show(thongBao, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachKhuyenMai();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sửa khuyến mãi: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN TÌM — Tìm kiếm theo tên chương trình
        // ══════════════════════════════════════════════════════════════════
        private void BtnTim_Click(object? sender, EventArgs e)
        {
            dataGridViewDSKhuyenMai.SelectionChanged -= DgvKM_SelectionChanged;

            try
            {
                DataTable dtAll = _busKM.LayDanhSachKhuyenMai();
                
                string tenTim = txtTenChuongTrinh.Text.Trim();
                string doiTuong = comboBoxDoiTuongApDung.SelectedItem?.ToString() ?? "";
                string dieuKien = txtDieuKien.Text.Trim();
                string giamSP = txtGiamTheoSanPham.Text.Trim();
                string giamDH = txtGiamTheoDonHang.Text.Trim();
                string slToiThieu = txtSoLuongToiThieu.Text.Trim();

                string filter = "";

                if (!string.IsNullOrWhiteSpace(tenTim))
                {
                    filter += $"(TenKM LIKE '%{tenTim.Replace("'", "''")}%')";
                }

                if (doiTuong == "Null")
                {
                    if (filter != "") filter += " AND ";
                    filter += "(DoiTuong = 'Tất Cả' OR DoiTuong IS NULL OR DoiTuong = '')";
                }
                else if (doiTuong != "" && doiTuong != "Tất Cả")
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(DoiTuong = '{doiTuong}')";
                }

                if (!string.IsNullOrWhiteSpace(dieuKien))
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(DieuKien LIKE '%{dieuKien.Replace("'", "''")}%')";
                }

                if (!string.IsNullOrWhiteSpace(giamSP))
                {
                    if (decimal.TryParse(giamSP, out decimal decVal))
                    {
                        if (filter != "") filter += " AND ";
                        filter += $"(MucGiamSP = {decVal})";
                    }
                }

                if (!string.IsNullOrWhiteSpace(giamDH))
                {
                    if (decimal.TryParse(giamDH, out decimal decVal))
                    {
                        if (filter != "") filter += " AND ";
                        filter += $"(MucGiamDH = {decVal})";
                    }
                }

                if (!string.IsNullOrWhiteSpace(slToiThieu))
                {
                    if (int.TryParse(slToiThieu, out int intVal))
                    {
                        if (filter != "") filter += " AND ";
                        filter += $"(SLToiThieu = {intVal})";
                    }
                }

                if (dateTimePickerNgayBatDau.Checked)
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(NgayBatDau >= #{dateTimePickerNgayBatDau.Value.ToString("yyyy-MM-dd")}#)";
                }

                if (dateTimePickerNgayKetThuc.Checked)
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(NgayKetThuc <= #{dateTimePickerNgayKetThuc.Value.ToString("yyyy-MM-dd")}#)";
                }

                if (filter != "")
                {
                    DataView dv = dtAll.DefaultView;
                    dv.RowFilter = filter;
                    dtAll = dv.ToTable();
                }

                dataGridViewDSKhuyenMai.DataSource = dtAll;
                dataGridViewDSKhuyenMai.ClearSelection();

                if (dtAll.Rows.Count == 0)
                    MessageBox.Show("Không tìm thấy chương trình khuyến mãi nào khớp.",
                        "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dataGridViewDSKhuyenMai.SelectionChanged += DgvKM_SelectionChanged;
            }
        }

        private void dataGridViewDSKhuyenMai_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSKhuyenMai.Rows[e.RowIndex];
            
            try
            {
                string maKM = row.Cells["MaKM"]?.Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrEmpty(maKM)) return;

                string tenKM = row.Cells["TenKM"]?.Value?.ToString()?.Trim() ?? "";
                string doiTuong = row.Cells["DoiTuong"]?.Value?.ToString()?.Trim() ?? "Tất Cả";
                if (string.IsNullOrEmpty(doiTuong) || doiTuong.Equals("Null", StringComparison.OrdinalIgnoreCase))
                {
                    doiTuong = "Tất Cả";
                }
                string dieuKien = row.Cells["DieuKien"]?.Value?.ToString()?.Trim() ?? "";
                DateTime ngayBD = Convert.ToDateTime(row.Cells["NgayBatDau"].Value);
                DateTime ngayKT = Convert.ToDateTime(row.Cells["NgayKetThuc"].Value);
                
                decimal? mucGiamSP = row.Cells["MucGiamSP"].Value == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row.Cells["MucGiamSP"].Value);
                decimal? mucGiamDH = row.Cells["MucGiamDH"].Value == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row.Cells["MucGiamDH"].Value);
                int? slToiThieu = row.Cells["SLToiThieu"].Value == DBNull.Value ? (int?)null : Convert.ToInt32(row.Cells["SLToiThieu"].Value);
                bool isHienThi = Convert.ToBoolean(row.Cells["isHienThi"].Value ?? true);

                DTO_KhuyenMai km = new DTO_KhuyenMai
                {
                    MaKM = maKM,
                    TenKM = tenKM,
                    DoiTuong = doiTuong,
                    DieuKien = string.IsNullOrEmpty(dieuKien) ? null! : dieuKien,
                    NgayBatDau = ngayBD,
                    NgayKetThuc = ngayKT,
                    MucGiamSP = mucGiamSP,
                    MucGiamDH = mucGiamDH,
                    SLToiThieu = slToiThieu,
                    IsHienThi = isHienThi
                };

                if (_busKM.CapNhatKhuyenMai(km))
                {
                    if (maKM == dataGridViewDSKhuyenMai.CurrentRow?.Cells["MaKM"]?.Value?.ToString()?.Trim())
                    {
                        txtTenChuongTrinh.Text = tenKM;
                        txtDieuKien.Text = dieuKien;
                        txtGiamTheoSanPham.Text = mucGiamSP?.ToString() ?? "";
                        txtGiamTheoDonHang.Text = mucGiamDH?.ToString() ?? "";
                        txtSoLuongToiThieu.Text = slToiThieu?.ToString() ?? "";
                        
                        int idx = comboBoxDoiTuongApDung.Items.IndexOf(doiTuong);
                        if (idx >= 0) comboBoxDoiTuongApDung.SelectedIndex = idx;
                        
                        dateTimePickerNgayBatDau.Value = ngayBD;
                        dateTimePickerNgayKetThuc.Value = ngayKT;
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật khuyến mãi trực tiếp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadDanhSachKhuyenMai();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dòng trực tiếp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadDanhSachKhuyenMai();
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // HELPER — Sinh mã KM mới
        // ══════════════════════════════════════════════════════════════════
        private string SinhMaKMMoi()
        {
            try
            {
                var dt = _busKM.LayDanhSachKhuyenMai();
                int maxSo = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string ma = row["MaKM"].ToString()!.Trim();
                    if (ma.StartsWith("KM") && ma.Length == 10)
                    {
                        if (int.TryParse(ma.Substring(2), out int so) && so > maxSo)
                            maxSo = so;
                    }
                }
                return "KM" + (maxSo + 1).ToString().PadLeft(8, '0');
            }
            catch
            {
                return "KM" + DateTime.Now.Ticks.ToString().Substring(0, 8);
            }
        }

        private void XoaFormNhapLieu()
        {
            txtTenChuongTrinh.Clear();
            txtDieuKien.Clear();
            txtGiamTheoSanPham.Clear();
            txtGiamTheoDonHang.Clear();
            txtSoLuongToiThieu.Clear();
            comboBoxDoiTuongApDung.SelectedIndex = 0;
            dateTimePickerNgayBatDau.Value  = DateTime.Today;
            dateTimePickerNgayKetThuc.Value = DateTime.Today.AddMonths(1);
            dateTimePickerNgayBatDau.Checked = false;
            dateTimePickerNgayKetThuc.Checked = false;
        }

        private void dataGridViewDSKhuyenMai_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSKhuyenMai.Rows[e.RowIndex];
            string colName = dataGridViewDSKhuyenMai.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "TenKM")
            {
                txtTenChuongTrinh.Text = val?.ToString()?.Trim();
            }
            else if (colName == "DieuKien")
            {
                txtDieuKien.Text = val?.ToString()?.Trim();
            }
            else if (colName == "DoiTuong")
            {
                string dt = val?.ToString()?.Trim() ?? "Tất Cả";
                int idx = comboBoxDoiTuongApDung.Items.IndexOf(dt);
                if (idx >= 0) comboBoxDoiTuongApDung.SelectedIndex = idx;
            }
            else if (colName == "NgayBatDau")
            {
                if (DateTime.TryParse(val?.ToString(), out DateTime dt))
                    dateTimePickerNgayBatDau.Value = dt;
            }
            else if (colName == "NgayKetThuc")
            {
                if (DateTime.TryParse(val?.ToString(), out DateTime dt))
                    dateTimePickerNgayKetThuc.Value = dt;
            }
            else if (colName == "MucGiamSP")
            {
                txtGiamTheoSanPham.Text = val?.ToString()?.Trim();
            }
            else if (colName == "MucGiamDH")
            {
                txtGiamTheoDonHang.Text = val?.ToString()?.Trim();
            }
            else if (colName == "SLToiThieu")
            {
                txtSoLuongToiThieu.Text = val?.ToString()?.Trim();
            }
        }
    }
}

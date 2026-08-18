using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormBaoHanh : Form
    {
        private readonly BUS_HauMai _busHauMai = new BUS_HauMai();
        private readonly BUS_DonHang _busDonHang = new BUS_DonHang();
        private readonly BUS_SanPham _busSanPham = new BUS_SanPham();
        private readonly BUS_KhachHang _busKhachHang = new BUS_KhachHang();

        private string? _maPBHDangChon = null;
        private string _trangThaiBanDau = "";

        public FormBaoHanh()
        {
            InitializeComponent();

            this.Load += FormBaoHanh_Load;
            dataGridViewPhieuBaoHanh.CellClick += dataGridViewPhieuBaoHanh_CellClick;

            btnTaoPhieuBaoHanh.Click += btnTaoPhieuBaoHanh_Click;
            btnSuaPhieuBaoHanh.Click += btnSuaPhieuBaoHanh_Click;
            btnTimPhieuBaoHanh.Click += btnTimPhieuBaoHanh_Click;

            // Thiết lập giá trị mặc định cho Combobox Loại bảo hành
            comboBoxLoaiBaoHanh.Items.Clear();
            comboBoxLoaiBaoHanh.Items.AddRange(new object[] { "Tất cả", "Cửa hàng", "Hãng" });
            comboBoxLoaiBaoHanh.SelectedIndex = 0;

            // Thiết lập giá trị mặc định cho Combobox Trạng thái
            comboBoxTrangThai.Items.Clear();
            comboBoxTrangThai.Items.AddRange(new object[] { "Tất cả", "Đang xử lý", "Hoàn thành", "Từ chối" });
            comboBoxTrangThai.SelectedIndex = 0;
        }

        private void FormBaoHanh_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridViewPhieuBaoHanh.ReadOnly = false;
            foreach (DataGridViewColumn col in dataGridViewPhieuBaoHanh.Columns)
            {
                if (col.Name == "LoaiBH" || col.Name == "TrangThai" || col.Name == "KetQua")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }
            dataGridViewPhieuBaoHanh.CellValueChanged += dataGridViewPhieuBaoHanh_CellValueChanged;
            dataGridViewPhieuBaoHanh.CellBeginEdit += dataGridViewPhieuBaoHanh_CellBeginEdit;
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = _busHauMai.LayDanhSachBaoHanh();

                string keyword = txtMaSerialSanPham.Text.Trim();
                string loaiBH = comboBoxLoaiBaoHanh.SelectedItem?.ToString() ?? "Tất cả";
                string trangThai = comboBoxTrangThai.SelectedItem?.ToString() ?? "Tất cả";
                string lyDoLoi = txtLyDoLoi.Text.Trim();
                string ketQua = txtKetQua.Text.Trim();

                string filter = "";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    filter += $"(MaSerialSP LIKE '%{keyword}%')";
                }

                if (loaiBH != "Tất cả")
                {
                    if (filter.Length > 0) filter += " AND ";
                    filter += $"(LoaiBH LIKE '%{loaiBH}%')";
                }

                if (trangThai != "Tất cả")
                {
                    if (filter.Length > 0) filter += " AND ";
                    string mappedTrangThai = trangThai;
                    if (trangThai.Equals("Đang xử lý", StringComparison.OrdinalIgnoreCase)) mappedTrangThai = "Đang Xử Lý";
                    else if (trangThai.Equals("Hoàn thành", StringComparison.OrdinalIgnoreCase)) mappedTrangThai = "Hoàn Thành";
                    else if (trangThai.Equals("Từ chối", StringComparison.OrdinalIgnoreCase)) mappedTrangThai = "Từ Chối";
                    filter += $"(TrangThai LIKE '%{mappedTrangThai}%')";
                }

                if (!string.IsNullOrWhiteSpace(lyDoLoi))
                {
                    if (filter.Length > 0) filter += " AND ";
                    filter += $"(LyDoLoi LIKE '%{lyDoLoi.Replace("'", "''")}%')";
                }

                if (!string.IsNullOrWhiteSpace(ketQua))
                {
                    if (filter.Length > 0) filter += " AND ";
                    filter += $"(KetQua LIKE '%{ketQua.Replace("'", "''")}%')";
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = filter;
                    dt = dv.ToTable();
                }

                dataGridViewPhieuBaoHanh.DataSource = dt;

                // Định dạng hiển thị GridView
                if (dataGridViewPhieuBaoHanh.Columns["MaPBH"] != null)
                    dataGridViewPhieuBaoHanh.Columns["MaPBH"].HeaderText = "Mã bảo hành";
                if (dataGridViewPhieuBaoHanh.Columns["MaDH"] != null)
                    dataGridViewPhieuBaoHanh.Columns["MaDH"].HeaderText = "Mã đơn hàng";
                if (dataGridViewPhieuBaoHanh.Columns["MaKH"] != null)
                    dataGridViewPhieuBaoHanh.Columns["MaKH"].HeaderText = "Mã khách hàng";
                if (dataGridViewPhieuBaoHanh.Columns["MaSerialSP"] != null)
                    dataGridViewPhieuBaoHanh.Columns["MaSerialSP"].HeaderText = "Số Serial";
                if (dataGridViewPhieuBaoHanh.Columns["LoaiBH"] != null)
                    dataGridViewPhieuBaoHanh.Columns["LoaiBH"].HeaderText = "Loại bảo hành";
                if (dataGridViewPhieuBaoHanh.Columns["TrangThai"] != null)
                    dataGridViewPhieuBaoHanh.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewPhieuBaoHanh.Columns["NgayBatDau"] != null)
                    dataGridViewPhieuBaoHanh.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
                if (dataGridViewPhieuBaoHanh.Columns["NgayKetThuc"] != null)
                    dataGridViewPhieuBaoHanh.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
                if (dataGridViewPhieuBaoHanh.Columns["KetQua"] != null)
                    dataGridViewPhieuBaoHanh.Columns["KetQua"].HeaderText = "Kết quả";
                if (dataGridViewPhieuBaoHanh.Columns["LyDoLoi"] != null)
                    dataGridViewPhieuBaoHanh.Columns["LyDoLoi"].HeaderText = "Lý do lỗi";
                if (dataGridViewPhieuBaoHanh.Columns["NgayTao"] != null)
                    dataGridViewPhieuBaoHanh.Columns["NgayTao"].Visible = false;
                if (dataGridViewPhieuBaoHanh.Columns["NgayCapNhat"] != null)
                    dataGridViewPhieuBaoHanh.Columns["NgayCapNhat"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị phiếu bảo hành: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetCellValueSafe(DataGridViewRow row, string columnName)
        {
            if (row.DataGridView.Columns.Contains(columnName))
            {
                return row.Cells[columnName]?.Value?.ToString()?.Trim() ?? "";
            }
            return "";
        }

        private void dataGridViewPhieuBaoHanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewPhieuBaoHanh.Rows[e.RowIndex];

            _maPBHDangChon = GetCellValueSafe(row, "MaPBH");
            txtMaSerialSanPham.Text = GetCellValueSafe(row, "MaSerialSP");
            txtLyDoLoi.Text = GetCellValueSafe(row, "LyDoLoi");
            txtKetQua.Text = GetCellValueSafe(row, "KetQua");

            string loaiBH = GetCellValueSafe(row, "LoaiBH");
            comboBoxLoaiBaoHanh.Text = (loaiBH.Equals("Cửa Hàng", StringComparison.OrdinalIgnoreCase) || loaiBH.Equals("Cửa hàng", StringComparison.OrdinalIgnoreCase)) ? "Cửa hàng" : "Hãng";

            string trangThai = GetCellValueSafe(row, "TrangThai");
            _trangThaiBanDau = trangThai; // Save initial status

            comboBoxTrangThai.Text = trangThai.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase) ? "Đang xử lý" : 
                                     (trangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) ? "Hoàn thành" : "Từ chối");

            // Lock controls dynamically
            bool isLocked = trangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) || 
                            trangThai.Equals("Từ Chối", StringComparison.OrdinalIgnoreCase);

            txtLyDoLoi.ReadOnly = isLocked;
            txtKetQua.ReadOnly = isLocked;
            comboBoxLoaiBaoHanh.Enabled = !isLocked;
            comboBoxTrangThai.Enabled = !isLocked;
            btnSuaPhieuBaoHanh.Enabled = !isLocked;
        }

        private void dataGridViewPhieuBaoHanh_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewPhieuBaoHanh.Rows[e.RowIndex];
            string colName = dataGridViewPhieuBaoHanh.Columns[e.ColumnIndex].Name;
            string val = row.Cells[e.ColumnIndex].Value?.ToString()?.Trim() ?? "";

            if (colName == "LoaiBH")
            {
                comboBoxLoaiBaoHanh.Text = (val.Equals("Cửa Hàng", StringComparison.OrdinalIgnoreCase) || val.Equals("Cửa hàng", StringComparison.OrdinalIgnoreCase)) ? "Cửa hàng" : "Hãng";
            }
            else if (colName == "TrangThai")
            {
                comboBoxTrangThai.Text = val.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase) ? "Đang xử lý" :
                                         (val.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) ? "Hoàn thành" : "Từ chối");
            }
            else if (colName == "KetQua")
            {
                txtKetQua.Text = val;
            }
        }

        private void btnTaoPhieuBaoHanh_Click(object sender, EventArgs e)
        {
            try
            {
                string serial = txtMaSerialSanPham.Text.Trim();
                string lyDo = txtLyDoLoi.Text.Trim();

                if (string.IsNullOrWhiteSpace(serial))
                {
                    MessageBox.Show("Vui lòng nhập Số Serial sản phẩm lỗi.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(lyDo))
                {
                    MessageBox.Show("Vui lòng nhập Lý do lỗi sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra xem sản phẩm này có phiếu bảo hành nào đang xử lý hay không
                DataTable dtTatCaBH = _busHauMai.LayDanhSachBaoHanh();
                foreach (DataRow row in dtTatCaBH.Rows)
                {
                    if (row["MaSerialSP"].ToString()!.Trim().Equals(serial, StringComparison.OrdinalIgnoreCase))
                    {
                        string trangThai = row["TrangThai"]?.ToString()?.Trim() ?? "";
                        if (trangThai.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Sản phẩm với số Serial này đang có phiếu bảo hành chưa xử lý (Đang Xử Lý) trong hệ thống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                // Truy tìm đơn hàng chứa serial này
                var ctdh = _busDonHang.LayChiTietTheoSerial(serial);
                if (ctdh == null)
                {
                    MessageBox.Show("Sản phẩm này chưa được bán hoặc không tồn tại đơn bán hàng tương ứng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy đơn hàng để lấy MaKH và kiểm tra
                var dh = _busDonHang.LayTheoMa(ctdh.MaDH);
                if (dh == null)
                {
                    MessageBox.Show("Không tìm thấy đơn hàng tương ứng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dh.TrangThai == "Huỷ")
                {
                    MessageBox.Show("Đơn hàng tương ứng đã bị hủy. Không thể tạo bảo hành.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy thông tin khách hàng để xác định loại khách hàng (Lẻ/Sỉ)
                var kh = _busKhachHang.LayTheoMa(dh.MaKH);
                if (kh == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng đặt đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy thời gian bảo hành của loại sản phẩm
                var sp = _busSanPham.LayTheoSerial(serial);
                if (sp == null)
                {
                    MessageBox.Show("Sản phẩm không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var lsp = _busSanPham.LayLoaiSPTheoMa(sp.MaLoaiSP);
                if (lsp == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin loại sản phẩm bảo hành.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int thoiGianBaoHanhThang = lsp.ThoiGianBaoHanh;

                // Tạo DTO
                string maPBH = TaoMaPBHMoi();
                DTO_PhieuBaoHanh pbh = new DTO_PhieuBaoHanh
                {
                    MaPBH = maPBH,
                    MaDH = dh.MaDH,
                    MaKH = dh.MaKH,
                    MaSerialSP = serial,
                    NgayBatDau = DateTime.Today,
                    NgayKetThuc = DateTime.Today.AddMonths(thoiGianBaoHanhThang), // Mặc định tính theo thời hạn sản phẩm
                    TrangThai = "Đang Xử Lý",
                    LyDoLoi = lyDo,
                    KetQua = string.Empty
                };

                if (_busHauMai.TaoPhieuBaoHanh(pbh, kh.LoaiKH, thoiGianBaoHanhThang))
                {
                    MessageBox.Show($"Tạo phiếu bảo hành {maPBH} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Tạo phiếu bảo hành thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaPhieuBaoHanh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maPBHDangChon))
            {
                MessageBox.Show("Vui lòng chọn một phiếu bảo hành để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_trangThaiBanDau.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) || 
                _trangThaiBanDau.Equals("Từ Chối", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Phiếu bảo hành ở trạng thái Hoàn thành hoặc Từ chối không được phép chỉnh sửa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi hay không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string trangThaiMoi = "Đang Xử Lý";
                if (comboBoxTrangThai.Text == "Hoàn thành") trangThaiMoi = "Hoàn Thành";
                else if (comboBoxTrangThai.Text == "Từ chối") trangThaiMoi = "Từ Chối";

                string ketQua = txtKetQua.Text.Trim();
                string lyDoLoi = txtLyDoLoi.Text.Trim();
                string serial = txtMaSerialSanPham.Text.Trim();

                // Nếu chọn Hoàn Thành, hỏi người dùng thiết bị sửa xong đưa về 'Trong Kho' hay 'Lỗi' để trả NCC
                if (trangThaiMoi == "Hoàn Thành")
                {
                    var fixResult = MessageBox.Show("Sản phẩm đã sửa chữa xong và đưa trở lại kho hàng?\n(Chọn Yes: Chuyển về 'Trong Kho', Chọn No: Chuyển sang 'Lỗi')", 
                         "Xử lý sản phẩm hoàn thành bảo hành", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (fixResult == DialogResult.Yes)
                    {
                        _busSanPham.CapNhatTrangThaiSerial(serial, "Trong Kho");
                    }
                    else if (fixResult == DialogResult.No)
                    {
                        _busSanPham.CapNhatTrangThaiSerial(serial, "Lỗi");
                    }
                    else
                    {
                        return; // Hủy bỏ
                    }
                }
                else if (trangThaiMoi == "Từ Chối")
                {
                    // Từ chối bảo hành -> Trả lại máy cho khách hàng ở trạng thái 'Đã Bán' hoặc 'Lỗi' tùy thực tế
                    _busSanPham.CapNhatTrangThaiSerial(serial, "Đã Bán");
                }

                if (_busHauMai.CapNhatTrangThaiBaoHanh(_maPBHDangChon, trangThaiMoi) && 
                    _busHauMai.CapNhatKetQuaBaoHanh(_maPBHDangChon, ketQua) &&
                    _busHauMai.CapNhatLyDoLoiBaoHanh(_maPBHDangChon, lyDoLoi))
                {
                    MessageBox.Show("Cập nhật phiếu bảo hành thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật phiếu bảo hành thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimPhieuBaoHanh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LamMoiForm()
        {
            _maPBHDangChon = null;
            _trangThaiBanDau = "";
            txtMaSerialSanPham.Clear();
            txtLyDoLoi.Clear();
            txtKetQua.Clear();
            comboBoxLoaiBaoHanh.SelectedIndex = 0;
            comboBoxTrangThai.SelectedIndex = 0;

            // Reset controls read-only/enabled state
            txtLyDoLoi.ReadOnly = false;
            txtKetQua.ReadOnly = false;
            comboBoxLoaiBaoHanh.Enabled = true;
            comboBoxTrangThai.Enabled = true;
            btnSuaPhieuBaoHanh.Enabled = true;
        }

        private void dataGridViewPhieuBaoHanh_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewPhieuBaoHanh.Rows[e.RowIndex];
            string trangThai = GetCellValueSafe(row, "TrangThai");
            if (trangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) || 
                trangThai.Equals("Từ Chối", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Phiếu bảo hành ở trạng thái Hoàn thành hoặc Từ chối không được phép chỉnh sửa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private string TaoMaPBHMoi()
        {
            try
            {
                DataTable dt = _busHauMai.LayDanhSachBaoHanh();
                int soLon = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string ma = row["MaPBH"]?.ToString()?.Trim() ?? "";
                    if (ma.StartsWith("PBH") && ma.Length == 10)
                    {
                        if (int.TryParse(ma.Substring(3), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "PBH" + (soLon + 1).ToString().PadLeft(7, '0');
            }
            catch
            {
                return "PBH0000001";
            }
        }
    }
}

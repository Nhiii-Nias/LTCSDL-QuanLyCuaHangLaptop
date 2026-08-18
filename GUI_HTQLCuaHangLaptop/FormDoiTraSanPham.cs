using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormDoiTraSanPham : Form
    {
        private readonly BUS_HauMai _busHauMai = new BUS_HauMai();
        private readonly BUS_DonHang _busDonHang = new BUS_DonHang();
        private readonly BUS_SanPham _busSanPham = new BUS_SanPham();
        private readonly BUS_KhachHang _busKhachHang = new BUS_KhachHang();

        private string? _maPDTDangChon = null;

        public FormDoiTraSanPham()
        {
            InitializeComponent();

            this.Load += FormDoiTraSanPham_Load;
            dataGridViewPhieuDoiTra.CellClick += dataGridViewPhieuDoiTra_CellClick;

            btnTaoPhieuDoiTra.Click += btnTaoPhieuDoiTra_Click;
            btnSuaPhieuDoiTra.Click += btnSuaPhieuDoiTra_Click;
            btnTimPhieuDoiTra.Click += btnTimPhieuDoiTra_Click;

            // Nạp dữ liệu mặc định cho Combobox Hình thức xử lý
            comboBoxHinhThucXuLy.Items.Clear();
            comboBoxHinhThucXuLy.Items.AddRange(new object[] { "Tất cả", "Đổi Máy", "Hoàn Tiền", "Từ Chối" });
            comboBoxHinhThucXuLy.SelectedIndex = 0;

            // Nạp dữ liệu mặc định cho Combobox Trạng thái
            comboBoxTrangThai.Items.Clear();
            comboBoxTrangThai.Items.AddRange(new object[] { "Tất cả", "Đang xử lý", "Hoàn thành", "Từ chối" });
            comboBoxTrangThai.SelectedIndex = 0;

            txtMaDonHang.ReadOnly = false;
        }

        private void FormDoiTraSanPham_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridViewPhieuDoiTra.ReadOnly = false;
            foreach (DataGridViewColumn col in dataGridViewPhieuDoiTra.Columns)
            {
                if (col.Name == "LoaiXuLy" || col.Name == "TrangThai" || col.Name == "LyDo")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }
            dataGridViewPhieuDoiTra.CellValueChanged += dataGridViewPhieuDoiTra_CellValueChanged;
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = _busHauMai.LayDanhSachDoiTra();

                string keyword = txtMaSerialSanPham.Text.Trim();
                string maDH = txtMaDonHang.Text.Trim();
                string loaiXuLy = comboBoxHinhThucXuLy.SelectedItem?.ToString() ?? "Tất cả";
                string trangThai = comboBoxTrangThai.SelectedItem?.ToString() ?? "Tất cả";

                string filter = "";

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    filter += $"(MaPhieuDT LIKE '%{keyword}%' OR MaSerialSP LIKE '%{keyword}%')";
                }

                if (!string.IsNullOrWhiteSpace(maDH))
                {
                    if (filter.Length > 0) filter += " AND ";
                    filter += $"(MaDH LIKE '%{maDH}%')";
                }

                if (loaiXuLy != "Tất cả")
                {
                    if (filter.Length > 0) filter += " AND ";
                    filter += $"(LoaiXuLy LIKE '%{loaiXuLy}%')";
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

                if (!string.IsNullOrEmpty(filter))
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = filter;
                    dt = dv.ToTable();
                }

                dataGridViewPhieuDoiTra.DataSource = dt;

                // Định dạng hiển thị GridView
                if (dataGridViewPhieuDoiTra.Columns["MaPhieuDT"] != null)
                    dataGridViewPhieuDoiTra.Columns["MaPhieuDT"].HeaderText = "Mã phiếu đổi trả";
                if (dataGridViewPhieuDoiTra.Columns["MaDH"] != null)
                    dataGridViewPhieuDoiTra.Columns["MaDH"].HeaderText = "Mã đơn hàng";
                if (dataGridViewPhieuDoiTra.Columns["MaSerialSP"] != null)
                    dataGridViewPhieuDoiTra.Columns["MaSerialSP"].HeaderText = "Số Serial";
                if (dataGridViewPhieuDoiTra.Columns["MaKH"] != null)
                    dataGridViewPhieuDoiTra.Columns["MaKH"].HeaderText = "Mã khách hàng";
                if (dataGridViewPhieuDoiTra.Columns["NgayYeuCau"] != null)
                    dataGridViewPhieuDoiTra.Columns["NgayYeuCau"].HeaderText = "Ngày yêu cầu";
                if (dataGridViewPhieuDoiTra.Columns["LyDo"] != null)
                    dataGridViewPhieuDoiTra.Columns["LyDo"].HeaderText = "Lý do";
                if (dataGridViewPhieuDoiTra.Columns["LoaiXuLy"] != null)
                    dataGridViewPhieuDoiTra.Columns["LoaiXuLy"].HeaderText = "Hình thức xử lý";
                if (dataGridViewPhieuDoiTra.Columns["TrangThai"] != null)
                    dataGridViewPhieuDoiTra.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewPhieuDoiTra.Columns["NgayTao"] != null)
                    dataGridViewPhieuDoiTra.Columns["NgayTao"].Visible = false;
                if (dataGridViewPhieuDoiTra.Columns["NgayCapNhat"] != null)
                    dataGridViewPhieuDoiTra.Columns["NgayCapNhat"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách phiếu đổi trả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridViewPhieuDoiTra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewPhieuDoiTra.Rows[e.RowIndex];

            _maPDTDangChon = GetCellValueSafe(row, "MaPhieuDT");
            txtMaSerialSanPham.Text = GetCellValueSafe(row, "MaSerialSP");
            txtMaDonHang.Text = GetCellValueSafe(row, "MaDH");
            txtLyDoDoiTra.Text = GetCellValueSafe(row, "LyDo");

            comboBoxHinhThucXuLy.Text = GetCellValueSafe(row, "LoaiXuLy");

            string trangThai = GetCellValueSafe(row, "TrangThai");
            comboBoxTrangThai.Text = trangThai.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase) ? "Đang xử lý" :
                                     (trangThai.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) ? "Hoàn thành" : "Từ chối");
        }

        private void dataGridViewPhieuDoiTra_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewPhieuDoiTra.Rows[e.RowIndex];
            string colName = dataGridViewPhieuDoiTra.Columns[e.ColumnIndex].Name;
            string val = row.Cells[e.ColumnIndex].Value?.ToString()?.Trim() ?? "";

            if (colName == "LoaiXuLy")
            {
                comboBoxHinhThucXuLy.Text = val;
            }
            else if (colName == "TrangThai")
            {
                comboBoxTrangThai.Text = val.Equals("Đang Xử Lý", StringComparison.OrdinalIgnoreCase) ? "Đang xử lý" :
                                         (val.Equals("Hoàn Thành", StringComparison.OrdinalIgnoreCase) ? "Hoàn thành" : "Từ chối");
            }
            else if (colName == "LyDo")
            {
                txtLyDoDoiTra.Text = val;
            }
        }

        private void btnTaoPhieuDoiTra_Click(object sender, EventArgs e)
        {
            try
            {
                string serial = txtMaSerialSanPham.Text.Trim();
                string lyDo = txtLyDoDoiTra.Text.Trim();

                if (string.IsNullOrWhiteSpace(serial))
                {
                    MessageBox.Show("Vui lòng nhập Số Serial sản phẩm cần đổi trả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(lyDo))
                {
                    MessageBox.Show("Vui lòng nhập Lý do đổi trả (chỉ chấp nhận lỗi do nhà sản xuất).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tra cứu đơn hàng chứa serial này
                var ctdh = _busDonHang.LayChiTietTheoSerial(serial);
                if (ctdh == null)
                {
                    MessageBox.Show("Sản phẩm chưa được bán hoặc không tìm thấy thông tin hóa đơn tương ứng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtMaDonHang.Text = ctdh.MaDH.Trim();

                var dh = _busDonHang.LayTheoMa(ctdh.MaDH);
                if (dh == null)
                {
                    MessageBox.Show("Không tìm thấy đơn hàng tương ứng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dh.TrangThai == "Huỷ")
                {
                    MessageBox.Show("Đơn hàng này đã bị hủy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra điều kiện đổi trả
                var (hopLe, lyDoTuChoi) = _busHauMai.KiemTraDieuKienDoiTra(serial, dh.NgayDat);
                if (!hopLe)
                {
                    MessageBox.Show("Không đủ điều kiện đổi trả: " + lyDoTuChoi, "Từ chối đổi trả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maPDT = TaoMaPDTMoi();
                DTO_PhieuDoiTra pdt = new DTO_PhieuDoiTra
                {
                    MaPhieuDT = maPDT,
                    MaDH = dh.MaDH,
                    MaSerialSP = serial,
                    MaKH = dh.MaKH,
                    LyDo = lyDo,
                    LoaiXuLy = comboBoxHinhThucXuLy.Text == "Đổi Máy khác" ? "Đổi Máy" : comboBoxHinhThucXuLy.Text,
                    TrangThai = "Đang Xử Lý",
                    NgayYeuCau = DateTime.Today
                };

                if (_busHauMai.TaoPhieuDoiTra(pdt))
                {
                    MessageBox.Show($"Tạo phiếu đổi trả {maPDT} thành công!\nTrạng thái sản phẩm đổi sang 'Đổi Trả'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Tạo phiếu đổi trả thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaPhieuDoiTra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maPDTDangChon))
            {
                MessageBox.Show("Vui lòng chọn một phiếu đổi trả để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                string loaiXuLyMoi = comboBoxHinhThucXuLy.Text;
                if (loaiXuLyMoi == "Đổi Máy khác") loaiXuLyMoi = "Đổi Máy";

                string serial = txtMaSerialSanPham.Text.Trim();

                // Đồng bộ cập nhật trạng thái của serial dựa trên kết quả đổi trả
                if (trangThaiMoi == "Hoàn Thành")
                {
                    // Hoàn thành đổi trả: thu hồi máy cũ bị lỗi -> chuyển trạng thái serial thành 'Lỗi' để quản lý trả NCC
                    _busSanPham.CapNhatTrangThaiSerial(serial, "Lỗi");
                }
                else if (trangThaiMoi == "Từ Chối")
                {
                    // Từ chối đổi trả: khôi phục trạng thái serial về lại 'Đã Bán'
                    _busSanPham.CapNhatTrangThaiSerial(serial, "Đã Bán");
                }

                if (_busHauMai.CapNhatTrangThaiDoiTra(_maPDTDangChon, trangThaiMoi) && 
                    _busHauMai.CapNhatLoaiXuLyDoiTra(_maPDTDangChon, loaiXuLyMoi))
                {
                    MessageBox.Show("Cập nhật phiếu đổi trả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật phiếu đổi trả thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimPhieuDoiTra_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LamMoiForm()
        {
            _maPDTDangChon = null;
            txtMaSerialSanPham.Clear();
            txtMaDonHang.Clear();
            txtLyDoDoiTra.Clear();
            comboBoxHinhThucXuLy.SelectedIndex = 0;
            comboBoxTrangThai.SelectedIndex = 0;
        }

        private string TaoMaPDTMoi()
        {
            try
            {
                DataTable dt = _busHauMai.LayDanhSachDoiTra();
                int soLon = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string ma = row["MaPhieuDT"]?.ToString()?.Trim() ?? "";
                    if (ma.StartsWith("PDT") && ma.Length == 10)
                    {
                        if (int.TryParse(ma.Substring(3), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "PDT" + (soLon + 1).ToString().PadLeft(7, '0');
            }
            catch
            {
                return "PDT0000001";
            }
        }
    }
}

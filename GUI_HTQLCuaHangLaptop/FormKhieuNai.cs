using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormKhieuNai : Form
    {
        private readonly BUS_HauMai _busHauMai = new BUS_HauMai();
        private string? _maDonKNDangChon = null;
        private string _noiDungBanDau = "";

        public FormKhieuNai()
        {
            InitializeComponent();

            this.Load += FormKhieuNai_Load;
            dataGridViewDSKhieuNai.CellClick += dataGridViewDSKhieuNai_CellClick;

            btnSuaPhieuBaoHanh.Click += btnSuaPhieuBaoHanh_Click; // Sửa/Giải quyết
            btnTimPhieuBaoHanh.Click += btnTimPhieuBaoHanh_Click; // Tìm

            // Thiết lập giá trị mặc định cho Combobox trạng thái giải quyết
            comboBoxTrangThai.Items.Clear();
            comboBoxTrangThai.Items.AddRange(new object[] { "Đã giải quyết", "Đang xử lý", "Từ chối" });
            comboBoxTrangThai.SelectedIndex = -1;

            // Mở khóa cho phép nhân viên CSKH nhập liệu
            txtNoiDungPhanAnh.ReadOnly = false;
            comboBoxDonHangLienQuan.Enabled = true;
        }

        private void NapComboboxDonHangLienQuan()
        {
            try
            {
                DataTable dt = _busHauMai.LayDanhSachKhieuNai();
                string currentText = comboBoxDonHangLienQuan.Text;
                comboBoxDonHangLienQuan.Items.Clear();
                comboBoxDonHangLienQuan.Items.Add("");
                System.Collections.Generic.HashSet<string> dsMaDH = new System.Collections.Generic.HashSet<string>();
                foreach (DataRow r in dt.Rows)
                {
                    string maDH = r["MaDH"]?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(maDH) && !dsMaDH.Contains(maDH))
                    {
                        dsMaDH.Add(maDH);
                        comboBoxDonHangLienQuan.Items.Add(maDH);
                    }
                }
                comboBoxDonHangLienQuan.Text = currentText;
            }
            catch { }
        }

        private void FormKhieuNai_Load(object sender, EventArgs e)
        {
            LoadData();
            NapComboboxDonHangLienQuan();

            // Set grid to be editable except MaDonKN, MaDH, MaKH, NoiDung
            dataGridViewDSKhieuNai.ReadOnly = false;
            foreach (DataGridViewColumn col in dataGridViewDSKhieuNai.Columns)
            {
                if (col.Name == "TrangThai" || col.Name == "KetQua")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }
            dataGridViewDSKhieuNai.CellValueChanged += dataGridViewDSKhieuNai_CellValueChanged;
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = _busHauMai.LayDanhSachKhieuNai();

                string noiDung = txtNoiDungPhanAnh.Text.Trim();
                string maDH = comboBoxDonHangLienQuan.Text.Trim();
                string ketQua = txtKetQua.Text.Trim();
                string trangThai = comboBoxTrangThai.SelectedItem?.ToString() ?? "Tất cả";

                string filter = "";

                if (!string.IsNullOrWhiteSpace(noiDung))
                {
                    filter += $"(NoiDung LIKE '%{noiDung}%')";
                }

                if (!string.IsNullOrWhiteSpace(maDH))
                {
                    if (filter.Length > 0) filter += " AND ";
                    filter += $"(MaDH LIKE '%{maDH}%')";
                }

                if (!string.IsNullOrWhiteSpace(ketQua))
                {
                    if (filter.Length > 0) filter += " AND ";
                    filter += $"(KetQua LIKE '%{ketQua}%')";
                }

                if (trangThai != "Tất cả")
                {
                    if (filter.Length > 0) filter += " AND ";
                    string mappedTrangThai = trangThai;
                    if (trangThai.Equals("Đang xử lý", StringComparison.OrdinalIgnoreCase)) mappedTrangThai = "Đang Xử Lý";
                    else if (trangThai.Equals("Đã giải quyết", StringComparison.OrdinalIgnoreCase)) mappedTrangThai = "Đã Giải Quyết";
                    else if (trangThai.Equals("Từ chối", StringComparison.OrdinalIgnoreCase)) mappedTrangThai = "Từ Chối";
                    filter += $"(TrangThai LIKE '%{mappedTrangThai}%')";
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = filter;
                    dt = dv.ToTable();
                }

                dataGridViewDSKhieuNai.DataSource = dt;

                // Định dạng hiển thị GridView
                if (dataGridViewDSKhieuNai.Columns["MaDonKN"] != null)
                    dataGridViewDSKhieuNai.Columns["MaDonKN"].HeaderText = "Mã đơn khiếu nại";
                if (dataGridViewDSKhieuNai.Columns["MaDH"] != null)
                    dataGridViewDSKhieuNai.Columns["MaDH"].HeaderText = "Mã đơn hàng";
                if (dataGridViewDSKhieuNai.Columns["MaKH"] != null)
                    dataGridViewDSKhieuNai.Columns["MaKH"].HeaderText = "Mã khách hàng";
                if (dataGridViewDSKhieuNai.Columns["NoiDung"] != null)
                    dataGridViewDSKhieuNai.Columns["NoiDung"].HeaderText = "Nội dung phản ánh";
                if (dataGridViewDSKhieuNai.Columns["TrangThai"] != null)
                    dataGridViewDSKhieuNai.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewDSKhieuNai.Columns["KetQua"] != null)
                    dataGridViewDSKhieuNai.Columns["KetQua"].HeaderText = "Kết quả giải quyết";
                if (dataGridViewDSKhieuNai.Columns["NgayTao"] != null)
                    dataGridViewDSKhieuNai.Columns["NgayTao"].Visible = false;
                if (dataGridViewDSKhieuNai.Columns["NgayCapNhat"] != null)
                    dataGridViewDSKhieuNai.Columns["NgayCapNhat"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách đơn khiếu nại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSKhieuNai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSKhieuNai.Rows[e.RowIndex];

            _maDonKNDangChon = row.Cells["MaDonKN"]?.Value?.ToString()?.Trim();
            txtNoiDungPhanAnh.Text = row.Cells["NoiDung"]?.Value?.ToString()?.Trim();
            _noiDungBanDau = txtNoiDungPhanAnh.Text;
            
            // Nạp mã đơn hàng lên combobox (để hiển thị)
            comboBoxDonHangLienQuan.Text = row.Cells["MaDH"]?.Value?.ToString()?.Trim();
            
            txtKetQua.Text = row.Cells["KetQua"]?.Value?.ToString()?.Trim();

            string trangThai = row.Cells["TrangThai"]?.Value?.ToString()?.Trim() ?? "";
            comboBoxTrangThai.Text = (trangThai == "Đang Xử Lý") ? "Đang xử lý" :
                                     (trangThai == "Đã Giải Quyết" ? "Đã giải quyết" : "Từ chối");
        }

        private void btnSuaPhieuBaoHanh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maDonKNDangChon))
            {
                MessageBox.Show("Vui lòng chọn một đơn khiếu nại trong danh sách để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtNoiDungPhanAnh.Text.Trim() != _noiDungBanDau.Trim())
            {
                MessageBox.Show("Không được phép chỉnh sửa nội dung khiếu nại của khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi hay không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string trangThaiMoi = "Đang Xử Lý";
                if (comboBoxTrangThai.Text == "Đã giải quyết") trangThaiMoi = "Đã Giải Quyết";
                else if (comboBoxTrangThai.Text == "Từ chối") trangThaiMoi = "Từ Chối";

                string ketQua = txtKetQua.Text.Trim();

                if (_busHauMai.CapNhatTrangThaiKhieuNai(_maDonKNDangChon, trangThaiMoi) && 
                    _busHauMai.CapNhatKetQuaKhieuNai(_maDonKNDangChon, ketQua))
                {
                    MessageBox.Show("Cập nhật kết quả giải quyết khiếu nại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    NapComboboxDonHangLienQuan();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật khiếu nại thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            _maDonKNDangChon = null;
            _noiDungBanDau = "";
            txtNoiDungPhanAnh.Clear();
            comboBoxDonHangLienQuan.Text = "";
            txtKetQua.Clear();
            comboBoxTrangThai.SelectedIndex = -1;
        }

        private void dataGridViewDSKhieuNai_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSKhieuNai.Rows[e.RowIndex];
            string colName = dataGridViewDSKhieuNai.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (colName == "TrangThai")
            {
                string trangThai = val?.ToString()?.Trim() ?? "";
                comboBoxTrangThai.Text = (trangThai == "Đang Xử Lý") ? "Đang xử lý" :
                                         (trangThai == "Đã Giải Quyết" ? "Đã giải quyết" : "Từ chối");
            }
            else if (colName == "KetQua")
            {
                txtKetQua.Text = val?.ToString()?.Trim();
            }
        }
    }
}

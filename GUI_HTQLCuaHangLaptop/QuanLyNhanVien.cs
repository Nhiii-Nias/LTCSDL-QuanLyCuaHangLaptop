using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class QuanLyNhanVien : Form
    {
        private readonly BUS_NhanVien _busNV = new BUS_NhanVien();

        // Tài khoản đang đăng nhập (được truyền từ FormMain)
        private DTO_TaiKhoanNV _taiKhoanHienTai;

        // Mã nhân viên đang được chọn trong DataGridView
        private string? _maNVDangChon = null;

        // Chế độ: true = đang sửa, false = đang thêm mới
        private bool _dangSua = false;

        public QuanLyNhanVien(DTO_TaiKhoanNV taiKhoan)
        {
            InitializeComponent();
            _taiKhoanHienTai = taiKhoan;

            this.Load += QuanLyNhanVien_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnTimKiem.Click += btnTimKiem_Click;
            dataGridViewDSNhanVien.CellClick += dataGridViewDSNhanVien_CellClick;

            // txtMaNhanVien chỉ đọc (tự sinh)
            txtMaNhanVien.ReadOnly = true;
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN LOAD
        // ══════════════════════════════════════════════════════════════════
        private void QuanLyNhanVien_Load(object sender, EventArgs e)
        {
            dateTimePickerNgaySinh.MaxDate = DateTime.Today.AddYears(-18);
            dateTimePickerNgayBatDau.ShowCheckBox = true;
            dateTimePickerNgayBatDau.Checked = false;
            dateTimePickerNgaySinh.Value = DateTime.Today.AddYears(-25); // Mặc định 25 tuổi
            LoadData();
            LamMoiForm();
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD DỮ LIỆU VÀO DATAGRIDVIEW
        // ══════════════════════════════════════════════════════════════════
        private void LoadData(bool isSearch = false)
        {
            try
            {
                DataTable dt = _busNV.LayDanhSach();

                if (isSearch)
                {
                    string tenNV = txtHoTenNhanVien.Text.Trim();
                    string gioiTinh = comboBoxLoaiKhachHang.Text.Trim();
                    string sdt = txtSDT.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    string diaChi = txtDiaChi.Text.Trim();
                    string chucVu = txtChucVu.Text.Trim();
                    string luongStr = txtLuong.Text.Trim();

                    var query = dt.AsEnumerable();

                    if (!string.IsNullOrEmpty(tenNV))
                    {
                        query = query.Where(r => r.Field<string>("TenNV")?.IndexOf(tenNV, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    if (!string.IsNullOrEmpty(gioiTinh))
                    {
                        query = query.Where(r => string.Equals(r.Field<string>("GioiTinh"), gioiTinh, StringComparison.OrdinalIgnoreCase));
                    }

                    if (dateTimePickerNgayBatDau.Checked)
                    {
                        DateTime selectedDate = dateTimePickerNgayBatDau.Value.Date;
                        query = query.Where(r => r.Field<DateTime>("NgayVaoLam").Date == selectedDate);
                    }

                    if (!string.IsNullOrEmpty(sdt))
                    {
                        query = query.Where(r => r.Field<string>("SDT")?.Contains(sdt) == true);
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        query = query.Where(r => r.Field<string>("Email")?.IndexOf(email, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    if (!string.IsNullOrEmpty(diaChi))
                    {
                        query = query.Where(r => r.Field<string>("DiaChi")?.IndexOf(diaChi, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    if (!string.IsNullOrEmpty(chucVu))
                    {
                        query = query.Where(r => r.Field<string>("ChucVu")?.IndexOf(chucVu, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    if (!string.IsNullOrEmpty(luongStr) && decimal.TryParse(luongStr, out decimal luong))
                    {
                        query = query.Where(r => r.Field<decimal>("Luong") == luong);
                    }

                    if (query.Any())
                    {
                        dt = query.CopyToDataTable();
                    }
                    else
                    {
                        dt = dt.Clone();
                    }
                }

                dataGridViewDSNhanVien.DataSource = dt;

                // Đặt tên cột thân thiện
                if (dataGridViewDSNhanVien.Columns["MaNV"] != null)
                    dataGridViewDSNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                if (dataGridViewDSNhanVien.Columns["TenNV"] != null)
                    dataGridViewDSNhanVien.Columns["TenNV"].HeaderText = "Họ tên";
                if (dataGridViewDSNhanVien.Columns["GioiTinh"] != null)
                    dataGridViewDSNhanVien.Columns["GioiTinh"].HeaderText = "Giới tính";
                if (dataGridViewDSNhanVien.Columns["SinhNhat"] != null)
                    dataGridViewDSNhanVien.Columns["SinhNhat"].HeaderText = "Ngày sinh";
                if (dataGridViewDSNhanVien.Columns["SDT"] != null)
                    dataGridViewDSNhanVien.Columns["SDT"].HeaderText = "SĐT";
                if (dataGridViewDSNhanVien.Columns["DiaChi"] != null)
                    dataGridViewDSNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if (dataGridViewDSNhanVien.Columns["Email"] != null)
                    dataGridViewDSNhanVien.Columns["Email"].HeaderText = "Email";
                if (dataGridViewDSNhanVien.Columns["NgayVaoLam"] != null)
                    dataGridViewDSNhanVien.Columns["NgayVaoLam"].HeaderText = "Ngày vào làm";
                if (dataGridViewDSNhanVien.Columns["Luong"] != null)
                    dataGridViewDSNhanVien.Columns["Luong"].HeaderText = "Lương";
                if (dataGridViewDSNhanVien.Columns["ChucVu"] != null)
                    dataGridViewDSNhanVien.Columns["ChucVu"].HeaderText = "Chức vụ";
                if (dataGridViewDSNhanVien.Columns["NgayTao"] != null)
                    dataGridViewDSNhanVien.Columns["NgayTao"].HeaderText = "Ngày tạo";
                if (dataGridViewDSNhanVien.Columns["NgayCapNhat"] != null)
                    dataGridViewDSNhanVien.Columns["NgayCapNhat"].HeaderText = "Ngày cập nhật";
                if (dataGridViewDSNhanVien.Columns["IsDeleted"] != null)
                    dataGridViewDSNhanVien.Columns["IsDeleted"].Visible = false;
                if (dataGridViewDSNhanVien.Columns["NguoiTao"] != null)
                    dataGridViewDSNhanVien.Columns["NguoiTao"].Visible = false;
                if (dataGridViewDSNhanVien.Columns["NguoiCapNhat"] != null)
                    dataGridViewDSNhanVien.Columns["NguoiCapNhat"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CLICK VÀO DÒNG TRONG DATAGRIDVIEW — ĐIỀN VÀO FORM
        // ══════════════════════════════════════════════════════════════════
        private void dataGridViewDSNhanVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSNhanVien.Rows[e.RowIndex];

            _maNVDangChon = row.Cells["MaNV"]?.Value?.ToString()?.Trim();
            txtMaNhanVien.Text = _maNVDangChon;
            txtHoTenNhanVien.Text = row.Cells["TenNV"]?.Value?.ToString();
            comboBoxLoaiKhachHang.Text = row.Cells["GioiTinh"]?.Value?.ToString();

            if (row.Cells["NgayVaoLam"]?.Value != null && row.Cells["NgayVaoLam"].Value != DBNull.Value)
            {
                dateTimePickerNgayBatDau.Value = Convert.ToDateTime(row.Cells["NgayVaoLam"].Value);
                dateTimePickerNgayBatDau.Checked = true;
            }
            else
            {
                dateTimePickerNgayBatDau.Value = DateTime.Today;
                dateTimePickerNgayBatDau.Checked = false;
            }

            if (row.Cells["SinhNhat"]?.Value != null && row.Cells["SinhNhat"].Value != DBNull.Value)
            {
                dateTimePickerNgaySinh.Value = Convert.ToDateTime(row.Cells["SinhNhat"].Value);
            }
            else
            {
                dateTimePickerNgaySinh.Value = DateTime.Today.AddYears(-18);
            }

            txtSDT.Text = row.Cells["SDT"]?.Value?.ToString();
            txtEmail.Text = row.Cells["Email"]?.Value?.ToString();
            txtDiaChi.Text = row.Cells["DiaChi"]?.Value?.ToString();
            txtChucVu.Text = row.Cells["ChucVu"]?.Value?.ToString();
            txtLuong.Text = row.Cells["Luong"]?.Value?.ToString();
        }

        // ══════════════════════════════════════════════════════════════════
        // NÚT THÊM
        // ══════════════════════════════════════════════════════════════════
        private void btnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                var nv = DocDuLieuForm();
                // Sinh mã NV tự động
                nv.MaNV = TaoMaNVMoi();

                bool ketQua = _busNV.Them(nv, _taiKhoanHienTai.MaTK);
                if (ketQua)
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (OperationCanceledException)
            {
                // Người dùng đã hủy nhập ngày sinh — không làm gì
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

        // ══════════════════════════════════════════════════════════════════
        // NÚT SỬA
        // ══════════════════════════════════════════════════════════════════
        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_maNVDangChon))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn lưu thay đổi thông tin nhân viên này không?",
                "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                var nv = DocDuLieuForm();
                nv.MaNV = _maNVDangChon;

                bool ketQua = _busNV.Sua(nv, _taiKhoanHienTai.MaTK);
                if (ketQua)
                {
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (OperationCanceledException)
            {
                // Người dùng đã hủy nhập ngày sinh — không làm gì
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // NÚT XÓA (XÓA MỀM)
        // ══════════════════════════════════════════════════════════════════
        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_maNVDangChon))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên trong danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên '{_maNVDangChon}' không?\n(Dữ liệu sẽ bị ẩn, không xóa vật lý)",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ketQua = _busNV.Xoa(_maNVDangChon);
                if (ketQua)
                {
                    MessageBox.Show("Đã xóa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // NÚT TÌM KIẾM
        // ══════════════════════════════════════════════════════════════════
        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            LoadData(true);
        }

        // ══════════════════════════════════════════════════════════════════
        // HÀM HỖ TRỢ: ĐỌC DỮ LIỆU TỪ FORM
        // ══════════════════════════════════════════════════════════════════
        private DTO_NhanVien DocDuLieuForm()
        {
            string tenNV = txtHoTenNhanVien.Text.Trim();
            string gioiTinh = comboBoxLoaiKhachHang.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string chucVu = txtChucVu.Text.Trim();
            string luongStr = txtLuong.Text.Trim();

            DateTime ngayVaoLam = dateTimePickerNgayBatDau.Value;

            if (!decimal.TryParse(luongStr, out decimal luong))
                throw new ArgumentException("Lương phải là số (ví dụ: 5000000).");

            DateTime sinhNhat = dateTimePickerNgaySinh.Value;

            return new DTO_NhanVien
            {
                TenNV = tenNV,
                GioiTinh = gioiTinh,
                SinhNhat = sinhNhat,
                SDT = sdt,
                DiaChi = diaChi,
                Email = email,
                NgayVaoLam = ngayVaoLam,
                Luong = luong,
                ChucVu = chucVu,
            };
        }

        // ══════════════════════════════════════════════════════════════════
        // HÀM HỖ TRỢ: LÀM MỚI FORM (XÓA INPUT)
        // ══════════════════════════════════════════════════════════════════
        private void LamMoiForm()
        {
            _maNVDangChon = null;
            txtMaNhanVien.Text = "(Tự sinh)";
            txtHoTenNhanVien.Clear();
            comboBoxLoaiKhachHang.SelectedIndex = -1;
            dateTimePickerNgayBatDau.Value = DateTime.Today;
            dateTimePickerNgayBatDau.Checked = false;
            dateTimePickerNgaySinh.Value = DateTime.Today.AddYears(-18);
            txtSDT.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtChucVu.Clear();
            txtLuong.Clear();
        }

        // ══════════════════════════════════════════════════════════════════
        // SINH MÃ NHÂN VIÊN TỰ ĐỘNG: NV000001, NV000002, ...
        // ══════════════════════════════════════════════════════════════════
        private string TaoMaNVMoi()
        {
            try
            {
                DataTable dt = _busNV.LayDanhSach();
                int soLon = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string maNV = row["MaNV"]?.ToString()?.Trim() ?? "";
                    if (maNV.StartsWith("NV") && maNV.Length >= 3)
                    {
                        if (int.TryParse(maNV.Substring(2), out int so))
                            if (so > soLon) soLon = so;
                    }
                }
                return "NV" + (soLon + 1).ToString().PadLeft(8, '0');
            }
            catch
            {
                return "NV00000001";
            }
        }

        private void labelSDT_Click(object sender, EventArgs e)
        {
            // Không dùng
        }
    }
}

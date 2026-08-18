using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class TaiKhoanNhanVien : Form
    {
        private readonly BUS_TaiKhoan _busTK = new BUS_TaiKhoan();

        // Tài khoản đang đăng nhập (truyền từ FormMain/FormQuanLyHeThong)
        private DTO_TaiKhoanNV _taiKhoanHienTai;

        // Mã tài khoản đang được chọn trong DataGridView
        private string? _maTKDangChon = null;
        private DTO_TaiKhoanNV? _tkDangChon = null;

        public TaiKhoanNhanVien(DTO_TaiKhoanNV taiKhoan)
        {
            InitializeComponent();
            _taiKhoanHienTai = taiKhoan;

            this.Load += TaiKhoanNhanVien_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnTimKiem.Click += btnTimKiem_Click;
            dataGridViewDSTaiKhoanNhanVien.CellClick += dataGridViewDSTaiKhoanNhanVien_CellClick;

            // txtTenDangNhap không sửa được khi đang sửa (chỉ đọc)
            txtTenDangNhap.ReadOnly = false;
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD FORM
        // ══════════════════════════════════════════════════════════════════
        private void TaiKhoanNhanVien_Load(object sender, EventArgs e)
        {
            NapComboBoxVaiTro();
            NapComboBoxNVChuaCoTaiKhoan();
            LoadData();
            LamMoiForm();
        }

        // ══════════════════════════════════════════════════════════════════
        // NẠP COMBOBOX VAI TRÒ
        // ══════════════════════════════════════════════════════════════════
        private void NapComboBoxVaiTro()
        {
            try
            {
                DataTable dtVaiTro = _busTK.LayDanhSachVaiTro();
                comboBoxVaiTro.DataSource = dtVaiTro;
                comboBoxVaiTro.DisplayMember = "TenVaiTro";
                comboBoxVaiTro.ValueMember = "MaVaiTro";
                comboBoxVaiTro.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // NẠP COMBOBOX NHÂN VIÊN CHƯA CÓ TÀI KHOẢN
        // ══════════════════════════════════════════════════════════════════
        private void NapComboBoxNVChuaCoTaiKhoan()
        {
            try
            {
                DataTable dtNV = _busTK.LayDanhSachNVChuaCoTaiKhoan();
                comboBoxNVChuaCoTaiKhoan.DataSource = dtNV;
                comboBoxNVChuaCoTaiKhoan.DisplayMember = "TenNV";
                comboBoxNVChuaCoTaiKhoan.ValueMember = "MaNV";
                comboBoxNVChuaCoTaiKhoan.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH TÀI KHOẢN
        // ══════════════════════════════════════════════════════════════════
        private void LoadData(bool isSearch = false)
        {
            try
            {
                DataTable dt = _busTK.LayDanhSachTaiKhoanNV();

                if (isSearch)
                {
                    string usernameFilter = txtTenDangNhap.Text.Trim();
                    string roleFilter = comboBoxVaiTro.SelectedValue?.ToString()?.Trim();
                    string statusFilter = comboBoxTrangThai.SelectedItem?.ToString()?.Trim();

                    List<string> filterParts = new List<string>();

                    if (!string.IsNullOrWhiteSpace(usernameFilter))
                    {
                        string escapedUsername = usernameFilter.Replace("'", "''");
                        filterParts.Add($"(TenDangNhap LIKE '%{escapedUsername}%' OR MaTK LIKE '%{escapedUsername}%' OR MaNV LIKE '%{escapedUsername}%')");
                    }

                    if (!string.IsNullOrWhiteSpace(roleFilter))
                    {
                        string escapedRole = roleFilter.Replace("'", "''");
                        filterParts.Add($"MaVaiTro = '{escapedRole}'");
                    }

                    if (!string.IsNullOrWhiteSpace(statusFilter))
                    {
                        string escapedStatus = statusFilter.Replace("'", "''");
                        filterParts.Add($"TrangThai = '{escapedStatus}'");
                    }

                    if (filterParts.Count > 0)
                    {
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = string.Join(" AND ", filterParts);
                        dt = dv.ToTable();
                    }
                }

                dataGridViewDSTaiKhoanNhanVien.DataSource = dt;

                // Đặt tên cột thân thiện
                if (dataGridViewDSTaiKhoanNhanVien.Columns["MaTK"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["MaTK"].HeaderText = "Mã TK";
                if (dataGridViewDSTaiKhoanNhanVien.Columns["MaNV"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                if (dataGridViewDSTaiKhoanNhanVien.Columns["MaVaiTro"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["MaVaiTro"].HeaderText = "Vai trò";
                if (dataGridViewDSTaiKhoanNhanVien.Columns["TenDangNhap"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                if (dataGridViewDSTaiKhoanNhanVien.Columns["MatKhau"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["MatKhau"].Visible = false; // Không hiển thị mật khẩu
                if (dataGridViewDSTaiKhoanNhanVien.Columns["TrangThai"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dataGridViewDSTaiKhoanNhanVien.Columns["NgayTao"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["NgayTao"].HeaderText = "Ngày tạo";
                if (dataGridViewDSTaiKhoanNhanVien.Columns["NgayCapNhat"] != null)
                    dataGridViewDSTaiKhoanNhanVien.Columns["NgayCapNhat"].HeaderText = "Ngày cập nhật";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CLICK VÀO DÒNG DATAGRIDVIEW
        // ══════════════════════════════════════════════════════════════════
        private void dataGridViewDSTaiKhoanNhanVien_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSTaiKhoanNhanVien.Rows[e.RowIndex];

            _maTKDangChon = row.Cells["MaTK"]?.Value?.ToString()?.Trim();

            txtTenDangNhap.Text = row.Cells["TenDangNhap"]?.Value?.ToString();
            txtTenDangNhap.ReadOnly = false; // Cho phép sửa tên đăng nhập
            txtMatKhau.Text = ""; // Không hiển thị mật khẩu hash

            // Chọn vai trò trong combobox
            string maVaiTro = row.Cells["MaVaiTro"]?.Value?.ToString()?.Trim() ?? "";
            comboBoxVaiTro.SelectedValue = maVaiTro;

            // Chọn trạng thái
            string trangThai = row.Cells["TrangThai"]?.Value?.ToString() ?? "";
            comboBoxTrangThai.Text = trangThai;

            // Lấy DTO để dùng khi cập nhật
            if (!string.IsNullOrWhiteSpace(_maTKDangChon))
                _tkDangChon = _busTK.LayTaiKhoanNVTheoMa(_maTKDangChon);
        }

        // ══════════════════════════════════════════════════════════════════
        // NÚT THÊM
        // ══════════════════════════════════════════════════════════════════
        private void btnThem_Click(object? sender, EventArgs e)
        {
            // Kiểm tra đã chọn NV chưa
            if (comboBoxNVChuaCoTaiKhoan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để tạo tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBoxVaiTro.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string maNV = comboBoxNVChuaCoTaiKhoan.SelectedValue.ToString()!.Trim();
                string maVaiTro = comboBoxVaiTro.SelectedValue.ToString()!.Trim();
                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string trangThai = comboBoxTrangThai.Text.Trim();
                if (string.IsNullOrWhiteSpace(trangThai)) trangThai = "Hoạt Động";

                var tk = new DTO_TaiKhoanNV
                {
                    MaTK = _busTK.TaoMaTKNVMoi(),
                    MaNV = maNV,
                    MaVaiTro = maVaiTro,
                    TenDangNhap = tenDangNhap,
                    MatKhau = matKhau,
                    TrangThai = trangThai,
                };

                bool ketQua = _busTK.ThemTaiKhoanNV(tk);
                if (ketQua)
                {
                    MessageBox.Show("Tạo tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    NapComboBoxNVChuaCoTaiKhoan();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Tạo tài khoản thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
        // NÚT SỬA (CHỈ SỬA VAI TRÒ VÀ TRẠNG THÁI)
        // ══════════════════════════════════════════════════════════════════
        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (_tkDangChon == null || string.IsNullOrWhiteSpace(_maTKDangChon))
            {
                MessageBox.Show("Vui lòng chọn một tài khoản trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi thông tin tài khoản này không?",
                "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string maVaiTro = comboBoxVaiTro.SelectedValue?.ToString()?.Trim() ?? "";
                string trangThai = comboBoxTrangThai.Text.Trim();
                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();

                if (string.IsNullOrWhiteSpace(maVaiTro))
                {
                    MessageBox.Show("Vui lòng chọn vai trò.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tenDangNhap))
                {
                    MessageBox.Show("Tên đăng nhập không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _tkDangChon.MaVaiTro = maVaiTro;
                _tkDangChon.TrangThai = trangThai;
                _tkDangChon.TenDangNhap = tenDangNhap;

                if (!string.IsNullOrEmpty(matKhau))
                {
                    if (matKhau.Length < 6)
                    {
                        MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    _tkDangChon.MatKhau = _busTK.HashMatKhau(matKhau);
                }

                bool ketQua = _busTK.CapNhatTaiKhoanNV(_tkDangChon);
                if (ketQua)
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        // NÚT XÓA (KHÓA TÀI KHOẢN)
        // ══════════════════════════════════════════════════════════════════
        private void btnXoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_maTKDangChon))
            {
                MessageBox.Show("Vui lòng chọn một tài khoản trong danh sách để khóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn KHÓA tài khoản '{_maTKDangChon}' không?",
                "Xác nhận khóa tài khoản", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ketQua = _busTK.CapNhatTrangThaiNV(_maTKDangChon, "Khóa");
                if (ketQua)
                {
                    MessageBox.Show("Đã khóa tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
        // LÀM MỚI FORM
        // ══════════════════════════════════════════════════════════════════
        private void LamMoiForm()
        {
            _maTKDangChon = null;
            _tkDangChon = null;
            txtTenDangNhap.Clear();
            txtTenDangNhap.ReadOnly = false;
            txtMatKhau.Clear();
            comboBoxVaiTro.SelectedIndex = -1;
            comboBoxTrangThai.SelectedIndex = -1;
            comboBoxNVChuaCoTaiKhoan.SelectedIndex = -1;
        }
    }
}

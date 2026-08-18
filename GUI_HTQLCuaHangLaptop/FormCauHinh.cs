using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormCauHinh : Form
    {
        private readonly BUS_SanPham _busSP = new BUS_SanPham();
        private string? _maCauHinhDangChon = null;

        public FormCauHinh()
        {
            InitializeComponent();

            this.Load += FormCauHinh_Load;
            btnThemKhachHang.Click += btnThem_Click;
            btnSuaKhachHang.Click += btnSua_Click;
            btnXoaKhachHang.Click += btnXoa_Click;
            btnTim.Click += btnTim_Click;
            dataGridViewDSCauHinh.CellClick += dataGridViewDSCauHinh_CellClick;

            comboBoxHangSanXuat.SelectedIndexChanged += comboBoxHangSanXuat_SelectedIndexChanged;

            txtMaLoaiSanPham.ReadOnly = false;
        }

        private void FormCauHinh_Load(object sender, EventArgs e)
        {
            NapComboBoxLoaiSP();
            LamMoiForm();
        }

        private void NapComboBoxLoaiSP()
        {
            try
            {
                DataTable dt = _busSP.LayDanhSachLoaiSP();
                DataRow dr = dt.NewRow();
                dr["MaLoaiSP"] = "ALL";
                dr["TenLoai"] = "Tất cả";
                dt.Rows.InsertAt(dr, 0);

                comboBoxHangSanXuat.DisplayMember = "TenLoai";
                comboBoxHangSanXuat.ValueMember = "MaLoaiSP";
                comboBoxHangSanXuat.DataSource = dt;
                comboBoxHangSanXuat.SelectedValue = "ALL";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách loại sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxHangSanXuat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxHangSanXuat.SelectedValue != null)
            {
                string maLoai = comboBoxHangSanXuat.SelectedValue.ToString()!.Trim();
                LoadData(maLoai);
            }
            else
            {
                dataGridViewDSCauHinh.DataSource = null;
            }
        }

        private void LoadData(string maLoaiSP, string maCauHinh = "", string thongSo = "")
        {
            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(maLoaiSP) || maLoaiSP == "ALL")
                {
                    dt = _busSP.LayTatCaCauHinh();
                }
                else
                {
                    dt = _busSP.LayCauHinhTheoLoaiSP(maLoaiSP);
                }

                string filter = "";
                if (!string.IsNullOrWhiteSpace(maCauHinh))
                {
                    filter += $"(MaCauHinh LIKE '%{maCauHinh.Replace("'", "''")}%')";
                }
                if (!string.IsNullOrWhiteSpace(thongSo))
                {
                    if (filter != "") filter += " AND ";
                    filter += $"(TenThuocTinh LIKE '%{thongSo.Replace("'", "''")}%')";
                }

                if (filter != "")
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = filter;
                    dt = dv.ToTable();
                }

                dataGridViewDSCauHinh.DataSource = dt;

                // Định cấu hình cột hiển thị
                if (dataGridViewDSCauHinh.Columns["MaCauHinh"] != null)
                    dataGridViewDSCauHinh.Columns["MaCauHinh"].HeaderText = "Mã cấu hình";
                if (dataGridViewDSCauHinh.Columns["MaLoaiSP"] != null)
                    dataGridViewDSCauHinh.Columns["MaLoaiSP"].HeaderText = "Mã loại sản phẩm";
                if (dataGridViewDSCauHinh.Columns["TenThuocTinh"] != null)
                    dataGridViewDSCauHinh.Columns["TenThuocTinh"].HeaderText = "Thông số kỹ thuật";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách cấu hình: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewDSCauHinh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSCauHinh.Rows[e.RowIndex];

            _maCauHinhDangChon = row.Cells["MaCauHinh"]?.Value?.ToString()?.Trim();
            txtMaLoaiSanPham.Text = _maCauHinhDangChon;
            txtTenLoaiSanPham.Text = row.Cells["TenThuocTinh"]?.Value?.ToString()?.Trim();

            string configMaLoai = row.Cells["MaLoaiSP"]?.Value?.ToString()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(configMaLoai))
            {
                comboBoxHangSanXuat.SelectedValue = configMaLoai;
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string maLoai = "";
            if (comboBoxHangSanXuat.SelectedValue != null)
            {
                maLoai = comboBoxHangSanXuat.SelectedValue.ToString()!.Trim();
            }
            string maCauHinh = txtMaLoaiSanPham.Text.Trim();
            string thongSo = txtTenLoaiSanPham.Text.Trim();
            LoadData(maLoai, maCauHinh, thongSo);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (comboBoxHangSanXuat.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Loại sản phẩm để thêm cấu hình.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLoai = comboBoxHangSanXuat.SelectedValue.ToString()!.Trim();
            if (maLoai == "ALL")
            {
                MessageBox.Show("Không thể thêm cấu hình khi chọn 'Tất cả'. Vui lòng chọn một loại sản phẩm cụ thể.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string thongSo = txtTenLoaiSanPham.Text.Trim();

                if (string.IsNullOrWhiteSpace(thongSo))
                {
                    MessageBox.Show("Thông số kỹ thuật không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var ch = new DTO_CauHinh
                {
                    MaCauHinh = TaoMaCauHinhMoi(),
                    MaLoaiSP = maLoai,
                    TenThuocTinh = thongSo
                };

                if (_busSP.ThemCauHinh(ch))
                {
                    MessageBox.Show("Thêm cấu hình thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(maLoai);
                    LamMoiFormChiTiet();
                }
                else
                {
                    MessageBox.Show("Thêm cấu hình thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_maCauHinhDangChon))
            {
                MessageBox.Show("Vui lòng chọn một cấu hình trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thay đổi thông tin cấu hình này không?",
                "Xác nhận thay đổi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string maLoai = comboBoxHangSanXuat.SelectedValue!.ToString()!.Trim();
                string thongSo = txtTenLoaiSanPham.Text.Trim();

                if (string.IsNullOrWhiteSpace(thongSo))
                {
                    MessageBox.Show("Thông số kỹ thuật không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var ch = new DTO_CauHinh
                {
                    MaCauHinh = _maCauHinhDangChon,
                    MaLoaiSP = maLoai,
                    TenThuocTinh = thongSo
                };

                if (_busSP.CapNhatCauHinh(ch))
                {
                    MessageBox.Show("Cập nhật cấu hình thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(maLoai);
                    LamMoiFormChiTiet();
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
            if (string.IsNullOrWhiteSpace(_maCauHinhDangChon))
            {
                MessageBox.Show("Vui lòng chọn một cấu hình để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa cấu hình '{_maCauHinhDangChon}' không?\n(Dữ liệu sẽ bị xóa vĩnh viễn khỏi hệ thống)",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string maLoai = comboBoxHangSanXuat.SelectedValue!.ToString()!.Trim();
                if (_busSP.XoaCauHinh(_maCauHinhDangChon))
                {
                    MessageBox.Show("Xóa cấu hình thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(maLoai);
                    LamMoiFormChiTiet();
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

        private void LamMoiForm()
        {
            comboBoxHangSanXuat.SelectedIndex = -1;
            LamMoiFormChiTiet();
        }

        private void LamMoiFormChiTiet()
        {
            _maCauHinhDangChon = null;
            txtMaLoaiSanPham.Text = TaoMaCauHinhMoi();
            txtTenLoaiSanPham.Clear();
        }

        private string TaoMaCauHinhMoi()
        {
            try
            {
                DataTable dtCH = _busSP.LayTatCaCauHinh();
                int soLon = 0;
                foreach (DataRow rCH in dtCH.Rows)
                {
                    string maCH = rCH["MaCauHinh"]?.ToString()?.Trim() ?? "";
                    if (maCH.StartsWith("CH") && maCH.Length == 10)
                    {
                        if (int.TryParse(maCH.Substring(2), out int so))
                        {
                            if (so > soLon) soLon = so;
                        }
                    }
                }
                return "CH" + (soLon + 1).ToString().PadLeft(8, '0');
            }
            catch
            {
                return "CH00000001";
            }
        }
    }
}

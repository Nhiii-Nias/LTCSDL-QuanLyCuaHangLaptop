using System;
using System.Data;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class PhanQuyen : Form
    {
        private readonly BUS_TaiKhoan _busTK = new BUS_TaiKhoan();

        // Mã vai trò đang được chọn
        private string? _maVaiTroDangChon = null;
        private DTO_VaiTro? _vtDangChon = null;

        public PhanQuyen()
        {
            InitializeComponent();

            this.Load += PhanQuyen_Load;
            btnSua.Click += btnSua_Click;
            dataGridViewDSVaiTro.CellClick += dataGridViewDSVaiTro_CellClick;
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD FORM
        // ══════════════════════════════════════════════════════════════════
        private void PhanQuyen_Load(object sender, EventArgs e)
        {
            LoadData();
            LamMoiForm();
        }

        // ══════════════════════════════════════════════════════════════════
        // LOAD DANH SÁCH VAI TRÒ
        // ══════════════════════════════════════════════════════════════════
        private void LoadData()
        {
            try
            {
                DataTable dt = _busTK.LayDanhSachVaiTro();
                dataGridViewDSVaiTro.DataSource = dt;

                // Đặt tên cột
                if (dataGridViewDSVaiTro.Columns["MaVaiTro"] != null)
                    dataGridViewDSVaiTro.Columns["MaVaiTro"].HeaderText = "Mã vai trò";
                if (dataGridViewDSVaiTro.Columns["TenVaiTro"] != null)
                    dataGridViewDSVaiTro.Columns["TenVaiTro"].HeaderText = "Tên vai trò";
                if (dataGridViewDSVaiTro.Columns["MoTaQuyen"] != null)
                    dataGridViewDSVaiTro.Columns["MoTaQuyen"].HeaderText = "Mô tả quyền";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu vai trò: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CLICK VÀO DÒNG DATAGRIDVIEW
        // ══════════════════════════════════════════════════════════════════
        private void dataGridViewDSVaiTro_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridViewDSVaiTro.Rows[e.RowIndex];

            _maVaiTroDangChon = row.Cells["MaVaiTro"]?.Value?.ToString()?.Trim();
            txtTenVaiTro.Text = row.Cells["TenVaiTro"]?.Value?.ToString();
            txtMoTaPhanQuyen.Text = row.Cells["MoTaQuyen"]?.Value?.ToString();

            // Lấy đối tượng đầy đủ
            if (!string.IsNullOrWhiteSpace(_maVaiTroDangChon))
                _vtDangChon = _busTK.LayVaiTroTheoMa(_maVaiTroDangChon);
        }

        // ══════════════════════════════════════════════════════════════════
        // NÚT SỬA — Chỉ cho sửa Tên và Mô tả. Mã vai trò cố định.
        // ══════════════════════════════════════════════════════════════════
        private void btnSua_Click(object? sender, EventArgs e)
        {
            if (_vtDangChon == null || string.IsNullOrWhiteSpace(_maVaiTroDangChon))
            {
                MessageBox.Show("Vui lòng chọn một vai trò trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn thay đổi thông tin vai trò '{_maVaiTroDangChon}' không?",
                "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                string tenVaiTro = txtTenVaiTro.Text.Trim();
                string moTa = txtMoTaPhanQuyen.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenVaiTro))
                {
                    MessageBox.Show("Tên vai trò không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _vtDangChon.TenVaiTro = tenVaiTro;
                _vtDangChon.MoTaQuyen = moTa;

                bool ketQua = _busTK.CapNhatVaiTro(_vtDangChon);
                if (ketQua)
                {
                    MessageBox.Show("Cập nhật vai trò thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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



        // ══════════════════════════════════════════════════════════════════
        // LÀM MỚI FORM
        // ══════════════════════════════════════════════════════════════════
        private void LamMoiForm()
        {
            _maVaiTroDangChon = null;
            _vtDangChon = null;
            txtTenVaiTro.Clear();
            txtMoTaPhanQuyen.Clear();
        }
    }
}

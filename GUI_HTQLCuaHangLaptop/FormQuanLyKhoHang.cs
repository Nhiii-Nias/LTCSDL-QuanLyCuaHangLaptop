using System;
using System.Drawing;
using System.Windows.Forms;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyKhoHang : Form
    {
        private Panel panelContent;
        private Form? _formConHienTai = null;
        private readonly DTO_TaiKhoanNV _taiKhoanHienTai;

        public FormQuanLyKhoHang(DTO_TaiKhoanNV taiKhoan)
        {
            _taiKhoanHienTai = taiKhoan;
            InitializeComponent();

            // Khởi tạo panelContent động để nhúng form con
            this.IsMdiContainer = false;
            
            // Thay đổi thứ tự add để panelContent (Dock.Fill) nằm ở Back và không chui xuống dưới panel1 (Dock.Top)
            this.Controls.Remove(panel1);

            panelContent = new Panel();
            panelContent.Dock = DockStyle.Fill;
            panelContent.BackColor = Color.FromArgb(224, 224, 224); // Màu xám nhạt làm nền
            
            this.Controls.Add(panelContent);
            this.Controls.Add(panel1);

            // Đăng ký sự kiện ComboBox thay đổi lựa chọn
            comboBoxChucNang.SelectedIndexChanged += comboBoxChucNang_SelectedIndexChanged;

            // Load mặc định chức năng đầu tiên sau khi Form load
            this.Load += (s, e) =>
            {
                string maVaiTro = ChuanHoaMaVaiTro(_taiKhoanHienTai.MaVaiTro);
                if (maVaiTro == "VT004")
                {
                    comboBoxChucNang.SelectedIndex = 1;
                }
                else
                {
                    if (comboBoxChucNang.Items.Count > 0)
                    {
                        comboBoxChucNang.SelectedIndex = 0;
                    }
                }
            };
        }

        private string ChuanHoaMaVaiTro(string maVaiTro)
        {
            if (string.IsNullOrWhiteSpace(maVaiTro)) return string.Empty;
            string value = maVaiTro.Trim();
            if (value.StartsWith("VT") && value.Length == 10)
            {
                if (int.TryParse(value.Substring(2), out int num))
                {
                    return $"VT00{num}";
                }
            }
            return value;
        }

        private void comboBoxChucNang_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string maVaiTro = ChuanHoaMaVaiTro(_taiKhoanHienTai.MaVaiTro);
            if (maVaiTro == "VT004" && comboBoxChucNang.SelectedIndex != 1)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxChucNang.SelectedIndex = 1;
                return;
            }

            switch (comboBoxChucNang.SelectedIndex)
            {
                case 0:
                    MoFormCon(new FormQuanLyNhapHang(_taiKhoanHienTai));
                    break;
                case 1:
                    MoFormCon(new FormTonKho(_taiKhoanHienTai.MaVaiTro));
                    break;
                case 2:
                    MoFormCon(new FormQuanLyNhaCungCap(_taiKhoanHienTai.MaVaiTro));
                    break;
                case 3:
                    MoFormCon(new FormDoiTraNCC(_taiKhoanHienTai.MaVaiTro));
                    break;
            }
        }

        private void MoFormCon(Form formCon)
        {
            // Đóng form cũ nếu có
            if (_formConHienTai != null)
            {
                _formConHienTai.Close();
                _formConHienTai.Dispose();
            }

            _formConHienTai = formCon;

            // Đưa WindowState về Normal để thuộc tính Dock.Fill hoạt động chính xác khi nhúng vào Panel
            formCon.WindowState = FormWindowState.Normal;
            formCon.TopLevel = false;
            formCon.FormBorderStyle = FormBorderStyle.None;
            formCon.Dock = DockStyle.Fill;

            panelContent.Controls.Clear();
            panelContent.Controls.Add(formCon);
            formCon.Show();
        }
    }
}

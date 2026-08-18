using System;
using System.Drawing;
using System.Windows.Forms;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyHeThong : Form
    {
        private Panel panelContent;
        private Form? _formConHienTai = null;
        private DTO_TaiKhoanNV _taiKhoanHienTai;

        public FormQuanLyHeThong(DTO_TaiKhoanNV taiKhoan)
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
            comboBoxTrangThai.SelectedIndexChanged += comboBoxTrangThai_SelectedIndexChanged;

            // Load mặc định chức năng đầu tiên sau khi Form load
            this.Load += (s, e) =>
            {
                if (comboBoxTrangThai.Items.Count > 0)
                {
                    comboBoxTrangThai.SelectedIndex = 0;
                }
            };
        }

        private void comboBoxTrangThai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (comboBoxTrangThai.SelectedIndex)
            {
                case 0:
                    MoFormCon(new QuanLyNhanVien(_taiKhoanHienTai));
                    break;
                case 1:
                    MoFormCon(new TaiKhoanNhanVien(_taiKhoanHienTai));
                    break;
                case 2:
                    MoFormCon(new PhanQuyen());
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

using System;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;


namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormDangNhap : Form
    {
        private readonly BUS_TaiKhoan _busTaiKhoan = new BUS_TaiKhoan();
        private readonly BUS_NhanVien _busNhanVien = new BUS_NhanVien();

        public FormDangNhap()
        {
            InitializeComponent();

            // Ẩn ký tự mật khẩu
            txtMatKhau.PasswordChar = '●';

            // Đăng ký sự kiện
            btnDangNhap.Click += btnDangNhap_Click;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
            btnThoat.Click += btnThoat_Click;

            // Cho phép nhấn Enter để đăng nhập
            this.AcceptButton = btnDangNhap;
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: NÚT ĐĂNG NHẬP
        // ══════════════════════════════════════════════════════════════════
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Kiểm tra trống
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Không được để trống tên đăng nhập và mật khẩu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gọi BUS để xác thực đăng nhập
                DTO_TaiKhoanNV? taiKhoan = _busTaiKhoan.DangNhapNV(
                    txtTenDangNhap.Text.Trim(),
                    txtMatKhau.Text);

                if (taiKhoan == null)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác, hoặc tài khoản đã bị khóa.",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMatKhau.Clear();
                    txtMatKhau.Focus();
                    return;
                }

                // Lấy thông tin nhân viên tương ứng
                DTO_NhanVien? nhanVien = _busNhanVien.LayTheoMa(taiKhoan.MaNV);

                // Lấy thông tin vai trò qua BUS
                DTO_VaiTro? vaiTro = _busTaiKhoan.LayVaiTroTheoMa(taiKhoan.MaVaiTro);

                // Mở FormMain và truyền thông tin đăng nhập
                FormMain frmMain = new FormMain(taiKhoan, nhanVien, vaiTro);
                frmMain.FormClosed += (s, args) => {
                    if (frmMain.Tag != null && frmMain.Tag.ToString() == "LOGOUT")
                    {
                        txtMatKhau.Clear();
                        txtTenDangNhap.Clear();
                        this.Show();
                    }
                    else
                    {
                        this.Close();
                    }
                };
                this.Hide();
                frmMain.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}",
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: NÚT ĐỔI MẬT KHẨU (Quên mật khẩu)
        // ══════════════════════════════════════════════════════════════════
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            // Kiểm tra tên đăng nhập không được để trống
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập trước khi đổi mật khẩu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            try
            {
                // Kiểm tra tên đăng nhập có tồn tại trong CSDL không qua BUS
                DTO_TaiKhoanNV? tk = _busTaiKhoan.LayTaiKhoanNVTheoTenDangNhap(txtTenDangNhap.Text.Trim());

                if (tk == null)
                {
                    MessageBox.Show("Tên đăng nhập không tồn tại trong hệ thống.",
                        "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form đổi mật khẩu và truyền mã tài khoản
                FormThayDoiMatKhau frmDoi = new FormThayDoiMatKhau(tk.MaTK, txtTenDangNhap.Text.Trim());
                frmDoi.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra tài khoản: {ex.Message}",
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: NÚT THOÁT
        // ══════════════════════════════════════════════════════════════════
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát khỏi hệ thống?",
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}

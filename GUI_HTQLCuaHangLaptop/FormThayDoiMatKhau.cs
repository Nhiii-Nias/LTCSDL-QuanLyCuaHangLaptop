using System;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormThayDoiMatKhau : Form
    {
        private readonly BUS_TaiKhoan _busTaiKhoan = new BUS_TaiKhoan();
        private readonly string _maTK;          // Mã tài khoản cần đổi mật khẩu
        private readonly string _tenDangNhap;   // Dùng để hiển thị trên tiêu đề

        /// <summary>
        /// Khởi tạo form đổi mật khẩu với mã tài khoản và tên đăng nhập
        /// </summary>
        public FormThayDoiMatKhau(string maTK, string tenDangNhap)
        {
            InitializeComponent();
            _maTK = maTK;
            _tenDangNhap = tenDangNhap;

            // Ẩn ký tự mật khẩu cho cả 3 ô
            txtMatKhauCu.PasswordChar = '●';
            txtMatKhauMoi.PasswordChar = '●';
            txtXacNhanMatKhau.PasswordChar = '●';

            // Đăng ký sự kiện
            btnCapNhat.Click += btnCapNhat_Click;

            // Hiển thị tên đăng nhập trên tiêu đề form
            this.Text = $"Thay đổi mật khẩu — {_tenDangNhap}";
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: NÚT CẬP NHẬT
        // ══════════════════════════════════════════════════════════════════
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            // Kiểm tra các ô không được để trống
            if (string.IsNullOrWhiteSpace(txtMatKhauCu.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cũ.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauCu.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhauMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtXacNhanMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập xác nhận mật khẩu mới.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhanMatKhau.Focus();
                return;
            }

            // Điều kiện 2: Mật khẩu mới và mật khẩu xác nhận phải giống nhau
            if (txtMatKhauMoi.Text != txtXacNhanMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận không khớp. Vui lòng kiểm tra lại.",
                    "Không khớp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhanMatKhau.Clear();
                txtXacNhanMatKhau.Focus();
                return;
            }

            try
            {
                // Điều kiện 1: Mật khẩu cũ phải trùng khớp — BUS sẽ kiểm tra
                bool ketQua = _busTaiKhoan.DoiMatKhauNV(
                    _maTK,
                    txtMatKhauCu.Text,
                    txtMatKhauMoi.Text);

                if (ketQua)
                {
                    MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Đổi mật khẩu thất bại. Vui lòng thử lại.",
                        "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (InvalidOperationException ex)
            {
                // BUS ném InvalidOperationException khi mật khẩu cũ sai
                MessageBox.Show(ex.Message,
                    "Lỗi xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauCu.Clear();
                txtMatKhauCu.Focus();
            }
            catch (ArgumentException ex)
            {
                // BUS ném ArgumentException khi mật khẩu mới không hợp lệ (< 6 ký tự)
                MessageBox.Show(ex.Message,
                    "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

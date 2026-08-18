using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormMain : Form
    {
        // ── Thông tin người dùng đăng nhập ──────────────────────────────
        private DTO_TaiKhoanNV _taiKhoanHienTai;
        private DTO_NhanVien? _nhanVienHienTai;
        private DTO_VaiTro? _vaiTroHienTai;

        public static DTO_TaiKhoanNV? TaiKhoanDangNhap { get; set; }

        // ── Timer đo thời gian đăng nhập ───────────────────────────────
        private System.Windows.Forms.Timer _timer;
        private DateTime _thoiGianDangNhap;

        // ── Mapping: button → danh sách mã vai trò được phép truy cập ──
        // VT001 = Quản trị hệ thống
        // VT002 = Nhân viên bán hàng
        // VT003 = Nhân viên kho
        // VT004 = Nhân viên CSKH
        // VT005 = Quản lý/Giám đốc
        private readonly Dictionary<string, string[]> _quyenTruyCapButton = new Dictionary<string, string[]>
        {
            { "btnQuanLyHeThong",   new[] { "VT001" } },
            { "btnQuanLyDonHang",   new[] { "VT001", "VT002", "VT004" } },
            { "btnQuanLyHopDong",   new[] { "VT001", "VT002" } },
            { "btnQuanLyKhoHang",   new[] { "VT001", "VT003", "VT004" } },
            { "btnKhuyenMai",       new[] { "VT001", "VT002" } },
            { "btnBaoHanh",         new[] { "VT001", "VT004" } },
            { "btnDoiTra",          new[] { "VT001", "VT004" } },
            { "btnKhieuNai",        new[] { "VT001", "VT004" } },
            { "btnDanhMucSanPham",  new[] { "VT001", "VT003", "VT004" } },
            { "btnBaoCaoThongKe",   new[] { "VT001", "VT005" } },
            { "btnQuanLyKhachHang", new[] { "VT001", "VT002", "VT004" } },
        };

        public FormMain(DTO_TaiKhoanNV taiKhoan, DTO_NhanVien? nhanVien, DTO_VaiTro? vaiTro)
        {
            InitializeComponent();

            _taiKhoanHienTai = taiKhoan;
            _nhanVienHienTai = nhanVien;
            _vaiTroHienTai = vaiTro;
            TaiKhoanDangNhap = taiKhoan;

            // Đăng ký sự kiện
            this.Load += FormMain_Load;
            buttonDangXuat.Click += buttonDangXuat_Click;

            // Đăng ký sự kiện click cho từng button chức năng
            btnQuanLyHeThong.Click  += BtnChucNang_Click;
            btnQuanLyDonHang.Click  += BtnChucNang_Click;
            btnQuanLyKhachHang.Click += BtnChucNang_Click;
            btnQuanLyHopDong.Click  += BtnChucNang_Click;
            btnQuanLyKhoHang.Click  += BtnChucNang_Click;
            btnKhuyenMai.Click      += BtnChucNang_Click;
            btnBaoHanh.Click        += BtnChucNang_Click;
            btnDoiTra.Click         += BtnChucNang_Click;
            btnKhieuNai.Click       += BtnChucNang_Click;
            btnDanhMucSanPham.Click += BtnChucNang_Click;
            btnBaoCaoThongKe.Click  += BtnChucNang_Click;
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: FORM LOAD
        // ══════════════════════════════════════════════════════════════════
        private void FormMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin trên StatusStrip
            string tenNV = _nhanVienHienTai?.TenNV ?? _taiKhoanHienTai.TenDangNhap;
            string tenVaiTro = _vaiTroHienTai?.TenVaiTro ?? _taiKhoanHienTai.MaVaiTro;

            LabelTenNhanVien.Text = $"👤  {tenNV}";
            labelVaiTro.Text = $"🔑  {tenVaiTro}";
            labelThoiGian.Text = "⏱  00:00:00";

            // Khởi động timer đo thời gian
            _thoiGianDangNhap = DateTime.Now;
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000; // 1 giây
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: TIMER — Cập nhật thời gian mỗi giây
        // ══════════════════════════════════════════════════════════════════
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeSpan thoiGian = DateTime.Now - _thoiGianDangNhap;
            labelThoiGian.Text = "⏱  " + thoiGian.ToString(@"hh\:mm\:ss");
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: CLICK CÁC NÚT CHỨC NĂNG — Kiểm tra phân quyền
        // ══════════════════════════════════════════════════════════════════
        private void BtnChucNang_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;

            string tenBtn = btn.Name;

            // Kiểm tra quyền truy cập
            if (_quyenTruyCapButton.TryGetValue(tenBtn, out string[]? danhSachQuyen))
            {
                string maVaiTro = ChuanHoaMaVaiTro(_taiKhoanHienTai.MaVaiTro);
                bool coQuyen = Array.Exists(danhSachQuyen, q => ChuanHoaMaVaiTro(q) == maVaiTro);

                if (!coQuyen)
                {
                    MessageBox.Show("Bạn không có quyền truy cập.",
                        "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Mở form tương ứng với button được nhấn
            MoFormTuButton(tenBtn);
        }

        /// <summary>
        /// Chuẩn hóa mã vai trò để đảm bảo tương thích giữa database (ví dụ: VT00000001) và mapping trong code (VT001).
        /// </summary>
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

        // ══════════════════════════════════════════════════════════════════
        // HÀM MỞ FORM CON THEO TÊN BUTTON (MDI Child hoặc Dialog)
        // ══════════════════════════════════════════════════════════════════
        private void MoFormTuButton(string tenButton)
        {
            Form? formMoi = tenButton switch
            {
                "btnQuanLyHeThong"   => new FormQuanLyHeThong(_taiKhoanHienTai),
                "btnQuanLyDonHang"   => new FormQuanLyDonHang(_nhanVienHienTai?.MaNV ?? "NV00000001", _taiKhoanHienTai.MaVaiTro, _nhanVienHienTai?.TenNV),
                "btnQuanLyHopDong"   => new FormQuanLyHopDong(_nhanVienHienTai?.MaNV ?? "NV00000001"),
                "btnQuanLyKhoHang"   => new FormQuanLyKhoHang(_taiKhoanHienTai),
                "btnQuanLyKhachHang" => new FormQuanLyKhachHang(_nhanVienHienTai?.MaNV ?? "NV00000001"),
                "btnKhuyenMai"       => new FormQuanLyKhuyenMai(),
                "btnBaoHanh"         => new FormBaoHanh(),
                "btnDoiTra"          => new FormDoiTraSanPham(),
                "btnKhieuNai"        => new FormKhieuNai(),
                "btnDanhMucSanPham"  => new FormDanhMucSanPham(_taiKhoanHienTai),
                "btnBaoCaoThongKe"   => new FormBaoCaoThongKe(),
                _ => null
            };

            if (formMoi == null)
            {
                return;
            }

            // Đóng tất cả các form con MDI hiện tại trước khi mở form mới để tránh chồng đè
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }

            // Gán MDI parent và hiện form phẳng, không viền để lấp đầy FormMain, loại bỏ ControlBox phụ
            formMoi.MdiParent = this;
            formMoi.WindowState = FormWindowState.Normal;
            formMoi.FormBorderStyle = FormBorderStyle.None;
            formMoi.Dock = DockStyle.Fill;
            formMoi.Show();

            // Đảm bảo ép lại WindowState và Dock sau khi Show để tránh WinForms tự động phục hồi thuộc tính Maximized thiết kế
            formMoi.WindowState = FormWindowState.Normal;
            formMoi.Dock = DockStyle.Fill;
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN: NÚT ĐĂNG XUẤT
        // ══════════════════════════════════════════════════════════════════
        private void buttonDangXuat_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Dừng timer
                _timer?.Stop();
                _timer?.Dispose();

                // Đóng tất cả MDI children
                foreach (Form f in this.MdiChildren)
                {
                    f.Close();
                }

                // Gắn tín hiệu đăng xuất và đóng FormMain
                this.Tag = "LOGOUT";
                this.Close();
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // OVERRIDE: XỬ LÝ KHI ĐÓNG FORM
        // ══════════════════════════════════════════════════════════════════
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer?.Stop();
            _timer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}

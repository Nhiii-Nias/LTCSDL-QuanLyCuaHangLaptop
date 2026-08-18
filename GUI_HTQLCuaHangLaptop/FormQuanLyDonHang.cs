using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormQuanLyDonHang : Form
    {
        // ── BUS ─────────────────────────────────────────────────────────
        private readonly BUS_DonHang    _busDH  = new BUS_DonHang();
        private readonly BUS_SanPham    _busSP  = new BUS_SanPham();
        private readonly BUS_KhuyenMai  _busKM  = new BUS_KhuyenMai();
        private readonly BUS_HopDong    _busHD  = new BUS_HopDong();

        // ── Mã nhân viên đang đăng nhập ────────────────────────────────
        private readonly string _maNV;
        private readonly string? _maVaiTro;
        private readonly string? _tenNV;

        // ── Bảng tạm (chưa lưu DB) ─────────────────────────────────────
        // Cột: MaDH, MaLoaiSP, TenLoai, SoLuong, DonGia, ThanhTien
        private DataTable _bangTam = new DataTable();
        private bool      _dangTaoMoi = false;    // true khi đang trong chế độ tạo đơn mới
        private string    _maDHTam    = string.Empty; // mã đơn hàng tạm thời

        // ── Danh sách các loại sản phẩm (để ComboBox) ──────────────────
        private DataTable _dsLoaiSP = new DataTable();

        private bool IsVT004(string? maVaiTro)
        {
            if (string.IsNullOrWhiteSpace(maVaiTro)) return false;
            string value = maVaiTro.Trim();
            if (value == "VT004" || value == "VT00000004") return true;
            if (value.StartsWith("VT") && value.Length == 10)
            {
                if (int.TryParse(value.Substring(2), out int num))
                {
                    return num == 4;
                }
            }
            return false;
        }

        public FormQuanLyDonHang(string maNV = "NV00000001", string? maVaiTro = null, string? tenNV = null)
        {
            InitializeComponent();
            _maNV = maNV;
            _maVaiTro = maVaiTro;
            _tenNV = tenNV;

            // Khởi tạo bảng tạm
            _bangTam.Columns.Add("MaDH",      typeof(string));
            _bangTam.Columns.Add("MaLoaiSP",  typeof(string));
            _bangTam.Columns.Add("TenLoai",   typeof(string));
            _bangTam.Columns.Add("SoLuong",   typeof(int));
            _bangTam.Columns.Add("DonGia",    typeof(decimal));
            _bangTam.Columns.Add("ThanhTien", typeof(decimal));

            // Sự kiện
            this.Load += FormQuanLyDonHang_Load;
            comboBoxLoaiGridVewHienThi.SelectedIndexChanged += ComboBoxLoaiHienThi_SelectedIndexChanged;
            btnThem.Click      += BtnThem_Click;
            btnXoa.Click       += BtnXoa_Click;
            btnSua.Click       += BtnSua_Click;
            btnTim.Click       += BtnTim_Click;
            btnXacNhan.Click   += BtnXacNhan_Click;
            comboBoxChonLoaiSanPham.SelectedIndexChanged += ComboBoxLoaiSP_Changed;
            txtMaKH.TextChanged += (s, e) => {
                if (_dangTaoMoi) CapNhatGioHangVaKhuyenMai();
            };
            comboBoxMaKhuyenMai.SelectedIndexChanged += (s, e) => {
                if (_dangTaoMoi) TinhVaHienThiTien();
            };
            dataGridView.CellClick += DataGridView_CellClick;
            dataGridView.DataSourceChanged += dataGridView_DataSourceChanged;
            dataGridView.CellValueChanged += dataGridView_CellValueChanged;
        }

        // ══════════════════════════════════════════════════════════════════
        // FORM LOAD
        // ══════════════════════════════════════════════════════════════════
        private void FormQuanLyDonHang_Load(object sender, EventArgs e)
        {
            // Reset các label kết quả về 0
            DatLabelVe0();

            // Load ComboBox loại SP từ DB
            NapComboBoxLoaiSanPham();

            // Load ComboBox trạng thái đúng giá trị
            comboBoxTrangThai.Items.Clear();
            comboBoxTrangThai.Items.AddRange(new object[] {
                "Tất cả", "Chờ Xử Lý", "Đang Giao", "Hoàn Thành", "Huỷ" });
            comboBoxTrangThai.SelectedIndex = 0;

            // Load ComboBox phương thức thanh toán
            comboBoxPhuongThucThanhToan.Items.Clear();
            comboBoxPhuongThucThanhToan.Items.AddRange(new object[] {
                "Tất cả", "Tiền Mặt", "Chuyển Khoản", "Thẻ" });
            comboBoxPhuongThucThanhToan.SelectedIndex = 0;

            // Load ComboBox hợp đồng (hiển thị toàn bộ mã HD đang Hiệu Lực)
            NapComboBoxHopDong();

            // Mặc định hiển thị "Danh sách đơn hàng"
            comboBoxLoaiGridVewHienThi.SelectedIndex = 0;

            CapNhatKhaNangChinhSua();
        }

        // ══════════════════════════════════════════════════════════════════
        // HELPERS — NAP DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════
        private void NapComboBoxLoaiSanPham()
        {
            try
            {
                _dsLoaiSP = _busSP.LayDanhSachLoaiSP();
                comboBoxChonLoaiSanPham.Items.Clear();
                comboBoxChonLoaiSanPham.Items.Add("Tất cả loại sản phẩm");
                foreach (DataRow row in _dsLoaiSP.Rows)
                {
                    comboBoxChonLoaiSanPham.Items.Add(
                        $"{row["MaLoaiSP"]} - {row["TenLoai"]}");
                }
                if (comboBoxChonLoaiSanPham.Items.Count > 0)
                    comboBoxChonLoaiSanPham.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách loại sản phẩm: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapComboBoxHopDong()
        {
            try
            {
                comboBoxMaHopDong.Items.Clear();
                comboBoxMaHopDong.Items.Add("(Không có)");
                var dtHD = _busHD.LayDanhSachHopDong();
                foreach (DataRow row in dtHD.Rows)
                {
                    if (row["TrangThai"].ToString() == "Hiệu Lực")
                        comboBoxMaHopDong.Items.Add(row["MaHD"].ToString()!.Trim());
                }
                comboBoxMaHopDong.SelectedIndex = 0;
            }
            catch { comboBoxMaHopDong.SelectedIndex = -1; }
        }

        private void DatLabelVe0()
        {
            labelKetQuaTongTienHang.Text    = "0 ₫";
            labelKetQuaKhuyenMaiApDung.Text = "Không có";
            labelKetQuaSoTienGiam.Text      = "0 ₫";
            labelKetQuaTongTienPhaiTra.Text = "0 ₫";

            comboBoxMaKhuyenMai.Items.Clear();
            comboBoxMaKhuyenMai.Items.Add("(Không có)");
            comboBoxMaKhuyenMai.SelectedIndex = 0;
        }

        private void NapComboBoxKhuyenMaiChoTimKiem()
        {
            try
            {
                comboBoxMaKhuyenMai.Items.Clear();
                comboBoxMaKhuyenMai.Items.Add("Tất cả");
                comboBoxMaKhuyenMai.Items.Add("Không khuyến mãi");
                var dtKM = _busKM.LayDanhSachKhuyenMai();
                foreach (DataRow row in dtKM.Rows)
                {
                    if (DateTime.TryParse(row["NgayBatDau"]?.ToString(), out DateTime ngayBD) &&
                        DateTime.TryParse(row["NgayKetThuc"]?.ToString(), out DateTime ngayKT))
                    {
                        if (ngayBD <= DateTime.Today && DateTime.Today <= ngayKT)
                        {
                            comboBoxMaKhuyenMai.Items.Add(row["MaKM"].ToString()!.Trim());
                        }
                    }
                }
                comboBoxMaKhuyenMai.SelectedIndex = 0;
            }
            catch
            {
                comboBoxMaKhuyenMai.Items.Clear();
                comboBoxMaKhuyenMai.Items.Add("Tất cả");
                comboBoxMaKhuyenMai.Items.Add("Không khuyến mãi");
                comboBoxMaKhuyenMai.SelectedIndex = 0;
            }
        }

        private void CapNhatKhaNangChinhSua()
        {
            bool isSanPhamMode = (comboBoxLoaiGridVewHienThi.SelectedIndex == 1);
            bool isChiTietMode = (comboBoxLoaiGridVewHienThi.SelectedIndex == 2);
            bool isTamMode = _dangTaoMoi;

            if (isSanPhamMode)
            {
                // Tất cả textbox, combobox khác ngoài comboBoxChonLoaiSanPham đều không chỉnh sửa hay nhập được
                txtMaDonHang.Enabled = false;
                txtMaKH.Enabled = false;
                txtTenNhanVien.Enabled = false;
                comboBoxPhuongThucThanhToan.Enabled = false;
                comboBoxMaKhuyenMai.Enabled = false;
                comboBoxMaHopDong.Enabled = false;
                comboBoxTrangThai.Enabled = false;
                txtSoLuong.Enabled = false;
                comboBoxChonLoaiSanPham.Enabled = true;

                // Chỉ có button tìm kiếm hoạt động, còn lại đều không hoạt động
                btnTim.Enabled = true;
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                btnSua.Enabled = false;
                btnXacNhan.Enabled = false;
            }
            else if (isChiTietMode)
            {
                txtMaDonHang.Enabled = true;
                txtMaKH.Enabled = false;
                txtTenNhanVien.Enabled = false;
                comboBoxPhuongThucThanhToan.Enabled = false;
                comboBoxMaKhuyenMai.Enabled = false;
                comboBoxMaHopDong.Enabled = false;
                comboBoxTrangThai.Enabled = false;
                txtSoLuong.Enabled = true;
                comboBoxChonLoaiSanPham.Enabled = true;

                btnTim.Enabled = true;
                btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnSua.Enabled = false;
                btnXacNhan.Enabled = false;
            }
            else if (isTamMode)
            {
                txtMaDonHang.Enabled = false;
                txtMaKH.Enabled = true;
                txtTenNhanVien.Enabled = false;
                comboBoxPhuongThucThanhToan.Enabled = true;
                comboBoxMaKhuyenMai.Enabled = true;
                comboBoxMaHopDong.Enabled = true;
                comboBoxTrangThai.Enabled = true;
                txtSoLuong.Enabled = true;
                comboBoxChonLoaiSanPham.Enabled = true;

                btnTim.Enabled = false;
                btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnSua.Enabled = false;
                btnXacNhan.Enabled = true;
            }
            else
            {
                txtMaDonHang.Enabled = true;
                txtMaKH.Enabled = true;
                txtTenNhanVien.Enabled = true;
                comboBoxPhuongThucThanhToan.Enabled = true;
                comboBoxMaKhuyenMai.Enabled = true;
                comboBoxMaHopDong.Enabled = true;
                comboBoxTrangThai.Enabled = true;
                txtSoLuong.Enabled = true;
                comboBoxChonLoaiSanPham.Enabled = true;

                btnTim.Enabled = true;
                btnThem.Enabled = true;
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
                btnXacNhan.Enabled = false;
            }
        }

        private string? LayMaLoaiSPDuocChon()
        {
            if (comboBoxChonLoaiSanPham.SelectedIndex <= 0) return null;
            string text = comboBoxChonLoaiSanPham.SelectedItem!.ToString()!;
            if (!text.Contains("-")) return null;
            return text.Split('-')[0].Trim();
        }

        private string FormatTien(decimal so) =>
            so.ToString("N0") + " ₫";

        // ══════════════════════════════════════════════════════════════════
        // COMBOBOX CHỌN LOẠI HIỂN THỊ
        // ══════════════════════════════════════════════════════════════════
        private void ComboBoxLoaiHienThi_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangTaoMoi)
            {
                // Đang tạo đơn → luôn hiển thị bảng tạm
                dataGridView.DataSource = _bangTam;
                return;
            }

            switch (comboBoxLoaiGridVewHienThi.SelectedIndex)
            {
                case 0: // Danh sách đơn hàng
                    HienThiDanhSachDonHang();
                    break;
                case 1: // Danh sách sản phẩm
                    HienThiDanhSachSanPham();
                    break;
                case 2: // Chi tiết đơn hàng
                    HienThiChiTietDonHang();
                    break;
            }
            CapNhatKhaNangChinhSua();
        }

        private void HienThiDanhSachDonHang()
        {
            try
            {
                dataGridView.DataSource = _busDH.LayDanhSachDonHang();
                NapComboBoxKhuyenMaiChoTimKiem();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách đơn hàng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiDanhSachSanPham()
        {
            try
            {
                dataGridView.DataSource = _busSP.LayDanhSachSanPham();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách sản phẩm: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable? MaskPlaceholderSerials(DataTable? dt)
        {
            if (dt == null) return null;
            DataTable dtCloned = dt.Copy();
            if (dtCloned.Columns.Contains("MaSerialSP"))
            {
                foreach (DataRow row in dtCloned.Rows)
                {
                    string serial = row["MaSerialSP"]?.ToString()?.Trim() ?? "";
                    if (serial.StartsWith("x-", StringComparison.OrdinalIgnoreCase))
                    {
                        row["MaSerialSP"] = "x";
                    }
                }
            }
            return dtCloned;
        }

        private void HienThiChiTietDonHang()
        {
            try
            {
                // Hiển thị chi tiết của đơn được chọn trong DGV hoặc toàn bộ
                string? maDHChon = null;
                if (dataGridView.CurrentRow != null && dataGridView.Columns.Contains("MaDH"))
                    maDHChon = dataGridView.CurrentRow.Cells["MaDH"].Value?.ToString()?.Trim();

                if (!string.IsNullOrWhiteSpace(maDHChon))
                    dataGridView.DataSource = MaskPlaceholderSerials(_busDH.LayChiTietDonHang(maDHChon));
                else
                    dataGridView.DataSource = MaskPlaceholderSerials(_busDH.LayTatCaChiTietDonHang());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải chi tiết đơn hàng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN THÊM — Tạo đơn hàng tạm hoặc thêm dòng vào bảng tạm
        // ══════════════════════════════════════════════════════════════════
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (comboBoxLoaiGridVewHienThi.SelectedIndex == 1) return;
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maLoaiSP = LayMaLoaiSPDuocChon() ?? "";

            if (comboBoxLoaiGridVewHienThi.SelectedIndex == 2) // Chi tiết đơn hàng mode
            {
                if (dataGridView.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn một dòng chi tiết thuộc đơn hàng cần thêm sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maDHSelected = dataGridView.CurrentRow.Cells["MaDH"].Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(maDHSelected)) return;

                var dh = _busDH.LayTheoMa(maDHSelected);
                if (dh == null) return;

                if (dh.TrangThai != "Chờ Xử Lý")
                {
                    MessageBox.Show("Chỉ được phép thêm sản phẩm mới vào đơn hàng ở trạng thái 'Chờ Xử Lý'.", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(maLoaiSP))
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy số lượng
                if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuongChiTiet) || soLuongChiTiet <= 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dtTonKhoChiTiet = _busDH.LaySanPhamTonKhoTheoLoaiSP(maLoaiSP);
                if (dtTonKhoChiTiet.Rows.Count < soLuongChiTiet && string.IsNullOrEmpty(dh.MaHD))
                {
                    MessageBox.Show($"Không đủ hàng tồn kho. Còn {dtTonKhoChiTiet.Rows.Count} sản phẩm khả dụng.", "Lỗi tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var listSerials = new List<string>();
                    int layDuoc = 0;
                    foreach (DataRow r in dtTonKhoChiTiet.Rows)
                    {
                        if (layDuoc >= soLuongChiTiet) break;
                        listSerials.Add(r["MaSerialSP"].ToString()!.Trim());
                        layDuoc++;
                    }

                    if (layDuoc < soLuongChiTiet && !string.IsNullOrEmpty(dh.MaHD))
                    {
                        for (int i = layDuoc; i < soLuongChiTiet; i++)
                        {
                            listSerials.Add($"x-{maLoaiSP}-{maDHSelected}-{i}");
                        }
                    }

                    bool ok = _busDH.ThemSanPhamVaoDonHangHienCo(maDHSelected, maLoaiSP, listSerials);
                    if (ok)
                    {
                        MessageBox.Show($"✅ Đã thêm {soLuongChiTiet} sản phẩm vào đơn hàng '{maDHSelected}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiChiTietDonHang();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(maLoaiSP))
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm trước.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy số lượng
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu chưa tạo đơn mới → khởi tạo
            if (!_dangTaoMoi)
            {
                // Kiểm tra mã đơn hàng nhập tay
                string maDHNhap = txtMaDonHang.Text.Trim();
                string maDHSuDung;

                if (string.IsNullOrWhiteSpace(maDHNhap))
                {
                    // Tự sinh mã mới
                    maDHSuDung = _busDH.TaoMaDHMoi();
                }
                else
                {
                    // Kiểm tra xem mã đã tồn tại chưa
                    var dhCu = _busDH.LayTheoMa(maDHNhap);
                    if (dhCu != null)
                    {
                        // Đơn hàng đã có trong DB → sinh mã mới
                        maDHSuDung = _busDH.TaoMaDHMoi();
                        MessageBox.Show($"Mã đơn hàng '{maDHNhap}' đã tồn tại. " +
                            $"Hệ thống tự tạo mã mới: {maDHSuDung}",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        maDHSuDung = maDHNhap;
                    }
                }

                _maDHTam    = maDHSuDung;
                _dangTaoMoi = true;
                _bangTam.Rows.Clear();
                txtMaDonHang.Text = _maDHTam;
                txtMaDonHang.ReadOnly = true;

                // Tự động điền tên nhân viên đăng nhập và khóa lại
                txtTenNhanVien.Text = _tenNV ?? _maNV;
                txtTenNhanVien.ReadOnly = true;

                dataGridView.DataSource = _bangTam;
            }

            // Lấy tên loại SP
            string tenLoai = "";
            decimal donGia = 0m;
            foreach (DataRow row in _dsLoaiSP.Rows)
            {
                if (row["MaLoaiSP"].ToString()!.Trim().Equals(maLoaiSP, StringComparison.OrdinalIgnoreCase))
                {
                    tenLoai = row["TenLoai"].ToString()!;
                    donGia  = Convert.ToDecimal(row["GiaBanGoc"]);
                    break;
                }
            }

            // Kiểm tra đủ tồn kho không
            var dtTonKho = _busDH.LaySanPhamTonKhoTheoLoaiSP(maLoaiSP);
            // Đếm số lượng đã đặt trong bảng tạm cho loại SP này
            int daChon = 0;
            foreach (DataRow row in _bangTam.Rows)
                if (row["MaLoaiSP"].ToString()!.Trim().Equals(maLoaiSP, StringComparison.OrdinalIgnoreCase))
                    daChon += Convert.ToInt32(row["SoLuong"]);

            bool coHopDong = comboBoxMaHopDong.SelectedIndex > 0;
            if (!coHopDong && dtTonKho.Rows.Count < daChon + soLuong)
            {
                MessageBox.Show($"Không đủ hàng tồn kho. " +
                    $"Còn {dtTonKho.Rows.Count} sản phẩm loại '{tenLoai}', " +
                    $"đã chọn {daChon}, không thể thêm {soLuong} nữa.",
                    "Lỗi tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm dòng vào bảng tạm
            decimal thanhTien = donGia * soLuong;
            _bangTam.Rows.Add(_maDHTam, maLoaiSP, tenLoai, soLuong, donGia, thanhTien);

            // Cập nhật các label tổng tiền
            CapNhatGioHangVaKhuyenMai();
        }

        private List<DTO_ChiTietDonHang> LayDanhSachChiTietTam()
        {
            var list = new List<DTO_ChiTietDonHang>();
            foreach (DataRow row in _bangTam.Rows)
            {
                string loaiSP = row["MaLoaiSP"].ToString()!.Trim();
                int soLuong = Convert.ToInt32(row["SoLuong"]);
                decimal donGia = Convert.ToDecimal(row["DonGia"]);

                DataTable dtTonKho = _busDH.LaySanPhamTonKhoTheoLoaiSP(loaiSP);
                int layDuoc = 0;
                foreach (DataRow spRow in dtTonKho.Rows)
                {
                    if (layDuoc >= soLuong) break;
                    string serial = spRow["MaSerialSP"].ToString()!.Trim();
                    list.Add(new DTO_ChiTietDonHang
                    {
                        MaDH = _maDHTam,
                        MaSerialSP = serial,
                        GiaBan = donGia,
                        PhanTramGiam = 0
                    });
                    layDuoc++;
                }
                
                while (layDuoc < soLuong)
                {
                    list.Add(new DTO_ChiTietDonHang
                    {
                        MaDH = _maDHTam,
                        MaSerialSP = $"x-{loaiSP}-{_maDHTam}-{layDuoc}",
                        GiaBan = donGia,
                        PhanTramGiam = 0
                    });
                    layDuoc++;
                }
            }
            return list;
        }

        private void CapNhatDanhSachKhuyenMaiPhuHop()
        {
            try
            {
                string maKH = txtMaKH.Text.Trim();
                var chiTiet = LayDanhSachChiTietTam();

                string selectedKM = comboBoxMaKhuyenMai.SelectedItem?.ToString() ?? "";

                comboBoxMaKhuyenMai.Items.Clear();
                comboBoxMaKhuyenMai.Items.Add("(Không có)");

                if (!string.IsNullOrWhiteSpace(maKH) && chiTiet.Count > 0)
                {
                    var lstKM = _busKM.LayDanhSachKMCoTheThuHuong(maKH, chiTiet, DateTime.Today);
                    foreach (var km in lstKM)
                    {
                        comboBoxMaKhuyenMai.Items.Add(km.MaKM);
                    }
                }

                if (comboBoxMaKhuyenMai.Items.Contains(selectedKM))
                {
                    comboBoxMaKhuyenMai.SelectedItem = selectedKM;
                }
                else
                {
                    comboBoxMaKhuyenMai.SelectedIndex = 0;
                }
            }
            catch
            {
                // Muted
            }
        }

        private void CapNhatGioHangVaKhuyenMai()
        {
            CapNhatKhaNangChinhSua();
            CapNhatDanhSachKhuyenMaiPhuHop();
            TinhVaHienThiTien();
        }

        private void TinhVaHienThiTien()
        {
            decimal tongTienHang = 0m;
            foreach (DataRow row in _bangTam.Rows)
                tongTienHang += Convert.ToDecimal(row["ThanhTien"]);

            labelKetQuaTongTienHang.Text = FormatTien(tongTienHang);

            string maKMChon = comboBoxMaKhuyenMai.SelectedItem?.ToString() ?? "";
            if (!string.IsNullOrEmpty(maKMChon) && maKMChon != "(Không có)")
            {
                try
                {
                    var km = _busKM.LayTheoMa(maKMChon);
                    if (km != null)
                    {
                        labelKetQuaKhuyenMaiApDung.Text = km.TenKM;
                        var chiTiet = LayDanhSachChiTietTam();
                        decimal soTienGiam = _busKM.TinhTienGiam(km, chiTiet);
                        decimal tongTienPhaiTra = tongTienHang - soTienGiam;
                        if (tongTienPhaiTra < 0m) tongTienPhaiTra = 0m;

                        labelKetQuaSoTienGiam.Text = FormatTien(soTienGiam);
                        labelKetQuaTongTienPhaiTra.Text = FormatTien(tongTienPhaiTra);
                    }
                    else
                    {
                        labelKetQuaKhuyenMaiApDung.Text = "Không có";
                        labelKetQuaSoTienGiam.Text = FormatTien(0m);
                        labelKetQuaTongTienPhaiTra.Text = FormatTien(tongTienHang);
                    }
                }
                catch
                {
                    labelKetQuaKhuyenMaiApDung.Text = "Không có";
                    labelKetQuaSoTienGiam.Text = FormatTien(0m);
                    labelKetQuaTongTienPhaiTra.Text = FormatTien(tongTienHang);
                }
            }
            else
            {
                labelKetQuaKhuyenMaiApDung.Text = "Không có";
                labelKetQuaSoTienGiam.Text = FormatTien(0m);
                labelKetQuaTongTienPhaiTra.Text = FormatTien(tongTienHang);
            }
        }

        private void ComboBoxLoaiSP_Changed(object? sender, EventArgs e)
        {
            // Không cần xử lý đặc biệt
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN XÁC NHẬN — Lưu đơn hàng tạm vào CSDL
        // ══════════════════════════════════════════════════════════════════
        private void BtnXacNhan_Click(object? sender, EventArgs e)
        {
            if (comboBoxLoaiGridVewHienThi.SelectedIndex == 1) return;
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_dangTaoMoi || _bangTam.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có đơn hàng nào để xác nhận.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validate MaKH
            string maKH = txtMaKH.Text.Trim();
            if (string.IsNullOrWhiteSpace(maKH))
            {
                if (comboBoxMaHopDong.SelectedIndex > 0)
                {
                    string maHDSelected = comboBoxMaHopDong.SelectedItem?.ToString()?.Trim() ?? "";
                    var hd = _busHD.LayTheoMa(maHDSelected);
                    if (hd != null && !string.IsNullOrWhiteSpace(hd.MaKH))
                    {
                        maKH = hd.MaKH.Trim();
                        txtMaKH.Text = maKH;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(maKH))
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Phương thức thanh toán
            string phuongThuc = comboBoxPhuongThucThanhToan.SelectedItem?.ToString() ?? "Tiền Mặt";

            // Mã hợp đồng
            string? maHD = null;
            if (comboBoxMaHopDong.SelectedIndex > 0)
                maHD = comboBoxMaHopDong.SelectedItem?.ToString()?.Trim();

            // Mã khuyến mãi
            string? maKM = null;
            if (comboBoxMaKhuyenMai.SelectedIndex > 0)
                maKM = comboBoxMaKhuyenMai.SelectedItem?.ToString()?.Trim();

            try
            {
                // Lấy MaNV từ session đăng nhập
                string maNV = _maNV;

                // Xây dựng DTO đơn hàng
                var dh = new DTO_DonHang
                {
                    MaDH = _maDHTam,
                    MaNV = maNV,
                    MaKH = maKH,
                    MaKM = maKM!,
                    MaHD = maHD!,
                    PhuongThucThanhToan = phuongThuc,
                    NgayDat = DateTime.Now,
                };

                // Xây dựng danh sách serial (lấy serial tồn kho theo từng loại SP)
                var danhSachSerial = new List<string>();
                foreach (DataRow row in _bangTam.Rows)
                {
                    string loaiSP = row["MaLoaiSP"].ToString()!.Trim();
                    int soLuong   = Convert.ToInt32(row["SoLuong"]);

                    var dtTonKho = _busDH.LaySanPhamTonKhoTheoLoaiSP(loaiSP);
                    int layDuoc  = 0;
                    foreach (DataRow spRow in dtTonKho.Rows)
                    {
                        if (layDuoc >= soLuong) break;
                        string serial = spRow["MaSerialSP"].ToString()!.Trim();
                        // Tránh trùng serial đã chọn
                        if (!danhSachSerial.Contains(serial))
                        {
                            danhSachSerial.Add(serial);
                            layDuoc++;
                        }
                    }
                    if (layDuoc < soLuong)
                    {
                        if (maHD != null)
                        {
                            for (int i = layDuoc; i < soLuong; i++)
                            {
                                string placeholderSerial = $"x-{loaiSP}-{_maDHTam}-{i}";
                                danhSachSerial.Add(placeholderSerial);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Không đủ serial tồn kho cho loại '{row["TenLoai"]}'. " +
                                $"Cần {soLuong} nhưng chỉ còn {layDuoc} sản phẩm khả dụng.",
                                "Lỗi tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // Gọi BUS tạo đơn hàng (transaction 3 bước)
                bool ok = _busDH.TaoDonHang(dh, danhSachSerial);

                if (ok)
                {
                    // Tính toán hiển thị kết quả thực tế
                    var dhSaved = _busDH.LayTheoMa(_maDHTam);
                    if (dhSaved != null)
                    {
                        labelKetQuaTongTienHang.Text    = FormatTien(dhSaved.TongTien);
                        labelKetQuaKhuyenMaiApDung.Text = string.IsNullOrEmpty(dhSaved.MaKM) ? "Không có" : dhSaved.MaKM;
                        decimal tienGiam = dhSaved.TongTien - (dhSaved.TienSauGiam ?? dhSaved.TongTien);
                        labelKetQuaSoTienGiam.Text      = FormatTien(tienGiam);
                        labelKetQuaTongTienPhaiTra.Text = FormatTien(dhSaved.TienSauGiam ?? dhSaved.TongTien);
                    }

                    MessageBox.Show($"✅ Tạo đơn hàng '{_maDHTam}' thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset bảng tạm
                    _bangTam.Rows.Clear();
                    _dangTaoMoi          = false;
                    _maDHTam             = string.Empty;
                    txtMaDonHang.Text    = string.Empty;
                    txtMaDonHang.ReadOnly = false;

                    // Mở khóa ô Tên nhân viên
                    txtTenNhanVien.Text  = string.Empty;
                    txtTenNhanVien.ReadOnly = false;

                    // Trở về hiển thị danh sách đơn hàng
                    comboBoxLoaiGridVewHienThi.SelectedIndex = 0;
                    HienThiDanhSachDonHang();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo đơn hàng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN XÓA — Chỉ xóa được khi đang trong trạng thái tạo mới (chưa lưu DB)
        // ══════════════════════════════════════════════════════════════════
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (comboBoxLoaiGridVewHienThi.SelectedIndex == 1) return;
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_dangTaoMoi)
            {
                if (comboBoxLoaiGridVewHienThi.SelectedIndex == 1) // SanPham
                {
                    MessageBox.Show("Bạn không có quyền xoá sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (comboBoxLoaiGridVewHienThi.SelectedIndex == 2) // Chi tiết đơn hàng
                {
                    if (dataGridView.CurrentRow == null)
                    {
                        MessageBox.Show("Vui lòng chọn dòng chi tiết đơn hàng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string maDHSelect = dataGridView.CurrentRow.Cells["MaDH"].Value?.ToString()?.Trim() ?? "";
                    string maSerialSP = dataGridView.CurrentRow.Cells["MaSerialSP"].Value?.ToString()?.Trim() ?? "";

                    if (string.IsNullOrWhiteSpace(maDHSelect) || string.IsNullOrWhiteSpace(maSerialSP))
                    {
                        MessageBox.Show("Không thể xác định thông tin chi tiết đơn hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var dh = _busDH.LayTheoMa(maDHSelect);
                    if (dh == null) return;
                    if (dh.TrangThai != "Chờ Xử Lý")
                    {
                        MessageBox.Show("Chỉ được phép xóa chi tiết đơn hàng của các đơn hàng ở trạng thái 'Chờ Xử Lý'.", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult confirmDetail = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa sản phẩm '{maSerialSP}' ra khỏi đơn hàng '{maDHSelect}' không?",
                        "Xác nhận xóa chi tiết đơn hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmDetail != DialogResult.Yes) return;

                    try
                    {
                        bool ok = _busDH.XoaDongChiTietDonHang(maDHSelect, maSerialSP);
                        if (ok)
                        {
                            MessageBox.Show("✅ Đã xóa chi tiết đơn hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HienThiChiTietDonHang();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa chi tiết đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
                MessageBox.Show("Chỉ có thể xóa đơn hàng đang trong quá trình tạo (chưa xác nhận).\n" +
                    "Để hủy đơn hàng đã lưu, hãy dùng chức năng Sửa → đổi trạng thái sang 'Huỷ'.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataGridView.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng sản phẩm trên bảng tạm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa dòng sản phẩm được chọn khỏi bảng tạm không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                int idx = dataGridView.CurrentRow.Index;
                if (idx >= 0 && idx < _bangTam.Rows.Count)
                {
                    _bangTam.Rows.RemoveAt(idx);
                    CapNhatGioHangVaKhuyenMai();
                    MessageBox.Show("Đã xóa dòng sản phẩm được chọn khỏi bảng tạm.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN SỬA — Chỉ sửa MaKM, PhuongThucThanhToan và TrangThai
        // ══════════════════════════════════════════════════════════════════
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (comboBoxLoaiGridVewHienThi.SelectedIndex == 1) return;
            if (IsVT004(_maVaiTro))
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_dangTaoMoi)
            {
                MessageBox.Show("Không thể sửa trong khi đang tạo đơn hàng mới.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Phải chọn một dòng trong DataGridView (tab Danh sách đơn hàng)
            if (dataGridView.CurrentRow == null || !dataGridView.Columns.Contains("MaDH"))
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần sửa từ danh sách (tab Danh sách đơn hàng).",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maDH = dataGridView.CurrentRow.Cells["MaDH"].Value?.ToString()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(maDH))
            {
                MessageBox.Show("Không xác định được mã đơn hàng.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dhGoc = _busDH.LayTheoMa(maDH);
            if (dhGoc == null)
            {
                MessageBox.Show("Đơn hàng không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dhGoc.TrangThai != "Chờ Xử Lý")
            {
                MessageBox.Show("Chỉ được phép chỉnh sửa những đơn hàng ở trạng thái 'Chờ Xử Lý'.", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tra cứu nhân viên từ tên nhân viên nhập vào
            string? maNVMoi = null;
            string tenNVInput = txtTenNhanVien.Text.Trim();
            if (!string.IsNullOrWhiteSpace(tenNVInput))
            {
                BUS_NhanVien busNV = new BUS_NhanVien();
                DataTable dtNV = busNV.LayDanhSach();
                bool timThay = false;
                foreach (DataRow r in dtNV.Rows)
                {
                    if (r["TenNV"].ToString()!.Trim().Equals(tenNVInput, StringComparison.OrdinalIgnoreCase))
                    {
                        maNVMoi = r["MaNV"].ToString()!.Trim();
                        timThay = true;
                        break;
                    }
                }
                if (!timThay)
                {
                    MessageBox.Show("Tên nhân viên không tồn tại trong hệ thống.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn thay đổi đơn hàng '{maDH}' không?",
                "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                string? maKMMoi = null;
                if (comboBoxMaKhuyenMai.SelectedIndex > 0)
                    maKMMoi = comboBoxMaKhuyenMai.SelectedItem?.ToString()?.Trim();
                string  phuongThucMoi = comboBoxPhuongThucThanhToan.SelectedItem?.ToString() ?? "Tiền Mặt";
                string  trangThaiMoi  = comboBoxTrangThai.SelectedItem?.ToString() ?? "Chờ Xử Lý";

                bool ok = _busDH.CapNhatGioiHan(maDH, maKMMoi, phuongThucMoi, trangThaiMoi, maNVMoi);
                if (ok)
                {
                    MessageBox.Show("✅ Cập nhật đơn hàng thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachDonHang();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Kiểm tra lại dữ liệu.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi sửa đơn hàng: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // BTN TÌM KIẾM — Tìm theo nhiều điều kiện kết hợp
        // ══════════════════════════════════════════════════════════════════
        private void BtnTim_Click(object? sender, EventArgs e)
        {
            if (comboBoxLoaiGridVewHienThi.SelectedIndex == 1) // Danh sách sản phẩm
            {
                try
                {
                    string? maLoaiSP = LayMaLoaiSPDuocChon();
                    DataTable dt = _busSP.LayDanhSachSanPham();
                    var rows = dt.AsEnumerable();

                    if (!string.IsNullOrWhiteSpace(maLoaiSP))
                    {
                        rows = rows.Where(r => r.Field<string>("MaLoaiSP") != null && r.Field<string>("MaLoaiSP").Trim().Equals(maLoaiSP, StringComparison.OrdinalIgnoreCase));
                    }

                    DataTable filteredDt = rows.Any() ? rows.CopyToDataTable() : dt.Clone();
                    dataGridView.DataSource = filteredDt;

                    if (filteredDt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp với điều kiện tìm kiếm.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tìm kiếm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            if (_dangTaoMoi)
            {
                MessageBox.Show("Không thể tìm kiếm trong khi đang tạo đơn hàng.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxLoaiGridVewHienThi.SelectedIndex == 2) // Chi tiết đơn hàng mode
            {
                try
                {
                    string maDH = txtMaDonHang.Text.Trim();
                    string? maLoaiSP = LayMaLoaiSPDuocChon();

                    DataTable dt = MaskPlaceholderSerials(_busDH.LayTatCaChiTietDonHang());
                    var rows = dt.AsEnumerable();

                    if (!string.IsNullOrWhiteSpace(maDH))
                    {
                        rows = rows.Where(r => r.Field<string>("MaDH") != null && r.Field<string>("MaDH").Trim().StartsWith(maDH, StringComparison.OrdinalIgnoreCase));
                    }
                    if (!string.IsNullOrWhiteSpace(maLoaiSP))
                    {
                        rows = rows.Where(r => r.Field<string>("MaLoaiSP") != null && r.Field<string>("MaLoaiSP").Trim().Equals(maLoaiSP, StringComparison.OrdinalIgnoreCase));
                    }

                    DataTable filteredDt = rows.Any() ? rows.CopyToDataTable() : dt.Clone();
                    dataGridView.DataSource = filteredDt;

                    if (filteredDt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy chi tiết đơn hàng nào phù hợp với điều kiện tìm kiếm.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tìm kiếm chi tiết đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            try
            {
                // Lấy giá trị từ form
                string? maKH               = string.IsNullOrWhiteSpace(txtMaKH.Text)           ? null : txtMaKH.Text.Trim();
                string? tenNV              = string.IsNullOrWhiteSpace(txtTenNhanVien.Text)     ? null : txtTenNhanVien.Text.Trim();
                string? phuongThuc         = (comboBoxPhuongThucThanhToan.SelectedIndex < 0)    ? null : comboBoxPhuongThucThanhToan.SelectedItem?.ToString();
                string? maKM = null;
                if (comboBoxMaKhuyenMai.SelectedIndex > 0)
                {
                    string selectedKM = comboBoxMaKhuyenMai.SelectedItem?.ToString()?.Trim() ?? "";
                    if (selectedKM != "Tất cả")
                    {
                        maKM = selectedKM;
                    }
                }
                string? maHDTim            = (comboBoxMaHopDong.SelectedIndex <= 0)            ? null : comboBoxMaHopDong.SelectedItem?.ToString()?.Trim();
                string? maDH               = string.IsNullOrWhiteSpace(txtMaDonHang.Text)       ? null : txtMaDonHang.Text.Trim();
                string? maLoaiSP           = LayMaLoaiSPDuocChon();
                string? trangThai          = (comboBoxTrangThai.SelectedIndex < 0)              ? null : comboBoxTrangThai.SelectedItem?.ToString();

                // Kiểm tra có ít nhất 1 điều kiện
                bool coTimKiem = maKH != null || tenNV != null || (phuongThuc != null && phuongThuc != "Tất cả") ||
                                 maKM != null || maHDTim != null || maDH != null || maLoaiSP != null || (trangThai != null && trangThai != "Tất cả");

                DataTable dt;
                if (!coTimKiem)
                {
                    // Không có điều kiện → load toàn bộ theo tab đang chọn
                    ComboBoxLoaiHienThi_SelectedIndexChanged(null, EventArgs.Empty);
                    return;
                }

                // Tìm kiếm đơn hàng theo nhiều điều kiện
                dt = _busDH.TimKiemNhieuDieuKien(maKH, tenNV, phuongThuc, maKM, maHDTim, maDH, maLoaiSP, trangThai);

                // Chuyển sang tab "Danh sách đơn hàng" để hiển thị kết quả
                comboBoxLoaiGridVewHienThi.SelectedIndex = 0;
                dataGridView.DataSource = dt;

                if (dt.Rows.Count == 0)
                    MessageBox.Show("Không tìm thấy đơn hàng nào phù hợp với điều kiện tìm kiếm.",
                        "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DataGridView_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (_dangTaoMoi) return;
            if (dataGridView.CurrentRow == null) return;

            try
            {
                if (dataGridView.Columns.Contains("MaDH") && dataGridView.CurrentRow.Cells["MaDH"].Value != null)
                {
                    txtMaDonHang.Text = dataGridView.CurrentRow.Cells["MaDH"].Value.ToString()!.Trim();
                }

                if (dataGridView.Columns.Contains("MaKH") && dataGridView.CurrentRow.Cells["MaKH"].Value != null)
                {
                    txtMaKH.Text = dataGridView.CurrentRow.Cells["MaKH"].Value.ToString()!.Trim();
                }

                string maKM = "";
                if (dataGridView.Columns.Contains("MaKM") && dataGridView.CurrentRow.Cells["MaKM"].Value != null)
                {
                    maKM = dataGridView.CurrentRow.Cells["MaKM"].Value.ToString()!.Trim();
                }

                if (!string.IsNullOrEmpty(maKM))
                {
                    if (!comboBoxMaKhuyenMai.Items.Contains(maKM))
                    {
                        comboBoxMaKhuyenMai.Items.Add(maKM);
                    }
                    comboBoxMaKhuyenMai.SelectedItem = maKM;
                }
                else
                {
                    if (comboBoxMaKhuyenMai.Items.Count > 0)
                        comboBoxMaKhuyenMai.SelectedIndex = 0;
                }

                if (dataGridView.Columns.Contains("MaNV") && dataGridView.CurrentRow.Cells["MaNV"].Value != null)
                {
                    string maNV = dataGridView.CurrentRow.Cells["MaNV"].Value.ToString()!.Trim();
                    BUS_NhanVien busNV = new BUS_NhanVien();
                    var nv = busNV.LayTheoMa(maNV);
                    txtTenNhanVien.Text = nv?.TenNV ?? maNV;
                }
                else
                {
                    txtTenNhanVien.Text = string.Empty;
                }

                if (dataGridView.Columns.Contains("PhuongThucThanhToan") && dataGridView.CurrentRow.Cells["PhuongThucThanhToan"].Value != null)
                {
                    string pt = dataGridView.CurrentRow.Cells["PhuongThucThanhToan"].Value.ToString()!.Trim();
                    int idx = comboBoxPhuongThucThanhToan.FindStringExact(pt);
                    if (idx >= 0) comboBoxPhuongThucThanhToan.SelectedIndex = idx;
                }

                if (dataGridView.Columns.Contains("TrangThai") && dataGridView.CurrentRow.Cells["TrangThai"].Value != null)
                {
                    string tt = dataGridView.CurrentRow.Cells["TrangThai"].Value.ToString()!.Trim();
                    int idx = comboBoxTrangThai.FindStringExact(tt);
                    if (idx >= 0) comboBoxTrangThai.SelectedIndex = idx;
                }

                if (dataGridView.Columns.Contains("MaHD") && dataGridView.CurrentRow.Cells["MaHD"].Value != null)
                {
                    string mahd = dataGridView.CurrentRow.Cells["MaHD"].Value.ToString()!.Trim();
                    int idx = comboBoxMaHopDong.FindStringExact(mahd);
                    if (idx >= 0) comboBoxMaHopDong.SelectedIndex = idx;
                    else comboBoxMaHopDong.SelectedIndex = 0;
                }
                else
                {
                    comboBoxMaHopDong.SelectedIndex = 0;
                }
            }
            catch
            {
                // Phớt lờ lỗi load cell click phụ
            }
        }

        private void dataGridView_DataSourceChanged(object? sender, EventArgs e)
        {
            if (IsVT004(_maVaiTro))
            {
                dataGridView.ReadOnly = true;
                return;
            }

            dataGridView.ReadOnly = false;
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                if (col.Name == "MaKM" || col.Name == "PhuongThucThanhToan" || col.Name == "TrangThai" || col.Name == "MaHD" || col.Name == "MaKH" || col.Name == "SoLuong" || col.Name == "DonGia")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }
        }

        private void dataGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            string colName = dataGridView.Columns[e.ColumnIndex].Name;
            object val = row.Cells[e.ColumnIndex].Value;

            if (_dangTaoMoi)
            {
                if (colName == "SoLuong" || colName == "DonGia")
                {
                    int soLuong = 0;
                    decimal donGia = 0m;
                    if (row.Cells["SoLuong"].Value != null)
                        int.TryParse(row.Cells["SoLuong"].Value.ToString(), out soLuong);
                    if (row.Cells["DonGia"].Value != null)
                        decimal.TryParse(row.Cells["DonGia"].Value.ToString(), out donGia);

                    row.Cells["ThanhTien"].Value = soLuong * donGia;
                    CapNhatGioHangVaKhuyenMai();
                }
            }
            else
            {
                if (comboBoxLoaiGridVewHienThi.SelectedIndex == 0) // DonHang
                {
                    if (colName == "MaKM")
                    {
                        string maKM = val?.ToString()?.Trim() ?? "";
                        if (!string.IsNullOrEmpty(maKM))
                        {
                            if (!comboBoxMaKhuyenMai.Items.Contains(maKM))
                            {
                                comboBoxMaKhuyenMai.Items.Add(maKM);
                            }
                            comboBoxMaKhuyenMai.SelectedItem = maKM;
                        }
                        else
                        {
                            if (comboBoxMaKhuyenMai.Items.Count > 0)
                                comboBoxMaKhuyenMai.SelectedIndex = 0;
                        }
                    }
                    else if (colName == "PhuongThucThanhToan")
                    {
                        comboBoxPhuongThucThanhToan.Text = val?.ToString()?.Trim();
                    }
                    else if (colName == "TrangThai")
                    {
                        comboBoxTrangThai.Text = val?.ToString()?.Trim();
                    }
                    else if (colName == "MaHD")
                    {
                        comboBoxMaHopDong.Text = val?.ToString()?.Trim();
                    }
                    else if (colName == "MaKH")
                    {
                        txtMaKH.Text = val?.ToString()?.Trim();
                    }
                }
                else if (comboBoxLoaiGridVewHienThi.SelectedIndex == 1) // SanPham
                {
                    if (colName == "TrangThai")
                    {
                        comboBoxTrangThai.Text = val?.ToString()?.Trim();
                    }
                }
            }
        }
    }
}

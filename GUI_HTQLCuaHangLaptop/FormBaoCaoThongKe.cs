using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using BUS_HTQLCuaHangLaptop;
using DTO_HTQLCuaHangLaptop;

namespace GUI_HTQLCuaHangLaptop
{
    public partial class FormBaoCaoThongKe : Form
    {
        private readonly BUS_BaoCao _busBaoCao = new BUS_BaoCao();
        private DataTable? _dtBaoCao = null;

        public FormBaoCaoThongKe()
        {
            InitializeComponent();

            this.Load += FormBaoCaoThongKe_Load;
            btnTaoBaoCao.Click += BtnTaoBaoCao_Click;
            pictureBoxChart.Paint += PictureBoxChart_Paint;
            comboBoxLoaiBaoCao.SelectedIndexChanged += ComboBoxLoaiBaoCao_SelectedIndexChanged;
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN LOAD FORM
        // ══════════════════════════════════════════════════════════════════
        private void FormBaoCaoThongKe_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định cho DateTimePickers: Từ đầu năm nay đến hiện tại
            dateTimePickerNgayBatDau.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dateTimePickerNgayKetThuc.Value = DateTime.Now;

            // Nạp danh sách các Loại báo cáo vào ComboBox
            comboBoxLoaiBaoCao.Items.Clear();
            comboBoxLoaiBaoCao.Items.AddRange(new object[]
            {
                "Doanh thu theo tháng",
                "Doanh thu theo hình thức thanh toán",
                "Chi phí nhập hàng theo tháng",
                "Tồn kho theo danh mục"
            });
            comboBoxLoaiBaoCao.SelectedIndex = 0;

            // Chạy tạo báo cáo lần đầu
            TaoBaoCao();
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN THAY ĐỔI LOẠI BÁO CÁO
        // ══════════════════════════════════════════════════════════════════
        private void ComboBoxLoaiBaoCao_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Khi đổi loại báo cáo, nếu là "Tồn kho theo danh mục" thì ẩn/hiện bộ chọn ngày vì nó là thống kê tức thời
            bool laTonKho = comboBoxLoaiBaoCao.SelectedIndex == 3;
            dateTimePickerNgayBatDau.Enabled = !laTonKho;
            dateTimePickerNgayKetThuc.Enabled = !laTonKho;
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN CLICK NÚT TẠO BÁO CÁO
        // ══════════════════════════════════════════════════════════════════
        private void BtnTaoBaoCao_Click(object? sender, EventArgs e)
        {
            TaoBaoCao();
        }

        // ══════════════════════════════════════════════════════════════════
        // HÀM XỬ LÝ LẤY DỮ LIỆU VÀ CẬP NHẬT GIAO DIỆN
        // ══════════════════════════════════════════════════════════════════
        private void TaoBaoCao()
        {
            DateTime tuNgay = dateTimePickerNgayBatDau.Value.Date;
            DateTime denNgay = dateTimePickerNgayKetThuc.Value.Date;

            if (tuNgay > denNgay && comboBoxLoaiBaoCao.SelectedIndex != 3)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", 
                    "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int loaiBC = comboBoxLoaiBaoCao.SelectedIndex;
                switch (loaiBC)
                {
                    case 0: // Doanh thu theo tháng
                        _dtBaoCao = _busBaoCao.ThongKeDoanhThuTheoThang(tuNgay, denNgay);
                        HienThiBaoCaoDoanhThuThang();
                        break;
                    case 1: // Doanh thu theo hình thức thanh toán
                        _dtBaoCao = _busBaoCao.ThongKeTheoHinhThucThanhToan(tuNgay, denNgay);
                        HienThiBaoCaoHinhThucThanhToan();
                        break;
                    case 2: // Chi phí nhập hàng theo tháng
                        _dtBaoCao = _busBaoCao.ThongKeNhapHangTheoThang(tuNgay, denNgay);
                        HienThiBaoCaoNhapHangThang();
                        break;
                    case 3: // Tồn kho theo danh mục
                        _dtBaoCao = _busBaoCao.ThongKeTonKhoTheoDanhMuc();
                        HienThiBaoCaoTonKhoDanhMuc();
                        break;
                }

                // Vẽ lại biểu đồ
                pictureBoxChart.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo báo cáo: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CÁC HÀM HIỂN THỊ DỮ LIỆU RA GRIDVIEW VÀ KPI
        // ══════════════════════════════════════════════════════════════════
        private void HienThiBaoCaoDoanhThuThang()
        {
            dataGridView.DataSource = _dtBaoCao;
            if (dataGridView.Columns["Nam"] != null) dataGridView.Columns["Nam"].HeaderText = "Năm";
            if (dataGridView.Columns["Thang"] != null) dataGridView.Columns["Thang"].HeaderText = "Tháng";
            if (dataGridView.Columns["SoDonHang"] != null) dataGridView.Columns["SoDonHang"].HeaderText = "Số đơn";
            if (dataGridView.Columns["TongDoanhThu"] != null) dataGridView.Columns["TongDoanhThu"].HeaderText = "Doanh thu gốc";
            if (dataGridView.Columns["TongDoanhThuSauGiam"] != null) dataGridView.Columns["TongDoanhThuSauGiam"].HeaderText = "Doanh thu thực tế";

            // Định dạng cột tiền tệ
            if (dataGridView.Columns["TongDoanhThu"] != null) dataGridView.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
            if (dataGridView.Columns["TongDoanhThuSauGiam"] != null) dataGridView.Columns["TongDoanhThuSauGiam"].DefaultCellStyle.Format = "N0";

            // Tính KPI
            decimal tongDoanhThuSauGiam = 0m;
            decimal tongDoanhThuGoc = 0m;
            int tongSoDonHang = 0;

            if (_dtBaoCao != null)
            {
                foreach (DataRow row in _dtBaoCao.Rows)
                {
                    tongDoanhThuSauGiam += Convert.ToDecimal(row["TongDoanhThuSauGiam"]);
                    tongDoanhThuGoc += Convert.ToDecimal(row["TongDoanhThu"]);
                    tongSoDonHang += Convert.ToInt32(row["SoDonHang"]);
                }
            }

            lblCardTitle1.Text = "TỔNG DOANH THU THỰC TẾ";
            lblCardVal1.Text = DinhDangTien(tongDoanhThuSauGiam);

            lblCardTitle2.Text = "TỔNG SỐ ĐƠN HÀNG";
            lblCardVal2.Text = tongSoDonHang.ToString("N0") + " Đơn";

            lblCardTitle3.Text = "TỔNG SỐ TIỀN GIẢM KHUYẾN MÃI";
            lblCardVal3.Text = DinhDangTien(tongDoanhThuGoc - tongDoanhThuSauGiam);
        }

        private void HienThiBaoCaoHinhThucThanhToan()
        {
            dataGridView.DataSource = _dtBaoCao;
            if (dataGridView.Columns["PhuongThucThanhToan"] != null) dataGridView.Columns["PhuongThucThanhToan"].HeaderText = "Hình thức TT";
            if (dataGridView.Columns["SoDonHang"] != null) dataGridView.Columns["SoDonHang"].HeaderText = "Số đơn";
            if (dataGridView.Columns["TongDoanhThu"] != null) dataGridView.Columns["TongDoanhThu"].HeaderText = "Doanh thu gốc";
            if (dataGridView.Columns["TongDoanhThuSauGiam"] != null) dataGridView.Columns["TongDoanhThuSauGiam"].HeaderText = "Doanh thu thực";

            if (dataGridView.Columns["TongDoanhThu"] != null) dataGridView.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
            if (dataGridView.Columns["TongDoanhThuSauGiam"] != null) dataGridView.Columns["TongDoanhThuSauGiam"].DefaultCellStyle.Format = "N0";

            // Tính KPI
            decimal tongDoanhThu = 0m;
            int tongSoDonHang = 0;
            string hinhThucUaChuong = "Không có";
            int maxDon = -1;

            if (_dtBaoCao != null)
            {
                foreach (DataRow row in _dtBaoCao.Rows)
                {
                    decimal dt = Convert.ToDecimal(row["TongDoanhThuSauGiam"]);
                    int sd = Convert.ToInt32(row["SoDonHang"]);
                    string httt = row["PhuongThucThanhToan"]?.ToString() ?? "";

                    tongDoanhThu += dt;
                    tongSoDonHang += sd;

                    if (sd > maxDon)
                    {
                        maxDon = sd;
                        hinhThucUaChuong = httt;
                    }
                }
            }

            lblCardTitle1.Text = "TỔNG DOANH THU THỰC TẾ";
            lblCardVal1.Text = DinhDangTien(tongDoanhThu);

            lblCardTitle2.Text = "TỔNG SỐ ĐƠN HÀNG";
            lblCardVal2.Text = tongSoDonHang.ToString("N0") + " Đơn";

            lblCardTitle3.Text = "HÌNH THỨC SỬ DỤNG NHIỀU";
            lblCardVal3.Text = hinhThucUaChuong;
        }

        private void HienThiBaoCaoNhapHangThang()
        {
            dataGridView.DataSource = _dtBaoCao;
            if (dataGridView.Columns["Nam"] != null) dataGridView.Columns["Nam"].HeaderText = "Năm";
            if (dataGridView.Columns["Thang"] != null) dataGridView.Columns["Thang"].HeaderText = "Tháng";
            if (dataGridView.Columns["SoPhieuNhap"] != null) dataGridView.Columns["SoPhieuNhap"].HeaderText = "Số phiếu nhập";
            if (dataGridView.Columns["TongTienNhap"] != null) dataGridView.Columns["TongTienNhap"].HeaderText = "Tổng tiền nhập";
            if (dataGridView.Columns["TongSoLuongNhap"] != null) dataGridView.Columns["TongSoLuongNhap"].HeaderText = "Tổng SL nhập";

            if (dataGridView.Columns["TongTienNhap"] != null) dataGridView.Columns["TongTienNhap"].DefaultCellStyle.Format = "N0";

            // Tính KPI
            decimal tongTienNhap = 0m;
            int tongSoPhieu = 0;
            int tongSoLuong = 0;

            if (_dtBaoCao != null)
            {
                foreach (DataRow row in _dtBaoCao.Rows)
                {
                    tongTienNhap += Convert.ToDecimal(row["TongTienNhap"]);
                    tongSoPhieu += Convert.ToInt32(row["SoPhieuNhap"]);
                    tongSoLuong += Convert.ToInt32(row["TongSoLuongNhap"]);
                }
            }

            lblCardTitle1.Text = "TỔNG CHI PHÍ NHẬP HÀNG";
            lblCardVal1.Text = DinhDangTien(tongTienNhap);

            lblCardTitle2.Text = "TỔNG SỐ PHIẾU NHẬP KHỐ";
            lblCardVal2.Text = tongSoPhieu.ToString("N0") + " Phiếu";

            lblCardTitle3.Text = "TỔNG SỐ SẢN PHẨM NHẬP";
            lblCardVal3.Text = tongSoLuong.ToString("N0") + " Máy";
        }

        private void HienThiBaoCaoTonKhoDanhMuc()
        {
            dataGridView.DataSource = _dtBaoCao;
            if (dataGridView.Columns["DanhMuc"] != null) dataGridView.Columns["DanhMuc"].HeaderText = "Danh mục";
            if (dataGridView.Columns["SoLoaiSP"] != null) dataGridView.Columns["SoLoaiSP"].HeaderText = "Số mẫu SP";
            if (dataGridView.Columns["TongTonKho"] != null) dataGridView.Columns["TongTonKho"].HeaderText = "Tồn kho";
            if (dataGridView.Columns["TongDaBan"] != null) dataGridView.Columns["TongDaBan"].HeaderText = "Đã bán";
            if (dataGridView.Columns["TongBaoHanh"] != null) dataGridView.Columns["TongBaoHanh"].HeaderText = "Bảo hành";
            if (dataGridView.Columns["TongTatCa"] != null) dataGridView.Columns["TongTatCa"].HeaderText = "Tổng số serial";

            // Tính KPI
            int tongTon = 0;
            int tongBan = 0;
            int tongBaoHanh = 0;

            if (_dtBaoCao != null)
            {
                foreach (DataRow row in _dtBaoCao.Rows)
                {
                    tongTon += Convert.ToInt32(row["TongTonKho"]);
                    tongBan += Convert.ToInt32(row["TongDaBan"]);
                    tongBaoHanh += Convert.ToInt32(row["TongBaoHanh"]);
                }
            }

            lblCardTitle1.Text = "TỔNG SẢN PHẨM TỒN KHO";
            lblCardVal1.Text = tongTon.ToString("N0") + " Máy";

            lblCardTitle2.Text = "TỔNG SẢN PHẨM ĐÃ BÁN";
            lblCardVal2.Text = tongBan.ToString("N0") + " Máy";

            lblCardTitle3.Text = "ĐANG BẢO HÀNH";
            lblCardVal3.Text = tongBaoHanh.ToString("N0") + " Máy";
        }

        // ══════════════════════════════════════════════════════════════════
        // SỰ KIỆN PAINT: TỰ VẼ BIỂU ĐỒ BẰNG GDI+
        // ══════════════════════════════════════════════════════════════════
        private void PictureBoxChart_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle bounds = pictureBoxChart.ClientRectangle;

            // Xóa nền trắng và vẽ khung bao quanh
            g.Clear(Color.White);
            using (Pen borderPen = new Pen(Color.Gainsboro, 1))
            {
                g.DrawRectangle(borderPen, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }

            if (_dtBaoCao == null || _dtBaoCao.Rows.Count == 0)
            {
                using (Font italicFont = new Font("Constantia", 14F, FontStyle.Italic))
                {
                    string text = "Không có số liệu báo cáo để vẽ biểu đồ.";
                    SizeF size = g.MeasureString(text, italicFont);
                    g.DrawString(text, italicFont, Brushes.Gray, 
                        (bounds.Width - size.Width) / 2, (bounds.Height - size.Height) / 2);
                }
                return;
            }

            int loaiBC = comboBoxLoaiBaoCao.SelectedIndex;

            switch (loaiBC)
            {
                case 0: // Doanh thu theo tháng -> Biểu đồ cột
                    VeBieuDoCot(g, bounds, "BIỂU ĐỒ DOANH THU THEO THÁNG", "Tháng", "Doanh thu", "Thang", "Nam", "TongDoanhThuSauGiam");
                    break;
                case 1: // Doanh thu theo hình thức thanh toán -> Biểu đồ tròn
                    VeBieuDoTron(g, bounds, "BIỂU ĐỒ DOANH THU THEO HÌNH THỨC THANH TOÁN", "PhuongThucThanhToan", "TongDoanhThuSauGiam");
                    break;
                case 2: // Chi phí nhập hàng -> Biểu đồ cột
                    VeBieuDoCot(g, bounds, "BIỂU ĐỒ CHI PHÍ NHẬP HÀNG THEO THÁNG", "Tháng", "Tiền nhập", "Thang", "Nam", "TongTienNhap");
                    break;
                case 3: // Tồn kho theo danh mục -> Biểu đồ cột so sánh
                    VeBieuDoCotTonKho(g, bounds, "THỐNG KÊ TỒN KHO THEO DANH MỤC");
                    break;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CHI TIẾT: VẼ BIỂU ĐỒ CỘT (CHO DOANH THU & PHIẾU NHẬP)
        // ══════════════════════════════════════════════════════════════════
        private void VeBieuDoCot(Graphics g, Rectangle bounds, string tieuDe, string nhanX, string nhanY, 
            string colThang, string colNam, string colValue)
        {
            // Vẽ tiêu đề
            using (Font titleFont = new Font("Constantia", 14F, FontStyle.Bold))
            {
                SizeF titleSize = g.MeasureString(tieuDe, titleFont);
                g.DrawString(tieuDe, titleFont, Brushes.SlateBlue, (bounds.Width - titleSize.Width) / 2, 20);
            }

            // Khoảng vẽ biểu đồ
            int paddingLeft = 90;
            int paddingRight = 30;
            int paddingTop = 70;
            int paddingBottom = 60;
            Rectangle chartArea = new Rectangle(paddingLeft, paddingTop, 
                bounds.Width - paddingLeft - paddingRight, bounds.Height - paddingTop - paddingBottom);

            // Vẽ trục tọa độ
            using (Pen axisPen = new Pen(Color.DimGray, 2))
            {
                g.DrawLine(axisPen, chartArea.Left, chartArea.Bottom, chartArea.Right, chartArea.Bottom); // Trục X
                g.DrawLine(axisPen, chartArea.Left, chartArea.Top, chartArea.Left, chartArea.Bottom); // Trục Y
            }

            int rowCount = _dtBaoCao!.Rows.Count;
            decimal maxValue = 0m;
            foreach (DataRow row in _dtBaoCao.Rows)
            {
                decimal val = Convert.ToDecimal(row[colValue]);
                if (val > maxValue) maxValue = val;
            }

            if (maxValue == 0) maxValue = 1; // Tránh chia cho 0

            // Làm tròn maxValue lên một chút để biểu đồ đẹp hơn
            double log = Math.Log10((double)maxValue);
            double pow = Math.Pow(10, Math.Floor(log));
            if (pow < 1) pow = 1;
            maxValue = (decimal)(Math.Ceiling((double)maxValue / pow) * pow);

            // Vẽ các đường lưới Y và nhãn Y
            int lineCount = 5;
            using (Pen gridPen = new Pen(Color.FromArgb(235, 235, 235), 1))
            using (Font labelFont = new Font("Constantia", 9F))
            {
                for (int i = 0; i <= lineCount; i++)
                {
                    decimal gridVal = (maxValue / lineCount) * i;
                    float y = chartArea.Bottom - ((float)gridVal / (float)maxValue) * chartArea.Height;

                    // Vẽ đường lưới (bỏ qua đường trục dưới cùng)
                    if (i > 0)
                    {
                        g.DrawLine(gridPen, chartArea.Left, y, chartArea.Right, y);
                    }

                    // Nhãn trục Y
                    string yLabel = RutGiaTien(gridVal);
                    SizeF labelSize = g.MeasureString(yLabel, labelFont);
                    g.DrawString(yLabel, labelFont, Brushes.DimGray, chartArea.Left - labelSize.Width - 10, y - labelSize.Height / 2);
                }
            }

            // Vẽ các cột
            float totalColumnWidth = chartArea.Width / (float)rowCount;
            float barWidth = totalColumnWidth * 0.55f; // Chiếm 55% khoảng cách
            float startX = chartArea.Left + (totalColumnWidth - barWidth) / 2;

            using (Font labelFont = new Font("Constantia", 9F, FontStyle.Regular))
            using (Font valueFont = new Font("Constantia", 9F, FontStyle.Bold))
            {
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = _dtBaoCao.Rows[i];
                    decimal val = Convert.ToDecimal(row[colValue]);
                    string catLabel = $"Th.{row[colThang]}/{row[colNam]?.ToString()?.Substring(2)}";

                    float x = startX + i * totalColumnWidth;
                    float barHeight = ((float)val / (float)maxValue) * chartArea.Height;
                    float y = chartArea.Bottom - barHeight;

                    if (barHeight > 0)
                    {
                        RectangleF barRect = new RectangleF(x, y, barWidth, barHeight);

                        // Đổ màu Gradient cột từ SlateBlue sang LightBlue cực kỳ hiện đại
                        using (LinearGradientBrush brush = new LinearGradientBrush(barRect, Color.SlateBlue, Color.FromArgb(170, 170, 255), 90F))
                        {
                            g.FillRectangle(brush, barRect);
                        }

                        // Vẽ viền cột
                        using (Pen barBorder = new Pen(Color.FromArgb(100, 100, 200), 1))
                        {
                            g.DrawRectangle(barBorder, x, y, barWidth, barHeight);
                        }

                        // Vẽ giá trị số liệu trên đỉnh cột
                        string valText = RutGiaTien(val);
                        SizeF valSize = g.MeasureString(valText, valueFont);
                        g.DrawString(valText, valueFont, Brushes.SlateBlue, x + (barWidth - valSize.Width) / 2, y - valSize.Height - 3);
                    }

                    // Vẽ nhãn X
                    SizeF catSize = g.MeasureString(catLabel, labelFont);
                    g.DrawString(catLabel, labelFont, Brushes.DimGray, x + (barWidth - catSize.Width) / 2, chartArea.Bottom + 10);
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CHI TIẾT: VẼ BIỂU ĐỒ TRÒN (CHO HÌNH THỨC THANH TOÁN)
        // ══════════════════════════════════════════════════════════════════
        private void VeBieuDoTron(Graphics g, Rectangle bounds, string tieuDe, string colCategory, string colValue)
        {
            // Vẽ tiêu đề
            using (Font titleFont = new Font("Constantia", 14F, System.Drawing.FontStyle.Bold))
            {
                SizeF titleSize = g.MeasureString(tieuDe, titleFont);
                g.DrawString(tieuDe, titleFont, Brushes.SlateBlue, (bounds.Width - titleSize.Width) / 2, 20);
            }

            int rowCount = _dtBaoCao!.Rows.Count;
            decimal totalValue = 0m;
            foreach (DataRow row in _dtBaoCao.Rows)
            {
                totalValue += Convert.ToDecimal(row[colValue]);
            }

            if (totalValue == 0)
            {
                using (Font italicFont = new Font("Constantia", 12F, FontStyle.Italic))
                {
                    string text = "Tổng doanh thu bằng 0. Không thể hiển thị biểu đồ.";
                    SizeF size = g.MeasureString(text, italicFont);
                    g.DrawString(text, italicFont, Brushes.Gray, (bounds.Width - size.Width) / 2, (bounds.Height - size.Height) / 2);
                }
                return;
            }

            // Vùng vẽ hình tròn (ở bên trái) và Legend (ở bên phải)
            int diameter = Math.Min(bounds.Width - 300, bounds.Height - 120);
            if (diameter < 150) diameter = 150;
            int chartX = 50;
            int chartY = (bounds.Height - diameter) / 2 + 10;
            Rectangle pieRect = new Rectangle(chartX, chartY, diameter, diameter);

            // Bảng màu đẹp
            Color[] colors = new Color[]
            {
                Color.SlateBlue,
                Color.FromArgb(255, 128, 128),
                Color.Teal,
                Color.FromArgb(255, 192, 128),
                Color.MediumOrchid
            };

            float startAngle = 0f;
            int legendX = pieRect.Right + 40;
            int legendY = chartY + 20;

            using (Font legendFont = new Font("Constantia", 11F, FontStyle.Regular))
            using (Font titleLegendFont = new Font("Constantia", 12F, FontStyle.Bold))
            {
                g.DrawString("Chú thích cơ cấu:", titleLegendFont, Brushes.SlateBlue, legendX, legendY);
                legendY += 30;

                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = _dtBaoCao.Rows[i];
                    decimal val = Convert.ToDecimal(row[colValue]);
                    string category = row[colCategory]?.ToString() ?? "Khác";

                    float sweepAngle = (float)(val / totalValue) * 360f;
                    double percent = (double)(val / totalValue) * 100.0;

                    Color sliceColor = colors[i % colors.Length];

                    // Vẽ rẻ quạt
                    using (SolidBrush brush = new SolidBrush(sliceColor))
                    {
                        g.FillPie(brush, pieRect, startAngle, sweepAngle);
                    }

                    // Viền chia rẻ quạt
                    g.DrawPie(Pens.White, pieRect, startAngle, sweepAngle);

                    // Vẽ chú giải Legend bên phải
                    using (SolidBrush legendBrush = new SolidBrush(sliceColor))
                    {
                        g.FillRectangle(legendBrush, legendX, legendY, 18, 18);
                        g.DrawRectangle(Pens.DimGray, legendX, legendY, 18, 18);
                    }

                    string legendText = $"{category}: {percent:0.0}% ({DinhDangTien(val)})";
                    g.DrawString(legendText, legendFont, Brushes.Black, legendX + 28, legendY - 1);
                    
                    legendY += 30;
                    startAngle += sweepAngle;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CHI TIẾT: VẼ BIỂU ĐỒ CỘT ĐỐI CHIẾU CHO TỒN KHO & ĐÃ BÁN
        // ══════════════════════════════════════════════════════════════════
        private void VeBieuDoCotTonKho(Graphics g, Rectangle bounds, string tieuDe)
        {
            using (Font titleFont = new Font("Constantia", 14F, FontStyle.Bold))
            {
                SizeF titleSize = g.MeasureString(tieuDe, titleFont);
                g.DrawString(tieuDe, titleFont, Brushes.SlateBlue, (bounds.Width - titleSize.Width) / 2, 20);
            }

            int paddingLeft = 80;
            int paddingRight = 140; // Rộng hơn để vẽ Legend
            int paddingTop = 70;
            int paddingBottom = 60;
            Rectangle chartArea = new Rectangle(paddingLeft, paddingTop, 
                bounds.Width - paddingLeft - paddingRight, bounds.Height - paddingTop - paddingBottom);

            using (Pen axisPen = new Pen(Color.DimGray, 2))
            {
                g.DrawLine(axisPen, chartArea.Left, chartArea.Bottom, chartArea.Right, chartArea.Bottom);
                g.DrawLine(axisPen, chartArea.Left, chartArea.Top, chartArea.Left, chartArea.Bottom);
            }

            // Tìm giá trị max (so sánh cả tồn kho và đã bán)
            int maxVal = 10;
            int rowCount = _dtBaoCao!.Rows.Count;
            foreach (DataRow row in _dtBaoCao.Rows)
            {
                int tk = Convert.ToInt32(row["TongTonKho"]);
                int db = Convert.ToInt32(row["TongDaBan"]);
                if (tk > maxVal) maxVal = tk;
                if (db > maxVal) maxVal = db;
            }

            // Làm tròn Max Y
            maxVal = (int)(Math.Ceiling(maxVal / 10.0) * 10);

            // Vẽ lưới Y
            int lineCount = 5;
            using (Pen gridPen = new Pen(Color.FromArgb(235, 235, 235), 1))
            using (Font labelFont = new Font("Constantia", 9F))
            {
                for (int i = 0; i <= lineCount; i++)
                {
                    int gridVal = (maxVal / lineCount) * i;
                    float y = chartArea.Bottom - ((float)gridVal / (float)maxVal) * chartArea.Height;

                    if (i > 0)
                        g.DrawLine(gridPen, chartArea.Left, y, chartArea.Right, y);

                    string yLabel = gridVal.ToString();
                    SizeF labelSize = g.MeasureString(yLabel, labelFont);
                    g.DrawString(yLabel, labelFont, Brushes.DimGray, chartArea.Left - labelSize.Width - 10, y - labelSize.Height / 2);
                }
            }

            // Vẽ cột ghép (Cột 1: Tồn Kho - SlateBlue, Cột 2: Đã Bán - Coral)
            float totalWidth = chartArea.Width / (float)rowCount;
            float groupWidth = totalWidth * 0.70f;
            float singleBarWidth = groupWidth * 0.45f;
            float startX = chartArea.Left + (totalWidth - groupWidth) / 2;

            Color colorTonKho = Color.SlateBlue;
            Color colorDaBan = Color.FromArgb(255, 128, 128);

            using (Font labelFont = new Font("Constantia", 9F, FontStyle.Regular))
            using (Font valueFont = new Font("Constantia", 8.5F, FontStyle.Bold))
            {
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = _dtBaoCao.Rows[i];
                    string category = row["DanhMuc"]?.ToString() ?? "Khác";
                    int tk = Convert.ToInt32(row["TongTonKho"]);
                    int db = Convert.ToInt32(row["TongDaBan"]);

                    float groupX = startX + i * totalWidth;

                    // 1. Cột Tồn Kho
                    float barHeightTK = ((float)tk / (float)maxVal) * chartArea.Height;
                    float yTK = chartArea.Bottom - barHeightTK;
                    if (barHeightTK > 0)
                    {
                        RectangleF rectTK = new RectangleF(groupX, yTK, singleBarWidth, barHeightTK);
                        using (LinearGradientBrush brush = new LinearGradientBrush(rectTK, colorTonKho, Color.FromArgb(170, 170, 255), 90F))
                        {
                            g.FillRectangle(brush, rectTK);
                        }
                        g.DrawRectangle(Pens.SlateBlue, groupX, yTK, singleBarWidth, barHeightTK);

                        string valTK = tk.ToString();
                        SizeF sizeVal = g.MeasureString(valTK, valueFont);
                        g.DrawString(valTK, valueFont, Brushes.SlateBlue, groupX + (singleBarWidth - sizeVal.Width) / 2, yTK - sizeVal.Height - 2);
                    }

                    // 2. Cột Đã Bán
                    float xDB = groupX + singleBarWidth + (groupWidth * 0.05f); // Cách nhau 5%
                    float barHeightDB = ((float)db / (float)maxVal) * chartArea.Height;
                    float yDB = chartArea.Bottom - barHeightDB;
                    if (barHeightDB > 0)
                    {
                        RectangleF rectDB = new RectangleF(xDB, yDB, singleBarWidth, barHeightDB);
                        using (LinearGradientBrush brush = new LinearGradientBrush(rectDB, colorDaBan, Color.FromArgb(255, 192, 192), 90F))
                        {
                            g.FillRectangle(brush, rectDB);
                        }
                        g.DrawRectangle(Pens.Tomato, xDB, yDB, singleBarWidth, barHeightDB);

                        string valDB = db.ToString();
                        SizeF sizeVal = g.MeasureString(valDB, valueFont);
                        g.DrawString(valDB, valueFont, Brushes.Tomato, xDB + (singleBarWidth - sizeVal.Width) / 2, yDB - sizeVal.Height - 2);
                    }

                    // Vẽ nhãn danh mục dưới cột
                    SizeF catSize = g.MeasureString(category, labelFont);
                    g.DrawString(category, labelFont, Brushes.DimGray, groupX + (groupWidth - catSize.Width) / 2, chartArea.Bottom + 10);
                }

                // Vẽ Legend ở phía bên phải
                int legendX = chartArea.Right + 20;
                int legendY = chartArea.Top + 30;

                g.FillRectangle(Brushes.SlateBlue, legendX, legendY, 15, 15);
                g.DrawRectangle(Pens.Black, legendX, legendY, 15, 15);
                g.DrawString("Tồn kho", labelFont, Brushes.Black, legendX + 22, legendY - 1);

                legendY += 25;
                using (SolidBrush brush = new SolidBrush(colorDaBan))
                {
                    g.FillRectangle(brush, legendX, legendY, 15, 15);
                }
                g.DrawRectangle(Pens.Black, legendX, legendY, 15, 15);
                g.DrawString("Đã bán", labelFont, Brushes.Black, legendX + 22, legendY - 1);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // CÁC HÀM TIỆN ÍCH / FORMAT DỮ LIỆU
        // ══════════════════════════════════════════════════════════════════
        private string DinhDangTien(decimal so)
        {
            return so.ToString("N0") + " ₫";
        }

        private string RutGiaTien(decimal so)
        {
            if (so >= 1000000000m)
                return (so / 1000000000m).ToString("0.##", CultureInfo.InvariantCulture) + " Tỷ";
            if (so >= 1000000m)
                return (so / 1000000m).ToString("0.#", CultureInfo.InvariantCulture) + " Tr";
            if (so >= 1000m)
                return (so / 1000m).ToString("0.#", CultureInfo.InvariantCulture) + " K";
            return so.ToString("N0") + " ₫";
        }
    }
}

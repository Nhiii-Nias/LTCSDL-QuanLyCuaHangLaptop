namespace GUI_HTQLCuaHangLaptop
{
    partial class FormQuanLyDonHang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            dataGridView = new DataGridView();
            groupBox1 = new GroupBox();
            comboBoxLoaiGridVewHienThi = new ComboBox();
            label10 = new Label();
            comboBoxChonLoaiSanPham = new ComboBox();
            panel3 = new Panel();
            btnXacNhan = new Button();
            labelKetQuaKhuyenMaiApDung = new Label();
            labelKetQuaTongTienPhaiTra = new Label();
            labelKetQuaSoTienGiam = new Label();
            labelKetQuaTongTienHang = new Label();
            label3 = new Label();
            label5 = new Label();
            label1 = new Label();
            label4 = new Label();
            panelChinhSua = new Panel();
            labelSoLuong = new Label();
            txtSoLuong = new TextBox();
            labelTrangThai = new Label();
            comboBoxTrangThai = new ComboBox();
            labelMaDonHang = new Label();
            txtMaDonHang = new TextBox();
            comboBoxPhuongThucThanhToan = new ComboBox();
            btnTim = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnThem = new Button();
            comboBoxMaHopDong = new ComboBox();
            labelMaHopDong = new Label();
            labelMaKhuyenMai = new Label();
            labelPhuongThucThanhToan = new Label();
            labelTenNhanVien = new Label();
            txtTenNhanVien = new TextBox();
            labelMaKH = new Label();
            txtMaKH = new TextBox();
            comboBoxMaKhuyenMai = new ComboBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            groupBox1.SuspendLayout();
            panel3.SuspendLayout();
            panelChinhSua.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dataGridView);
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(326, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1068, 769);
            panel1.TabIndex = 0;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new Point(0, 87);
            dataGridView.Margin = new Padding(4);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1068, 682);
            dataGridView.TabIndex = 24;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(192, 192, 255);
            groupBox1.Controls.Add(comboBoxLoaiGridVewHienThi);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(comboBoxChonLoaiSanPham);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Font = new Font("Constantia", 20F);
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1068, 87);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chọn loại sản phẩm";
            // 
            // comboBoxLoaiGridVewHienThi
            // 
            comboBoxLoaiGridVewHienThi.FormattingEnabled = true;
            comboBoxLoaiGridVewHienThi.Items.AddRange(new object[] { "Danh sách đơn hàng", "Danh sách sản phẩm", "Danh sách chi tiết đơn hàng" });
            comboBoxLoaiGridVewHienThi.Location = new Point(363, 39);
            comboBoxLoaiGridVewHienThi.Name = "comboBoxLoaiGridVewHienThi";
            comboBoxLoaiGridVewHienThi.Size = new Size(270, 41);
            comboBoxLoaiGridVewHienThi.TabIndex = 3;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(354, 0);
            label10.Name = "label10";
            label10.Size = new Size(305, 33);
            label10.TabIndex = 2;
            label10.Text = "Chọn danh sách hiển thị";
            // 
            // comboBoxChonLoaiSanPham
            // 
            comboBoxChonLoaiSanPham.FormattingEnabled = true;
            comboBoxChonLoaiSanPham.Items.AddRange(new object[] { "Laptop", "Chuột", "Bàn phím" });
            comboBoxChonLoaiSanPham.Location = new Point(6, 40);
            comboBoxChonLoaiSanPham.Name = "comboBoxChonLoaiSanPham";
            comboBoxChonLoaiSanPham.Size = new Size(270, 41);
            comboBoxChonLoaiSanPham.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(btnXacNhan);
            panel3.Controls.Add(labelKetQuaKhuyenMaiApDung);
            panel3.Controls.Add(labelKetQuaTongTienPhaiTra);
            panel3.Controls.Add(labelKetQuaSoTienGiam);
            panel3.Controls.Add(labelKetQuaTongTienHang);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(label4);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(326, 531);
            panel3.Name = "panel3";
            panel3.Size = new Size(1068, 238);
            panel3.TabIndex = 2;
            // 
            // btnXacNhan
            // 
            btnXacNhan.BackColor = Color.FromArgb(192, 192, 255);
            btnXacNhan.Font = new Font("Constantia", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXacNhan.Location = new Point(6, 181);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(161, 45);
            btnXacNhan.TabIndex = 24;
            btnXacNhan.Text = "Xác nhận";
            btnXacNhan.UseVisualStyleBackColor = false;
            // 
            // labelKetQuaKhuyenMaiApDung
            // 
            labelKetQuaKhuyenMaiApDung.AutoSize = true;
            labelKetQuaKhuyenMaiApDung.Font = new Font("Constantia", 20F);
            labelKetQuaKhuyenMaiApDung.Location = new Point(640, 52);
            labelKetQuaKhuyenMaiApDung.Name = "labelKetQuaKhuyenMaiApDung";
            labelKetQuaKhuyenMaiApDung.Size = new Size(103, 33);
            labelKetQuaKhuyenMaiApDung.TabIndex = 32;
            labelKetQuaKhuyenMaiApDung.Text = "Kết quả";
            labelKetQuaKhuyenMaiApDung.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelKetQuaTongTienPhaiTra
            // 
            labelKetQuaTongTienPhaiTra.AutoSize = true;
            labelKetQuaTongTienPhaiTra.Font = new Font("Constantia", 20F);
            labelKetQuaTongTienPhaiTra.Location = new Point(640, 128);
            labelKetQuaTongTienPhaiTra.Name = "labelKetQuaTongTienPhaiTra";
            labelKetQuaTongTienPhaiTra.Size = new Size(103, 33);
            labelKetQuaTongTienPhaiTra.TabIndex = 31;
            labelKetQuaTongTienPhaiTra.Text = "Kết quả";
            labelKetQuaTongTienPhaiTra.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelKetQuaSoTienGiam
            // 
            labelKetQuaSoTienGiam.AutoSize = true;
            labelKetQuaSoTienGiam.Font = new Font("Constantia", 20F);
            labelKetQuaSoTienGiam.Location = new Point(640, 90);
            labelKetQuaSoTienGiam.Name = "labelKetQuaSoTienGiam";
            labelKetQuaSoTienGiam.Size = new Size(103, 33);
            labelKetQuaSoTienGiam.TabIndex = 30;
            labelKetQuaSoTienGiam.Text = "Kết quả";
            labelKetQuaSoTienGiam.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelKetQuaTongTienHang
            // 
            labelKetQuaTongTienHang.AutoSize = true;
            labelKetQuaTongTienHang.Font = new Font("Constantia", 20F);
            labelKetQuaTongTienHang.Location = new Point(640, 14);
            labelKetQuaTongTienHang.Name = "labelKetQuaTongTienHang";
            labelKetQuaTongTienHang.Size = new Size(103, 33);
            labelKetQuaTongTienHang.TabIndex = 26;
            labelKetQuaTongTienHang.Text = "Kết quả";
            labelKetQuaTongTienHang.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Constantia", 20F);
            label3.Location = new Point(3, 128);
            label3.Name = "label3";
            label3.Size = new Size(328, 33);
            label3.TabIndex = 27;
            label3.Text = "Số tiền thanh toán thực tế";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Constantia", 20F);
            label5.Location = new Point(7, 90);
            label5.Name = "label5";
            label5.Size = new Size(161, 33);
            label5.TabIndex = 29;
            label5.Text = "Số tiền giảm";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Constantia", 20F);
            label1.Location = new Point(7, 14);
            label1.Name = "label1";
            label1.Size = new Size(195, 33);
            label1.TabIndex = 25;
            label1.Text = "Tổng tiền hàng";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Constantia", 20F);
            label4.Location = new Point(7, 52);
            label4.Name = "label4";
            label4.Size = new Size(269, 33);
            label4.TabIndex = 28;
            label4.Text = "Khuyến mãi áp dụng: ";
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(comboBoxMaKhuyenMai);
            panelChinhSua.Controls.Add(labelSoLuong);
            panelChinhSua.Controls.Add(txtSoLuong);
            panelChinhSua.Controls.Add(labelTrangThai);
            panelChinhSua.Controls.Add(comboBoxTrangThai);
            panelChinhSua.Controls.Add(labelMaDonHang);
            panelChinhSua.Controls.Add(txtMaDonHang);
            panelChinhSua.Controls.Add(comboBoxPhuongThucThanhToan);
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnSua);
            panelChinhSua.Controls.Add(btnXoa);
            panelChinhSua.Controls.Add(btnThem);
            panelChinhSua.Controls.Add(comboBoxMaHopDong);
            panelChinhSua.Controls.Add(labelMaHopDong);
            panelChinhSua.Controls.Add(labelMaKhuyenMai);
            panelChinhSua.Controls.Add(labelPhuongThucThanhToan);
            panelChinhSua.Controls.Add(labelTenNhanVien);
            panelChinhSua.Controls.Add(txtTenNhanVien);
            panelChinhSua.Controls.Add(labelMaKH);
            panelChinhSua.Controls.Add(txtMaKH);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(326, 769);
            panelChinhSua.TabIndex = 3;
            // 
            // labelSoLuong
            // 
            labelSoLuong.AutoSize = true;
            labelSoLuong.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelSoLuong.ForeColor = Color.White;
            labelSoLuong.Location = new Point(15, 586);
            labelSoLuong.Margin = new Padding(4, 0, 4, 0);
            labelSoLuong.Name = "labelSoLuong";
            labelSoLuong.Size = new Size(99, 24);
            labelSoLuong.TabIndex = 26;
            labelSoLuong.Text = "Số lượng";
            // 
            // txtSoLuong
            // 
            txtSoLuong.Font = new Font("Constantia", 15F);
            txtSoLuong.Location = new Point(15, 621);
            txtSoLuong.Margin = new Padding(4);
            txtSoLuong.Name = "txtSoLuong";
            txtSoLuong.Size = new Size(271, 32);
            txtSoLuong.TabIndex = 27;
            // 
            // labelTrangThai
            // 
            labelTrangThai.AutoSize = true;
            labelTrangThai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTrangThai.ForeColor = Color.White;
            labelTrangThai.Location = new Point(15, 510);
            labelTrangThai.Margin = new Padding(4, 0, 4, 0);
            labelTrangThai.Name = "labelTrangThai";
            labelTrangThai.Size = new Size(109, 24);
            labelTrangThai.TabIndex = 25;
            labelTrangThai.Text = "Trạng thái";
            // 
            // comboBoxTrangThai
            // 
            comboBoxTrangThai.Font = new Font("Constantia", 15F);
            comboBoxTrangThai.FormattingEnabled = true;
            comboBoxTrangThai.Items.AddRange(new object[] { "Khách lẻ", "Khách sỉ" });
            comboBoxTrangThai.Location = new Point(15, 545);
            comboBoxTrangThai.Margin = new Padding(4);
            comboBoxTrangThai.Name = "comboBoxTrangThai";
            comboBoxTrangThai.Size = new Size(271, 32);
            comboBoxTrangThai.TabIndex = 24;
            // 
            // labelMaDonHang
            // 
            labelMaDonHang.AutoSize = true;
            labelMaDonHang.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaDonHang.ForeColor = Color.White;
            labelMaDonHang.Location = new Point(15, 435);
            labelMaDonHang.Margin = new Padding(4, 0, 4, 0);
            labelMaDonHang.Name = "labelMaDonHang";
            labelMaDonHang.Size = new Size(138, 24);
            labelMaDonHang.TabIndex = 22;
            labelMaDonHang.Text = "Mã đơn hàng";
            // 
            // txtMaDonHang
            // 
            txtMaDonHang.Font = new Font("Constantia", 15F);
            txtMaDonHang.Location = new Point(15, 470);
            txtMaDonHang.Margin = new Padding(4);
            txtMaDonHang.Name = "txtMaDonHang";
            txtMaDonHang.Size = new Size(271, 32);
            txtMaDonHang.TabIndex = 23;
            // 
            // comboBoxPhuongThucThanhToan
            // 
            comboBoxPhuongThucThanhToan.Font = new Font("Constantia", 15F);
            comboBoxPhuongThucThanhToan.FormattingEnabled = true;
            comboBoxPhuongThucThanhToan.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ" });
            comboBoxPhuongThucThanhToan.Location = new Point(15, 218);
            comboBoxPhuongThucThanhToan.Margin = new Padding(4);
            comboBoxPhuongThucThanhToan.Name = "comboBoxPhuongThucThanhToan";
            comboBoxPhuongThucThanhToan.Size = new Size(271, 32);
            comboBoxPhuongThucThanhToan.TabIndex = 21;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(15, 663);
            btnTim.Margin = new Padding(4);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(118, 37);
            btnTim.TabIndex = 20;
            btnTim.Text = "Tìm";
            btnTim.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnSua.Location = new Point(168, 719);
            btnSua.Margin = new Padding(4);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(118, 37);
            btnSua.TabIndex = 18;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnXoa.Location = new Point(13, 719);
            btnXoa.Margin = new Padding(4);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(118, 37);
            btnXoa.TabIndex = 17;
            btnXoa.Text = "Xoá";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            btnThem.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnThem.Location = new Point(168, 663);
            btnThem.Margin = new Padding(4);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(118, 37);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // comboBoxMaHopDong
            // 
            comboBoxMaHopDong.Font = new Font("Constantia", 15F);
            comboBoxMaHopDong.FormattingEnabled = true;
            comboBoxMaHopDong.Items.AddRange(new object[] { "Khách lẻ", "Khách sỉ" });
            comboBoxMaHopDong.Location = new Point(15, 388);
            comboBoxMaHopDong.Margin = new Padding(4);
            comboBoxMaHopDong.Name = "comboBoxMaHopDong";
            comboBoxMaHopDong.Size = new Size(271, 32);
            comboBoxMaHopDong.TabIndex = 5;
            // 
            // labelMaHopDong
            // 
            labelMaHopDong.AutoSize = true;
            labelMaHopDong.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaHopDong.ForeColor = Color.White;
            labelMaHopDong.Location = new Point(15, 352);
            labelMaHopDong.Margin = new Padding(4, 0, 4, 0);
            labelMaHopDong.Name = "labelMaHopDong";
            labelMaHopDong.Size = new Size(138, 24);
            labelMaHopDong.TabIndex = 12;
            labelMaHopDong.Text = "Mã hợp đồng";
            // 
            // labelMaKhuyenMai
            // 
            labelMaKhuyenMai.AutoSize = true;
            labelMaKhuyenMai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaKhuyenMai.ForeColor = Color.White;
            labelMaKhuyenMai.Location = new Point(15, 271);
            labelMaKhuyenMai.Margin = new Padding(4, 0, 4, 0);
            labelMaKhuyenMai.Name = "labelMaKhuyenMai";
            labelMaKhuyenMai.Size = new Size(159, 24);
            labelMaKhuyenMai.TabIndex = 10;
            labelMaKhuyenMai.Text = "Mã khuyến mãi";
            // 
            // labelPhuongThucThanhToan
            // 
            labelPhuongThucThanhToan.AutoSize = true;
            labelPhuongThucThanhToan.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelPhuongThucThanhToan.ForeColor = Color.White;
            labelPhuongThucThanhToan.Location = new Point(15, 190);
            labelPhuongThucThanhToan.Margin = new Padding(4, 0, 4, 0);
            labelPhuongThucThanhToan.Name = "labelPhuongThucThanhToan";
            labelPhuongThucThanhToan.Size = new Size(253, 24);
            labelPhuongThucThanhToan.TabIndex = 8;
            labelPhuongThucThanhToan.Text = "Phương thức thanh toán";
            // 
            // labelTenNhanVien
            // 
            labelTenNhanVien.AutoSize = true;
            labelTenNhanVien.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenNhanVien.ForeColor = Color.White;
            labelTenNhanVien.Location = new Point(15, 109);
            labelTenNhanVien.Margin = new Padding(4, 0, 4, 0);
            labelTenNhanVien.Name = "labelTenNhanVien";
            labelTenNhanVien.Size = new Size(145, 24);
            labelTenNhanVien.TabIndex = 6;
            labelTenNhanVien.Text = "Tên nhân viên";
            // 
            // txtTenNhanVien
            // 
            txtTenNhanVien.Font = new Font("Constantia", 15F);
            txtTenNhanVien.Location = new Point(15, 144);
            txtTenNhanVien.Margin = new Padding(4);
            txtTenNhanVien.Name = "txtTenNhanVien";
            txtTenNhanVien.Size = new Size(271, 32);
            txtTenNhanVien.TabIndex = 7;
            // 
            // labelMaKH
            // 
            labelMaKH.AutoSize = true;
            labelMaKH.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaKH.ForeColor = Color.White;
            labelMaKH.Location = new Point(15, 28);
            labelMaKH.Margin = new Padding(4, 0, 4, 0);
            labelMaKH.Name = "labelMaKH";
            labelMaKH.Size = new Size(157, 24);
            labelMaKH.TabIndex = 5;
            labelMaKH.Text = "Mã khách hàng";
            // 
            // txtMaKH
            // 
            txtMaKH.Font = new Font("Constantia", 15F);
            txtMaKH.Location = new Point(15, 63);
            txtMaKH.Margin = new Padding(4);
            txtMaKH.Name = "txtMaKH";
            txtMaKH.Size = new Size(271, 32);
            txtMaKH.TabIndex = 5;
            // 
            // comboBoxMaKhuyenMai
            // 
            comboBoxMaKhuyenMai.Font = new Font("Constantia", 15F);
            comboBoxMaKhuyenMai.FormattingEnabled = true;
            comboBoxMaKhuyenMai.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ" });
            comboBoxMaKhuyenMai.Location = new Point(15, 299);
            comboBoxMaKhuyenMai.Margin = new Padding(4);
            comboBoxMaKhuyenMai.Name = "comboBoxMaKhuyenMai";
            comboBoxMaKhuyenMai.Size = new Size(271, 32);
            comboBoxMaKhuyenMai.TabIndex = 28;
            // 
            // FormQuanLyDonHang
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 769);
            ControlBox = false;
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormQuanLyDonHang";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panelChinhSua;
        private Button btnTim;
        private Button btnSua;
        private Button btnXoa;
        private Button btnThem;
        private ComboBox comboBoxMaHopDong;
        private Label labelMaHopDong;
        private Label labelMaKhuyenMai;
        private Label labelPhuongThucThanhToan;
        private Label labelTenNhanVien;
        private TextBox txtTenNhanVien;
        private Label labelMaKH;
        private TextBox txtMaKH;
        private ComboBox comboBoxChonLoaiSanPham;
        private ComboBox comboBoxPhuongThucThanhToan;
        private GroupBox groupBox1;
        private DataGridView dataGridView;
        private Label labelKetQuaTongTienHang;
        private Label label3;
        private Label label5;
        private Label label1;
        private Label label4;
        private Label labelKetQuaKhuyenMaiApDung;
        private Label labelKetQuaTongTienPhaiTra;
        private Label labelKetQuaSoTienGiam;
        private Label labelMaDonHang;
        private TextBox txtMaDonHang;
        private ComboBox comboBoxLoaiGridVewHienThi;
        private Label label10;
        private Button btnXacNhan;
        private Label labelTrangThai;
        private ComboBox comboBoxTrangThai;
        private Label labelSoLuong;
        private TextBox txtSoLuong;
        private ComboBox comboBoxMaKhuyenMai;
    }
}
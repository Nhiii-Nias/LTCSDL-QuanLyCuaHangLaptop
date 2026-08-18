namespace GUI_HTQLCuaHangLaptop
{
    partial class FormQuanLyNhapHang
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
            dataGridViewDSPhieuNhap = new DataGridView();
            groupBox1 = new GroupBox();
            label2 = new Label();
            textBox2 = new TextBox();
            label1 = new Label();
            textBox1 = new TextBox();
            comboBoxChonLoaiSanPham = new ComboBox();
            panelChinhSua = new Panel();
            dateTimePickerNgayBatDau = new DateTimePicker();
            btnTim = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnThem = new Button();
            comboBoxMaHopDong = new ComboBox();
            labelMaHopDong = new Label();
            labelMaKhuyenMai = new Label();
            txtMaKhuyenMai = new TextBox();
            labelPhuongThucThanhToan = new Label();
            labelTenNhanVien = new Label();
            txtTenNhanVien = new TextBox();
            labelMaKH = new Label();
            comboBoxMaNhaCungCap = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSPhieuNhap).BeginInit();
            groupBox1.SuspendLayout();
            panelChinhSua.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewDSPhieuNhap
            // 
            dataGridViewDSPhieuNhap.AllowUserToAddRows = false;
            dataGridViewDSPhieuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSPhieuNhap.BackgroundColor = Color.White;
            dataGridViewDSPhieuNhap.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSPhieuNhap.Dock = DockStyle.Fill;
            dataGridViewDSPhieuNhap.Location = new Point(326, 87);
            dataGridViewDSPhieuNhap.Margin = new Padding(4);
            dataGridViewDSPhieuNhap.MultiSelect = false;
            dataGridViewDSPhieuNhap.Name = "dataGridViewDSPhieuNhap";
            dataGridViewDSPhieuNhap.ReadOnly = true;
            dataGridViewDSPhieuNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSPhieuNhap.Size = new Size(1466, 858);
            dataGridViewDSPhieuNhap.TabIndex = 27;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(192, 192, 255);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(comboBoxChonLoaiSanPham);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Font = new Font("Constantia", 20F);
            groupBox1.Location = new Point(326, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1466, 87);
            groupBox1.TabIndex = 26;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chọn loại sản phẩm";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Constantia", 20F);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(598, -1);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(109, 33);
            label2.TabIndex = 26;
            label2.Text = "Đơn giá";
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Constantia", 20F);
            textBox2.Location = new Point(597, 39);
            textBox2.Margin = new Padding(4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(271, 40);
            textBox2.TabIndex = 27;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Constantia", 20F);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(303, -1);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(190, 33);
            label1.TabIndex = 25;
            label1.Text = "Số lượng nhập";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Constantia", 20F);
            textBox1.Location = new Point(301, 40);
            textBox1.Margin = new Padding(4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(271, 40);
            textBox1.TabIndex = 25;
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
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(comboBoxMaNhaCungCap);
            panelChinhSua.Controls.Add(dateTimePickerNgayBatDau);
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnSua);
            panelChinhSua.Controls.Add(btnXoa);
            panelChinhSua.Controls.Add(btnThem);
            panelChinhSua.Controls.Add(comboBoxMaHopDong);
            panelChinhSua.Controls.Add(labelMaHopDong);
            panelChinhSua.Controls.Add(labelMaKhuyenMai);
            panelChinhSua.Controls.Add(txtMaKhuyenMai);
            panelChinhSua.Controls.Add(labelPhuongThucThanhToan);
            panelChinhSua.Controls.Add(labelTenNhanVien);
            panelChinhSua.Controls.Add(txtTenNhanVien);
            panelChinhSua.Controls.Add(labelMaKH);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(326, 945);
            panelChinhSua.TabIndex = 25;
            // 
            // dateTimePickerNgayBatDau
            // 
            dateTimePickerNgayBatDau.CalendarFont = new Font("Constantia", 20F);
            dateTimePickerNgayBatDau.CustomFormat = "dd/MM/yyyy";
            dateTimePickerNgayBatDau.Font = new Font("Constantia", 16F);
            dateTimePickerNgayBatDau.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayBatDau.Location = new Point(17, 224);
            dateTimePickerNgayBatDau.Name = "dateTimePickerNgayBatDau";
            dateTimePickerNgayBatDau.Size = new Size(269, 34);
            dateTimePickerNgayBatDau.TabIndex = 24;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(17, 437);
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
            btnSua.Location = new Point(170, 493);
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
            btnXoa.Location = new Point(15, 493);
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
            btnThem.Location = new Point(170, 437);
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
            comboBoxMaHopDong.Items.AddRange(new object[] { "Chờ xác nhận", "Đã nhập", "Huỷ" });
            comboBoxMaHopDong.Location = new Point(17, 386);
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
            labelMaHopDong.Location = new Point(15, 350);
            labelMaHopDong.Margin = new Padding(4, 0, 4, 0);
            labelMaHopDong.Name = "labelMaHopDong";
            labelMaHopDong.Size = new Size(109, 24);
            labelMaHopDong.TabIndex = 12;
            labelMaHopDong.Text = "Trạng thái";
            // 
            // labelMaKhuyenMai
            // 
            labelMaKhuyenMai.AutoSize = true;
            labelMaKhuyenMai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaKhuyenMai.ForeColor = Color.White;
            labelMaKhuyenMai.Location = new Point(15, 270);
            labelMaKhuyenMai.Margin = new Padding(4, 0, 4, 0);
            labelMaKhuyenMai.Name = "labelMaKhuyenMai";
            labelMaKhuyenMai.Size = new Size(155, 24);
            labelMaKhuyenMai.TabIndex = 10;
            labelMaKhuyenMai.Text = "Tổng tiền nhập";
            // 
            // txtMaKhuyenMai
            // 
            txtMaKhuyenMai.Font = new Font("Constantia", 15F);
            txtMaKhuyenMai.Location = new Point(17, 306);
            txtMaKhuyenMai.Margin = new Padding(4);
            txtMaKhuyenMai.Name = "txtMaKhuyenMai";
            txtMaKhuyenMai.Size = new Size(271, 32);
            txtMaKhuyenMai.TabIndex = 11;
            // 
            // labelPhuongThucThanhToan
            // 
            labelPhuongThucThanhToan.AutoSize = true;
            labelPhuongThucThanhToan.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelPhuongThucThanhToan.ForeColor = Color.White;
            labelPhuongThucThanhToan.Location = new Point(15, 188);
            labelPhuongThucThanhToan.Margin = new Padding(4, 0, 4, 0);
            labelPhuongThucThanhToan.Name = "labelPhuongThucThanhToan";
            labelPhuongThucThanhToan.Size = new Size(111, 24);
            labelPhuongThucThanhToan.TabIndex = 8;
            labelPhuongThucThanhToan.Text = "Ngày nhập";
            // 
            // labelTenNhanVien
            // 
            labelTenNhanVien.AutoSize = true;
            labelTenNhanVien.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenNhanVien.ForeColor = Color.White;
            labelTenNhanVien.Location = new Point(15, 108);
            labelTenNhanVien.Margin = new Padding(4, 0, 4, 0);
            labelTenNhanVien.Name = "labelTenNhanVien";
            labelTenNhanVien.Size = new Size(145, 24);
            labelTenNhanVien.TabIndex = 6;
            labelTenNhanVien.Text = "Tên nhân viên";
            // 
            // txtTenNhanVien
            // 
            txtTenNhanVien.Font = new Font("Constantia", 15F);
            txtTenNhanVien.Location = new Point(17, 144);
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
            labelMaKH.Size = new Size(172, 24);
            labelMaKH.TabIndex = 5;
            labelMaKH.Text = "Mã nhà cung cấp";
            // 
            // comboBoxMaNhaCungCap
            // 
            comboBoxMaNhaCungCap.Font = new Font("Constantia", 15F);
            comboBoxMaNhaCungCap.FormattingEnabled = true;
            comboBoxMaNhaCungCap.Items.AddRange(new object[] { "Chờ xác nhận", "Đã nhập", "Huỷ" });
            comboBoxMaNhaCungCap.Location = new Point(17, 64);
            comboBoxMaNhaCungCap.Margin = new Padding(4);
            comboBoxMaNhaCungCap.Name = "comboBoxMaNhaCungCap";
            comboBoxMaNhaCungCap.Size = new Size(271, 32);
            comboBoxMaNhaCungCap.TabIndex = 25;
            // 
            // FormQuanLyNhapHang
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1792, 945);
            ControlBox = false;
            Controls.Add(dataGridViewDSPhieuNhap);
            Controls.Add(groupBox1);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormQuanLyNhapHang";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSPhieuNhap).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewDSPhieuNhap;
        private GroupBox groupBox1;
        private ComboBox comboBoxChonLoaiSanPham;
        private Panel panelChinhSua;
        private Button btnTim;
        private Button btnSua;
        private Button btnXoa;
        private Button btnThem;
        private ComboBox comboBoxMaHopDong;
        private Label labelMaHopDong;
        private Label labelMaKhuyenMai;
        private TextBox txtMaKhuyenMai;
        private Label labelPhuongThucThanhToan;
        private Label labelTenNhanVien;
        private TextBox txtTenNhanVien;
        private Label labelMaKH;
        private DateTimePicker dateTimePickerNgayBatDau;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private ComboBox comboBoxMaNhaCungCap;
    }
}
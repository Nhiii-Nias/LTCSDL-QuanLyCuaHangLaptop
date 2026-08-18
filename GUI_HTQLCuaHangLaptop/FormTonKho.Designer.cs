namespace GUI_HTQLCuaHangLaptop
{
    partial class FormTonKho
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
            panelChinhSua = new Panel();
            dateTimePickerNgaySanXuat = new DateTimePicker();
            dateTimePickerNgayNhapKho = new DateTimePicker();
            labelMaPhieuNhapHang = new Label();
            txtMaPhieuNhapHang = new TextBox();
            btnTim = new Button();
            btnSua = new Button();
            comboBoxTrangThai = new ComboBox();
            labelTrangThai = new Label();
            labelNgaySanXuat = new Label();
            labelNgayNhapKho = new Label();
            labelTenSanPham = new Label();
            txtTenSanPham = new TextBox();
            labelMaSerialSanPham = new Label();
            txtMaSerialSanPham = new TextBox();
            panel1 = new Panel();
            dataGridViewDSSanPham = new DataGridView();
            groupBox1 = new GroupBox();
            comboBoxChonNhaSanXuat = new ComboBox();
            label1 = new Label();
            labelKetQuaTongSLLoaiSanPham = new Label();
            comboBoxChonLoaiSanPham = new ComboBox();
            label6 = new Label();
            panelChinhSua.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSSanPham).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(dateTimePickerNgaySanXuat);
            panelChinhSua.Controls.Add(dateTimePickerNgayNhapKho);
            panelChinhSua.Controls.Add(labelMaPhieuNhapHang);
            panelChinhSua.Controls.Add(txtMaPhieuNhapHang);
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnSua);
            panelChinhSua.Controls.Add(comboBoxTrangThai);
            panelChinhSua.Controls.Add(labelTrangThai);
            panelChinhSua.Controls.Add(labelNgaySanXuat);
            panelChinhSua.Controls.Add(labelNgayNhapKho);
            panelChinhSua.Controls.Add(labelTenSanPham);
            panelChinhSua.Controls.Add(txtTenSanPham);
            panelChinhSua.Controls.Add(labelMaSerialSanPham);
            panelChinhSua.Controls.Add(txtMaSerialSanPham);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(326, 746);
            panelChinhSua.TabIndex = 26;
            // 
            // dateTimePickerNgaySanXuat
            // 
            dateTimePickerNgaySanXuat.CalendarFont = new Font("Constantia", 20F);
            dateTimePickerNgaySanXuat.CustomFormat = "dd/MM/yyyy";
            dateTimePickerNgaySanXuat.Font = new Font("Constantia", 16F);
            dateTimePickerNgaySanXuat.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgaySanXuat.Location = new Point(17, 306);
            dateTimePickerNgaySanXuat.Name = "dateTimePickerNgaySanXuat";
            dateTimePickerNgaySanXuat.ShowCheckBox = true;
            dateTimePickerNgaySanXuat.Size = new Size(269, 34);
            dateTimePickerNgaySanXuat.TabIndex = 25;
            // 
            // dateTimePickerNgayNhapKho
            // 
            dateTimePickerNgayNhapKho.CalendarFont = new Font("Constantia", 20F);
            dateTimePickerNgayNhapKho.CustomFormat = "dd/MM/yyyy";
            dateTimePickerNgayNhapKho.Font = new Font("Constantia", 16F);
            dateTimePickerNgayNhapKho.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayNhapKho.Location = new Point(17, 224);
            dateTimePickerNgayNhapKho.Name = "dateTimePickerNgayNhapKho";
            dateTimePickerNgayNhapKho.ShowCheckBox = true;
            dateTimePickerNgayNhapKho.Size = new Size(269, 34);
            dateTimePickerNgayNhapKho.TabIndex = 24;
            // 
            // labelMaPhieuNhapHang
            // 
            labelMaPhieuNhapHang.AutoSize = true;
            labelMaPhieuNhapHang.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaPhieuNhapHang.ForeColor = Color.White;
            labelMaPhieuNhapHang.Location = new Point(15, 432);
            labelMaPhieuNhapHang.Margin = new Padding(4, 0, 4, 0);
            labelMaPhieuNhapHang.Name = "labelMaPhieuNhapHang";
            labelMaPhieuNhapHang.Size = new Size(208, 24);
            labelMaPhieuNhapHang.TabIndex = 22;
            labelMaPhieuNhapHang.Text = "Mã phiếu nhập hàng";
            // 
            // txtMaPhieuNhapHang
            // 
            txtMaPhieuNhapHang.Font = new Font("Constantia", 15F);
            txtMaPhieuNhapHang.Location = new Point(15, 468);
            txtMaPhieuNhapHang.Margin = new Padding(4);
            txtMaPhieuNhapHang.Name = "txtMaPhieuNhapHang";
            txtMaPhieuNhapHang.Size = new Size(271, 32);
            txtMaPhieuNhapHang.TabIndex = 23;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(15, 510);
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
            btnSua.Location = new Point(168, 510);
            btnSua.Margin = new Padding(4);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(118, 37);
            btnSua.TabIndex = 18;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // comboBoxTrangThai
            // 
            comboBoxTrangThai.Font = new Font("Constantia", 15F);
            comboBoxTrangThai.FormattingEnabled = true;
            comboBoxTrangThai.Items.AddRange(new object[] { "Trong kho", "Đã bán", "Bảo hành", "Lỗi", "Đổi trả" });
            comboBoxTrangThai.Location = new Point(15, 388);
            comboBoxTrangThai.Margin = new Padding(4);
            comboBoxTrangThai.Name = "comboBoxTrangThai";
            comboBoxTrangThai.Size = new Size(271, 32);
            comboBoxTrangThai.TabIndex = 5;
            // 
            // labelTrangThai
            // 
            labelTrangThai.AutoSize = true;
            labelTrangThai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTrangThai.ForeColor = Color.White;
            labelTrangThai.Location = new Point(15, 352);
            labelTrangThai.Margin = new Padding(4, 0, 4, 0);
            labelTrangThai.Name = "labelTrangThai";
            labelTrangThai.Size = new Size(109, 24);
            labelTrangThai.TabIndex = 12;
            labelTrangThai.Text = "Trạng thái";
            // 
            // labelNgaySanXuat
            // 
            labelNgaySanXuat.AutoSize = true;
            labelNgaySanXuat.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelNgaySanXuat.ForeColor = Color.White;
            labelNgaySanXuat.Location = new Point(15, 270);
            labelNgaySanXuat.Margin = new Padding(4, 0, 4, 0);
            labelNgaySanXuat.Name = "labelNgaySanXuat";
            labelNgaySanXuat.Size = new Size(143, 24);
            labelNgaySanXuat.TabIndex = 10;
            labelNgaySanXuat.Text = "Ngày sản xuất";
            // 
            // labelNgayNhapKho
            // 
            labelNgayNhapKho.AutoSize = true;
            labelNgayNhapKho.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelNgayNhapKho.ForeColor = Color.White;
            labelNgayNhapKho.Location = new Point(15, 188);
            labelNgayNhapKho.Margin = new Padding(4, 0, 4, 0);
            labelNgayNhapKho.Name = "labelNgayNhapKho";
            labelNgayNhapKho.Size = new Size(153, 24);
            labelNgayNhapKho.TabIndex = 8;
            labelNgayNhapKho.Text = "Ngày nhập kho";
            // 
            // labelTenSanPham
            // 
            labelTenSanPham.AutoSize = true;
            labelTenSanPham.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenSanPham.ForeColor = Color.White;
            labelTenSanPham.Location = new Point(15, 108);
            labelTenSanPham.Margin = new Padding(4, 0, 4, 0);
            labelTenSanPham.Name = "labelTenSanPham";
            labelTenSanPham.Size = new Size(143, 24);
            labelTenSanPham.TabIndex = 6;
            labelTenSanPham.Text = "Tên sản phẩm";
            // 
            // txtTenSanPham
            // 
            txtTenSanPham.Font = new Font("Constantia", 15F);
            txtTenSanPham.Location = new Point(15, 144);
            txtTenSanPham.Margin = new Padding(4);
            txtTenSanPham.Name = "txtTenSanPham";
            txtTenSanPham.Size = new Size(271, 32);
            txtTenSanPham.TabIndex = 7;
            // 
            // labelMaSerialSanPham
            // 
            labelMaSerialSanPham.AutoSize = true;
            labelMaSerialSanPham.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaSerialSanPham.ForeColor = Color.White;
            labelMaSerialSanPham.Location = new Point(15, 28);
            labelMaSerialSanPham.Margin = new Padding(4, 0, 4, 0);
            labelMaSerialSanPham.Name = "labelMaSerialSanPham";
            labelMaSerialSanPham.Size = new Size(199, 24);
            labelMaSerialSanPham.TabIndex = 5;
            labelMaSerialSanPham.Text = "Mã Serial sản phẩm";
            // 
            // txtMaSerialSanPham
            // 
            txtMaSerialSanPham.Font = new Font("Constantia", 15F);
            txtMaSerialSanPham.Location = new Point(15, 64);
            txtMaSerialSanPham.Margin = new Padding(4);
            txtMaSerialSanPham.Name = "txtMaSerialSanPham";
            txtMaSerialSanPham.Size = new Size(271, 32);
            txtMaSerialSanPham.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.Controls.Add(dataGridViewDSSanPham);
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(326, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1068, 746);
            panel1.TabIndex = 27;
            // 
            // dataGridViewDSSanPham
            // 
            dataGridViewDSSanPham.AllowUserToAddRows = false;
            dataGridViewDSSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSSanPham.BackgroundColor = Color.White;
            dataGridViewDSSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSSanPham.Dock = DockStyle.Fill;
            dataGridViewDSSanPham.Location = new Point(0, 87);
            dataGridViewDSSanPham.Margin = new Padding(4);
            dataGridViewDSSanPham.MultiSelect = false;
            dataGridViewDSSanPham.Name = "dataGridViewDSSanPham";
            dataGridViewDSSanPham.ReadOnly = true;
            dataGridViewDSSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSSanPham.Size = new Size(1068, 659);
            dataGridViewDSSanPham.TabIndex = 26;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(192, 192, 255);
            groupBox1.Controls.Add(comboBoxChonNhaSanXuat);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(labelKetQuaTongSLLoaiSanPham);
            groupBox1.Controls.Add(comboBoxChonLoaiSanPham);
            groupBox1.Controls.Add(label6);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Font = new Font("Constantia", 20F);
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1068, 87);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chọn loại sản phẩm";
            // 
            // comboBoxChonNhaSanXuat
            // 
            comboBoxChonNhaSanXuat.FormattingEnabled = true;
            comboBoxChonNhaSanXuat.Location = new Point(319, 40);
            comboBoxChonNhaSanXuat.Name = "comboBoxChonNhaSanXuat";
            comboBoxChonNhaSanXuat.Size = new Size(270, 41);
            comboBoxChonNhaSanXuat.TabIndex = 36;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Constantia", 20F);
            label1.Location = new Point(319, 0);
            label1.Name = "label1";
            label1.Size = new Size(234, 33);
            label1.TabIndex = 35;
            label1.Text = "Chọn nhà sản xuất";
            // 
            // labelKetQuaTongSLLoaiSanPham
            // 
            labelKetQuaTongSLLoaiSanPham.AutoSize = true;
            labelKetQuaTongSLLoaiSanPham.BackColor = Color.White;
            labelKetQuaTongSLLoaiSanPham.Font = new Font("Constantia", 20F);
            labelKetQuaTongSLLoaiSanPham.Location = new Point(835, 40);
            labelKetQuaTongSLLoaiSanPham.Name = "labelKetQuaTongSLLoaiSanPham";
            labelKetQuaTongSLLoaiSanPham.Size = new Size(103, 33);
            labelKetQuaTongSLLoaiSanPham.TabIndex = 34;
            labelKetQuaTongSLLoaiSanPham.Text = "Kết quả";
            labelKetQuaTongSLLoaiSanPham.TextAlign = ContentAlignment.MiddleRight;
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
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Constantia", 20F);
            label6.Location = new Point(621, 40);
            label6.Name = "label6";
            label6.Size = new Size(200, 33);
            label6.TabIndex = 33;
            label6.Text = "Tổng số lượng: ";
            // 
            // FormTonKho
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 746);
            ControlBox = false;
            Controls.Add(panel1);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormTonKho";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Minimized;
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSSanPham).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Label labelMaPhieuNhapHang;
        private TextBox txtMaPhieuNhapHang;
        private Button btnTim;
        private Button btnSua;
        private ComboBox comboBoxTrangThai;
        private Label labelTrangThai;
        private Label labelNgaySanXuat;
        private Label labelNgayNhapKho;
        private Label labelTenSanPham;
        private TextBox txtTenSanPham;
        private Label labelMaSerialSanPham;
        private TextBox txtMaSerialSanPham;
        private Panel panel1;
        private DataGridView dataGridViewDSSanPham;
        private GroupBox groupBox1;
        private ComboBox comboBoxChonLoaiSanPham;
        private Label labelKetQuaTongSLLoaiSanPham;
        private Label label6;
        private ComboBox comboBoxChonNhaSanXuat;
        private Label label1;
        private DateTimePicker dateTimePickerNgayNhapKho;
        private DateTimePicker dateTimePickerNgaySanXuat;
    }
}
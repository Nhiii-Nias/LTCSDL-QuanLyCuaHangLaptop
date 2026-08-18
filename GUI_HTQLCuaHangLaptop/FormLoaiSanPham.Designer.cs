namespace GUI_HTQLCuaHangLaptop
{
    partial class FormLoaiSanPham
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
            btnTim = new Button();
            btnSuaKhachHang = new Button();
            btnXoaKhachHang = new Button();
            btnThemKhachHang = new Button();
            labelHangSanXuat = new Label();
            labelTenLoaiSanPham = new Label();
            txtTenLoaiSanPham = new TextBox();
            labelMaLoaiSanPham = new Label();
            txtMaLoaiSanPham = new TextBox();
            dataGridViewDSLoaiSanPham = new DataGridView();
            labelDanhMucSanPham = new Label();
            labelSoThangBaoHanh = new Label();
            textBox2 = new TextBox();
            labelDonGiaBanGoc = new Label();
            textBox3 = new TextBox();
            comboBoxHangSanXuat = new ComboBox();
            comboBoxDanhMucSanPham = new ComboBox();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSLoaiSanPham).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(comboBoxDanhMucSanPham);
            panelChinhSua.Controls.Add(comboBoxHangSanXuat);
            panelChinhSua.Controls.Add(labelDonGiaBanGoc);
            panelChinhSua.Controls.Add(textBox3);
            panelChinhSua.Controls.Add(labelSoThangBaoHanh);
            panelChinhSua.Controls.Add(textBox2);
            panelChinhSua.Controls.Add(labelDanhMucSanPham);
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnSuaKhachHang);
            panelChinhSua.Controls.Add(btnXoaKhachHang);
            panelChinhSua.Controls.Add(btnThemKhachHang);
            panelChinhSua.Controls.Add(labelHangSanXuat);
            panelChinhSua.Controls.Add(labelTenLoaiSanPham);
            panelChinhSua.Controls.Add(txtTenLoaiSanPham);
            panelChinhSua.Controls.Add(labelMaLoaiSanPham);
            panelChinhSua.Controls.Add(txtMaLoaiSanPham);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(314, 746);
            panelChinhSua.TabIndex = 8;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(159, 584);
            btnTim.Margin = new Padding(4);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(118, 37);
            btnTim.TabIndex = 20;
            btnTim.Text = "Tìm";
            btnTim.UseVisualStyleBackColor = true;
            // 
            // btnSuaKhachHang
            // 
            btnSuaKhachHang.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnSuaKhachHang.Location = new Point(18, 583);
            btnSuaKhachHang.Margin = new Padding(4);
            btnSuaKhachHang.Name = "btnSuaKhachHang";
            btnSuaKhachHang.Size = new Size(118, 37);
            btnSuaKhachHang.TabIndex = 18;
            btnSuaKhachHang.Text = "Sửa";
            btnSuaKhachHang.UseVisualStyleBackColor = true;
            // 
            // btnXoaKhachHang
            // 
            btnXoaKhachHang.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnXoaKhachHang.Location = new Point(159, 539);
            btnXoaKhachHang.Margin = new Padding(4);
            btnXoaKhachHang.Name = "btnXoaKhachHang";
            btnXoaKhachHang.Size = new Size(118, 37);
            btnXoaKhachHang.TabIndex = 17;
            btnXoaKhachHang.Text = "Xoá";
            btnXoaKhachHang.UseVisualStyleBackColor = true;
            // 
            // btnThemKhachHang
            // 
            btnThemKhachHang.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnThemKhachHang.Location = new Point(18, 539);
            btnThemKhachHang.Margin = new Padding(4);
            btnThemKhachHang.Name = "btnThemKhachHang";
            btnThemKhachHang.Size = new Size(118, 37);
            btnThemKhachHang.TabIndex = 6;
            btnThemKhachHang.Text = "Thêm";
            btnThemKhachHang.UseVisualStyleBackColor = true;
            // 
            // labelHangSanXuat
            // 
            labelHangSanXuat.AutoSize = true;
            labelHangSanXuat.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelHangSanXuat.ForeColor = Color.White;
            labelHangSanXuat.Location = new Point(18, 196);
            labelHangSanXuat.Margin = new Padding(4, 0, 4, 0);
            labelHangSanXuat.Name = "labelHangSanXuat";
            labelHangSanXuat.Size = new Size(148, 24);
            labelHangSanXuat.TabIndex = 10;
            labelHangSanXuat.Text = "Hãng sản xuất";
            // 
            // labelTenLoaiSanPham
            // 
            labelTenLoaiSanPham.AutoSize = true;
            labelTenLoaiSanPham.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenLoaiSanPham.ForeColor = Color.White;
            labelTenLoaiSanPham.Location = new Point(18, 112);
            labelTenLoaiSanPham.Margin = new Padding(4, 0, 4, 0);
            labelTenLoaiSanPham.Name = "labelTenLoaiSanPham";
            labelTenLoaiSanPham.Size = new Size(185, 24);
            labelTenLoaiSanPham.TabIndex = 8;
            labelTenLoaiSanPham.Text = "Tên loại sản phẩm";
            // 
            // txtTenLoaiSanPham
            // 
            txtTenLoaiSanPham.Font = new Font("Constantia", 15F);
            txtTenLoaiSanPham.Location = new Point(18, 150);
            txtTenLoaiSanPham.Margin = new Padding(4);
            txtTenLoaiSanPham.Name = "txtTenLoaiSanPham";
            txtTenLoaiSanPham.Size = new Size(271, 32);
            txtTenLoaiSanPham.TabIndex = 9;
            // 
            // labelMaLoaiSanPham
            // 
            labelMaLoaiSanPham.AutoSize = true;
            labelMaLoaiSanPham.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaLoaiSanPham.ForeColor = Color.White;
            labelMaLoaiSanPham.Location = new Point(18, 28);
            labelMaLoaiSanPham.Margin = new Padding(4, 0, 4, 0);
            labelMaLoaiSanPham.Name = "labelMaLoaiSanPham";
            labelMaLoaiSanPham.Size = new Size(180, 24);
            labelMaLoaiSanPham.TabIndex = 5;
            labelMaLoaiSanPham.Text = "Mã loại sản phẩm";
            // 
            // txtMaLoaiSanPham
            // 
            txtMaLoaiSanPham.Font = new Font("Constantia", 15F);
            txtMaLoaiSanPham.Location = new Point(18, 66);
            txtMaLoaiSanPham.Margin = new Padding(4);
            txtMaLoaiSanPham.Name = "txtMaLoaiSanPham";
            txtMaLoaiSanPham.Size = new Size(271, 32);
            txtMaLoaiSanPham.TabIndex = 5;
            // 
            // dataGridViewDSLoaiSanPham
            // 
            dataGridViewDSLoaiSanPham.AllowUserToAddRows = false;
            dataGridViewDSLoaiSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSLoaiSanPham.BackgroundColor = Color.White;
            dataGridViewDSLoaiSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSLoaiSanPham.Dock = DockStyle.Fill;
            dataGridViewDSLoaiSanPham.Location = new Point(314, 0);
            dataGridViewDSLoaiSanPham.Margin = new Padding(4);
            dataGridViewDSLoaiSanPham.MultiSelect = false;
            dataGridViewDSLoaiSanPham.Name = "dataGridViewDSLoaiSanPham";
            dataGridViewDSLoaiSanPham.ReadOnly = true;
            dataGridViewDSLoaiSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSLoaiSanPham.Size = new Size(1080, 746);
            dataGridViewDSLoaiSanPham.TabIndex = 7;
            // 
            // labelDanhMucSanPham
            // 
            labelDanhMucSanPham.AutoSize = true;
            labelDanhMucSanPham.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelDanhMucSanPham.ForeColor = Color.White;
            labelDanhMucSanPham.Location = new Point(18, 280);
            labelDanhMucSanPham.Margin = new Padding(4, 0, 4, 0);
            labelDanhMucSanPham.Name = "labelDanhMucSanPham";
            labelDanhMucSanPham.Size = new Size(208, 24);
            labelDanhMucSanPham.TabIndex = 21;
            labelDanhMucSanPham.Text = "Danh mục sản phẩm";
            // 
            // labelSoThangBaoHanh
            // 
            labelSoThangBaoHanh.AutoSize = true;
            labelSoThangBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelSoThangBaoHanh.ForeColor = Color.White;
            labelSoThangBaoHanh.Location = new Point(18, 364);
            labelSoThangBaoHanh.Margin = new Padding(4, 0, 4, 0);
            labelSoThangBaoHanh.Name = "labelSoThangBaoHanh";
            labelSoThangBaoHanh.Size = new Size(189, 24);
            labelSoThangBaoHanh.TabIndex = 23;
            labelSoThangBaoHanh.Text = "Số tháng bảo hành";
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Constantia", 15F);
            textBox2.Location = new Point(18, 402);
            textBox2.Margin = new Padding(4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(271, 32);
            textBox2.TabIndex = 24;
            // 
            // labelDonGiaBanGoc
            // 
            labelDonGiaBanGoc.AutoSize = true;
            labelDonGiaBanGoc.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelDonGiaBanGoc.ForeColor = Color.White;
            labelDonGiaBanGoc.Location = new Point(18, 448);
            labelDonGiaBanGoc.Margin = new Padding(4, 0, 4, 0);
            labelDonGiaBanGoc.Name = "labelDonGiaBanGoc";
            labelDonGiaBanGoc.Size = new Size(167, 24);
            labelDonGiaBanGoc.TabIndex = 25;
            labelDonGiaBanGoc.Text = "Đơn giá bán gốc";
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Constantia", 15F);
            textBox3.Location = new Point(18, 486);
            textBox3.Margin = new Padding(4);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(271, 32);
            textBox3.TabIndex = 26;
            // 
            // comboBoxHangSanXuat
            // 
            comboBoxHangSanXuat.Font = new Font("Constantia", 15F);
            comboBoxHangSanXuat.FormattingEnabled = true;
            comboBoxHangSanXuat.Location = new Point(18, 234);
            comboBoxHangSanXuat.Margin = new Padding(4);
            comboBoxHangSanXuat.Name = "comboBoxHangSanXuat";
            comboBoxHangSanXuat.Size = new Size(271, 32);
            comboBoxHangSanXuat.TabIndex = 27;
            // 
            // comboBoxDanhMucSanPham
            // 
            comboBoxDanhMucSanPham.Font = new Font("Constantia", 15F);
            comboBoxDanhMucSanPham.FormattingEnabled = true;
            comboBoxDanhMucSanPham.Items.AddRange(new object[] { "Laptop", "Chuột", "Bàn phím" });
            comboBoxDanhMucSanPham.Location = new Point(18, 318);
            comboBoxDanhMucSanPham.Margin = new Padding(4);
            comboBoxDanhMucSanPham.Name = "comboBoxDanhMucSanPham";
            comboBoxDanhMucSanPham.Size = new Size(271, 32);
            comboBoxDanhMucSanPham.TabIndex = 28;
            // 
            // FormLoaiSanPham
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 746);
            ControlBox = false;
            Controls.Add(dataGridViewDSLoaiSanPham);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 4, 4, 4);
            Name = "FormLoaiSanPham";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSLoaiSanPham).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Button btnTim;
        private Button btnSuaKhachHang;
        private Button btnXoaKhachHang;
        private Button btnThemKhachHang;
        private Label labelHangSanXuat;
        private Label labelTenLoaiSanPham;
        private TextBox txtTenLoaiSanPham;
        private Label labelMaLoaiSanPham;
        private TextBox txtMaLoaiSanPham;
        private DataGridView dataGridViewDSLoaiSanPham;
        private Label labelSoThangBaoHanh;
        private TextBox textBox2;
        private Label labelDanhMucSanPham;
        private Label labelDonGiaBanGoc;
        private TextBox textBox3;
        private ComboBox comboBoxDanhMucSanPham;
        private ComboBox comboBoxHangSanXuat;
    }
}
namespace GUI_HTQLCuaHangLaptop
{
    partial class FormBaoHanh
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
            dataGridViewPhieuBaoHanh = new DataGridView();
            txtMaSerialSanPham = new TextBox();
            labelMaSerialSP = new Label();
            labelLoaiBaoHanh = new Label();
            labelTrangThai = new Label();
            labelKetQua = new Label();
            txtKetQua = new TextBox();
            comboBoxTrangThai = new ComboBox();
            btnTaoPhieuBaoHanh = new Button();
            btnSuaPhieuBaoHanh = new Button();
            btnTimPhieuBaoHanh = new Button();
            comboBoxLoaiBaoHanh = new ComboBox();
            panelChinhSua = new Panel();
            labelLyDoLoi = new Label();
            txtLyDoLoi = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPhieuBaoHanh).BeginInit();
            panelChinhSua.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewPhieuBaoHanh
            // 
            dataGridViewPhieuBaoHanh.AllowUserToAddRows = false;
            dataGridViewPhieuBaoHanh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPhieuBaoHanh.BackgroundColor = Color.White;
            dataGridViewPhieuBaoHanh.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPhieuBaoHanh.Dock = DockStyle.Fill;
            dataGridViewPhieuBaoHanh.Location = new Point(308, 0);
            dataGridViewPhieuBaoHanh.Margin = new Padding(4);
            dataGridViewPhieuBaoHanh.MultiSelect = false;
            dataGridViewPhieuBaoHanh.Name = "dataGridViewPhieuBaoHanh";
            dataGridViewPhieuBaoHanh.ReadOnly = true;
            dataGridViewPhieuBaoHanh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPhieuBaoHanh.Size = new Size(1086, 792);
            dataGridViewPhieuBaoHanh.TabIndex = 3;
            // 
            // txtMaSerialSanPham
            // 
            txtMaSerialSanPham.Font = new Font("Constantia", 15F);
            txtMaSerialSanPham.Location = new Point(15, 63);
            txtMaSerialSanPham.Margin = new Padding(4);
            txtMaSerialSanPham.Name = "txtMaSerialSanPham";
            txtMaSerialSanPham.Size = new Size(271, 32);
            txtMaSerialSanPham.TabIndex = 5;
            // 
            // labelMaSerialSP
            // 
            labelMaSerialSP.AutoSize = true;
            labelMaSerialSP.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaSerialSP.ForeColor = Color.White;
            labelMaSerialSP.Location = new Point(15, 28);
            labelMaSerialSP.Margin = new Padding(4, 0, 4, 0);
            labelMaSerialSP.Name = "labelMaSerialSP";
            labelMaSerialSP.Size = new Size(203, 24);
            labelMaSerialSP.TabIndex = 5;
            labelMaSerialSP.Text = "Mã serial sản phẩm:";
            // 
            // labelLoaiBaoHanh
            // 
            labelLoaiBaoHanh.AutoSize = true;
            labelLoaiBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelLoaiBaoHanh.ForeColor = Color.White;
            labelLoaiBaoHanh.Location = new Point(15, 106);
            labelLoaiBaoHanh.Margin = new Padding(4, 0, 4, 0);
            labelLoaiBaoHanh.Name = "labelLoaiBaoHanh";
            labelLoaiBaoHanh.Size = new Size(147, 24);
            labelLoaiBaoHanh.TabIndex = 6;
            labelLoaiBaoHanh.Text = "Loại bảo hành";
            // 
            // labelTrangThai
            // 
            labelTrangThai.AutoSize = true;
            labelTrangThai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTrangThai.ForeColor = Color.White;
            labelTrangThai.Location = new Point(15, 189);
            labelTrangThai.Margin = new Padding(4, 0, 4, 0);
            labelTrangThai.Name = "labelTrangThai";
            labelTrangThai.Size = new Size(109, 24);
            labelTrangThai.TabIndex = 10;
            labelTrangThai.Text = "Trạng thái";
            // 
            // labelKetQua
            // 
            labelKetQua.AutoSize = true;
            labelKetQua.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelKetQua.ForeColor = Color.White;
            labelKetQua.Location = new Point(15, 348);
            labelKetQua.Margin = new Padding(4, 0, 4, 0);
            labelKetQua.Name = "labelKetQua";
            labelKetQua.Size = new Size(85, 24);
            labelKetQua.TabIndex = 12;
            labelKetQua.Text = "Kết quả";
            // 
            // txtKetQua
            // 
            txtKetQua.Font = new Font("Constantia", 15F);
            txtKetQua.Location = new Point(15, 383);
            txtKetQua.Margin = new Padding(4);
            txtKetQua.Name = "txtKetQua";
            txtKetQua.Size = new Size(271, 32);
            txtKetQua.TabIndex = 14;
            // 
            // comboBoxTrangThai
            // 
            comboBoxTrangThai.Font = new Font("Constantia", 15F);
            comboBoxTrangThai.FormattingEnabled = true;
            comboBoxTrangThai.Items.AddRange(new object[] { "Đang xử lý", "Hoàn thành", "Từ chối" });
            comboBoxTrangThai.Location = new Point(15, 224);
            comboBoxTrangThai.Margin = new Padding(4);
            comboBoxTrangThai.Name = "comboBoxTrangThai";
            comboBoxTrangThai.Size = new Size(271, 32);
            comboBoxTrangThai.TabIndex = 16;
            // 
            // btnTaoPhieuBaoHanh
            // 
            btnTaoPhieuBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnTaoPhieuBaoHanh.Location = new Point(16, 440);
            btnTaoPhieuBaoHanh.Margin = new Padding(4);
            btnTaoPhieuBaoHanh.Name = "btnTaoPhieuBaoHanh";
            btnTaoPhieuBaoHanh.Size = new Size(78, 37);
            btnTaoPhieuBaoHanh.TabIndex = 6;
            btnTaoPhieuBaoHanh.Text = "Tạo";
            btnTaoPhieuBaoHanh.UseVisualStyleBackColor = true;
            // 
            // btnSuaPhieuBaoHanh
            // 
            btnSuaPhieuBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnSuaPhieuBaoHanh.Location = new Point(112, 440);
            btnSuaPhieuBaoHanh.Margin = new Padding(4);
            btnSuaPhieuBaoHanh.Name = "btnSuaPhieuBaoHanh";
            btnSuaPhieuBaoHanh.Size = new Size(78, 37);
            btnSuaPhieuBaoHanh.TabIndex = 18;
            btnSuaPhieuBaoHanh.Text = "Sửa";
            btnSuaPhieuBaoHanh.UseVisualStyleBackColor = true;
            // 
            // btnTimPhieuBaoHanh
            // 
            btnTimPhieuBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnTimPhieuBaoHanh.Location = new Point(208, 440);
            btnTimPhieuBaoHanh.Margin = new Padding(4);
            btnTimPhieuBaoHanh.Name = "btnTimPhieuBaoHanh";
            btnTimPhieuBaoHanh.Size = new Size(78, 37);
            btnTimPhieuBaoHanh.TabIndex = 19;
            btnTimPhieuBaoHanh.Text = "Tìm";
            btnTimPhieuBaoHanh.UseVisualStyleBackColor = true;
            // 
            // labelLyDoLoi
            // 
            labelLyDoLoi.AutoSize = true;
            labelLyDoLoi.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelLyDoLoi.ForeColor = Color.White;
            labelLyDoLoi.Location = new Point(15, 267);
            labelLyDoLoi.Margin = new Padding(4, 0, 4, 0);
            labelLyDoLoi.Name = "labelLyDoLoi";
            labelLyDoLoi.Size = new Size(95, 24);
            labelLyDoLoi.TabIndex = 21;
            labelLyDoLoi.Text = "Lý do lỗi:";
            // 
            // txtLyDoLoi
            // 
            txtLyDoLoi.Font = new Font("Constantia", 15F);
            txtLyDoLoi.Location = new Point(15, 302);
            txtLyDoLoi.Margin = new Padding(4);
            txtLyDoLoi.Name = "txtLyDoLoi";
            txtLyDoLoi.Size = new Size(271, 32);
            txtLyDoLoi.TabIndex = 22;
            // 
            // comboBoxLoaiBaoHanh
            // 
            comboBoxLoaiBaoHanh.Font = new Font("Constantia", 15F);
            comboBoxLoaiBaoHanh.FormattingEnabled = true;
            comboBoxLoaiBaoHanh.Items.AddRange(new object[] { "Cửa hàng", "Hãng" });
            comboBoxLoaiBaoHanh.Location = new Point(15, 141);
            comboBoxLoaiBaoHanh.Margin = new Padding(4);
            comboBoxLoaiBaoHanh.Name = "comboBoxLoaiBaoHanh";
            comboBoxLoaiBaoHanh.Size = new Size(271, 32);
            comboBoxLoaiBaoHanh.TabIndex = 20;
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(comboBoxLoaiBaoHanh);
            panelChinhSua.Controls.Add(btnTimPhieuBaoHanh);
            panelChinhSua.Controls.Add(btnSuaPhieuBaoHanh);
            panelChinhSua.Controls.Add(btnTaoPhieuBaoHanh);
            panelChinhSua.Controls.Add(comboBoxTrangThai);
            panelChinhSua.Controls.Add(txtKetQua);
            panelChinhSua.Controls.Add(labelKetQua);
            panelChinhSua.Controls.Add(labelTrangThai);
            panelChinhSua.Controls.Add(labelLoaiBaoHanh);
            panelChinhSua.Controls.Add(labelMaSerialSP);
            panelChinhSua.Controls.Add(txtMaSerialSanPham);
            panelChinhSua.Controls.Add(txtLyDoLoi);
            panelChinhSua.Controls.Add(labelLyDoLoi);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(308, 792);
            panelChinhSua.TabIndex = 4;
            // 
            // FormBaoHanh
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 792);
            ControlBox = false;
            Controls.Add(dataGridViewPhieuBaoHanh);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormBaoHanh";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dataGridViewPhieuBaoHanh).EndInit();
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridViewPhieuBaoHanh;
        private TextBox txtMaSerialSanPham;
        private Label labelMaSerialSP;
        private Label labelLoaiBaoHanh;
        private Label labelTrangThai;
        private Label labelKetQua;
        private TextBox txtKetQua;
        private ComboBox comboBoxTrangThai;
        private Button btnTaoPhieuBaoHanh;
        private Button btnSuaPhieuBaoHanh;
        private Button btnTimPhieuBaoHanh;
        private ComboBox comboBoxLoaiBaoHanh;
        private Panel panelChinhSua;
        private TextBox txtLyDoLoi;
        private Label labelLyDoLoi;
    }
}
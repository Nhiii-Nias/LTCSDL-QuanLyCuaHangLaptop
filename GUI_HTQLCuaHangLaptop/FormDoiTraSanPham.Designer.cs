namespace GUI_HTQLCuaHangLaptop
{
    partial class FormDoiTraSanPham
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
            txtMaDonHang = new TextBox();
            comboBoxTrangThai = new ComboBox();
            btnTimPhieuDoiTra = new Button();
            btnSuaPhieuDoiTra = new Button();
            btnTaoPhieuDoiTra = new Button();
            comboBoxHinhThucXuLy = new ComboBox();
            labelTrangThai = new Label();
            labelHinhThucXuLy = new Label();
            labelLyDoDoiTra = new Label();
            txtLyDoDoiTra = new TextBox();
            labelMaDonHang = new Label();
            labelMaSerialSP = new Label();
            txtMaSerialSanPham = new TextBox();
            dataGridViewPhieuDoiTra = new DataGridView();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPhieuDoiTra).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(txtMaDonHang);
            panelChinhSua.Controls.Add(comboBoxTrangThai);
            panelChinhSua.Controls.Add(btnTimPhieuDoiTra);
            panelChinhSua.Controls.Add(btnSuaPhieuDoiTra);
            panelChinhSua.Controls.Add(btnTaoPhieuDoiTra);
            panelChinhSua.Controls.Add(comboBoxHinhThucXuLy);
            panelChinhSua.Controls.Add(labelTrangThai);
            panelChinhSua.Controls.Add(labelHinhThucXuLy);
            panelChinhSua.Controls.Add(labelLyDoDoiTra);
            panelChinhSua.Controls.Add(txtLyDoDoiTra);
            panelChinhSua.Controls.Add(labelMaDonHang);
            panelChinhSua.Controls.Add(labelMaSerialSP);
            panelChinhSua.Controls.Add(txtMaSerialSanPham);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(308, 792);
            panelChinhSua.TabIndex = 6;
            // 
            // txtMaDonHang
            // 
            txtMaDonHang.Font = new Font("Constantia", 15F);
            txtMaDonHang.Location = new Point(15, 141);
            txtMaDonHang.Margin = new Padding(4);
            txtMaDonHang.Name = "txtMaDonHang";
            txtMaDonHang.Size = new Size(271, 32);
            txtMaDonHang.TabIndex = 22;
            // 
            // comboBoxTrangThai
            // 
            comboBoxTrangThai.Font = new Font("Constantia", 15F);
            comboBoxTrangThai.FormattingEnabled = true;
            comboBoxTrangThai.Items.AddRange(new object[] { "Đang xử lý", "Hoàn thành", "Từ chối" });
            comboBoxTrangThai.Location = new Point(15, 375);
            comboBoxTrangThai.Margin = new Padding(4);
            comboBoxTrangThai.Name = "comboBoxTrangThai";
            comboBoxTrangThai.Size = new Size(271, 32);
            comboBoxTrangThai.TabIndex = 21;
            // 
            // btnTimPhieuDoiTra
            // 
            btnTimPhieuDoiTra.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnTimPhieuDoiTra.Location = new Point(207, 431);
            btnTimPhieuDoiTra.Margin = new Padding(4);
            btnTimPhieuDoiTra.Name = "btnTimPhieuDoiTra";
            btnTimPhieuDoiTra.Size = new Size(78, 37);
            btnTimPhieuDoiTra.TabIndex = 19;
            btnTimPhieuDoiTra.Text = "Tìm";
            btnTimPhieuDoiTra.UseVisualStyleBackColor = true;
            // 
            // btnSuaPhieuDoiTra
            // 
            btnSuaPhieuDoiTra.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnSuaPhieuDoiTra.Location = new Point(111, 431);
            btnSuaPhieuDoiTra.Margin = new Padding(4);
            btnSuaPhieuDoiTra.Name = "btnSuaPhieuDoiTra";
            btnSuaPhieuDoiTra.Size = new Size(78, 37);
            btnSuaPhieuDoiTra.TabIndex = 18;
            btnSuaPhieuDoiTra.Text = "Sửa";
            btnSuaPhieuDoiTra.UseVisualStyleBackColor = true;
            // 
            // btnTaoPhieuDoiTra
            // 
            btnTaoPhieuDoiTra.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnTaoPhieuDoiTra.Location = new Point(15, 431);
            btnTaoPhieuDoiTra.Margin = new Padding(4);
            btnTaoPhieuDoiTra.Name = "btnTaoPhieuDoiTra";
            btnTaoPhieuDoiTra.Size = new Size(78, 37);
            btnTaoPhieuDoiTra.TabIndex = 6;
            btnTaoPhieuDoiTra.Text = "Tạo";
            btnTaoPhieuDoiTra.UseVisualStyleBackColor = true;
            // 
            // comboBoxHinhThucXuLy
            // 
            comboBoxHinhThucXuLy.Font = new Font("Constantia", 15F);
            comboBoxHinhThucXuLy.FormattingEnabled = true;
            comboBoxHinhThucXuLy.Items.AddRange(new object[] { "Đổi máy khác", "Hoàn tiền" });
            comboBoxHinhThucXuLy.Location = new Point(15, 297);
            comboBoxHinhThucXuLy.Margin = new Padding(4);
            comboBoxHinhThucXuLy.Name = "comboBoxHinhThucXuLy";
            comboBoxHinhThucXuLy.Size = new Size(271, 32);
            comboBoxHinhThucXuLy.TabIndex = 16;
            // 
            // labelTrangThai
            // 
            labelTrangThai.AutoSize = true;
            labelTrangThai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTrangThai.ForeColor = Color.White;
            labelTrangThai.Location = new Point(15, 340);
            labelTrangThai.Margin = new Padding(4, 0, 4, 0);
            labelTrangThai.Name = "labelTrangThai";
            labelTrangThai.Size = new Size(109, 24);
            labelTrangThai.TabIndex = 12;
            labelTrangThai.Text = "Trạng thái";
            // 
            // labelHinhThucXuLy
            // 
            labelHinhThucXuLy.AutoSize = true;
            labelHinhThucXuLy.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelHinhThucXuLy.ForeColor = Color.White;
            labelHinhThucXuLy.Location = new Point(15, 262);
            labelHinhThucXuLy.Margin = new Padding(4, 0, 4, 0);
            labelHinhThucXuLy.Name = "labelHinhThucXuLy";
            labelHinhThucXuLy.Size = new Size(164, 24);
            labelHinhThucXuLy.TabIndex = 10;
            labelHinhThucXuLy.Text = "Hình thức xử lý";
            // 
            // labelLyDoDoiTra
            // 
            labelLyDoDoiTra.AutoSize = true;
            labelLyDoDoiTra.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelLyDoDoiTra.ForeColor = Color.White;
            labelLyDoDoiTra.Location = new Point(15, 184);
            labelLyDoDoiTra.Margin = new Padding(4, 0, 4, 0);
            labelLyDoDoiTra.Name = "labelLyDoDoiTra";
            labelLyDoDoiTra.Size = new Size(129, 24);
            labelLyDoDoiTra.TabIndex = 8;
            labelLyDoDoiTra.Text = "Lý do đổi trả";
            // 
            // txtLyDoDoiTra
            // 
            txtLyDoDoiTra.Font = new Font("Constantia", 15F);
            txtLyDoDoiTra.Location = new Point(15, 219);
            txtLyDoDoiTra.Margin = new Padding(4);
            txtLyDoDoiTra.Name = "txtLyDoDoiTra";
            txtLyDoDoiTra.Size = new Size(271, 32);
            txtLyDoDoiTra.TabIndex = 9;
            // 
            // labelMaDonHang
            // 
            labelMaDonHang.AutoSize = true;
            labelMaDonHang.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaDonHang.ForeColor = Color.White;
            labelMaDonHang.Location = new Point(15, 106);
            labelMaDonHang.Margin = new Padding(4, 0, 4, 0);
            labelMaDonHang.Name = "labelMaDonHang";
            labelMaDonHang.Size = new Size(138, 24);
            labelMaDonHang.TabIndex = 6;
            labelMaDonHang.Text = "Mã đơn hàng";
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
            // txtMaSerialSanPham
            // 
            txtMaSerialSanPham.Font = new Font("Constantia", 15F);
            txtMaSerialSanPham.Location = new Point(15, 63);
            txtMaSerialSanPham.Margin = new Padding(4);
            txtMaSerialSanPham.Name = "txtMaSerialSanPham";
            txtMaSerialSanPham.Size = new Size(271, 32);
            txtMaSerialSanPham.TabIndex = 5;
            // 
            // dataGridViewPhieuDoiTra
            // 
            dataGridViewPhieuDoiTra.AllowUserToAddRows = false;
            dataGridViewPhieuDoiTra.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPhieuDoiTra.BackgroundColor = Color.White;
            dataGridViewPhieuDoiTra.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPhieuDoiTra.Dock = DockStyle.Fill;
            dataGridViewPhieuDoiTra.Location = new Point(308, 0);
            dataGridViewPhieuDoiTra.Margin = new Padding(4);
            dataGridViewPhieuDoiTra.MultiSelect = false;
            dataGridViewPhieuDoiTra.Name = "dataGridViewPhieuDoiTra";
            dataGridViewPhieuDoiTra.ReadOnly = true;
            dataGridViewPhieuDoiTra.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPhieuDoiTra.Size = new Size(1086, 792);
            dataGridViewPhieuDoiTra.TabIndex = 5;
            // 
            // FormDoiTraSanPham
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 792);
            ControlBox = false;
            Controls.Add(dataGridViewPhieuDoiTra);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormDoiTraSanPham";
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPhieuDoiTra).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Button btnTimPhieuDoiTra;
        private Button btnSuaPhieuDoiTra;
        private Button btnTaoPhieuDoiTra;
        private ComboBox comboBoxHinhThucXuLy;
        private Label labelTrangThai;
        private Label labelHinhThucXuLy;
        private Label labelLyDoDoiTra;
        private TextBox txtLyDoDoiTra;
        private Label labelMaDonHang;
        private Label labelMaSerialSP;
        private TextBox txtMaSerialSanPham;
        private DataGridView dataGridViewPhieuDoiTra;
        private ComboBox comboBoxTrangThai;
        private TextBox txtMaDonHang;
    }
}
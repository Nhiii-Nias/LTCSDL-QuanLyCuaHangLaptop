namespace GUI_HTQLCuaHangLaptop
{
    partial class FormKhieuNai
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
            comboBoxDonHangLienQuan = new ComboBox();
            btnTimPhieuBaoHanh = new Button();
            btnSuaPhieuBaoHanh = new Button();
            comboBoxTrangThai = new ComboBox();
            txtKetQua = new TextBox();
            labelKetQua = new Label();
            labelTrangThai = new Label();
            labelLoaiBaoHanh = new Label();
            labelMaSerialSP = new Label();
            txtNoiDungPhanAnh = new TextBox();
            dataGridViewDSKhieuNai = new DataGridView();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSKhieuNai).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(comboBoxDonHangLienQuan);
            panelChinhSua.Controls.Add(btnTimPhieuBaoHanh);
            panelChinhSua.Controls.Add(btnSuaPhieuBaoHanh);
            panelChinhSua.Controls.Add(comboBoxTrangThai);
            panelChinhSua.Controls.Add(txtKetQua);
            panelChinhSua.Controls.Add(labelKetQua);
            panelChinhSua.Controls.Add(labelTrangThai);
            panelChinhSua.Controls.Add(labelLoaiBaoHanh);
            panelChinhSua.Controls.Add(labelMaSerialSP);
            panelChinhSua.Controls.Add(txtNoiDungPhanAnh);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(308, 769);
            panelChinhSua.TabIndex = 6;
            // 
            // comboBoxDonHangLienQuan
            // 
            comboBoxDonHangLienQuan.Font = new Font("Constantia", 15F);
            comboBoxDonHangLienQuan.FormattingEnabled = true;
            comboBoxDonHangLienQuan.Location = new Point(15, 141);
            comboBoxDonHangLienQuan.Margin = new Padding(4);
            comboBoxDonHangLienQuan.Name = "comboBoxDonHangLienQuan";
            comboBoxDonHangLienQuan.Size = new Size(271, 32);
            comboBoxDonHangLienQuan.TabIndex = 20;
            // 
            // btnTimPhieuBaoHanh
            // 
            btnTimPhieuBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnTimPhieuBaoHanh.Location = new Point(152, 356);
            btnTimPhieuBaoHanh.Margin = new Padding(4);
            btnTimPhieuBaoHanh.Name = "btnTimPhieuBaoHanh";
            btnTimPhieuBaoHanh.Size = new Size(117, 37);
            btnTimPhieuBaoHanh.TabIndex = 19;
            btnTimPhieuBaoHanh.Text = "Tìm";
            btnTimPhieuBaoHanh.UseVisualStyleBackColor = true;
            // 
            // btnSuaPhieuBaoHanh
            // 
            btnSuaPhieuBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            btnSuaPhieuBaoHanh.Location = new Point(26, 356);
            btnSuaPhieuBaoHanh.Margin = new Padding(4);
            btnSuaPhieuBaoHanh.Name = "btnSuaPhieuBaoHanh";
            btnSuaPhieuBaoHanh.Size = new Size(109, 37);
            btnSuaPhieuBaoHanh.TabIndex = 18;
            btnSuaPhieuBaoHanh.Text = "Sửa";
            btnSuaPhieuBaoHanh.UseVisualStyleBackColor = true;
            // 
            // comboBoxTrangThai
            // 
            comboBoxTrangThai.Font = new Font("Constantia", 15F);
            comboBoxTrangThai.FormattingEnabled = true;
            comboBoxTrangThai.Location = new Point(13, 219);
            comboBoxTrangThai.Margin = new Padding(4);
            comboBoxTrangThai.Name = "comboBoxTrangThai";
            comboBoxTrangThai.Size = new Size(271, 32);
            comboBoxTrangThai.TabIndex = 16;
            // 
            // txtKetQua
            // 
            txtKetQua.Font = new Font("Constantia", 15F);
            txtKetQua.Location = new Point(13, 297);
            txtKetQua.Margin = new Padding(4);
            txtKetQua.Name = "txtKetQua";
            txtKetQua.Size = new Size(271, 32);
            txtKetQua.TabIndex = 14;
            // 
            // labelKetQua
            // 
            labelKetQua.AutoSize = true;
            labelKetQua.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelKetQua.ForeColor = Color.White;
            labelKetQua.Location = new Point(13, 262);
            labelKetQua.Margin = new Padding(4, 0, 4, 0);
            labelKetQua.Name = "labelKetQua";
            labelKetQua.Size = new Size(85, 24);
            labelKetQua.TabIndex = 12;
            labelKetQua.Text = "Kết quả";
            // 
            // labelTrangThai
            // 
            labelTrangThai.AutoSize = true;
            labelTrangThai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTrangThai.ForeColor = Color.White;
            labelTrangThai.Location = new Point(13, 184);
            labelTrangThai.Margin = new Padding(4, 0, 4, 0);
            labelTrangThai.Name = "labelTrangThai";
            labelTrangThai.Size = new Size(109, 24);
            labelTrangThai.TabIndex = 10;
            labelTrangThai.Text = "Trạng thái";
            // 
            // labelLoaiBaoHanh
            // 
            labelLoaiBaoHanh.AutoSize = true;
            labelLoaiBaoHanh.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelLoaiBaoHanh.ForeColor = Color.White;
            labelLoaiBaoHanh.Location = new Point(15, 106);
            labelLoaiBaoHanh.Margin = new Padding(4, 0, 4, 0);
            labelLoaiBaoHanh.Name = "labelLoaiBaoHanh";
            labelLoaiBaoHanh.Size = new Size(209, 24);
            labelLoaiBaoHanh.TabIndex = 6;
            labelLoaiBaoHanh.Text = "Đơn hàng liên quan ";
            // 
            // labelMaSerialSP
            // 
            labelMaSerialSP.AutoSize = true;
            labelMaSerialSP.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaSerialSP.ForeColor = Color.White;
            labelMaSerialSP.Location = new Point(15, 28);
            labelMaSerialSP.Margin = new Padding(4, 0, 4, 0);
            labelMaSerialSP.Name = "labelMaSerialSP";
            labelMaSerialSP.Size = new Size(194, 24);
            labelMaSerialSP.TabIndex = 5;
            labelMaSerialSP.Text = "Nội dung phản ảnh";
            // 
            // txtNoiDungPhanAnh
            // 
            txtNoiDungPhanAnh.Font = new Font("Constantia", 15F);
            txtNoiDungPhanAnh.Location = new Point(15, 63);
            txtNoiDungPhanAnh.Margin = new Padding(4);
            txtNoiDungPhanAnh.Name = "txtNoiDungPhanAnh";
            txtNoiDungPhanAnh.Size = new Size(271, 32);
            txtNoiDungPhanAnh.TabIndex = 5;
            // 
            // dataGridViewDSKhieuNai
            // 
            dataGridViewDSKhieuNai.AllowUserToAddRows = false;
            dataGridViewDSKhieuNai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSKhieuNai.BackgroundColor = Color.White;
            dataGridViewDSKhieuNai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSKhieuNai.Dock = DockStyle.Fill;
            dataGridViewDSKhieuNai.Location = new Point(308, 0);
            dataGridViewDSKhieuNai.Margin = new Padding(4);
            dataGridViewDSKhieuNai.MultiSelect = false;
            dataGridViewDSKhieuNai.Name = "dataGridViewDSKhieuNai";
            dataGridViewDSKhieuNai.ReadOnly = true;
            dataGridViewDSKhieuNai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSKhieuNai.Size = new Size(1086, 769);
            dataGridViewDSKhieuNai.TabIndex = 5;
            // 
            // FormKhieuNai
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 769);
            ControlBox = false;
            Controls.Add(dataGridViewDSKhieuNai);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormKhieuNai";
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSKhieuNai).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Button btnTimPhieuBaoHanh;
        private Button btnSuaPhieuBaoHanh;
        private ComboBox comboBoxTrangThai;
        private TextBox txtKetQua;
        private Label labelKetQua;
        private Label labelTrangThai;
        private Label labelLoaiBaoHanh;
        private Label labelMaSerialSP;
        private TextBox txtNoiDungPhanAnh;
        private DataGridView dataGridViewDSKhieuNai;
        private ComboBox comboBoxDonHangLienQuan;
    }
}
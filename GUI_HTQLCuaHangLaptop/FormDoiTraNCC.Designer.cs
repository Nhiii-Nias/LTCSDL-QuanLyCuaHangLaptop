namespace GUI_HTQLCuaHangLaptop
{
    partial class FormDoiTraNCC
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
            dateTimePickerNgayTra = new DateTimePicker();
            btnTim = new Button();
            btnThemKhachHang = new Button();
            labelNgayTra = new Label();
            labelLyDoTra = new Label();
            txtLyDoTra = new TextBox();
            labelTenNhaCungCap = new Label();
            txtNhaCungCap = new TextBox();
            labelSerialSPLoi = new Label();
            dataGridViewDSDoiTraNCC = new DataGridView();
            comboBoxSerialSPLoi = new ComboBox();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSDoiTraNCC).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(comboBoxSerialSPLoi);
            panelChinhSua.Controls.Add(dateTimePickerNgayTra);
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnThemKhachHang);
            panelChinhSua.Controls.Add(labelNgayTra);
            panelChinhSua.Controls.Add(labelLyDoTra);
            panelChinhSua.Controls.Add(txtLyDoTra);
            panelChinhSua.Controls.Add(labelTenNhaCungCap);
            panelChinhSua.Controls.Add(txtNhaCungCap);
            panelChinhSua.Controls.Add(labelSerialSPLoi);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(314, 746);
            panelChinhSua.TabIndex = 6;
            // 
            // dateTimePickerNgayTra
            // 
            dateTimePickerNgayTra.CalendarFont = new Font("Constantia", 20F);
            dateTimePickerNgayTra.CustomFormat = "dd/mm/yyyy";
            dateTimePickerNgayTra.Font = new Font("Constantia", 16F);
            dateTimePickerNgayTra.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayTra.Location = new Point(17, 325);
            dateTimePickerNgayTra.Name = "dateTimePickerNgayTra";
            dateTimePickerNgayTra.Size = new Size(269, 34);
            dateTimePickerNgayTra.TabIndex = 23;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(163, 385);
            btnTim.Margin = new Padding(4);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(125, 37);
            btnTim.TabIndex = 20;
            btnTim.Text = "Tìm";
            btnTim.UseVisualStyleBackColor = true;
            // 
            // btnThemKhachHang
            // 
            btnThemKhachHang.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnThemKhachHang.Location = new Point(17, 385);
            btnThemKhachHang.Margin = new Padding(4);
            btnThemKhachHang.Name = "btnThemKhachHang";
            btnThemKhachHang.Size = new Size(125, 37);
            btnThemKhachHang.TabIndex = 6;
            btnThemKhachHang.Text = "Xác nhận";
            btnThemKhachHang.UseVisualStyleBackColor = true;
            // 
            // labelNgayTra
            // 
            labelNgayTra.AutoSize = true;
            labelNgayTra.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelNgayTra.ForeColor = Color.White;
            labelNgayTra.Location = new Point(17, 286);
            labelNgayTra.Margin = new Padding(4, 0, 4, 0);
            labelNgayTra.Name = "labelNgayTra";
            labelNgayTra.Size = new Size(90, 24);
            labelNgayTra.TabIndex = 13;
            labelNgayTra.Text = "Ngày trả";
            // 
            // labelLyDoTra
            // 
            labelLyDoTra.AutoSize = true;
            labelLyDoTra.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelLyDoTra.ForeColor = Color.White;
            labelLyDoTra.Location = new Point(17, 200);
            labelLyDoTra.Margin = new Padding(4, 0, 4, 0);
            labelLyDoTra.Name = "labelLyDoTra";
            labelLyDoTra.Size = new Size(93, 24);
            labelLyDoTra.TabIndex = 10;
            labelLyDoTra.Text = "Lý do trả";
            // 
            // txtLyDoTra
            // 
            txtLyDoTra.Font = new Font("Constantia", 15F);
            txtLyDoTra.Location = new Point(17, 239);
            txtLyDoTra.Margin = new Padding(4);
            txtLyDoTra.Name = "txtLyDoTra";
            txtLyDoTra.Size = new Size(271, 32);
            txtLyDoTra.TabIndex = 11;
            // 
            // labelTenNhaCungCap
            // 
            labelTenNhaCungCap.AutoSize = true;
            labelTenNhaCungCap.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenNhaCungCap.ForeColor = Color.White;
            labelTenNhaCungCap.Location = new Point(17, 114);
            labelTenNhaCungCap.Margin = new Padding(4, 0, 4, 0);
            labelTenNhaCungCap.Name = "labelTenNhaCungCap";
            labelTenNhaCungCap.Size = new Size(177, 24);
            labelTenNhaCungCap.TabIndex = 8;
            labelTenNhaCungCap.Text = "Tên nhà cung cấp";
            // 
            // txtNhaCungCap
            // 
            txtNhaCungCap.Font = new Font("Constantia", 15F);
            txtNhaCungCap.Location = new Point(17, 153);
            txtNhaCungCap.Margin = new Padding(4);
            txtNhaCungCap.Name = "txtNhaCungCap";
            txtNhaCungCap.Size = new Size(271, 32);
            txtNhaCungCap.TabIndex = 9;
            // 
            // labelSerialSPLoi
            // 
            labelSerialSPLoi.AutoSize = true;
            labelSerialSPLoi.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelSerialSPLoi.ForeColor = Color.White;
            labelSerialSPLoi.Location = new Point(17, 28);
            labelSerialSPLoi.Margin = new Padding(4, 0, 4, 0);
            labelSerialSPLoi.Name = "labelSerialSPLoi";
            labelSerialSPLoi.Size = new Size(195, 24);
            labelSerialSPLoi.TabIndex = 5;
            labelSerialSPLoi.Text = "Serial sản phẩm lỗi";
            // 
            // dataGridViewDSDoiTraNCC
            // 
            dataGridViewDSDoiTraNCC.AllowUserToAddRows = false;
            dataGridViewDSDoiTraNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSDoiTraNCC.BackgroundColor = Color.White;
            dataGridViewDSDoiTraNCC.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSDoiTraNCC.Dock = DockStyle.Fill;
            dataGridViewDSDoiTraNCC.Location = new Point(314, 0);
            dataGridViewDSDoiTraNCC.Margin = new Padding(4);
            dataGridViewDSDoiTraNCC.MultiSelect = false;
            dataGridViewDSDoiTraNCC.Name = "dataGridViewDSDoiTraNCC";
            dataGridViewDSDoiTraNCC.ReadOnly = true;
            dataGridViewDSDoiTraNCC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSDoiTraNCC.Size = new Size(1080, 746);
            dataGridViewDSDoiTraNCC.TabIndex = 5;
            // 
            // comboBoxSerialSPLoi
            // 
            comboBoxSerialSPLoi.Font = new Font("Constantia", 15F);
            comboBoxSerialSPLoi.FormattingEnabled = true;
            comboBoxSerialSPLoi.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ" });
            comboBoxSerialSPLoi.Location = new Point(15, 68);
            comboBoxSerialSPLoi.Margin = new Padding(4);
            comboBoxSerialSPLoi.Name = "comboBoxSerialSPLoi";
            comboBoxSerialSPLoi.Size = new Size(271, 32);
            comboBoxSerialSPLoi.TabIndex = 24;
            // 
            // FormDoiTraNCC
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 746);
            ControlBox = false;
            Controls.Add(dataGridViewDSDoiTraNCC);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormDoiTraNCC";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSDoiTraNCC).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Button btnTim;
        private Button btnThemKhachHang;
        private Label labelNgayTra;
        private Label labelLyDoTra;
        private TextBox txtLyDoTra;
        private Label labelTenNhaCungCap;
        private TextBox txtNhaCungCap;
        private Label labelSerialSPLoi;
        private DataGridView dataGridViewDSDoiTraNCC;
        private DateTimePicker dateTimePickerNgayTra;
        private ComboBox comboBoxSerialSPLoi;
    }
}
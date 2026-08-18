namespace GUI_HTQLCuaHangLaptop
{
    partial class FormQuanLyHopDong
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
            comboBoxMaKhachHang = new ComboBox();
            dateTimePickerNgayHetHan = new DateTimePicker();
            dateTimePickerNgayHieuLuc = new DateTimePicker();
            dateTimePickerNgayKy = new DateTimePicker();
            btnTim = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnThem = new Button();
            labelTrangThai = new Label();
            comboBoxTrangThai = new ComboBox();
            labelNgayHetHan = new Label();
            labelNgayHieuLuc = new Label();
            labelGiaTriHopDong = new Label();
            txtGiaTriHopDong = new TextBox();
            labelNgayKy = new Label();
            labelMaKH = new Label();
            dataGridViewDSHopDong = new DataGridView();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSHopDong).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(comboBoxMaKhachHang);
            panelChinhSua.Controls.Add(dateTimePickerNgayHetHan);
            panelChinhSua.Controls.Add(dateTimePickerNgayHieuLuc);
            panelChinhSua.Controls.Add(dateTimePickerNgayKy);
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnSua);
            panelChinhSua.Controls.Add(btnXoa);
            panelChinhSua.Controls.Add(btnThem);
            panelChinhSua.Controls.Add(labelTrangThai);
            panelChinhSua.Controls.Add(comboBoxTrangThai);
            panelChinhSua.Controls.Add(labelNgayHetHan);
            panelChinhSua.Controls.Add(labelNgayHieuLuc);
            panelChinhSua.Controls.Add(labelGiaTriHopDong);
            panelChinhSua.Controls.Add(txtGiaTriHopDong);
            panelChinhSua.Controls.Add(labelNgayKy);
            panelChinhSua.Controls.Add(labelMaKH);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(314, 746);
            panelChinhSua.TabIndex = 4;
            // 
            // comboBoxMaKhachHang
            // 
            comboBoxMaKhachHang.Font = new Font("Constantia", 15F);
            comboBoxMaKhachHang.FormattingEnabled = true;
            comboBoxMaKhachHang.Items.AddRange(new object[] { "Khách lẻ", "Khách sỉ" });
            comboBoxMaKhachHang.Location = new Point(13, 56);
            comboBoxMaKhachHang.Margin = new Padding(4);
            comboBoxMaKhachHang.Name = "comboBoxMaKhachHang";
            comboBoxMaKhachHang.Size = new Size(271, 32);
            comboBoxMaKhachHang.TabIndex = 24;
            // 
            // dateTimePickerNgayHetHan
            // 
            dateTimePickerNgayHetHan.CalendarFont = new Font("Constantia", 20F);
            dateTimePickerNgayHetHan.CustomFormat = "dd/MM/yyyy";
            dateTimePickerNgayHetHan.Font = new Font("Constantia", 16F);
            dateTimePickerNgayHetHan.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayHetHan.Location = new Point(15, 379);
            dateTimePickerNgayHetHan.Name = "dateTimePickerNgayHetHan";
            dateTimePickerNgayHetHan.Size = new Size(269, 34);
            dateTimePickerNgayHetHan.TabIndex = 23;
            // 
            // dateTimePickerNgayHieuLuc
            // 
            dateTimePickerNgayHieuLuc.CalendarFont = new Font("Constantia", 20F);
            dateTimePickerNgayHieuLuc.CustomFormat = "dd/MM/yyyy";
            dateTimePickerNgayHieuLuc.Font = new Font("Constantia", 16F);
            dateTimePickerNgayHieuLuc.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayHieuLuc.Location = new Point(15, 299);
            dateTimePickerNgayHieuLuc.Name = "dateTimePickerNgayHieuLuc";
            dateTimePickerNgayHieuLuc.Size = new Size(269, 34);
            dateTimePickerNgayHieuLuc.TabIndex = 22;
            // 
            // dateTimePickerNgayKy
            // 
            dateTimePickerNgayKy.CalendarFont = new Font("Constantia", 20F);
            dateTimePickerNgayKy.CustomFormat = "dd/MM/yyyy";
            dateTimePickerNgayKy.Font = new Font("Constantia", 16F);
            dateTimePickerNgayKy.Format = DateTimePickerFormat.Custom;
            dateTimePickerNgayKy.Location = new Point(15, 141);
            dateTimePickerNgayKy.Name = "dateTimePickerNgayKy";
            dateTimePickerNgayKy.Size = new Size(269, 34);
            dateTimePickerNgayKy.TabIndex = 21;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(158, 556);
            btnTim.Margin = new Padding(4);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(118, 37);
            btnTim.TabIndex = 20;
            btnTim.Text = "Tìm";
            btnTim.UseVisualStyleBackColor = true;
            btnTim.Click += btnTim_Click;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnSua.Location = new Point(17, 555);
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
            btnXoa.Location = new Point(158, 511);
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
            btnThem.Location = new Point(17, 511);
            btnThem.Margin = new Padding(4);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(118, 37);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // labelTrangThai
            // 
            labelTrangThai.AutoSize = true;
            labelTrangThai.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTrangThai.ForeColor = Color.White;
            labelTrangThai.Location = new Point(15, 424);
            labelTrangThai.Margin = new Padding(4, 0, 4, 0);
            labelTrangThai.Name = "labelTrangThai";
            labelTrangThai.Size = new Size(109, 24);
            labelTrangThai.TabIndex = 13;
            labelTrangThai.Text = "Trạng thái";
            // 
            // comboBoxTrangThai
            // 
            comboBoxTrangThai.Font = new Font("Constantia", 15F);
            comboBoxTrangThai.FormattingEnabled = true;
            comboBoxTrangThai.Items.AddRange(new object[] { "Khách lẻ", "Khách sỉ" });
            comboBoxTrangThai.Location = new Point(15, 459);
            comboBoxTrangThai.Margin = new Padding(4);
            comboBoxTrangThai.Name = "comboBoxTrangThai";
            comboBoxTrangThai.Size = new Size(271, 32);
            comboBoxTrangThai.TabIndex = 5;
            // 
            // labelNgayHetHan
            // 
            labelNgayHetHan.AutoSize = true;
            labelNgayHetHan.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelNgayHetHan.ForeColor = Color.White;
            labelNgayHetHan.Location = new Point(15, 344);
            labelNgayHetHan.Margin = new Padding(4, 0, 4, 0);
            labelNgayHetHan.Name = "labelNgayHetHan";
            labelNgayHetHan.Size = new Size(136, 24);
            labelNgayHetHan.TabIndex = 12;
            labelNgayHetHan.Text = "Ngày hết hạn";
            // 
            // labelNgayHieuLuc
            // 
            labelNgayHieuLuc.AutoSize = true;
            labelNgayHieuLuc.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelNgayHieuLuc.ForeColor = Color.White;
            labelNgayHieuLuc.Location = new Point(15, 264);
            labelNgayHieuLuc.Margin = new Padding(4, 0, 4, 0);
            labelNgayHieuLuc.Name = "labelNgayHieuLuc";
            labelNgayHieuLuc.Size = new Size(143, 24);
            labelNgayHieuLuc.TabIndex = 10;
            labelNgayHieuLuc.Text = "Ngày hiệu lực";
            // 
            // labelGiaTriHopDong
            // 
            labelGiaTriHopDong.AutoSize = true;
            labelGiaTriHopDong.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelGiaTriHopDong.ForeColor = Color.White;
            labelGiaTriHopDong.Location = new Point(15, 186);
            labelGiaTriHopDong.Margin = new Padding(4, 0, 4, 0);
            labelGiaTriHopDong.Name = "labelGiaTriHopDong";
            labelGiaTriHopDong.Size = new Size(170, 24);
            labelGiaTriHopDong.TabIndex = 8;
            labelGiaTriHopDong.Text = "Giá trị hợp đồng";
            // 
            // txtGiaTriHopDong
            // 
            txtGiaTriHopDong.Font = new Font("Constantia", 15F);
            txtGiaTriHopDong.Location = new Point(15, 221);
            txtGiaTriHopDong.Margin = new Padding(4);
            txtGiaTriHopDong.Name = "txtGiaTriHopDong";
            txtGiaTriHopDong.Size = new Size(271, 32);
            txtGiaTriHopDong.TabIndex = 9;
            // 
            // labelNgayKy
            // 
            labelNgayKy.AutoSize = true;
            labelNgayKy.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelNgayKy.ForeColor = Color.White;
            labelNgayKy.Location = new Point(15, 106);
            labelNgayKy.Margin = new Padding(4, 0, 4, 0);
            labelNgayKy.Name = "labelNgayKy";
            labelNgayKy.Size = new Size(84, 24);
            labelNgayKy.TabIndex = 6;
            labelNgayKy.Text = "Ngày ký";
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
            // dataGridViewDSHopDong
            // 
            dataGridViewDSHopDong.AllowUserToAddRows = false;
            dataGridViewDSHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSHopDong.BackgroundColor = Color.White;
            dataGridViewDSHopDong.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSHopDong.Dock = DockStyle.Fill;
            dataGridViewDSHopDong.Location = new Point(314, 0);
            dataGridViewDSHopDong.Margin = new Padding(4);
            dataGridViewDSHopDong.MultiSelect = false;
            dataGridViewDSHopDong.Name = "dataGridViewDSHopDong";
            dataGridViewDSHopDong.ReadOnly = true;
            dataGridViewDSHopDong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSHopDong.Size = new Size(1080, 746);
            dataGridViewDSHopDong.TabIndex = 3;
            // 
            // FormQuanLyHopDong
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 746);
            ControlBox = false;
            Controls.Add(dataGridViewDSHopDong);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormQuanLyHopDong";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSHopDong).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Button btnTim;
        private Button btnSua;
        private Button btnXoa;
        private Button btnThem;
        private Label labelTrangThai;
        private ComboBox comboBoxTrangThai;
        private Label labelNgayHetHan;
        private Label labelNgayHieuLuc;
        private Label labelGiaTriHopDong;
        private TextBox txtGiaTriHopDong;
        private Label labelNgayKy;
        private Label labelMaKH;
        private DataGridView dataGridViewDSHopDong;
        private DateTimePicker dateTimePickerNgayKy;
        private DateTimePicker dateTimePickerNgayHetHan;
        private DateTimePicker dateTimePickerNgayHieuLuc;
        private ComboBox comboBoxMaKhachHang;
    }
}
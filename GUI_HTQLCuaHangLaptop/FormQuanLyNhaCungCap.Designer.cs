namespace GUI_HTQLCuaHangLaptop
{
    partial class FormQuanLyNhaCungCap
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
            labelDiaChi = new Label();
            txtDiaChi = new TextBox();
            labelEmail = new Label();
            txtEmail = new TextBox();
            labelSDT = new Label();
            txtSDT = new TextBox();
            labelTenNhaCungCap = new Label();
            txtTenNhaCungCap = new TextBox();
            dataGridViewDSNhaCungCap = new DataGridView();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSNhaCungCap).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnSuaKhachHang);
            panelChinhSua.Controls.Add(btnXoaKhachHang);
            panelChinhSua.Controls.Add(btnThemKhachHang);
            panelChinhSua.Controls.Add(labelDiaChi);
            panelChinhSua.Controls.Add(txtDiaChi);
            panelChinhSua.Controls.Add(labelEmail);
            panelChinhSua.Controls.Add(txtEmail);
            panelChinhSua.Controls.Add(labelSDT);
            panelChinhSua.Controls.Add(txtSDT);
            panelChinhSua.Controls.Add(labelTenNhaCungCap);
            panelChinhSua.Controls.Add(txtTenNhaCungCap);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(314, 769);
            panelChinhSua.TabIndex = 4;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(158, 452);
            btnTim.Margin = new Padding(4);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(118, 37);
            btnTim.TabIndex = 20;
            btnTim.Text = "Tìm";
            btnTim.UseVisualStyleBackColor = true;
            btnTim.Click += btnTim_Click;
            // 
            // btnSuaKhachHang
            // 
            btnSuaKhachHang.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnSuaKhachHang.Location = new Point(17, 451);
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
            btnXoaKhachHang.Location = new Point(158, 407);
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
            btnThemKhachHang.Location = new Point(17, 407);
            btnThemKhachHang.Margin = new Padding(4);
            btnThemKhachHang.Name = "btnThemKhachHang";
            btnThemKhachHang.Size = new Size(118, 37);
            btnThemKhachHang.TabIndex = 6;
            btnThemKhachHang.Text = "Thêm";
            btnThemKhachHang.UseVisualStyleBackColor = true;
            // 
            // labelDiaChi
            // 
            labelDiaChi.AutoSize = true;
            labelDiaChi.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelDiaChi.ForeColor = Color.White;
            labelDiaChi.Location = new Point(18, 292);
            labelDiaChi.Margin = new Padding(4, 0, 4, 0);
            labelDiaChi.Name = "labelDiaChi";
            labelDiaChi.Size = new Size(79, 24);
            labelDiaChi.TabIndex = 13;
            labelDiaChi.Text = "Địa chỉ";
            // 
            // txtDiaChi
            // 
            txtDiaChi.Font = new Font("Constantia", 15F);
            txtDiaChi.Location = new Point(17, 332);
            txtDiaChi.Margin = new Padding(4);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(271, 32);
            txtDiaChi.TabIndex = 14;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelEmail.ForeColor = Color.White;
            labelEmail.Location = new Point(18, 204);
            labelEmail.Margin = new Padding(4, 0, 4, 0);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(66, 24);
            labelEmail.TabIndex = 10;
            labelEmail.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Constantia", 15F);
            txtEmail.Location = new Point(18, 244);
            txtEmail.Margin = new Padding(4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(271, 32);
            txtEmail.TabIndex = 11;
            // 
            // labelSDT
            // 
            labelSDT.AutoSize = true;
            labelSDT.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelSDT.ForeColor = Color.White;
            labelSDT.Location = new Point(18, 116);
            labelSDT.Margin = new Padding(4, 0, 4, 0);
            labelSDT.Name = "labelSDT";
            labelSDT.Size = new Size(137, 24);
            labelSDT.TabIndex = 8;
            labelSDT.Text = "Số điện thoại";
            // 
            // txtSDT
            // 
            txtSDT.Font = new Font("Constantia", 15F);
            txtSDT.Location = new Point(18, 156);
            txtSDT.Margin = new Padding(4);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(271, 32);
            txtSDT.TabIndex = 9;
            // 
            // labelTenNhaCungCap
            // 
            labelTenNhaCungCap.AutoSize = true;
            labelTenNhaCungCap.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenNhaCungCap.ForeColor = Color.White;
            labelTenNhaCungCap.Location = new Point(18, 28);
            labelTenNhaCungCap.Margin = new Padding(4, 0, 4, 0);
            labelTenNhaCungCap.Name = "labelTenNhaCungCap";
            labelTenNhaCungCap.Size = new Size(177, 24);
            labelTenNhaCungCap.TabIndex = 5;
            labelTenNhaCungCap.Text = "Tên nhà cung cấp";
            // 
            // txtTenNhaCungCap
            // 
            txtTenNhaCungCap.Font = new Font("Constantia", 15F);
            txtTenNhaCungCap.Location = new Point(18, 68);
            txtTenNhaCungCap.Margin = new Padding(4);
            txtTenNhaCungCap.Name = "txtTenNhaCungCap";
            txtTenNhaCungCap.Size = new Size(271, 32);
            txtTenNhaCungCap.TabIndex = 5;
            // 
            // dataGridViewDSNhaCungCap
            // 
            dataGridViewDSNhaCungCap.AllowUserToAddRows = false;
            dataGridViewDSNhaCungCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSNhaCungCap.BackgroundColor = Color.White;
            dataGridViewDSNhaCungCap.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSNhaCungCap.Dock = DockStyle.Fill;
            dataGridViewDSNhaCungCap.Location = new Point(314, 0);
            dataGridViewDSNhaCungCap.Margin = new Padding(4);
            dataGridViewDSNhaCungCap.MultiSelect = false;
            dataGridViewDSNhaCungCap.Name = "dataGridViewDSNhaCungCap";
            dataGridViewDSNhaCungCap.ReadOnly = true;
            dataGridViewDSNhaCungCap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSNhaCungCap.Size = new Size(1080, 769);
            dataGridViewDSNhaCungCap.TabIndex = 3;
            // 
            // FormQuanLyNhaCungCap
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 769);
            ControlBox = false;
            Controls.Add(dataGridViewDSNhaCungCap);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormQuanLyNhaCungCap";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSNhaCungCap).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Button btnTim;
        private Button btnSuaKhachHang;
        private Button btnXoaKhachHang;
        private Button btnThemKhachHang;
        private Label labelDiaChi;
        private TextBox txtDiaChi;
        private Label labelEmail;
        private TextBox txtEmail;
        private Label labelSDT;
        private TextBox txtSDT;
        private DataGridView dataGridViewDSNhaCungCap;
        private Label labelTenNhaCungCap;
        private TextBox txtTenNhaCungCap;
    }
}
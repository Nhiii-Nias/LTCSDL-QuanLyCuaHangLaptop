namespace GUI_HTQLCuaHangLaptop
{
    partial class FormHangSanXuat
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
            labelQuocGia = new Label();
            txtQuocGia = new TextBox();
            labelTenHangSanXuat = new Label();
            txtTenHangSanXuat = new TextBox();
            labelMaHangSanXuat = new Label();
            txtMaHangSanXuat = new TextBox();
            dataGridViewDSHangSanXuat = new DataGridView();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSHangSanXuat).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(btnTim);
            panelChinhSua.Controls.Add(btnSuaKhachHang);
            panelChinhSua.Controls.Add(btnXoaKhachHang);
            panelChinhSua.Controls.Add(btnThemKhachHang);
            panelChinhSua.Controls.Add(labelQuocGia);
            panelChinhSua.Controls.Add(txtQuocGia);
            panelChinhSua.Controls.Add(labelTenHangSanXuat);
            panelChinhSua.Controls.Add(txtTenHangSanXuat);
            panelChinhSua.Controls.Add(labelMaHangSanXuat);
            panelChinhSua.Controls.Add(txtMaHangSanXuat);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(314, 746);
            panelChinhSua.TabIndex = 6;
            // 
            // btnTim
            // 
            btnTim.Font = new Font("Constantia", 13F, FontStyle.Bold);
            btnTim.Location = new Point(159, 350);
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
            btnSuaKhachHang.Location = new Point(18, 349);
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
            btnXoaKhachHang.Location = new Point(159, 305);
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
            btnThemKhachHang.Location = new Point(18, 305);
            btnThemKhachHang.Margin = new Padding(4);
            btnThemKhachHang.Name = "btnThemKhachHang";
            btnThemKhachHang.Size = new Size(118, 37);
            btnThemKhachHang.TabIndex = 6;
            btnThemKhachHang.Text = "Thêm";
            btnThemKhachHang.UseVisualStyleBackColor = true;
            // 
            // labelQuocGia
            // 
            labelQuocGia.AutoSize = true;
            labelQuocGia.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelQuocGia.ForeColor = Color.White;
            labelQuocGia.Location = new Point(18, 204);
            labelQuocGia.Margin = new Padding(4, 0, 4, 0);
            labelQuocGia.Name = "labelQuocGia";
            labelQuocGia.Size = new Size(96, 24);
            labelQuocGia.TabIndex = 10;
            labelQuocGia.Text = "Quốc gia";
            // 
            // txtQuocGia
            // 
            txtQuocGia.Font = new Font("Constantia", 15F);
            txtQuocGia.Location = new Point(18, 244);
            txtQuocGia.Margin = new Padding(4);
            txtQuocGia.Name = "txtQuocGia";
            txtQuocGia.Size = new Size(271, 32);
            txtQuocGia.TabIndex = 11;
            // 
            // labelTenHangSanXuat
            // 
            labelTenHangSanXuat.AutoSize = true;
            labelTenHangSanXuat.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenHangSanXuat.ForeColor = Color.White;
            labelTenHangSanXuat.Location = new Point(18, 116);
            labelTenHangSanXuat.Margin = new Padding(4, 0, 4, 0);
            labelTenHangSanXuat.Name = "labelTenHangSanXuat";
            labelTenHangSanXuat.Size = new Size(184, 24);
            labelTenHangSanXuat.TabIndex = 8;
            labelTenHangSanXuat.Text = "Tên hãng sản xuất";
            // 
            // txtTenHangSanXuat
            // 
            txtTenHangSanXuat.Font = new Font("Constantia", 15F);
            txtTenHangSanXuat.Location = new Point(18, 156);
            txtTenHangSanXuat.Margin = new Padding(4);
            txtTenHangSanXuat.Name = "txtTenHangSanXuat";
            txtTenHangSanXuat.Size = new Size(271, 32);
            txtTenHangSanXuat.TabIndex = 9;
            // 
            // labelMaHangSanXuat
            // 
            labelMaHangSanXuat.AutoSize = true;
            labelMaHangSanXuat.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMaHangSanXuat.ForeColor = Color.White;
            labelMaHangSanXuat.Location = new Point(18, 28);
            labelMaHangSanXuat.Margin = new Padding(4, 0, 4, 0);
            labelMaHangSanXuat.Name = "labelMaHangSanXuat";
            labelMaHangSanXuat.Size = new Size(179, 24);
            labelMaHangSanXuat.TabIndex = 5;
            labelMaHangSanXuat.Text = "Mã hãng sản xuất";
            // 
            // txtMaHangSanXuat
            // 
            txtMaHangSanXuat.Font = new Font("Constantia", 15F);
            txtMaHangSanXuat.Location = new Point(18, 68);
            txtMaHangSanXuat.Margin = new Padding(4);
            txtMaHangSanXuat.Name = "txtMaHangSanXuat";
            txtMaHangSanXuat.Size = new Size(271, 32);
            txtMaHangSanXuat.TabIndex = 5;
            // 
            // dataGridViewDSHangSanXuat
            // 
            dataGridViewDSHangSanXuat.AllowUserToAddRows = false;
            dataGridViewDSHangSanXuat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSHangSanXuat.BackgroundColor = Color.White;
            dataGridViewDSHangSanXuat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSHangSanXuat.Dock = DockStyle.Fill;
            dataGridViewDSHangSanXuat.Location = new Point(314, 0);
            dataGridViewDSHangSanXuat.Margin = new Padding(4);
            dataGridViewDSHangSanXuat.MultiSelect = false;
            dataGridViewDSHangSanXuat.Name = "dataGridViewDSHangSanXuat";
            dataGridViewDSHangSanXuat.ReadOnly = true;
            dataGridViewDSHangSanXuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSHangSanXuat.Size = new Size(1080, 746);
            dataGridViewDSHangSanXuat.TabIndex = 5;
            // 
            // FormHangSanXuat
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 746);
            ControlBox = false;
            Controls.Add(dataGridViewDSHangSanXuat);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "FormHangSanXuat";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSHangSanXuat).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Button btnTim;
        private Button btnSuaKhachHang;
        private Button btnXoaKhachHang;
        private Button btnThemKhachHang;
        private Label labelQuocGia;
        private TextBox txtQuocGia;
        private Label labelTenHangSanXuat;
        private TextBox txtTenHangSanXuat;
        private Label labelMaHangSanXuat;
        private TextBox txtMaHangSanXuat;
        private DataGridView dataGridViewDSHangSanXuat;
    }
}
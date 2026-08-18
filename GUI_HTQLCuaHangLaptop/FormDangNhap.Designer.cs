namespace GUI_HTQLCuaHangLaptop
{
    partial class FormDangNhap
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
            labelTenDangNhap = new Label();
            labelMatKhau = new Label();
            labelTieuDe = new Label();
            panel1 = new Panel();
            txtMatKhau = new TextBox();
            txtTenDangNhap = new TextBox();
            btnDangNhap = new Button();
            btnDoiMatKhau = new Button();
            btnThoat = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // labelTenDangNhap
            // 
            labelTenDangNhap.AutoSize = true;
            labelTenDangNhap.Font = new Font("Constantia", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTenDangNhap.Location = new Point(39, 42);
            labelTenDangNhap.Name = "labelTenDangNhap";
            labelTenDangNhap.Size = new Size(231, 49);
            labelTenDangNhap.TabIndex = 0;
            labelTenDangNhap.Text = "Tài khoản: ";
            // 
            // labelMatKhau
            // 
            labelMatKhau.AutoSize = true;
            labelMatKhau.Font = new Font("Constantia", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelMatKhau.Location = new Point(39, 137);
            labelMatKhau.Name = "labelMatKhau";
            labelMatKhau.Size = new Size(221, 49);
            labelMatKhau.TabIndex = 1;
            labelMatKhau.Text = "Mật khẩu: ";
            // 
            // labelTieuDe
            // 
            labelTieuDe.AutoSize = true;
            labelTieuDe.Font = new Font("Constantia", 35.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTieuDe.ForeColor = SystemColors.ButtonHighlight;
            labelTieuDe.Location = new Point(271, 9);
            labelTieuDe.Name = "labelTieuDe";
            labelTieuDe.Size = new Size(575, 58);
            labelTieuDe.TabIndex = 2;
            labelTieuDe.Text = "ĐĂNG NHẬP HỆ THỐNG";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(192, 192, 255);
            panel1.Controls.Add(txtMatKhau);
            panel1.Controls.Add(txtTenDangNhap);
            panel1.Controls.Add(labelTenDangNhap);
            panel1.Controls.Add(labelMatKhau);
            panel1.Location = new Point(37, 70);
            panel1.Name = "panel1";
            panel1.Size = new Size(1010, 229);
            panel1.TabIndex = 3;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Font = new Font("Constantia", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMatKhau.Location = new Point(276, 133);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(669, 56);
            txtMatKhau.TabIndex = 3;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Font = new Font("Constantia", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTenDangNhap.Location = new Point(276, 39);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(669, 56);
            txtTenDangNhap.TabIndex = 2;
            // 
            // btnDangNhap
            // 
            btnDangNhap.Font = new Font("Constantia", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangNhap.Location = new Point(37, 305);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(174, 62);
            btnDangNhap.TabIndex = 4;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.UseVisualStyleBackColor = true;
            // 
            // btnDoiMatKhau
            // 
            btnDoiMatKhau.Font = new Font("Constantia", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDoiMatKhau.Location = new Point(217, 305);
            btnDoiMatKhau.Name = "btnDoiMatKhau";
            btnDoiMatKhau.Size = new Size(225, 62);
            btnDoiMatKhau.TabIndex = 5;
            btnDoiMatKhau.Text = "Đổi mật khẩu";
            btnDoiMatKhau.UseVisualStyleBackColor = true;
            // 
            // btnThoat
            // 
            btnThoat.Font = new Font("Constantia", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThoat.Location = new Point(873, 305);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(174, 62);
            btnThoat.TabIndex = 6;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = true;
            // 
            // FormDangNhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1081, 386);
            Controls.Add(btnThoat);
            Controls.Add(btnDoiMatKhau);
            Controls.Add(labelTieuDe);
            Controls.Add(btnDangNhap);
            Controls.Add(panel1);
            MaximizeBox = false;
            Name = "FormDangNhap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập hệ thống";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTenDangNhap;
        private Label labelMatKhau;
        private Label labelTieuDe;
        private Panel panel1;
        private TextBox txtTenDangNhap;
        private Button btnDangNhap;
        private Button btnDoiMatKhau;
        private TextBox txtMatKhau;
        private Button btnThoat;
    }
}
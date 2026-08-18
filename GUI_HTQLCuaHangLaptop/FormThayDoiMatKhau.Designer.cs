namespace GUI_HTQLCuaHangLaptop
{
    partial class FormThayDoiMatKhau
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
            labelTieuDe = new Label();
            txtMatKhauMoi = new TextBox();
            txtMatKhauCu = new TextBox();
            labelMatKhauCu = new Label();
            labelMatKhauMoi = new Label();
            btnCapNhat = new Button();
            panel1 = new Panel();
            txtXacNhanMatKhau = new TextBox();
            labelXacNhanMatKhau = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // labelTieuDe
            // 
            labelTieuDe.AutoSize = true;
            labelTieuDe.Font = new Font("Constantia", 35.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTieuDe.ForeColor = SystemColors.ButtonHighlight;
            labelTieuDe.Location = new Point(311, 9);
            labelTieuDe.Name = "labelTieuDe";
            labelTieuDe.Size = new Size(516, 58);
            labelTieuDe.TabIndex = 7;
            labelTieuDe.Text = "THAY ĐỔI MẬT KHẨU";
            // 
            // txtMatKhauMoi
            // 
            txtMatKhauMoi.Font = new Font("Constantia", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMatKhauMoi.Location = new Point(423, 124);
            txtMatKhauMoi.Name = "txtMatKhauMoi";
            txtMatKhauMoi.Size = new Size(553, 56);
            txtMatKhauMoi.TabIndex = 3;
            // 
            // txtMatKhauCu
            // 
            txtMatKhauCu.Font = new Font("Constantia", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMatKhauCu.Location = new Point(423, 32);
            txtMatKhauCu.Name = "txtMatKhauCu";
            txtMatKhauCu.Size = new Size(553, 56);
            txtMatKhauCu.TabIndex = 2;
            // 
            // labelMatKhauCu
            // 
            labelMatKhauCu.AutoSize = true;
            labelMatKhauCu.Font = new Font("Constantia", 27.75F, FontStyle.Bold);
            labelMatKhauCu.Location = new Point(39, 41);
            labelMatKhauCu.Name = "labelMatKhauCu";
            labelMatKhauCu.Size = new Size(256, 45);
            labelMatKhauCu.TabIndex = 0;
            labelMatKhauCu.Text = "Mật khẩu cũ: ";
            // 
            // labelMatKhauMoi
            // 
            labelMatKhauMoi.AutoSize = true;
            labelMatKhauMoi.Font = new Font("Constantia", 27.75F, FontStyle.Bold);
            labelMatKhauMoi.Location = new Point(39, 131);
            labelMatKhauMoi.Name = "labelMatKhauMoi";
            labelMatKhauMoi.Size = new Size(290, 45);
            labelMatKhauMoi.TabIndex = 1;
            labelMatKhauMoi.Text = "Mật khẩu mới: ";
            // 
            // btnCapNhat
            // 
            btnCapNhat.Font = new Font("Constantia", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCapNhat.Location = new Point(35, 380);
            btnCapNhat.Name = "btnCapNhat";
            btnCapNhat.Size = new Size(174, 62);
            btnCapNhat.TabIndex = 9;
            btnCapNhat.Text = "Cập nhật";
            btnCapNhat.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(192, 192, 255);
            panel1.Controls.Add(txtXacNhanMatKhau);
            panel1.Controls.Add(labelXacNhanMatKhau);
            panel1.Controls.Add(txtMatKhauMoi);
            panel1.Controls.Add(txtMatKhauCu);
            panel1.Controls.Add(labelMatKhauCu);
            panel1.Controls.Add(labelMatKhauMoi);
            panel1.Location = new Point(35, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(1010, 299);
            panel1.TabIndex = 8;
            // 
            // txtXacNhanMatKhau
            // 
            txtXacNhanMatKhau.Font = new Font("Constantia", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtXacNhanMatKhau.Location = new Point(423, 214);
            txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
            txtXacNhanMatKhau.Size = new Size(553, 56);
            txtXacNhanMatKhau.TabIndex = 5;
            // 
            // labelXacNhanMatKhau
            // 
            labelXacNhanMatKhau.AutoSize = true;
            labelXacNhanMatKhau.Font = new Font("Constantia", 27.75F, FontStyle.Bold);
            labelXacNhanMatKhau.Location = new Point(39, 221);
            labelXacNhanMatKhau.Name = "labelXacNhanMatKhau";
            labelXacNhanMatKhau.Size = new Size(378, 45);
            labelXacNhanMatKhau.TabIndex = 4;
            labelXacNhanMatKhau.Text = "Xác nhận mật khẩu: ";
            // 
            // FormThayDoiMatKhau
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1081, 462);
            Controls.Add(labelTieuDe);
            Controls.Add(btnCapNhat);
            Controls.Add(panel1);
            MaximizeBox = false;
            Name = "FormThayDoiMatKhau";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thay đổi mật khẩu";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTieuDe;
        private TextBox txtMatKhauMoi;
        private TextBox txtMatKhauCu;
        private Label labelMatKhauCu;
        private Label labelMatKhauMoi;
        private Button btnCapNhat;
        private Panel panel1;
        private TextBox txtXacNhanMatKhau;
        private Label labelXacNhanMatKhau;
    }
}
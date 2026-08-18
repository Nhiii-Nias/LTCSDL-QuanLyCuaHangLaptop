namespace GUI_HTQLCuaHangLaptop
{
    partial class FormQuanLyKhoHang
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
            panel1 = new Panel();
            comboBoxChucNang = new ComboBox();
            labelTrangThai = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(comboBoxChucNang);
            panel1.Controls.Add(labelTrangThai);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1792, 72);
            panel1.TabIndex = 2;
            // 
            // comboBoxChucNang
            // 
            comboBoxChucNang.Font = new Font("Constantia", 30F);
            comboBoxChucNang.FormattingEnabled = true;
            comboBoxChucNang.Items.AddRange(new object[] { "Quản lý nhập hàng", "Tồn kho", "Quản lý nhà cung cấp", "Đổi trả sản phẩm với nhà cung cấp" });
            comboBoxChucNang.Location = new Point(281, 8);
            comboBoxChucNang.Margin = new Padding(4);
            comboBoxChucNang.Name = "comboBoxChucNang";
            comboBoxChucNang.Size = new Size(972, 57);
            comboBoxChucNang.TabIndex = 18;
            // 
            // labelTrangThai
            // 
            labelTrangThai.AutoSize = true;
            labelTrangThai.Font = new Font("Constantia", 30F, FontStyle.Bold);
            labelTrangThai.ForeColor = Color.White;
            labelTrangThai.Location = new Point(13, 11);
            labelTrangThai.Margin = new Padding(4, 0, 4, 0);
            labelTrangThai.Name = "labelTrangThai";
            labelTrangThai.Size = new Size(237, 49);
            labelTrangThai.TabIndex = 17;
            labelTrangThai.Text = "Chức năng:";
            // 
            // FormQuanLyKhoHang
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1792, 974);
            ControlBox = false;
            Controls.Add(panel1);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            IsMdiContainer = true;
            Margin = new Padding(4);
            Name = "FormQuanLyKhoHang";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox comboBoxChucNang;
        private Label labelTrangThai;
    }
}
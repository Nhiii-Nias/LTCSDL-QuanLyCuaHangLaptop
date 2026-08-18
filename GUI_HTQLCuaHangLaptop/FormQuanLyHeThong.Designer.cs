namespace GUI_HTQLCuaHangLaptop
{
    partial class FormQuanLyHeThong
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
            comboBoxTrangThai = new ComboBox();
            labelTrangThai = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(comboBoxTrangThai);
            panel1.Controls.Add(labelTrangThai);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1394, 72);
            panel1.TabIndex = 1;
            // 
            // comboBoxTrangThai
            // 
            comboBoxTrangThai.Font = new Font("Constantia", 30F);
            comboBoxTrangThai.FormattingEnabled = true;
            comboBoxTrangThai.Items.AddRange(new object[] { "Quản lý nhân viên", "Quản lý tài khoản", "Phân quyền" });
            comboBoxTrangThai.Location = new Point(281, 8);
            comboBoxTrangThai.Margin = new Padding(4);
            comboBoxTrangThai.Name = "comboBoxTrangThai";
            comboBoxTrangThai.Size = new Size(972, 57);
            comboBoxTrangThai.TabIndex = 18;
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
            // FormQuanLyHeThong
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 769);
            ControlBox = false;
            Controls.Add(panel1);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            IsMdiContainer = true;
            Margin = new Padding(4, 4, 4, 4);
            Name = "FormQuanLyHeThong";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý hệ thống";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox comboBoxTrangThai;
        private Label labelTrangThai;
    }
}
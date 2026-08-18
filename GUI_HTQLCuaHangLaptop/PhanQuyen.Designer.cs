namespace GUI_HTQLCuaHangLaptop
{
    partial class PhanQuyen
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
            labelTenVaiTro = new Label();
            txtTenVaiTro = new TextBox();
            btnSua = new Button();
            labelMoTaPhanQuyen = new Label();
            txtMoTaPhanQuyen = new TextBox();
            dataGridViewDSVaiTro = new DataGridView();
            panelChinhSua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSVaiTro).BeginInit();
            SuspendLayout();
            // 
            // panelChinhSua
            // 
            panelChinhSua.Controls.Add(labelTenVaiTro);
            panelChinhSua.Controls.Add(txtTenVaiTro);
            panelChinhSua.Controls.Add(btnSua);
            panelChinhSua.Controls.Add(labelMoTaPhanQuyen);
            panelChinhSua.Controls.Add(txtMoTaPhanQuyen);
            panelChinhSua.Dock = DockStyle.Left;
            panelChinhSua.Location = new Point(0, 0);
            panelChinhSua.Margin = new Padding(4);
            panelChinhSua.Name = "panelChinhSua";
            panelChinhSua.Size = new Size(314, 746);
            panelChinhSua.TabIndex = 8;
            // 
            // labelTenVaiTro
            // 
            labelTenVaiTro.AutoSize = true;
            labelTenVaiTro.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelTenVaiTro.ForeColor = Color.White;
            labelTenVaiTro.Location = new Point(13, 61);
            labelTenVaiTro.Margin = new Padding(4, 0, 4, 0);
            labelTenVaiTro.Name = "labelTenVaiTro";
            labelTenVaiTro.Size = new Size(111, 24);
            labelTenVaiTro.TabIndex = 20;
            labelTenVaiTro.Text = "Tên vai trò";
            // 
            // txtTenVaiTro
            // 
            txtTenVaiTro.Font = new Font("Constantia", 15F);
            txtTenVaiTro.Location = new Point(13, 109);
            txtTenVaiTro.Margin = new Padding(4);
            txtTenVaiTro.Name = "txtTenVaiTro";
            txtTenVaiTro.Size = new Size(271, 32);
            txtTenVaiTro.TabIndex = 21;
            // 
            // btnSua
            // 
            btnSua.Font = new Font("Constantia", 12.25F, FontStyle.Bold);
            btnSua.Location = new Point(13, 267);
            btnSua.Margin = new Padding(4);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(271, 37);
            btnSua.TabIndex = 18;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // labelMoTaPhanQuyen
            // 
            labelMoTaPhanQuyen.AutoSize = true;
            labelMoTaPhanQuyen.Font = new Font("Constantia", 15F, FontStyle.Bold);
            labelMoTaPhanQuyen.ForeColor = Color.White;
            labelMoTaPhanQuyen.Location = new Point(13, 165);
            labelMoTaPhanQuyen.Margin = new Padding(4, 0, 4, 0);
            labelMoTaPhanQuyen.Name = "labelMoTaPhanQuyen";
            labelMoTaPhanQuyen.Size = new Size(183, 24);
            labelMoTaPhanQuyen.TabIndex = 10;
            labelMoTaPhanQuyen.Text = "Mô tả phân quyền";
            // 
            // txtMoTaPhanQuyen
            // 
            txtMoTaPhanQuyen.Font = new Font("Constantia", 15F);
            txtMoTaPhanQuyen.Location = new Point(13, 213);
            txtMoTaPhanQuyen.Margin = new Padding(4);
            txtMoTaPhanQuyen.Name = "txtMoTaPhanQuyen";
            txtMoTaPhanQuyen.Size = new Size(271, 32);
            txtMoTaPhanQuyen.TabIndex = 11;
            // 
            // dataGridViewDSVaiTro
            // 
            dataGridViewDSVaiTro.AllowUserToAddRows = false;
            dataGridViewDSVaiTro.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDSVaiTro.BackgroundColor = Color.White;
            dataGridViewDSVaiTro.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDSVaiTro.Dock = DockStyle.Fill;
            dataGridViewDSVaiTro.Location = new Point(314, 0);
            dataGridViewDSVaiTro.Margin = new Padding(4);
            dataGridViewDSVaiTro.MultiSelect = false;
            dataGridViewDSVaiTro.Name = "dataGridViewDSVaiTro";
            dataGridViewDSVaiTro.ReadOnly = true;
            dataGridViewDSVaiTro.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDSVaiTro.Size = new Size(1080, 746);
            dataGridViewDSVaiTro.TabIndex = 7;
            // 
            // PhanQuyen
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateBlue;
            ClientSize = new Size(1394, 746);
            ControlBox = false;
            Controls.Add(dataGridViewDSVaiTro);
            Controls.Add(panelChinhSua);
            Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "PhanQuyen";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            panelChinhSua.ResumeLayout(false);
            panelChinhSua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDSVaiTro).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelChinhSua;
        private Label labelTenVaiTro;
        private TextBox txtTenVaiTro;
        private Button btnSua;
        private Label labelMoTaPhanQuyen;
        private TextBox txtMoTaPhanQuyen;
        private DataGridView dataGridViewDSVaiTro;
    }
}
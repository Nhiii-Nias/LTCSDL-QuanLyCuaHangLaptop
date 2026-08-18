using System.Drawing;
using System.Windows.Forms;

namespace GUI_HTQLCuaHangLaptop
{
    partial class FormBaoCaoThongKe
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelInput = new System.Windows.Forms.Panel();
            this.labelNgayBatDau = new System.Windows.Forms.Label();
            this.dateTimePickerNgayBatDau = new System.Windows.Forms.DateTimePicker();
            this.labelNgayKetThuc = new System.Windows.Forms.Label();
            this.dateTimePickerNgayKetThuc = new System.Windows.Forms.DateTimePicker();
            this.labelLoaiBaoCao = new System.Windows.Forms.Label();
            this.comboBoxLoaiBaoCao = new System.Windows.Forms.ComboBox();
            this.btnTaoBaoCao = new System.Windows.Forms.Button();

            this.panelContent = new System.Windows.Forms.Panel();
            this.panelSummary = new System.Windows.Forms.Panel();
            
            this.panelCard1 = new System.Windows.Forms.Panel();
            this.lblCardTitle1 = new System.Windows.Forms.Label();
            this.lblCardVal1 = new System.Windows.Forms.Label();

            this.panelCard2 = new System.Windows.Forms.Panel();
            this.lblCardTitle2 = new System.Windows.Forms.Label();
            this.lblCardVal2 = new System.Windows.Forms.Label();

            this.panelCard3 = new System.Windows.Forms.Panel();
            this.lblCardTitle3 = new System.Windows.Forms.Label();
            this.lblCardVal3 = new System.Windows.Forms.Label();

            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.pictureBoxChart = new System.Windows.Forms.PictureBox();

            this.panelInput.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelCard1.SuspendLayout();
            this.panelCard2.SuspendLayout();
            this.panelCard3.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChart)).BeginInit();
            this.SuspendLayout();

            // 
            // panelInput
            // 
            this.panelInput.BackColor = System.Drawing.Color.SlateBlue;
            this.panelInput.Controls.Add(this.labelNgayBatDau);
            this.panelInput.Controls.Add(this.dateTimePickerNgayBatDau);
            this.panelInput.Controls.Add(this.labelNgayKetThuc);
            this.panelInput.Controls.Add(this.dateTimePickerNgayKetThuc);
            this.panelInput.Controls.Add(this.labelLoaiBaoCao);
            this.panelInput.Controls.Add(this.comboBoxLoaiBaoCao);
            this.panelInput.Controls.Add(this.btnTaoBaoCao);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInput.Location = new System.Drawing.Point(0, 0);
            this.panelInput.Margin = new System.Windows.Forms.Padding(4);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(326, 769);
            this.panelInput.TabIndex = 0;
            // 
            // labelNgayBatDau
            // 
            this.labelNgayBatDau.AutoSize = true;
            this.labelNgayBatDau.Font = new System.Drawing.Font("Constantia", 15F, System.Drawing.FontStyle.Bold);
            this.labelNgayBatDau.ForeColor = System.Drawing.Color.White;
            this.labelNgayBatDau.Location = new System.Drawing.Point(15, 28);
            this.labelNgayBatDau.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNgayBatDau.Name = "labelNgayBatDau";
            this.labelNgayBatDau.Size = new System.Drawing.Size(91, 24);
            this.labelNgayBatDau.TabIndex = 1;
            this.labelNgayBatDau.Text = "Từ ngày";
            // 
            // dateTimePickerNgayBatDau
            // 
            this.dateTimePickerNgayBatDau.CalendarFont = new System.Drawing.Font("Constantia", 12F);
            this.dateTimePickerNgayBatDau.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerNgayBatDau.Font = new System.Drawing.Font("Constantia", 15F);
            this.dateTimePickerNgayBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerNgayBatDau.Location = new System.Drawing.Point(15, 63);
            this.dateTimePickerNgayBatDau.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePickerNgayBatDau.Name = "dateTimePickerNgayBatDau";
            this.dateTimePickerNgayBatDau.Size = new System.Drawing.Size(271, 32);
            this.dateTimePickerNgayBatDau.TabIndex = 2;
            // 
            // labelNgayKetThuc
            // 
            this.labelNgayKetThuc.AutoSize = true;
            this.labelNgayKetThuc.Font = new System.Drawing.Font("Constantia", 15F, System.Drawing.FontStyle.Bold);
            this.labelNgayKetThuc.ForeColor = System.Drawing.Color.White;
            this.labelNgayKetThuc.Location = new System.Drawing.Point(15, 120);
            this.labelNgayKetThuc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNgayKetThuc.Name = "labelNgayKetThuc";
            this.labelNgayKetThuc.Size = new System.Drawing.Size(102, 24);
            this.labelNgayKetThuc.TabIndex = 3;
            this.labelNgayKetThuc.Text = "Đến ngày";
            // 
            // dateTimePickerNgayKetThuc
            // 
            this.dateTimePickerNgayKetThuc.CalendarFont = new System.Drawing.Font("Constantia", 12F);
            this.dateTimePickerNgayKetThuc.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerNgayKetThuc.Font = new System.Drawing.Font("Constantia", 15F);
            this.dateTimePickerNgayKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerNgayKetThuc.Location = new System.Drawing.Point(15, 155);
            this.dateTimePickerNgayKetThuc.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePickerNgayKetThuc.Name = "dateTimePickerNgayKetThuc";
            this.dateTimePickerNgayKetThuc.Size = new System.Drawing.Size(271, 32);
            this.dateTimePickerNgayKetThuc.TabIndex = 4;
            // 
            // labelLoaiBaoCao
            // 
            this.labelLoaiBaoCao.AutoSize = true;
            this.labelLoaiBaoCao.Font = new System.Drawing.Font("Constantia", 15F, System.Drawing.FontStyle.Bold);
            this.labelLoaiBaoCao.ForeColor = System.Drawing.Color.White;
            this.labelLoaiBaoCao.Location = new System.Drawing.Point(15, 212);
            this.labelLoaiBaoCao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLoaiBaoCao.Name = "labelLoaiBaoCao";
            this.labelLoaiBaoCao.Size = new System.Drawing.Size(130, 24);
            this.labelLoaiBaoCao.TabIndex = 5;
            this.labelLoaiBaoCao.Text = "Loại báo cáo";
            // 
            // comboBoxLoaiBaoCao
            // 
            this.comboBoxLoaiBaoCao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLoaiBaoCao.Font = new System.Drawing.Font("Constantia", 15F);
            this.comboBoxLoaiBaoCao.FormattingEnabled = true;
            this.comboBoxLoaiBaoCao.Location = new System.Drawing.Point(15, 247);
            this.comboBoxLoaiBaoCao.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxLoaiBaoCao.Name = "comboBoxLoaiBaoCao";
            this.comboBoxLoaiBaoCao.Size = new System.Drawing.Size(271, 32);
            this.comboBoxLoaiBaoCao.TabIndex = 6;
            // 
            // btnTaoBaoCao
            // 
            this.btnTaoBaoCao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnTaoBaoCao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaoBaoCao.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Bold);
            this.btnTaoBaoCao.ForeColor = System.Drawing.Color.Black;
            this.btnTaoBaoCao.Location = new System.Drawing.Point(15, 310);
            this.btnTaoBaoCao.Margin = new System.Windows.Forms.Padding(4);
            this.btnTaoBaoCao.Name = "btnTaoBaoCao";
            this.btnTaoBaoCao.Size = new System.Drawing.Size(271, 45);
            this.btnTaoBaoCao.TabIndex = 7;
            this.btnTaoBaoCao.Text = "TẠO BÁO CÁO";
            this.btnTaoBaoCao.UseVisualStyleBackColor = false;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.tableLayoutPanelMain);
            this.panelContent.Controls.Add(this.panelSummary);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(326, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1068, 769);
            this.panelContent.TabIndex = 1;
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.White;
            this.panelSummary.Controls.Add(this.panelCard1);
            this.panelSummary.Controls.Add(this.panelCard2);
            this.panelSummary.Controls.Add(this.panelCard3);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1068, 120);
            this.panelSummary.TabIndex = 0;
            // 
            // panelCard1
            // 
            this.panelCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.panelCard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCard1.Controls.Add(this.lblCardTitle1);
            this.panelCard1.Controls.Add(this.lblCardVal1);
            this.panelCard1.Location = new System.Drawing.Point(15, 15);
            this.panelCard1.Name = "panelCard1";
            this.panelCard1.Size = new System.Drawing.Size(320, 90);
            this.panelCard1.TabIndex = 0;
            // 
            // lblCardTitle1
            // 
            this.lblCardTitle1.Font = new System.Drawing.Font("Constantia", 14F, System.Drawing.FontStyle.Bold);
            this.lblCardTitle1.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblCardTitle1.Location = new System.Drawing.Point(10, 10);
            this.lblCardTitle1.Name = "lblCardTitle1";
            this.lblCardTitle1.Size = new System.Drawing.Size(300, 25);
            this.lblCardTitle1.TabIndex = 0;
            this.lblCardTitle1.Text = "KPI 1";
            // 
            // lblCardVal1
            // 
            this.lblCardVal1.Font = new System.Drawing.Font("Constantia", 22F, System.Drawing.FontStyle.Bold);
            this.lblCardVal1.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblCardVal1.Location = new System.Drawing.Point(10, 40);
            this.lblCardVal1.Name = "lblCardVal1";
            this.lblCardVal1.Size = new System.Drawing.Size(300, 40);
            this.lblCardVal1.TabIndex = 1;
            this.lblCardVal1.Text = "0";
            // 
            // panelCard2
            // 
            this.panelCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.panelCard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCard2.Controls.Add(this.lblCardTitle2);
            this.panelCard2.Controls.Add(this.lblCardVal2);
            this.panelCard2.Location = new System.Drawing.Point(355, 15);
            this.panelCard2.Name = "panelCard2";
            this.panelCard2.Size = new System.Drawing.Size(320, 90);
            this.panelCard2.TabIndex = 1;
            // 
            // lblCardTitle2
            // 
            this.lblCardTitle2.Font = new System.Drawing.Font("Constantia", 14F, System.Drawing.FontStyle.Bold);
            this.lblCardTitle2.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblCardTitle2.Location = new System.Drawing.Point(10, 10);
            this.lblCardTitle2.Name = "lblCardTitle2";
            this.lblCardTitle2.Size = new System.Drawing.Size(300, 25);
            this.lblCardTitle2.TabIndex = 0;
            this.lblCardTitle2.Text = "KPI 2";
            // 
            // lblCardVal2
            // 
            this.lblCardVal2.Font = new System.Drawing.Font("Constantia", 22F, System.Drawing.FontStyle.Bold);
            this.lblCardVal2.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblCardVal2.Location = new System.Drawing.Point(10, 40);
            this.lblCardVal2.Name = "lblCardVal2";
            this.lblCardVal2.Size = new System.Drawing.Size(300, 40);
            this.lblCardVal2.TabIndex = 1;
            this.lblCardVal2.Text = "0";
            // 
            // panelCard3
            // 
            this.panelCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.panelCard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCard3.Controls.Add(this.lblCardTitle3);
            this.panelCard3.Controls.Add(this.lblCardVal3);
            this.panelCard3.Location = new System.Drawing.Point(695, 15);
            this.panelCard3.Name = "panelCard3";
            this.panelCard3.Size = new System.Drawing.Size(320, 90);
            this.panelCard3.TabIndex = 2;
            // 
            // lblCardTitle3
            // 
            this.lblCardTitle3.Font = new System.Drawing.Font("Constantia", 14F, System.Drawing.FontStyle.Bold);
            this.lblCardTitle3.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblCardTitle3.Location = new System.Drawing.Point(10, 10);
            this.lblCardTitle3.Name = "lblCardTitle3";
            this.lblCardTitle3.Size = new System.Drawing.Size(300, 25);
            this.lblCardTitle3.TabIndex = 0;
            this.lblCardTitle3.Text = "KPI 3";
            // 
            // lblCardVal3
            // 
            this.lblCardVal3.Font = new System.Drawing.Font("Constantia", 22F, System.Drawing.FontStyle.Bold);
            this.lblCardVal3.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblCardVal3.Location = new System.Drawing.Point(10, 40);
            this.lblCardVal3.Name = "lblCardVal3";
            this.lblCardVal3.Size = new System.Drawing.Size(300, 40);
            this.lblCardVal3.TabIndex = 1;
            this.lblCardVal3.Text = "0";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanelMain.Controls.Add(this.dataGridView, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxChart, 1, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 120);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1068, 649);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(10, 10);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(10);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(460, 629);
            this.dataGridView.TabIndex = 0;
            // 
            // pictureBoxChart
            // 
            this.pictureBoxChart.BackColor = System.Drawing.Color.White;
            this.pictureBoxChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxChart.Location = new System.Drawing.Point(490, 10);
            this.pictureBoxChart.Margin = new System.Windows.Forms.Padding(10);
            this.pictureBoxChart.Name = "pictureBoxChart";
            this.pictureBoxChart.Size = new System.Drawing.Size(568, 629);
            this.pictureBoxChart.TabIndex = 1;
            this.pictureBoxChart.TabStop = false;
            // 
            // FormBaoCaoThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1394, 769);
            this.ControlBox = false;
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelInput);
            this.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormBaoCaoThongKe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.panelCard1.ResumeLayout(false);
            this.panelCard2.ResumeLayout(false);
            this.panelCard3.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChart)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Label labelNgayBatDau;
        private System.Windows.Forms.DateTimePicker dateTimePickerNgayBatDau;
        private System.Windows.Forms.Label labelNgayKetThuc;
        private System.Windows.Forms.DateTimePicker dateTimePickerNgayKetThuc;
        private System.Windows.Forms.Label labelLoaiBaoCao;
        private System.Windows.Forms.ComboBox comboBoxLoaiBaoCao;
        private System.Windows.Forms.Button btnTaoBaoCao;

        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelSummary;
        
        private System.Windows.Forms.Panel panelCard1;
        private System.Windows.Forms.Label lblCardTitle1;
        private System.Windows.Forms.Label lblCardVal1;

        private System.Windows.Forms.Panel panelCard2;
        private System.Windows.Forms.Label lblCardTitle2;
        private System.Windows.Forms.Label lblCardVal2;

        private System.Windows.Forms.Panel panelCard3;
        private System.Windows.Forms.Label lblCardTitle3;
        private System.Windows.Forms.Label lblCardVal3;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.PictureBox pictureBoxChart;
    }
}
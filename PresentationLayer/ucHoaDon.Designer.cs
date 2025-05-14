namespace PresentationLayer
{
    partial class ucHoaDon
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_maHD = new System.Windows.Forms.Label();
            this.lb_maTT = new System.Windows.Forms.Label();
            this.lb_tenBan = new System.Windows.Forms.Label();
            this.lb_gioTT = new System.Windows.Forms.Label();
            this.lb_ngayTT = new System.Windows.Forms.Label();
            this.lb_tenTN = new System.Windows.Forms.Label();
            this.lb_phuongthucTT = new System.Windows.Forms.Label();
            this.lb_tongTien = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMain.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Location = new System.Drawing.Point(8, 8);
            this.panelMain.Size = new System.Drawing.Size(376, 213);
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Padding = new System.Windows.Forms.Padding(12);
            this.panelMain.Controls.Add(this.lb_maHD);
            this.panelMain.Controls.Add(this.lb_maTT);
            this.panelMain.Controls.Add(this.lb_tenBan);
            this.panelMain.Controls.Add(this.lb_gioTT);
            this.panelMain.Controls.Add(this.lb_ngayTT);
            this.panelMain.Controls.Add(this.lb_tenTN);
            this.panelMain.Controls.Add(this.lb_phuongthucTT);
            this.panelMain.Controls.Add(this.lb_tongTien);
            // 
            // lb_maHD
            // 
            this.lb_maHD.AutoSize = true;
            this.lb_maHD.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_maHD.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lb_maHD.Location = new System.Drawing.Point(16, 12);
            this.lb_maHD.Name = "lb_maHD";
            this.lb_maHD.Size = new System.Drawing.Size(60, 30);
            this.lb_maHD.TabIndex = 0;
            this.lb_maHD.Text = "HĐ:";
            // 
            // lb_maTT
            // 
            this.lb_maTT.AutoSize = true;
            this.lb_maTT.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_maTT.ForeColor = System.Drawing.Color.Gray;
            this.lb_maTT.Location = new System.Drawing.Point(16, 48);
            this.lb_maTT.Name = "lb_maTT";
            this.lb_maTT.Size = new System.Drawing.Size(65, 19);
            this.lb_maTT.TabIndex = 1;
            this.lb_maTT.Text = "Check:";
            // 
            // lb_tenBan
            // 
            this.lb_tenBan.AutoSize = true;
            this.lb_tenBan.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_tenBan.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.lb_tenBan.Location = new System.Drawing.Point(16, 75);
            this.lb_tenBan.Name = "lb_tenBan";
            this.lb_tenBan.Size = new System.Drawing.Size(44, 20);
            this.lb_tenBan.TabIndex = 2;
            this.lb_tenBan.Text = "Bàn:";
            // 
            // lb_gioTT
            // 
            this.lb_gioTT.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_gioTT.ForeColor = System.Drawing.Color.Gray;
            this.lb_gioTT.Location = new System.Drawing.Point(260, 75);
            this.lb_gioTT.Name = "lb_gioTT";
            this.lb_gioTT.Size = new System.Drawing.Size(90, 20);
            this.lb_gioTT.TabIndex = 3;
            this.lb_gioTT.Text = "00:00";
            this.lb_gioTT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_ngayTT
            // 
            this.lb_ngayTT.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ngayTT.ForeColor = System.Drawing.Color.Gray;
            this.lb_ngayTT.Location = new System.Drawing.Point(260, 48);
            this.lb_ngayTT.Name = "lb_ngayTT";
            this.lb_ngayTT.Size = new System.Drawing.Size(90, 20);
            this.lb_ngayTT.TabIndex = 4;
            this.lb_ngayTT.Text = "30/04/1975";
            this.lb_ngayTT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_tenTN
            // 
            this.lb_tenTN.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_tenTN.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.lb_tenTN.Location = new System.Drawing.Point(16, 110);
            this.lb_tenTN.Name = "lb_tenTN";
            this.lb_tenTN.Size = new System.Drawing.Size(170, 20);
            this.lb_tenTN.TabIndex = 5;
            this.lb_tenTN.Text = "TN Chiều";
            this.lb_tenTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_phuongthucTT
            // 
            this.lb_phuongthucTT.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_phuongthucTT.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.lb_phuongthucTT.Location = new System.Drawing.Point(16, 140);
            this.lb_phuongthucTT.Name = "lb_phuongthucTT";
            this.lb_phuongthucTT.Size = new System.Drawing.Size(170, 20);
            this.lb_phuongthucTT.TabIndex = 6;
            this.lb_phuongthucTT.Text = "Cast";
            this.lb_phuongthucTT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_tongTien
            // 
            this.lb_tongTien.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_tongTien.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.lb_tongTien.Location = new System.Drawing.Point(16, 170);
            this.lb_tongTien.Name = "lb_tongTien";
            this.lb_tongTien.Size = new System.Drawing.Size(334, 35);
            this.lb_tongTien.TabIndex = 7;
            this.lb_tongTien.Text = "100000";
            this.lb_tongTien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ucHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelMain);
            this.Name = "ucHoaDon";
            this.Size = new System.Drawing.Size(392, 229);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lb_maHD;
        private System.Windows.Forms.Label lb_maTT;
        private System.Windows.Forms.Label lb_tenBan;
        private System.Windows.Forms.Label lb_gioTT;
        private System.Windows.Forms.Label lb_ngayTT;
        private System.Windows.Forms.Label lb_tenTN;
        private System.Windows.Forms.Label lb_phuongthucTT;
        private System.Windows.Forms.Label lb_tongTien;
        private System.Windows.Forms.Panel panelMain;
    }
}

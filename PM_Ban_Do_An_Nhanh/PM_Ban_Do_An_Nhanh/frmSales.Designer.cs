namespace PM_Ban_Do_An_Nhanh
{
    partial class frmSales
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
        private System.Windows.Forms.FlowLayoutPanel pnlMonAn;
        private System.Windows.Forms.Button btnPrintLastBill;

        private void InitializeComponent()
        {
            this.pnlMonAn = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dgvKhachHang = new System.Windows.Forms.DataGridView();
            this.dgvOrderList = new System.Windows.Forms.DataGridView();
            this.lblTongTienText = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblTenKhachHang = new System.Windows.Forms.Label();
            this.txtSDTKhachHang = new System.Windows.Forms.TextBox();
            this.btnTimKH = new System.Windows.Forms.Button();
            this.btnXoaKH = new System.Windows.Forms.Button();
            this.btnPlusItem = new System.Windows.Forms.Button();
            this.btnMinusItem = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnHuyDon = new System.Windows.Forms.Button();
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
            this.MaKH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenKH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SDT_KhachHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMonAn
            // 
            this.pnlMonAn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMonAn.Location = new System.Drawing.Point(0, 0);
            this.pnlMonAn.Name = "pnlMonAn";
            this.pnlMonAn.Size = new System.Drawing.Size(1319, 179);
            this.pnlMonAn.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExportPdf);
            this.panel1.Controls.Add(this.dgvHoaDon);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.dgvKhachHang);
            this.panel1.Controls.Add(this.dgvOrderList);
            this.panel1.Controls.Add(this.lblTongTienText);
            this.panel1.Controls.Add(this.lblTongTien);
            this.panel1.Controls.Add(this.lblTenKhachHang);
            this.panel1.Controls.Add(this.txtSDTKhachHang);
            this.panel1.Controls.Add(this.btnTimKH);
            this.panel1.Controls.Add(this.btnXoaKH);
            this.panel1.Controls.Add(this.btnPlusItem);
            this.panel1.Controls.Add(this.btnMinusItem);
            this.panel1.Controls.Add(this.btnRemoveItem);
            this.panel1.Controls.Add(this.btnThanhToan);
            this.panel1.Controls.Add(this.btnHuyDon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.panel1.Location = new System.Drawing.Point(0, 179);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1319, 480);
            this.panel1.TabIndex = 1;
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = true;
            this.btnPrint.Location = new System.Drawing.Point(752, 228);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(86, 26);
            this.btnPrint.TabIndex = 25;
            this.btnPrint.Text = "In hóa đơn";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dgvKhachHang
            // 
            this.dgvKhachHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKhachHang.Location = new System.Drawing.Point(12, 340);
            this.dgvKhachHang.Name = "dgvKhachHang";
            this.dgvKhachHang.RowHeadersWidth = 51;
            this.dgvKhachHang.RowTemplate.Height = 24;
            this.dgvKhachHang.Size = new System.Drawing.Size(495, 137);
            this.dgvKhachHang.TabIndex = 24;
            this.dgvKhachHang.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKhachHang_CellClick);
            // 
            // dgvOrderList
            // 
            this.dgvOrderList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderList.Location = new System.Drawing.Point(0, 6);
            this.dgvOrderList.Name = "dgvOrderList";
            this.dgvOrderList.RowHeadersWidth = 51;
            this.dgvOrderList.Size = new System.Drawing.Size(729, 273);
            this.dgvOrderList.TabIndex = 12;
            // 
            // lblTongTienText
            // 
            this.lblTongTienText.AutoSize = true;
            this.lblTongTienText.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTongTienText.Location = new System.Drawing.Point(553, 301);
            this.lblTongTienText.Name = "lblTongTienText";
            this.lblTongTienText.Size = new System.Drawing.Size(89, 19);
            this.lblTongTienText.TabIndex = 13;
            this.lblTongTienText.Text = "Tổng tiền:";
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTongTien.Location = new System.Drawing.Point(648, 301);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(58, 19);
            this.lblTongTien.TabIndex = 15;
            this.lblTongTien.Text = "0 VNĐ";
            // 
            // lblTenKhachHang
            // 
            this.lblTenKhachHang.AutoSize = true;
            this.lblTenKhachHang.Location = new System.Drawing.Point(27, 282);
            this.lblTenKhachHang.Name = "lblTenKhachHang";
            this.lblTenKhachHang.Size = new System.Drawing.Size(117, 16);
            this.lblTenKhachHang.TabIndex = 16;
            this.lblTenKhachHang.Text = "Tên khách hàng: ";
            // 
            // txtSDTKhachHang
            // 
            this.txtSDTKhachHang.Location = new System.Drawing.Point(27, 312);
            this.txtSDTKhachHang.Name = "txtSDTKhachHang";
            this.txtSDTKhachHang.Size = new System.Drawing.Size(150, 22);
            this.txtSDTKhachHang.TabIndex = 14;
            // 
            // btnTimKH
            // 
            this.btnTimKH.Location = new System.Drawing.Point(185, 310);
            this.btnTimKH.Name = "btnTimKH";
            this.btnTimKH.Size = new System.Drawing.Size(75, 23);
            this.btnTimKH.TabIndex = 17;
            this.btnTimKH.Text = "Tìm KH";
            this.btnTimKH.UseVisualStyleBackColor = true;
            this.btnTimKH.Click += new System.EventHandler(this.btnTimKH_Click);
            // 
            // btnXoaKH
            // 
            this.btnXoaKH.Location = new System.Drawing.Point(265, 310);
            this.btnXoaKH.Name = "btnXoaKH";
            this.btnXoaKH.Size = new System.Drawing.Size(75, 23);
            this.btnXoaKH.TabIndex = 18;
            this.btnXoaKH.Text = "Xóa KH";
            this.btnXoaKH.UseVisualStyleBackColor = true;
            this.btnXoaKH.Click += new System.EventHandler(this.btnXoaKH_Click);
            // 
            // btnPlusItem
            // 
            this.btnPlusItem.Location = new System.Drawing.Point(752, 22);
            this.btnPlusItem.Name = "btnPlusItem";
            this.btnPlusItem.Size = new System.Drawing.Size(30, 30);
            this.btnPlusItem.TabIndex = 19;
            this.btnPlusItem.Text = "+";
            this.btnPlusItem.UseVisualStyleBackColor = true;
            this.btnPlusItem.Click += new System.EventHandler(this.btnPlusItem_Click);
            // 
            // btnMinusItem
            // 
            this.btnMinusItem.Location = new System.Drawing.Point(802, 22);
            this.btnMinusItem.Name = "btnMinusItem";
            this.btnMinusItem.Size = new System.Drawing.Size(30, 30);
            this.btnMinusItem.TabIndex = 20;
            this.btnMinusItem.Text = "-";
            this.btnMinusItem.UseVisualStyleBackColor = true;
            this.btnMinusItem.Click += new System.EventHandler(this.btnMinusItem_Click);
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Location = new System.Drawing.Point(750, 69);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(86, 30);
            this.btnRemoveItem.TabIndex = 21;
            this.btnRemoveItem.Text = "Xóa món";
            this.btnRemoveItem.UseVisualStyleBackColor = true;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.AutoSize = true;
            this.btnThanhToan.Location = new System.Drawing.Point(752, 120);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(89, 30);
            this.btnThanhToan.TabIndex = 22;
            this.btnThanhToan.Text = "Thanh toán";
            this.btnThanhToan.UseVisualStyleBackColor = true;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // btnHuyDon
            // 
            this.btnHuyDon.Location = new System.Drawing.Point(752, 174);
            this.btnHuyDon.Name = "btnHuyDon";
            this.btnHuyDon.Size = new System.Drawing.Size(84, 30);
            this.btnHuyDon.TabIndex = 23;
            this.btnHuyDon.Text = "Hủy đơn";
            this.btnHuyDon.UseVisualStyleBackColor = true;
            this.btnHuyDon.Click += new System.EventHandler(this.btnHuyDon_Click);
            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoaDon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaKH,
            this.TenKH,
            this.SDT_KhachHang});
            this.dgvHoaDon.Location = new System.Drawing.Point(867, 6);
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.RowHeadersWidth = 51;
            this.dgvHoaDon.RowTemplate.Height = 24;
            this.dgvHoaDon.Size = new System.Drawing.Size(440, 273);
            this.dgvHoaDon.TabIndex = 26;
            this.dgvHoaDon.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHoaDon_CellClick);
            // 
            // MaKH
            // 
            this.MaKH.HeaderText = "Mã Khách";
            this.MaKH.MinimumWidth = 6;
            this.MaKH.Name = "MaKH";
            this.MaKH.ReadOnly = true;
            this.MaKH.Visible = false;
            this.MaKH.Width = 125;
            // 
            // TenKH
            // 
            this.TenKH.DataPropertyName = "TenKH";
            this.TenKH.HeaderText = "Tên khách hàng";
            this.TenKH.MinimumWidth = 6;
            this.TenKH.Name = "TenKH";
            this.TenKH.ReadOnly = true;
            this.TenKH.Width = 125;
            // 
            // SDT_KhachHang
            // 
            this.SDT_KhachHang.DataPropertyName = "SDT_KhachHang";
            this.SDT_KhachHang.HeaderText = "Số điện thoại ";
            this.SDT_KhachHang.MinimumWidth = 6;
            this.SDT_KhachHang.Name = "SDT_KhachHang";
            this.SDT_KhachHang.ReadOnly = true;
            this.SDT_KhachHang.Width = 125;
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.AutoSize = true;
            this.btnExportPdf.Location = new System.Drawing.Point(752, 272);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(96, 26);
            this.btnExportPdf.TabIndex = 27;
            this.btnExportPdf.Text = "Xuất PDF";
            this.btnExportPdf.UseVisualStyleBackColor = true;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // frmSales
            // 
            this.ClientSize = new System.Drawing.Size(1319, 659);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlMonAn);
            this.Name = "frmSales";
            this.Text = "Bán hàng";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvOrderList;
        private System.Windows.Forms.Label lblTongTienText;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblTenKhachHang;
        private System.Windows.Forms.TextBox txtSDTKhachHang;
        private System.Windows.Forms.Button btnTimKH;
        private System.Windows.Forms.Button btnXoaKH;
        private System.Windows.Forms.Button btnPlusItem;
        private System.Windows.Forms.Button btnMinusItem;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnHuyDon;
        private System.Windows.Forms.DataGridView dgvKhachHang;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaKH;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenKH;
        private System.Windows.Forms.DataGridViewTextBoxColumn SDT_KhachHang;
        private System.Windows.Forms.Button btnExportPdf;
    }
}
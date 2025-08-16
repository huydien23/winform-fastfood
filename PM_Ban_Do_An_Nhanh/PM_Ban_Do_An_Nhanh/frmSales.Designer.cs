using System;

namespace PM_Ban_Do_An_Nhanh
{
    partial class frmSales
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabOrder;
        private System.Windows.Forms.TabPage tabCustomer;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.FlowLayoutPanel pnlMonAn;
        private System.Windows.Forms.DataGridView dgvOrderList;
        private System.Windows.Forms.DataGridView dgvKhachHang;
        private System.Windows.Forms.DataGridView dgvHoaDon;
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
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.TextBox txtSearchMenu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabOrder = new System.Windows.Forms.TabPage();
            this.tabCustomer = new System.Windows.Forms.TabPage();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.pnlMonAn = new System.Windows.Forms.FlowLayoutPanel();
            this.dgvOrderList = new System.Windows.Forms.DataGridView();
            this.dgvKhachHang = new System.Windows.Forms.DataGridView();
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
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
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.txtSearchMenu = new System.Windows.Forms.TextBox();

            this.mainTabControl.SuspendLayout();
            this.tabOrder.SuspendLayout();
            this.tabCustomer.SuspendLayout();
            this.tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            this.SuspendLayout();

            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabOrder);
            this.mainTabControl.Controls.Add(this.tabCustomer);
            this.mainTabControl.Controls.Add(this.tabHistory);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.mainTabControl.ItemSize = new System.Drawing.Size(120, 40);
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1400, 800);
            this.mainTabControl.TabIndex = 0;

            // 
            // tabOrder
            // 
            this.tabOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabOrder.Location = new System.Drawing.Point(4, 44);
            this.tabOrder.Name = "tabOrder";
            this.tabOrder.Padding = new System.Windows.Forms.Padding(16);
            this.tabOrder.Size = new System.Drawing.Size(1392, 752);
            this.tabOrder.TabIndex = 0;
            this.tabOrder.Text = "🛒 Đặt hàng";

            // 
            // tabCustomer
            // 
            this.tabCustomer.Controls.Add(this.dgvKhachHang);
            this.tabCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabCustomer.Location = new System.Drawing.Point(4, 44);
            this.tabCustomer.Name = "tabCustomer";
            this.tabCustomer.Padding = new System.Windows.Forms.Padding(16);
            this.tabCustomer.Size = new System.Drawing.Size(1392, 752);
            this.tabCustomer.TabIndex = 1;
            this.tabCustomer.Text = "👥 Khách hàng";

            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.dgvHoaDon);
            this.tabHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabHistory.Location = new System.Drawing.Point(4, 44);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Padding = new System.Windows.Forms.Padding(16);
            this.tabHistory.Size = new System.Drawing.Size(1392, 752);
            this.tabHistory.TabIndex = 2;
            this.tabHistory.Text = "📋 Lịch sử";

            // 
            // pnlMonAn
            // 
            this.pnlMonAn.AutoScroll = true;
            this.pnlMonAn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlMonAn.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlMonAn.WrapContents = true;
            this.pnlMonAn.Name = "pnlMonAn";
            this.pnlMonAn.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMonAn.TabIndex = 0;

            // 
            // dgvOrderList
            // 
            this.dgvOrderList.AllowUserToAddRows = false;
            this.dgvOrderList.AllowUserToDeleteRows = false;
            this.dgvOrderList.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrderList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrderList.ColumnHeadersHeight = 45;
            this.dgvOrderList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvOrderList.MultiSelect = false;
            this.dgvOrderList.Name = "dgvOrderList";
            this.dgvOrderList.ReadOnly = true;
            this.dgvOrderList.RowHeadersVisible = false;
            this.dgvOrderList.RowTemplate.Height = 35;
            this.dgvOrderList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderList.TabIndex = 1;

            // 
            // dgvKhachHang
            // 
            this.dgvKhachHang.AllowUserToAddRows = false;
            this.dgvKhachHang.AllowUserToDeleteRows = false;
            this.dgvKhachHang.BackgroundColor = System.Drawing.Color.White;
            this.dgvKhachHang.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvKhachHang.ColumnHeadersHeight = 45;
            this.dgvKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKhachHang.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvKhachHang.MultiSelect = false;
            this.dgvKhachHang.Name = "dgvKhachHang";
            this.dgvKhachHang.ReadOnly = true;
            this.dgvKhachHang.RowHeadersVisible = false;
            this.dgvKhachHang.RowTemplate.Height = 35;
            this.dgvKhachHang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKhachHang.TabIndex = 2;
            this.dgvKhachHang.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKhachHang_CellClick);

            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.AllowUserToAddRows = false;
            this.dgvHoaDon.AllowUserToDeleteRows = false;
            this.dgvHoaDon.BackgroundColor = System.Drawing.Color.White;
            this.dgvHoaDon.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvHoaDon.ColumnHeadersHeight = 45;
            this.dgvHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHoaDon.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvHoaDon.MultiSelect = false;
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.ReadOnly = true;
            this.dgvHoaDon.RowHeadersVisible = false;
            this.dgvHoaDon.RowTemplate.Height = 35;
            this.dgvHoaDon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHoaDon.TabIndex = 3;
            this.dgvHoaDon.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHoaDon_CellClick);

            // Basic initialization - detailed setup will be done in SetupOrderTab
            this.lblTongTien = new System.Windows.Forms.Label();
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Text = "0 VNĐ";
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.White;

            this.lblTenKhachHang = new System.Windows.Forms.Label();
            this.lblTenKhachHang.Name = "lblTenKhachHang";
            this.lblTenKhachHang.Text = "Khách lẻ";
            this.lblTenKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.txtSDTKhachHang = new System.Windows.Forms.TextBox();
            this.txtSDTKhachHang.Name = "txtSDTKhachHang";
            this.txtSDTKhachHang.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.txtSearchMenu = new System.Windows.Forms.TextBox();
            this.txtSearchMenu.Name = "txtSearchMenu";
            this.txtSearchMenu.Font = new System.Drawing.Font("Segoe UI", 11F);

            // Buttons basic setup
            this.btnTimKH = new System.Windows.Forms.Button();
            this.btnTimKH.Name = "btnTimKH";
            this.btnTimKH.Text = "🔍 Tìm kiếm";
            this.btnTimKH.Click += new System.EventHandler(this.btnTimKH_Click);

            this.btnXoaKH = new System.Windows.Forms.Button();
            this.btnXoaKH.Name = "btnXoaKH";
            this.btnXoaKH.Text = "❌ Xóa chọn";
            this.btnXoaKH.Click += new System.EventHandler(this.btnXoaKH_Click);

            this.btnPlusItem = new System.Windows.Forms.Button();
            this.btnPlusItem.Name = "btnPlusItem";
            this.btnPlusItem.Text = "➕";
            this.btnPlusItem.Click += new System.EventHandler(this.btnPlusItem_Click);

            this.btnMinusItem = new System.Windows.Forms.Button();
            this.btnMinusItem.Name = "btnMinusItem";
            this.btnMinusItem.Text = "➖";
            this.btnMinusItem.Click += new System.EventHandler(this.btnMinusItem_Click);

            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Text = "🗑️";
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);

            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Text = "💳 THANH TOÁN";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);

            this.btnHuyDon = new System.Windows.Forms.Button();
            this.btnHuyDon.Name = "btnHuyDon";
            this.btnHuyDon.Text = "❌ HỦY ĐƠN";
            this.btnHuyDon.Click += new System.EventHandler(this.btnHuyDon_Click);

            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Text = "🖨️ In hóa đơn";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Text = "📄 Xuất PDF";
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);

            // Form properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.mainTabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "frmSales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🍔 Hệ thống bán hàng - FastFood Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.mainTabControl.ResumeLayout(false);
            this.tabOrder.ResumeLayout(false);
            this.tabCustomer.ResumeLayout(false);
            this.tabHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            this.ResumeLayout(false);
        }
    

        private void SetupOrderTabContent()
        {
            // Split layout: Menu (left) + Order details (right)
            System.Windows.Forms.SplitContainer mainSplit = new System.Windows.Forms.SplitContainer();
            mainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            mainSplit.SplitterDistance = 800;
            mainSplit.SplitterWidth = 8;
            mainSplit.BackColor = System.Drawing.Color.FromArgb(220, 221, 225);

            // LEFT PANEL: Menu
            this.SetupMenuPanel(mainSplit.Panel1);

            // RIGHT PANEL: Order details + Actions
            this.SetupOrderPanel(mainSplit.Panel2);

            this.tabOrder.Controls.Add(mainSplit);
        }

        private void SetupMenuPanel(System.Windows.Forms.Control parent)
        {
            // Header
            System.Windows.Forms.Panel headerPanel = new System.Windows.Forms.Panel();
            headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            headerPanel.Height = 60;
            headerPanel.BackColor = System.Drawing.Color.White;
            headerPanel.Padding = new System.Windows.Forms.Padding(16);

            System.Windows.Forms.Label titleLabel = new System.Windows.Forms.Label();
            titleLabel.Text = "🍔 THỰC ĐƠN";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            titleLabel.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            titleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            titleLabel.Width = 180;

            // Search box with functionality
            System.Windows.Forms.TextBox searchBox = new System.Windows.Forms.TextBox();
            searchBox.Name = "txtSearchMenu";
            searchBox.Font = new System.Drawing.Font("Segoe UI", 11F);
            searchBox.Width = 280;
            searchBox.Height = 32;
            searchBox.Dock = System.Windows.Forms.DockStyle.Right;
            searchBox.Text = "Tìm kiếm món ăn...";
            searchBox.ForeColor = System.Drawing.Color.Gray;

            // Add event handlers to simulate placeholder behavior
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Tìm kiếm món ăn...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = System.Drawing.Color.Black;
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Tìm kiếm món ăn...";
                    searchBox.ForeColor = System.Drawing.Color.Gray;
                }
            };

            // Add search functionality
            searchBox.TextChanged += (s, e) => {
                string searchText = searchBox.Text.Trim().ToLowerInvariant();
                if (searchText != "tìm kiếm món ăn...")
                {
                    FilterMenuItems(searchText);
                }
            };

            headerPanel.Controls.AddRange(new System.Windows.Forms.Control[] { titleLabel, searchBox });
            this.pnlMonAn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMonAn.AutoScroll = true;
            this.pnlMonAn.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.pnlMonAn.Padding = new System.Windows.Forms.Padding(10); 
            this.pnlMonAn.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlMonAn.WrapContents = true;

            parent.Controls.AddRange(new System.Windows.Forms.Control[] { headerPanel, this.pnlMonAn });
        }

        private void SetupOrderPanel(System.Windows.Forms.Control parent)
        {
            // Header
            System.Windows.Forms.Panel headerPanel = new System.Windows.Forms.Panel();
            headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            headerPanel.Height = 50; // Reduced from 60 to 50
            headerPanel.BackColor = System.Drawing.Color.White;
            headerPanel.Padding = new System.Windows.Forms.Padding(12); // Reduced padding

            System.Windows.Forms.Label titleLabel = new System.Windows.Forms.Label();
            titleLabel.Text = "🛒 ĐƠN HÀNG HIỆN TẠI";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold); // Reduced from 14F to 12F
            titleLabel.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
            titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            headerPanel.Controls.Add(titleLabel);

            // Setup DataGridView styling
            this.SetupOrderListColumns();

            // Footer with total and actions
            System.Windows.Forms.Panel footerPanel = this.CreateOrderFooter();
            footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            footerPanel.Height = 150;

            parent.Controls.AddRange(new System.Windows.Forms.Control[] { headerPanel, this.dgvOrderList, footerPanel });
        }

        private void SetupOrderListColumns()
        {
            this.dgvOrderList.Columns.Clear();
            
            // Hidden MaMon column
            this.dgvOrderList.Columns.Add("MaMon", "Mã Món");
            this.dgvOrderList.Columns["MaMon"].Visible = false;

            // Tên món
            this.dgvOrderList.Columns.Add("TenMon", "🍽️ Tên món");
            this.dgvOrderList.Columns["TenMon"].Width = 200;
            this.dgvOrderList.Columns["TenMon"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            // Số lượng
            this.dgvOrderList.Columns.Add("SoLuong", "SL");
            this.dgvOrderList.Columns["SoLuong"].Width = 60;
            this.dgvOrderList.Columns["SoLuong"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvOrderList.Columns["SoLuong"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);

            // Đơn giá
            this.dgvOrderList.Columns.Add("DonGia", "💰 Đơn giá");
            this.dgvOrderList.Columns["DonGia"].Width = 100;
            this.dgvOrderList.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            this.dgvOrderList.Columns["DonGia"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // Thành tiền
            this.dgvOrderList.Columns.Add("ThanhTien", "💵 Thành tiền");
            this.dgvOrderList.Columns["ThanhTien"].Width = 120;
            this.dgvOrderList.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            this.dgvOrderList.Columns["ThanhTien"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvOrderList.Columns["ThanhTien"].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 248, 220);

            // Apply modern styling
            this.ApplyModernGridStyle(this.dgvOrderList, "success");
        }

        private System.Windows.Forms.Panel CreateOrderFooter()
        {
            System.Windows.Forms.Panel footer = new System.Windows.Forms.Panel();
            footer.BackColor = System.Drawing.Color.White;
            footer.Padding = new System.Windows.Forms.Padding(16);

            // Customer section
            System.Windows.Forms.Panel customerSection = new System.Windows.Forms.Panel();
            customerSection.Dock = System.Windows.Forms.DockStyle.Top;
            customerSection.Height = 60;
            customerSection.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);

            // Customer info panel
            System.Windows.Forms.Panel customerInfoPanel = new System.Windows.Forms.Panel();
            customerInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            customerInfoPanel.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            customerInfoPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);

            System.Windows.Forms.Label lblCustomerIcon = new System.Windows.Forms.Label();
            lblCustomerIcon.Text = "👤";
            lblCustomerIcon.Font = new System.Drawing.Font("Segoe UI", 12F);
            lblCustomerIcon.Location = new System.Drawing.Point(12, 12);
            lblCustomerIcon.Size = new System.Drawing.Size(30, 25);

            System.Windows.Forms.Label lblCustomerTitle = new System.Windows.Forms.Label();
            lblCustomerTitle.Text = "Khách hàng:";
            lblCustomerTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblCustomerTitle.Location = new System.Drawing.Point(45, 12);
            lblCustomerTitle.Size = new System.Drawing.Size(85, 25);
            lblCustomerTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            // Use the existing lblTenKhachHang instead of creating a new one
            this.lblTenKhachHang.Text = "Khách lẻ";
            this.lblTenKhachHang.Location = new System.Drawing.Point(135, 12);
            this.lblTenKhachHang.Size = new System.Drawing.Size(200, 25);
            this.lblTenKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTenKhachHang.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);

            System.Windows.Forms.Button btnSelectCustomer = new System.Windows.Forms.Button();
            btnSelectCustomer.Text = "Chọn KH";
            btnSelectCustomer.Size = new System.Drawing.Size(80, 30);
            btnSelectCustomer.Location = new System.Drawing.Point(345, 10);
            btnSelectCustomer.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnSelectCustomer.ForeColor = System.Drawing.Color.White;
            btnSelectCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSelectCustomer.FlatAppearance.BorderSize = 0;
            btnSelectCustomer.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnSelectCustomer.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSelectCustomer.Click += (s, e) => mainTabControl.SelectedTab = tabCustomer;

            customerInfoPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblCustomerIcon, lblCustomerTitle, this.lblTenKhachHang, btnSelectCustomer
            });

            customerSection.Controls.Add(customerInfoPanel);

            // Order actions section
            System.Windows.Forms.Panel orderActionsSection = new System.Windows.Forms.Panel();
            orderActionsSection.Dock = System.Windows.Forms.DockStyle.Top;
            orderActionsSection.Height = 50;
            orderActionsSection.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);

            System.Windows.Forms.FlowLayoutPanel orderActionsFlow = new System.Windows.Forms.FlowLayoutPanel();
            orderActionsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            orderActionsFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            orderActionsFlow.WrapContents = false;
            orderActionsFlow.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);

            // Item quantity buttons
            this.btnPlusItem = this.CreateActionButton("➕", System.Drawing.Color.FromArgb(46, 204, 113), "Tăng số lượng");
            this.btnMinusItem = this.CreateActionButton("➖", System.Drawing.Color.FromArgb(230, 126, 34), "Giảm số lượng");
            this.btnRemoveItem = this.CreateActionButton("🗑️", System.Drawing.Color.FromArgb(231, 76, 60), "Xóa món");

            // Spacer
            System.Windows.Forms.Panel spacer = new System.Windows.Forms.Panel();
            spacer.Width = 20;
            spacer.Height = 35;

            orderActionsFlow.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnPlusItem, this.btnMinusItem, this.btnRemoveItem, spacer
            });

            orderActionsSection.Controls.Add(orderActionsFlow);

            // Total section
            System.Windows.Forms.Panel totalSection = new System.Windows.Forms.Panel();
            totalSection.Dock = System.Windows.Forms.DockStyle.Top;
            totalSection.Height = 65;
            totalSection.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            totalSection.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            totalSection.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);

            System.Windows.Forms.Label lblTotalIcon = new System.Windows.Forms.Label();
            lblTotalIcon.Text = "💰";
            lblTotalIcon.Font = new System.Drawing.Font("Segoe UI", 14F);
            lblTotalIcon.ForeColor = System.Drawing.Color.White;
            lblTotalIcon.Location = new System.Drawing.Point(16, 20);
            lblTotalIcon.Size = new System.Drawing.Size(30, 25);

            System.Windows.Forms.Label lblTotalText = new System.Windows.Forms.Label();
            lblTotalText.Text = "TỔNG CỘNG:";
            lblTotalText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTotalText.ForeColor = System.Drawing.Color.White;
            lblTotalText.Location = new System.Drawing.Point(50, 20);
            lblTotalText.Size = new System.Drawing.Size(120, 25);

            // Use the existing lblTongTien from InitializeComponent and configure it properly
            this.lblTongTien.Text = "0 VNĐ";
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.White;
            this.lblTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTongTien.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblTongTien.Location = new System.Drawing.Point(280, 15);
            this.lblTongTien.Size = new System.Drawing.Size(200, 35);

            totalSection.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTotalIcon, lblTotalText, this.lblTongTien
            });

            // Main action buttons section
            System.Windows.Forms.Panel mainActionsSection = new System.Windows.Forms.Panel();
            mainActionsSection.Dock = System.Windows.Forms.DockStyle.Bottom;
            mainActionsSection.Height = 60;
            mainActionsSection.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);

            System.Windows.Forms.FlowLayoutPanel mainActionsFlow = new System.Windows.Forms.FlowLayoutPanel();
            mainActionsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            mainActionsFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            mainActionsFlow.WrapContents = false;
            mainActionsFlow.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);

            this.btnThanhToan.Text = "💳 THANH TOÁN";
            this.btnThanhToan.Size = new System.Drawing.Size(160, 45);
            this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.FlatAppearance.BorderSize = 0;
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnHuyDon.Text = "❌ HỦY ĐƠN";
            this.btnHuyDon.Size = new System.Drawing.Size(130, 45);
            this.btnHuyDon.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnHuyDon.ForeColor = System.Drawing.Color.White;
            this.btnHuyDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuyDon.FlatAppearance.BorderSize = 0;
            this.btnHuyDon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnHuyDon.Margin = new System.Windows.Forms.Padding(0);
            this.btnHuyDon.Cursor = System.Windows.Forms.Cursors.Hand;

            mainActionsFlow.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnThanhToan, this.btnHuyDon
            });

            mainActionsSection.Controls.Add(mainActionsFlow);

            // Add all sections to footer
            footer.Controls.AddRange(new System.Windows.Forms.Control[] {
                customerSection, orderActionsSection, totalSection, mainActionsSection
            });

            return footer;
        }

        private System.Windows.Forms.Button CreateActionButton(string text, System.Drawing.Color backgroundColor, string tooltip = "")
        {
            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
            btn.Text = text;
            btn.Size = new System.Drawing.Size(50, 35);
            btn.BackColor = backgroundColor;
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new System.Drawing.Font("Segoe UI", 12F);
            btn.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;

            // Add tooltip if provided
            if (!string.IsNullOrEmpty(tooltip))
            {
                System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
                toolTip.SetToolTip(btn, tooltip);
            }

            // Add hover effects
            btn.MouseEnter += (s, e) => {
                btn.BackColor = System.Drawing.Color.FromArgb(
                    Math.Min(255, backgroundColor.R + 20),
                    Math.Min(255, backgroundColor.G + 20),
                    Math.Min(255, backgroundColor.B + 20)
                );
            };
            btn.MouseLeave += (s, e) => btn.BackColor = backgroundColor;

            if (text == "➕") btn.Click += new System.EventHandler(this.btnPlusItem_Click);
            if (text == "➖") btn.Click += new System.EventHandler(this.btnMinusItem_Click);
            if (text == "🗑️") btn.Click += new System.EventHandler(this.btnRemoveItem_Click);

            return btn;
        }

        private void SetupCustomerTab()
        {
            // Renamed the duplicate method to avoid conflict
            SetupCustomerTabUI();
        }

        private void SetupCustomerTabUI()
        {
            // Header
            System.Windows.Forms.Panel headerPanel = new System.Windows.Forms.Panel();
            headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = System.Drawing.Color.White;
            headerPanel.Padding = new System.Windows.Forms.Padding(20);

            System.Windows.Forms.Label titleLabel = new System.Windows.Forms.Label();
            titleLabel.Text = "👥 QUẢN LÝ KHÁCH HÀNG";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            titleLabel.ForeColor = System.Drawing.Color.FromArgb(155, 89, 182);
            titleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            headerPanel.Controls.Add(titleLabel);

            // Search panel
            System.Windows.Forms.Panel searchPanel = new System.Windows.Forms.Panel();
            searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            searchPanel.Height = 60;
            searchPanel.BackColor = System.Drawing.Color.White;
            searchPanel.Padding = new System.Windows.Forms.Padding(20);

            this.txtSDTKhachHang = new System.Windows.Forms.TextBox();
            this.txtSDTKhachHang.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSDTKhachHang.Size = new System.Drawing.Size(300, 35);
            this.txtSDTKhachHang.Location = new System.Drawing.Point(20, 15);

            this.btnTimKH = new System.Windows.Forms.Button();
            this.btnTimKH.Text = "🔍 Tìm kiếm";
            this.btnTimKH.Size = new System.Drawing.Size(100, 35);
            this.btnTimKH.Location = new System.Drawing.Point(330, 15);
            this.btnTimKH.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnTimKH.ForeColor = System.Drawing.Color.White;
            this.btnTimKH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKH.Click += new System.EventHandler(this.btnTimKH_Click);

            this.btnXoaKH = new System.Windows.Forms.Button();
            this.btnXoaKH.Text = "❌ Xóa chọn";
            this.btnXoaKH.Size = new System.Drawing.Size(100, 35);
            this.btnXoaKH.Location = new System.Drawing.Point(440, 15);
            this.btnXoaKH.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnXoaKH.ForeColor = System.Drawing.Color.White;
            this.btnXoaKH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaKH.Click += new System.EventHandler(this.btnXoaKH_Click);

            searchPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.txtSDTKhachHang, this.btnTimKH, this.btnXoaKH
            });

            this.SetupCustomerGridColumns();
            this.ApplyModernGridStyle(this.dgvKhachHang, "warning");

            this.tabCustomer.Controls.AddRange(new System.Windows.Forms.Control[] { headerPanel, searchPanel });
        }

        private void SetupHistoryTab()
        {
            // Renamed the duplicate method to avoid conflict
            SetupHistoryTabUI();
        }

        private void SetupHistoryTabUI()
        {
            // Header
            System.Windows.Forms.Panel headerPanel = new System.Windows.Forms.Panel();
            headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = System.Drawing.Color.White;
            headerPanel.Padding = new System.Windows.Forms.Padding(20);

            System.Windows.Forms.Label titleLabel = new System.Windows.Forms.Label();
            titleLabel.Text = "📋 LỊCH SỬ ĐƠN HÀNG";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            titleLabel.ForeColor = System.Drawing.Color.FromArgb(230, 126, 34);
            titleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            headerPanel.Controls.Add(titleLabel);

            // Action panel
            System.Windows.Forms.Panel actionPanel = new System.Windows.Forms.Panel();
            actionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            actionPanel.Height = 60;
            actionPanel.BackColor = System.Drawing.Color.White;
            actionPanel.Padding = new System.Windows.Forms.Padding(20);

            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPrint.Text = "🖨️ In hóa đơn";
            this.btnPrint.Size = new System.Drawing.Size(120, 40);
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnExportPdf.Text = "📄 Xuất PDF";
            this.btnExportPdf.Size = new System.Drawing.Size(120, 40);
            this.btnExportPdf.Location = new System.Drawing.Point(140, 10);
            this.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnExportPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);

            actionPanel.Controls.AddRange(new System.Windows.Forms.Control[] { this.btnPrint, this.btnExportPdf });

            this.SetupHistoryGridColumns();
            this.ApplyModernGridStyle(this.dgvHoaDon, "primary");

            this.tabHistory.Controls.AddRange(new System.Windows.Forms.Control[] { headerPanel, actionPanel });
        }

        private void SetupCustomerGridColumns()
        {
            this.dgvKhachHang.Columns.Clear();
            this.dgvKhachHang.Columns.Add("MaKH", "ID");
            this.dgvKhachHang.Columns.Add("TenKH", "👤 Tên khách hàng");
            this.dgvKhachHang.Columns.Add("SDT", "📱 Số điện thoại");
            this.dgvKhachHang.Columns.Add("DiaChi", "🏠 Địa chỉ");
            this.dgvKhachHang.Columns.Add("Email", "📧 Email");
            
            this.dgvKhachHang.Columns["MaKH"].Visible = false;
            this.dgvKhachHang.Columns["TenKH"].Width = 200;
            this.dgvKhachHang.Columns["SDT"].Width = 150;
            this.dgvKhachHang.Columns["DiaChi"].Width = 250;
            this.dgvKhachHang.Columns["Email"].Width = 200;
        }

        private void SetupHistoryGridColumns()
        {
            this.dgvHoaDon.Columns.Clear();
            this.dgvHoaDon.Columns.Add("MaDH", "ID");
            this.dgvHoaDon.Columns.Add("NgayLap", "📅 Ngày lập");
            this.dgvHoaDon.Columns.Add("TenKH", "👤 Khách hàng");
            this.dgvHoaDon.Columns.Add("TongTien", "💰 Tổng tiền");
            this.dgvHoaDon.Columns.Add("TrangThaiThanhToan", "📊 Trạng thái");
            
            this.dgvHoaDon.Columns["MaDH"].Width = 80;
            this.dgvHoaDon.Columns["NgayLap"].Width = 150;
            this.dgvHoaDon.Columns["TenKH"].Width = 200;
            this.dgvHoaDon.Columns["TongTien"].Width = 150;
            this.dgvHoaDon.Columns["TrangThaiThanhToan"].Width = 120;
            
            this.dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            this.dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
        }

        private void ApplyModernGridStyle(System.Windows.Forms.DataGridView grid, string theme)
        {
            System.Drawing.Color headerColor, accentColor;

            switch (theme)
            {
                case "success":
                    headerColor = System.Drawing.Color.FromArgb(46, 204, 113);
                    accentColor = System.Drawing.Color.FromArgb(26, 188, 156);
                    break;
                case "warning":
                    headerColor = System.Drawing.Color.FromArgb(230, 126, 34);
                    accentColor = System.Drawing.Color.FromArgb(243, 156, 18);
                    break;
                case "danger":
                    headerColor = System.Drawing.Color.FromArgb(231, 76, 60);
                    accentColor = System.Drawing.Color.FromArgb(192, 57, 43);
                    break;
                default:
                    headerColor = System.Drawing.Color.FromArgb(52, 152, 219);
                    accentColor = System.Drawing.Color.FromArgb(41, 128, 185);
                    break;
            }

            // Header style
            grid.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = headerColor,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = headerColor,
                Padding = new System.Windows.Forms.Padding(8, 8, 8, 8)
            };

            // Cell style
            grid.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.FromArgb(44, 62, 80),
                Font = new System.Drawing.Font("Segoe UI", 10F),
                SelectionBackColor = System.Drawing.Color.FromArgb(174, 214, 241),
                SelectionForeColor = System.Drawing.Color.FromArgb(44, 62, 80),
                Padding = new System.Windows.Forms.Padding(8, 4, 8, 4)
            };

            // Alternating row style
            grid.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(248, 249, 250),
                SelectionBackColor = System.Drawing.Color.FromArgb(174, 214, 241)
            };
        }
    }
}

using PM_Ban_Do_An_Nhanh.BLL;
using PM_Ban_Do_An_Nhanh.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace PM_Ban_Do_An_Nhanh
{
    public partial class frmSales : Form
    {
        private MonAnBLL monAnBLL = new MonAnBLL();
        private DonHangBLL donHangBLL = new DonHangBLL();
        private KhachHangBLL khachHangBLL = new KhachHangBLL();

        private List<ChiTietDonHang> currentOrderItems = new List<ChiTietDonHang>();
        private KhachHang selectedCustomer = null;
        private int? selectedMaDH = null;

        private bool isProcessingPayment = false;
        private DateTime lastPaymentTime = DateTime.MinValue;

        public frmSales()
        {
            InitializeComponent();
            this.Text = "🍔 Hệ thống bán hàng - FastFood Manager";
            this.Load += frmSales_Load;

            // Test database connection first
            if (!TestDatabaseConnection())
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu. Vui lòng kiểm tra lại cấu hình.",
                               "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load data when switching tabs
            mainTabControl.SelectedIndexChanged += MainTabControl_SelectedIndexChanged;
        }

        private bool TestDatabaseConnection()
        {
            try
            {
                return DBConnection.TestConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra kết nối database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (mainTabControl.SelectedIndex)
            {
                case 1: // Customer tab
                    LoadKhachHangToGrid();
                    break;
                case 2: // History tab
                    LoadHoaDonToGrid();
                    break;
            }
        }

        private void frmSales_Load(object sender, EventArgs e)
        {
            // Setup UI sau khi form được load
            SetupOrderTab();
            SetupCustomerTabContent();
            SetupHistoryTabContent();
            LoadHoaDonToGrid();
        }

        private void SetupOrderTab()
        {
            try
            {
                // Create main split container for Order tab
                SplitContainer mainSplit = new SplitContainer();
                mainSplit.Dock = DockStyle.Fill;
                mainSplit.SplitterDistance = 800;
                mainSplit.SplitterWidth = 8;
                mainSplit.BackColor = Color.FromArgb(220, 221, 225);
                mainSplit.Orientation = Orientation.Vertical;

                // Setup left panel (Menu)
                SetupMenuPanelContent(mainSplit.Panel1);

                // Setup right panel (Order details)
                SetupOrderPanelContent(mainSplit.Panel2);

                tabOrder.Controls.Clear();
                tabOrder.Controls.Add(mainSplit);

                // Load data after UI is set up
                LoadMonAnToPanel();
                SetupOrderDataGridView();
                ClearOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thiết lập tab đặt hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupMenuPanelContent(Control parent)
        {
            // Header panel for menu
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(parent.Width, 70);
            headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            headerPanel.BackColor = Color.White;
            headerPanel.Padding = new Padding(16);

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "🍔 THỰC ĐƠN";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(52, 152, 219);
            titleLabel.Location = new Point(16, 20);
            titleLabel.Size = new Size(180, 30);
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Search box
            txtSearchMenu.Location = new Point(parent.Width - 300, 19);
            txtSearchMenu.Size = new Size(280, 32);
            txtSearchMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearchMenu.Text = "Tìm kiếm món ăn...";
            txtSearchMenu.ForeColor = Color.Gray;

            // Add placeholder behavior for search
            txtSearchMenu.GotFocus += (s, e) =>
            {
                if (txtSearchMenu.Text == "Tìm kiếm món ăn...")
                {
                    txtSearchMenu.Text = "";
                    txtSearchMenu.ForeColor = Color.Black;
                }
            };

            txtSearchMenu.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchMenu.Text))
                {
                    txtSearchMenu.Text = "Tìm kiếm món ăn...";
                    txtSearchMenu.ForeColor = Color.Gray;
                }
            };

            txtSearchMenu.TextChanged += (s, e) =>
            {
                string searchText = txtSearchMenu.Text.Trim().ToLowerInvariant();
                if (searchText != "tìm kiếm món ăn...")
                {
                    FilterMenuItems(searchText);
                }
            };

            headerPanel.Controls.AddRange(new Control[] { titleLabel, txtSearchMenu });

            // Menu panel
            pnlMonAn.Location = new Point(0, 85); // 70px header + 15px margin
            pnlMonAn.Size = new Size(parent.Width, parent.Height - 85);
            pnlMonAn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlMonAn.AutoScroll = true;
            pnlMonAn.BackColor = Color.FromArgb(248, 249, 250);
            pnlMonAn.Padding = new Padding(15, 15, 15, 15);
            pnlMonAn.FlowDirection = FlowDirection.LeftToRight;
            pnlMonAn.WrapContents = true;

            // Add controls to parent
            parent.Controls.AddRange(new Control[] { headerPanel, pnlMonAn });

            // Handle parent resize
            parent.Resize += (s, e) =>
            {
                headerPanel.Size = new Size(parent.Width, 70);
                txtSearchMenu.Location = new Point(parent.Width - 300, 19);
                pnlMonAn.Size = new Size(parent.Width, parent.Height - 85);
            };
        }

        private void SetupCustomerTabContent()
        {
            // Clear existing controls first
            tabCustomer.Controls.Clear();

            // Header panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(tabCustomer.Width, 80);
            headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            headerPanel.BackColor = Color.White;
            headerPanel.Padding = new Padding(20);

            Label titleLabel = new Label();
            titleLabel.Text = "👥 QUẢN LÝ KHÁCH HÀNG";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(155, 89, 182);
            titleLabel.Location = new Point(20, 25);
            titleLabel.Size = new Size(400, 30);
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;

            headerPanel.Controls.Add(titleLabel);

            // Search panel
            Panel searchPanel = new Panel();
            searchPanel.Location = new Point(0, 80);
            searchPanel.Size = new Size(tabCustomer.Width, 60);
            searchPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchPanel.BackColor = Color.White;
            searchPanel.Padding = new Padding(20);

            txtSDTKhachHang.Size = new Size(300, 35);
            txtSDTKhachHang.Location = new Point(20, 15);

            btnTimKH.Size = new Size(100, 35);
            btnTimKH.Location = new Point(330, 15);
            btnTimKH.BackColor = Color.FromArgb(52, 152, 219);
            btnTimKH.ForeColor = Color.White;
            btnTimKH.FlatStyle = FlatStyle.Flat;

            btnXoaKH.Size = new Size(100, 35);
            btnXoaKH.Location = new Point(440, 15);
            btnXoaKH.BackColor = Color.FromArgb(231, 76, 60);
            btnXoaKH.ForeColor = Color.White;
            btnXoaKH.FlatStyle = FlatStyle.Flat;

            searchPanel.Controls.AddRange(new Control[] {
                txtSDTKhachHang, btnTimKH, btnXoaKH
            });

            // DataGridView
            dgvKhachHang.Location = new Point(0, 155); // 80px header + 60px search + 15px margin
            dgvKhachHang.Size = new Size(tabCustomer.Width, tabCustomer.Height - 155);
            dgvKhachHang.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Add all controls to tab
            tabCustomer.Controls.AddRange(new Control[] { headerPanel, searchPanel, dgvKhachHang });

            // Handle tab resize
            tabCustomer.Resize += (s, e) =>
            {
                headerPanel.Size = new Size(tabCustomer.Width, 80);
                searchPanel.Size = new Size(tabCustomer.Width, 60);
                dgvKhachHang.Size = new Size(tabCustomer.Width, tabCustomer.Height - 155);
            };
        }

        private void SetupOrderPanelContent(Control parent)
        {
            // Header
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 50;
            headerPanel.BackColor = Color.White;
            headerPanel.Padding = new Padding(12);

            Label titleLabel = new Label();
            titleLabel.Text = "🛒 ĐƠN HÀNG HIỆN TẠI";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(46, 204, 113);
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;

            headerPanel.Controls.Add(titleLabel);

            // Footer with actions (increased height to include total section)
            Panel footerPanel = CreateOrderFooterContent();
            footerPanel.Dock = DockStyle.Bottom;
            footerPanel.Height = 240;

            // DataGridView for order list
            dgvOrderList.Dock = DockStyle.Fill;

            parent.Controls.AddRange(new Control[] { headerPanel, dgvOrderList, footerPanel });
        }

        private Panel CreateOrderFooterContent()
        {
            Panel footer = new Panel();
            footer.BackColor = Color.White;
            footer.Padding = new Padding(16);

            // Total section (positioned after order list, before action buttons)
            Panel totalSection = new Panel();
            totalSection.Dock = DockStyle.Top;
            totalSection.Height = 60;
            totalSection.BackColor = Color.FromArgb(46, 204, 113);
            totalSection.Padding = new Padding(16, 12, 16, 12);

            Label lblTotalTitle = new Label();
            lblTotalTitle.Text = "💰 TỔNG CỘNG:";
            lblTotalTitle.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalTitle.ForeColor = Color.White;
            lblTotalTitle.Location = new Point(16, 8);
            lblTotalTitle.Size = new Size(150, 20);
            lblTotalTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblTongTien.Text = "0 VNĐ";
            lblTongTien.Font = new System.Drawing.Font("Segoe UI", 16F, FontStyle.Bold);
            lblTongTien.ForeColor = Color.White;
            lblTongTien.Location = new Point(16, 28);
            lblTongTien.Size = new Size(300, 24);
            lblTongTien.TextAlign = ContentAlignment.MiddleLeft;

            totalSection.Controls.AddRange(new Control[] { lblTotalTitle, lblTongTien });

            // Customer section
            Panel customerSection = new Panel();
            customerSection.Dock = DockStyle.Top;
            customerSection.Height = 50;
            customerSection.BackColor = Color.FromArgb(248, 249, 250);
            customerSection.Padding = new Padding(12, 8, 12, 8);

            Label lblCustomerIcon = new Label();
            lblCustomerIcon.Text = "👤";
            lblCustomerIcon.Font = new System.Drawing.Font("Segoe UI", 12F);
            lblCustomerIcon.Location = new Point(12, 12);
            lblCustomerIcon.Size = new Size(30, 25);

            Label lblCustomerTitle = new Label();
            lblCustomerTitle.Text = "Khách hàng:";
            lblCustomerTitle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            lblCustomerTitle.Location = new Point(45, 12);
            lblCustomerTitle.Size = new Size(85, 25);
            lblCustomerTitle.ForeColor = Color.FromArgb(44, 62, 80);

            lblTenKhachHang.Location = new Point(135, 12);
            lblTenKhachHang.Size = new Size(200, 25);
            lblTenKhachHang.ForeColor = Color.FromArgb(127, 140, 141);

            Button btnSelectCustomer = new Button();
            btnSelectCustomer.Text = "Chọn KH";
            btnSelectCustomer.Size = new Size(80, 30);
            btnSelectCustomer.Location = new Point(345, 10);
            btnSelectCustomer.BackColor = Color.FromArgb(52, 152, 219);
            btnSelectCustomer.ForeColor = Color.White;
            btnSelectCustomer.FlatStyle = FlatStyle.Flat;
            btnSelectCustomer.FlatAppearance.BorderSize = 0;
            btnSelectCustomer.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnSelectCustomer.Cursor = Cursors.Hand;
            btnSelectCustomer.Click += (s, e) => mainTabControl.SelectedTab = tabCustomer;

            customerSection.Controls.AddRange(new Control[] {
                lblCustomerIcon, lblCustomerTitle, lblTenKhachHang, btnSelectCustomer
            });

            // Order actions section
            Panel orderActionsSection = new Panel();
            orderActionsSection.Dock = DockStyle.Top;
            orderActionsSection.Height = 50;
            orderActionsSection.Padding = new Padding(0, 8, 0, 8);

            FlowLayoutPanel orderActionsFlow = new FlowLayoutPanel();
            orderActionsFlow.Dock = DockStyle.Fill;
            orderActionsFlow.FlowDirection = FlowDirection.LeftToRight;
            orderActionsFlow.WrapContents = false;
            orderActionsFlow.Padding = new Padding(4, 0, 4, 0);

            // Style action buttons
            StyleActionButton(btnPlusItem, Color.FromArgb(46, 204, 113), "Tăng số lượng");
            StyleActionButton(btnMinusItem, Color.FromArgb(230, 126, 34), "Giảm số lượng");
            StyleActionButton(btnRemoveItem, Color.FromArgb(231, 76, 60), "Xóa món");

            orderActionsFlow.Controls.AddRange(new Control[] {
                btnPlusItem, btnMinusItem, btnRemoveItem
            });

            orderActionsSection.Controls.Add(orderActionsFlow);

            // Main action buttons section
            Panel mainActionsSection = new Panel();
            mainActionsSection.Dock = DockStyle.Bottom;
            mainActionsSection.Height = 60;
            mainActionsSection.Padding = new Padding(0, 12, 0, 0);

            FlowLayoutPanel mainActionsFlow = new FlowLayoutPanel();
            mainActionsFlow.Dock = DockStyle.Fill;
            mainActionsFlow.FlowDirection = FlowDirection.LeftToRight;
            mainActionsFlow.WrapContents = false;
            mainActionsFlow.Padding = new Padding(4, 0, 4, 0);

            // Style main action buttons
            btnThanhToan.Size = new Size(160, 45);
            btnThanhToan.BackColor = Color.FromArgb(46, 204, 113);
            btnThanhToan.ForeColor = Color.White;
            btnThanhToan.FlatStyle = FlatStyle.Flat;
            btnThanhToan.FlatAppearance.BorderSize = 0;
            btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 11F, FontStyle.Bold);
            btnThanhToan.Margin = new Padding(0, 0, 12, 0);
            btnThanhToan.Cursor = Cursors.Hand;

            btnHuyDon.Size = new Size(130, 45);
            btnHuyDon.BackColor = Color.FromArgb(231, 76, 60);
            btnHuyDon.ForeColor = Color.White;
            btnHuyDon.FlatStyle = FlatStyle.Flat;
            btnHuyDon.FlatAppearance.BorderSize = 0;
            btnHuyDon.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            btnHuyDon.Cursor = Cursors.Hand;

            mainActionsFlow.Controls.AddRange(new Control[] {
                btnThanhToan, btnHuyDon
            });

            mainActionsSection.Controls.Add(mainActionsFlow);

            // Add sections in correct order for proper display
            // First add bottom-docked controls 
            footer.Controls.Add(mainActionsSection);  // Will be at bottom
            
            // Then add top-docked controls (in order since first added appears at top)
            footer.Controls.Add(customerSection);     // Will be at top
            footer.Controls.Add(totalSection);        // Will be below customer section
            footer.Controls.Add(orderActionsSection); // Will be below total section

            return footer;
        }

        private void StyleActionButton(Button btn, Color backgroundColor, string tooltip = "")
        {
            btn.Size = new Size(50, 35);
            btn.BackColor = backgroundColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new System.Drawing.Font("Segoe UI", 12F);
            btn.Margin = new Padding(0, 0, 12, 0);
            btn.Cursor = Cursors.Hand;

            // Add tooltip if provided
            if (!string.IsNullOrEmpty(tooltip))
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(btn, tooltip);
            }

            // Add hover effects
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(
                    Math.Min(255, backgroundColor.R + 20),
                    Math.Min(255, backgroundColor.G + 20),
                    Math.Min(255, backgroundColor.B + 20)
                );
            };
            btn.MouseLeave += (s, e) => btn.BackColor = backgroundColor;
        }

        private void SetupHistoryTabContent()
        {
            // Clear existing controls first
            tabHistory.Controls.Clear();

            // Header panel
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(tabHistory.Width, 80);
            headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            headerPanel.BackColor = Color.White;
            headerPanel.Padding = new Padding(20);

            Label titleLabel = new Label();
            titleLabel.Text = "📋 LỊCH SỬ ĐƠN HÀNG";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(230, 126, 34);
            titleLabel.Location = new Point(20, 25);
            titleLabel.Size = new Size(400, 30);
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;

            headerPanel.Controls.Add(titleLabel);

            // Action panel
            Panel actionPanel = new Panel();
            actionPanel.Location = new Point(0, tabHistory.Height - 60);
            actionPanel.Size = new Size(tabHistory.Width, 60);
            actionPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            actionPanel.BackColor = Color.White;
            actionPanel.Padding = new Padding(20);

            btnPrint.Size = new Size(120, 40);
            btnPrint.Location = new Point(20, 10);
            btnPrint.BackColor = Color.FromArgb(52, 152, 219);
            btnPrint.ForeColor = Color.White;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new System.Drawing.Font("Segoe UI", 10F);

            btnExportPdf.Size = new Size(120, 40);
            btnExportPdf.Location = new Point(150, 10);
            btnExportPdf.BackColor = Color.FromArgb(46, 204, 113);
            btnExportPdf.ForeColor = Color.White;
            btnExportPdf.FlatStyle = FlatStyle.Flat;
            btnExportPdf.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Add Delete All Orders button
            Button btnXoaTatCaHoaDon = new Button();
            btnXoaTatCaHoaDon.Text = "Xóa tất cả HĐ";
            btnXoaTatCaHoaDon.Size = new Size(130, 40);
            btnXoaTatCaHoaDon.Location = new Point(280, 10);
            btnXoaTatCaHoaDon.BackColor = Color.FromArgb(231, 76, 60);
            btnXoaTatCaHoaDon.ForeColor = Color.White;
            btnXoaTatCaHoaDon.FlatStyle = FlatStyle.Flat;
            btnXoaTatCaHoaDon.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnXoaTatCaHoaDon.Click += btnXoaDonHangTrungLap_Click;

            actionPanel.Controls.AddRange(new Control[] { btnPrint, btnExportPdf, btnXoaTatCaHoaDon });

            // DataGridView
            dgvHoaDon.Location = new Point(0, 95); // 80px header + 15px margin
            dgvHoaDon.Size = new Size(tabHistory.Width, tabHistory.Height - 155); // 95px top + 60px bottom
            dgvHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Add all controls to tab
            tabHistory.Controls.AddRange(new Control[] { headerPanel, actionPanel, dgvHoaDon });

            // Handle tab resize
            tabHistory.Resize += (s, e) =>
            {
                headerPanel.Size = new Size(tabHistory.Width, 80);
                actionPanel.Location = new Point(0, tabHistory.Height - 60);
                actionPanel.Size = new Size(tabHistory.Width, 60);
                dgvHoaDon.Size = new Size(tabHistory.Width, tabHistory.Height - 155);
            };
        }

        private void LoadMonAnToPanel()
        {
            pnlMonAn.Controls.Clear();
            try
            {
                DataTable dtMonAn = monAnBLL.HienThiDanhSachMonAn();

                foreach (DataRow row in dtMonAn.Rows)
                {
                    var card = new MenuItemCard();
                    int maMon = Convert.ToInt32(row["MaMon"]);
                    string tenMon = row["TenMon"].ToString();
                    decimal gia = Convert.ToDecimal(row["Gia"]);
                    string imagePath = row.Table.Columns.Contains("HinhAnh") ? row["HinhAnh"].ToString() : null;

                    // Nếu chưa có đường dẫn ảnh, dùng hàm tìm ảnh cũ
                    if (string.IsNullOrEmpty(imagePath))
                        imagePath = FindImageForMon(tenMon);

                    card.SetData(maMon, tenMon, gia, imagePath);
                    card.Width = 240;
                    card.Height = 104;
                    card.Margin = new Padding(8, 8, 8, 8);

                    if (row["TrangThai"].ToString() == "Hết hàng")
                    {
                        card.Enabled = false;
                        card.BackColor = Color.LightGray;
                    }

                    card.AddClicked += (s, e) =>
                    {
                        AddItemToOrderList(e.MaMon, e.TenMon, e.Price, e.Quantity);
                        mainTabControl.SelectedTab = tabOrder;
                    };

                    pnlMonAn.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Force scroll về top và refresh layout sau khi add cards
            pnlMonAn.VerticalScroll.Value = 0;
            pnlMonAn.PerformLayout();
            pnlMonAn.Refresh();
        }

        private void SetupOrderDataGridView()
        {
            dgvOrderList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrderList.Columns.Clear();

            // Hidden MaMon column
            dgvOrderList.Columns.Add("MaMon", "Mã Món");
            dgvOrderList.Columns["MaMon"].Visible = false;

            // Tên món
            dgvOrderList.Columns.Add("TenMon", "🍽️ Tên món");
            dgvOrderList.Columns["TenMon"].Width = 200;
            dgvOrderList.Columns["TenMon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Số lượng
            dgvOrderList.Columns.Add("SoLuong", "SL");
            dgvOrderList.Columns["SoLuong"].Width = 60;
            dgvOrderList.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOrderList.Columns["SoLuong"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11F, FontStyle.Bold);

            // Đơn giá
            dgvOrderList.Columns.Add("DonGia", "💰 Đơn giá");
            dgvOrderList.Columns["DonGia"].Width = 100;
            dgvOrderList.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvOrderList.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Thành tiền
            dgvOrderList.Columns.Add("ThanhTien", "💵 Thành tiền");
            dgvOrderList.Columns["ThanhTien"].Width = 120;
            dgvOrderList.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            dgvOrderList.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderList.Columns["ThanhTien"].DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            dgvOrderList.AllowUserToAddRows = false;
            dgvOrderList.ReadOnly = true;
        }

        private void AddItemToOrderList(int maMon, string tenMon, decimal donGia)
        {
            ChiTietDonHang existingItem = currentOrderItems.FirstOrDefault(item => item.MaMon == maMon);

            if (existingItem != null)
            {
                existingItem.SoLuong++;
            }
            else
            {
                currentOrderItems.Add(new ChiTietDonHang
                {
                    MaMon = maMon,
                    TenMon = tenMon,
                    SoLuong = 1,
                    DonGia = donGia
                });
            }
            UpdateOrderDisplay();
        }

        private void AddItemToOrderList(int maMon, string tenMon, decimal donGia, int quantity)
        {
            ChiTietDonHang existingItem = currentOrderItems.FirstOrDefault(item => item.MaMon == maMon);

            if (existingItem != null)
            {
                existingItem.SoLuong += quantity;
            }
            else
            {
                currentOrderItems.Add(new ChiTietDonHang
                {
                    MaMon = maMon,
                    TenMon = tenMon,
                    SoLuong = quantity,
                    DonGia = donGia
                });
            }
            UpdateOrderDisplay();
        }

        private void UpdateOrderDisplay()
        {
            dgvOrderList.Rows.Clear();
            decimal tongTien = 0;
            
            foreach (var item in currentOrderItems)
            {
                decimal thanhTien = item.SoLuong * item.DonGia;
                dgvOrderList.Rows.Add(item.MaMon, item.TenMon, item.SoLuong, item.DonGia, thanhTien);
                tongTien += thanhTien;
            }
            
            // Cập nhật hiển thị tổng tiền
            lblTongTien.Text = $"{tongTien:N0} VNĐ";
        }

        private void ClearOrder()
        {
            currentOrderItems.Clear();
            UpdateOrderDisplay();
            ClearCustomerInfo();
            // Đặt lại tổng tiền về 0
            lblTongTien.Text = "0 VNĐ";
        }

        private void ClearCustomerInfo()
        {
            txtSDTKhachHang.Clear();
            lblTenKhachHang.Text = "Khách lẻ";
            selectedCustomer = null;
        }

        // Keep all existing event handlers and methods...
        private void btnPlusItem_Click(object sender, EventArgs e)
        {
            if (dgvOrderList.CurrentRow != null)
            {
                int maMon = Convert.ToInt32(dgvOrderList.CurrentRow.Cells["MaMon"].Value);
                ChiTietDonHang item = currentOrderItems.FirstOrDefault(i => i.MaMon == maMon);
                if (item != null)
                {
                    item.SoLuong++;
                    UpdateOrderDisplay();
                }
            }
        }

        private void btnMinusItem_Click(object sender, EventArgs e)
        {
            if (dgvOrderList.CurrentRow != null)
            {
                int maMon = Convert.ToInt32(dgvOrderList.CurrentRow.Cells["MaMon"].Value);
                ChiTietDonHang item = currentOrderItems.FirstOrDefault(i => i.MaMon == maMon);
                if (item != null && item.SoLuong > 1)
                {
                    item.SoLuong--;
                    UpdateOrderDisplay();
                }
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvOrderList.CurrentRow != null && MessageBox.Show("Bạn muốn xóa món này khỏi đơn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int maMon = Convert.ToInt32(dgvOrderList.CurrentRow.Cells["MaMon"].Value);
                currentOrderItems.RemoveAll(i => i.MaMon == maMon);
                UpdateOrderDisplay();
            }
        }

        private void btnHuyDon_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn hủy đơn hàng hiện tại không?", "Hủy đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearOrder();
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            // Prevent double-click
            if (isProcessingPayment)
            {
                MessageBox.Show("Đang xử lý thanh toán, vui lòng đợi...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Prevent rapid payments
            TimeSpan timeSinceLastPayment = DateTime.Now - lastPaymentTime;
            if (timeSinceLastPayment.TotalSeconds < 3)
            {
                MessageBox.Show("Vui lòng đợi ít nhất 3 giây trước khi thực hiện thanh toán tiếp theo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currentOrderItems.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm món ăn vào đơn hàng để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tongTien = currentOrderItems.Sum(item => item.SoLuong * item.DonGia);
            if (tongTien <= 0)
            {
                MessageBox.Show("Tổng tiền đơn hàng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string confirmMessage = $"Xác nhận thanh toán đơn hàng:\n" +
                                   $"Tổng tiền: {tongTien:N0} VNĐ\n";

            if (selectedCustomer != null)
            {
                confirmMessage += $"Khách hàng: {selectedCustomer.TenKH}\n";
            }
            else
            {
                confirmMessage += "Khách hàng: Khách lẻ\n";
            }

            confirmMessage += "Bạn có muốn tiếp tục thanh toán?";

            if (MessageBox.Show(confirmMessage, "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Disable button and set processing flag
            isProcessingPayment = true;
            btnThanhToan.Enabled = false;
            btnThanhToan.Text = "Đang xử lý...";

            try
            {
                DonHang donHangMoi = new DonHang
                {
                    NgayLap = DateTime.Now,
                    TongTien = tongTien,
                    TrangThaiThanhToan = "Đã thanh toán",
                    MaKH = selectedCustomer?.MaKH
                };

                foreach (var item in currentOrderItems)
                {
                    if (item.MaMon <= 0 || item.SoLuong <= 0 || item.DonGia <= 0)
                    {
                        MessageBox.Show($"Thông tin món {item.TenMon} không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                int maDonHangMoi = donHangBLL.ThemDonHang(donHangMoi, currentOrderItems);
                if (maDonHangMoi > 0)
                {
                    MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lastPaymentTime = DateTime.Now;
                    InHoaDon(maDonHangMoi);
                    ClearOrder();

                    try
                    {
                        LoadHoaDonToGrid();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Warning: Could not refresh history: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Payment Error Details: {ex}");
            }
            finally
            {
                // Re-enable button and reset processing flag
                isProcessingPayment = false;
                btnThanhToan.Enabled = true;
                btnThanhToan.Text = "Thanh toán";
            }
        }

        private void btnXoaDonHangTrungLap_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tất cả đơn hàng trùng lặp?\n\nChỉ giữ lại đơn hàng đầu tiên của mỗi nhóm trùng lặp.",
                "Xác nhận xóa đơn hàng trùng lặp",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    string result = donHangBLL.XoaDonHangTrungLap();
                    MessageBox.Show(result, "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadHoaDonToGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa đơn hàng trùng lặp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvKhachHang.Rows[e.RowIndex].Cells["MaKH"].Value != null)
            {
                var row = dgvKhachHang.Rows[e.RowIndex];
                selectedCustomer = new KhachHang
                {
                    MaKH = Convert.ToInt32(row.Cells["MaKH"].Value),
                    TenKH = row.Cells["TenKH"].Value?.ToString(),
                    SDT = row.Cells["SDT"].Value?.ToString(),
                    DiaChi = dgvKhachHang.Columns.Contains("DiaChi") ? row.Cells["DiaChi"].Value?.ToString() : "",
                    Email = dgvKhachHang.Columns.Contains("Email") ? row.Cells["Email"].Value?.ToString() : ""
                };

                lblTenKhachHang.Text = $"{selectedCustomer.TenKH} (SĐT: {selectedCustomer.SDT})";

                mainTabControl.SelectedTab = tabOrder;
                MessageBox.Show($"Đã chọn khách hàng: {selectedCustomer.TenKH}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTimKH_Click(object sender, EventArgs e)
        {
            string input = txtSDTKhachHang.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại hoặc tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable dtKhachHang = khachHangBLL.HienThiDanhSachKhachHang();
                var filteredRows = dtKhachHang.AsEnumerable()
                    .Where(r =>
                        r.Field<string>("SDT") == input ||
                        r.Field<string>("TenKH").IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0);

                if (filteredRows.Any())
                {
                    dgvKhachHang.DataSource = filteredRows.CopyToDataTable();
                    dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dgvKhachHang.Columns.Contains("MaKH"))
                        dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                    if (dgvKhachHang.Columns.Contains("TenKH"))
                        dgvKhachHang.Columns["TenKH"].HeaderText = "Tên Khách Hàng";
                    if (dgvKhachHang.Columns.Contains("SDT"))
                        dgvKhachHang.Columns["SDT"].HeaderText = "SĐT";
                    if (dgvKhachHang.Columns.Contains("DiaChi"))
                        dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
                    if (dgvKhachHang.Columns.Contains("Email"))
                        dgvKhachHang.Columns["Email"].HeaderText = "Email";
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvKhachHang.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            ClearCustomerInfo();
            MessageBox.Show("Đã hủy chọn khách hàng cho hóa đơn này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadKhachHangToGrid()
        {
            try
            {
                DataTable dtKhachHang = khachHangBLL.HienThiDanhSachKhachHang();
                dgvKhachHang.DataSource = dtKhachHang;
                dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                dgvKhachHang.Columns["TenKH"].HeaderText = "Tên Khách Hàng";
                dgvKhachHang.Columns["SDT"].HeaderText = "SĐT";
                if (dgvKhachHang.Columns.Contains("DiaChi"))
                    dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if (dgvKhachHang.Columns.Contains("Email"))
                    dgvKhachHang.Columns["Email"].HeaderText = "Email";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHoaDon.Rows[e.RowIndex].Cells["MaDH"].Value != null)
            {
                selectedMaDH = Convert.ToInt32(dgvHoaDon.Rows[e.RowIndex].Cells["MaDH"].Value);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (selectedMaDH == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            InHoaDon(selectedMaDH.Value);
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (selectedMaDH == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xuất PDF.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ExportHoaDonToPdf(selectedMaDH.Value);
        }

        private void FilterMenuItems(string searchText)
        {
            try
            {
                pnlMonAn.Controls.Clear();

                DataTable dtMonAn = monAnBLL.HienThiDanhSachMonAn();

                foreach (DataRow row in dtMonAn.Rows)
                {
                    string tenMon = row["TenMon"].ToString().ToLowerInvariant();

                    if (string.IsNullOrEmpty(searchText) || tenMon.Contains(searchText))
                    {
                        var card = new MenuItemCard();
                        int maMon = Convert.ToInt32(row["MaMon"]);
                        string originalTenMon = row["TenMon"].ToString();
                        decimal gia = Convert.ToDecimal(row["Gia"]);
                        string imagePath = row.Table.Columns.Contains("HinhAnh") ? row["HinhAnh"].ToString() : null;

                        if (string.IsNullOrEmpty(imagePath))
                            imagePath = FindImageForMon(originalTenMon);

                        card.SetData(maMon, originalTenMon, gia, imagePath);
                        card.Width = 240;
                        card.Height = 104;
                        card.Margin = new Padding(6);

                        if (row["TrangThai"].ToString() == "Hết hàng")
                        {
                            card.Enabled = false;
                            card.BackColor = Color.LightGray;
                        }

                        card.AddClicked += (s, e) =>
                        {
                            AddItemToOrderList(e.MaMon, e.TenMon, e.Price, e.Quantity);
                            mainTabControl.SelectedTab = tabOrder;
                        };

                        pnlMonAn.Controls.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Force scroll về top và refresh layout sau khi filter
            pnlMonAn.VerticalScroll.Value = 0;
            pnlMonAn.PerformLayout();
            pnlMonAn.Refresh();
        }

        private string FindImageForMon(string tenMon)
        {
            try
            {
                string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                if (!Directory.Exists(imagesDir)) return null;

                string normalized = RemoveDiacritics(tenMon).ToLowerInvariant();
                normalized = System.Text.RegularExpressions.Regex.Replace(normalized, "[^a-z0-9]", "-");

                foreach (var file in Directory.GetFiles(imagesDir))
                {
                    string name = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                    if (name.Contains(normalized) || normalized.Contains(name))
                        return file;
                }

                var tokens = normalized.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var file in Directory.GetFiles(imagesDir))
                {
                    string name = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                    bool all = tokens.All(t => name.Contains(t));
                    if (all) return file;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private void InHoaDon(int maDH)
        {
            try
            {
                DataTable dtHoaDon = donHangBLL.LayChiTietDonHangChoIn(maDH);

                if (dtHoaDon.Rows.Count > 0)
                {
                    string hoaDonText = "===== HÓA ĐƠN BÁN HÀNG =====\n";
                    hoaDonText += $"Mã HĐ: {dtHoaDon.Rows[0]["MaDH"]}\n";
                    hoaDonText += $"Ngày: {Convert.ToDateTime(dtHoaDon.Rows[0]["NgayLap"]).ToString("dd/MM/yyyy HH:mm")}\n";

                    if (dtHoaDon.Rows[0]["TenKH"] != DBNull.Value)
                    {
                        hoaDonText += $"Khách hàng: {dtHoaDon.Rows[0]["TenKH"]} (SĐT: {dtHoaDon.Rows[0]["SDT_KhachHang"]})\n";
                    }
                    else
                    {
                        hoaDonText += "Khách hàng: Khách lẻ\n";
                    }

                    hoaDonText += "----------------------------------------\n";
                    hoaDonText += string.Format("{0,-25} {1,5} {2,10}\n", "Món", "SL", "TTien");
                    hoaDonText += "----------------------------------------\n";
                    foreach (DataRow row in dtHoaDon.Rows)
                    {
                        hoaDonText += string.Format("{0,-25} {1,5} {2,10:N0}\n",
                                                    row["TenMon"],
                                                    row["SoLuong"],
                                                    Convert.ToDecimal(row["ThanhTien"]));
                    }
                    hoaDonText += "----------------------------------------\n";
                    hoaDonText += $"Tổng cộng: {Convert.ToDecimal(dtHoaDon.Rows[0]["TongTien"]):N0} VNĐ\n";
                    hoaDonText += "========================================\n";
                    hoaDonText += "Cảm ơn quý khách và hẹn gặp lại!\n";

                    MessageBox.Show(hoaDonText, "Hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn để in.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportHoaDonToPdf(int maDH)
        {
            try
            {
                DataTable dtHoaDon = donHangBLL.LayChiTietDonHangChoIn(maDH);

                if (dtHoaDon.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn để in.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"HoaDon_{maDH}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                        pdfDoc.Add(new Paragraph("===== HÓA ĐƠN BÁN HÀNG =====", titleFont));
                        pdfDoc.Add(new Paragraph($"Mã HĐ: {dtHoaDon.Rows[0]["MaDH"]}", normalFont));
                        pdfDoc.Add(new Paragraph($"Ngày: {Convert.ToDateTime(dtHoaDon.Rows[0]["NgayLap"]).ToString("dd/MM/yyyy HH:mm")}", normalFont));

                        if (dtHoaDon.Rows[0]["TenKH"] != DBNull.Value)
                        {
                            pdfDoc.Add(new Paragraph($"Khách hàng: {dtHoaDon.Rows[0]["TenKH"]} (SĐT: {dtHoaDon.Rows[0]["SDT_KhachHang"]})", normalFont));
                        }
                        else
                        {
                            pdfDoc.Add(new Paragraph("Khách hàng: Khách lẻ", normalFont));
                        }

                        pdfDoc.Add(new Paragraph(" "));

                        PdfPTable table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 2, 1, 1 });

                        table.AddCell("Món");
                        table.AddCell("SL");
                        table.AddCell("Thành Tiền");

                        foreach (DataRow row in dtHoaDon.Rows)
                        {
                            table.AddCell(row["TenMon"].ToString());
                            table.AddCell(row["SoLuong"].ToString());
                            table.AddCell(string.Format("{0:N0}", Convert.ToDecimal(row["ThanhTien"])));
                        }

                        pdfDoc.Add(table);
                        pdfDoc.Add(new Paragraph(" "));
                        pdfDoc.Add(new Paragraph($"Tổng cộng: {Convert.ToDecimal(dtHoaDon.Rows[0]["TongTien"]):N0} VND", normalFont));
                        pdfDoc.Add(new Paragraph("========================================", normalFont));
                        pdfDoc.Add(new Paragraph("Cảm ơn quý khách và hẹn gặp lại!", normalFont));

                        pdfDoc.Close();
                        stream.Close();
                    }

                    MessageBox.Show("Xuất hóa đơn ra PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadHoaDonToGrid()
        {
            try
            {
                DataTable hoaDonList = donHangBLL.LayDanhSachDonHang();
                dgvHoaDon.DataSource = hoaDonList;
                dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (hoaDonList.Columns.Contains("MaDH"))
                    dgvHoaDon.Columns["MaDH"].HeaderText = "Mã HĐ";
                if (hoaDonList.Columns.Contains("NgayLap"))
                    dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
                if (hoaDonList.Columns.Contains("TongTien"))
                    dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";
                if (hoaDonList.Columns.Contains("TrangThaiThanhToan"))
                    dgvHoaDon.Columns["TrangThaiThanhToan"].HeaderText = "Trạng Thái Thanh Toán";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
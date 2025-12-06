using PM_Ban_Do_An_Nhanh.BLL;
using PM_Ban_Do_An_Nhanh.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PM_Ban_Do_An_Nhanh
{
    public partial class frmReport : Form
    {
        private DonHangBLL donHangBLL = new DonHangBLL();
        
        // Dashboard Cards
        private Panel pnlDashboard;
        private Panel cardDoanhThuHomNay;
        private Panel cardSoDonHang;
        private Panel cardDoanhThuThang;
        private Panel cardTopMon;
        
        // Charts
        private Chart chartDoanhThu;
        private Chart chartMonBanChay;
        
        // Export button
        private Button btnExportCSV;

        public frmReport()
        {
            InitializeComponent();
            this.Text = "📊 Báo cáo thống kê";
            this.WindowState = FormWindowState.Maximized;
            
            // CRITICAL: Set format FIRST, then set value
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            
            // Set default date range: Start of current month → Today
            DateTime today = DateTime.Today;
            DateTime monthStart = new DateTime(today.Year, today.Month, 1);
            dtpTuNgay.Value = monthStart;
            dtpDenNgay.Value = today;
            dtpTuNgay.Checked = true;
            dtpDenNgay.Checked = true;
            
            SetupButtonStyles();
            SetupDashboard();
            SetupCharts();
            SetupExportButton();
            
            // Auto-load on form load
            this.Load += (s, e) => LoadReports();
        }

        private void SetupDashboard()
        {
            // Create Dashboard panel at top
            pnlDashboard = new Panel();
            pnlDashboard.Dock = DockStyle.Top;
            pnlDashboard.Height = 120;
            pnlDashboard.BackColor = Color.FromArgb(248, 249, 250);
            pnlDashboard.Padding = new Padding(10);

            // Create 4 cards
            int cardWidth = 220;
            int spacing = 20;

            cardDoanhThuHomNay = CreateDashboardCard("💰 Doanh thu hôm nay", "0 VNĐ", Color.FromArgb(46, 204, 113), 10);
            cardSoDonHang = CreateDashboardCard("📦 Đơn hàng hôm nay", "0", Color.FromArgb(52, 152, 219), 10 + cardWidth + spacing);
            cardDoanhThuThang = CreateDashboardCard("📊 Doanh thu tháng", "0 VNĐ", Color.FromArgb(155, 89, 182), 10 + (cardWidth + spacing) * 2);
            cardTopMon = CreateDashboardCard("🏆 Món bán chạy nhất", "---", Color.FromArgb(241, 196, 15), 10 + (cardWidth + spacing) * 3);

            pnlDashboard.Controls.Add(cardDoanhThuHomNay);
            pnlDashboard.Controls.Add(cardSoDonHang);
            pnlDashboard.Controls.Add(cardDoanhThuThang);
            pnlDashboard.Controls.Add(cardTopMon);

            // Add dashboard to form (before tabControl)
            this.Controls.Add(pnlDashboard);
            pnlDashboard.BringToFront();
        }

        private Panel CreateDashboardCard(string title, string value, Color accentColor, int left)
        {
            Panel card = new Panel();
            card.Size = new Size(220, 90);
            card.Location = new Point(left, 10);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.None;
            
            // Add shadow effect with border
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
                // Accent bar on left
                using (SolidBrush brush = new SolidBrush(accentColor))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, 5, card.Height);
                }
            };

            // Title label
            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblTitle.ForeColor = Color.FromArgb(127, 140, 141);
            lblTitle.Location = new Point(15, 12);
            lblTitle.AutoSize = true;
            card.Controls.Add(lblTitle);

            // Value label
            Label lblValue = new Label();
            lblValue.Name = "lblValue";
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblValue.ForeColor = accentColor;
            lblValue.Location = new Point(15, 40);
            lblValue.Size = new Size(200, 40);
            lblValue.AutoEllipsis = true;
            card.Controls.Add(lblValue);

            return card;
        }

        private void SetupCharts()
        {
            // Add a new tab for charts
            TabPage tabCharts = new TabPage("📈 Biểu đồ");
            tabCharts.BackColor = Color.White;
            tabControlReports.TabPages.Add(tabCharts);

            // Split container for 2 charts
            SplitContainer splitCharts = new SplitContainer();
            splitCharts.Dock = DockStyle.Fill;
            splitCharts.Orientation = Orientation.Horizontal;
            splitCharts.SplitterDistance = 250;
            tabCharts.Controls.Add(splitCharts);

            // Revenue Bar Chart
            chartDoanhThu = new Chart();
            chartDoanhThu.Dock = DockStyle.Fill;
            chartDoanhThu.BackColor = Color.White;
            
            ChartArea areaDoanhThu = new ChartArea("AreaDoanhThu");
            areaDoanhThu.AxisX.Title = "Ngày";
            areaDoanhThu.AxisY.Title = "Doanh thu (VNĐ)";
            areaDoanhThu.AxisY.LabelStyle.Format = "N0";
            areaDoanhThu.AxisX.MajorGrid.LineColor = Color.LightGray;
            areaDoanhThu.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartDoanhThu.ChartAreas.Add(areaDoanhThu);

            Series seriesDoanhThu = new Series("Doanh thu");
            seriesDoanhThu.ChartType = SeriesChartType.Column;
            seriesDoanhThu.Color = Color.FromArgb(52, 152, 219);
            seriesDoanhThu.IsValueShownAsLabel = true;
            seriesDoanhThu.LabelFormat = "N0";
            chartDoanhThu.Series.Add(seriesDoanhThu);

            Title titleDoanhThu = new Title("📊 Doanh thu theo ngày");
            titleDoanhThu.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            chartDoanhThu.Titles.Add(titleDoanhThu);

            splitCharts.Panel1.Controls.Add(chartDoanhThu);

            // Pie Chart for Top Items
            chartMonBanChay = new Chart();
            chartMonBanChay.Dock = DockStyle.Fill;
            chartMonBanChay.BackColor = Color.White;

            ChartArea areaPie = new ChartArea("AreaPie");
            chartMonBanChay.ChartAreas.Add(areaPie);

            Series seriesPie = new Series("Top món");
            seriesPie.ChartType = SeriesChartType.Pie;
            seriesPie.IsValueShownAsLabel = true;
            seriesPie.LabelFormat = "N0";
            seriesPie["PieLabelStyle"] = "Outside";
            chartMonBanChay.Series.Add(seriesPie);

            Title titlePie = new Title("🏆 Tỷ trọng món ăn bán chạy (Top 5)");
            titlePie.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            chartMonBanChay.Titles.Add(titlePie);

            Legend legendPie = new Legend("LegendPie");
            legendPie.Docking = Docking.Right;
            chartMonBanChay.Legends.Add(legendPie);

            splitCharts.Panel2.Controls.Add(chartMonBanChay);
        }

        private void SetupExportButton()
        {
            btnExportCSV = new Button();
            btnExportCSV.Text = "📥 Xuất Excel";
            btnExportCSV.Size = new Size(127, 30);
            btnExportCSV.Location = new Point(11, 380);
            btnExportCSV.FlatStyle = FlatStyle.Flat;
            btnExportCSV.BackColor = Color.FromArgb(39, 174, 96);
            btnExportCSV.ForeColor = Color.White;
            btnExportCSV.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExportCSV.Cursor = Cursors.Hand;
            btnExportCSV.Click += BtnExportCSV_Click;
            
            panel1.Controls.Add(btnExportCSV);
        }

        private void BtnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                saveDialog.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                saveDialog.Title = "Xuất báo cáo ra file CSV";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    
                    // Header
                    sb.AppendLine("BÁO CÁO DOANH THU");
                    sb.AppendLine($"Từ ngày: {(dtpTuNgay.Checked ? dtpTuNgay.Value.ToShortDateString() : "Tất cả")}");
                    sb.AppendLine($"Đến ngày: {(dtpDenNgay.Checked ? dtpDenNgay.Value.ToShortDateString() : "Tất cả")}");
                    sb.AppendLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    sb.AppendLine();
                    
                    // Revenue data
                    sb.AppendLine("DOANH THU THEO NGÀY");
                    sb.AppendLine("Ngày,Doanh thu (VNĐ)");
                    foreach (DataGridViewRow row in dgvDoanhThu.Rows)
                    {
                        if (row.Cells["Ngay"].Value != null)
                        {
                            string ngay = Convert.ToDateTime(row.Cells["Ngay"].Value).ToString("dd/MM/yyyy");
                            string doanhThu = row.Cells["DoanhThuNgay"].Value?.ToString() ?? "0";
                            sb.AppendLine($"{ngay},{doanhThu}");
                        }
                    }
                    sb.AppendLine();
                    
                    // Top items data
                    sb.AppendLine("MÓN ĂN BÁN CHẠY");
                    sb.AppendLine("Tên món,Số lượng bán,Doanh thu (VNĐ)");
                    foreach (DataGridViewRow row in dgvMonBanChay.Rows)
                    {
                        if (row.Cells["TenMon"].Value != null)
                        {
                            string tenMon = row.Cells["TenMon"].Value?.ToString() ?? "";
                            string soLuong = row.Cells["TongSoLuongBan"].Value?.ToString() ?? "0";
                            string doanhThu = row.Cells["TongDoanhThuMon"].Value?.ToString() ?? "0";
                            sb.AppendLine($"\"{tenMon}\",{soLuong},{doanhThu}");
                        }
                    }

                    File.WriteAllText(saveDialog.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Đã xuất báo cáo thành công!\n📁 {saveDialog.FileName}", "✅ Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDashboardCards()
        {
            try
            {
                // Use filter range from DateTimePickers
                DateTime? tuNgay = dtpTuNgay.Checked ? dtpTuNgay.Value : (DateTime?)null;
                DateTime? denNgay = dtpDenNgay.Checked ? dtpDenNgay.Value : (DateTime?)null;
                
                DateTime today = DateTime.Today;
                DateTime monthStart = new DateTime(today.Year, today.Month, 1);

                // Card 1: Today's revenue (always today regardless of filter)
                DataTable dtToday = donHangBLL.LayThongKeDoanhThu(today, today);
                decimal doanhThuHomNay = 0;
                if (dtToday.Rows.Count > 0 && dtToday.Rows[0]["DoanhThuNgay"] != DBNull.Value)
                {
                    doanhThuHomNay = Convert.ToDecimal(dtToday.Rows[0]["DoanhThuNgay"]);
                }

                // Card 2: Order count today
                DataTable dtOrders = donHangBLL.LayDanhSachDonHang(today, today, null);
                int soDonHomNay = dtOrders.Rows.Count;

                // Card 3: Revenue for FILTERED RANGE (not just this month)
                decimal doanhThuKhoangThang = 0;
                if (dgvDoanhThu.Rows.Count > 0)
                {
                    // Calculate from already loaded data in grid
                    foreach (DataGridViewRow row in dgvDoanhThu.Rows)
                    {
                        if (row.Cells["DoanhThuNgay"].Value != null && row.Cells["DoanhThuNgay"].Value != DBNull.Value)
                        {
                            doanhThuKhoangThang += Convert.ToDecimal(row.Cells["DoanhThuNgay"].Value);
                        }
                    }
                }

                // Card 4: Top item from FILTERED RANGE
                string topMon = "---";
                if (dgvMonBanChay.Rows.Count > 0 && dgvMonBanChay.Rows[0].Cells["TenMon"].Value != null)
                {
                    topMon = dgvMonBanChay.Rows[0].Cells["TenMon"].Value.ToString();
                }

                // Update cards
                UpdateCardValue(cardDoanhThuHomNay, $"{doanhThuHomNay:N0} VNĐ");
                UpdateCardValue(cardSoDonHang, soDonHomNay.ToString());
                UpdateCardValue(cardDoanhThuThang, $"{doanhThuKhoangThang:N0} VNĐ");
                UpdateCardValue(cardTopMon, topMon);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating dashboard: {ex.Message}");
            }
        }

        private void UpdateCardValue(Panel card, string value)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl.Name == "lblValue" && ctrl is Label lbl)
                {
                    lbl.Text = value;
                    break;
                }
            }
        }

        private void UpdateCharts()
        {
            try
            {
                // Update Revenue Chart
                chartDoanhThu.Series["Doanh thu"].Points.Clear();
                foreach (DataGridViewRow row in dgvDoanhThu.Rows)
                {
                    if (row.Cells["Ngay"].Value != null)
                    {
                        DateTime ngay = Convert.ToDateTime(row.Cells["Ngay"].Value);
                        decimal doanhThu = Convert.ToDecimal(row.Cells["DoanhThuNgay"].Value);
                        chartDoanhThu.Series["Doanh thu"].Points.AddXY(ngay.ToString("dd/MM"), (double)doanhThu);
                    }
                }

                // Update Pie Chart (Top 5)
                chartMonBanChay.Series["Top món"].Points.Clear();
                int count = 0;
                Color[] pieColors = { 
                    Color.FromArgb(46, 204, 113), 
                    Color.FromArgb(52, 152, 219), 
                    Color.FromArgb(155, 89, 182), 
                    Color.FromArgb(241, 196, 15),
                    Color.FromArgb(231, 76, 60)
                };
                
                foreach (DataGridViewRow row in dgvMonBanChay.Rows)
                {
                    if (count >= 5) break;
                    if (row.Cells["TenMon"].Value != null)
                    {
                        string tenMon = row.Cells["TenMon"].Value.ToString();
                        decimal soLuong = Convert.ToDecimal(row.Cells["TongSoLuongBan"].Value);
                        int pointIndex = chartMonBanChay.Series["Top món"].Points.AddXY(tenMon, (double)soLuong);
                        DataPoint point = chartMonBanChay.Series["Top món"].Points[pointIndex];
                        point.Color = pieColors[count % pieColors.Length];
                        point.LegendText = tenMon;
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating charts: {ex.Message}");
            }
        }

        private void SetupButtonStyles()
        {
            ButtonStyleHelper.ApplyPrimaryStyle(btnXemBaoCao, "📊 Xem báo cáo", "Tạo và hiển thị báo cáo theo khoảng thời gian đã chọn", ButtonSize.Large);
            ButtonStyleHelper.ApplyInfoStyle(btnInBaoCao, "🖨️ In báo cáo", "In báo cáo hiện tại", ButtonSize.Large);

            SetupDateTimePickerStyles();
            SetupDataGridViewStyles();
            SetupFormStyle();
            SetupLabelsStyle();
        }

        private void SetupDateTimePickerStyles()
        {
            dtpTuNgay.Font = new Font("Segoe UI", 11F);
            dtpDenNgay.Font = new Font("Segoe UI", 11F);
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Format = DateTimePickerFormat.Short;
        }

        private void SetupDataGridViewStyles()
        {
            ButtonStyleHelper.ApplyModernDataGridViewStyle(dgvDoanhThu, "success");
            dgvDoanhThu.BackgroundColor = Color.White;
            dgvDoanhThu.BorderStyle = BorderStyle.None;
            dgvDoanhThu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDoanhThu.MultiSelect = false;
            dgvDoanhThu.ReadOnly = true;
            dgvDoanhThu.AllowUserToAddRows = false;
            dgvDoanhThu.AllowUserToDeleteRows = false;

            ButtonStyleHelper.ApplyModernDataGridViewStyle(dgvMonBanChay, "warning");
            dgvMonBanChay.BackgroundColor = Color.White;
            dgvMonBanChay.BorderStyle = BorderStyle.None;
            dgvMonBanChay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMonBanChay.MultiSelect = false;
            dgvMonBanChay.ReadOnly = true;
            dgvMonBanChay.AllowUserToAddRows = false;
            dgvMonBanChay.AllowUserToDeleteRows = false;
        }

        private void SetupFormStyle()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 10F);
        }

        private void SetupLabelsStyle()
        {
            if (this.Controls.Find("lblDateRangeTitle", true).Length > 0)
            {
                Label lblDateRangeTitle = (Label)this.Controls.Find("lblDateRangeTitle", true)[0];
                StyleSectionLabel(lblDateRangeTitle, "📅 Chọn khoảng thời gian");
            }

            if (this.Controls.Find("lblRevenueTitle", true).Length > 0)
            {
                Label lblRevenueTitle = (Label)this.Controls.Find("lblRevenueTitle", true)[0];
                StyleSectionLabel(lblRevenueTitle, "💰 Thống kê doanh thu");
            }

            if (this.Controls.Find("lblTopItemsTitle", true).Length > 0)
            {
                Label lblTopItemsTitle = (Label)this.Controls.Find("lblTopItemsTitle", true)[0];
                StyleSectionLabel(lblTopItemsTitle, "🏆 Top món ăn bán chạy");
            }

            if (lblTotalRevenue != null)
            {
                lblTotalRevenue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                lblTotalRevenue.ForeColor = ButtonStyleHelper.SuccessGreen;
                lblTotalRevenue.BackColor = Color.FromArgb(248, 255, 248);
                lblTotalRevenue.BorderStyle = BorderStyle.FixedSingle;
                lblTotalRevenue.TextAlign = ContentAlignment.MiddleCenter;
                lblTotalRevenue.Padding = new Padding(10);
            }
        }

        private void StyleSectionLabel(Label label, string text)
        {
            label.Text = text;
            label.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(52, 73, 94);
            label.BackColor = Color.White;
            label.BorderStyle = BorderStyle.None;
            label.Padding = new Padding(8);
        }

        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void LoadReports()
        {
            DateTime? tuNgay = dtpTuNgay.Checked ? dtpTuNgay.Value : (DateTime?)null;
            DateTime? denNgay = dtpDenNgay.Checked ? dtpDenNgay.Value : (DateTime?)null;

            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value > denNgay.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "⚠️ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblTotalRevenue.Text = "Đang tải...";

                LoadDoanhThuReport(tuNgay, denNgay);
                LoadMonAnBanChayReport(tuNgay, denNgay);
                UpdateDashboardCards();
                UpdateCharts();

                MessageBox.Show("Đã tải báo cáo thành công! 📊", "✅ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message, "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTotalRevenue.Text = "Lỗi tải dữ liệu";
            }
        }

        private void LoadDoanhThuReport(DateTime? tuNgay, DateTime? denNgay)
        {
            try
            {
                DataTable dtDoanhThu = donHangBLL.LayThongKeDoanhThu(tuNgay, denNgay);
                dgvDoanhThu.DataSource = dtDoanhThu;

                if (dtDoanhThu.Rows.Count > 0)
                {
                    Ngay.HeaderText = "📅 Ngày";
                    DoanhThuNgay.HeaderText = "💰 Doanh Thu";
                    
                    Ngay.Width = 150;
                    DoanhThuNgay.Width = 200;
                    
                    Ngay.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    DoanhThuNgay.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    
                    Ngay.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DoanhThuNgay.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Ngay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Ngay.DefaultCellStyle.Format = "dd/MM/yyyy"; // Format date as dd/MM/yyyy
                    DoanhThuNgay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    
                    DoanhThuNgay.DefaultCellStyle.Format = "N0 VNĐ";

                    decimal tongDoanhThu = 0;
                    foreach (DataRow row in dtDoanhThu.Rows)
                    {
                        if (row["DoanhThuNgay"] != DBNull.Value)
                        {
                            tongDoanhThu += Convert.ToDecimal(row["DoanhThuNgay"]);
                        }
                    }
                    
                    lblTotalRevenue.Text = $"💰 {tongDoanhThu:N0} VNĐ";
                }
                else
                {
                    lblTotalRevenue.Text = "📊 Chưa có dữ liệu";
                }
            }
            catch (Exception ex)
            {
                lblTotalRevenue.Text = $"❌ Lỗi: {ex.Message}";
            }
        }

        private void LoadMonAnBanChayReport(DateTime? tuNgay, DateTime? denNgay)
        {
            DataTable dtMonBanChay = donHangBLL.LayMonAnBanChay(tuNgay, denNgay, 10);
            dgvMonBanChay.DataSource = dtMonBanChay;

            if (dtMonBanChay.Rows.Count > 0)
            {
                dgvMonBanChay.Columns["TenMon"].HeaderText = "🍽️ Tên Món";
                dgvMonBanChay.Columns["TongSoLuongBan"].HeaderText = "📊 Tổng SL Bán";
                dgvMonBanChay.Columns["TongDoanhThuMon"].HeaderText = "💰 Tổng Doanh Thu";
                dgvMonBanChay.Columns["TongDoanhThuMon"].DefaultCellStyle.Format = "N0";
                dgvMonBanChay.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                for (int i = 0; i < Math.Min(3, dgvMonBanChay.Rows.Count); i++)
                {
                    Color highlightColor = i == 0 ? Color.FromArgb(255, 215, 0) :
                                          i == 1 ? Color.FromArgb(192, 192, 192) :
                                                   Color.FromArgb(205, 127, 50);

                    dgvMonBanChay.Rows[i].DefaultCellStyle.BackColor = highlightColor;
                    dgvMonBanChay.Rows[i].DefaultCellStyle.SelectionBackColor = ButtonStyleHelper.LightenColor(highlightColor, 20);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                using (Pen pen = new Pen(Color.FromArgb(220, 221, 225), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            }
        }

        private void btnInBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDoanhThu.Rows.Count == 0 && dgvMonBanChay.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để in. Vui lòng tạo báo cáo trước.", "⚠️ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += new PrintPageEventHandler(PrintReportPage_PrintPage);

                PrintPreviewDialog previewDlg = new PrintPreviewDialog();
                previewDlg.Document = printDoc;
                previewDlg.Text = "🖨️ Xem trước báo cáo";
                previewDlg.WindowState = FormWindowState.Maximized;

                if (previewDlg.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("In báo cáo thành công! 🖨️", "✅ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuẩn bị in báo cáo: " + ex.Message, "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReportPage_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font subHeaderFont = new Font("Arial", 12, FontStyle.Bold);
            Font contentFont = new Font("Arial", 10, FontStyle.Regular);
            Brush brush = Brushes.Black;
            Brush blueBrush = new SolidBrush(ButtonStyleHelper.PrimaryBlue);
            Brush greenBrush = new SolidBrush(ButtonStyleHelper.SuccessGreen);

            float yPos = 50;
            int leftMargin = 50;
            int rightMargin = e.MarginBounds.Right - 50;

            string reportTitle = "📊 BÁO CÁO THỐNG KÊ DOANH THU";
            SizeF titleSize = e.Graphics.MeasureString(reportTitle, titleFont);
            e.Graphics.DrawString(reportTitle, titleFont, blueBrush,
                new Point((int)((e.PageBounds.Width - titleSize.Width) / 2), (int)yPos));
            yPos += titleFont.GetHeight() + 30;

            e.Graphics.DrawLine(new Pen(ButtonStyleHelper.PrimaryBlue, 2), leftMargin, yPos, rightMargin, yPos);
            yPos += 20;

            string period = "📅 Thời gian: " + (dtpTuNgay.Checked ? dtpTuNgay.Value.ToShortDateString() : "Tất cả") +
                " đến " + (dtpDenNgay.Checked ? dtpDenNgay.Value.ToShortDateString() : "Tất cả");
            e.Graphics.DrawString(period, headerFont, brush, leftMargin, yPos);
            yPos += headerFont.GetHeight() + 10;

            string generateTime = "🕒 Thời gian tạo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            e.Graphics.DrawString(generateTime, contentFont, brush, leftMargin, yPos);
            yPos += contentFont.GetHeight() + 30;

            e.Graphics.DrawString("💰 THỐNG KÊ DOANH THU", subHeaderFont, greenBrush, leftMargin, yPos);
            yPos += subHeaderFont.GetHeight() + 15;

            Rectangle revenueBox = new Rectangle(leftMargin, (int)yPos, rightMargin - leftMargin, 40);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(240, 255, 240)), revenueBox);
            e.Graphics.DrawRectangle(new Pen(ButtonStyleHelper.SuccessGreen, 2), revenueBox);

            string totalRevenueText = "💰 TỔNG DOANH THU: " + lblTotalRevenue.Text.Replace("💰 ", "");
            SizeF revenueSize = e.Graphics.MeasureString(totalRevenueText, headerFont);
            e.Graphics.DrawString(totalRevenueText, headerFont, greenBrush,
                leftMargin + (revenueBox.Width - revenueSize.Width) / 2, yPos + 10);
            yPos += 60;

            if (dgvDoanhThu.Rows.Count > 0 && dgvDoanhThu.Rows.Count <= 10)
            {
                e.Graphics.DrawString("📊 Chi tiết doanh thu theo ngày:", subHeaderFont, brush, leftMargin, yPos);
                yPos += subHeaderFont.GetHeight() + 10;

                float colWidth = (rightMargin - leftMargin) / 2;
                e.Graphics.DrawString("📅 Ngày", contentFont, brush, leftMargin, yPos);
                e.Graphics.DrawString("💰 Doanh Thu", contentFont, brush, leftMargin + colWidth, yPos);
                yPos += contentFont.GetHeight() + 5;

                e.Graphics.DrawLine(Pens.Gray, leftMargin, yPos, rightMargin, yPos);
                yPos += 5;

                for (int i = 0; i < Math.Min(10, dgvDoanhThu.Rows.Count); i++)
                {
                    if (dgvDoanhThu.Rows[i].Cells["Ngay"].Value != null)
                    {
                        DateTime ngay = Convert.ToDateTime(dgvDoanhThu.Rows[i].Cells["Ngay"].Value);
                        decimal doanhThu = Convert.ToDecimal(dgvDoanhThu.Rows[i].Cells["DoanhThuNgay"].Value);

                        e.Graphics.DrawString(ngay.ToString("dd/MM/yyyy"), contentFont, brush, leftMargin, yPos);
                        e.Graphics.DrawString(doanhThu.ToString("N0") + " VNĐ", contentFont, brush, leftMargin + colWidth, yPos);
                        yPos += contentFont.GetHeight() + 3;
                    }
                }
                yPos += 20;
            }

            e.Graphics.DrawString("🏆 TOP MÓN ĂN BÁN CHẠY", subHeaderFont, new SolidBrush(ButtonStyleHelper.WarningOrange), leftMargin, yPos);
            yPos += subHeaderFont.GetHeight() + 15;

            if (dgvMonBanChay.Rows.Count > 0)
            {
                float col1Width = (rightMargin - leftMargin) * 0.4f;
                float col2Width = (rightMargin - leftMargin) * 0.25f;
                float col3Width = (rightMargin - leftMargin) * 0.35f;

                e.Graphics.DrawString("🍽️ Tên Món", contentFont, brush, leftMargin, yPos);
                e.Graphics.DrawString("📊 Số Lượng", contentFont, brush, leftMargin + col1Width, yPos);
                e.Graphics.DrawString("💰 Doanh Thu", contentFont, brush, leftMargin + col1Width + col2Width, yPos);
                yPos += contentFont.GetHeight() + 5;

                e.Graphics.DrawLine(Pens.Gray, leftMargin, yPos, rightMargin, yPos);
                yPos += 5;

                for (int i = 0; i < Math.Min(15, dgvMonBanChay.Rows.Count); i++)
                {
                    if (dgvMonBanChay.Rows[i].Cells["TenMon"].Value != null)
                    {
                        string ranking = i == 0 ? "🥇 " : i == 1 ? "🥈 " : i == 2 ? "🥉 " : $"{i + 1}. ";

                        string tenMon = ranking + dgvMonBanChay.Rows[i].Cells["TenMon"].Value.ToString();
                        string soLuong = dgvMonBanChay.Rows[i].Cells["TongSoLuongBan"].Value.ToString();
                        string doanhThu = Convert.ToDecimal(dgvMonBanChay.Rows[i].Cells["TongDoanhThuMon"].Value).ToString("N0") + " VNĐ";

                        Font itemFont = i < 3 ? new Font("Arial", 10, FontStyle.Bold) : contentFont;

                        e.Graphics.DrawString(tenMon, itemFont, brush, leftMargin, yPos);
                        e.Graphics.DrawString(soLuong, itemFont, brush, leftMargin + col1Width, yPos);
                        e.Graphics.DrawString(doanhThu, itemFont, brush, leftMargin + col1Width + col2Width, yPos);
                        yPos += itemFont.GetHeight() + 3;
                    }
                }
            }
            else
            {
                e.Graphics.DrawString("📭 Không có dữ liệu món ăn bán chạy trong khoảng thời gian này.", contentFont, brush, leftMargin, yPos);
            }

            yPos = e.PageBounds.Height - 100;
            e.Graphics.DrawLine(new Pen(ButtonStyleHelper.PrimaryBlue, 1), leftMargin, yPos, rightMargin, yPos);
            yPos += 10;
            string footer = "🏪 Hệ thống FastFood Manager - Báo cáo được tạo tự động";
            e.Graphics.DrawString(footer, new Font("Arial", 8, FontStyle.Italic), brush, leftMargin, yPos);

            e.HasMorePages = false;
        }
    }
}
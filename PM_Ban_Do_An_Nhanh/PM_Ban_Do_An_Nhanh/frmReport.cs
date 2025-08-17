using PM_Ban_Do_An_Nhanh.BLL;
using PM_Ban_Do_An_Nhanh.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PM_Ban_Do_An_Nhanh
{
    public partial class frmReport : Form
    {
        private DonHangBLL donHangBLL = new DonHangBLL();

        public frmReport()
        {
            InitializeComponent();
            this.Text = "📊 Báo cáo thống kê";
            dtpTuNgay.Value = DateTime.Today.AddMonths(-1); // Mặc định 1 tháng trước
            dtpDenNgay.Value = DateTime.Today;
            dtpTuNgay.Checked = true;
            dtpDenNgay.Checked = true;
            SetupButtonStyles();
        }

        private void SetupButtonStyles()
        {
            // Style buttons với icons và tooltips
            ButtonStyleHelper.ApplyPrimaryStyle(btnXemBaoCao, "📊 Xem báo cáo", "Tạo và hiển thị báo cáo theo khoảng thời gian đã chọn", ButtonSize.Large);
            ButtonStyleHelper.ApplyInfoStyle(btnInBaoCao, "🖨️ In báo cáo", "In báo cáo hiện tại", ButtonSize.Large);

            // Style các controls khác
            SetupDateTimePickerStyles();
            SetupDataGridViewStyles();
            SetupFormStyle();
            SetupLabelsStyle();
        }

        private void SetupDateTimePickerStyles()
        {
            // Style DateTimePicker
            dtpTuNgay.Font = new Font("Segoe UI", 11F);
            dtpDenNgay.Font = new Font("Segoe UI", 11F);
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Format = DateTimePickerFormat.Short;
        }

        private void SetupDataGridViewStyles()
        {
            // Style DataGridView cho doanh thu
            ButtonStyleHelper.ApplyModernDataGridViewStyle(dgvDoanhThu, "success");
            dgvDoanhThu.BackgroundColor = Color.White;
            dgvDoanhThu.BorderStyle = BorderStyle.None;
            dgvDoanhThu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDoanhThu.MultiSelect = false;
            dgvDoanhThu.ReadOnly = true;
            dgvDoanhThu.AllowUserToAddRows = false;
            dgvDoanhThu.AllowUserToDeleteRows = false;

            // Style DataGridView cho món ăn bán chạy
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
            // Style form
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 10F);
        }

        private void SetupLabelsStyle()
        {
            // Style labels để tạo sections đẹp hơn
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

            // Style total revenue label
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
                // Show loading message
                lblTotalRevenue.Text = "Đang tải...";

                // Debug: Kiểm tra dữ liệu trong database
                CheckDatabaseData();

                LoadDoanhThuReport(tuNgay, denNgay);
                LoadMonAnBanChayReport(tuNgay, denNgay);

                MessageBox.Show("Đã tải báo cáo thành công! 📊", "✅ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message, "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTotalRevenue.Text = "Lỗi tải dữ liệu";
            }
        }

        private void CheckDatabaseData()
        {
            try
            {
                DataTable dtAllOrders = donHangBLL.LayDanhSachDonHang();
                Console.WriteLine($"Tổng số đơn hàng trong DB: {dtAllOrders.Rows.Count}");
                
                if (dtAllOrders.Rows.Count > 0)
                {
                    Console.WriteLine("Mẫu đơn hàng:");
                    for (int i = 0; i < Math.Min(3, dtAllOrders.Rows.Count); i++)
                    {
                        Console.WriteLine($"Đơn hàng {i}: MaDH={dtAllOrders.Rows[i]["MaDH"]}, NgayLap={dtAllOrders.Rows[i]["NgayLap"]}, TongTien={dtAllOrders.Rows[i]["TongTien"]}, TrangThai={dtAllOrders.Rows[i]["TrangThaiThanhToan"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi kiểm tra dữ liệu DB: {ex.Message}");
            }
        }

        private void LoadDoanhThuReport(DateTime? tuNgay, DateTime? denNgay)
        {
            try
            {
                DataTable dtDoanhThu = donHangBLL.LayThongKeDoanhThu(tuNgay, denNgay);
                dgvDoanhThu.DataSource = dtDoanhThu;

                // Debug: Kiểm tra dữ liệu
                Console.WriteLine($"Số dòng dữ liệu: {dtDoanhThu.Rows.Count}");
                if (dtDoanhThu.Rows.Count > 0)
                {
                    Console.WriteLine("Dữ liệu mẫu:");
                    for (int i = 0; i < Math.Min(3, dtDoanhThu.Rows.Count); i++)
                    {
                        Console.WriteLine($"Row {i}: Ngày={dtDoanhThu.Rows[i]["Ngay"]}, DoanhThu={dtDoanhThu.Rows[i]["DoanhThuNgay"]}");
                    }
                }

                if (dtDoanhThu.Rows.Count > 0)
                {
                    // Thiết lập tiêu đề cột với icons
                    Ngay.HeaderText = "📅 Ngày";
                    DoanhThuNgay.HeaderText = "💰 Doanh Thu";
                    
                    // Thiết lập width cố định cho các cột để đều nhau
                    Ngay.Width = 150;
                    DoanhThuNgay.Width = 200;
                    
                    // Tắt AutoSizeMode để có thể set width cố định
                    Ngay.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    DoanhThuNgay.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    
                    // Căn giữa header và cell
                    Ngay.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DoanhThuNgay.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Ngay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DoanhThuNgay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    
                    // Format số tiền
                    DoanhThuNgay.DefaultCellStyle.Format = "N0 VNĐ";

                    // Tính tổng doanh thu
                    decimal tongDoanhThu = 0;
                    foreach (DataRow row in dtDoanhThu.Rows)
                    {
                        if (row["DoanhThuNgay"] != DBNull.Value)
                        {
                            decimal doanhThu = Convert.ToDecimal(row["DoanhThuNgay"]);
                            tongDoanhThu += doanhThu;
                            Console.WriteLine($"Cộng dồn: {doanhThu:N0} -> Tổng: {tongDoanhThu:N0}");
                        }
                    }
                    
                                         Console.WriteLine($"Tổng cuối cùng: {tongDoanhThu:N0}");
                     
                     // Đảm bảo cập nhật UI thread
                     if (lblTotalRevenue.InvokeRequired)
                     {
                         lblTotalRevenue.Invoke(new Action(() => {
                             lblTotalRevenue.Text = $"💰 {tongDoanhThu:N0} VNĐ";
                             lblTotalRevenue.Refresh();
                         }));
                     }
                     else
                     {
                         lblTotalRevenue.Text = $"💰 {tongDoanhThu:N0} VNĐ";
                         lblTotalRevenue.Refresh();
                     }
                }
                else
                {
                    lblTotalRevenue.Text = "📊 Chưa có dữ liệu";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong LoadDoanhThuReport: {ex.Message}");
                lblTotalRevenue.Text = $"❌ Lỗi: {ex.Message}";
            }
        }

        private void LoadMonAnBanChayReport(DateTime? tuNgay, DateTime? denNgay)
        {
            DataTable dtMonBanChay = donHangBLL.LayMonAnBanChay(tuNgay, denNgay, 10); // Lấy Top 10 món
            dgvMonBanChay.DataSource = dtMonBanChay;

            if (dtMonBanChay.Rows.Count > 0)
            {
                // Thiết lập tiêu đề cột với icons
                dgvMonBanChay.Columns["TenMon"].HeaderText = "🍽️ Tên Món";
                dgvMonBanChay.Columns["TongSoLuongBan"].HeaderText = "📊 Tổng SL Bán";
                dgvMonBanChay.Columns["TongDoanhThuMon"].HeaderText = "💰 Tổng Doanh Thu";
                dgvMonBanChay.Columns["TongDoanhThuMon"].DefaultCellStyle.Format = "N0";
                dgvMonBanChay.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                // Highlight top 3 món bán chạy nhất
                for (int i = 0; i < Math.Min(3, dgvMonBanChay.Rows.Count); i++)
                {
                    Color highlightColor = i == 0 ? Color.FromArgb(255, 215, 0) :  // Gold for #1
                                          i == 1 ? Color.FromArgb(192, 192, 192) : // Silver for #2
                                                   Color.FromArgb(205, 127, 50);    // Bronze for #3

                    dgvMonBanChay.Rows[i].DefaultCellStyle.BackColor = highlightColor;
                    dgvMonBanChay.Rows[i].DefaultCellStyle.SelectionBackColor = ButtonStyleHelper.LightenColor(highlightColor, 20);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Draw border for panel if needed
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
                // Validate data before printing
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
            // Thiết lập font và brush để vẽ
            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font subHeaderFont = new Font("Arial", 12, FontStyle.Bold);
            Font contentFont = new Font("Arial", 10, FontStyle.Regular);
            Brush brush = Brushes.Black;
            Brush blueBrush = new SolidBrush(ButtonStyleHelper.PrimaryBlue);
            Brush greenBrush = new SolidBrush(ButtonStyleHelper.SuccessGreen);

            // Vị trí bắt đầu
            float yPos = 50;
            int leftMargin = 50;
            int rightMargin = e.MarginBounds.Right - 50;

            // Vẽ tiêu đề chính
            string reportTitle = "📊 BÁO CÁO THỐNG KÊ DOANH THU";
            SizeF titleSize = e.Graphics.MeasureString(reportTitle, titleFont);
            e.Graphics.DrawString(reportTitle, titleFont, blueBrush,
                new Point((int)((e.PageBounds.Width - titleSize.Width) / 2), (int)yPos));
            yPos += titleFont.GetHeight() + 30;

            // Vẽ đường kẻ
            e.Graphics.DrawLine(new Pen(ButtonStyleHelper.PrimaryBlue, 2), leftMargin, yPos, rightMargin, yPos);
            yPos += 20;

            // Vẽ thông tin thời gian báo cáo
            string period = "📅 Thời gian: " + (dtpTuNgay.Checked ? dtpTuNgay.Value.ToShortDateString() : "Tất cả") +
                " đến " + (dtpDenNgay.Checked ? dtpDenNgay.Value.ToShortDateString() : "Tất cả");
            e.Graphics.DrawString(period, headerFont, brush, leftMargin, yPos);
            yPos += headerFont.GetHeight() + 10;

            string generateTime = "🕒 Thời gian tạo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            e.Graphics.DrawString(generateTime, contentFont, brush, leftMargin, yPos);
            yPos += contentFont.GetHeight() + 30;

            // Vẽ tiêu đề doanh thu
            e.Graphics.DrawString("💰 THỐNG KÊ DOANH THU", subHeaderFont, greenBrush, leftMargin, yPos);
            yPos += subHeaderFont.GetHeight() + 15;

            // Vẽ tổng doanh thu trong box
            Rectangle revenueBox = new Rectangle(leftMargin, (int)yPos, rightMargin - leftMargin, 40);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(240, 255, 240)), revenueBox);
            e.Graphics.DrawRectangle(new Pen(ButtonStyleHelper.SuccessGreen, 2), revenueBox);

            string totalRevenueText = "💰 TỔNG DOANH THU: " + lblTotalRevenue.Text.Replace("💰 ", "");
            SizeF revenueSize = e.Graphics.MeasureString(totalRevenueText, headerFont);
            e.Graphics.DrawString(totalRevenueText, headerFont, greenBrush,
                leftMargin + (revenueBox.Width - revenueSize.Width) / 2, yPos + 10);
            yPos += 60;

            // Vẽ thống kê theo ngày (nếu có dữ liệu chi tiết)
            if (dgvDoanhThu.Rows.Count > 0 && dgvDoanhThu.Rows.Count <= 10)
            {
                e.Graphics.DrawString("📊 Chi tiết doanh thu theo ngày:", subHeaderFont, brush, leftMargin, yPos);
                yPos += subHeaderFont.GetHeight() + 10;

                // Header cho bảng doanh thu
                float colWidth = (rightMargin - leftMargin) / 2;
                e.Graphics.DrawString("📅 Ngày", contentFont, brush, leftMargin, yPos);
                e.Graphics.DrawString("💰 Doanh Thu", contentFont, brush, leftMargin + colWidth, yPos);
                yPos += contentFont.GetHeight() + 5;

                // Vẽ đường kẻ header
                e.Graphics.DrawLine(Pens.Gray, leftMargin, yPos, rightMargin, yPos);
                yPos += 5;

                // Vẽ dữ liệu doanh thu
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

            // Vẽ tiêu đề món ăn bán chạy
            e.Graphics.DrawString("🏆 TOP MÓN ĂN BÁN CHẠY", subHeaderFont, new SolidBrush(ButtonStyleHelper.WarningOrange), leftMargin, yPos);
            yPos += subHeaderFont.GetHeight() + 15;

            // Vẽ danh sách món ăn bán chạy
            if (dgvMonBanChay.Rows.Count > 0)
            {
                // Header cho bảng món ăn
                float col1Width = (rightMargin - leftMargin) * 0.4f;
                float col2Width = (rightMargin - leftMargin) * 0.25f;
                float col3Width = (rightMargin - leftMargin) * 0.35f;

                e.Graphics.DrawString("🍽️ Tên Món", contentFont, brush, leftMargin, yPos);
                e.Graphics.DrawString("📊 Số Lượng", contentFont, brush, leftMargin + col1Width, yPos);
                e.Graphics.DrawString("💰 Doanh Thu", contentFont, brush, leftMargin + col1Width + col2Width, yPos);
                yPos += contentFont.GetHeight() + 5;

                // Vẽ đường kẻ header
                e.Graphics.DrawLine(Pens.Gray, leftMargin, yPos, rightMargin, yPos);
                yPos += 5;

                // Vẽ dữ liệu món ăn (tối đa 15 món để vừa trang)
                for (int i = 0; i < Math.Min(15, dgvMonBanChay.Rows.Count); i++)
                {
                    if (dgvMonBanChay.Rows[i].Cells["TenMon"].Value != null)
                    {
                        // Thêm emoji ranking cho top 3
                        string ranking = i == 0 ? "🥇 " : i == 1 ? "🥈 " : i == 2 ? "🥉 " : $"{i + 1}. ";

                        string tenMon = ranking + dgvMonBanChay.Rows[i].Cells["TenMon"].Value.ToString();
                        string soLuong = dgvMonBanChay.Rows[i].Cells["TongSoLuongBan"].Value.ToString();
                        string doanhThu = Convert.ToDecimal(dgvMonBanChay.Rows[i].Cells["TongDoanhThuMon"].Value).ToString("N0") + " VNĐ";

                        // Sử dụng font đậm cho top 3
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

            // Footer
            yPos = e.PageBounds.Height - 100;
            e.Graphics.DrawLine(new Pen(ButtonStyleHelper.PrimaryBlue, 1), leftMargin, yPos, rightMargin, yPos);
            yPos += 10;
            string footer = "🏪 Hệ thống FastFood Manager - Báo cáo được tạo tự động";
            e.Graphics.DrawString(footer, new Font("Arial", 8, FontStyle.Italic), brush, leftMargin, yPos);

            e.HasMorePages = false;
        }
    }
}
using PM_Ban_Do_An_Nhanh.BLL;
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
            this.Text = "Báo cáo thống kê";
            dtpTuNgay.Value = DateTime.Today.AddMonths(-1); // Mặc định 1 tháng trước
            dtpDenNgay.Value = DateTime.Today;
            dtpTuNgay.Checked = true;
            dtpDenNgay.Checked = true;
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
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                LoadDoanhThuReport(tuNgay, denNgay);
                LoadMonAnBanChayReport(tuNgay, denNgay);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDoanhThuReport(DateTime? tuNgay, DateTime? denNgay)
        {
            DataTable dtDoanhThu = donHangBLL.LayThongKeDoanhThu(tuNgay, denNgay);
            dgvDoanhThu.DataSource = dtDoanhThu;

            dgvDoanhThu.Columns["Ngay"].HeaderText = "Ngày";
            dgvDoanhThu.Columns["DoanhThuNgay"].HeaderText = "Doanh Thu";
            dgvDoanhThu.Columns["DoanhThuNgay"].DefaultCellStyle.Format = "N0";
            dgvDoanhThu.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            decimal tongDoanhThu = 0;
            if (dtDoanhThu != null && dtDoanhThu.Rows.Count > 0)
            {
                tongDoanhThu = dtDoanhThu.AsEnumerable().Sum(row => row.Field<decimal>("DoanhThuNgay"));
            }
            lblTotalRevenue.Text = tongDoanhThu.ToString("N0") + " VNĐ";
        }

        private void LoadMonAnBanChayReport(DateTime? tuNgay, DateTime? denNgay)
        {
            DataTable dtMonBanChay = donHangBLL.LayMonAnBanChay(tuNgay, denNgay, 10); // Lấy Top 10 món
            dgvMonBanChay.DataSource = dtMonBanChay;

            dgvMonBanChay.Columns["TenMon"].HeaderText = "Tên Món";
            dgvMonBanChay.Columns["TongSoLuongBan"].HeaderText = "Tổng SL Bán";
            dgvMonBanChay.Columns["TongDoanhThuMon"].HeaderText = "Tổng Doanh Thu";
            dgvMonBanChay.Columns["TongDoanhThuMon"].DefaultCellStyle.Format = "N0";
            dgvMonBanChay.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnInBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += new PrintPageEventHandler(PrintReportPage_PrintPage); 

                PrintPreviewDialog previewDlg = new PrintPreviewDialog();
                previewDlg.Document = printDoc;
                previewDlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuẩn bị in báo cáo: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReportPage_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Thiết lập font và brush để vẽ
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font contentFont = new Font("Arial", 10, FontStyle.Regular);
            Brush brush = Brushes.Black;

            // Vị trí bắt đầu
            float yPos = 50;
            int leftMargin = 50;
            int rightMargin = e.MarginBounds.Right - 50;

            // Vẽ tiêu đề chính
            string reportTitle = "BÁO CÁO THỐNG KÊ";
            e.Graphics.DrawString(reportTitle, titleFont, brush,
                new Point((e.PageBounds.Width - e.Graphics.MeasureString(reportTitle, titleFont).ToSize().Width) / 2, (int)yPos));
            yPos += titleFont.GetHeight() + 20;

            // Vẽ thông tin thời gian báo cáo
            string period = "Từ ngày: " + (dtpTuNgay.Checked ? dtpTuNgay.Value.ToShortDateString() : "Tất cả") +
                " đến ngày: " + (dtpDenNgay.Checked ? dtpDenNgay.Value.ToShortDateString() : "Tất cả");
            e.Graphics.DrawString(period, headerFont, brush, leftMargin, yPos);
            yPos += headerFont.GetHeight() + 20;

            // Vẽ tiêu đề doanh thu
            e.Graphics.DrawString("THỐNG KÊ DOANH THU", headerFont, brush, leftMargin, yPos);
            yPos += headerFont.GetHeight() + 10;

            // Vẽ tổng doanh thu
            e.Graphics.DrawString("Tổng doanh thu: " + lblTotalRevenue.Text, headerFont, brush, leftMargin, yPos);
            yPos += headerFont.GetHeight() + 20;

            // Vẽ tiêu đề món ăn bán chạy
            e.Graphics.DrawString("TOP MÓN ĂN BÁN CHẠY", headerFont, brush, leftMargin, yPos);
            yPos += headerFont.GetHeight() + 10;

            // Vẽ danh sách món ăn bán chạy
            if (dgvMonBanChay.Rows.Count > 0)
            {
                // Vẽ header cho bảng món ăn
                float colWidth = (rightMargin - leftMargin) / 3;
                e.Graphics.DrawString("Tên Món", contentFont, brush, leftMargin, yPos);
                e.Graphics.DrawString("Số Lượng", contentFont, brush, leftMargin + colWidth, yPos);
                e.Graphics.DrawString("Doanh Thu", contentFont, brush, leftMargin + colWidth * 2, yPos);
                yPos += contentFont.GetHeight() + 5;

                // Vẽ dữ liệu món ăn
                for (int i = 0; i < dgvMonBanChay.Rows.Count; i++)
                {
                    if (dgvMonBanChay.Rows[i].Cells["TenMon"].Value != null)
                    {
                        e.Graphics.DrawString(dgvMonBanChay.Rows[i].Cells["TenMon"].Value.ToString(),
                            contentFont, brush, leftMargin, yPos);
                        e.Graphics.DrawString(dgvMonBanChay.Rows[i].Cells["TongSoLuongBan"].Value.ToString(),
                            contentFont, brush, leftMargin + colWidth, yPos);
                        e.Graphics.DrawString(Convert.ToDecimal(dgvMonBanChay.Rows[i].Cells["TongDoanhThuMon"].Value).ToString("N0") + " VNĐ",
                            contentFont, brush, leftMargin + colWidth * 2, yPos);
                        yPos += contentFont.GetHeight() + 5;
                    }
                }
            }
            else
            {
                e.Graphics.DrawString("Không có dữ liệu món ăn bán chạy", contentFont, brush, leftMargin, yPos);
            }

            e.HasMorePages = false;
        }
    }
}


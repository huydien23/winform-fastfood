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

        public frmSales()
        {
            InitializeComponent();
            this.Text = "Quầy bán hàng";
            LoadMonAnToPanel();
            SetupOrderDataGridView();
            ClearOrder();
            LoadKhachHangToGrid();
            LoadHoaDonToGrid();
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

                    // Try to find image file in Images folder matching the item name
                    string imagePath = FindImageForMon(tenMon);
                    card.SetData(maMon, tenMon, gia, imagePath);
                    card.Width = 240;
                    card.Height = 104;
                    card.Margin = new Padding(6);

                    if (row["TrangThai"].ToString() == "Hết hàng")
                    {
                        card.Enabled = false;
                        card.BackColor = System.Drawing.Color.LightGray;
                    }

                    card.AddClicked += (s, e) =>
                    {
                        AddItemToOrderList(e.MaMon, e.TenMon, e.Price);
                    };

                    pnlMonAn.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMonAn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int maMon = Convert.ToInt32(clickedButton.Tag);

            try
            {
                DataTable dtMonAn = monAnBLL.HienThiDanhSachMonAn();
                DataRow selectedMonAnRow = dtMonAn.AsEnumerable().FirstOrDefault(r => r.Field<int>("MaMon") == maMon);

                if (selectedMonAnRow != null)
                {
                    string tenMon = selectedMonAnRow.Field<string>("TenMon");
                    decimal donGia = selectedMonAnRow.Field<decimal>("Gia");

                    AddItemToOrderList(maMon, tenMon, donGia);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món vào đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupOrderDataGridView()
        {
            dgvOrderList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrderList.Columns.Clear();
            dgvOrderList.Columns.Add("MaMon", "Mã Món");
            dgvOrderList.Columns["MaMon"].Visible = false;
            dgvOrderList.Columns.Add("TenMon", "Tên Món");

            dgvOrderList.Columns.Add("SoLuong", "SL");

            dgvOrderList.Columns.Add("DonGia", "Đơn Giá");
            dgvOrderList.Columns["DonGia"].DefaultCellStyle.Format = "N0";

            dgvOrderList.Columns.Add("ThanhTien", "Thành Tiền");
            dgvOrderList.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";


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

        private void UpdateOrderDisplay()
        {
            dgvOrderList.Rows.Clear();
            decimal totalAmount = 0;
            foreach (var item in currentOrderItems)
            {
                decimal thanhTien = item.SoLuong * item.DonGia;
                dgvOrderList.Rows.Add(item.MaMon, item.TenMon, item.SoLuong, item.DonGia, thanhTien);
                totalAmount += thanhTien;
            }
            lblTongTien.Text = totalAmount.ToString("N0") + " VNĐ";
        }

        private string FindImageForMon(string tenMon)
        {
            try
            {
                string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                if (!Directory.Exists(imagesDir)) return null;

                // normalize name: remove diacritics, spaces, to lower
                string normalized = RemoveDiacritics(tenMon).ToLowerInvariant();
                normalized = System.Text.RegularExpressions.Regex.Replace(normalized, "[^a-z0-9]", "-");
                // try to match any file that contains normalized tokens
                foreach (var file in Directory.GetFiles(imagesDir))
                {
                    string name = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                    if (name.Contains(normalized) || normalized.Contains(name))
                        return file;
                }

                // fallback: try simple token match
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
            var stringBuilder = new System.Text.StringBuilder();

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

        private void ClearOrder()
        {
            currentOrderItems.Clear();
            UpdateOrderDisplay();
            ClearCustomerInfo();
        }

        private void ClearCustomerInfo()
        {
            txtSDTKhachHang.Clear();
            lblTenKhachHang.Text = "Chưa chọn khách hàng";
            selectedCustomer = null;
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
                lblTenKhachHang.Text = $"Khách hàng: {selectedCustomer.TenKH} (SĐT: {selectedCustomer.SDT})";
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
                // Lọc kết quả theo SDT hoặc tên khách hàng
                var filteredRows = dtKhachHang.AsEnumerable()
                    .Where(r =>
                        r.Field<string>("SDT") == input ||
                        r.Field<string>("TenKH").IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0);

                if (filteredRows.Any())
                {
                    dgvKhachHang.DataSource = filteredRows.CopyToDataTable();
                    dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                    dgvKhachHang.Columns["TenKH"].HeaderText = "Tên Khách Hàng";
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

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (currentOrderItems.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm món ăn vào đơn hàng để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tongTien = currentOrderItems.Sum(item => item.SoLuong * item.DonGia);

            DonHang donHangMoi = new DonHang
            {
                NgayLap = DateTime.Now,
                TongTien = tongTien,
                TrangThaiThanhToan = "Đã thanh toán",
                MaKH = selectedCustomer?.MaKH
            };

            try
            {
                int maDonHangMoi = donHangBLL.ThemDonHang(donHangMoi, currentOrderItems);
                if (maDonHangMoi > 0)
                {
                    MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InHoaDon(maDonHangMoi);
                    ClearOrder(); // Reset giao diện sau khi thanh toán
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void btnPrintBill_Click(object sender, EventArgs e)
        {
            if (selectedMaDH == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            InHoaDon(selectedMaDH.Value);
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

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHoaDon.Rows[e.RowIndex].Cells["MaDH"].Value != null)
            {
                selectedMaDH = Convert.ToInt32(dgvHoaDon.Rows[e.RowIndex].Cells["MaDH"].Value);
            }
        }

        private void frmSales_Load(object sender, EventArgs e)
        {
            LoadHoaDonToGrid();
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

                        // Tiêu đề
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

                        // Bảng chi tiết món
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
                        pdfDoc.Add(new Paragraph($"Tổng cộng: {Convert.ToDecimal(dtHoaDon.Rows[0]["TongTien"]):N0} VNĐ", normalFont));
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

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (selectedMaDH == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xuất PDF.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ExportHoaDonToPdf(selectedMaDH.Value);
        }
    }
}

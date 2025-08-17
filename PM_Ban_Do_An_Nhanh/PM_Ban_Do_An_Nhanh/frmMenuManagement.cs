using PM_Ban_Do_An_Nhanh.BLL;
using PM_Ban_Do_An_Nhanh.Helpers;
using PM_Ban_Do_An_Nhanh.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PM_Ban_Do_An_Nhanh
{
    public partial class frmMenuManagement : Form
    {
        private MonAnBLL monAnBLL = new MonAnBLL();
        private DanhMucBLL danhMucBLL = new DanhMucBLL();
        private int selectedMonAnId = -1;
        private string selectedImagePath = ""; // Đường dẫn ảnh được chọn

        public frmMenuManagement()
        {
            InitializeComponent();
            this.Text = "🍽️ Quản lý thực đơn";
            LoadDataToDataGridView();
            LoadDanhMucToComboBox();
            SetupTrangThaiComboBox();
            ClearInputFields();
            SetupButtonStyles();
        }

        private void SetupButtonStyles()
        {
            // Style buttons với icons và tooltips
            ButtonStyleHelper.ApplySuccessStyle(btnThem, "➕ Thêm món", "Thêm món ăn mới vào thực đơn", ButtonSize.Medium);
            ButtonStyleHelper.ApplyWarningStyle(btnSua, "✏️ Sửa món", "Chỉnh sửa thông tin món ăn đã chọn", ButtonSize.Medium);
            ButtonStyleHelper.ApplyDangerStyle(btnXoa, "🗑️ Xóa món", "Xóa món ăn đã chọn khỏi thực đơn", ButtonSize.Medium);
            ButtonStyleHelper.ApplyInfoStyle(btnLamMoi, "🔄 Làm mới", "Làm mới danh sách và xóa form", ButtonSize.Medium);
            ButtonStyleHelper.ApplyPrimaryStyle(btnChonHinh, "🖼️ Chọn hình", "Chọn hình ảnh cho món ăn", ButtonSize.Medium);

            // Style các controls khác
            ButtonStyleHelper.ApplyModernTextBoxStyle(txtTenMon);
            ButtonStyleHelper.ApplyModernTextBoxStyle(txtGia);
            ButtonStyleHelper.ApplyModernComboBoxStyle(cboDanhMuc);
            ButtonStyleHelper.ApplyModernComboBoxStyle(cboTrangThai);

            // Style DataGridView
            ButtonStyleHelper.ApplyModernDataGridViewStyle(dgvMonAn, "warning");

            // Style form
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 10F);

            // Style PictureBox
            picHinhAnh.BorderStyle = BorderStyle.FixedSingle;
            picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
            picHinhAnh.BackColor = Color.White;
        }

        private void LoadDataToDataGridView()
        {
            try
            {
                dgvMonAn.DataSource = monAnBLL.HienThiDanhSachMonAn();
                dgvMonAn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Thiết lập tiêu đề cột với icons
                dgvMonAn.Columns["MaMon"].HeaderText = "🔢 Mã Món";
                dgvMonAn.Columns["TenMon"].HeaderText = "🍽️ Tên Món";
                dgvMonAn.Columns["Gia"].HeaderText = "💰 Giá";
                dgvMonAn.Columns["Gia"].DefaultCellStyle.Format = "N0";
                dgvMonAn.Columns["MaDM"].Visible = false;
                dgvMonAn.Columns["TenDM"].HeaderText = "📂 Danh Mục";
                dgvMonAn.Columns["TrangThai"].HeaderText = "📊 Trạng Thái";
                dgvMonAn.Columns["HinhAnh"].Visible = false; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu món ăn: " + ex.Message, "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhMucToComboBox()
        {
            try
            {
                DataTable dtDanhMuc = danhMucBLL.LayDanhSachDanhMuc();
                cboDanhMuc.DataSource = dtDanhMuc;
                cboDanhMuc.DisplayMember = "TenDM";
                cboDanhMuc.ValueMember = "MaDM";
                cboDanhMuc.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message, "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTrangThaiComboBox()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Còn hàng");
            cboTrangThai.Items.Add("Hết hàng");
            cboTrangThai.SelectedIndex = 0;
        }

        private void dgvMonAn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var value = dgvMonAn.Rows[e.RowIndex].Cells["MaMon"].Value;
                if (value != null && int.TryParse(value.ToString(), out int id))
                    selectedMonAnId = id;
                else
                    selectedMonAnId = -1;

                txtTenMon.Text = dgvMonAn.Rows[e.RowIndex].Cells["TenMon"].Value.ToString();
                txtGia.Text = dgvMonAn.Rows[e.RowIndex].Cells["Gia"].Value.ToString();
                cboDanhMuc.SelectedValue = dgvMonAn.Rows[e.RowIndex].Cells["MaDM"].Value;
                cboTrangThai.SelectedItem = dgvMonAn.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();

                // Load ảnh hiện tại
                string imagePath = dgvMonAn.Rows[e.RowIndex].Cells["HinhAnh"].Value?.ToString();
                LoadImageToPictureBox(imagePath);
            }
        }

        private void LoadImageToPictureBox(string imagePath)
        {
            try
            {
                var image = ImageHelper.LoadMenuItemImage(imagePath);
                if (image != null)
                {
                    picHinhAnh.Image?.Dispose(); // Dispose ảnh cũ
                    picHinhAnh.Image = image;
                }
                else
                {
                    picHinhAnh.Image?.Dispose();
                    picHinhAnh.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}", "⚠️ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                picHinhAnh.Image?.Dispose();
                picHinhAnh.Image = null;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string tenMon = txtTenMon.Text;
            decimal gia = Convert.ToDecimal(txtGia.Text);
            int maDM = (int)cboDanhMuc.SelectedValue;
            string trangThai = cboTrangThai.SelectedItem.ToString();

            try
            {
                if (monAnBLL.ThemMonAn(tenMon, gia, maDM, trangThai, selectedImagePath))
                {
                    MessageBox.Show("Thêm món ăn thành công! 🎉", "✅ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Thêm món ăn thất bại. Vui lòng kiểm tra lại.", "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "❌ Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMonAnId == -1)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần sửa.", "⚠️ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput()) return;

            string tenMon = txtTenMon.Text;
            decimal gia = Convert.ToDecimal(txtGia.Text);
            int maDM = (int)cboDanhMuc.SelectedValue;
            string trangThai = cboTrangThai.SelectedItem.ToString();

            try
            {
                if (monAnBLL.SuaMonAn(selectedMonAnId, tenMon, gia, maDM, trangThai, selectedImagePath))
                {
                    MessageBox.Show("Sửa món ăn thành công! 🎉", "✅ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Sửa món ăn thất bại. Vui lòng kiểm tra lại.", "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "❌ Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMonAnId == -1)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần xóa.", "⚠️ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa món ăn '{txtTenMon.Text}'?\n\n⚠️ Lưu ý: Món ăn sẽ bị xóa vĩnh viễn!", "🗑️ Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (monAnBLL.XoaMonAn(selectedMonAnId))
                    {
                        MessageBox.Show("Xóa món ăn thành công! 🎉", "✅ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataToDataGridView();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Xóa món ăn thất bại. Có thể món ăn này đang được sử dụng trong các đơn hàng hoặc có ràng buộc khác.", "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "❌ Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            LoadDataToDataGridView();
            MessageBox.Show("Đã làm mới dữ liệu! 🔄", "🔄 Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món ăn.", "⚠️ Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenMon.Focus();
                return false;
            }
            if (!decimal.TryParse(txtGia.Text, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Giá phải là một số hợp lệ và lớn hơn 0.", "⚠️ Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGia.Focus();
                return false;
            }
            if (cboDanhMuc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cho món ăn.", "⚠️ Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboDanhMuc.Focus();
                return false;
            }
            if (cboTrangThai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái món ăn.", "⚠️ Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboTrangThai.Focus();
                return false;
            }
            return true;
        }

        private void ClearInputFields()
        {
            selectedMonAnId = -1;
            selectedImagePath = "";
            txtTenMon.Clear();
            txtGia.Clear();
            cboDanhMuc.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = 0;
            picHinhAnh.Image?.Dispose();
            picHinhAnh.Image = null;
            txtTenMon.Focus();
        }

        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
                openFileDialog.Title = "Chọn hình ảnh món ăn";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        selectedImagePath = openFileDialog.FileName;
                        picHinhAnh.Image?.Dispose();
                        picHinhAnh.Image = Image.FromFile(selectedImagePath);
                        MessageBox.Show("Đã chọn hình ảnh thành công! 📷", "✅ Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}", "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        selectedImagePath = "";
                    }
                }
            }
        }
    }
}
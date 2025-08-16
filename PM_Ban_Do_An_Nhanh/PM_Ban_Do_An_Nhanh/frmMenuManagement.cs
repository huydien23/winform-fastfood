using PM_Ban_Do_An_Nhanh.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public frmMenuManagement()
        {
            InitializeComponent();
            this.Text = "Quản lý thực đơn";
            LoadDataToDataGridView();
            LoadDanhMucToComboBox();
            SetupTrangThaiComboBox();
            ClearInputFields();
        }

        private void LoadDataToDataGridView()
        {
            try
            {
                dgvMonAn.DataSource = monAnBLL.HienThiDanhSachMonAn();
                dgvMonAn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvMonAn.Columns["MaMon"].HeaderText = "Mã Món";
                dgvMonAn.Columns["TenMon"].HeaderText = "Tên Món";
                dgvMonAn.Columns["Gia"].HeaderText = "Giá";
                dgvMonAn.Columns["Gia"].DefaultCellStyle.Format = "N0"; // Định dạng tiền tệ
                dgvMonAn.Columns["MaDM"].Visible = false; // Ẩn cột mã danh mục
                dgvMonAn.Columns["TenDM"].HeaderText = "Danh Mục";
                dgvMonAn.Columns["TrangThai"].HeaderText = "Trạng Thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTrangThaiComboBox()
        {
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
                if (monAnBLL.ThemMonAn(tenMon, gia, maDM, trangThai))
                {
                    MessageBox.Show("Thêm món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Thêm món ăn thất bại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMonAnId == -1)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput()) return;

            string tenMon = txtTenMon.Text;
            decimal gia = Convert.ToDecimal(txtGia.Text);
            int maDM = (int)cboDanhMuc.SelectedValue;
            string trangThai = cboTrangThai.SelectedItem.ToString();

            try
            {
                if (monAnBLL.SuaMonAn(selectedMonAnId, tenMon, gia, maDM, trangThai))
                {
                    MessageBox.Show("Sửa món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Sửa món ăn thất bại. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMonAnId == -1)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa món ăn này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (monAnBLL.XoaMonAn(selectedMonAnId))
                    {
                        MessageBox.Show("Xóa món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataToDataGridView();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Xóa món ăn thất bại. Có thể món ăn này đang được sử dụng trong các đơn hàng hoặc có ràng buộc khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            LoadDataToDataGridView();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món ăn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!decimal.TryParse(txtGia.Text, out decimal gia))
            {
                MessageBox.Show("Giá phải là một số hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (cboDanhMuc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cho món ăn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (cboTrangThai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái món ăn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ClearInputFields()
        {
            selectedMonAnId = -1;
            txtTenMon.Clear();
            txtGia.Clear();
            cboDanhMuc.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = 0; // Mặc định là 'Còn hàng'
            txtTenMon.Focus();
        }
    }
}



using PM_Ban_Do_An_Nhanh.BLL;
using PM_Ban_Do_An_Nhanh.Entities;
using PM_Ban_Do_An_Nhanh.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PM_Ban_Do_An_Nhanh
{
    public partial class frmUserManagement : Form
    {
        private TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
        private int selectedMaTK = 0;

        public frmUserManagement()
        {
            InitializeComponent();
            this.Load += FrmUserManagement_Load;
        }

        private void FrmUserManagement_Load(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra quyền Admin
                SessionContext.RequireAdmin();
                
                LoadData();
                SetupUI();
                ClearForm();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void SetupUI()
        {
            // Setup ComboBox Role
            cboRole.Items.Clear();
            cboRole.Items.Add("Admin");
            cboRole.Items.Add("Staff");
            cboRole.SelectedIndex = 1; // Default Staff

            // Setup DataGridView
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.CellClick += DgvUsers_CellClick;
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = taiKhoanBLL.GetAllTaiKhoan();
                dgvUsers.DataSource = dt;

                // Đặt tiêu đề cột
                if (dgvUsers.Columns.Count > 0)
                {
                    dgvUsers.Columns["MaTK"].HeaderText = "Mã TK";
                    dgvUsers.Columns["TenTK"].HeaderText = "Tên hiển thị";
                    dgvUsers.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                    dgvUsers.Columns["Role"].HeaderText = "Vai trò";
                    dgvUsers.Columns["Email"].HeaderText = "Email";
                    dgvUsers.Columns["IsActive"].HeaderText = "Hoạt động";

                    // Format cột IsActive
                    dgvUsers.Columns["IsActive"].DefaultCellStyle.Format = "Có;Không";
                }

                lblTotal.Text = $"Tổng số: {dt.Rows.Count} tài khoản";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedMaTK = Convert.ToInt32(row.Cells["MaTK"].Value);
                txtTenTK.Text = row.Cells["TenTK"].Value.ToString();
                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value.ToString();
                cboRole.SelectedItem = row.Cells["Role"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                chkIsActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);

                // Chuyển sang chế độ sửa
                btnAdd.Text = "Thêm mới";
                btnEdit.Enabled = true;
                btnChangePassword.Enabled = true;
                btnToggleActive.Enabled = true;
                btnToggleActive.Text = chkIsActive.Checked ? "Khóa tài khoản" : "Mở khóa";
            }
        }

        private void ClearForm()
        {
            selectedMaTK = 0;
            txtTenTK.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtEmail.Clear();
            cboRole.SelectedIndex = 1; // Default Staff
            chkIsActive.Checked = true;

            btnAdd.Text = "Thêm mới";
            btnEdit.Enabled = false;
            btnChangePassword.Enabled = false;
            btnToggleActive.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu cho tài khoản mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                TaiKhoan tk = new TaiKhoan
                {
                    TenTK = txtTenTK.Text.Trim(),
                    TenDangNhap = txtTenDangNhap.Text.Trim(),
                    MatKhau = txtMatKhau.Text.Trim(),
                    Role = cboRole.SelectedItem.ToString(),
                    Email = txtEmail.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                if (taiKhoanBLL.ThemTaiKhoan(tk))
                {
                    // Log audit
                    AuditLogger.LogCreate("TaiKhoan", 0, tk.TenDangNhap, $"Tạo tài khoản: {tk.TenTK} ({tk.Role})");
                    
                    MessageBox.Show("Thêm tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMaTK == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TaiKhoan tk = new TaiKhoan
                {
                    MaTK = selectedMaTK,
                    TenTK = txtTenTK.Text.Trim(),
                    TenDangNhap = txtTenDangNhap.Text.Trim(),
                    Role = cboRole.SelectedItem.ToString(),
                    Email = txtEmail.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                if (taiKhoanBLL.SuaTaiKhoan(tk))
                {
                    // Log audit
                    AuditLogger.LogUpdate("TaiKhoan", selectedMaTK, 
                        $"Old: {txtTenDangNhap.Text}", 
                        $"New: {tk.TenTK} ({tk.Role})", 
                        $"Cập nhật tài khoản: {tk.TenDangNhap}");
                    
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMaTK == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần đổi mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newPassword = Microsoft.VisualBasic.Interaction.InputBox(
                    "Nhập mật khẩu mới (tối thiểu 6 ký tự):",
                    "Đổi mật khẩu",
                    ""
                );

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    return;
                }

                if (taiKhoanBLL.DoiMatKhau(selectedMaTK, newPassword))
                {
                    // Log audit
                    AuditLogger.Log("UPDATE", "TaiKhoan", selectedMaTK, null, "***", "Đổi mật khẩu");
                    
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnToggleActive_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMaTK == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Không cho phép tự khóa tài khoản của mình
                if (selectedMaTK == SessionContext.CurrentUser.MaTK)
                {
                    MessageBox.Show("Bạn không thể khóa tài khoản của chính mình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool newStatus = !chkIsActive.Checked;
                string action = newStatus ? "mở khóa" : "khóa";

                if (MessageBox.Show($"Bạn có chắc muốn {action} tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (taiKhoanBLL.ToggleActiveStatus(selectedMaTK, newStatus))
                    {
                        // Log audit
                        string actionDesc = newStatus ? "Mở khóa" : "Khóa";
                        AuditLogger.Log("UPDATE", "TaiKhoan", selectedMaTK, 
                            chkIsActive.Checked.ToString(), 
                            newStatus.ToString(), 
                            $"{actionDesc} tài khoản");
                        
                        MessageBox.Show($"Đã {action} tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }
    }
}

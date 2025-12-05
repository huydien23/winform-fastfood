using PM_Ban_Do_An_Nhanh.Helpers;
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
    public partial class frmMain : Form
    {
        private TabPage tabPageDanhMuc;

        public frmMain()
        {
            InitializeComponent();
            this.Text = "Hệ thống quản lý bán thức ăn nhanh";
            HienThiThongTinNguoiDung();
            ApplyRoleBasedPermissions();

            
            tabPageDanhMuc = new TabPage("Danh Mục");
            tabControlMain.TabPages.Add(tabPageDanhMuc);
        }

        private void HienThiThongTinNguoiDung()
        {
            if (SessionContext.IsLoggedIn)
            {
                string roleDisplay = SessionContext.IsAdmin ? "[Admin]" : "[Nhân viên]";
                lblUserInfo.Text = $"Xin chào, {SessionContext.DisplayName}\n{roleDisplay}";
                btnSales.Enabled = true;
                btnMenuManagement.Enabled = true;
                btnReport.Enabled = true;
            }
            else
            {
                MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập. Vui lòng đăng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
            }
        }

        private void ApplyRoleBasedPermissions()
        {
            // Chỉ Admin mới thấy các chức năng quản lý
            if (SessionContext.IsAdmin)
            {
                // Admin có full quyền
                btnMenuManagement.Visible = true;
                btnDanhMuc.Visible = true;
                btnReport.Visible = true;
                // Thêm button quản lý tài khoản cho Admin
                AddUserManagementButton();
            }
            else if (SessionContext.IsStaff)
            {
                // Staff chỉ được bán hàng và xem khách hàng
                btnMenuManagement.Visible = false;
                btnDanhMuc.Visible = false;
                btnReport.Visible = false;
            }
        }

        private void AddUserManagementButton()
        {
            // Tạo button quản lý tài khoản động
            Button btnUserManagement = new Button();
            btnUserManagement.Name = "btnUserManagement";
            btnUserManagement.Text = "Quản lý tài khoản";
            btnUserManagement.Font = new Font("Arial", 10.2F, FontStyle.Bold);
            btnUserManagement.Dock = DockStyle.Top;
            btnUserManagement.Height = 46;
            btnUserManagement.Click += BtnUserManagement_Click;
            
            // Tạo spacer panel để có khoảng cách giống các button khác
            Panel spacerPanel = new Panel();
            spacerPanel.Dock = DockStyle.Top;
            spacerPanel.Height = 29;
            
            // Thêm vào panel1
            panel1.Controls.Add(btnUserManagement);
            panel1.Controls.Add(spacerPanel);
            
            // Tìm vị trí của btnDanhMuc để đặt button mới ngay sau nó
            int danhMucIndex = panel1.Controls.IndexOf(btnDanhMuc);
            if (danhMucIndex >= 0)
            {
                // Đặt spacer ngay sau btnDanhMuc
                panel1.Controls.SetChildIndex(spacerPanel, danhMucIndex);
                // Đặt button ngay sau spacer
                panel1.Controls.SetChildIndex(btnUserManagement, danhMucIndex);
            }
        }

        private void BtnUserManagement_Click(object sender, EventArgs e)
        {
            try
            {
                SessionContext.RequireAdmin();
                frmUserManagement userMgmtForm = new frmUserManagement();
                userMgmtForm.ShowDialog();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            frmSales salesForm = new frmSales();
            LoadFormInTabPage(salesForm, tabPageSales);
            tabControlMain.SelectedTab = tabPageSales;
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            frmAddCustomer khachHangForm = new frmAddCustomer();
            LoadFormInTabPage(khachHangForm, tabPageCustomer);
            tabControlMain.SelectedTab = tabPageCustomer;
        }

        private void btnMenuManagement_Click(object sender, EventArgs e)
        {
            frmMenuManagement menuForm = new frmMenuManagement();
            LoadFormInTabPage(menuForm, tabPageMenu);
            tabControlMain.SelectedTab = tabPageMenu;
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            frmReport reportForm = new frmReport();
            LoadFormInTabPage(reportForm, tabPageReport);
            tabControlMain.SelectedTab = tabPageReport;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất khỏi hệ thống không?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GlobalVariables.LoggedInUser = null;
                SessionContext.Logout();
                this.Hide();
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void LoadFormInTabPage(Form form, TabPage tabPage)
        {
            tabPage.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tabPage.Controls.Add(form);
            form.Show();
        }

        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            frmDanhMuc danhmucForm = new frmDanhMuc();
            LoadFormInTabPage(danhmucForm, tabPageDanhMuc);
            tabControlMain.SelectedTab = tabPageDanhMuc;
        }
    }
}

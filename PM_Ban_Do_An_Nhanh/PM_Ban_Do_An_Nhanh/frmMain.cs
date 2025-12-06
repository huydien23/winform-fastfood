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
            this.WindowState = FormWindowState.Maximized; // Responsive layout
            this.KeyPreview = true; // Enable keyboard shortcuts
            HienThiThongTinNguoiDung();
            ApplyRoleBasedPermissions();
            SetupTooltips(); // Setup tooltips

            
            tabPageDanhMuc = new TabPage("Danh Mục");
            tabControlMain.TabPages.Add(tabPageDanhMuc);
        }

        private void SetupTooltips()
        {
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            toolTip1.ShowAlways = true;

            // Navigation buttons
            toolTip1.SetToolTip(btnSales, "Quản lý bán hàng và đơn hàng (F2)");
            toolTip1.SetToolTip(btnKhachHang, "Quản lý thông tin khách hàng (F3)");
            toolTip1.SetToolTip(btnMenuManagement, "Quản lý món ăn và thực đơn (F4)");
            toolTip1.SetToolTip(btnReport, "Xem báo cáo và thống kê (F6)");
            toolTip1.SetToolTip(btnDanhMuc, "Quản lý danh mục món ăn (F7 - Admin)");
            toolTip1.SetToolTip(btnLogout, "Đăng xuất khỏi hệ thống");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // F-keys shortcuts
            switch (keyData)
            {
                case Keys.F2: // Bán hàng
                    if (btnSales.Enabled)
                    {
                        btnSales_Click(null, null);
                        return true;
                    }
                    break;

                case Keys.F3: // Khách hàng
                    if (btnKhachHang.Enabled)
                    {
                        btnKhachHang_Click(null, null);
                        return true;
                    }
                    break;

                case Keys.F4: // Quản lý món ăn
                    if (btnMenuManagement.Enabled && btnMenuManagement.Visible)
                    {
                        btnMenuManagement_Click(null, null);
                        return true;
                    }
                    break;

                case Keys.F5: // Refresh current tab
                    RefreshCurrentTab();
                    return true;

                case Keys.F6: // Báo cáo
                    if (btnReport.Enabled && btnReport.Visible)
                    {
                        btnReport_Click(null, null);
                        return true;
                    }
                    break;

                case Keys.F7: // Danh mục (Admin only)
                    if (btnDanhMuc != null && btnDanhMuc.Visible)
                    {
                        btnDanhMuc_Click(null, null);
                        return true;
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RefreshCurrentTab()
        {
            // Refresh data in current active tab
            if (tabControlMain.SelectedTab != null)
            {
                foreach (Control ctrl in tabControlMain.SelectedTab.Controls)
                {
                    if (ctrl is Form form && form.GetType().GetMethod("LoadData") != null)
                    {
                        form.GetType().GetMethod("LoadData").Invoke(form, null);
                    }
                }
            }
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

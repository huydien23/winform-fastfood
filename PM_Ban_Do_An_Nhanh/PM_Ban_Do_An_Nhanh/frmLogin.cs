using PM_Ban_Do_An_Nhanh.BLL;
using PM_Ban_Do_An_Nhanh.Entities;
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
    public partial class frmLogin : Form
    {
        private TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();

        public frmLogin()
        {
            InitializeComponent();
            this.Text = "Đăng nhập hệ thống";
            txtUsername.Text = "admin";
            txtPassword.Text = "admin123";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TaiKhoan loggedInUser = taiKhoanBLL.KiemTraDangNhap(username, password);

                if (loggedInUser != null)
                {
                    GlobalVariables.LoggedInUser = loggedInUser;

                    MessageBox.Show($"Chào mừng {loggedInUser.TenTK}!", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmMain mainForm = new frmMain();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối hoặc xử lý dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát ứng dụng không?", "Thoát ứng dụng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }

    public static class GlobalVariables
    {
        public static TaiKhoan LoggedInUser { get; set; }
    }
}

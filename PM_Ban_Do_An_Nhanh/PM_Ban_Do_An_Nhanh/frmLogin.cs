using PM_Ban_Do_An_Nhanh.BLL;
using PM_Ban_Do_An_Nhanh.DAL;
using PM_Ban_Do_An_Nhanh.Entities;
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
                // Kiểm tra tài khoản có bị khóa không
                var lockInfo = new TaiKhoanDAL().IsAccountLocked(username);
                if (lockInfo.IsLocked)
                {
                    TimeSpan timeRemaining = lockInfo.LockedUntil.Value - DateTime.Now;
                    int minutesRemaining = (int)Math.Ceiling(timeRemaining.TotalMinutes);
                    MessageBox.Show(
                        $"⚠️ Tài khoản đã bị khóa do đăng nhập sai quá nhiều lần.\n\n" +
                        $"Thời gian còn lại: {minutesRemaining} phút\n\n" +
                        $"Vui lòng thử lại sau.",
                        "Tài khoản bị khóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                TaiKhoan loggedInUser = taiKhoanBLL.KiemTraDangNhap(username, password);

                if (loggedInUser != null)
                {
                    // Reset failed attempts khi login thành công
                    new TaiKhoanDAL().ResetFailedAttempts(username);
                    
                    // Lưu vào cả GlobalVariables (backward compatibility) và SessionContext
                    GlobalVariables.LoggedInUser = loggedInUser;
                    SessionContext.CurrentUser = loggedInUser;

                    string roleDisplay = loggedInUser.Role == "Admin" ? "Quản trị viên" : "Nhân viên";
                    MessageBox.Show($"Chào mừng {loggedInUser.TenTK}!\\nVai trò: {roleDisplay}", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmMain mainForm = new frmMain();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    // Tăng failed attempts khi login sai
                    new TaiKhoanDAL().IncrementFailedAttempts(username);
                    
                    // Lấy thông tin số lần sai hiện tại
                    var info = new TaiKhoanDAL().IsAccountLocked(username);
                    int remainingAttempts = 5 - info.FailedAttempts;
                    
                    if (remainingAttempts > 0)
                    {
                        MessageBox.Show(
                            $"❌ Tên đăng nhập hoặc mật khẩu không đúng.\n\n" +
                            $"Số lần thử còn lại: {remainingAttempts}\n" +
                            $"⚠️ Tài khoản sẽ bị khóa 15 phút sau 5 lần sai.",
                            "Lỗi đăng nhập",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            $"⚠️ Tài khoản đã bị khóa do đăng nhập sai 5 lần.\n\n" +
                            $"Vui lòng thử lại sau 15 phút.",
                            "Tài khoản bị khóa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối hoặc xử lý dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
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

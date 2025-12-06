using System;
using System.Windows.Forms;

namespace PM_Ban_Do_An_Nhanh.Helpers
{
    /// <summary>
    /// Helper để hiển thị error messages thân thiện
    /// </summary>
    public static class ErrorHandler
    {
        public static void ShowError(string message, string title = "Lỗi")
        {
            MessageBox.Show(
                $"❌ {message}\n\nVui lòng thử lại hoặc liên hệ quản trị viên nếu lỗi vẫn tiếp diễn.",
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        public static void ShowWarning(string message, string title = "Cảnh báo")
        {
            MessageBox.Show(
                $"⚠️ {message}",
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        public static void ShowInfo(string message, string title = "Thông báo")
        {
            MessageBox.Show(
                $"ℹ️ {message}",
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        public static void ShowSuccess(string message, string title = "Thành công")
        {
            MessageBox.Show(
                $"✅ {message}",
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        public static bool Confirm(string message, string title = "Xác nhận")
        {
            return MessageBox.Show(
                $"❓ {message}",
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes;
        }

        /// <summary>
        /// Handle exception và hiển thị message thân thiện
        /// </summary>
        public static void HandleException(Exception ex, string context = "")
        {
            string message = GetFriendlyMessage(ex);
            if (!string.IsNullOrEmpty(context))
            {
                message = $"{context}\n\n{message}";
            }

            ShowError(message, "Đã xảy ra lỗi");

            // Log to file (optional)
            LogError(ex, context);
        }

        private static string GetFriendlyMessage(Exception ex)
        {
            // Convert technical errors to user-friendly messages
            if (ex is System.Data.SqlClient.SqlException)
            {
                return "Không thể kết nối đến cơ sở dữ liệu.\nVui lòng kiểm tra kết nối mạng.";
            }
            else if (ex is UnauthorizedAccessException)
            {
                return "Bạn không có quyền thực hiện thao tác này.";
            }
            else if (ex is System.IO.FileNotFoundException)
            {
                return "Không tìm thấy file cần thiết.\nVui lòng kiểm tra lại.";
            }
            else if (ex is ArgumentException || ex is FormatException)
            {
                return "Dữ liệu nhập vào không hợp lệ.\nVui lòng kiểm tra lại thông tin.";
            }
            else
            {
                return $"Đã xảy ra lỗi không mong muốn.\n\nChi tiết: {ex.Message}";
            }
        }

        private static void LogError(Exception ex, string context)
        {
            try
            {
                string logPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "error_log.txt"
                );

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {context}\n{ex}\n\n";
                System.IO.File.AppendAllText(logPath, logEntry);
            }
            catch
            {
                // Ignore logging errors
            }
        }
    }
}

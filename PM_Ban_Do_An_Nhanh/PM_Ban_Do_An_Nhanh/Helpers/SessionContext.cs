using PM_Ban_Do_An_Nhanh.Entities;
using System;

namespace PM_Ban_Do_An_Nhanh.Helpers
{
    /// <summary>
    /// Lưu trữ thông tin phiên làm việc của user hiện tại
    /// </summary>
    public static class SessionContext
    {
        private static TaiKhoan _currentUser;

        /// <summary>
        /// Tài khoản đang đăng nhập
        /// </summary>
        public static TaiKhoan CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        /// <summary>
        /// Kiểm tra có user đang đăng nhập không
        /// </summary>
        public static bool IsLoggedIn
        {
            get { return _currentUser != null; }
        }

        /// <summary>
        /// Kiểm tra user hiện tại có phải Admin không
        /// </summary>
        public static bool IsAdmin
        {
            get { return _currentUser != null && _currentUser.Role == "Admin"; }
        }

        /// <summary>
        /// Kiểm tra user hiện tại có phải Staff không
        /// </summary>
        public static bool IsStaff
        {
            get { return _currentUser != null && _currentUser.Role == "Staff"; }
        }

        /// <summary>
        /// Lấy tên hiển thị của user
        /// </summary>
        public static string DisplayName
        {
            get { return _currentUser?.TenTK ?? "Guest"; }
        }

        /// <summary>
        /// Lấy role của user
        /// </summary>
        public static string Role
        {
            get { return _currentUser?.Role ?? ""; }
        }

        /// <summary>
        /// Đăng xuất - xóa thông tin session
        /// </summary>
        public static void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Kiểm tra quyền truy cập (throw exception nếu không đủ quyền)
        /// </summary>
        public static void RequireAdmin()
        {
            if (!IsAdmin)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền truy cập chức năng này. Chỉ Admin mới được phép.");
            }
        }

        /// <summary>
        /// Kiểm tra quyền truy cập (return false nếu không đủ quyền)
        /// </summary>
        public static bool CheckAdminPermission()
        {
            return IsAdmin;
        }
    }
}

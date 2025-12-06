using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Ban_Do_An_Nhanh.Entities
{
    public class TaiKhoan
    {
        public int MaTK { get; set; }
        public string TenTK { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Role { get; set; } // "Admin" hoặc "Staff"
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Login attempt tracking
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LastFailedLoginTime { get; set; }
        public DateTime? LockedUntil { get; set; }
    }
}

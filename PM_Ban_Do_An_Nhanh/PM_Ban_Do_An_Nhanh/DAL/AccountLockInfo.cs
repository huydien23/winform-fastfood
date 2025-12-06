using System;

namespace PM_Ban_Do_An_Nhanh.DAL
{
    /// <summary>
    /// Model để trả về thông tin khóa tài khoản
    /// </summary>
    public class AccountLockInfo
    {
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public int FailedAttempts { get; set; }

        public AccountLockInfo(bool isLocked, DateTime? lockedUntil, int failedAttempts)
        {
            IsLocked = isLocked;
            LockedUntil = lockedUntil;
            FailedAttempts = failedAttempts;
        }
    }
}

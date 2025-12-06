using System;

namespace PM_Ban_Do_An_Nhanh.Entities
{
    /// <summary>
    /// Entity cho Audit Log
    /// </summary>
    public class AuditLog
    {
        public int LogID { get; set; }
        public DateTime ActionTime { get; set; }
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }          // "CREATE", "UPDATE", "DELETE"
        public string TableName { get; set; }       // "TaiKhoan", "MonAn", etc.
        public int? RecordID { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Description { get; set; }
    }
}

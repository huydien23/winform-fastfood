using PM_Ban_Do_An_Nhanh.DAL;
using PM_Ban_Do_An_Nhanh.Entities;
using PM_Ban_Do_An_Nhanh.Helpers;
using System;

namespace PM_Ban_Do_An_Nhanh.Helpers
{
    /// <summary>
    /// Helper class để ghi audit log dễ dàng
    /// </summary>
    public static class AuditLogger
    {
        private static AuditLogDAL auditLogDAL = new AuditLogDAL();

        /// <summary>
        /// Ghi log thao tác
        /// </summary>
        public static void Log(string action, string tableName, int? recordID = null, string oldValue = null, string newValue = null, string description = null)
        {
            try
            {
                var log = new AuditLog
                {
                    ActionTime = DateTime.Now,
                    UserID = SessionContext.CurrentUser?.MaTK,
                    UserName = SessionContext.CurrentUser?.TenTK ?? "Unknown",
                    Action = action,
                    TableName = tableName,
                    RecordID = recordID,
                    OldValue = oldValue,
                    NewValue = newValue,
                    Description = description
                };

                auditLogDAL.WriteLog(log);
            }
            catch (Exception ex)
            {
                // Log error nhưng không throw để không ảnh hưởng đến business logic
                Console.WriteLine($"Audit log error: {ex.Message}");
            }
        }

        /// <summary>
        /// Log CREATE action
       /// </summary>
        public static void LogCreate(string tableName, int recordID, string newValue, string description = null)
        {
            Log("CREATE", tableName, recordID, null, newValue, description);
        }

        /// <summary>
        /// Log UPDATE action
        /// </summary>
        public static void LogUpdate(string tableName, int recordID, string oldValue, string newValue, string description = null)
        {
            Log("UPDATE", tableName, recordID, oldValue, newValue, description);
        }

        /// <summary>
        /// Log DELETE action
        /// </summary>
        public static void LogDelete(string tableName, int recordID, string oldValue, string description = null)
        {
            Log("DELETE", tableName, recordID, oldValue, null, description);
        }
    }
}

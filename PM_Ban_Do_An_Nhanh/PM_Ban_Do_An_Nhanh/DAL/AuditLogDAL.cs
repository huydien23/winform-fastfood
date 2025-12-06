using PM_Ban_Do_An_Nhanh.Entities;
using System;
using System.Data.SqlClient;

namespace PM_Ban_Do_An_Nhanh.DAL
{
    /// <summary>
    /// Data Access Layer cho Audit Log
    /// </summary>
    public class AuditLogDAL
    {
        public bool WriteLog(AuditLog log)
        {
            string query = @"
                INSERT INTO AuditLog (ActionTime, UserID, UserName, Action, TableName, RecordID, OldValue, NewValue, Description)
                VALUES (@ActionTime, @UserID, @UserName, @Action, @TableName, @RecordID, @OldValue, @NewValue, @Description)";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ActionTime", log.ActionTime);
                    cmd.Parameters.AddWithValue("@UserID", log.UserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserName", log.UserName);
                    cmd.Parameters.AddWithValue("@Action", log.Action);
                    cmd.Parameters.AddWithValue("@TableName", log.TableName);
                    cmd.Parameters.AddWithValue("@RecordID", log.RecordID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OldValue", log.OldValue ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NewValue", log.NewValue ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", log.Description ?? (object)DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

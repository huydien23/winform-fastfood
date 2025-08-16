using System;
using System.Data;
using System.Data.SqlClient;
using PM_Ban_Do_An_Nhanh.Entities;

namespace PM_Ban_Do_An_Nhanh.DAL
{
    public class DanhMucDAL
    {
        public DataTable LayDanhSachDanhMuc()
        {
            DataTable dt = new DataTable();
            string query = "SELECT MaDM, TenDM FROM DanhMuc";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool ThemDanhMuc(string tenDM)
        {
            try
            {
                string query = "INSERT INTO DanhMuc (TenDM) VALUES (@TenDM)";
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDM", tenDM);
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SuaDanhMuc(int maDM, string tenDM)
        {
            try
            {
                string query = "UPDATE DanhMuc SET TenDM = @TenDM WHERE MaDM = @MaDM";
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDM", tenDM);
                        cmd.Parameters.AddWithValue("@MaDM", maDM);
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool XoaDanhMuc(int maDM)
        {
            try
            {
                string query = "DELETE FROM DanhMuc WHERE MaDM = @MaDM";
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDM", maDM);
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
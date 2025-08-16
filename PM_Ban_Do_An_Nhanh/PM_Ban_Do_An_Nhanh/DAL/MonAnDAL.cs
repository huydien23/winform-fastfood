using System;
using System.Data;
using System.Data.SqlClient;
using PM_Ban_Do_An_Nhanh.Entities;

namespace PM_Ban_Do_An_Nhanh.DAL
{
    public class MonAnDAL
    {
        public DataTable LayDanhSachMonAn()
        {
            DataTable dt = new DataTable();
            string query = "SELECT MaMon, TenMon, Gia, M.MaDM, TenDM, TrangThai FROM MonAn M JOIN DanhMuc DM ON M.MaDM = DM.MaDM";
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

        public bool ThemMonAn(MonAn monAn)
        {
            string query = "INSERT INTO MonAn (TenMon, Gia, MaDM, TrangThai) VALUES (@TenMon, @Gia, @MaDM, @TrangThai)";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenMon", monAn.TenMon);
                    cmd.Parameters.AddWithValue("@Gia", monAn.Gia);
                    cmd.Parameters.AddWithValue("@MaDM", monAn.MaDM);
                    cmd.Parameters.AddWithValue("@TrangThai", monAn.TrangThai);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool SuaMonAn(MonAn monAn)
        {
            string query = "UPDATE MonAn SET TenMon = @TenMon, Gia = @Gia, MaDM = @MaDM, TrangThai = @TrangThai WHERE MaMon = @MaMon";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenMon", monAn.TenMon);
                    cmd.Parameters.AddWithValue("@Gia", monAn.Gia);
                    cmd.Parameters.AddWithValue("@MaDM", monAn.MaDM);
                    cmd.Parameters.AddWithValue("@TrangThai", monAn.TrangThai);
                    cmd.Parameters.AddWithValue("@MaMon", monAn.MaMon);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool XoaMonAn(int maMon)
        {
            string query = "DELETE FROM MonAn WHERE MaMon = @MaMon";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMon", maMon);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}

using PM_Ban_Do_An_Nhanh.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Ban_Do_An_Nhanh.DAL
{
    public class TaiKhoanDAL
    {
        public TaiKhoan KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            TaiKhoan tk = null;
            string query = "SELECT MaTK, TenTK, TenDangNhap, MatKhau, Role, Email, IsActive FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau AND IsActive = 1";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tk = new TaiKhoan
                            {
                                MaTK = Convert.ToInt32(reader["MaTK"]),
                                TenTK = reader["TenTK"].ToString(),
                                TenDangNhap = reader["TenDangNhap"].ToString(),
                                MatKhau = reader["MatKhau"].ToString(),
                                Role = reader["Role"] != DBNull.Value ? reader["Role"].ToString() : "Staff",
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                                IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }
            return tk;
        }

        // Lấy tất cả tài khoản
        public DataTable GetAllTaiKhoan()
        {
            DataTable dt = new DataTable();
            string query = "SELECT MaTK, TenTK, TenDangNhap, Role, Email, IsActive FROM TaiKhoan ORDER BY MaTK";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        // Thêm tài khoản mới
        public bool ThemTaiKhoan(TaiKhoan tk)
        {
            string query = "INSERT INTO TaiKhoan (TenTK, TenDangNhap, MatKhau, Role, Email, IsActive) VALUES (@TenTK, @TenDangNhap, @MatKhau, @Role, @Email, @IsActive)";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenTK", tk.TenTK);
                    cmd.Parameters.AddWithValue("@TenDangNhap", tk.TenDangNhap);
                    cmd.Parameters.AddWithValue("@MatKhau", tk.MatKhau);
                    cmd.Parameters.AddWithValue("@Role", tk.Role);
                    cmd.Parameters.AddWithValue("@Email", tk.Email ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", tk.IsActive);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Cập nhật tài khoản
        public bool SuaTaiKhoan(TaiKhoan tk)
        {
            string query = "UPDATE TaiKhoan SET TenTK = @TenTK, TenDangNhap = @TenDangNhap, Role = @Role, Email = @Email, IsActive = @IsActive WHERE MaTK = @MaTK";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", tk.MaTK);
                    cmd.Parameters.AddWithValue("@TenTK", tk.TenTK);
                    cmd.Parameters.AddWithValue("@TenDangNhap", tk.TenDangNhap);
                    cmd.Parameters.AddWithValue("@Role", tk.Role);
                    cmd.Parameters.AddWithValue("@Email", tk.Email ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", tk.IsActive);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Đổi mật khẩu
        public bool DoiMatKhau(int maTK, string matKhauMoi)
        {
            string query = "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE MaTK = @MaTK";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhauMoi);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Khóa/Mở khóa tài khoản
        public bool ToggleActiveStatus(int maTK, bool isActive)
        {
            string query = "UPDATE TaiKhoan SET IsActive = @IsActive WHERE MaTK = @MaTK";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Kiểm tra tên đăng nhập đã tồn tại
        public bool KiemTraTenDangNhapTonTai(string tenDangNhap, int? maTKHienTai = null)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
            if (maTKHienTai.HasValue)
            {
                query += " AND MaTK != @MaTK";
            }
            
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                    if (maTKHienTai.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@MaTK", maTKHienTai.Value);
                    }
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
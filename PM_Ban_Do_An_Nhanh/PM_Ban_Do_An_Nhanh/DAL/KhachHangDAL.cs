using System;
using System.Data;
using System.Data.SqlClient;
using PM_Ban_Do_An_Nhanh.Entities;

namespace PM_Ban_Do_An_Nhanh.DAL
{
    public class KhachHangDAL
    {
        public DataTable LayDanhSachKhachHang()
        {
            DataTable dt = new DataTable();
            string query = "SELECT MaKH, TenKH, SDT, DiaChi, Email, NgaySinh FROM KhachHang";
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

        public KhachHang LayThongTinKhachHangBySDT(string sdt)
        {
            KhachHang kh = null;
            string query = "SELECT MaKH, TenKH, SDT, DiaChi, Email, NgaySinh FROM KhachHang WHERE SDT = @SDT";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kh = new KhachHang
                            {
                                MaKH = Convert.ToInt32(reader["MaKH"]),
                                TenKH = reader["TenKH"].ToString(),
                                SDT = reader["SDT"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                                NgaySinh = reader["NgaySinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgaySinh"])
                            };
                        }
                    }
                }
            }
            return kh;
        }

        public bool ThemKhachHang(KhachHang khachHang)
        {
            string query = "INSERT INTO KhachHang (TenKH, SDT, DiaChi, Email, NgaySinh) VALUES (@TenKH, @SDT, @DiaChi, @Email, @NgaySinh)";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenKH", khachHang.TenKH);
                    cmd.Parameters.AddWithValue("@SDT", khachHang.SDT);
                    cmd.Parameters.AddWithValue("@DiaChi", (object)khachHang.DiaChi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)khachHang.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgaySinh", (object)khachHang.NgaySinh ?? DBNull.Value);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public bool CapNhatKhachHang(KhachHang khachHang)
        {
            string query = "UPDATE KhachHang SET TenKH = @TenKH, DiaChi = @DiaChi, Email = @Email, NgaySinh = @NgaySinh WHERE SDT = @SDT";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenKH", khachHang.TenKH);
                    cmd.Parameters.AddWithValue("@SDT", khachHang.SDT);
                    cmd.Parameters.AddWithValue("@DiaChi", (object)khachHang.DiaChi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)khachHang.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgaySinh", (object)khachHang.NgaySinh ?? DBNull.Value);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool XoaKhachHang(string sdt)
        {
            string query = "DELETE FROM KhachHang WHERE SDT = @SDT";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    conn.Open();

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 547)
                        {
                            throw new Exception("Không thể xóa khách hàng này vì đang được tham chiếu trong đơn hàng.");
                        }
                        throw;
                    }
                }
            }
        }

        public DataTable LayLichSuDonHang(int maKH)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    dh.MaDH,
                    dh.NgayLap,
                    dh.TongTien, 
                    dh.TrangThai
                FROM DonHang dh
                WHERE dh.MaKH = @MaKH
                ORDER BY dh.NgayLap DESC";
            
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable LayChiTietDonHang(int maDH)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    ma.TenMon,
                    ctdh.SoLuong,
                    ctdh.DonGia,
                    (ctdh.SoLuong * ctdh.DonGia) AS ThanhTien
                FROM ChiTietDonHang ctdh
                INNER JOIN MonAn ma ON ctdh.MaMon = ma.MaMon
                WHERE ctdh.MaDH = @MaDH";
            
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDH", maDH);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool CapNhatDiemTichLuy(int maKH, int diem)
        {
            string query = "UPDATE KhachHang SET DiemTichLuy = @Diem WHERE MaKH = @MaKH";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Diem", diem);
                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public int LayDiemTichLuy(int maKH)
        {
            int diem = 0;
            string query = "SELECT DiemTichLuy FROM KhachHang WHERE MaKH = @MaKH";
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        diem = Convert.ToInt32(result);
                    }
                }
            }
            return diem;
        }
    }
}

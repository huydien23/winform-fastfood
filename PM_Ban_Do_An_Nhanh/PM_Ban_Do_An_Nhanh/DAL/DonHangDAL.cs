using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using PM_Ban_Do_An_Nhanh.Entities;

namespace PM_Ban_Do_An_Nhanh.DAL
{
    public class DonHangDAL
    {
        public int ThemDonHang(DonHang donHang, List<ChiTietDonHang> chiTietList)
        {
            int maDonHangMoi = -1;
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string queryDonHang = "INSERT INTO DonHang (NgayLap, TongTien, TrangThaiThanhToan, MaKH) VALUES (@NgayLap, @TongTien, @TrangThaiThanhToan, @MaKH); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmdDonHang = new SqlCommand(queryDonHang, conn, transaction))
                    {
                        cmdDonHang.Parameters.AddWithValue("@NgayLap", donHang.NgayLap);
                        cmdDonHang.Parameters.AddWithValue("@TongTien", donHang.TongTien);
                        cmdDonHang.Parameters.AddWithValue("@TrangThaiThanhToan", donHang.TrangThaiThanhToan);
                        cmdDonHang.Parameters.AddWithValue("@MaKH", (object)donHang.MaKH ?? DBNull.Value);
                        maDonHangMoi = Convert.ToInt32(cmdDonHang.ExecuteScalar());
                    }

                    string queryChiTiet = "INSERT INTO ChiTietDonHang (MaDH, MaMon, SoLuong, DonGia) VALUES (@MaDH, @MaMon, @SoLuong, @DonGia)";
                    foreach (var chiTiet in chiTietList)
                    {
                        using (SqlCommand cmdChiTiet = new SqlCommand(queryChiTiet, conn, transaction))
                        {
                            cmdChiTiet.Parameters.AddWithValue("@MaDH", maDonHangMoi);
                            cmdChiTiet.Parameters.AddWithValue("@MaMon", chiTiet.MaMon);
                            cmdChiTiet.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                            cmdChiTiet.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                            cmdChiTiet.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Lỗi khi thêm đơn hàng và chi tiết: " + ex.Message);
                }
            }
            return maDonHangMoi;
        }

        public DataTable LayChiTietDonHangChoIn(int maDH)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT
                    dh.MaDH,
                    dh.NgayLap,
                    dh.TongTien,
                    dh.TrangThaiThanhToan,
                    kh.TenKH,
                    kh.SDT AS SDT_KhachHang,
                    ma.TenMon,
                    ct.SoLuong,
                    ct.DonGia,
                    (ct.SoLuong * ct.DonGia) AS ThanhTien
                FROM DonHang dh
                LEFT JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                JOIN ChiTietDonHang ct ON dh.MaDH = ct.MaDH
                JOIN MonAn ma ON ct.MaMon = ma.MaMon
                WHERE dh.MaDH = @MaDH";

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

        public DataTable LayDanhSachDonHang(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT
                    dh.MaDH,
                    dh.NgayLap,
                    dh.TongTien,
                    dh.TrangThaiThanhToan,
                    kh.TenKH,
                    kh.SDT AS SDT_KhachHang
                FROM DonHang dh
                LEFT JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                WHERE 1=1";

            if (tuNgay.HasValue)
            {
                query += " AND CAST(dh.NgayLap AS DATE) >= @TuNgay";
            }
            if (denNgay.HasValue)
            {
                query += " AND CAST(dh.NgayLap AS DATE) <= @DenNgay";
            }
            query += " ORDER BY dh.NgayLap DESC";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (tuNgay.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Value.Date);
                    }
                    if (denNgay.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@DenNgay", denNgay.Value.Date);
                    }
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable LayThongKeDoanhThu(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT
                    CAST(dh.NgayLap AS DATE) AS Ngay,
                    SUM(dh.TongTien) AS DoanhThuNgay
                FROM DonHang dh
                WHERE dh.TrangThaiThanhToan = N'Đã thanh toán'";

            if (tuNgay.HasValue)
            {
                query += " AND CAST(dh.NgayLap AS DATE) >= @TuNgay";
            }
            if (denNgay.HasValue)
            {
                query += " AND CAST(dh.NgayLap AS DATE) <= @DenNgay";
            }
            query += " GROUP BY CAST(dh.NgayLap AS DATE) ORDER BY Ngay";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (tuNgay.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Value.Date);
                    }
                    if (denNgay.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@DenNgay", denNgay.Value.Date);
                    }
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable LayMonAnBanChay(DateTime? tuNgay = null, DateTime? denNgay = null, int topN = 5)
        {
            DataTable dt = new DataTable();
            string query = $@"
                SELECT TOP {topN}
                    ma.TenMon,
                    SUM(ct.SoLuong) AS TongSoLuongBan,
                    SUM(ct.SoLuong * ct.DonGia) AS TongDoanhThuMon
                FROM ChiTietDonHang ct
                JOIN MonAn ma ON ct.MaMon = ma.MaMon
                JOIN DonHang dh ON ct.MaDH = dh.MaDH
                WHERE dh.TrangThaiThanhToan = N'Đã thanh toán'";

            if (tuNgay.HasValue)
            {
                query += " AND CAST(dh.NgayLap AS DATE) >= @TuNgay";
            }
            if (denNgay.HasValue)
            {
                query += " AND CAST(dh.NgayLap AS DATE) <= @DenNgay";
            }
            query += " GROUP BY ma.TenMon ORDER BY TongSoLuongBan DESC";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (tuNgay.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Value.Date);
                    }
                    if (denNgay.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@DenNgay", denNgay.Value.Date);
                    }
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}
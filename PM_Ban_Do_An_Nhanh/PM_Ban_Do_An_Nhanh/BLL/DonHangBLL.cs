using PM_Ban_Do_An_Nhanh.DAL;
using PM_Ban_Do_An_Nhanh.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Ban_Do_An_Nhanh.BLL
{
    public class DonHangBLL
    {
        private DonHangDAL donHangDAL = new DonHangDAL();

        public int ThemDonHang(DonHang donHang, List<ChiTietDonHang> chiTietList)
        {
            // Enhanced validation
            if (donHang == null)
                throw new ArgumentNullException(nameof(donHang));

            if (donHang.TongTien <= 0)
                throw new ArgumentException("Tổng tiền phải lớn hơn 0");

            if (chiTietList == null || chiTietList.Count == 0)
                throw new ArgumentException("Danh sách chi tiết đơn hàng không được trống");

            // Validate each order item
            foreach (var item in chiTietList)
            {
                if (item.MaMon <= 0)
                    throw new ArgumentException($"Mã món không hợp lệ: {item.MaMon}");

                if (item.SoLuong <= 0)
                    throw new ArgumentException($"Số lượng phải lớn hơn 0 cho món: {item.TenMon}");

                if (item.DonGia <= 0)
                    throw new ArgumentException($"Đơn giá phải lớn hơn 0 cho món: {item.TenMon}");
            }

            try
            {
                return donHangDAL.ThemDonHang(donHang, chiTietList);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm đơn hàng: {ex.Message}", ex);
            }
        }

        public DataTable LayChiTietDonHangChoIn(int maDH)
        {
            return donHangDAL.LayChiTietDonHangChoIn(maDH);
        }

        public DataTable LayDanhSachDonHang(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return donHangDAL.LayDanhSachDonHang(tuNgay, denNgay);
        }

        public DataTable LayThongKeDoanhThu(DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return donHangDAL.LayThongKeDoanhThu(tuNgay, denNgay);
        }

        public DataTable LayMonAnBanChay(DateTime? tuNgay = null, DateTime? denNgay = null, int topN = 5)
        {
            return donHangDAL.LayMonAnBanChay(tuNgay, denNgay, topN);
        }
    }
}

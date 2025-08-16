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
            if (donHang.TongTien < 0 || chiTietList == null || chiTietList.Count == 0)
            {
                return -1;
            }
            return donHangDAL.ThemDonHang(donHang, chiTietList);
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

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
    public class MonAnBLL
    {
        private MonAnDAL monAnDAL = new MonAnDAL();

        public DataTable HienThiDanhSachMonAn()
        {
            return monAnDAL.LayDanhSachMonAn();
        }

        public bool ThemMonAn(string tenMon, decimal gia, int maDM, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(tenMon) || gia <= 0)
            {
                return false;
            }
            MonAn monAn = new MonAn { TenMon = tenMon, Gia = gia, MaDM = maDM, TrangThai = trangThai };
            return monAnDAL.ThemMonAn(monAn);
        }

        public bool SuaMonAn(int maMon, string tenMon, decimal gia, int maDM, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(tenMon) || gia <= 0 || maMon <= 0)
            {
                return false;
            }
            MonAn monAn = new MonAn { MaMon = maMon, TenMon = tenMon, Gia = gia, MaDM = maDM, TrangThai = trangThai };
            return monAnDAL.SuaMonAn(monAn);
        }

        public bool XoaMonAn(int maMon)
        {
            if (maMon <= 0) return false;
            return monAnDAL.XoaMonAn(maMon);
        }
    }
}

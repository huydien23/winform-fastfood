using PM_Ban_Do_An_Nhanh.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Ban_Do_An_Nhanh.BLL
{
    public class DanhMucBLL
    {
        private DanhMucDAL danhMucDAL = new DanhMucDAL();

        public DataTable LayDanhSachDanhMuc()
        {
            return danhMucDAL.LayDanhSachDanhMuc();
        }
    }
}

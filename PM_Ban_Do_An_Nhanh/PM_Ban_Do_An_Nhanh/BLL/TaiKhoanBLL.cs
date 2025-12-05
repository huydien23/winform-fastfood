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
    public class TaiKhoanBLL
    {
        private TaiKhoanDAL taiKhoanDAL = new TaiKhoanDAL();

        public TaiKhoan KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                return null;
            }
            return taiKhoanDAL.KiemTraDangNhap(tenDangNhap, matKhau);
        }

        public DataTable GetAllTaiKhoan()
        {
            return taiKhoanDAL.GetAllTaiKhoan();
        }

        public bool ThemTaiKhoan(TaiKhoan tk)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(tk.TenTK) || string.IsNullOrWhiteSpace(tk.TenDangNhap) || string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                throw new Exception("Vui lòng nhập đầy đủ thông tin tài khoản.");
            }

            if (tk.TenDangNhap.Length < 3)
            {
                throw new Exception("Tên đăng nhập phải có ít nhất 3 ký tự.");
            }

            if (tk.MatKhau.Length < 6)
            {
                throw new Exception("Mật khẩu phải có ít nhất 6 ký tự.");
            }

            if (taiKhoanDAL.KiemTraTenDangNhapTonTai(tk.TenDangNhap))
            {
                throw new Exception("Tên đăng nhập đã tồn tại.");
            }

            return taiKhoanDAL.ThemTaiKhoan(tk);
        }

        public bool SuaTaiKhoan(TaiKhoan tk)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(tk.TenTK) || string.IsNullOrWhiteSpace(tk.TenDangNhap))
            {
                throw new Exception("Vui lòng nhập đầy đủ thông tin tài khoản.");
            }

            if (tk.TenDangNhap.Length < 3)
            {
                throw new Exception("Tên đăng nhập phải có ít nhất 3 ký tự.");
            }

            if (taiKhoanDAL.KiemTraTenDangNhapTonTai(tk.TenDangNhap, tk.MaTK))
            {
                throw new Exception("Tên đăng nhập đã tồn tại.");
            }

            return taiKhoanDAL.SuaTaiKhoan(tk);
        }

        public bool DoiMatKhau(int maTK, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(matKhauMoi))
            {
                throw new Exception("Mật khẩu không được để trống.");
            }

            if (matKhauMoi.Length < 6)
            {
                throw new Exception("Mật khẩu phải có ít nhất 6 ký tự.");
            }

            return taiKhoanDAL.DoiMatKhau(maTK, matKhauMoi);
        }

        public bool ToggleActiveStatus(int maTK, bool isActive)
        {
            return taiKhoanDAL.ToggleActiveStatus(maTK, isActive);
        }
    }
}
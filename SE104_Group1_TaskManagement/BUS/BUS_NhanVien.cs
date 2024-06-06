using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BUS
{
    public class BUS_NhanVien
    {
        static BUS_NhanVien _instance = new BUS_NhanVien();
        public static BUS_NhanVien Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BUS_NhanVien();
                return _instance;
            }
        }

        DAL_NhanVien dalNV = new DAL_NhanVien();
        DAL_TaiKhoan dalTK = new DAL_TaiKhoan();
        public BindingList<DTO_NhanVien> GetAllData()
        {
            BindingList<DTO_NhanVien> result = new BindingList<DTO_NhanVien>();
            DataTable dsNhanVien = dalNV.GetAllData();

            for (int i = 0; i < dsNhanVien.Rows.Count; i++)
            {
                DTO_NhanVien temp = new DTO_NhanVien();
                temp.MANV = dsNhanVien.Rows[i]["MANV"].ToString();
                temp.TENNV = dsNhanVien.Rows[i]["HoTen"].ToString();
                temp.EMAIL = dsNhanVien.Rows[i]["EMAIL"].ToString();
                temp.PHONE = dsNhanVien.Rows[i]["SoDT"].ToString();
                temp.LEVEL = int.Parse(dsNhanVien.Rows[i]["LVL"].ToString());
                temp.NGAYSINH = dsNhanVien.Rows[i]["NGSINH"].ToString();
                temp.MACM = dsNhanVien.Rows[i]["MACM"].ToString();
                temp.GHICHU = dsNhanVien.Rows[i]["GHICHU"].ToString();
                result.Add(temp);
            }
            return result;
        }

        public BindingList<DTO_NhanVien> FindNV(DTO_NhanVien filter)
        {
            BindingList<DTO_NhanVien> result = new BindingList<DTO_NhanVien>();
            DataTable dsNhanVien = dalNV.GetDataByFilter(filter);

            for (int i = 0; i < dsNhanVien.Rows.Count; i++)
            {
                DTO_NhanVien temp = new DTO_NhanVien();
                temp.MANV = dsNhanVien.Rows[i]["MANV"].ToString();
                temp.TENNV = dsNhanVien.Rows[i]["HoTen"].ToString();
                temp.EMAIL = dsNhanVien.Rows[i]["EMAIL"].ToString();
                temp.PHONE = dsNhanVien.Rows[i]["SoDT"].ToString();
                temp.LEVEL = int.Parse(dsNhanVien.Rows[i]["LVL"].ToString());
                temp.NGAYSINH = dsNhanVien.Rows[i]["NGSINH"].ToString();
                temp.MACM = dsNhanVien.Rows[i]["MACM"].ToString();
                temp.GHICHU = dsNhanVien.Rows[i]["GHICHU"].ToString();
                result.Add(temp);
            }
            return result;
        }


        //Tra ve 0 neu them, xoa, sua nhan vien = 0, tra ve -1 neu them, xoa, sua nhan vien that bai
        public (bool, string) AddData(DTO_NhanVien nhanVienMoi)
        {
            (bool result, string message) = IsValidStaffInfo(nhanVienMoi);
            if (result == false)
            {
                return IsValidStaffInfo(nhanVienMoi);
            }
            else
            {
                return (dalNV.AddData(nhanVienMoi));
            }
        }

        public (bool, string) DeleteByID(DTO_NhanVien nhanVienCanXoa)
        {
            BindingList<DTO_DuAn> list = BUS_DuAn.Instance.FindDA(new DTO_DuAn("", "", nhanVienCanXoa.MANV), -1, -1);
            foreach (var da in list)
            {
                da.MAOWNER = "";
                if(da.STAT== "On-going" || da.STAT == "Delayed") BUS_DuAn.Instance.EditProject(da);
            }

            BUS_CongViec.Instance.DeletePCByNV(nhanVienCanXoa);
            return dalNV.DeleteByID(nhanVienCanXoa.MANV);
        }

        public (bool, string) SuaNhanVien(DTO_NhanVien nhanVienCanSua)
        {
            (bool result, string message) = IsValidStaffInfo(nhanVienCanSua);
            if (result == false)
            {
                return IsValidStaffInfo(nhanVienCanSua);
            }
            else
            {
                return (dalNV.SetData(nhanVienCanSua));
            }
        }
        public DTO_NhanVien GetByID (string ID)
        {
            return dalNV.GetByID(ID);
        }

        
        //Check staff info 
        public static (bool, string) IsValidStaffInfo(DTO_NhanVien nv)
        {
            if (nv == null)
                return (false, "Thong tin khong hop le");
            if (!IsValidName(nv.TENNV))
                return (false, "Ten khong hop le!");
            if (!IsValidEmail(nv.EMAIL))
                return (false, "Email khong hop le!");
            if (!IsValidPhoneNumber(nv.PHONE))
                return (false, "So dien thoai khong hop le!");
            if (!IsValidBirthDay(DateTime.Now))
                return (false, "Ngay sinh khong hop le");
            return (true, "Thong tin hop le");

        }

        //Check Name
        private static bool IsValidName(string name)
        {
            if (name == null)
                return false;
            else
            {
                foreach (char c in name)
                {
                    if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        //Check Email
        public static bool IsValidEmail(string email)
        {
            if (email == null)
                return false;
            else
            {
                // Mẫu regex để kiểm tra định dạng email
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

                // Kiểm tra xem chuỗi khớp với mẫu regex hay không
                bool isMatch = Regex.IsMatch(email, pattern);

                return isMatch;
            }
        }

        //Check phone
        public static bool IsValidPhoneNumber(string soDienThoai)
        {
            if (soDienThoai == null)
                return false;
            else
            {
                // Mẫu regex để kiểm tra định dạng số điện thoại
                string pattern = @"^0\d{9}$";

                // Kiểm tra xem chuỗi khớp với mẫu regex hay không
                bool isMatch = Regex.IsMatch(soDienThoai, pattern);

                return isMatch;
            }
        }
        //Check birthday
        public static bool IsValidBirthDay(DateTime ngaySinh)
        {
            if (ngaySinh == null || ngaySinh >= DateTime.Now)
                return false;
            else
                return true;
        }

        //Lấy và đổi quyền hạn
        
        public string DoiQuyenHan(DTO_TaiKhoan crnUser, DTO_NhanVien nv, string QH)
        {
            if (BUS_TaiKhoan.Instance.checkQH(crnUser, "SuaNV"))
            {
                return dalTK.ChangeQH(nv.MANV, QH);
            }
            else return "Không có quyền hạn này";
        }

        public DTO_QuyenHan GetQuyenHan(DTO_NhanVien nv)
        {
            return dalTK.GetQH(nv.MANV);
        }
    }
}

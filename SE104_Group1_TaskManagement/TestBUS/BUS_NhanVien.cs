using DAL;
using DTO;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace TestBUS
{
    public class BUS_NhanVien
    {
        DAL_NhanVien dalNV = new DAL_NhanVien();
        DAL_ChuyenMon dalCM = new DAL_ChuyenMon();

        public (bool, string) SetData(DTO_NhanVien nhanvien)
        {
            if (nhanvien == null || nhanvien.MANV == "") return (false, "Nhân viên không tồn tại");
            else
            {
                nhanvien.MACM = dalCM.ConvertNametoID(nhanvien.MACM);
                return dalNV.SetData(nhanvien);
            }    
        }
        public DTO_NhanVien GetByID(string ID)
        {
            DTO_NhanVien res = dalNV.GetByID(ID);
            if (res != null)
                res.MACM = dalCM.ConvertIDtoName(res.MACM);
            return res;
        }
        public ObservableCollection<DTO_NhanVien> GetAllData()
        {
            DataTable dt = dalNV.GetAllData();
            ObservableCollection<DTO_NhanVien> collection = new ObservableCollection<DTO_NhanVien>();
            foreach (DataRow row in dt.Rows)
            {
                DTO_NhanVien nhanVien = new DTO_NhanVien();
                nhanVien.MANV = row[dt.Columns[0]].ToString();
                nhanVien.TENNV = row[dt.Columns[1]].ToString();
                nhanVien.EMAIL = row[dt.Columns[2]].ToString();
                nhanVien.PHONE = row[dt.Columns[3]].ToString();
                nhanVien.NGAYSINH = DateTime.Parse(row[dt.Columns[4]].ToString());
                int level = -1;
                int.TryParse(row[dt.Columns[5]].ToString(), out level);
                nhanVien.LEVEL = level;
                nhanVien.MACM = dalCM.ConvertIDtoName(row[dt.Columns[6]].ToString());
                nhanVien.GHICHU = row[dt.Columns[7]].ToString();

                collection.Add(nhanVien);
            }
            return collection;
        }

        public (bool, string) AddData(DTO_NhanVien temp)
        {
            temp.MACM = dalCM.ConvertNametoID(temp.MACM);
            return dalNV.AddData(temp);
        }
    }
}

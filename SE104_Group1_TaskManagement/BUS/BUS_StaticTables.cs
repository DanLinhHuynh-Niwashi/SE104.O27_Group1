using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BUS
{
    public class BUS_StaticTables
    {
        static DAL_QuyenHan dalQH = new DAL_QuyenHan();
        static DAL_ChuyenMon dalCM = new DAL_ChuyenMon();
        static DAL_LoaiSK dalLoaiSK = new DAL_LoaiSK();
        static DAL_NhanVien dalNhanVien = new DAL_NhanVien();
        static DAL_PhanCong dalPhanCong = new DAL_PhanCong();

        static BUS_StaticTables _instance = new BUS_StaticTables();
        public static BUS_StaticTables Instance {
            get
            {
                if (_instance == null)
                    _instance = new BUS_StaticTables();
                return _instance;
            }
        }

        public Dictionary<string,DTO_QuyenHan> GetAllDataQH()
        {
            Dictionary<string, DTO_QuyenHan> result = new Dictionary<string, DTO_QuyenHan>();
            DataTable ds = dalQH.GetAllData();

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                DTO_QuyenHan temp = new DTO_QuyenHan();
                if (temp!=null && ds.Rows[i]["MAQH"]!=null && ds.Rows[i]["TENQH"] != null)
                {
                    temp.MAQH = ds.Rows[i]["MAQH"].ToString();
                    temp.TENQH = ds.Rows[i]["TENQH"].ToString();
                    result.Add(temp.MAQH, temp);
                }    
            }
            return result;
        }

        public Dictionary<string, DTO_ChuyenMon> GetAllDataCM()
        {
            Dictionary<string, DTO_ChuyenMon> result = new Dictionary<string, DTO_ChuyenMon>();
            DataTable ds = dalCM.GetAllData();

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                DTO_ChuyenMon temp = new DTO_ChuyenMon();
                if (temp != null && ds.Rows[i]["MACM"] != null && ds.Rows[i]["INSHORT"] != null && ds.Rows[i]["TENCM"]!=null)
                {
                    temp.MACM = ds.Rows[i]["MACM"].ToString();
                    temp.INSHORT = ds.Rows[i]["INSHORT"].ToString();
                    temp.TENCM = ds.Rows[i]["TENCM"].ToString();
                    temp.ISDELETED = ds.Rows[i]["ISDELETED"].ToString() == "1" ? true : false;
                    result.Add(temp.MACM, temp);
                }
            }
            return result;
        }

        public Dictionary<string, DTO_LoaiSK> GetAllDataLSK()
        {
            Dictionary<string, DTO_LoaiSK> result = new Dictionary<string, DTO_LoaiSK>();
            DataTable ds = dalLoaiSK.GetAllData();

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                DTO_LoaiSK temp = new DTO_LoaiSK();
                temp.MALSK = ds.Rows[i]["MALSK"].ToString();
                temp.INSHORT = ds.Rows[i]["INSHORT"].ToString();
                temp.TENLSK = ds.Rows[i]["TENLSK"].ToString();
                temp.MAX = long.Parse(Convert.ToInt64(ds.Rows[i]["MoneyMax"]).ToString());
                temp.MIN = long.Parse(Convert.ToInt64(ds.Rows[i]["MoneyMin"]).ToString());
                temp.ISDELETED = ds.Rows[i]["ISDELETED"].ToString() == "1" ? true : false;
                result.Add(temp.MALSK, temp);
            }
            return result;
        }

        public Dictionary<string, DTO_NhanVien> GetAllDataNV()
        {
            Dictionary<string, DTO_NhanVien> result = new Dictionary<string, DTO_NhanVien>();
            DataTable dsNhanVien = dalNhanVien.GetAllData();

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
                result.Add(temp.MANV, temp);
            }
            return result;
        }

        public Dictionary<string, DTO_PhanCong> GetAllDataPC()
        {
            Dictionary<string, DTO_PhanCong> result = new Dictionary<string, DTO_PhanCong>();
            DataTable dsPhanCong = dalPhanCong.GetAllData();

            for (int i = 0; i < dsPhanCong.Rows.Count; i++)
            {
                DTO_PhanCong temp = new DTO_PhanCong();
                temp.MANV = dsPhanCong.Rows[i]["MANV"].ToString();
                temp.MACV = dsPhanCong.Rows[i]["MACV"].ToString();
                result.Add(temp.MANV, temp);
            }
            return result;
        }

        public Dictionary<string, DTO_ChuyenMon> GetAllRawDataCM()
        {
            Dictionary<string, DTO_ChuyenMon> result = new Dictionary<string, DTO_ChuyenMon>();
            DataTable ds = dalCM.GetAllRawData();

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                DTO_ChuyenMon temp = new DTO_ChuyenMon();
                if (temp != null && ds.Rows[i]["MACM"] != null && ds.Rows[i]["INSHORT"] != null && ds.Rows[i]["TENCM"] != null)
                {
                    temp.MACM = ds.Rows[i]["MACM"].ToString();
                    temp.INSHORT = ds.Rows[i]["INSHORT"].ToString();
                    temp.TENCM = ds.Rows[i]["TENCM"].ToString();
                    temp.ISDELETED = ds.Rows[i]["ISDELETED"].ToString() == "1" ? true : false;
                    result.Add(temp.MACM, temp);
                }
            }
            return result;
        }

        public Dictionary<string, DTO_LoaiSK> GetAllRawDataLSK()
        {
            Dictionary<string, DTO_LoaiSK> result = new Dictionary<string, DTO_LoaiSK>();
            DataTable ds = dalLoaiSK.GetAllRawData();

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                DTO_LoaiSK temp = new DTO_LoaiSK();
                temp.MALSK = ds.Rows[i]["MALSK"].ToString();
                temp.INSHORT = ds.Rows[i]["INSHORT"].ToString();
                temp.TENLSK = ds.Rows[i]["TENLSK"].ToString();
                temp.MAX = long.Parse(Convert.ToInt64(ds.Rows[i]["MoneyMax"]).ToString());
                temp.MIN = long.Parse(Convert.ToInt64(ds.Rows[i]["MoneyMin"]).ToString());
                temp.ISDELETED = ds.Rows[i]["ISDELETED"].ToString() == "1" ? true : false;
                result.Add(temp.MALSK, temp);
            }
            return result;
        }

        public Dictionary<string, DTO_NhanVien> GetAllRawDataNV()
        {
            Dictionary<string, DTO_NhanVien> result = new Dictionary<string, DTO_NhanVien>();
            DataTable dsNhanVien = dalNhanVien.GetAllRawData();

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
                result.Add(temp.MANV, temp);
            }
            return result;
        }

        public BindingList<DTO_ChuyenMon> GetAllDataCMBinding()
        {
            BindingList<DTO_ChuyenMon> result = new BindingList<DTO_ChuyenMon>();
            DataTable ds = dalCM.GetAllRawData();

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                DTO_ChuyenMon temp = new DTO_ChuyenMon();
                if (temp != null && ds.Rows[i]["MACM"] != null && ds.Rows[i]["INSHORT"] != null && ds.Rows[i]["TENCM"] != null)
                {
                    temp.MACM = ds.Rows[i]["MACM"].ToString();
                    temp.INSHORT = ds.Rows[i]["INSHORT"].ToString();
                    temp.TENCM = ds.Rows[i]["TENCM"].ToString();
                    temp.ISDELETED = ds.Rows[i]["ISDELETED"].ToString() == "1" ? true : false;
                    result.Add(temp);
                }
            }
            return result;
        }

        public BindingList<DTO_LoaiSK> GetAllDataLSKBinding()
        {
            BindingList<DTO_LoaiSK> result = new BindingList<DTO_LoaiSK>();
            DataTable ds = dalLoaiSK.GetAllRawData();

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                DTO_LoaiSK temp = new DTO_LoaiSK();
                temp.MALSK = ds.Rows[i]["MALSK"].ToString();
                temp.INSHORT = ds.Rows[i]["INSHORT"].ToString();
                temp.TENLSK = ds.Rows[i]["TENLSK"].ToString();
                temp.MAX = long.Parse(Convert.ToInt64(ds.Rows[i]["MoneyMax"]).ToString());
                temp.MIN = long.Parse(Convert.ToInt64(ds.Rows[i]["MoneyMin"]).ToString());
                temp.ISDELETED = ds.Rows[i]["ISDELETED"].ToString() == "1" ? true : false;
                result.Add(temp);
            }
            return result;
        }

        public (bool, string) DeleteCMByID(string id)
        {
            return dalCM.DeleteByID(id);
        }
        public (bool, string) DeleteLSKByID(string id)
        {
            return dalLoaiSK.DeleteByID(id);
        }

        //Edit LSK, CM
        public (bool, string) EditLSK(DTO_LoaiSK lsk)
        {
            if (IsValidLSK(lsk).Item1 == false) return IsValidLSK(lsk);
            return dalLoaiSK.SetData(lsk);
        }
        public (bool, string) EditCM(DTO_ChuyenMon cm)
        {
            if (IsValidCM(cm).Item1 == false) return IsValidCM(cm);
            return dalCM.SetData(cm);
        }

        //Add LSK, CM
        public (bool, string) AddLSK(DTO_LoaiSK lsk)
        {
            if (IsValidLSK(lsk).Item1 == false) return IsValidLSK(lsk);
            return dalLoaiSK.AddData(lsk);
        }
        public (bool, string) AddCM(DTO_ChuyenMon cm)
        {
            if (IsValidCM(cm).Item1 == false) return IsValidCM(cm);
            return dalCM.AddData(cm);
        }

        //Kiem tra
        (bool, string) IsValidLSK(DTO_LoaiSK lsk)
        {
            if (lsk == null) return (false, "Không có đầu vào");
            if (lsk.TENLSK == "") return (false, "Nhập tên loại sự kiện");
            if (!IsValidShort(lsk.INSHORT, 4)) return (false, "Loại sự kiện cần được ký hiệu bằng 04 chữ in hoa.");
            if (lsk.MIN > lsk.MAX) return (false, "Ngân sách tối thiểu không lớn hơn ngân sách tối đa");
            return (true, "");
        }

        (bool, string) IsValidCM(DTO_ChuyenMon cm)
        {
            if (cm == null) return (false, "Không có đầu vào");
            if (cm.TENCM == "") return (false, "Nhập tên chuyên môn");
            if (!IsValidShort(cm.INSHORT, 3)) return (false, "Chuyên môn cần được ký hiệu bằng 03 chữ in hoa.");
            return (true, "");
        }

        
        public bool IsValidShort(string inshort, int num)
        {
            if (inshort == null)
            {
                return false;
            }
            else
            {
                string pattern = @"^([A-Z]{"+num+"})$";
                return Regex.IsMatch(inshort, pattern);
            }
        }

        //ThamSo

        public DTO_ThamSo GetThamSo()
        {
            return DAL_ThamSo.Instance.GetAllData();
        }
        public (bool, string) SetThamSo(DTO_ThamSo thamSo)
        {
            return DAL_ThamSo.Instance.SetData(thamSo);
        }
    }
}

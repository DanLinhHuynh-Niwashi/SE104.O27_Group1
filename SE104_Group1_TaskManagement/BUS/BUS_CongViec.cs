using DAL;
using DTO;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.AxHost;

namespace BUS
{
    public class BUS_CongViec
    {
        static BUS_CongViec _instance = new BUS_CongViec();
        public static BUS_CongViec Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BUS_CongViec();
                return _instance;
            }
        }

        DAL_CongViec dalCV = new DAL_CongViec();
        DAL_DuAn dalDA = new DAL_DuAn();
        DAL_PhanCong dalPC = new DAL_PhanCong();

        public (bool, string) PhanCong(string MACV, string MANV)
        {
            DTO_PhanCong dTO_PhanCong = new DTO_PhanCong(MANV, MACV);
            return (dalPC.AddData(dTO_PhanCong));
        }
        public void DeletePCByTask(DTO_CongViec cv)
        {
            if (cv == null) return;
            (bool, string) res1 = dalPC.DeleteByMACV(cv.MACV);
        }
        public void DeletePCByNV(DTO_NhanVien nv)
        {
            if (nv == null) return;
            (bool, string) res1 = dalPC.DeleteByMANV(nv.MANV);
        }
        public BindingList<DTO_PhanCong> GetPhanCong(DTO_CongViec cv)
        {
            BindingList<DTO_PhanCong> result = new BindingList<DTO_PhanCong>();
            DataTable dsPhanCong = dalPC.GetByMACV(cv.MACV);
            for (int i = 0; i < dsPhanCong.Rows.Count; i++)
            {
                //string _macv;
                DTO_PhanCong temp = new DTO_PhanCong();
                temp.MACV = dsPhanCong.Rows[i]["MACV"].ToString();
                temp.MANV = dsPhanCong.Rows[i]["MANV"].ToString();
                result.Add(temp);
            }
            return result;
        }


        public BindingList<DTO_CongViec> GetAllData()
        {
            BindingList<DTO_CongViec> result = new BindingList<DTO_CongViec>();
            DataTable dsCongViec = dalCV.GetAllData();

            for (int i = 0; i < dsCongViec.Rows.Count; i++)
            {
                //string _macv;
                DTO_CongViec temp = new DTO_CongViec();
                temp.MADA = dsCongViec.Rows[i]["MADA"].ToString();
                temp.MACV = dsCongViec.Rows[i]["MACV"].ToString();
                temp.MACM = dsCongViec.Rows[i]["MACM"].ToString();
                temp.TENCV = dsCongViec.Rows[i]["TENCV"].ToString();
                temp.TSTART = dsCongViec.Rows[i]["TStart"].ToString();
                temp.TEND = dsCongViec.Rows[i]["TEnd"].ToString();
                temp.NGANSACH = long.Parse(Convert.ToInt64(dsCongViec.Rows[i]["NGANSACH"]).ToString());
                temp.DADUNG = long.Parse(Convert.ToInt64(dsCongViec.Rows[i]["DADUNG"]).ToString());
                temp.TIENDO = int.Parse(Convert.ToInt32(dsCongViec.Rows[i]["TIENDO"]).ToString());
                temp.YCDK = dsCongViec.Rows[i]["YCDINHKEM"].ToString();
                temp.TEPDK = dsCongViec.Rows[i]["TEPDINHKEM"].ToString();
                result.Add(temp);
            }
            return result;
        }

        public BindingList<DTO_CongViec> GetByMaDA(string MaDA)
        {
            BindingList<DTO_CongViec> result = new BindingList<DTO_CongViec>();
            DataTable dsCongViec = dalCV.GetByMaDA(MaDA);

            for (int i = 0; i < dsCongViec.Rows.Count; i++)
            {
                //string _macv;
                DTO_CongViec temp = new DTO_CongViec();
                temp.MADA = dsCongViec.Rows[i]["MADA"].ToString();
                temp.MACV = dsCongViec.Rows[i]["MACV"].ToString();
                temp.MACM = dsCongViec.Rows[i]["MACM"].ToString();
                temp.TENCV = dsCongViec.Rows[i]["TENCV"].ToString();
                temp.TSTART = dsCongViec.Rows[i]["TStart"].ToString();
                temp.TEND = dsCongViec.Rows[i]["TEnd"].ToString();
                temp.NGANSACH = long.Parse(Convert.ToInt64(dsCongViec.Rows[i]["NGANSACH"]).ToString());
                temp.DADUNG = long.Parse(Convert.ToInt64(dsCongViec.Rows[i]["DADUNG"]).ToString());
                temp.TIENDO = int.Parse(Convert.ToInt32(dsCongViec.Rows[i]["TIENDO"]).ToString());
                temp.YCDK = dsCongViec.Rows[i]["YCDINHKEM"].ToString();
                temp.TEPDK = dsCongViec.Rows[i]["TEPDINHKEM"].ToString();
                result.Add(temp);
            }
            return result;
        }

        //ADD
        public (bool, string) AddData(DTO_CongViec CongViecMoi)
        {
            (bool result, string message) = IsValidProjectInfo(CongViecMoi);
            if (result == false)
            {
                return IsValidProjectInfo(CongViecMoi);
            }
            else
            {
                return (dalCV.AddData(CongViecMoi));
            }
        }

        //EDIT
        public (bool, string) EditTask(DTO_CongViec CongViecCanSua)
        {
            (bool result, string message) = IsValidProjectInfo(CongViecCanSua);
            if (result == false)
            {
                return IsValidProjectInfo(CongViecCanSua);
            }
            else
            {
                return (dalCV.SetData(CongViecCanSua));
            }
        }

        //SET STATUS



        //GETBy
        public DTO_CongViec GetByID(string MACV)
        {
            return dalCV.GetByID(MACV);
        }
        public (string, DataTable) GetByName(string name)
        {
            bool result = (IsValidNameTask(name));
            if (result == false)
            {
                return ("Ten cong viec khong hop le", null);
            }
            else
                return (null, dalCV.GetByName(name));
        }


        public BindingList<DTO_CongViec> FindCV(DTO_CongViec filter, long NganSachL, long NganSachH)
        {
            BindingList<DTO_CongViec> result = new BindingList<DTO_CongViec>();
            DataTable dsCongViec = dalCV.GetDataByFilter(filter, NganSachL, NganSachH);

            for (int i = 0; i < dsCongViec.Rows.Count; i++)
            {
                DTO_CongViec temp = new DTO_CongViec();
                temp.MADA = dsCongViec.Rows[i]["MADA"].ToString();
                temp.MACM = dsCongViec.Rows[i]["MACM"].ToString();
                temp.MACV = dsCongViec.Rows[i]["MACV"].ToString();
                temp.TENCV = dsCongViec.Rows[i]["TENCV"].ToString();
                temp.TSTART = dsCongViec.Rows[i]["TStart"].ToString();
                temp.TEND = dsCongViec.Rows[i]["TEnd"].ToString();
                temp.NGANSACH = long.Parse(Convert.ToInt64(dsCongViec.Rows[i]["NGANSACH"]).ToString());
                temp.DADUNG = long.Parse(Convert.ToInt64(dsCongViec.Rows[i]["DADUNG"]).ToString());
                temp.TIENDO = int.Parse(Convert.ToInt32(dsCongViec.Rows[i]["TIENDO"]).ToString());
                temp.YCDK = dsCongViec.Rows[i]["YCDINHKEM"].ToString();
                temp.TEPDK = dsCongViec.Rows[i]["TEPDINHKEM"].ToString();
                result.Add(temp);
            }
            return result;
        }

        public DataTable GetByNganSachMoreLess(long NganSachH, long NganSachL)
        {
            return dalDA.GetByNganSachMoreLess(NganSachH, NganSachL);
        }

        //check staff info 
        public static (bool, string) IsValidProjectInfo(DTO_CongViec CV)
        {
            if (CV == null)
                return (false, "Cong viec khong ton tai");
            if (!IsValidNameTask(CV.TENCV))
                return (false, "Ten cong viec khong hop le");
            if (!IsValidTSTART(CV.TSTART).Item1)
                return (IsValidTSTART(CV.TSTART));
            if (!IsValidTEND(CV.TEND, CV.TSTART).Item1)
                return (IsValidTEND(CV.TEND, CV.TSTART));
            return (true, "Thong tin hop le");
        }

        //check Project's ID
        private static bool IsValidNameTask(string tencv)
        {
            if (tencv == null)
                return false;
            else
            {
                foreach (char c in tencv)
                {
                    if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        //check start date
        public static (bool, string) IsValidTSTART(string NgayBatDau)
        {
            if (NgayBatDau == "")
                return (false, "Start date must not null");
            DateTime timeBD;
            bool A = DateTime.TryParseExact(NgayBatDau, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out timeBD);
            if (A == false) return (false, "Sai dinh dang ngay bat dau");
            if (timeBD.Date < DateTime.Now.Date) return (false, "Ngay bat dau phai bang/ sau hom nay");
            return (true, "");
        }

        //check end date 
        public static (bool, string) IsValidTEND(string NgayKetThuc, string NgayBatDau)
        {
            if (NgayBatDau == "" || NgayKetThuc == "")
                return (false, "Start date and End date must not null");
            DateTime time;
            bool A = DateTime.TryParseExact(NgayKetThuc, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out time);
            if (A == false) return (false, "Sai dinh dang ngay ket thuc");
            DateTime timeBD;
            A = DateTime.TryParseExact(NgayBatDau, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out timeBD);
            if (A == false) return (false, "Sai dinh dang ngay bat dau");
            if (time < timeBD) return (false, "Ngay ket thuc phai bang/ sau ngay bat dau");
            return (true, "");
        }
        //xoa
        public (bool, string) DeleteByID(DTO_CongViec CongViecCanXoa)
        {
            (bool, string) res1 = dalPC.DeleteByMACV(CongViecCanXoa.MACV);
            if (res1.Item1 == false && res1.Item2 != "") return res1;
            return dalCV.DeleteByID(CongViecCanXoa.MACV);
        }
    }
}
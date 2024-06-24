using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.AxHost;

namespace BUS
{
    public class BUS_DuAn
    {
        static BUS_DuAn _instance = new BUS_DuAn();
        public static BUS_DuAn Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BUS_DuAn();
                return _instance;
            }
        }

        DAL_DuAn dalDA = new DAL_DuAn();
        public BindingList<DTO_DuAn> GetAllData()
        {
            BindingList<DTO_DuAn> result = new BindingList<DTO_DuAn>();
            DataTable dsDuAn = dalDA.GetAllData();

            for (int i = 0; i < dsDuAn.Rows.Count; i++)
            {
                DTO_DuAn temp = new DTO_DuAn();
                temp.MADA = dsDuAn.Rows[i]["MaDA"].ToString();
                temp.MALSK = dsDuAn.Rows[i]["MaLSK"].ToString();
                temp.MAOWNER = dsDuAn.Rows[i]["MaOwner"] != null ? dsDuAn.Rows[i]["MaOwner"].ToString():"";
                temp.TENDA = dsDuAn.Rows[i]["TenDA"].ToString();
                temp.NGANSACH = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["NGANSACH"]).ToString());
                temp.TSTART = (dsDuAn.Rows[i]["TStart"] as DateTime?);
                temp.TEND = (dsDuAn.Rows[i]["TEnd"] as DateTime?);
                temp.STAT = dsDuAn.Rows[i]["TINHTRANG"].ToString();
                temp.DADUNG = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["DADUNG"]).ToString());
                temp.TIENDO = Convert.ToInt16(dsDuAn.Rows[i]["TIENDO"]);
                result.Add(temp);
            }
            return result;
        }
        
        //ADD
        public (bool, string) AddData(DTO_DuAn DuAnMoi)
        {
            (bool result, string message) = IsValidProjectInfo(DuAnMoi);
            if (result == false)
            {
                return IsValidProjectInfo(DuAnMoi);
            }
            else
            {
                return (dalDA.AddData(DuAnMoi));
            }
        }

        // DELETE
        public (bool, string) DeleteByID(DTO_DuAn DuAnCanXoa)
        {
            return dalDA.DeleteByID(DuAnCanXoa.MADA);
        }

        //EDIT
        public (bool, string) EditProject(DTO_DuAn DuAnCanSua)
        {
            (bool result, string message) = IsValidProjectInfo(DuAnCanSua, true);
            if (result == false)
            {
                return (result, message);
            }
            else
            {
                return (dalDA.SetData(DuAnCanSua));
            }
        }

        //SET STATUS
        public (bool, string) SetStatus(string MADA, string Status)
        {
            return (dalDA.SetStatByID(MADA, Status));
        }

        //GETBy
        public string GetStatbyID(string ID)
        {
            return dalDA.GetStatByID(ID);
        }
        public DTO_DuAn GetByID(string ID)
        {
            return dalDA.GetByID(ID);
        }
        public (string?, DataTable?) GetByName(string name)
        {
            bool result = (IsValidNameProject(name));
            if (result == false)
            {
                return ("Ten du an khong hop le", null);
            }
            else
                return (null, dalDA.GetByName(name));
        }

        //FindDA
        public BindingList<DTO_DuAn> FindDA(DTO_DuAn filter, long NganSachL, long NganSachH)
        {
            BindingList<DTO_DuAn> result = new BindingList<DTO_DuAn>();
            DataTable dsDuAn = dalDA.GetDataByFilter(filter, NganSachL, NganSachH);

            for (int i = 0; i < dsDuAn.Rows.Count; i++)
            {
                DTO_DuAn temp = new DTO_DuAn();
                temp.MADA = dsDuAn.Rows[i]["MaDA"].ToString();
                temp.MALSK = dsDuAn.Rows[i]["MaLSK"].ToString();
                temp.MAOWNER = dsDuAn.Rows[i]["MaOwner"] != null ? dsDuAn.Rows[i]["MaOwner"].ToString() : "";
                temp.TENDA = dsDuAn.Rows[i]["TenDA"].ToString();
                temp.TSTART = dsDuAn.Rows[i]["TStart"] as DateTime?;
                temp.TEND = dsDuAn.Rows[i]["TEnd"] as DateTime?;
                temp.NGANSACH = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["NGANSACH"]).ToString());
                temp.STAT = dsDuAn.Rows[i]["TINHTRANG"].ToString();
                temp.DADUNG = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["DADUNG"]).ToString());
                temp.TIENDO = Convert.ToInt16(dsDuAn.Rows[i]["TIENDO"]);
                result.Add(temp);
            }
            return result;
        }


        //check staff info 
        public static (bool, string) IsValidProjectInfo(DTO_DuAn DA, bool isEditing = false)
        {
            if (DA == null)
                return (false, "Du an khong ton tai");
            if (!IsValidNameProject(DA.TENDA))
                return (false, "Ten du an khong hop le");
            if (!isEditing)
                if (!IsValidTSTART(DA.TSTART).Item1)
                    return (IsValidTSTART(DA.TSTART));
            if (!IsValidTEND(DA.TEND, DA.TSTART).Item1)
                return (IsValidTEND(DA.TEND, DA.TSTART));
            return (true, "Thong tin hop le");
        }

        //check Project's name
        private static bool IsValidNameProject(string name)
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


        public static (bool, string) IsValidTSTART(DateTime? NgayBatDau)
        {
            if (NgayBatDau == null)
                return (false, "Start date must not null");
            // DateTime timeBD;
            //  bool A = DateTime.TryParseExact(NgayBatDau, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out timeBD);
            // if (A == false) return (false, "Sai dinh dang ngay bat dau");
            if (NgayBatDau.Value.Date < DateTime.Now.Date) return (false, "Ngay bat dau phai bang/ sau hom nay");
            return (true, "");
        }

        //check end date 
        public static (bool, string) IsValidTEND(DateTime? NgayKetThuc, DateTime? NgayBatDau)
        {
            if (NgayBatDau == null || NgayKetThuc == null)
                return (false, "Start date and End date must not null");
            /* DateTime time;
             bool A = DateTime.TryParseExact(NgayKetThuc, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out time);
             if (A == false) return (false, "Sai dinh dang ngay ket thuc");
             DateTime timeBD;
             A = DateTime.TryParseExact(NgayBatDau, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out timeBD);
             if (A == false) return (false, "Sai dinh dang ngay bat dau");*/
            if (NgayKetThuc < NgayBatDau) return (false, "Ngay ket thuc phai bang/ sau ngay bat dau");
            return (true, "");
        }


        //TONG NGAN SACH
        public long CalTongNganSach(BindingList<DTO_DuAn> table)
        {
            long res = 0;
            foreach (DTO_DuAn pj in  table)
            {
                res += pj.NGANSACH;
            }    
            return res;
        }

        //TONG DA DUNG
        public long CalTongDaDung(BindingList<DTO_DuAn> table)
        {
            long res = 0;
            foreach (DTO_DuAn pj in table)
            {
                res += pj.DADUNG;
            }
            return res;
        }
    }
}
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Drawing2D;
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
                temp.MAOWNER = dsDuAn.Rows[i]["MaOwner"].ToString();
                temp.TENDA = dsDuAn.Rows[i]["TenDA"].ToString();
                temp.NGANSACH = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["NGANSACH"]).ToString());
                temp.TSTART = dsDuAn.Rows[i]["TStart"].ToString();
                temp.TEND = dsDuAn.Rows[i]["TEnd"].ToString();
                temp.STAT = dsDuAn.Rows[i]["TINHTRANG"].ToString();
                temp.DADUNG = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["DADUNG"]).ToString());
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
            (bool result, string message) = IsValidProjectInfo(DuAnCanSua);
            if (result == false)
            {
                return IsValidProjectInfo(DuAnCanSua);
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
        public (string, DataTable) GetByName(string name)
        {
            bool result = (IsValidNameProject(name));
            if (result == false)
            {
                return ("Ten du an khong hop le", null);
            }
            else
                return (null, dalDA.GetByName(name));
        }
        public (string, DataTable) GetByTStartLimit(DateTime TStartLimit)
        {
            bool result = (IsValidTSTART(TStartLimit));
            if (result == false)
            {
                return ("Ten du an khong hop le", null);
            }
            else
                return (null, dalDA.GetByTStartLimit(TStartLimit));
        }
        public (string, DataTable) GetByTENDLimit(DateTime TEndLimit)
        {
            bool result = (IsValidTEND(TEndLimit));
            if (result == false)
            {
                return ("Ten du an khong hop le", null);
            }
            else
                return (null, dalDA.GetByTEndLimit(TEndLimit));
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
                temp.MAOWNER = dsDuAn.Rows[i]["MaOwner"].ToString();
                temp.TENDA = dsDuAn.Rows[i]["TenDA"].ToString();
                temp.TSTART = dsDuAn.Rows[i]["TStart"].ToString();
                temp.TEND = dsDuAn.Rows[i]["TEnd"].ToString();
                temp.NGANSACH = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["NGANSACH"]).ToString());
                temp.STAT = dsDuAn.Rows[i]["TINHTRANG"].ToString();
                temp.DADUNG = long.Parse(Convert.ToInt64(dsDuAn.Rows[i]["DADUNG"]).ToString());
                result.Add(temp);
            }
            return result;
        }

        public DataTable GetByNganSachMoreLess(long NganSachH, long NganSachL)
        {
            return dalDA.GetByNganSachMoreLess(NganSachH, NganSachL);
        }
        public DataTable GetByLoaiSK(string MALSK)
        {
            return dalDA.GetByLoaiSK(MALSK);
        }
        public DataTable GetByOwner(string MAOWNER)
        {
            return dalDA.GetByOwner(MAOWNER);
        }
        public DataTable GetByStat(string STAT)
        {
            return dalDA.GetByStat(STAT);
        }

        //check staff info 
        public static (bool, string) IsValidProjectInfo(DTO_DuAn DA)
        {
            if (DA == null)
                return (false, "Du an khong ton tai");
            if (!IsValidNameProject(DA.TENDA))
                return (false, "Ten du an khong hop le");
            if (!IsValidTSTART(DateTime.Now))
                return (false, "Ngay bat dau khong hop le");
            if (!IsValidTEND(DateTime.Now))
                return (false, "Ngay ket thuc khong hop le");
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


        //check start date
        public static bool IsValidTSTART(DateTime NgayBatDau)
        {
            if (NgayBatDau == null)
                return false;
            else
                return true;
        }

        //check end date 
        public static bool IsValidTEND(DateTime NgayKetThuc)
        {
            DateTime NgayBatDau;
            if (NgayKetThuc == null)
                return false;
            else
                return true;
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
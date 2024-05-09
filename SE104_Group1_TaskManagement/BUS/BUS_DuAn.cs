﻿using DAL;
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
                temp.MADA = dsDuAn.Rows[i]["MADA"].ToString();
                temp.MALSK = dsDuAn.Rows[i]["MALSK"].ToString();
                temp.MAOWNER = dsDuAn.Rows[i]["MAOWNER"].ToString();
                temp.TENDA = dsDuAn.Rows[i]["TENDA"].ToString();
                temp.NGANSACH = long.Parse(dsDuAn.Rows[i]["NGANSACH"].ToString());
                temp.TSTART = dsDuAn.Rows[i]["TSTART"].ToString();
                temp.TEND = dsDuAn.Rows[i]["TEND"].ToString();
                temp.STAT = dsDuAn.Rows[i]["STAT"].ToString();
                result.Add(temp);
            }
            return result;
        }

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
        public DTO_DuAn GetByID(string ID)
        {
            return dalDA.GetByID(ID);
        }
        public DataTable FindDA(DTO_DuAn filter)
        {
            return dalDA.GetDataByFilter(filter);
        }
        //check staff info 
        public static (bool, string) IsValidProjectInfo(DTO_DuAn DA)
        {
            if (DA == null)
                return (false, "Du an khong ton tai");
            if (!IsValidNameProject(DA.TENDA))
                return (false, "Ten du an hop le");
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
            if (NgayBatDau == null || NgayBatDau >= DateTime.Now)
                return false;
            else
                return true;
        }
        
        //check end date 
        public static bool IsValidTEND(DateTime NgayKetThuc)
        {
            if (NgayKetThuc == null || NgayKetThuc >= DateTime.Now)
                return false;
            else
                return true;
        }
    }
}

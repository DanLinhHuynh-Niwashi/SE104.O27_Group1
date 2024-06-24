using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DAL_ThamSo : BaseClass
    {
        static DAL_ThamSo _instance = new DAL_ThamSo();
        public static DAL_ThamSo Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DAL_ThamSo();
                return _instance;
            }
        }
        public DTO_ThamSo GetAllData()
        {

            DTO_ThamSo dt = new DTO_ThamSo();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM THAMSO";

                var command = new SqlCommand(
                    queryString,
                    conn);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                dt.PERCENTUP = (float)reader.GetDouble(0);
                reader.Close();
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                conn.Close();
                return dt;
            }

        }

        public (bool, string) SetData(DTO_ThamSo newTS)
        {

            try
            {
                conn.Open();
                string queryString = "UPDATE THAMSO SET PERCENTUP=@percentup";
                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@percentup", newTS.PERCENTUP);
                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Sửa thành công.");
                }

                conn.Close();
                return (false, "Sửa không thành công.");
            }
            catch (SqlException e)
            {
                Debug.Write(e.ToString());
                conn.Close();
                return (false, e.Message);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                conn.Close();
                return (false, ex.Message);
            }

        }
    }
}

using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_ChuyenMon:BaseClass
    {
        public string ConvertNametoID(string TenCM)
        {
            string macm;
            try
            {
                conn.Open();
                string cmString = "SELECT MACM FROM CHUYENMON WHERE TENCM='" + TenCM + "'";
                var command = new SqlCommand(cmString, conn);
                macm = command.ExecuteScalar().ToString();
                if (macm == null)
                { macm = ""; }
                conn.Close();
                return macm;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                macm = "";
                conn.Close();
                return macm;
            }
        }

        public string ConvertIDtoName(string MACM)
        {
            string tencm;
            try
            {
                conn.Open();
                string cmString = "SELECT TENCM FROM CHUYENMON WHERE MACM=" + MACM;
                var command = new SqlCommand(cmString, conn);
                tencm = (string)command.ExecuteScalar();
                if (tencm == null)
                { tencm = ""; }
                conn.Close();
                return tencm;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                tencm = "";
                conn.Close();
                return tencm;
            }
        }

        public (bool, string) SetData(DTO_ChuyenMon cm_new)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE CHUYENMON SET TENCM='"+cm_new.TENCM+"', INSHORT = '"+cm_new.INSHORT+"' WHERE MACM=" + cm_new.MACM +"";
                var command = new SqlCommand(
                    queryString,
                    conn);

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

        public (bool, string) AddData(DTO_ChuyenMon cm)
        {
            try
            {
                conn.Open();
                string queryString = "INSERT INTO CHUYENMON VALUES (@tencm, @inshort, 0)";
                var command = new SqlCommand(
                    queryString,
                    conn);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@inshort", cm.INSHORT);
                command.Parameters.AddWithValue("@tencm", cm.TENCM);

                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Thêm thành công.");
                }

                conn.Close();
                return (false, "Thêm không thành công.");
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

        public (bool, string) DeleteByID(string MACM)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE CHUYENMON SET isDeleted = 1 WHERE MACM=" + MACM;


                var command = new SqlCommand(
                    queryString,
                    conn);
                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Xóa chuyên môn thành công.");
                }
                conn.Close();
                return (false, "Xóa chuyên môn không thành công.");
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
        
        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM CHUYENMON Where isDeleted <> 1";
                var command = new SqlCommand(
                    queryString,
                    conn);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                return dt;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                conn.Close();
                return dt;
            }
        }

        public DataTable GetAllRawData()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM CHUYENMON";
                var command = new SqlCommand(
                    queryString,
                    conn);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                return dt;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                conn.Close();
                return dt;
            }
        }
    }
}

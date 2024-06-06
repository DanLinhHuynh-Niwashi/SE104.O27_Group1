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
    public class DAL_LoaiSK:BaseClass
    {
        public string ConvertNametoID(string TenLSK)
        {
            string macm;
            try
            {
                conn.Open();
                string cmString = "SELECT MALSK FROM LOAISK WHERE TENLSK=N'" + TenLSK + "'";
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

        public string ConvertIDtoName(string MALSK)
        {
            string tencm;
            try
            {
                conn.Open();
                string cmString = "SELECT TENLSK FROM LOAISK WHERE MALSK=" + MALSK;
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

        public (bool, string) SetData(DTO_LoaiSK lsk_new)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE LOAISK SET TENLSK= @tenlsk, MONEYMIN=@min, MONEYMAX=@max WHERE MALSK = @malsk";
                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@tenlsk", lsk_new.TENLSK);
                command.Parameters.AddWithValue("@min", lsk_new.MIN);
                command.Parameters.AddWithValue("@max", lsk_new.MAX);
                command.Parameters.AddWithValue("@malsk", lsk_new.MALSK);
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

        public (bool, string) AddData(DTO_LoaiSK lsk)
        {
            try
            {
                conn.Open();
                string queryString = "INSERT INTO LOAISK VALUES (@tenlsk, @min, @max, @inshort, 0)";
                var command = new SqlCommand(
                    queryString,
                    conn);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@tenlsk", lsk.TENLSK);
                command.Parameters.AddWithValue("@min", lsk.MIN);
                command.Parameters.AddWithValue("@max", lsk.MAX);
                command.Parameters.AddWithValue("@inshort", lsk.INSHORT);
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

        public (bool, string) DeleteByID(string MALSK)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE LOAISK set isDeleted = 1 WHERE MALSK=" + MALSK;


                var command = new SqlCommand(
                    queryString,
                    conn);
                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Xóa loại sự kiện thành công.");
                }
                conn.Close();
                return (false, "Xóa loại sự kiện không thành công.");
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

        public (long, long) GetMinMaxByID (string MALSK)
        {
            (long, long) res = (0,0);
            try
            {
                conn.Open();
                string queryString = "SELECT MONEYMIN, MONEYMAX FROM LOAISK WHERE MALSK='" + MALSK + "'";
                var command = new SqlCommand(
                    queryString,
                    conn);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                res.Item1 = (long)reader.GetDecimal(0);
                res.Item2 = (long)reader.GetDecimal(1);
                conn.Close();
                return res;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                conn.Close();
                return res;
            }
        }
        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM LOAISK Where isDeleted <> 1";
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
                string queryString = "SELECT * FROM LOAISK";
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

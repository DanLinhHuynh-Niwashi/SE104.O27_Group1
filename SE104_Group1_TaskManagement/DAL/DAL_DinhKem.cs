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
    public class DAL_DinhKem:BaseClass
    {
        public (bool, string) AddData(DTO_DinhKem dk)
        {
            try
            {
                conn.Open();
                string queryString = "INSERT INTO DINHKEM VALUES (@manv, @macv, @tep)";
                var command = new SqlCommand(
                    queryString,
                    conn);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@macv", dk.MACV);
                command.Parameters.AddWithValue("@manv", dk.MANV);
                command.Parameters.AddWithValue("@tep", dk.TEP);

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

        public (bool, string) Delete(DTO_DinhKem dk)
        {
            try
            {
                conn.Open();
                string queryString = "DELETE FROM DINHKEM WHERE MANV=@manv and MACV = @macv and TEP = @tep";
                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.AddWithValue("@macv", dk.MACV);
                command.Parameters.AddWithValue("@manv", dk.MANV);
                command.Parameters.AddWithValue("@tep", dk.TEP);

                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Xóa thành công.");
                }
                conn.Close();
                return (false, "Xóa không thành công.");
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

        public DataTable GetByMANV(string MANV)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM DINHKEM WHERE MANV=@manv";
                var command = new SqlCommand(
                    queryString,
                    conn);
                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.AddWithValue("@manv", MANV);

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
        public DataTable GetByMANVandMACV(string MANV, string MACV)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM DINHKEM WHERE MANV=@manv and MACV = @macv";
                var command = new SqlCommand(
                    queryString,
                    conn);
                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.AddWithValue("@manv", MANV);
                command.Parameters.AddWithValue("@macv", MACV);
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
        public DataTable GetByMACV(string MACV)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM DINHKEM WHERE MACV=@macv";
                var command = new SqlCommand(
                    queryString,
                    conn);
                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.AddWithValue("@macv", MACV);
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
        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM DINHKEM";
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

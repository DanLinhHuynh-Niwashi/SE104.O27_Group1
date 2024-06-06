using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace DAL
{
    public class DAL_DuAn:BaseClass
    {
        public (bool, string) AddData(DTO_DuAn duAn)
        {
            try
            {
                string mada = getCrnID();

                conn.Open();
                string queryString = "INSERT INTO DU AN VALUES (@mada, @malsk, @tenda, @ngansach, CONVERT(smalldatetime,@tstart, 104),  CONVERT(smalldatetime,@tend, 104), @maowner, @stat, @dadung)";
                var command = new SqlCommand(
                    queryString,
                    conn);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@mada", mada);
                command.Parameters.AddWithValue("@malsk", duAn.MALSK);
                command.Parameters.AddWithValue("@tenda", duAn.TENDA);
                command.Parameters.AddWithValue("@ngansach", duAn.NGANSACH);
                command.Parameters.AddWithValue("@tstart", duAn.TSTART);
                command.Parameters.AddWithValue("@tend", duAn.TEND);
                if (duAn.MAOWNER == "")
                {
                    command.Parameters.AddWithValue("@maowner", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@maowner", duAn.MAOWNER);
                }
                command.Parameters.AddWithValue("@stat", duAn.STAT);
                command.Parameters.AddWithValue("@dadung", 0);
                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Thêm thành công!");
                }

                conn.Close();
                return (false, "Thêm không thành công!");
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
        public (bool, string) SetData(DTO_DuAn da_new)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE DUAN SET TENDA=@tenda, MALSK=@malsk, NGANSACH=@ngansach, TSTART=CONVERT(smalldatetime,@tstart, 104), TEND= CONVERT(smalldatetime,@tend, 104), MAOWNER=@maowner, TINHTRANG=@stat WHERE MADA=@mada";
                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@mada", da_new.MADA);
                command.Parameters.AddWithValue("@malsk", da_new.MALSK);
                command.Parameters.AddWithValue("@tenda", da_new.TENDA);
                command.Parameters.AddWithValue("@ngansach", da_new.NGANSACH);
                command.Parameters.AddWithValue("@tstart", da_new.TSTART);
                command.Parameters.AddWithValue("@tend", da_new.TEND);
                if (da_new.MAOWNER =="")
                {
                    command.Parameters.AddWithValue("@maowner", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@maowner", da_new.MAOWNER);
                }
                
                command.Parameters.AddWithValue("@stat", da_new.STAT);
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
        public (bool, string) SetStatByID(string MADA, string Stat)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE DUAN SET TINHTRANG = '"+Stat+"' WHERE MADA='" + MADA + "'";


                var command = new SqlCommand(
                    queryString,
                    conn);
                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Chỉnh sửa tình trạng dự án thành công.");
                }
                conn.Close();
                return (false, "Chỉnh sửa tình trạng dự án không thành công.");
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
        public string GetStatByID(string MADA)
        {
            try
            {
                conn.Open();
                string queryString = "SELECT TINHTRANG FROM DUAN WHERE MADA='" + MADA + "'";
                var command = new SqlCommand(
                   queryString,
                   conn);
                string stat = (string)command.ExecuteScalar();
                if (stat == null) stat = "";
                conn.Close();
                return stat;
            }
            catch (SqlException e)
            {
                Debug.Write(e.ToString());
                conn.Close();
                return "";
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                conn.Close();
                return "";
            }
        }
        public DTO_DuAn GetByID(string MADA)
        {

            try
            {
                DTO_DuAn res = new DTO_DuAn();
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH, DADUNG, CONVERT(VARCHAR(10),TSTART,104) as TSTART,  CONVERT(VARCHAR(10),TEND,104) as TEND, MAOWNER, TINHTRANG FROM DUAN WHERE MADA=@mada\r\n";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@mada", MADA);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                res.MADA = reader.GetString(0);
                res.TENDA = reader.GetString(1);
                res.MALSK = reader.GetInt32(2).ToString();
                res.NGANSACH = (long)reader.GetDecimal(3);
                res.DADUNG = (long)reader.GetDecimal(4);
                res.TSTART = reader.GetString(5);
                res.TEND = reader.GetString(6);
                res.MAOWNER = reader.IsDBNull(7) ? "":reader.GetString(7);
                res.STAT = reader.GetString(8);
                
                reader.Close();
                conn.Close();
                return res;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                conn.Close();
                return null;
            }
        }
        public DataTable GetByName(string TENDA)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH,  CONVERT(VARCHAR(10),TSTART,104) AS TStart,  CONVERT(VARCHAR(10),TEND,104) AS TEnd, MAOWNER, TINHTRANG FROM DUAN" +
                    " WHERE TENDA LIKE @tenda";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@tenda", TENDA);

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

        public DataTable GetByTStartLimit(DateTime TStartLimit) //lấy dự án bắt đầu sau mốc thời gian TStart
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH,  CONVERT(VARCHAR(10),TSTART,104) AS TStart,  CONVERT(VARCHAR(10),TEND,104) AS TEnd, MAOWNER, TINHTRANG FROM DUAN" +
                    " WHERE TStart <= CONVERT(smalldatetime,@tstart, 104)";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@tstart", TStartLimit);

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

        public DataTable GetByTEndLimit(DateTime TEndLimit) //lấy dự án kết thúc trước mốc thời gian TEnd
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH, CONVERT(VARCHAR(10),TSTART,104) AS TStart, CONVERT(VARCHAR(10),TEND,104) AS TEnd, MAOWNER, TINHTRANG FROM DUAN" +
                    " WHERE TEnd >= CONVERT(smalldatetime,@tend, 104)";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@tend", TEndLimit);

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

        public DataTable GetByNganSachMoreLess(long NganSachH, long NganSachL) //lấy dự án có ngân sách ít hơn NganSachH, nhiều hơn NganSachL
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH,  CONVERT(VARCHAR(10),TSTART,104) AS TStart, CONVERT(VARCHAR(10),TEND,104) AS TEnd, MAOWNER, TINHTRANG FROM DUAN" +
                    " WHERE NGANSACH => @ngansachl AND NGANSACH <= @ngansachh";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ngansachh", NganSachH);
                command.Parameters.AddWithValue("@ngansachl", NganSachL);
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

        public DataTable GetByLoaiSK(string MALSK)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH, CONVERT(VARCHAR(10),TSTART,104) AS TStart, CONVERT(VARCHAR(10),TEND,104) AS TEnd, MAOWNER, TINHTRANG FROM DUAN WHERE MALSK = @malsk";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@malsk", MALSK);

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
        public DataTable GetByOwner(string MAOWNER)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH,  CONVERT(VARCHAR(10),TSTART,104) AS TStart,  CONVERT(VARCHAR(10),TEND,104) AS TEnd, MAOWNER, TINHTRANG FROM DUAN WHERE MAOWNER = @maowner";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@maowner", MAOWNER);

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
        public DataTable GetByStat(string STAT)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH, CONVERT(VARCHAR(10),TSTART,104) AS TStart, CONVERT(VARCHAR(10),TEND,104) AS TEnd, MAOWNER, TINHTRANG FROM DUAN WHERE TINHTRANG LIKE @stat";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@stat", STAT);

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
        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                
                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH, DADUNG, CONVERT(VARCHAR(10),TSTART,104) as TSTART,  CONVERT(VARCHAR(10),TEND,104) as TEND, MAOWNER, TINHTRANG FROM DUAN WHERE MADA IS NOT NULL\r\n";

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

        //Nếu có filter nào, set giá trị của filter đó vào DTO, nếu không có thì set "" với string và -1 với số
        //Ngân sách nằm trong khoảng [NganSachL, NganSachH], nạp thêm thông tin này nếu cần dùng
        //Với thời gian, nằm trong khoảng [TSTART, TEND] (dự án bắt đầu sau TSTART, kết thúc trước TEND)
        public DataTable GetDataByFilter(DTO_DuAn filter, long NganSachL = -1, long NganSachH = -1)
        {
            DataTable dt = new DataTable();
            try
            {

                conn.Open();
                string queryString = "SELECT MADA, TENDA, MALSK, NGANSACH, DADUNG, CONVERT(VARCHAR(10),TSTART,104) as TStart,  CONVERT(VARCHAR(10),TEND,104) as TEnd, MAOWNER, TINHTRANG FROM DUAN WHERE MADA IS NOT NULL";

                if (filter.MADA != "" || filter.TENDA != "")
                {
                    queryString += " AND MADA LIKE '%" + filter.MADA + "%' OR TENDA LIKE '%" + filter.TENDA + "%'";
                }   
                if (filter.MALSK != "")
                {
                    queryString += " AND MALSK = " + filter.MALSK;
                }   
                if (filter.MAOWNER != "")
                {
                    queryString += " AND MAOWNER LIKE '%" + filter.MAOWNER + "%'";
                }   
                if (NganSachL != -1)
                {
                    queryString += " AND NGANSACH >= " + NganSachL;
                }   
                if (NganSachH != -1)
                {
                    queryString += " AND NGANSACH <= " + NganSachH;
                }
                if (filter.STAT != "")
                {
                    queryString += " AND TINHTRANG = '" + filter.STAT +"'";
                }   
                if (filter.TSTART != "")
                {
                    queryString += " AND TSTART >= CONVERT(smalldatetime,'" + filter.TSTART +"', 104)";
                }    
                if (filter.TEND != "")
                {
                    queryString += " AND TEND <= CONVERT(smalldatetime,'" + filter.TEND + "', 104)";
                }    
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
        string getCrnID()
        {
            try
            {
                conn.Open();
                string idString = "Select count(MADA) from DUAN";
                var command = new SqlCommand(idString, conn);
                int id = (int)command.ExecuteScalar();
                id = id + 1;
                conn.Close();
                return id.ToString();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                conn.Close();
                return "";
            }
        }

        public (bool, string) DeleteByID(string MADA)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE DUAN SET IsDeleted = 1 WHERE MADA='" + MADA + "'";


                var command = new SqlCommand(
                    queryString,
                    conn);
                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Xóa project thành công.");
                }
                conn.Close();
                return (false, "Xóa project không thành công.");
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

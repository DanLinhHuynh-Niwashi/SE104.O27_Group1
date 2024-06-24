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
    public class DAL_QuyenHan : BaseClass
    {
        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM QUYENHAN";
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
        public bool CheckPermission(DTO_TaiKhoan tk, string action)
        {
            try
            {
                conn.Open();
                string queryString = @"SELECT COUNT(*) 
                                       FROM TAIKHOAN nv
                                       JOIN QUYENHAN qh ON nv.MaQH = qh.MaQH
                                       JOIN CT_QUYENHAN cth ON qh.MaQH = cth.MaQH
                                       WHERE nv.MANV = @maNV AND cth.Action = @action";
                var command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@maNV", tk.MANV);
                command.Parameters.AddWithValue("@action", action);
                int count = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                conn.Close();
                return false;
            }
        }
       
    }
}
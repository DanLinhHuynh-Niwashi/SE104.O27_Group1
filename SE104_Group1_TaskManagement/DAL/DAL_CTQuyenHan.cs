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
    public class DAL_CTQuyenHan : BaseClass
    {
        public (bool, string) AddPermission(DTO_QuyenHan QH, string action)
        {
            try
            {
                conn.Open();
                string queryString = @"INSERT INTO CT_QUYENHAN VALUES (@maQH, @action)";
                var command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@maQH", QH.MAQH);
                command.Parameters.AddWithValue("@action", action);
                command.ExecuteNonQuery();
                conn.Close();
                return (true, "Thêm hoạt động thành công!");
            }
            catch (Exception ex)
            {
                conn.Close();
                return (false, "Error: " + ex.Message);
            }
        }

        public DataTable GetAllDataByQH(DTO_QuyenHan QH)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT * FROM CT_QUYENHAN WHERE MAQH = @maqh";

                var command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@maQH", QH.MAQH);

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

        public (bool, string) DeletePermission(DTO_QuyenHan QH, string action)
        {
            try
            {
                conn.Open();
                string queryString = @"DELETE FROM CT_QUYENHAN WHERE MAQH = @maQH AND ACTION = @action";
                var command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@maQH", QH.MAQH);
                command.Parameters.AddWithValue("@action", action);
                int a = command.ExecuteNonQuery();
                conn.Close();
                if (a > 0)
                {
                    return (true, "Xóa hoạt động thành công!");
                }
                return (false, "Xóa hoạt động không thành công!");
            }
            catch (Exception ex)
            {
                conn.Close();
                return (false, "Error: " + ex.Message);
            }
        }


    }
}

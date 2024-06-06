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
    public class DAL_TaiKhoan : BaseClass
    {

        public (DTO_TaiKhoan, string) CheckLogicDTO(DTO_TaiKhoan taikhoan)
        {
            DTO_TaiKhoan user = new DTO_TaiKhoan();
            // Hàm connect tới CSDL 
            //SqlConnection conn = BaseClass;
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("proc_logic", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@user", taikhoan.EMAIL);
                command.Parameters.AddWithValue("@pass", taikhoan.PASS);
                // Kiểm tra quyền các bạn thêm 1 cái parameter...
                command.Connection = conn;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.MAQH = reader.GetString(0);
                        user.EMAIL = reader.GetString(1);
                        user.PASS = reader.GetString(2);
                        user.MANV = reader.GetString(3);
                    }
                    reader.Close();
                    conn.Close();

                }
                else
                {
                    reader.Close();
                    conn.Close();
                }

                return (user, "");
            }
            catch(Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }    
                return (null, ex.Message);
            }
        }

        public string TaoMoiTaiKhoan(DTO_TaiKhoan taiKhoan)
        {
            try
            {
                conn.Open();


                /* // Truy vấn để lấy mã nhân viên mới
                string query = "SELECT MAX(CAST(SUBSTRING(MANV, 3, LEN(MANV) - 2) AS INT)) FROM NHANVIEN";
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                int newManv = 1;

                if (result != DBNull.Value)
                {
                    newManv = Convert.ToInt32(result) + 1;
                }

                // Tạo mã nhân viên mới
                string newManvString = "NV" + newManv.ToString("0000");

                // Thêm mã nhân viên vào đối tượng tài khoản
                taiKhoan.MANV = newManvString;*/

                // Tạo tài khoản

                SqlCommand command = new SqlCommand("proc_tao_tai_khoan", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@maqh", taiKhoan.MAQH);
                command.Parameters.AddWithValue("@email", taiKhoan.EMAIL);
                command.Parameters.AddWithValue("@pass", taiKhoan.PASS);
                command.Parameters.AddWithValue("@manv", taiKhoan.MANV);


                command.ExecuteNonQuery();
                conn.Close();
                return "Tạo tài khoản thành công!";
            }
            catch (SqlException ex)
            {
                conn.Close();
                return "Lỗi khi tạo tài khoản: " + ex.Message;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public (string, DTO_TaiKhoan) ChangePassword(string email, string oldPassword, string newPassword)
        {
            DTO_TaiKhoan res = new DTO_TaiKhoan();
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("proc_change_password", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@OldPassword", oldPassword);
                command.Parameters.AddWithValue("@NewPassword", newPassword);
                command.ExecuteNonQuery();
                conn.Close();
                (res, string a) = CheckLogicDTO(new DTO_TaiKhoan("", email, newPassword, ""));
                
                return ("Đổi mật khẩu thành công!", res);
            }
            catch (SqlException ex)
            {
                conn.Close();
                return ("Lỗi khi đổi mật khẩu: " + ex.Message, res);
            }
            finally
            {
                conn.Close();
            }
        }

        public string ChangeQH(string manv, string newMAQH)
        {
            try
            {
                conn.Open();
                string query = "UPDATE TAIKHOAN SET MAQH = @newMAQH WHERE MANV = @manv";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@newMAQH", newMAQH);
                command.Parameters.AddWithValue("@manv", manv);
                command.ExecuteNonQuery();
                conn.Close();
                return "";
            }
            catch (SqlException ex)
            {
                conn.Close();
                return (ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public DTO_QuyenHan GetQH(string manv)
        {
            DTO_QuyenHan res = new DTO_QuyenHan();
            try
            {
                conn.Open();
                string query = "SELECT * FROM QUYENHAN JOIN TAIKHOAN ON QUYENHAN.MAQH = TAIKHOAN.MAQH WHERE TAIKHOAN.MANV = @manv";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@manv", manv);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                res.MAQH = reader.GetString(0);
                res.TENQH = reader.GetString(1);
                reader.Close();
                conn.Close();
                return res;
            }
            catch (SqlException ex)
            {
                conn.Close();
                return res;
            }
            finally
            {
                conn.Close();
            }
        }

        //}
        //--Thinh code
        //public static DTO_NhanVien CheckLogicDTO(DTO_TaiKhoan taikhoan)
        //{
        //    DTO_NhanVien user = null;


        //    return user;
        //}
    }
}

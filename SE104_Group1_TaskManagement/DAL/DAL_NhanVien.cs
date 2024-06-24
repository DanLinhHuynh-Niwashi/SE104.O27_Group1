﻿using System;
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
    public class DAL_NhanVien : BaseClass
    {
        public (bool, string) SetData(DTO_NhanVien nv_new)
        {
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand("proc_sua_nhan_vien", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@manv", nv_new.MANV);
                command.Parameters.AddWithValue("@hoten", nv_new.TENNV);
                command.Parameters.AddWithValue("@khongdau", ConvertToUnsign(nv_new.TENNV));
                command.Parameters.AddWithValue("@email", nv_new.EMAIL);
                command.Parameters.AddWithValue("@sodt", nv_new.PHONE);
                command.Parameters.AddWithValue("@phai", nv_new.GENDER);
                command.Parameters.AddWithValue("@ngsinh", nv_new.NGAYSINH);
                command.Parameters.AddWithValue("@lvl", nv_new.LEVEL);
                command.Parameters.AddWithValue("@macm", nv_new.MACM);
                command.Parameters.AddWithValue("@ghichu", nv_new.GHICHU);

                command.ExecuteNonQuery();
                conn.Close();
                return (true, "Sửa thành công.");
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
        public (bool, string) DeleteByID(string MANV)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE NHANVIEN SET IsDeleted = 1 WHERE MANV='" + MANV + "'";


                var command = new SqlCommand(
                    queryString,
                    conn);
                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Xóa nhân viên thành công.");
                }
                conn.Close();
                return (false, "Xóa nhân viên không thành công.");
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
        public (bool, string) AddData(DTO_NhanVien nhanVien)
        {
            try
            {
                string manv = getCrnID();
                //thêm tên cm vào
                conn.Open();

                SqlCommand command = new SqlCommand("proc_tao_nhan_vien", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@manv", manv);
                command.Parameters.AddWithValue("@hoten", nhanVien.TENNV);
                command.Parameters.AddWithValue("@khongdau", ConvertToUnsign(nhanVien.TENNV));
                command.Parameters.AddWithValue("@email", nhanVien.EMAIL);
                command.Parameters.AddWithValue("@sodt", nhanVien.PHONE);
                command.Parameters.AddWithValue("@ngsinh", nhanVien.NGAYSINH);
                command.Parameters.AddWithValue("@phai", nhanVien.GENDER);
                command.Parameters.AddWithValue("@lvl", nhanVien.LEVEL);
                command.Parameters.AddWithValue("@macm", nhanVien.MACM);
                command.Parameters.AddWithValue("@ghichu", nhanVien.GHICHU);

                command.ExecuteNonQuery();
                conn.Close();
                return (true, manv);

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
        public DTO_NhanVien GetByID(string MANV)
        {
            DTO_NhanVien res = new DTO_NhanVien();
            try
            {

                conn.Open();
                string queryString = "SELECT MANV, HOTEN, EMAIL, SODT, CONVERT(DATETIME, NGSINH, 104) as NGSINH, LVL, MACM, GHICHU, PHAI FROM NHANVIEN WHERE MANV=@manv AND IsDeleted <> 1";

                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@manv", MANV);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                res.MANV = reader.GetString(0);
                res.TENNV = reader.GetString(1);
                res.EMAIL = reader.GetString(2);
                res.PHONE = reader.GetString(3);
                res.NGAYSINH = reader.GetDateTime(4);
                res.LEVEL = reader.GetInt16(5);
                res.MACM = reader.GetInt32(6).ToString();
                res.GHICHU = reader.GetString(7);
                res.GENDER = reader.GetString(8);
                reader.Close();
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
                string queryString = "SELECT MANV, HOTEN, EMAIL, SODT, CONVERT(DATETIME, NGSINH, 104) as NGSINH, LVL, MACM, GHICHU, PHAI FROM NHANVIEN WHERE IsDeleted <> 1";

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
                string queryString = "SELECT MANV, HOTEN, EMAIL, SODT, CONVERT(DATETIME, NGSINH, 104) as NGSINH, LVL, MACM, GHICHU, PHAI FROM NHANVIEN";

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
        public DataTable GetDataByFilter(DTO_NhanVien filter)
        {
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                string queryString = "SELECT MANV, HOTEN, EMAIL, SODT, CONVERT(DATETIME, NGSINH, 104) as NGSINH, LVL, MACM, GHICHU, PHAI FROM NHANVIEN WHERE IsDeleted <> 1";

                if (filter.MANV != "" || filter.TENNV != "")
                {
                    string cvt = ConvertToUnsign(filter.TENNV);
                    Debug.WriteLine(cvt);
                    queryString += " AND MANV LIKE '%" + filter.MANV + "%' OR HOTENKHONGDAU LIKE '%" + cvt + "%'";
                }
                if (filter.EMAIL != "")
                {
                    queryString += " AND EMAIL LIKE '%" + filter.EMAIL + "%'";
                }
                if (filter.PHONE != "")
                {
                    queryString += " AND SODT LIKE '%" + filter.PHONE + "%'";
                }
                if (filter.LEVEL != -1)
                {
                    queryString += " AND LVL=" + filter.LEVEL;
                }
                if (filter.MACM != "")
                {
                    queryString += " AND MACM=" + filter.MACM;
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
                string idString = "SELECT TOP 1 MANV FROM NHANVIEN ORDER BY MANV DESC";
                var command = new SqlCommand(idString, conn);
                string id = (string)command.ExecuteScalar();
                int number = 0;
                if (id != null)
                {
                    number = int.Parse(id.Substring(id.Length - 4)) + 1;
                }

                conn.Close();
                return "NV" + number.ToString("0000");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                conn.Close();
                return "";
            }
        }

    }
}
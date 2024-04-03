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
    public class DAL_NhanVien:BaseClass
    {
        public (bool, string) SetData(DTO_NhanVien nv_new)
        {
            try
            {
                conn.Open();
                string queryString = "UPDATE NHANVIEN SET HOTEN=@hoten, EMAIL=@email, SODT=@sdt, NGSINH= CONVERT(smalldatetime,@ngaysinh, 104), LVL=@lvl, MACM=@macm, GHICHU=@ghichu WHERE MANV=@manv";
                var command = new SqlCommand(
                    queryString,
                    conn);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@manv", nv_new.MANV);
                command.Parameters.AddWithValue("@hoten", nv_new.TENNV);
                command.Parameters.AddWithValue("@email", nv_new.EMAIL);
                command.Parameters.AddWithValue("@sdt", nv_new.PHONE);
                command.Parameters.AddWithValue("@ngaysinh", nv_new.NGAYSINH);
                command.Parameters.AddWithValue("@lvl", nv_new.LEVEL);
                command.Parameters.AddWithValue("@macm", nv_new.MACM);
                command.Parameters.AddWithValue("@ghichu", nv_new.GHICHU);
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
        public (bool, string) DeleteByID (string MANV)
        {
            try
            {
                conn.Open();
                string queryString = "DELETE FROM NHANVIEN WHERE MANV='" + MANV + "'";


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

                conn.Open();
                string queryString = "INSERT INTO NHANVIEN VALUES (@manv, @hoten, @email, @sdt, CONVERT(smalldatetime,@ngaysinh, 104), @lvl, @macm, @ghichu)";
                var command = new SqlCommand(
                    queryString,
                    conn);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@manv", manv);
                command.Parameters.AddWithValue("@hoten", nhanVien.TENNV);
                command.Parameters.AddWithValue("@email", nhanVien.EMAIL);
                command.Parameters.AddWithValue("@sdt", nhanVien.PHONE);
                command.Parameters.AddWithValue("@ngaysinh", nhanVien.NGAYSINH);
                command.Parameters.AddWithValue("@lvl", nhanVien.LEVEL);
                command.Parameters.AddWithValue("@macm", nhanVien.MACM);
                command.Parameters.AddWithValue("@ghichu", nhanVien.GHICHU);

                if (command.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return (true, "Thêm nhân viên thành công!");
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
        public DTO_NhanVien GetByID(string MANV)
        {
            
            try
            {
                DTO_NhanVien res = new DTO_NhanVien();
                conn.Open();
                string queryString = "SELECT MANV, HOTEN, EMAIL, SODT, CONVERT(VARCHAR(10), NGSINH, 104), LVL, MACM, GHICHU FROM NHANVIEN WHERE MANV=@manv";

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
                res.NGAYSINH = reader.GetString(4);
                res.LEVEL = reader.GetInt16(5);
                res.MACM = reader.GetString(6);
                res.GHICHU = reader.GetString(7);
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
        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string queryString = "SELECT MANV, HOTEN, EMAIL, SODT, CONVERT(VARCHAR(10), NGSINH, 104), LVL, MACM, GHICHU FROM NHANVIEN";

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

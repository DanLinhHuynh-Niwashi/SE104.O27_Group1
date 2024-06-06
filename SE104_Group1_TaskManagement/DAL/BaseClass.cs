using Microsoft.Data.SqlClient;
using System;
using System.Data;
using DTO;
using System.Text.RegularExpressions;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using Diacritics.Extensions;
using System.Diagnostics;

namespace DAL
{
    public class BaseClass
    {
        protected static string connectionString = "Server=tcp:se104-task-management.database.windows.net,1433;Initial Catalog=se104_Task_Management_db;Persist Security Info=False;User ID=imAdmin;Password=Abcde12345.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        protected SqlConnection conn = new SqlConnection(connectionString);

        public string ConvertToUnsign(string strInput)
        {
            string cvt = strInput.RemoveDiacritics().ToLower();
            Debug.WriteLine(cvt);

            return cvt;
        }
    }
    
}

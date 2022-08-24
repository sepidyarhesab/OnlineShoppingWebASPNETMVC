using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SepidyarHesabExtensions.Classes
{
    public static class SepidyarHesabCodeGenerator
    {
        public static string GenerateCode(string schema , string table)
        {
            var message = "SP-000000";
            try
            {
                message = "SP-" + DateTime.Now.Ticks;
                //var random = new Random();
                //message = "SP-" + random.Next(100000, 999999);
                //string con = WebConfigurationManager.AppSettings["Connection"];
                //SqlConnection conn = new SqlConnection(con);
                //conn.Open();
                //SqlCommand command = new SqlCommand(" SELECT CASE WHEN EXISTS ( SELECT Code FROM " + "[" + schema + "]" + "." + "[" + table + "]" + " WHERE Code = '"+message+"' ) THEN 'TRUE' ELSE 'FALSE' END", conn);
                //string result = command.ExecuteScalar().ToString();
                //if (result =="FALSE")
                //{
                //    return message;
                //}
                //else
                //{
                //    message = "SP-" + random.Next(100000, 999999);
                //    conn.Open();
                //    SqlCommand command2 = new SqlCommand(" SELECT CASE WHEN EXISTS ( SELECT Code FROM " + "[" + schema + "]" + "." + "[" + table + "]" + " WHERE Code = '" + message + "' ) THEN 'TRUE' ELSE 'FALSE' END", conn);
                //    string result2 = command2.ExecuteScalar().ToString();
                //    if (result2 == "FALSE")
                //    {
                //        return message;
                //    }
                //    else
                //    {
                //        message = "000000";
                //    }
                //}

                //conn.Close();
            }
            catch (Exception e)
            {
                message = "SP-" + DateTime.Now.Ticks;
            }
            return message;
        }
    }
}

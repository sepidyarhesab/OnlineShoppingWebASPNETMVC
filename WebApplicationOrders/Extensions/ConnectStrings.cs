using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SepidyarHesabDrWebApplication.Extensions
{
    public static class ConnectStrings
    {
        public static string ConnectionSepidar()
        {
            string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Sepidar.txt");

            string connectionString = @"data source=" + lines[0] + ";initial catalog=" + lines[1] + ";persist security info=True;user id=" + lines[2] + ";password=" + lines[3] + ";MultipleActiveResultSets=True;App=EntityFramework;";
            string format = "metadata=res://*/Database.SepidarDatabase.SepidarData.csdl|res://*/Database.SepidarDatabase.SepidarData.ssdl|res://*/Database.SepidarDatabase.SepidarData.msl;provider=System.Data.SqlClient;provider connection string=\"{0}\"";
            string type = "providerName=System.Data.EntityClient";
            return String.Format(format, connectionString, type);
        }
        public static string ConnectionSepidyarHesab()
        {
            string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/SepidyarHesab.txt");
            string connectionString = @"data source=" + lines[0] + ";initial catalog=" + lines[1] + ";persist security info=True;user id=" + lines[2] + ";password=" + lines[3] + ";MultipleActiveResultSets=True;App=EntityFramework;";
            string format = "metadata=res://*/Database.SepidyarHesabDatabase.SepidyarHesabData.csdl|res://*/Database.SepidyarHesabDatabase.SepidyarHesabData.ssdl|res://*/Database.SepidyarHesabDatabase.SepidyarHesabData.msl;provider=System.Data.SqlClient;provider connection string=\"{0}\"";
            string type = "providerName=System.Data.EntityClient";
            return String.Format(format, connectionString, type);
        }
    }
}
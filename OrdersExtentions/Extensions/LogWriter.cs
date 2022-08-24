using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OrdersExtentions.Extensions
{
    public static class LogWriter
    {
        public static void Logger(string message, string datetime, string line)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/Log.txt";
            try
            {
                var msg = "Application Write  : " + message + " Datetime : " + datetime + " Line  : " + line;
                if (File.Exists(path))
                {

                    File.AppendAllLines(path, new[] { msg });
                }
                else
                {
                    using (StreamWriter sww = File.CreateText(path))
                    {
                        sww.WriteLine(msg);
                        sww.WriteLine(Environment.NewLine);
                        sww.Close();
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllLines(path, new[] { e.Message });
            }
        }

        public static void Orders(string message, string datetime, string line)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/Order.txt";
            try
            {
                var msg = "Application Write  : " + message + " Datetime : " + datetime + " Line  : " + line;
                if (File.Exists(path))
                {

                    File.AppendAllLines(path, new[] { msg });
                }
                else
                {
                    using (StreamWriter sww = File.CreateText(path))
                    {
                        sww.WriteLine(msg);
                        sww.WriteLine(Environment.NewLine);
                        sww.Close();
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllLines(path, new[] { e.Message });
            }
        }


        public static void LoggerImport(string message, string datetime, string line)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/LogExcel.txt";
            try
            {
                var msg = "Application Write  : " + message + " Datetime : " + datetime + " Line  : " + line;
                if (File.Exists(path))
                {

                    File.AppendAllLines(path, new[] { msg });
                }
                else
                {
                    using (StreamWriter sww = File.CreateText(path))
                    {
                        sww.WriteLine(msg);
                        sww.WriteLine(Environment.NewLine);
                        sww.Close();
                    }
                }
            }
            catch (Exception e)
            {
                File.AppendAllLines(path, new[] { e.Message });
            }
        }

    }
}
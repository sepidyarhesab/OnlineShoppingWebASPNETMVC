using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
namespace Rozhano_WebApplication2.Extensions
{
    public class PersianDate
    {
        string dat, sal, mah, roz, ret = "";
        public string MiladiToShamsi(DateTime _date)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(pc.GetYear(_date).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(_date).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(_date).ToString("00"));
            return sb.ToString();

        }

        public string shamsitomiladi(string s)
        {
            dat = s;
            sal = dat.Substring(0, 4);
            mah = dat.Substring(5, 2);
            roz = dat.Substring(8, 2);
            PersianCalendar pc = new PersianCalendar();
            ret = pc.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), 0, 0, 0, 0).ToString();
            return ret.ToString();
        }
        public string shamsitomiladi1(string y, string m, string d)
        {
            // dat = s;
            try
            {
                if (y != "" && m != "" && d != "" && y != "0" && m != "0" && d != "0")
                {
                    sal = y;
                    mah = m;
                    roz = d;
                    PersianCalendar pc = new PersianCalendar();
                    ret = pc.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), 0, 0, 0, 0).ToString();
                }
            }
            catch
            {

            }
            return ret.ToString();
        }

    }
}

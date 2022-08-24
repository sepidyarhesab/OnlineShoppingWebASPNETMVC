using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SepidyarHesabExtensions.Classes
{
    public static class SepidyarHesabInternetProtocol
    {
        public static string GetInternetProtocol()
        {
            string IPAdd = string.Empty;
            IPAdd = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IPAdd))
            {
                IPAdd = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return Convert.ToString(IPAdd);
        }
    }
}

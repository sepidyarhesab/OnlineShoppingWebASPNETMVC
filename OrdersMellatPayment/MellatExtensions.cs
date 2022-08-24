using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OrdersMellatPayment
{
    public static class MellatExtensions
    {

        public static void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }


        public static string RefId = "";
        public static string SetDefaultDate()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
        }

        public static string SetDefaultTime()
        {
            return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

    }
}

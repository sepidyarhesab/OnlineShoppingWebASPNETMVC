using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationOrders.Extensions
{
    public static class Secure
    {
        #region Cpu
        public static string GetCpu()
        {
            var cpuInfo = "null";
            try
            {
                var mc = new ManagementClass("win32_processor");
                var moc = mc.GetInstances();

                foreach (var o in moc)
                {
                    var mo = (ManagementObject)o;
                    if (cpuInfo != "") continue;
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            catch (Exception e)
            {

            }
            return cpuInfo;
        }


        #endregion
        #region Hard
        public static string GetHard()
        {
            var volumeSerial = "null";
            try
            {
                var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + "C" + @":""");
                dsk.Get();
                volumeSerial = dsk["VolumeSerialNumber"].ToString();
                return volumeSerial;
            }
            catch (Exception e)
            {

            }

            return volumeSerial;
        }


        #endregion
        #region Name
        public static string GetName()
        {
            var name = "";
            try
            {
                name = Environment.MachineName;
            }
            catch (Exception e)
            {
                name = "null";
            }

            return name;
        }


        #endregion
        #region IP

        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static string GetLocalIp()
        {
            try
            {
                string externalip = new WebClient().DownloadString("http://icanhazip.com");
                if (externalip.Contains("\n"))
                {
                    return externalip.Replace("\n","");
                }
                else
                {
                    return externalip;
                }
            }
            catch (Exception e)
            {
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.Win32;

namespace WebApplicationOrders.Extensions
{
    public static class UsedSession
    {

        public static void UsedAccess()
        {
            try
            {
                //string[] str = new[] {""};
                //string webApplicationOrders = WebConfigurationManager.AppSettings["WebApplicationOrders"];
                //var falsee = Crypto.Encrypt("false");
                //var token = Crypto.Encrypt(Guid.NewGuid().ToString());
                //var cpu = Crypto.Encrypt(Secure.GetCpu());
                //var hdd = Crypto.Encrypt(Secure.GetHard());
                //var name = Crypto.Encrypt(Secure.GetName());
                //var ip = Crypto.Encrypt(Secure.GetLocalIp());
                //var iplocal = Crypto.Encrypt(Secure.GetLocalIpAddress());
                //var datestart = Crypto.Encrypt(DateTime.Now.Ticks.ToString());
                //var dateend = Crypto.Encrypt(DateTime.Now.AddYears(1).Ticks.ToString());
                // str = new[] {"Windows Registry Editor Version 5.00"};
                // str = new[] { @"[HKEY_CURRENT_USER\Software\" + webApplicationOrders + "]" };
                // str = new[] { '"' + "LICENSE" + '"' + "=" + '"' + token + '"' };
                // str = new[] { '"' + "CPU" + '"' + "=" + '"' + cpu + '"' };
                // str = new[] { '"' + "HDD" + '"' + "=" + '"' + hdd + '"' };
                // str = new[] { '"' + "NAME" + '"' + "=" + '"' + name + '"' };
                // str = new[] { '"' + "IP" + '"' + "=" + '"' + ip + '"' };
                // str = new[] { '"' + "HOST" + '"' + "=" + '"' + iplocal + '"' };
                // str = new[] { '"' + "ACCESS" + '"' + "=" + '"' + falsee + '"' };
                // str = new[] { '"' + "START" + '"' + "=" + '"' + datestart + '"' };
                // str = new[] { '"' + "END" + '"' + "=" + '"' + dateend + '"' };

                //RegistryPermission rg =
                //    new RegistryPermission(RegistryPermissionAccess.AllAccess | RegistryPermissionAccess.Write | RegistryPermissionAccess.Read, @"HKEY_CURRENT_USER\Software\");
                //var sn = Registry.GetValue(@"HKEY_CURRENT_USER\Software\" + webApplicationOrders, "ACCESS", "null") ?? "null";
                //if (sn == "null")
                //{
                //    //RegistryKey reg;
                //    //reg = Registry.CurrentUser.OpenSubKey("software", true);
                //    //reg.CreateSubKey(webApplicationOrders);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "LICENSE", token);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "CPU", cpu);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "HDD", hdd);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "NAME", name);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "IP", ip);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "HOST", iplocal);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "ACCESS", falsee);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "START", datestart);
                //    //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Sepidyarhesab", "END", dateend);
                //    //var sms = new SmsProviders();
                //    //sms.SendAdmin(09120448735, Crypto.Decrypt(name), webApplicationOrders, Crypto.Decrypt(hdd), Crypto.Decrypt(ip), Crypto.Decrypt(datestart));
                //}
                //else if (Crypto.Decrypt(sn.ToString()) == "true")
                //{

                //}
                //else
                //{
                //    var sms = new SmsProviders();
                //    sms.SendAdmin(09120448735, name, cpu, hdd, ip, "false");
                //}
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message, "0", "50");
            }


        }



        public static void Reg(string message)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/license";
            try
            {
                var msg = message + Environment.NewLine;
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

        // Session["UserId"] = // UserID
        // Session["BackUrl"]  = // CallBack Url
        // Session["Carts"]  = // Carts
        // Session["ErrorPay"]  = // Error Pay
        // Session["Authentication"]  = // Saved Authentication
    }
}

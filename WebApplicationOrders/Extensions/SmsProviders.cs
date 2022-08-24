using System;
using System.Collections.Generic;
using System.Linq;
using SepidyarHesabExtensions.Classes;
using SmsIrRestful;
using OrdersDatabase.Models;


namespace WebApplicationOrders.Extensions
{
    public  class SmsProviders
    {
        public string token = new SmsToken().Token;
        public  void SendAuthentication(long mobile, string code)
        {
            var db = new Orders_Entities();
            var query =db.Table_Settings.FirstOrDefault(c => c.IsOk);
            if (query != null)
            {
                var ultraFastSend = new UltraFastSend
                {
                    Mobile = mobile,
                    TemplateId = 25020,
                    ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "CompanyName", ParameterValue = query.PrimaryTitle }, new UltraFastParameters { Parameter = "AuthenticationCode", ParameterValue = code }, }).ToArray()
                };
                var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
                if (ultraFastSendRespone.IsSuccessful)
                {
                    LogWriter.Logger("Success","SMS","Success");
                }
                else
                {
                    var ultraFastSend2 = new UltraFastSend
                    {
                        Mobile = mobile,
                        TemplateId = 25020,
                        ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "CompanyName", ParameterValue = query.PrimaryTitle }, new UltraFastParameters { Parameter = "AuthenticationCode", ParameterValue = code }, }).ToArray()
                    };
                    var ultraFastSendRespone2 = new UltraFast().Send(token, ultraFastSend2);
                    if (ultraFastSendRespone2.IsSuccessful)
                    {
                        LogWriter.Logger("Success", "SMS", "Success");
                    }
                    else
                    {
                        LogWriter.Logger("Error : " + ultraFastSendRespone.VerificationCodeId, "Error : " + ultraFastSendRespone.Message, "Error");

                    }
                }
            }
        }
        public  void SendAuthenticationInformation(long mobile, string username)
        {
            var db = new Orders_Entities();
            var query =db.Table_Settings.FirstOrDefault(c => c.IsOk);
            if (query != null)
            {
                var ultraFastSend = new UltraFastSend
                {
                    Mobile = mobile,
                    TemplateId = 25021,
                    ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "CompanyName", ParameterValue = query.TertiaryTitle }, new UltraFastParameters { Parameter = "Username", ParameterValue = username }, new UltraFastParameters { Parameter = "Password", ParameterValue = "1234" } }).ToArray()
                };
                var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
                if (ultraFastSendRespone.IsSuccessful)
                {

                }
                else
                {

                }
            }
        }       
        public  void SendSmsStatus(long mobile, string status)
        {
            var db = new Orders_Entities();
            var query =db.Table_Settings.FirstOrDefault(c => c.IsOk);
            if (query != null)
            {
                var ultraFastSend = new UltraFastSend
                {
                    Mobile = mobile,
                    TemplateId = 57497,
                    ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "CompanyName", ParameterValue = query.TertiaryTitle }, new UltraFastParameters { Parameter = "Status", ParameterValue = status }}).ToArray()
                };
                var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
                if (ultraFastSendRespone.IsSuccessful)
                {

                }
                else
                {

                }
            }
        }
        public  void SendSmsPostalCode(long mobile, string postalcode)
        {
            var db = new Orders_Entities();
            var query =db.Table_Settings.FirstOrDefault(c => c.IsOk);
            if (query != null)
            {
                var stringphone = "0"+mobile.ToString();
                var user = db.Table_User.FirstOrDefault(c => c.Phone == stringphone);
                if (user != null)
                {
                    var ultraFastSend = new UltraFastSend
                    {
                        Mobile = mobile,
                        TemplateId = 62575,
                        ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "CompanyName", ParameterValue = query.TertiaryTitle }, new UltraFastParameters { Parameter = "Fullname", ParameterValue = user.Name + " " + user.Family }, new UltraFastParameters { Parameter = "PostalCode", ParameterValue = postalcode } }).ToArray()
                    };
                    var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
                    if (ultraFastSendRespone.IsSuccessful)
                    {

                    }
                    else
                    {

                    }

                }

            }
        }
        public  void SendGenerateQuotations(long mobile, string code)
        {
            var db = new Orders_Entities();
            var phone = "0" + mobile.ToString();
            var query =db.Table_Settings.FirstOrDefault(c => c.IsOk);
            if (query != null)
            {
                var user =db.Table_User.FirstOrDefault(c => c.Phone == phone);
                if (user != null)
                {
                    var ultraFastSend = new UltraFastSend
                    {
                        Mobile = mobile,
                        TemplateId = 25022,
                        ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "CompanyName", ParameterValue = query.PrimaryTitle }, new UltraFastParameters { Parameter = "CustomerFullname", ParameterValue = user.Name + " " + user.Family }, new UltraFastParameters { Parameter = "PreInvoiceCode", ParameterValue = code } }).ToArray()
                    };
                    var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
                    if (ultraFastSendRespone.IsSuccessful)
                    {

                    }
                    else
                    {

                    }

                }
                else
                {
                    var ultraFastSend = new UltraFastSend
                    {
                        Mobile = mobile,
                        TemplateId = 25022,
                        ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "CompanyName", ParameterValue = query.PrimaryTitle }, new UltraFastParameters { Parameter = "CustomerFullname", ParameterValue = "مشتری جدید" }, new UltraFastParameters { Parameter = "PreInvoiceCode", ParameterValue = code } }).ToArray()
                    };
                    var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
                    if (ultraFastSendRespone.IsSuccessful)
                    {

                    }
                    else
                    {

                    }

                }
            }
        }
        public  string SendGenerateQuotationsAdmin(long phone, string admin, string fullname, string companyName)
        {
            var message = "";
            var ultraFastSend = new UltraFastSend
            {
                Mobile = long.Parse(admin),
                TemplateId = 56703,
                ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "Fullname", ParameterValue = fullname }, new UltraFastParameters { Parameter = "Phone", ParameterValue = phone.ToString() }, new UltraFastParameters { Parameter = "CompanyName", ParameterValue = companyName } }).ToArray()
            };
            var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
            if (ultraFastSendRespone.IsSuccessful)
            {
                message = "Success";
            }
            else
            {
                message = ultraFastSendRespone.Message;
            }

            return message;
        }

        public  string SendPayment(long phone, string fullname, string companyName, string code)
        {
            var message = "";
            var ultraFastSend = new UltraFastSend
            {
                Mobile = phone,
                TemplateId = 62657,
                ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "Fullname", ParameterValue = fullname }, new UltraFastParameters { Parameter = "Code", ParameterValue = code }, new UltraFastParameters { Parameter = "CompanyName", ParameterValue = companyName } }).ToArray()
            };
            var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
            if (ultraFastSendRespone.IsSuccessful)
            {
                message = "Success";
            }
            else
            {
                message = ultraFastSendRespone.Message;
            }

            return message;
        }
        public  void SendAdminNewsLetter(long phone, string admin, string companyname)
        {
            var ultraFastSend = new UltraFastSend
            {
                Mobile = long.Parse(admin),
                TemplateId = 56704,
                ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "Phone", ParameterValue = phone.ToString() }, new UltraFastParameters { Parameter = "CompanyName", ParameterValue = companyname } }).ToArray()
            };
            var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {
            }

        }   
        public  void SendAdmin(long phone, string computer , string cpu , string hdd , string ip , string start)
        {
            var ultraFastSend = new UltraFastSend
            {
                Mobile = phone,
                TemplateId = 62382,
                ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "ComputerName", ParameterValue =computer }, new UltraFastParameters { Parameter = "ComputerHDD", ParameterValue = hdd }, new UltraFastParameters { Parameter = "ComputerCPU", ParameterValue = cpu }, new UltraFastParameters { Parameter = "InternetProtocol", ParameterValue = ip }, new UltraFastParameters { Parameter = "Date", ParameterValue = start } }).ToArray()
            };
            var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {
            }

        }

        //public static void SendEmergency(string message , string type)
        //{
        //    try
        //    {
        //        //26230
        //        //36907
        //        var query =db.Table_Settings.Where(c => c.IsOk)
        //            .OrderBy(c => c.Sort).ToList();
        //        if (query.Count > 0)
        //        {
        //            foreach (var receiver in query)
        //            {
        //                var ultraFastSend = new UltraFastSend
        //                {
        //                    Mobile = long.Parse(receiver.Phone),
        //                    TemplateId = 26230,
        //                    ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "AdminFullname", ParameterValue = receiver.Name +" " + receiver.Family }, new UltraFastParameters { Parameter = "Type", ParameterValue = type }, new UltraFastParameters { Parameter = "Code", ParameterValue = message } }).ToArray()
        //                };
        //                var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
        //                if (ultraFastSendRespone.IsSuccessful)
        //                {

        //                }
        //                else
        //                {

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //public static void SendResavation(string fullname   , string phone , string code)
        //{
        //    try
        //    {
        //        bool ok = false;
        //        for (int i = 0; i < 100; i++)
        //        {
        //            if (ok)
        //            {

        //            }
        //            else
        //            {
        //                var ultraFastSend = new UltraFastSend
        //                {
        //                    Mobile = long.Parse(phone),
        //                    TemplateId = 36907,
        //                    ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "Fullname", ParameterValue = fullname }, new UltraFastParameters { Parameter = "Code", ParameterValue = code } }).ToArray()
        //                };
        //                var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
        //                if (ultraFastSendRespone.IsSuccessful)
        //                {
        //                    ok = true;
        //                }
        //                else
        //                {

        //                }
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //public static void SendBookingOnline(string fullname, string phone, string time , string date , string compnay)
        //{
        //    try
        //    {
        //        var ultraFastSend = new UltraFastSend
        //        {
        //            Mobile = long.Parse(phone),
        //            TemplateId = 33871,
        //            ParameterArray = new List<UltraFastParameters>(new[] { new UltraFastParameters { Parameter = "Fullname", ParameterValue = fullname }, new UltraFastParameters { Parameter = "Date", ParameterValue = date }, new UltraFastParameters { Parameter = "Time", ParameterValue = time }, new UltraFastParameters { Parameter = "CompanyName", ParameterValue = compnay } }).ToArray()
        //        };
        //        var ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
        //        if (ultraFastSendRespone.IsSuccessful)
        //        {

        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
    }
}
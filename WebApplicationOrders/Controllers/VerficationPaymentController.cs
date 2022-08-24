using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using SepidyarHesabExtensions.Classes;
using WebApplicationOrders.Database;
using WebApplicationOrders.Extensions;
using WebApplicationOrders.Models.Database.OrderDatabase;

namespace WebApplicationOrders.Controllers
{
    public class VerficationPaymentController : Controller
    {
        // GET: VerficationPayment



        public ActionResult Index(string Authority, string Status)
        {
            String _status = Status;

            if (_status != "OK")
            {
                LogWriter.Logger("Payment Not Verified => Because : "+ "Status = NOOK",DateTime.Now.ToString(),"23");
                Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با خطا مواجه شد!";
                return RedirectToAction("Finished");
            }

            if (Request.Cookies["Payment"] != null)
            {
                string Amount = Request.Cookies["Payment"].Values["Amount"];
                string UserPhone = Request.Cookies["Payment"].Values["UserPhone"];
                string Code = Request.Cookies["Payment"].Values["Code"];

                LogWriter.Logger("My Amount : Cookie A :" + Amount, DateTime.Now.ToString(), "42");
                LogWriter.Logger("My Amount : Cookie U :" + UserPhone, DateTime.Now.ToString(), "42");
                LogWriter.Logger("My Amount : Cookie C :" + Code, DateTime.Now.ToString(), "42");
            }


            var zarinpal = ZarinPal.ZarinPal.Get();
            String _authority = Authority;
            string MerchantID = WebConfigurationManager.AppSettings["MerchantID"];
            long _amount = 11111;
            if (Session["Amount"] != null)
            {
                var amount = long.Parse(Session["Amount"].ToString());
                _amount = amount;
            }
            else
            {
                _amount = 01010101;
                LogWriter.Logger("My Amount : 01010101", DateTime.Now.ToString(), "42");
                Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با خطا مواجه شد!";
                return RedirectToAction("Finished");
            }
            


            var verificationRequest = new ZarinPal.PaymentVerification(MerchantID, _amount, _authority);
            var verificationResponse = zarinpal.InvokePaymentVerification(verificationRequest);
            if (verificationResponse.Status == 100)
            {
                LogWriter.Orders(
                    "Payment Order  Success= " + _authority, DateTime.Now.ToString(), "54");
                if (Session["OrderCode"] != null)
                {
                    Session["Carts"] = null;
                    LogWriter.Orders(
                        "Payment Order  Success = " + Session["OrderCode"].ToString(), DateTime.Now.ToString(), "54");
                    var db = new Orders_Entities();
                    var code = Session["OrderCode"].ToString();
                    var queryUpdate = db.Table_Order.FirstOrDefault(c => c.Code == code);
                    if (queryUpdate != null)
                    {
                        db.SP_UpdateOrder(queryUpdate.Id, queryUpdate.Code, true, true);
                        var queryUsers = db.Table_User.FirstOrDefault(c => c.Id == queryUpdate.CreatorRef);
                        if (queryUsers != null)
                        {
                            string adminPhone = WebConfigurationManager.AppSettings["PhoneAdmin"];
                            string CompanyName = WebConfigurationManager.AppSettings["CompanyName"]; var sms = new SmsProviders();
                            var a = sms.SendGenerateQuotationsAdmin(long.Parse(queryUsers.Phone), adminPhone, queryUsers.Name + " " + queryUsers.Family, CompanyName);
                            if (a == "Success")
                            {
                                Response.Write("<script>alert(" + "Send" + ")</script>");
                            }
                            else
                            {
                                Response.Write("<script>alert(" + a + ")</script>");
                            }

                            db.SP_InsertSMS(SepidyarHesabCodeGenerator.GenerateCode("Orders", "Table_Order"),
                                "پرداخت کاربر : " + queryUsers.Name + " " + queryUsers.Family + " " + "به مبلغ : " +
                                _amount + " با شماره تماس : " + queryUsers.Phone,queryUsers.Id);
                            sms.SendPayment(long.Parse(queryUsers.Phone), queryUsers.Name + " " + queryUsers.Family,
                                CompanyName, verificationResponse.RefID);

                        }

                        //queryUpdate.IsOk = true;
                        //queryUpdate.IsPay = true;
                        //db.SaveChanges();
                        LogWriter.Logger("Order Success = " + queryUpdate.Phone, DateTime.Now.ToString(), "82");
                        Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با موفقیت انجام شد!";
                        Session["PaymentCode"] = verificationResponse.RefID;
                        Session["Carts"] = null;
                        return RedirectToAction("Finished");
                    }
                    else
                    {
                        Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با خطا مواجه شد!";
                        LogWriter.Logger("Table_Order == null", DateTime.Now.ToString(), "91");
                        Session["PaymentCode"] = verificationResponse.RefID;
                        return RedirectToAction("Finished");
                    }
             
                }
                else
                {
                    Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با خطا مواجه شد!";
                    Session["PaymentCode"] = verificationResponse.RefID;
                    LogWriter.Logger("Session[OrderCode] == null", DateTime.Now.ToString(), "100");
                    return RedirectToAction("Finished");
                }
            }
            else
            {
                LogWriter.Logger("verificationResponse != 100", DateTime.Now.ToString(), "105");
                Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با خطا مواجه شد!";
                Session["PaymentCode"] = verificationResponse.RefID;
                return RedirectToAction("Finished");
            }

        }

        public ActionResult Finished()
        {
            return View();
        }
    }
}
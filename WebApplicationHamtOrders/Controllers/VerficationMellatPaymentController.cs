using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using log4net;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersMellatPayment;
using OrdersMellatPayment.PaymentGetwayMellat;
using SepidyarHesabExtensions.Extentions;

namespace WebApplicationHamtOrders.Controllers
{
    public class VerficationMellatPaymentController : Controller
    {
        private readonly static ILog logger = LogManager.GetLogger(typeof(VerficationMellatPaymentController));
        // GET: VerficationMellatPayment
        public static readonly string PgwSite = ConfigurationManager.AppSettings["PgwSite"];
        public static readonly string CallBackUrl = ConfigurationManager.AppSettings["CallBackUrl"];
        public static readonly string TerminalId = ConfigurationManager.AppSettings["TerminalId"];
        public static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        public static readonly string UserPassword = ConfigurationManager.AppSettings["UserPassword"];
        public ActionResult Index()
        {
            //string a = Request.Params["RefId"];
            //string ba = Request.Params["ResCode"];
            //string d = Request.Params["SaleOrderId"];
            //string c = Request.Params["SaleReferenceId"];
            var refId = Request.Params["RefId"];
            var resCode = Request.Params["ResCode"];
            var saleOrderId = Request.Params["SaleOrderId"];
            var saleReferenceId = Request.Params["SaleReferenceId"];


            if (resCode == "0")
            {
                string result = "";
                MellatExtensions.BypassCertificateError();
                PaymentGatewayImplService bp = new PaymentGatewayImplService();
                result = bp.bpVerifyRequest(long.Parse(TerminalId), UserName, UserPassword, long.Parse(saleOrderId),
                    long.Parse(saleOrderId), long.Parse(saleReferenceId));
                if (result == "0")
                {
                    LogWriter.Orders(
"Payment Order  Success = " + saleOrderId.ToString(), DateTime.Now.ToString(), "54");
                    var db = new Orders_Entities();
                    var code = saleOrderId;
                    var queryUpdate = db.Table_Order.FirstOrDefault(c => c.TransactionCode == code);
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
                                queryUpdate.Price + " با شماره تماس : " + queryUsers.Phone, queryUsers.Id);
                            sms.SendPayment(long.Parse(queryUsers.Phone), queryUsers.Name + " " + queryUsers.Family,
                                CompanyName, code);

                        }

                        //queryUpdate.IsOk = true;
                        //queryUpdate.IsPay = true;
                        //db.SaveChanges();
                        LogWriter.Logger("Order Success = " + queryUpdate.Phone, DateTime.Now.ToString(), "82");
                        Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با موفقیت انجام شد!";
                        Session["OrderCode"] = saleOrderId;
                        //Session["PaymentCode"] = verificationResponse.RefID;
                        Session["Carts"] = null;
                        return RedirectToAction("Finished");
                    }
                    else
                    {
                        Session["JavaScriptFunctionPayment"] = "عملیات پرداخت شما با خطا مواجه شد!";
                        LogWriter.Logger("Table_Order == null - " + code, DateTime.Now.ToString(), "91");
                        return RedirectToAction("Finished");
                    }

                }
                else
                {
                    LogWriter.Logger("error " + result + "Ref = " + refId + "- ResCode = " + resCode + "-SaleOrderCode = " + saleOrderId + " - SaleRefrecnce = " + saleReferenceId, DateTime.Now.ToString(), "98");
                    TempData["JavaScriptFunction"] = IziToast.Error("عملیات خطا دارد", "error " + result + "Ref = " + refId + "- ResCode = " + resCode + "-SaleOrderCode = " + saleOrderId + " - SaleRefrecnce = " + saleReferenceId);
                }
            }
            else
            {
                LogWriter.Logger("error " + resCode + "Ref = " + refId + "- ResCode = " + resCode + "-SaleOrderCode = " + saleOrderId + " - SaleRefrecnce = " + saleReferenceId + resCode, DateTime.Now.ToString(), "104");
            }
            return RedirectToAction("Finished");
        }

        public ViewResult Finished()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersOrders.ViewModels.Orders;

namespace WebApplicationHamtOrders.Controllers
{
    public class VerficationRefahPaymentController : Controller
    {

        bool isError = false;
        string errorMsg = "";
        string succeedMsg = "";
        // GET: VerficationRefahPayment
        public ActionResult Index()
        {
            var vm = new VMSamanPaymentResult();
            if (RequestFeildIsEmpty())
            {
                var transactionState = Request.Form["state"].ToString();
                var traceNumber = Request.Form["TraceNo"].ToString();
                var reservationNumber = Request.Form["ResNum"].ToString();
                var refrenceNumber = Request.Form["RefNum"].ToString();
                LogWriter.Logger("state", transactionState, "");
                LogWriter.Logger("TraceNo", traceNumber, "");
                LogWriter.Logger("ResNum", reservationNumber, "");
                LogWriter.Logger("RefNum", refrenceNumber, "");
                if (transactionState.Equals("OK"))
                {
                    LogWriter.Logger("state", "OK", "");
                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    var MID = ConfigurationManager.AppSettings["MID"];
                    ///WebService Instance
                    var srv = new Sep.RefrencePayment.PaymentIFBinding();
                    var result = srv.verifyTransaction(refrenceNumber, MID);

                    if (result > 0)
                    {
                        LogWriter.Logger("state", "0", "");
                        var db = new Orders_Entities();
                        var query = db.Table_Order.FirstOrDefault(c => c.TransactionCode == reservationNumber);
                        if (query != null)
                        {
                            LogWriter.Logger("query", reservationNumber, "");
                            if (result == double.Parse((query.NetPrice ?? 0).ToString())) // چک کردن مبلغ بازگشتی از سرویس با مبلغ تراکنش
                            {
                                LogWriter.Logger("NET PRICE", "SUCCESS", "");
                                db.SP_UpdateOrder(query.Id, query.Code, true, true);
                                isError = false;
                                vm.Class = "Success";
                                vm.Messages = "Success";
                                vm.IsOk = true;
                                vm.RefNumber = refrenceNumber;
                                vm.ResNumber = reservationNumber;
                                vm.UserId = Request.Form["AdditionalData1"].ToString() ?? "";
                                vm.OrderId = Request.Form["AdditionalData2"].ToString() ?? "";
                                LogWriter.Logger("DATA2", Request.Form["AdditionalData2"].ToString(), "");
                                LogWriter.Logger("DATA1", Request.Form["AdditionalData1"].ToString(), "");
                            }
                            else
                            {
                                vm.Messages = "NET PRICE" + "NOOO";
                                vm.IsOk = false;
                                LogWriter.Logger("NET PRICE", "NOOO", "");
                                string userName = Request.Form["MID"];//نام کاربری همان ام آی دی است
                                string pass = ""; // رمز عبور برای شما ایمیل شده است
                                srv.reverseTransaction(Request.Form["RefNum"], Request.Form["MID"], userName, pass);
                            }
                        }
                        else
                        {
                            vm.Messages = "query" + "null";
                            vm.IsOk = false;
                            LogWriter.Logger("query", "null", "");
                        }
                    }
                    else
                    {
                        LogWriter.Logger("TransactionChecking", TransactionChecking((int)result), "");
                        vm.Messages = TransactionChecking((int)result);
                        vm.IsOk = false;
                        vm.Class = "Error";
                        vm.RefNumber = result.ToString();
                    }
                }
                else
                {
                    vm.IsOk = false;
                    vm.Class = "Error";
                    isError = true;
                    errorMsg = "متاسفانه بانک خريد شما را تاييد نکرده است";

                    if (transactionState.Equals("Canceled By User") || transactionState.Equals(string.Empty))
                    {
                        // Transaction was canceled by user
                        isError = true;
                        errorMsg = "تراكنش توسط خريدار كنسل شد";
                    }
                    else if (transactionState.Equals("Invalid Amount"))
                    {
                        // Amount of revers teransaction is more than teransaction
                    }
                    else if (transactionState.Equals("Invalid Transaction"))
                    {
                        // Can not find teransaction
                    }
                    else if (transactionState.Equals("Invalid Card Number"))
                    {
                        // Card number is wrong
                    }
                    else if (transactionState.Equals("No Such Issuer"))
                    {
                        // Issuer can not find
                    }
                    else if (transactionState.Equals("Expired Card Pick Up"))
                    {
                        // The card is expired
                    }
                    else if (transactionState.Equals("Allowable PIN Tries Exceeded Pick Up"))
                    {
                        // For third time user enter a wrong PIN so card become invalid
                    }
                    else if (transactionState.Equals("Incorrect PIN"))
                    {
                        // Pin card is wrong
                    }
                    else if (transactionState.Equals("Exceeds Withdrawal Amount Limit"))
                    {
                        // Exceeds withdrawal from amount limit
                    }
                    else if (transactionState.Equals("Transaction Cannot Be Completed"))
                    {
                        // PIN and PAD are currect but Transaction Cannot Be Completed
                    }
                    else if (transactionState.Equals("Response Received Too Late"))
                    {
                        // Timeout occur
                    }
                    else if (transactionState.Equals("Suspected Fraud Pick Up"))
                    {
                        // User did not insert cvv2 & expiredate or they are wrong.
                    }
                    else if (transactionState.Equals("No Sufficient Funds"))
                    {
                        // there are not suficient funds in the account
                    }
                    else if (transactionState.Equals("Issuer Down Slm"))
                    {
                        // The bank server is down
                    }
                    else if (transactionState.Equals("TME Error"))
                    {
                        // unknown error occur
                    }
                    LogWriter.Logger("errorMsg 165", errorMsg, "");
                    vm.Messages = errorMsg;
                }
            }
            else
            {
                LogWriter.Logger("query", "RequestFeildIsEmpty", "");
                vm.Messages = errorMsg;
                vm.IsOk = false;
            }
            return View(vm);
        }

        private bool RequestFeildIsEmpty()
        {
            if (Request.Form["state"].ToString().Equals(string.Empty))
            {
                isError = true;
                errorMsg = "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است";
                LogWriter.Logger(errorMsg,"state null","");
                return true;
            }

            if (Request.Form["RefNum"].ToString().Equals(string.Empty) && Request.Form["state"].ToString().Equals(string.Empty))
            {
                isError = true;
                errorMsg = "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت";
                LogWriter.Logger(errorMsg, "Request.Form[RefNum] null", "");
                return true;
            }

            if (Request.Form["ResNum"].ToString().Equals(string.Empty) && Request.Form["state"].ToString().Equals(string.Empty))
            {
                isError = true;
                LogWriter.Logger(errorMsg, "Request.Form[state] null", "");
                errorMsg = "خطا در برقرار ارتباط با بانک";
                return true;
            }
            return false;
        }

        private string TransactionChecking(int i)
        {
            bool isError = false;
            string errorMsg = "";
            switch (i)
            {

                case -1:		//TP_ERROR
                    isError = true;
                    errorMsg = "بروز خطا درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-1";
                    break;
                case -2:		//ACCOUNTS_DONT_MATCH
                    isError = true;
                    errorMsg = "بروز خطا در هنگام تاييد رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-2";
                    break;
                case -3:		//BAD_INPUT
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-3";
                    break;
                case -4:		//INVALID_PASSWORD_OR_ACCOUNT
                    isError = true;
                    errorMsg = "خطاي دروني سيستم درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-4";
                    break;
                case -5:		//DATABASE_EXCEPTION
                    isError = true;
                    errorMsg = "خطاي دروني سيستم درهنگام بررسي صحت رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-5";
                    break;
                case -7:		//ERROR_STR_NULL
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-7";
                    break;
                case -8:		//ERROR_STR_TOO_LONG
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-8";
                    break;
                case -9:		//ERROR_STR_NOT_AL_NUM
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-9";
                    break;
                case -10:	//ERROR_STR_NOT_BASE64
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-10";
                    break;
                case -11:	//ERROR_STR_TOO_SHORT
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-11";
                    break;
                case -12:		//ERROR_STR_NULL
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-12";
                    break;
                case -13:		//ERROR IN AMOUNT OF REVERS TRANSACTION AMOUNT
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-13";
                    break;
                case -14:	//INVALID TRANSACTION
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-14";
                    break;
                case -15:	//RETERNED AMOUNT IS WRONG
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-15";
                    break;
                case -16:	//INTERNAL ERROR
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-16";
                    break;
                case -17:	// REVERS TRANSACTIN FROM OTHER BANK
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-17";
                    break;
                case -18:	//INVALID IP
                    isError = true;
                    errorMsg = "خطا در پردازش رسيد ديجيتالي در نتيجه خريد شما تاييد نگرييد" + "-18";
                    break;

            }
            return errorMsg;
        }
    }
}
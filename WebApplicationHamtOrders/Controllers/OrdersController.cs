using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using OrdersExtentions.Extensions;
using OrdersMellatPayment;
using OrdersMellatPayment.PaymentGetwayMellat;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using OrdersSettings.Repository.Settings;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;



namespace WebApplicationHamtOrders.Controllers
{
    public class OrdersController : Controller
    {

        /// <summary>
        /// Version Amin-2022-08-04
        /// </summary>
        // GET: Orders
        public static readonly string PgwSite = ConfigurationManager.AppSettings["PgwSite"];
        public static readonly string CallBackUrl = ConfigurationManager.AppSettings["CallBackUrl"];
        public static readonly string TerminalId = ConfigurationManager.AppSettings["TerminalId"];
        public static readonly string UserName = ConfigurationManager.AppSettings["UserName"];
        public static readonly string UserPassword = ConfigurationManager.AppSettings["UserPassword"];

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                Session["UrlBack"] = "/Orders";
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult Index(string transferpay)
        {
            Session["TransferPay"] = transferpay;
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                Session["UrlBack"] = "/Orders";
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult FinalSum(string sum)
        {
            Session["SumPayment"] = sum;
            return RedirectToAction("Index", "Carts");
        }

        [HttpPost]
        public ActionResult SubmitOrder(VMOrders.VmOrderSubmit values, HttpPostedFileBase file, string Browser, string IPAddress, string AddressRef)
        {
            try
            {
                values.Address += AddressRef;
                if (ModelState.IsValid)
                {
                    if (values.Name != "")
                    {
                        if (values.Family != "")
                        {
                            if (values.Phone != "")
                            {
                                if (values.Address != "")
                                {
                                    var carts = new List<VMOrders.VmOrderSubmit>();
                                    if (Session["Carts"] != null)
                                    {
                                        #region Generate Invoice
                                        var persian = values.Phone.PersianToEnglish();
                                        carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                                        //var carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                                        var codedis = "";
                                        if (Session["CodeDis"] != null)
                                        {
                                            codedis = Session["CodeDis"].ToString();
                                        }
                                        var resuult = RepOrders.RepositoryCartsCode(carts, codedis);
                                        decimal transferpay = 0;
                                        if (Session["TransferPay"] != null)
                                        {
                                            transferpay = decimal.Parse(Session["TransferPay"].ToString());
                                        }
                                        var result = RepOrders.RepositorySubmitOrders(values, resuult.CartsItems, file, persian, transferpay, Browser, IPAddress, resuult.Discount);
                                        if (result.Contains("Error"))
                                        {
                                            Session["OrderCode"] = "Error";
                                            Session["JavaScriptFunction"] = "Error";
                                            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                                            Response.Redirect("/Orders/OrderFinished");
                                        }
                                        else
                                        {
                                            Session["JavaScriptFunction"] = "Success";
                                            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
                                            var SplitResult = result.Split('&');
                                            var SplitCode = SplitResult[0];
                                            Session["OrderCode"] = SplitCode;
                                            var SplitTransaction = SplitResult[1];


                                            long Amount = 0;
                                            if (Session["PayAmount"] != null)
                                            {
                                                Amount = long.Parse(Session["PayAmount"].ToString());
                                            }
                                            //decimal fee = 0;
                                            //if (carts != null)
                                            //{
                                            //    foreach (var cart in carts)
                                            //    {


                                            //        if (Session["SumPayment"] != null)
                                            //        {
                                            //            var SumWithDiscount = int.Parse(Session["SumPayment"].ToString());
                                            //            fee = SumWithDiscount;
                                            //            /*  if (cart.Discount > 0)
                                            //              {
                                            //                  var sum = ((cart.Fee) * cart.Quantity);
                                            //                  fee += sum;
                                            //              }
                                            //              else
                                            //              {
                                            //                  var sum = (cart.Fee * cart.Quantity);
                                            //                  var discount = (sum * cart.Discount) / 100;
                                            //                  fee += sum - discount;
                                            //              }*/
                                            //        }
                                            //        else
                                            //        {
                                            //            var sum = ((cart.Fee) * cart.Quantity);
                                            //            fee += sum;
                                            //        }
                                            //    }

                                            //}



                                            //var resultPay = Repfooter.RepInformationFooter();
                                            //if (resultPay.Count > 0)
                                            //{
                                            //    foreach (var item in resultPay)
                                            //    {
                                            //        fee += decimal.Parse(item.TarnsferPay);
                                            //        Amount = long.Parse(fee.ToString());
                                            //    }
                                            //}
                                            //if (transferpay <= 0)
                                            //{
                                            //    var resultPay = Repfooter.RepInformationFooter();
                                            //    if (resultPay.Count > 0)
                                            //    {
                                            //        foreach (var item in resultPay)
                                            //        {
                                            //            transferpay = decimal.Parse(item.TarnsferPay ?? "0");
                                            //        }
                                            //    }

                                            //    Amount = Amount - long.Parse(transferpay.ToString());
                                            //}

                                            string adminPhone = WebConfigurationManager.AppSettings["PhoneAdmin"];

                                            string CompanyName = WebConfigurationManager.AppSettings["CompanyName"];

                                            var sms = new SmsProviders();
                                            var a = sms.SendGenerateQuotationsAdmin(long.Parse(values.Phone), adminPhone, values.Name + " " + values.Family, CompanyName);
                                            if (a == "Success")
                                            {
                                                Response.Write("<script>alert(" + "Send" + ")</script>");
                                            }
                                            else
                                            {
                                                Response.Write("<script>alert(" + a + ")</script>");
                                            }
                                            string configPayment = WebConfigurationManager.AppSettings["PaymentMethod"];
                                            if (configPayment == "Zarinpal")
                                            {
                                                Response.Redirect(ZarinPalStart(values.Name, values.Family, values.Phone, SplitCode, Amount));
                                            }

                                            if (configPayment == "Mellat")
                                            {
                                                MellatStart(Amount + long.Parse((transferpay).ToString()), SplitTransaction, values.Phone.PersianToEnglish());
                                                return RedirectToAction("ConnectingToMellat");
                                            }
                                            //Response.Redirect("/Orders/OrderFinished");
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                                        Response.Redirect("/Product");
                                    }

                                }
                                else
                                {
                                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                                    Response.Redirect("/Product");
                                }

                            }
                            else
                            {
                                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                                Response.Redirect("/Product");
                            }

                        }
                        else
                        {
                            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                            Response.Redirect("/Product");
                        }

                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                        Response.Redirect("/Product");
                    }


                }
            }
            catch (Exception e)
            {

            }


            return View("OrderFinished");
        }


        public string ZarinPalStart(string name, string family, string phone, string code, long amount)
        {
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
            string MerchantID = WebConfigurationManager.AppSettings["MerchantID"];
            string CallbackURL = WebConfigurationManager.AppSettings["CallbackURL"];

            String Description = name + " " + family + "_" + phone + "_" + code + "_" + amount;

            HttpCookie cAmount = new HttpCookie("Amount", amount.ToString());

            HttpCookie cUserPhone = new HttpCookie("UserPhone", phone);

            HttpCookie cCode = new HttpCookie("Code", code);

            HttpCookie cook = new HttpCookie("Payment");

            cook.Values.Add("Amount", amount.ToString());

            cook.Values.Add("UserPhone", phone);

            cook.Values.Add("Code", code);

            Response.Cookies.Add(cook);

            cAmount.Expires = DateTime.Now.AddHours(2);

            cUserPhone.Expires = DateTime.Now.AddHours(2);

            cCode.Expires = DateTime.Now.AddHours(2);
            Session["Amount"] = amount;
            Session["UserPhone"] = phone;
            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantID, amount, CallbackURL, Description);
            LogWriter.Logger("New Order = " + pr, DateTime.Now.ToString(), "162");
            var res = zarinpal.InvokePaymentRequest(pr);
            LogWriter.Logger("New Order = " + res, DateTime.Now.ToString(), "164");


            if (res.Status == 100)
            {
                LogWriter.Logger("New Order = " + res.Status, DateTime.Now.ToString(), "184");
                return res.PaymentURL;
            }
            else
            {
                LogWriter.Logger("New Order  res= " + res.Status, DateTime.Now.ToString(), "184");
                return "/Carts";
            }
        }


        public void MellatStart(long amount, string payid, string phone)
        {
            var transcations = Guid.NewGuid().ToString();
            var result = "";
            var phonenumber = "98" + phone;
            MellatExtensions.BypassCertificateError();
            var order = long.Parse(payid);
            var addision = MellatExtensions.SetDefaultTime();
            var addision2 = MellatExtensions.SetDefaultDate();
            var bp = new PaymentGatewayImplService();
            result = bp.bpPayRequest(Int64.Parse(TerminalId), UserName, UserPassword, order, amount * 10, addision2, addision, transcations, CallBackUrl, phone, phonenumber, "", "", "", "");
            if (result.Contains(','))
            {
                LogWriter.Logger(result, DateTime.Now.ToLongDateString(), "1");
                string[] res = result.Split(',');
                if (res[0] == "0")
                {
                    Session["CallMelat"] = res[1] + "&" + phonenumber;
                    //return RedirectToAction("ConnectingToMellat", res[1]);
                }
                else
                {
                    Session["CallMelat"] = res[1] + "&" + phonenumber;
                }
            }
            else
            {
                LogWriter.Logger(result, DateTime.Now.ToLongDateString(), "2");
                //ViewBag.PaymentMessage = "error " + result;
                if (result == "0")
                {
                    Session["CallMelat"] = result + "&" + phonenumber;
                    //return RedirectToAction("ConnectingToMellat", result);
                }
                else
                {
                    Session["CallMelat"] = result + "&" + phonenumber;
                    //return RedirectToAction("ConnectingToMellat", result);
                }
            }
        }
        #region Mellat
        public ActionResult ConnectingToMellat()
        {
            return View();
        }
        #endregion
        public ActionResult OrderFinished()
        {
            return View();
        }

        [HttpPost]
        public decimal AddToCarts(string id, string name, decimal fee, string quantity, Guid guid, Guid? CategoriesRef)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();
            var idd = Guid.Parse(id);
            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;

            }


            if ((carts ?? throw new InvalidOperationException()).Any(p => p.ProductId == idd))
            {
                int index = carts.FindIndex(p => p.ProductId == idd);
                carts[index].Quantity += int.Parse(quantity);
                carts[index].Fee += fee;
            }
            else
            {
                carts.Add(new VMOrders.VmOrderSubmit
                {


                    Code = name,
                    Quantity = int.Parse(quantity),
                    Name = name,
                    Fee = fee,
                    CategoryRef = CategoriesRef,
                    ProductId = idd,

                });
            }
            Session["Carts"] = carts;
            return carts.Sum(p => p.Quantity);
        }

        [HttpPost]
        public decimal AddToCartsWithSize(string code, string name, decimal fee, string quantity, string color, string size, Guid guid)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;

            }

            var guidColor = Guid.Parse(color);
            var guidSize = Guid.Parse(size);
            if ((carts ?? throw new InvalidOperationException()).Any(p => p.ProductId == guid && p.ColorRef == guidColor && p.SizeRef == guidSize))
            {
                int index = carts.FindIndex(p => p.ProductId == guid && p.ColorRef == guidColor && p.SizeRef == guidSize);
                carts[index].Quantity += int.Parse(quantity);
                carts[index].Fee += fee;
            }
            else
            {
                carts.Add(new VMOrders.VmOrderSubmit
                {
                    Color = color,
                    Size = size,
                    ColorRef = guidColor,
                    SizeRef = guidSize,
                    Code = code,
                    Quantity = int.Parse(quantity),
                    Name = name,
                    Fee = fee,
                    ProductId = guid,
                });
            }
            Session["Carts"] = carts;
            return carts.Sum(p => p.Quantity);
        }
        //public decimal AddToCartsWithSize(string id, string name, decimal fee, string quantity, string color, string size, Guid guid)
        //{
        //    var carts = new List<VMOrders.VmOrderSubmit>();
        //    var idd = Guid.Parse(id);
        //    if (Session["Carts"] != null)
        //    {
        //        carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
        //    }

        //    var guidColor = Guid.Parse(color);
        //    var guidSize = Guid.Parse(size);
        //    if ((carts ?? throw new InvalidOperationException()).Any(p => p.ProductId == idd && p.ColorRef == guidColor && p.SizeRef == guidSize))
        //    {
        //        int index = carts.FindIndex(p => p.ProductId == idd && p.ColorRef == guidColor && p.SizeRef == guidSize);
        //        carts[index].Quantity += int.Parse(quantity);
        //        carts[index].Fee += fee;
        //    }
        //    else
        //    {
        //        carts.Add(new VMOrders.VmOrderSubmit
        //        {
        //            Color = color,
        //            Size = size,
        //            ColorRef = guidColor,
        //            SizeRef = guidSize,
        //            Code = name,
        //            Quantity = int.Parse(quantity),
        //            Name = name,
        //            Fee = fee,
        //            ProductId = idd,
        //        });
        //    }
        //    Session["Carts"] = carts;
        //    return carts.Sum(p => p.Quantity);
        //}

        public decimal CartsCount()
        {
            try
            {

                if (Session["Carts"] != null)
                {
                    List<VMOrders.VmOrderSubmit> carts;
                    carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                    if (carts != null)
                    {
                        return carts.Count;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;

                }
            }
            catch (Exception e)
            {
                return 0;
            }
            //var userref = User.Identity.Name;
        }

        public void DeleteCarts(string id)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                var found = carts.FirstOrDefault(c => c.Code == id);
                if (found != null)
                {
                    if (found.Quantity <= 1)
                    {
                        carts.Remove(found);
                    }
                    else
                    {
                        found.Quantity--;
                    }
                }
            }
            Response.Redirect("/Carts");
        }

        [HttpPost]
        public void DeleteCartsPost(string id)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                var found = carts.FirstOrDefault(c => c.Code == id);
                if (found != null)
                {
                    if (found.Quantity <= 1)
                    {
                        carts.Remove(found);
                    }
                    else
                    {
                        found.Quantity--;
                    }
                }
            }
            Response.Redirect("/Carts");
        }    
        
        
        [HttpPost]
        public void UpdateCartsPost(string id,string entites)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                var found = carts.FirstOrDefault(c => c.Code == id);
                if (found != null)
                {
                    if (entites == "+")
                    {
                        found.Quantity++;
                    }
                    else
                    {
                        if (found.Quantity <= 1)
                        {
                            carts.Remove(found);
                        }
                        else
                        {
                            found.Quantity--;
                        }
                    }

                }
            }
            Response.Redirect("/Carts");
        }

        [HttpPost]
        public ActionResult SubmitPicture(HttpPostedFileBase FileName)
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Submit(HttpPostedFileBase FileName)
        {
            return View("OrderFinished");
        }



        [HttpPost]
        public void SetLocationRef(int id)
        {
            if (id != null)
            {
                Session["UserLocationRef"] = id;
            }
        }

        [HttpPost]
        public ActionResult Province(int id)
        {
            var result = RepAccountDashboard.SelectionProvince(id);
            if (result.Count > 0)
            {
                return PartialView("Body/P_Dropdown_Province", result);
            }
            else
            {
                return PartialView("Body/P_Dropdown_Province");
            }
        }











    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;
using WebApplicationOrders.Extensions;



namespace WebApplicationOrders.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders


        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = Guid.Parse(User.Identity.Name);
                var result = RepAccountDashboard.RepAccountDashboardInformation(id);
                return View(result);
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
                                        var persian = values.Phone.PersianToEnglish();
                                        carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                                        var codedis = "";
                                        if (Session["CodeDis"] != null)
                                        {
                                            codedis = Session["CodeDis"].ToString();
                                        }
                                        var resuult = RepOrders.RepositoryCartsCode(carts, codedis);
                                        decimal transferpay = 0;
                                       
                                        var result = RepOrders.RepositorySubmitOrders(values, resuult.CartsItems, file, persian, transferpay, "", "", resuult.Discount);
                                        if (result.Contains("Error"))
                                        {
                                            Session["OrderCode"] = "Error";
                                            Session["JavaScriptFunction"] = "Error";
                                            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                                            Response.Redirect("/Orders/OrderFinished");
                                        }
                                        else
                                        {
                                            Session["OrderCode"] = result;
                                            Session["JavaScriptFunction"] = "Success";
                                            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");

                                            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                                            string MerchantID = WebConfigurationManager.AppSettings["MerchantID"];
                                            string CallbackURL = WebConfigurationManager.AppSettings["CallbackURL"];


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

                                            String Description = values.Name + " " + values.Family + "_" + values.Phone + "_" + result + "_" + Amount;
                                            LogWriter.Logger("New Order = " + Description, DateTime.Now.ToString(), "134");
                                            HttpCookie cAmount = new HttpCookie("Amount", Amount.ToString());
                                            LogWriter.Logger("New Order = " + cAmount, DateTime.Now.ToString(), "136");
                                            HttpCookie cUserPhone = new HttpCookie("UserPhone", values.Phone);
                                            LogWriter.Logger("New Order = " + cUserPhone, DateTime.Now.ToString(), "138");
                                            HttpCookie cCode = new HttpCookie("Code", result);
                                            LogWriter.Logger("New Order = " + cCode, DateTime.Now.ToString(), "140");
                                            HttpCookie cook = new HttpCookie("Payment");
                                            LogWriter.Logger("New Order = " + cook, DateTime.Now.ToString(), "142");
                                            cook.Values.Add("Amount", Amount.ToString());
                                            LogWriter.Logger("New Order = " + Amount.ToString(), DateTime.Now.ToString(), "144");
                                            cook.Values.Add("UserPhone", values.Phone);
                                            LogWriter.Logger("New Order = " + values.Phone, DateTime.Now.ToString(), "146");
                                            cook.Values.Add("Code", result);
                                            LogWriter.Logger("New Order = " + result, DateTime.Now.ToString(), "148");
                                            Response.Cookies.Add(cook);
                                            LogWriter.Logger("New Order = " + cook, DateTime.Now.ToString(), "150");
                                            cAmount.Expires = DateTime.Now.AddHours(2);
                                            LogWriter.Logger("New Order = " + DateTime.Now.AddHours(2).ToString(), DateTime.Now.ToString(), "152");
                                            cUserPhone.Expires = DateTime.Now.AddHours(2);
                                            LogWriter.Logger("New Order = " + DateTime.Now.AddHours(2).ToString(), DateTime.Now.ToString(), "154");
                                            cCode.Expires = DateTime.Now.AddHours(2);
                                            LogWriter.Logger("New Order = " + cCode.Expires, DateTime.Now.ToString(), "156");
                                            Session["Amount"] = Amount;
                                            LogWriter.Logger("New Order = " + Amount, DateTime.Now.ToString(), "158");
                                            Session["UserPhone"] = values.Phone;
                                            LogWriter.Logger("New Order = " + values.Phone, DateTime.Now.ToString(), "160");
                                            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantID, Amount, CallbackURL, Description);
                                            LogWriter.Logger("New Order = " + pr, DateTime.Now.ToString(), "162");
                                            var res = zarinpal.InvokePaymentRequest(pr);
                                            LogWriter.Logger("New Order = " + res, DateTime.Now.ToString(), "164");
                                            string adminPhone = WebConfigurationManager.AppSettings["PhoneAdmin"];
                                            LogWriter.Logger("New Order = " + adminPhone, DateTime.Now.ToString(), "166");
                                            string CompanyName = WebConfigurationManager.AppSettings["CompanyName"];
                                            LogWriter.Logger("New Order = " + CompanyName, DateTime.Now.ToString(), "168");
                                            var sms = new SmsProviders();
                                            var a = sms.SendGenerateQuotationsAdmin(long.Parse(values.Phone), adminPhone, values.Name + " " + values.Family, CompanyName);
                                            if (a == "Success")
                                            {
                                                LogWriter.Logger("New Order = " + a, DateTime.Now.ToString(), "174");
                                                Response.Write("<script>alert(" + "Send" + ")</script>");
                                            }
                                            else
                                            {
                                                Response.Write("<script>alert(" + a + ")</script>");
                                            }

                                            if (res.Status == 100)
                                            {
                                                LogWriter.Logger("New Order = " + res.Status, DateTime.Now.ToString(), "184");
                                                Response.Redirect(res.PaymentURL);
                                            }

                                            LogWriter.Logger("New Order  res= " + res.Status, DateTime.Now.ToString(), "184");


                                            //Response.Redirect("/Orders/OrderFinished");
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

        public ActionResult OrderFinished()
        {
            return View();
        }

        [HttpPost]
        public decimal AddToCarts(string id, string name, decimal fee, string quantity, Guid guid, Guid? CategoriesRef)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;

            }


            if ((carts ?? throw new InvalidOperationException()).Any(p => p.ProductId == guid))
            {
                int index = carts.FindIndex(p => p.ProductId == guid);
                carts[index].Quantity += int.Parse(quantity);
                carts[index].Fee += fee;
            }
            else
            {
                carts.Add(new VMOrders.VmOrderSubmit
                {


                    Code = id,
                    Quantity = int.Parse(quantity),
                    Name = name,
                    Fee = fee,
                    CategoryRef = CategoriesRef,
                    ProductId = guid,

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
        public ActionResult SubmitPicture(HttpPostedFileBase FileName)
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Submit(HttpPostedFileBase FileName)
        {
            return View("OrderFinished");
        }



    }
}
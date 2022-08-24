using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using OrdersExtentions.Extensions;
using OrdersOrders.Repository.Orders;
using OrdersOrders.ViewModels.Orders;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;



namespace ApplicationOrders.Controllers
{
    public class OrdersController : Controller
    {



        // GET: Orders


        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult FinalSum(string sum)
        {
            Session["SumPayment"] = sum;
            return RedirectToAction("Index", "Carts");
        }




        [HttpPost]
        public ActionResult SubmitOrder(VMOrders.VmOrderSubmit values, HttpPostedFileBase file)
        {

            //if (ModelState.IsValid)
            //{
            //    if (values.Name != "")
            //    {
            //        if (values.Family != "")
            //        {
            //            if (values.Phone != "")
            //            {
            //                if (values.Address != "")
            //                {
            //                    var carts = new List<VMOrders.VmOrderSubmit>();
            //                    if (Session["Carts"] != null)
            //                    {
            //                        var persian = values.Phone.PersianToEnglish();
            //                        carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
            //                        var result = RepOrders.RepositorySubmitOrders(values, carts, file, persian);
            //                        if (result.Contains("Error"))
            //                        {
            //                            Session["OrderCode"] = "Error";
            //                            Session["JavaScriptFunction"] = "Error";
            //                            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            //                            Response.Redirect("/Orders/OrderFinished");
            //                        }
            //                        else
            //                        {
            //                            Session["OrderCode"] = result;
            //                            Session["JavaScriptFunction"] = "Success";
            //                            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");

            //                            //ZarinPal.ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
            //                            //string MerchantID = WebConfigurationManager.AppSettings["MerchantID"];
            //                            //string CallbackURL = WebConfigurationManager.AppSettings["CallbackURL"];


            //                            long Amount = 0;
            //                            if (Session["PayAmount"] != null)
            //                            {
            //                                Amount = long.Parse(Session["PayAmount"].ToString());
            //                            }
            //                            //decimal fee = 0;
            //                            //if (carts != null)
            //                            //{
            //                            //    foreach (var cart in carts)
            //                            //    {


            //                            //        if (Session["SumPayment"] != null)
            //                            //        {
            //                            //            var SumWithDiscount = int.Parse(Session["SumPayment"].ToString());
            //                            //            fee = SumWithDiscount;
            //                            //            /*  if (cart.Discount > 0)
            //                            //              {
            //                            //                  var sum = ((cart.Fee) * cart.Quantity);
            //                            //                  fee += sum;
            //                            //              }
            //                            //              else
            //                            //              {
            //                            //                  var sum = (cart.Fee * cart.Quantity);
            //                            //                  var discount = (sum * cart.Discount) / 100;
            //                            //                  fee += sum - discount;
            //                            //              }*/
            //                            //        }
            //                            //        else
            //                            //        {
            //                            //            var sum = ((cart.Fee) * cart.Quantity);
            //                            //            fee += sum;
            //                            //        }
            //                            //    }

            //                            //}



            //                            //var resultPay = Repfooter.RepInformationFooter();
            //                            //if (resultPay.Count > 0)
            //                            //{
            //                            //    foreach (var item in resultPay)
            //                            //    {
            //                            //        fee += decimal.Parse(item.TarnsferPay);
            //                            //        Amount = long.Parse(fee.ToString());
            //                            //    }
            //                            //}

            //                            String Description = values.Name + " " + values.Family + "_" + values.Phone + "_" + result + "_" + Amount;
            //                            HttpCookie cAmount = new HttpCookie("Amount", Amount.ToString());
            //                            HttpCookie cUserPhone = new HttpCookie("UserPhone", values.Phone);
            //                            HttpCookie cCode = new HttpCookie("Code", result);

            //                            HttpCookie cook = new HttpCookie("Payment");
            //                            cook.Values.Add("Amount", Amount.ToString());
            //                            cook.Values.Add("UserPhone", values.Phone);
            //                            cook.Values.Add("Code", result);
            //                            Response.Cookies.Add(cook);
            //                            cAmount.Expires = DateTime.Now.AddHours(2);
            //                            cUserPhone.Expires = DateTime.Now.AddHours(2);
            //                            cCode.Expires = DateTime.Now.AddHours(2);
            //                            Session["Amount"] = Amount;
            //                            Session["UserPhone"] = values.Phone;
            //                            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantID, Amount, CallbackURL, Description);
            //                            var res = zarinpal.InvokePaymentRequest(pr);
            //                            string adminPhone = WebConfigurationManager.AppSettings["PhoneAdmin"];
            //                            string CompanyName = WebConfigurationManager.AppSettings["CompanyName"];
            //                            var sms = new SmsProviders();
            //                            var a = sms.SendGenerateQuotationsAdmin(long.Parse(values.Phone), adminPhone, values.Name + " " + values.Family, CompanyName);
            //                            if (a == "Success")
            //                            {
            //                                Response.Write("<script>alert(" + "Send" + ")</script>");
            //                            }
            //                            else
            //                            {
            //                                Response.Write("<script>alert(" + a + ")</script>");
            //                            }

            //                            if (res.Status == 100)
            //                            {

            //                                Response.Redirect(res.PaymentURL);
            //                            }



            //                            //Response.Redirect("/Orders/OrderFinished");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            //                        Response.Redirect("/Product");
            //                    }

            //                }
            //                else
            //                {
            //                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            //                    Response.Redirect("/Product");
            //                }

            //            }
            //            else
            //            {
            //                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            //                Response.Redirect("/Product");
            //            }

            //        }
            //        else
            //        {
            //            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            //            Response.Redirect("/Product");
            //        }

            //    }
            //    else
            //    {
            //        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            //        Response.Redirect("/Product");
            //    }


            //}

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
        public decimal AddToCartsWithSize(string id, string name, decimal fee, string quantity, string color, string size, Guid guid)
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
                    Code = id,
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
            //var userref = User.Identity.Name;
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;

            }
            else
            {
                return 0;

            }

            return carts.Count;
        }



        public void DeleteCarts(string id)
        {
            var carts = new List<VMOrders.VmOrderSubmit>();

            if (Session["Carts"] != null)
            {
                carts = Session["Carts"] as List<VMOrders.VmOrderSubmit>;
                var found = carts.FirstOrDefault(c => c.Code == id );
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
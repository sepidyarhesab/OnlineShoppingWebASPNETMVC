using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OrdersGeneral.Repository.General;
using SepidyarHesabExtensions.Extentions;
using WebApplicationOrders.Database;
using WebApplicationOrders.Models.Database.OrderDatabase;


namespace WebApplicationOrders.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }





        [HttpPost]
        public ActionResult SubmitRegister(string name, string family, string phone)
        {
            try
            {
                if (name != "")
                {
                    if (family != "")
                    {
                        if (phone != "")
                        {
                            var db = new Orders_Entities();
                            var mobile = db.Table_User.ToList().Exists(c=>c.Phone == phone);
                            if (!mobile)
                            {
                                var result = RepUsers.RepositoryRegisterUser(name, family, phone);
                                if (result.Contains("Error"))
                                {
                                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                                    return View("Index");
                                }
                                else
                                {
                                    FormsAuthentication.SetAuthCookie(result, true);
                                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");

                                    if (Session["UrlBack"] != null)
                                    {
                                        return RedirectToAction("Index", "Orders");
                                    }
                                    else
                                    {
                                        return View("Index");
                                    }
                                }
                            }
                            else
                            {
                                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "شماره تلفن تکراری است");
                                return View("Index");

                            }
                        }
                        else
                        {
                            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "شماره موبایل خود را وارد کنید");
                            return View("Index");
                        }
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نام خانوادگی خود را وارد کنید");
                        return View("Index");
                    }
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نام خود را وارد کنید");
                    return View("Index");
                }
            }
            catch (Exception e)
            {
               
            }
            return View("Index");
        }
    }
}
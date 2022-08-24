using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OrdersDatabase.Models;
using OrdersGeneral.Repository.General;
using SepidyarHesabExtensions.Extentions;



namespace ApplicationOrders.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }






        [HttpPost]
        public ActionResult SubmitRegister(string name , string family ,string phone)
        {
            if (name != "")
            {
                if (family != "")
                {
                    if (phone != "")
                    {
                        var db = new Orders_Entities();
                        var mobile = db.Table_User.Where(c => c.Phone == phone).Count();
                        if (mobile == 0)
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
                                return View("Index");
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
    }
}
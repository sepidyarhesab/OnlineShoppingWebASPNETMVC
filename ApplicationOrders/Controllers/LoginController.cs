using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OrdersExtentions.Extensions;
using OrdersGeneral.Repository.General;
using Rozhano_WebApplication2.Extensions;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;

namespace ApplicationOrders.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login(string phone)
        {
            if (phone != "")
            {
                var result = RepUsers.RepositoryLoginUser(PersianDigits.PersianToEnglish(phone));
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                    LogWriter.Logger(result, "", "");
                    return View("Index");
                }
                else
                {
                    Session["LoginData"] = result;
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                    return RedirectToAction("Index", "Verify");
                }
            }
            else
            {
                return View("Index");
            }
        }


        public ActionResult Login()
        {
            return View("Index");
        }


        public void SingOut()
        {
            FormsAuthentication.SignOut();
            Response.Redirect("/Default");
        }


        public void Send()
        {
            var sms = new SmsProviders();
            sms.SendGenerateQuotations(09120448735, "2000");
        }


    }
}
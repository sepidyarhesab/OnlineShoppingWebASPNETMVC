using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Extentions;

namespace WebApplicationHamtOrders.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }

        public void GenerateContactUs(string Name, string Objects, string Phone, string Email, string Note)
        {

            if (Name != "")
            {
                if (Phone != "")
                {
                    if (Note != "")
                    {
                        var Userid = Guid.Parse(User.Identity.Name);
                        var result = RepContactUs.AddContact(Name, Objects, Email, Phone, Note, Userid);
                        if (result.Contains("Error"))
                        {
                            Session["Error"] = "Error";
                            TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است",
                                "خطا در ایجاد سطر به دلیل : " + result);
                        }
                        else
                        {
                            Session["Success"] = "Success";
                            Session["Result"] = result;
                            TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نام نمیتواند خالی باشد");
                }
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نام نمیتواند خالی باشد");
            }

            Response.Redirect("/ContactUs/Result");

        }

        public ActionResult ContactUs()
        {
            var query = RepContactUs.RepositoryContactManagements();
            return View(query);
        }
        [HttpPost]
        public ActionResult ContactUs(string search)
        {
            if (search != "")
            {
                var result = RepContactUs.RepositoryContactManagementsSearch(search);
                return View(result);
            }
            else
            {
                var query = RepContactUs.RepositoryContactManagements();
                return View(query);
            }
        }

        
        public void Answer(Guid Id)
        {
            var db = new Orders_Entities();
            var phone = "";
            var name = "";
            var objects = "";
            var code = "";
            var query = db.Table_ContactUs.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                name = query.Name;
                phone = query.Phone;
                if (query.Objects == "Complaint")
                {
                    objects = "انتقاد یا شکایات";
                }
                if (query.Objects == "Offer")
                {
                    objects = "پیشنهاد";
                }
                if (query.Objects == "Pursuit")
                {
                    objects = "پیگیری سفارش";
                }
                if (query.Objects == "Service")
                {
                    objects = "خدمات پس از فروش";
                }
                if (query.Objects == "Management")
                {
                    objects = "مدیریت";
                }
                if (query.Objects == "Others")
                {
                    objects = "سایر موضوعات";
                }
                code = query.Code;
                long mobile = long.Parse(phone);

                var sms = new SmsProviders();
                sms.SendSMSContactus(mobile,  code ,  objects,  name);
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Session["SMS"] = "اس ام اس برای کاربر ارسال شد.";
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Session["SMS"] = "اس ام اس برای کاربر ارسال نشده است..";
            }

            Response.Redirect("/ContactUs/ContactUs");
        }


        public ActionResult Result()
        {
            return View();
        }

        public void Delete(Guid id)
        {
            var result = RepContactUs.DeleteRow(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ContactUs/ContactUs");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/ContactUs/ContactUs");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ContactUs/ContactUs");

            }


        }

        public void ChangeStatus(Guid id)
        {
            var result = RepContactUs.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ContactUs/ContactUs");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ContactUs/ContactUs");
            }

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersSettings.Repository.Settings;
using OrdersSettings.ViewModels.Settings;
using SepidyarHesabExtensions.Extentions;

namespace SepidyarHesabDrWebApplication.Controllers
{
    
    public class ViewConfigurationAdminController : Controller
    {
        // GET: ViewConfigurationAdmin
        public RepViewConfiguration rep = new RepViewConfiguration();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("Index");
            }
            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public void Generate(ViewModelsMainViewConfigurations values)
        {
            var Userid = Guid.Parse (User.Identity.Name);
            
            var result = RepViewConfiguration.Add(values, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
            }

            Response.Redirect("/ViewConfigurationAdmin/Index");
        }

        public ActionResult Edit(Guid id)
        {


            if (User.Identity.IsAuthenticated)
            {

                var result = RepViewConfiguration.Edit(id);
                if (result != null)
                {
                    return View(result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                    return View("Index");
                }
            }
            return RedirectToAction("Index", "Login");

        }
        [HttpPost]
        public void EditRow(ViewModelsMainViewConfigurations value)
        {

            if (ModelState.IsValid)
            {
                var Userid =Guid.Parse(User.Identity.Name);
                var result = RepViewConfiguration.EditRow(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/ViewConfigurationAdmin/Index");


        }
        public void Delete(Guid Id)
        {

            var Result = RepViewConfiguration.Delete(Id);
            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ViewConfigurationAdmin/CodeIndex");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/ViewConfigurationAdmin/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ViewConfigurationAdmin/Index");

            }

        }
        public void ChangeStatus(Guid id)
        {
            var Result = RepViewConfiguration.ChangeStatus(id);
            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ViewConfigurationAdmin/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ViewConfigurationAdmin/Index");
            }

        }


        public void ChangeMain(Guid id)
        {
            var Result = RepViewConfiguration.ChangeMain(id);
            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ViewConfigurationAdmin/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ViewConfigurationAdmin/Index");
            }

        }


    }
}
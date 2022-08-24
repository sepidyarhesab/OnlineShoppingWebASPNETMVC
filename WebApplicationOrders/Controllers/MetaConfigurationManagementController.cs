using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersSettings.Repository.Settings;
using OrdersSettings.ViewModels.Settings;
using SepidyarHesabExtensions.Extentions;

namespace WebApplicationOrders.Controllers
{
    public class MetaConfigurationManagementController : Controller
    {
        // GET: MetaConfigurationManagement
        public RepMetaConfiguration rep = new RepMetaConfiguration();
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
        [ValidateInput(false)]
        public void Generate(VMMetaConfiguration.ViewModelsMetaConfigurations values)
        {
            var Userid = Guid.Parse(User.Identity.Name);

            var result = RepMetaConfiguration.Add(values, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
            }

            Response.Redirect("/MetaConfigurationManagement/Index");
        }

        public ActionResult Edit(Guid id)
        {


            if (User.Identity.IsAuthenticated)
            {

                var result = RepMetaConfiguration.Edit(id);
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
        [ValidateInput(false)]
        public void EditRow(VMMetaConfiguration.ViewModelsMetaConfigurations value)
        {

            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepMetaConfiguration.EditRow(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/MetaConfigurationManagement/Index");


        }
        public void Delete(Guid Id)
        {

            var Result = RepMetaConfiguration.Delete(Id);
            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MetaConfigurationManagement/Index");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/MetaConfigurationManagement/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MetaConfigurationManagement/Index");

            }

        }
        public void ChangeStatus(Guid id)
        {
            var Result = RepMetaConfiguration.ChangeStatus(id);
            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MetaConfigurationManagement/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MetaConfigurationManagement/Index");
            }

        }


        public void ChangeMain(Guid id)
        {
            var Result = RepMetaConfiguration.ChangeMain(id);
            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MetaConfigurationManagement/Index");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MetaConfigurationManagement/Index");
            }

        }
    }
}
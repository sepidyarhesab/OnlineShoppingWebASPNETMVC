
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using SepidyarHesabExtensions.Extentions;
using OrdersGeneral.ViewModels.General;
using OrdersSettings.ViewModels.Settings;

namespace WebApplicationOrders.Controllers
{
    public class SettingsManagementController : Controller
    {
        // GETSettingsManagement

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
        #region ChangeStatus
        public void ChangeStatus(Guid id)
        {
            var result = RepSettings.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SettingsManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SettingsManagement");
            }

        }



        #endregion
        #region Edit

        public ActionResult Edit(Guid id)
        {
            var result = RepSettings.RepositoryMainsSettingsManagmetsById(id);
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

        public void EditRow(VMSettings.VMSettingsManagmet value, HttpPostedFileBase file, HttpPostedFileBase file2)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepSettings.RepositoryMainSettingsManagemenetEditById(value,file,file2, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
            }

            Response.Redirect("/SettingsManagement");
        }




        #endregion

        [HttpPost]
        public void Generate(VMSettings.VMSettingsManagmet values)
        {


            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepSettings.AddNewRow(values, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "عنوان اصلی نمیتواند خالی باشد");
            }





            Response.Redirect("/SettingsManagement/Index");
        }

        public void Delete(Guid Id)
        {
            var result = RepSettings.DeleteSetting(Id);
            if (result.Contains("Error"))
            {

                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else if (result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/SettingsManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SettingsManagement");
            }
        }
    }
}
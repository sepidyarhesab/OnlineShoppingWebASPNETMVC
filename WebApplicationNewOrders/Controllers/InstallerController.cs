using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersSettings.Repository.Settings;
using OrdersSettings.ViewModels.Settings;
using SepidyarHesabExtensions.Extentions;



namespace WebApplicationNewOrders.Controllers
{
    public class InstallerController : Controller
    {
        // GET: Installer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Token()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public void Generate(VMSettings.VMSettingsManagmet values)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepInstaller.Generate(values, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/Installer/Token");
            }
            Response.Redirect("/Installer");
        }
    }
}
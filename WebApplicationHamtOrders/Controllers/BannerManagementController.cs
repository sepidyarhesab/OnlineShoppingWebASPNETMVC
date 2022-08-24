using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Extentions;

namespace WebApplicationHamtOrders.Controllers
{
    public class BannerManagementController : Controller
    {
        // GET: BannerManagement
        public ActionResult Index()
        {
            var result = RepBanner.RepositoryBannerManagements();
            return View(result);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public void Generate(VMBanner.VmBannerManagement values, HttpPostedFileBase FileNameDesktop)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepBanner.AddNewRow(values, FileNameDesktop, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] =
                    IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
            }
            Response.Redirect("/BannerManagement/Index");
        }


        public ActionResult Edit(Guid id)
        {
            var result = RepBanner.EditBanner(id);
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


        [HttpPost]
        public void EditRow(VMBanner.VmBannerManagement value, HttpPostedFileBase FileNameDesktop, HttpPostedFileBase FileNameMobile)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepBanner.RepositoryBannerEdit(value, FileNameDesktop, FileNameMobile, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/BannerManagement");
        }

        #region Delete
        public void Delete(Guid id)
        {
            var result = RepBanner.DeleteRow(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/BannerManagement");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/BannerManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/BannerManagement");

            }


        }


        #endregion

        #region Changestatus
        public void ChangeStatus(Guid id)
        {
            var result = RepBanner.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/BannerManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/BannerManagement");
            }

        }


        #endregion

    }
}
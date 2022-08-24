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
    public class KnowingController : Controller
    {
        // GET: Knowing
        public ActionResult Index()
        {
            var query = RepKnowing.RepositoryKnowing();
            return View(query);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(Guid Id)
        {
            var result = RepKnowing.EditKnowingManagement(Id);
            if (result != null)
            {
                return View(result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("AboutUs");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public void Edit(VMKnowing.VmKnowingManagement value, HttpPostedFileBase FileName)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepKnowing.Edit(value, FileName, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/Knowing/AboutUs");
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public void Generate(VMKnowing.VmKnowingManagement value, HttpPostedFileBase FileName)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepKnowing.Generate(value, FileName,  Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
            }
            Response.Redirect("/Knowing/AboutUs");
        }

        public void Delete(Guid id)
        {
            var result = RepKnowing.DeleteRow(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/Knowing/AboutUs");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/Knowing/AboutUs");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/Knowing/AboutUs");

            }


        }

        #region Changestatus
        public void ChangeStatus(Guid id)
        {
            var result = RepKnowing.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/Knowing/AboutUs");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/Knowing/AboutUs");
            }

        }

        #endregion

        public ActionResult AboutUs()
        {
            var query = RepKnowing.RepositoryKnowingManagements();
            return View(query);
        }


    }
}
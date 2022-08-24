using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Extentions;



namespace WebApplicationOrders.Controllers
{
    public class SlidersManagementController : Controller
    {
        // GET: SlidersManagement

        public ActionResult Index()
        {

            var query = RepSliders.RepositoryMainSlidersManagemenet();
            return View(query);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepSliders.RepositoryMainSlidersManagemenetSearch(search);
                return View(result);
            }
            else
            {
                var query = RepSliders.RepositoryMainSlidersManagemenet();
                return View(query);
            }
        }

        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }



        #region Changestatus
        public void ChangeStatus(Guid id)
        {
            var result = RepSliders.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SlidersManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SlidersManagement");
            }

        }


        #endregion


        #region Delete
        public void Delete(Guid id)
        {
            var result = RepSliders.DeleteRow(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SlidersManagement");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/SlidersManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SlidersManagement");

            }


        }


        #endregion

        #region Edit

        public ActionResult Edit(Guid id)
        {

            var result = RepSliders.RepositoryMainSlidersManagemenetById(id);
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
        public void EditRow(VMSliders.VmMainSlidersGenerate value, HttpPostedFileBase FileNameDesktop, HttpPostedFileBase FileNameMobile)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepSliders.RepositoryMainSlidersManagemenetEditById(value, FileNameDesktop, FileNameMobile, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/SlidersManagement");
        }

        #endregion



        [HttpPost]
        public void Generate(VMSliders.VmMainSlidersGenerate values, HttpPostedFileBase FileNameDesktop, HttpPostedFileBase FileNameMobile)
        {
            if (ModelState.IsValid)
            {
                if (FileNameDesktop != null)
                {
                    if (values.PrimaryTitle != "")
                    {
                        var Userid = Guid.Parse(User.Identity.Name);
                        var result = RepSliders.AddNewRow(values, FileNameDesktop,FileNameMobile,Userid);
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
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "فایل نمیتواند خالی باشد");

                }
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "مدل ارسالی شما اشتباه است");
            }
            Response.Redirect("/SlidersManagement/Index");
        }
      

    }
}
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
    public class ServiceManagementController : Controller
    {
        // GET: ServiceManagement
        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return View();
            //}
            //return RedirectToAction("Index", "Login");

            var query = RepServices.RepositoryMainServicesManagemenet();
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepServices.RepositoryMainServicesManagemenetSearch(search);
                return View(result);
            }
            else
            {
                var query = RepServices.RepositoryMainServicesManagemenet();
                return View(query);
            }
        }


        #region Add
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public void Generate(VMServices.VMMainServicesManagement values, HttpPostedFileBase filename)
        {
            if (ModelState.IsValid)
            {
                if (filename != null)
                {
                    if (values.PrimaryTitle != "")
                    {
                        var Userid = Guid.Parse(User.Identity.Name);
                        var result = RepServices.AddnewRow(values, filename, Userid);
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
            Response.Redirect("/ServiceManagement/Index");
        }

        #endregion
        #region ChangeStatus
        public void ChangeStatus(Guid id)
        {
            var result = RepServices.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ServiceManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ServiceManagement");
            }

        }



        #endregion

        #region Edit
        public ActionResult Edit(Guid id)
        {
            var result = RepServices.RepositoryMainServicesManagemenetById(id);
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
        public void EditRow(VMServices.VMMainServicesManagement value, HttpPostedFileBase filename)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepServices.RepositoryMainServicesManagemenetEditById(value, filename, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/ServiceManagement");
        }

        


        #endregion


        #region Delete
        public void DeleteRow(Guid id)
        {
            var result = RepServices.DeleteRow(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ServiceManagement");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/ServiceManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ServiceManagement");
            }

        }


        #endregion

    }
}
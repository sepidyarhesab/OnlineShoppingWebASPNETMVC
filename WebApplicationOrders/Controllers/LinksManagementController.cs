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
    public class LinksManagementController : Controller
    {
        // GET: LinksManagement



        #region Index
        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return View();
            //}
            //return RedirectToAction("Index", "Login");
            var query = RepLinks.RepositoryMainLinksManagement();
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepLinks.RepositoryMainLinksManagementSearch(search);
                return View(result);
            }
            else
            {
                var query = RepLinks.RepositoryMainLinksManagement();
                return View(query);
            }
            
        }


        #endregion

        #region ChangeStatus
        public void ChangeStatus(Guid id)
        {
            var result = RepLinks.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/LinksManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/LinksManagement");
            }

        }



        #endregion

        #region Delete
        public void Delete(Guid id)
        {
            var result = RepLinks.DeleteRow(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/LinksManagement");
            }
            else if (result.Contains("TRUE"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/LinksManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/LinksManagement");
            }

        }


        #endregion

        #region Add
        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }



        #endregion


        #region Generate
        [HttpPost]

        public void Generate(VMLinks.VMMainLinksManagementGenerate values)
        {
            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepLinks.AddNewRow(values, Userid);
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

            Response.Redirect("/LinksManagement/Index");
        }





        #endregion


        #region Edit
        public ActionResult Edit(Guid id)
        {
            var result = RepLinks.RepositoryMainLinksManagementById(id);
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
        public void EditRow(VMLinks.VMMainLinksManagement value, HttpPostedFileBase filename)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepLinks.RepositoryMainLinksManagemenetEditById(value, filename,Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/LinksManagement");
        }




        #endregion


    }

}
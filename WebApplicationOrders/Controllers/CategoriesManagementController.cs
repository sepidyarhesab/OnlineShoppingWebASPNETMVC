using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using OrdersInventory.ViewModels.Inventory;
using SepidyarHesabExtensions.Extentions;

namespace WebApplicationOrders.Controllers
{
    public class CategoriesManagementController : Controller
    {
        // GET: CategoriesManagement
        public RepProducts rep = new RepProducts();

        public ActionResult Index()
        {
            var query = rep.RepositoryCategoryManagment();
            return View(query);

        }

        [HttpPost]
        public ActionResult Index(string search)
        {

            if (search != "")
            {
                var result = rep.RepositoryCategoryManagmentSearch(search);
                return View(result);
            }
            else
            {
                var query = rep.RepositoryCategoryManagment();
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

        public ActionResult Edit(Guid id)
        {


            if (User.Identity.IsAuthenticated)
            {
                
                var result = rep.RepositoryCategoryMangmentByid(id);
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
        public void EditRow(VMProduct.ViewModelCategoreis value, HttpPostedFileBase FileNameDesktop)
        {
            
                if (ModelState.IsValid)
                {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.RepositoryCategoryMangmentEditById(value, FileNameDesktop, Userid);
                    if (result.Contains("Error"))
                    {
                        TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                    }
                    else
                    {
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                    }
                }
                Response.Redirect("/CategoriesManagement/Index");
        }

        public void Delete(Guid Id)
        {

            var Result = rep.DeleteCategory(Id);

            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/CategoriesManagement");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/CategoriesManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/CategoriesManagement");
            }




        }

        [HttpPost]
        public void Generate(VMProduct.ViewModelCategoreis values, HttpPostedFileBase file)
        {


            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.AddNewCategories(values, file, Userid);
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

            Response.Redirect("/CategoriesManagement/Index");
        }


        public void ChangeStatus(Guid Id)
        {
            var Result = rep.ChangeCategoryStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/CategoriesManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/CategoriesManagement");
            }

        }
    }
}
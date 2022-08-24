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
    public class BlogCategoriesManagementController : Controller
    {
        // GET: BlogCategoriesManagement
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
        [HttpPost]
        public void Generate(VMBlogs.ViewModelCategories values)
        {


            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepBlog.AddNewBlogCategories(values, Userid);
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

            Response.Redirect("/BlogCategoriesManagement/Index");
        }

        public ActionResult Edit(Guid id)
        {


            if (User.Identity.IsAuthenticated)
            {

                var result = RepBlog.RepositoryBlogCategoryMangmentByid(id);
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
        public void EditRow(VMBlogs.ViewModelCategories value)
        {

            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepBlog.RepositoryBlogCategoryMangmentEditById(value ,Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/BlogCategoriesManagement/Index");


        }

        public void Delete(Guid Id)
        {

            var Result = RepBlog.DeleteBlogCategory(Id);

            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/BlogCategoriesManagement");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/BlogCategoriesManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/BlogCategoriesManagement");
            }




        }

        public void ChangeBlogStatus(Guid Id)
        {
            var Result = RepBlog.ChangeBlogCategoryStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/BlogCategoriesManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/BlogCategoriesManagement");
            }

        }
    }
}
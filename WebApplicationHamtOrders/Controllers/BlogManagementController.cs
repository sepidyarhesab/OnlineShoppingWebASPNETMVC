using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Extentions;



namespace WebApplicationHamtOrders.Controllers
{
    public class BlogManagementController : Controller
    {
        // GET: BlogManagement
        public ActionResult Index()
        {
            var query = RepBlog.RepositoryMainBlogManagement();
            return View(query);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepBlog.RepositoryBlogManagementSearch(search);
                return View(result);
            }
            else
            {
                var query = RepBlog.RepositoryMainBlogManagement();
                return View(query);
            }
        }
        

        public ActionResult Add()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public void Generate(VMBlogs.VMBlog values, HttpPostedFileBase FileName)
        {
            if (FileName != null)
            {
                if (values.PrimaryTitle != "")
                {
                    var Userid = Guid.Parse(User.Identity.Name) ;
                    var result = RepBlog.AddNewBlogRow(values, FileName, Userid);
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
            Response.Redirect("/BlogManagement/Index");
        }

        public void ChangeStatus(Guid id)
        {
            var Result = RepBlog.ChangeBlogStatus(id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/BlogManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/BlogManagement");
            }

        }

        public void Delete(Guid Id)
        {
            var Result = RepBlog.DeleteBlog(Id);

            if (Result.Contains("Error"))
            {

                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/BlogManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/BlogManagement");
            }
        }

        public ActionResult Edit(Guid id)
        {
            
            var result = RepBlog.RepositoryMainBlogMangmentByid(id);
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


        [ValidateInput(false)]
        [HttpPost]
        public void EditRow(VMBlogs.VMBlog value, HttpPostedFileBase filename)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepBlog.RepositoryMainBlogManagemenetEditById(value, filename, Userid);
                if (result.Contains("Error"))
                { 
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/BlogManagement");
        }



    }


}
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
    public class MoreProductPropertyManagementController : Controller
    {

        public RepProducts rep = new RepProducts();
        #region  Color 
        public ActionResult IndexColor()
        {
            
           var query = rep.RepositoryMainColorMangment();
           return View(query);
           
        }


        [HttpPost]
        public ActionResult IndexColor(string search)
        {
            if (search != "")
            {
                var result = rep.RepositoryMainColorMangmentSearch(search);
                return View(result);
            }
            else
            {
                var query = rep.RepositoryMainColorMangment();
                return View(query);
            }
        }
        public ActionResult AddColor()
        {
            return View();
        }

        public ActionResult EditColor(Guid id)
        {
            var result = rep.RepositoryColorMangmentByid(id);
            if (result != null)
            {
                return View(result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("IndexColor");
            }
        }

        [HttpPost]
        public void EditColorRow(VMProduct.ViewModelProductColor values)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.RepositoryMainColorManagemenetEditById(values,Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/MoreProductPropertyManagement/ IndexColor");
        }

        public void Delete(Guid Id)
        {

            var Result = rep.DeleteColor(Id);

            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MoreProductPropertyManagement/IndexColor");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/MoreProductPropertyManagement/IndexColor");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MoreProductPropertyManagement/IndexColor");
            }



        }

        public void ChangeStatus(Guid Id)
        {
            var Result = rep.ChangeColorStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MoreProductPropertyManagement/IndexColor");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MoreProductPropertyManagement/IndexColor");
            }

        }

        [HttpPost]
        public void Generate(VMProduct.ViewModelProductColor values)
        {

            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.AddNewProductColor(values, Userid);
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

            Response.Redirect("/MoreProductPropertyManagement/IndexColor");
        }


        #endregion
        // GET: MoreProperty

        #region  Size
        public ActionResult IndexSize()
        {
            var query = rep.RepositoryMainSizeMangment();
            return View(query);
        }
        [HttpPost]
        public ActionResult IndexSize(string search)
        {
            if (search != "")
            {
                var result = rep.RepositoryMainSizeMangmentSearch(search);
                return View(result);
            }
            else
            {
                var query = rep.RepositoryMainSizeMangment();
                return View(query);
            }
        }

        public ActionResult AddSize()
        {
            return View();
        }

        public ActionResult EditSize(Guid id)
        {
            var result = rep.RepositorySizeMangmentByid(id);
            if (result != null)
            {
                return View(result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("IndexSize");
            }
        }

        [HttpPost]
        public void EditRowSize(VMProduct.ViewModelProductSize value)
        {

            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.RepositorySizeMangmentEditById(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/MoreProductPropertyManagement/IndexSize");


        }
        [HttpPost]
        public void GenerateSize(VMProduct.ViewModelProductSize values)
        {

            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.AddNewProductSize(values, Userid);
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

            Response.Redirect("/MoreProductPropertyManagement/IndexSize");
        }

        public void DeleteSize(Guid Id)
        {

            var Result = rep.DeleteSize(Id);

            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MoreProductPropertyManagement/IndexSize");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/MoreProductPropertyManagement/IndexSize");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MoreProductPropertyManagement/IndexSize");
            }


          


        }
        public void ChangeSizeStatus(Guid Id)
        {
            var Result = rep.ChangeSizeStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/MoreProductPropertyManagement/IndexSize");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/MoreProductPropertyManagement/IndexSize");
            }

        }
        #endregion



    }
}
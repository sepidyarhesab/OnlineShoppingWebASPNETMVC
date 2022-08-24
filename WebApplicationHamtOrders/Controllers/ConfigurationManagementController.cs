using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersInventory.Repository.Inventory;
using OrdersInventory.ViewModels.Inventory;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicationHamtOrders.Controllers
{
    public class ConfigurationManagementController : Controller
    {
        // GET: Configuration

        public RepProducts rep = new RepProducts();

        public ActionResult Index()
        {

            //if (User.Identity.IsAuthenticated)
            //{
            //    return View();
            //}
            //return RedirectToAction("Index", "Login");
            var query = rep.RepositoryMainConfiguration();
            return View(query);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {

            if (search != "")
            {
                var result = rep.RepositoryMainConfigurationSearch(search);
                return View(result);
            }
            else
            {
                var query = rep.RepositoryMainConfiguration();
                return View(query);
            }

        }
        

        [HttpPost]
        public ActionResult IndexCategories(string id)
        {

            if (id != "")
            {
                var idguid = Guid.Parse(Session["IdSelectionConfig"].ToString());
                Session["CategoriesRef"] = id;
                Response.Redirect("/ConfigurationManagement/AddSelection/"+ idguid);
                return View("Index");
            }
            else
            {
                var idd = Guid.Parse(id);
                var query = rep.RepositoryMainProductsManagementByConfigRef(idd);
                return View("AddSelection", query);


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

        [HttpPost]
        public void Generate(VMProduct.ViewModelConfiguration values)
        {

            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.AddNewConfiguration(values, Userid);
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

            Response.Redirect("/ConfigurationManagement/Index");
        }

        public ActionResult AddSelection(Guid id)
        {
            if (Session["CategoriesRef"] != null)
            {
                var idguid = Guid.Parse(Session["IdSelectionConfig"].ToString());
                var idd = Guid.Parse(Session["CategoriesRef"].ToString());
                var result = rep.RepositoryMainProductsMangmentByConfigRefAndByCagegoriesRef(idguid, idd);
                return View(result);
            }
            else
            {
                Session["IdSelectionConfig"] = id;
                ViewBag.Id = id;
                var query = rep.RepositoryMainProductsManagementByConfigRef(id);
                return View(query);
            }
        }

        [HttpPost]
        public ActionResult AddSelection(Guid id,string search)
        {
            Session["IdSelectionConfig"] = id;
            ViewBag.Id = id;
            ViewBag.search = search;
            
            if (search != "")
            {
                var result = rep.RepositoryMainProductsMangmentByConfigRefAndSearchKey(id, search);
                return View(result);
            }
            else
            {
                var query = rep.RepositoryMainProductsManagementByConfigRef(id);
                return View(query);
            }
        }


        

        public ActionResult AddSelectionProduct(Guid id)
        {
            if (Session["IdSelectionConfig"] != null)
            {
                var configRef = Guid.Parse(Session["IdSelectionConfig"].ToString());
                var query = rep.RepositoryMainProductAddToConfigurationSelection(configRef,id);
                if (query =="Success")
                {
                    return Redirect("/ConfigurationManagement/AddSelection/" + configRef);
                }
                else
                {
                    return Redirect("/ConfigurationManagement/AddSelection/" + configRef);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
           
;
        }


        public ActionResult Edit(Guid id)
        {
            var result = rep.RepositoryConfigurationMangmentByid(id);
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
        public void EditRow(VMProduct.ViewModelConfiguration value)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.RepositoryConfigurationMangmentEditById(value, Userid);
               
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/ConfigurationManagement");
        }
        public void Delete(Guid Id)
        {
            var Result = rep.DeleteConfiguration(Id);

            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ConfigurationManagement");


            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/ConfigurationManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ConfigurationManagement");
            }

           

        }
        public void ChangeStatus(Guid id)
        {
            var Result = rep.ChangeConfigurationStatus(id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ConfigurationManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ConfigurationManagement");
            }

        }
    }
}
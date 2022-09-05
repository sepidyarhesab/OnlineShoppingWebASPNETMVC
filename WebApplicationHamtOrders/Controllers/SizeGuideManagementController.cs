using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using OrdersInventory.Repository.Inventory;
using OrdersInventory.ViewModels.Inventory;
using SepidyarHesabExtensions.Extentions;

namespace WebApplicationHamtOrders.Controllers
{
    public class SizeGuideManagementController : Controller
    {
        // GET: SizeGuideManagement
        public ActionResult Index()
        {
            var result = RepProductsSizeGuide.RepositoryAdminProductSizeGuideList();
            return View(result);
        }

        //End-----------------------------
        //Post: Search
        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepProductsSizeGuide.RepositoryAdminProductSizeGuideSearch(search);
                return View(result);
            }
            else
            {
                var result = RepProductsSizeGuide.RepositoryAdminProductSizeGuideList();
                return View(result);
            }
        }
        //End---------------------------------
        //Repository Add New SizeGuide
        public ActionResult Add()
        {
            return View();
        }

        public void Generate(VMProductsSizeGuides.ViewModelProductSizeGuide value)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepProductsSizeGuide.Add(value, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] =
                    IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SizeGuideManagement/Index");
            }
        }
        //End------------------------------------------------
        //Repository Delete SizeGuide
        public void DeleteSizeGuide(Guid id)
        {
            var result = RepProductsSizeGuide.DeleteSizeGuideCategory(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SizeGuideManagement");
            }
            else if (result.Contains("True"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/SizeGuideManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SizeGuideManagement");
            }
        }
        //End---------------------------------
        //ChangeStatus SizeGuide
        public void ChangeStatusSizeGuide(Guid id)
        {
            var result = RepProductsSizeGuide.ChangeStatusProductSizeGuide(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SizeGuideManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SizeGuideManagement");
            }
        }
        //End-------------------------------------
        //Open Edit page for Product SizeGuide and Update it
        public ActionResult EditProductSizeGuide(Guid Id)
        {
            var result = RepProductsSizeGuide.EditSizeGuide(Id);
            return View(result);
                
        }
        public void UpdateProductSizeGuide(Guid Id, string Sort, string PrimaryTitle, string SecondaryTitle,Guid CategoriesRef)
        {
            var db = new Orders_Entities();
            var query = db.Table_Product_SizeGuide.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                query.PrimaryTitle = PrimaryTitle;
                query.SecondaryTitle = SecondaryTitle;
                query.Sort = int.Parse(Sort);
                query.ModifierDate = DateTime.Now;
                query.CategoryRef=CategoriesRef;
                query.Version++;
                db.SaveChanges();
            }
            Response.Redirect("Index");
        }
        //End--------------------------------------
        // <<<<<<<<<<<<Table_Product_SizeGuideValues Controllers>>>>>>>>>>>>>
        // GET: SizeGuideManagement
        public ActionResult IndexProductSizeGuideValues(Guid id)
        {
            Session["SizeGuideId"] = id;
            var result = RepProductsSizeGuide.RepositoryAdminProductSizeGuideValuesList(id);
            if (result != null)
            {
                return View(result);
            }
            return View(result);
        }

        //End-----------------------------
        //Post: Search
        [HttpPost]
        public ActionResult IndexProductSizeGuideValues(Guid id,string search)
        {
            Session["SizeGuideId"] = id;
            if (search != "")
            {
                var result = RepProductsSizeGuide.RepositoryAdminProductSizeGuideValuesSearch(search);
                return View(result);
            }
            else
            {
                var result = RepProductsSizeGuide.RepositoryAdminProductSizeGuideValuesList(id);
                return View(result);
            }
        }
        //End---------------------------------
        //ChangeStatus SizeGuideValues
        public void ChangeStatusSizeGuideValue(Guid id)
        {
            var result = RepProductsSizeGuide.ChangeStatusProductSizeGuideValues(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SizeGuideManagement/IndexProductSizeGuideValues/"+ Session["SizeGuideId"]);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SizeGuideManagement/IndexProductSizeGuideValues/"+ Session["SizeGuideId"]);
            }
        }
        //End------------------------------------------------
        //Repository Delete SizeGuideValues
        public void DeleteSizeGuideValue(Guid id)
        {
            var result = RepProductsSizeGuide.DeleteSizeGuideValues(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SizeGuideManagement/IndexProductSizeGuideValues/"+Session["SizeGuideId"]);
            }
            else if (result.Contains("True"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/SizeGuideManagement/IndexProductSizeGuideValues/"+ Session["SizeGuideId"]);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SizeGuideManagement/IndexProductSizeGuideValues/"+ Session["SizeGuideId"]);
            }
        }
        //End---------------------------------
        //Repository Add New row in Table SizeGuideValues
        public ActionResult AddNewSizeGuideValues()
        {
            return View();
        }

        public void GenerateSizeGuideValues(VMProductsSizeGuides.ViewModelProductSizeValuesGuide value)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepProductsSizeGuide.AddSizeGuideValues(value, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] =
                    IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SizeGuideManagement/IndexProductSizeGuideValues/"+ Session["SizeGuideId"]);
            }
        }
        //End-------------------------------------
        //Open Edit page for property of Product SizeGuide and Update it
        public ActionResult EditProductSizeGuideValue(Guid Id)
        {
            var result = RepProductsSizeGuide.EditSizeGuideValues(Id);
            return View(result);

        }
        public void UpdateProductSizeGuideValue(Guid Id, string Sort, string PropertyTitle, string SecondaryTitle,string SizeValue, Guid SizeGuideRef)
        {
            var db = new Orders_Entities();
            var query = db.Table_Product_SizeGuideValues.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                query.PropertyTitle = PropertyTitle;
                query.SecondaryTitle = SecondaryTitle;
                query.Sort = int.Parse(Sort);
                query.ModifierDate = DateTime.Now;
                query.SizeGuideRef = SizeGuideRef;
                query.SizeValue = SizeValue;
                query.Version++;
                db.SaveChanges();
            }
            Response.Redirect("/SizeGuideManagement/IndexProductSizeGuideValues/" + Session["SizeGuideId"]);
        }

    }
}
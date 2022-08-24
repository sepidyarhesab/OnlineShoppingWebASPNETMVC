using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Extentions;



namespace WebApplicationOrders.Controllers
{
    public class AdminCategoriesController : Controller
    {
        // GET: AdminCategories
        public ActionResult Index()
        {
            var query = RepAdminCategories.RepositoryAdminCategoriesManagement();
            return View(query);
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepAdminCategories.RepositoryAdminCategoriesManagementSearch(search);
                return View(result);
            }
            else
            {
                var query = RepAdminCategories.RepositoryAdminCategoriesManagement();
                return View(query);
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(Guid? Id)
        {
            if (Id != null)
            {
                var result = RepAdminCategories.Edit(Id);
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
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                return View("Index");
            }
        }

        public void Update(Guid Id, string Sort, string PrimaryTitle)
        {
            var db = new Orders_Entities();
            var query = db.Table_MenuAdmin_Category.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                query.PrimaryTitle = PrimaryTitle;
                query.Sort = int.Parse(Sort);
                query.ModifireDate = DateTime.Now;
                query.Version++;
                db.SaveChanges();
            }
            Response.Redirect("Index");
        }

        public void Generate(VmAdminCategories.VmAdminCategoriesManagement value)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepAdminCategories.Add(value, Userid);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] =
                    IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/AdminCategories/Index");
            }
        }

        public void ChangeStatus(Guid id)
        {
            var result = RepAdminCategories.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/AdminCategories");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/AdminCategories");
            }
        }

        public void Delete(Guid id)
        {
            var result = RepAdminCategories.Delete(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/AdminCategories");
            }
            else if (result.Contains("True"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/AdminCategories");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/AdminCategories");
            }
        }

    }
}
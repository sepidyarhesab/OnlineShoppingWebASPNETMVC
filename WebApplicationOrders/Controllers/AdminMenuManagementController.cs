using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Extentions;
using WebApplicationOrders.Models.Database.OrderDatabase;


namespace WebApplicationOrders.Controllers
{
    public class AdminMenuManagementController : Controller
    {
        // GET: AdminMenuManagement
        public ActionResult Index()
        {
            var query = RepMenuAdmin.RepAdminMenuManagements();
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepMenuAdmin.RepAdminMenuManagementsSearch(search);
                return View(result);
            }
            else
            {
                var query = RepMenuAdmin.RepAdminMenuManagements();
                return View(query);
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        public void Generate(VmMenuAdmin.VmMenuAdminManagement value,HttpPostedFileBase FileName)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            var result = RepMenuAdmin.Add(value, Userid,FileName);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "خطا در ایجاد سطر به دلیل : " + result);
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/AdminMenuManagement/Index");
            }
        }

        public void ChangeStatus(Guid id)
        {
            var result = RepMenuAdmin.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/AdminMenuManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/AdminMenuManagement");
            }

        }


        public void Delete(Guid id)
        {
            var result = RepMenuAdmin.Delete(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/AdminMenuManagement");
            }
            else if (result.Contains("True"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/AdminMenuManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/AdminMenuManagement");
            }


        }

        public ActionResult Edit(Guid? Id)
        {
            if (Id != null)
            {
                var result = RepMenuAdmin.Edit(Id);
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

        public void Update(Guid Id, string Sort, string PrimaryTitle, string SecondaryTitle,
            string TertiaryTitle, string Url, Guid CategoryRef)
        {
            var db = new Orders_Entities();
            var query = db.Table_MenuAdmin.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                query.PrimaryTitle = PrimaryTitle;
                query.Sort = int.Parse(Sort);
                query.ModifireDate = DateTime.Now;
                query.Version++;
                query.Url = Url;
                query.TertiaryTitle = TertiaryTitle;
                query.SecondaryTitle = SecondaryTitle;
                query.CategoryRef = CategoryRef;
                db.SaveChanges();
            }
            Response.Redirect("Index");
        }
    }
    }

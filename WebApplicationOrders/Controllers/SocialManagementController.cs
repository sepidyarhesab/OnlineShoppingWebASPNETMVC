using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OrdersDatabase.Models;
using OrdersGeneral.Repository.General;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;



namespace WebApplicationOrders.Controllers
{
    public class SocialManagementController : Controller
    {
        // GET: SocialManagement
        public ActionResult Index()
        {
            var query = RepSocialMedia.RepositorySocialMediaManagement();
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                var result = RepSocialMedia.RepositorySocialMediaManagementSearch(search);
                return View(result);
            }
            else
            {
                var query = RepSocialMedia.RepositorySocialMediaManagement();
                return View(query);
            }
        }


        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string primaryTitle, string icon, string url)
        {
            var db = new Orders_Entities();
            var query = db.Table_SocialMedia.ToList().Exists(c => c.Url == url);
            if (!query)
            {
                var queryAdd = db.Table_SocialMedia.Add(new Table_SocialMedia()
                {
                    Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_SocialMedia"),
                    PrimaryTitle = primaryTitle,
                    Class = icon,
                    IsDelete = false,
                    IsOk = false,
                    CreatorDate = DateTime.Now,
                    CreatorRef = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    ModifierRef = Guid.NewGuid(),
                    ModifierDate  = DateTime.Now,
                    Sort = 1,
                    Version = 1,
                });
                db.Table_SocialMedia.Add(queryAdd);
                db.SaveChanges();
            }

            return View();
        }


        public ActionResult ChangeStatus(Guid id)
        {
            var result = RepSocialMedia.ChangeStatus(id);
            if (result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/SocialManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SocialManagement");
            }
            return View();
        }

        public void Delete(Guid Id)
        {
            var result = RepSocialMedia.DeleteSocial(Id);
            if (result.Contains("Error"))
            {

                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else if (result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/SocialManagement");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/SocialManagement");
            }
        }
        

        public void Generate(VmSocialMedia.VmSocialMediaManagement values)
        {


            if (values.PrimaryTitle != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = RepSocialMedia.AddNewRow(values, Userid);
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





            Response.Redirect("/SocialManagement/Index");
        }

        public ActionResult Edit(Guid? Id)
        {
            if (Id != null)
            {
                var query = RepSocialMedia.EditSocial(Id);
                if (query != null)
                {
                    return View(query);
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

        [HttpPost]
        public void Update(Guid Id, string Sort, string PrimaryTitle,  string Url, string Classs, string Value)
        {
            var db = new Orders_Entities();
            var query = db.Table_SocialMedia.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                query.PrimaryTitle = PrimaryTitle;
                query.Sort = int.Parse(Sort);
                query.ModifierDate = DateTime.Now;
                query.Version++;
                query.Url = Url;
                query.Class = Classs;
                query.SecondaryTitle = Value;
                db.SaveChanges();
            }
            Response.Redirect("Index");
        }
        
    }
        


  
}
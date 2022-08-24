using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersOrders.Repository.Orders;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;


namespace WebApplicationNewOrders.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {


            return View();


        }
        public ActionResult Factor()
        {


            return View();


        }

        public ActionResult Factors(string id)
        {
            Session["PicturesId"] = id;
            return View("SubmitPicture");
        }


        public ActionResult SubmitPicture()
        {


            return View();
        }


        [HttpPost]
        public ActionResult SubmitPicture(HttpPostedFileBase file)
        {
            if (Session["PicturesId"] != null)
            {
                var db = new Orders_Entities();
                var id = Guid.Parse(Session["PicturesId"].ToString());
                var filename = "Default";
                var fileExtention = "png";
                var time = DateTime.Now.Ticks.ToString();
                var code = "SP-" + time;
                var filelenght = 200;
                if (file != null)
                {
                    var userRef = Guid.NewGuid();
                    filelenght = file.ContentLength;
                    filename = "Factor_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainFactor + filename + fileExtention);
                    file.SaveAs(pathCombine);
                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload
                    {
                        Id = Guid.NewGuid(),
                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                        Tables = "Table_Product",
                        Schemas = "General",
                        Ref = id,
                        FileExtensions = fileExtention,
                        FileLenght = filelenght,
                        FileUniqeName = filename + fileExtention,
                        Sort = 1,
                        IsDelete = false,
                        FileName = filename,
                        Version = 1,
                        CreatorDate = DateTime.Now,
                        CreatorRef = userRef,
                        ModifierRef = userRef,
                        ModifierDate = DateTime.Now,
                        IsOk = true,
                        IsMain = true,
                    });

                    db.SaveChanges();

                }
            }
            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            Response.Redirect("/Dashboard/Factor");
            return View();

        }

        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditProfile(string name, string family, string phone, string identification,
            string postalcode, string address, bool? sex)
        {
            var Userid = Guid.Parse(User.Identity.Name);
            if (User.Identity.IsAuthenticated)
            {
                if (Userid != null)
                {
                    var result = RepAccountDashboard.RepositoryUpdateUsers(name, family, phone, identification,
                        postalcode, address, sex, Userid);
                }
            }

            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            return View("EditProfile");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string id, string olpassword, string newpassword, string trynewpassword)
        {
            var db = new Orders_Entities();
            var idd = Guid.Parse(id);
            var query = db.Table_User.AsNoTracking().FirstOrDefault(c => c.Id == idd);
            if (query != null)
            {
                if (query.Password == olpassword)
                {
                    if (newpassword == trynewpassword)
                    {
                        query.Password = newpassword;
                        db.SaveChanges();
                    }
                }
            }
            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            return View("Index");
        }       
        
        
        [HttpPost]
        public ActionResult ChangeProfile(string id,HttpPostedFileBase file)
        {
            var db = new Orders_Entities();
            var idd = Guid.Parse(id);
            var query = db.Table_User.FirstOrDefault(c => c.Id == idd);
            if (query != null)
            {
                var filename = "Default";
                var fileExtention = "png";
                var time = DateTime.Now.Ticks.ToString();
                var code = "SP-" + time;
                var filelenght = 200;
                if (file != null)
                {
                    var userRef = Guid.NewGuid();
                    filelenght = file.ContentLength;
                    filename = "User_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProfile + filename + fileExtention);
                    file.SaveAs(pathCombine);
                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload
                    {
                        Id = Guid.NewGuid(),
                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                        Tables = "Table_User",
                        Schemas = "Account",
                        Ref = idd,
                        FileExtensions = fileExtention,
                        FileLenght = filelenght,
                        FileUniqeName = filename + fileExtention,
                        Sort = 1,
                        IsDelete = false,
                        FileName = filename,
                        Version = 1,
                        CreatorDate = DateTime.Now,
                        CreatorRef = userRef,
                        ModifierRef = userRef,
                        ModifierDate = DateTime.Now,
                        IsOk = true,
                        IsMain = true,
                    });

                    db.SaveChanges();

                }
            }
            TempData["JavaScriptFunction"] = IziToast.Success("عملیات با موفقیت انجام شد", "عملیات با موفقیت انجام شد");
            return View("Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using OrdersInventory.Repository.Inventory;
using OrdersInventory.ViewModels.Inventory;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;
using OrdersDatabase.Models;
using WebApplicationOrders.Extensions;



namespace WebApplicationOrders.Controllers
{
    public class ProductManagmentController : Controller
    {
        // GET: ProductManagment
        public RepProducts rep = new RepProducts();

        public ActionResult Index()
        {
            if (Session["SearchProduct"] != null)
            {
                string search = Session["SearchProduct"].ToString();
                var result = rep.RepositoryMainProductsMangmentSearch(search);
                return View(result);
            }
            else
            {
                var query = rep.RepositoryMainProductsMangment();
                return View(query);
            }
        }

        public ActionResult Reload()
        {
            Session["SearchProduct"] = null;
            var query = rep.RepositoryMainProductsMangment();
            return View("Index", query);
        }
        [HttpPost]
        public ActionResult Index(string search)
        {
            if (search != "")
            {
                Session["SearchProduct"] = search;
                var result = rep.RepositoryMainProductsMangmentSearch(search);
                return View(result);
            }
            else
            {
                if (Session["SearchProduct"] != null)
                {
                    Session["SearchProduct"] = search;
                    var result = rep.RepositoryMainProductsMangmentSearch(search);
                    return View(result);
                }
                else
                {
                    var query = rep.RepositoryMainProductsMangment();
                    return View(query);
                }
            }
        }

        public ActionResult Sliders()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
        public void Delete(Guid Id)
        {
            var Result = rep.Delete(Id);

            if (Result.Contains("Error"))
            {

                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/ProductManagment");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ProductManagment");
            }



        }

        #region add
        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public void Generate(VMProduct.VmMainProductMangement values, HttpPostedFileBase FileName)
        {
            if (FileName != null)
            {
                if (values.PrimaryTitle != "")
                {
                    var Userid = Guid.Parse(User.Identity.Name);
                    var result = rep.AddNewRow(values, FileName, Userid);
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
            Response.Redirect("/ProductManagment/Index");
        }
        #endregion

        
        #region changestatus
        public void ChangeStatus(Guid Id)
        {
            var Result = rep.ChangeStatus(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ProductManagment");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ProductManagment");
            }

        }

        public void ChangeStatusIsMain(Guid id)
        {
            rep.ChangeStatusPicturesIsMain(id);
            if (Session["PicturesId"] != null)
            {
                var idd = Session["PicturesId"].ToString();
                Response.Redirect("/ProductManagment/Pictures/" + idd);
            }
        }

        #endregion

        
        #region Picture
        public ActionResult Pictures(Guid id)
        {
            Session["PicturesId"] = id;
            List<VMProduct.VmMainProductMangement> result = rep.RepositoryGetFullPictures(id);
            if (result.Count > 0)
            {
                return View(result);
            }
            return View();
        }
        public ActionResult PicturesAdd()
        {
            return View();
        }
        public void DeletePictures(Guid id)
        {

            rep.DeletePicturesIsMain(id);
            if (Session["PicturesId"] != null)
            {
                var idd = Session["PicturesId"].ToString();
                Response.Redirect("/ProductManagment/Pictures/" + idd);
            }
        }
        public void ChangeStatusPictures(Guid id)
        {
            rep.ChangeStatusPictures(id);
            if (Session["PicturesId"] != null)
            {
                var idd = Session["PicturesId"].ToString();
                Response.Redirect("/ProductManagment/Pictures/" + idd);
            }

        }

        [HttpPost]
        public ActionResult GeneratePictues(HttpPostedFileBase FileName)
        {
            if (Session["PicturesId"] != null)
            {
                var id = Session["PicturesId"].ToString();
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.PictureAdd(FileName, Guid.Parse(id), Userid);
                if (result.Contains("Success"))
                {
                    Response.Redirect("/ProductManagment/Pictures/" + id);
                }

            }

            return View("Index");
        }
        #endregion


        #region Property
        public ActionResult Property(Guid id)
        {
            Session["PropertyId"] = id;
            List<VMProduct.ProductProperty> result = rep.RepositoryProperty(id);

            return View(result);


        }

        public ActionResult PropertyAdd()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public void GenerateProperty(VMProduct.ProductProperty values)
        {
            if (values.Id != "")
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.AddNewRowProperty(values);
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

            Response.Redirect("/ProductManagment/Property/" + values.Id);
        }
        public void DeleteProperty(Guid Id)
        {
            var Result = rep.DeleteProperty(Id);

            if (Result.Contains("success"))
            {

                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ProductManagment/");

            }

            if (Result.Contains("true"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                Response.Redirect("/ProductManagment/");

            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ProductManagment/");
            }



        }
        public void ChangeStatusProperty(Guid Id)
        {
            var Result = rep.ChangeStatusProperty(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                Response.Redirect("/ProductManagment/");
            }
            else
            {
                TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                Response.Redirect("/ProductManagment/");
            }

        }
        public void ChangeStatusIsMainProperty(Guid id)
        {
            rep.ChangeStatusIsMainProperty(id);
            if (Session["PropertyId"] != null)
            {
                var idd = Session["PropertyId"].ToString();
                Response.Redirect("/ProductManagment/Property/" + idd);
            }
        }

        #endregion


        #region Edit
        public ActionResult Edit(Guid id)
        {

            if (User.Identity.IsAuthenticated)
            {
                var result = rep.RepositoryMainProductsMangmentByid(id);
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
        [ValidateInput(false)]
        public void EditRow(VMProduct.VmMainProductMangement value)
        {
            var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.RepositoryMainProductManagemenetEditById(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
           
            Response.Redirect("/ProductManagment");
        }




        #endregion


        #region ColorSize
        public ActionResult ColorSize(Guid id)
        {
            Session["ProductId"] = id;
            var query = rep.RepositoryColorSizeList(id);
            return View(query);
        }


        //[HttpPost]
        //public ActionResult ColorSize(Guid id, string search)
        //{
        //    Session["ProductId"] = id;
        //    if (search != "")
        //    {
        //        var result = rep.RepositoryColorSizeListsearch(id ,search);
        //        return View(result);
        //    }
        //    else
        //    {
        //        var query = rep.RepositoryColorSizeList(id);
        //        return View(query);
        //    }
        //}


        [HttpPost]
        public void AddColorSize(Guid id, Guid ColorRef, Guid SizeRef, decimal Quantity, decimal Fee, int Sort)
        {
            var db = new Orders_Entities();
            var query = db.Table_Product_Summary.ToList().Exists(c => c.ProductRef == id && c.ColorRef == ColorRef && c.SizeRef == SizeRef && Quantity == Quantity && Fee == Fee);
            if (!query)
            {
                var stock = Guid.Parse("15bf9127-9f7b-4df6-b99a-19697ade04b4");
                var saletype = Guid.Parse("46d60381-a6f8-46c0-98a4-4090d62f0b91");
                var querycolor = db.Table_Product_Summary.Add(new Table_Product_Summary()
                {
                    Code = SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary"),
                    ProductRef = id,
                    ColorRef = ColorRef,
                    SizeRef = SizeRef,
                    Fee = Fee,
                    Quantity = Quantity,
                    IsDelete = false,
                    IsOk = false,
                    CreatorDate = DateTime.Now,
                    CreatorRef = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    ModifierRef = Guid.NewGuid(),
                    ModifierDate = DateTime.Now,
                    Sort = Sort,
                    Version = 1,
                    SaleRef = saletype,
                    StockRef = stock,
                });
                db.Table_Product_Summary.Add(querycolor);
                db.SaveChanges();
            }
            if (Session["ProductId"] != null)
            {
                Response.Redirect("/ProductManagment/ColorSize/" + Session["ProductId"]);
            }
            else
            {
                Response.Redirect("/ProductManagment/");
            }
            
        }


        public ActionResult EditColorSize(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = rep.RepositoryEditListColorSize(id);
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

        public void EditColorSizeList(VMProduct.VmMainProductMangement value)
        {
            if (ModelState.IsValid)
            {
                var Userid = Guid.Parse(User.Identity.Name);
                var result = rep.RepositoryEditColorSize(value, Userid);
                if (result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                }
                else
                {
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                }
            }
            Response.Redirect("/ProductManagment/ColorSize/" + Session["ProductId"]);
        }

        public void DeleteSummary(Guid id)
        {
            var Result = rep.DeleteSummary(id);

            if (Result.Contains("Error"))
            {

                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
            }
            else if (Result.Contains("true"))
            {
                if (Session["ProductId"] != null)
                {
                    var pid = Session["ProductId"].ToString();
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "لطفا غیر فعال کنید");
                    Response.Redirect("/ProductManagment/ColorSize/" + pid);
                }
                else
                {
                    Response.Redirect("/ProductManagment/");
                }
            }
            else
            {
                if (Session["ProductId"] != null)
                {
                    var pid = Session["ProductId"].ToString();
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                    Response.Redirect("/ProductManagment/ColorSize/" + pid);
                }
                else
                {
                    Response.Redirect("/ProductManagment/");
                }
            }



        }
        public void ChangeSummary(Guid Id)
        {
            var Result = rep.ChangeSummary(Id);


            if (Result.Contains("Error"))
            {
                TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                if (Session["ProductId"] != null)
                {
                    var pid = Session["ProductId"].ToString();
                    Response.Redirect("/ProductManagment/ColorSize/" + pid);
                }
                else
                {
                    Response.Redirect("/ProductManagment");
                }
            }
            else
            {
                if (Session["ProductId"] != null)
                {
                    var pid = Session["ProductId"].ToString();
                    TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                    Response.Redirect("/ProductManagment/ColorSize/" + pid);
                }
                else
                {
                    Response.Redirect("/ProductManagment");
                }

            }

        }

        public void IsMain(Guid Id)
        {
            if (Session["ProductId"] != null)
            {
                var productid = Guid.Parse(Session["ProductId"].ToString());
                var Result = rep.IsMain(Id, productid);


                if (Result.Contains("Error"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "نرم افزار خطا داده است");
                    if (Session["ProductId"] != null)
                    {
                        var pid = Session["ProductId"].ToString();
                        Response.Redirect("/ProductManagment/ColorSize/" + pid);
                    }
                    else
                    {
                        Response.Redirect("/ProductManagment");
                    }
                }
                else if (Result.Contains("Exist"))
                {
                    TempData["JavaScriptFunction"] = IziToast.Error("خطایی رخ داده است", "شما تنها اجازه دارید یک ویژگی را اصلی کنید");
                    if (Session["ProductId"] != null)
                    {
                        var pid = Session["ProductId"].ToString();
                        Response.Redirect("/ProductManagment/ColorSize/" + pid);
                    }
                    else
                    {
                        Response.Redirect("/ProductManagment");
                    }
                }
                else
                {
                    if (Session["ProductId"] != null)
                    {
                        var pid = Session["ProductId"].ToString();
                        TempData["JavaScriptFunction"] = IziToast.Success("عملیات موفقیت امیز بود", "عملیات موفقیت امیز بود");
                        Response.Redirect("/ProductManagment/ColorSize/" + pid);
                    }
                    else
                    {
                        Response.Redirect("/ProductManagment");
                    }

                }
            }
            else
            {
                Response.Redirect("/ProductManagment");
            }

        }

        #endregion

        #region Import & Export
        public FileResult ExportToExcel()
        {
            DataTable dt = new DataTable("Sheet1");
            //gv.DataSource = rep.RepositoryMainProductsMangmentExcelReport();
            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("CodeProduct"),
                                                     new DataColumn("PrimaryTitle"),
                                                     new DataColumn("Quantity"),
                                                     new DataColumn("Fee"),
                                                     new DataColumn("Color"),
                                                     new DataColumn("Size"),
                                                     new DataColumn("ProductRef"),
                                                     new DataColumn("ColorRef"),
                                                     new DataColumn("SizeRef")
                                                     });

            var insuranceCertificate = rep.RepositoryMainProductsMangmentExcelReport();

            foreach (var insurance in insuranceCertificate)
            {
                dt.Rows.Add(insurance.CodeProduct, insurance.PrimaryTitle, insurance.Quantity, insurance.Fee, insurance.Color, insurance.Size, insurance.ProductRef, insurance.ColorRef, insurance.SizeRef);
            }

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary") + ".xlsx");
                }
            }
        }


        [HttpPost]
        public void ImportFromExcel(HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                LogWriter.LoggerImport("Model Valid", DateTime.Now.ToString(), "746");
                if (postedFile != null && postedFile.ContentLength > (1024 * 1024 * 50))  // 50MB limit  
                {
                    LogWriter.LoggerImport("Model Poted File Lenght : " + postedFile.ContentLength , DateTime.Now.ToString(), "746");
                    ModelState.AddModelError("postedFile", "Your file is to large. Maximum size allowed is 50MB !");
                }

                else
                {
                    string filePath = string.Empty;
                    string path = Server.MapPath("~/Static/Content/Excel/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //For Excel 97-03.  
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //For Excel 07 and above.  
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }

                    try
                    {
                        DataTable dt = new DataTable();
                        conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.  
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.  
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }

                        try
                        {
                            var db = new Orders_Entities();
                            foreach (DataRow row in dt.Rows)
                            {
                                string codeProduct = row["CodeProduct"].ToString();
                                string title = row["PrimaryTitle"].ToString();
                                decimal quantity = decimal.Parse(row["Quantity"].ToString());
                                decimal fee = decimal.Parse(row["Fee"].ToString());
                                Guid productRef = Guid.Parse(row["ProductRef"].ToString());
                                Guid colorRef = Guid.Parse(row["ColorRef"].ToString());
                                Guid sizeRef = Guid.Parse(row["SizeRef"].ToString());

                                var querySummary =
                                    db.Table_Product_Summary.FirstOrDefault(c => c.ProductRef == productRef && c.ColorRef == colorRef && c.SizeRef == sizeRef);
                                if (querySummary != null)
                                {
                                    querySummary.Quantity = quantity;
                                    querySummary.Fee = fee;
                                    querySummary.SizeRef = sizeRef;
                                    querySummary.ColorRef = colorRef;
                                    db.SaveChanges();
                                    var queryProduct =
                                        db.Table_Product.FirstOrDefault(c => c.Code == codeProduct);
                                    if (queryProduct != null)
                                    {
                                        queryProduct.PrimaryTitle = title;
                                        db.SaveChanges();
                                    }
                                    LogWriter.LoggerImport("Modified Row = : " + title + "_" + codeProduct, DateTime.Now.ToString(), "746");
                                }
                                else
                                {
                                    var stock = Guid.Parse("15bf9127-9f7b-4df6-b99a-19697ade04b4");
                                    var sale = Guid.Parse("46d60381-a6f8-46c0-98a4-4090d62f0b91");
                                    var queryAddSummary = db.Table_Product_Summary.Add(new Table_Product_Summary
                                    {
                                        Id = Guid.NewGuid(),
                                        ProductRef = productRef,
                                        Code = SepidyarHesabCodeGenerator.GenerateCode("", ""),
                                        Quantity = quantity,
                                        Fee = fee,
                                        IsOk = true,
                                        CreatorDate = DateTime.Now,
                                        Sort = 1,
                                        IsDelete = false,
                                        IsMain = true,
                                        Version = 1,
                                        CreatorRef = Guid.NewGuid(),
                                        ModifierRef = Guid.NewGuid(),
                                        ModifierDate = DateTime.Now,
                                        ColorRef = colorRef,
                                        SaleRef = sale,
                                        SizeRef = sizeRef,
                                        StockRef = stock,
                                    });
                                    db.Table_Product_Summary.Add(queryAddSummary);
                                    db.SaveChanges();
                                    LogWriter.LoggerImport("Add Row = : " + title + "_" + codeProduct, DateTime.Now.ToString(), "746");

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LogWriter.LoggerImport("Error : " + e.Message + "  -  :", DateTime.Now.ToString(), "746");
                            //return Json("error" + e.Message);
                        }




                        //conString = ConfigurationManager.ConnectionStrings["Orders_Entities"].ConnectionString;
                        //using (SqlConnection con = new SqlConnection(conString))
                        //{
                        //    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        //    {
                        //        //Set the database table name.  
                        //        sqlBulkCopy.DestinationTableName = "Table_Product";
                        //        con.Open();
                        //        sqlBulkCopy.WriteToServer(dt);
                        //        con.Close();
                        //        return Json("File uploaded successfully");
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        LogWriter.LoggerImport("Error : " + e.Message + "  -  :", DateTime.Now.ToString(), "746");
                        //return Json("error" + e.Message);
                    }
                    //return RedirectToAction("Index");  
                }
            }
            else
            {
                    LogWriter.LoggerImport("Model Not Vliad  " , DateTime.Now.ToString(), "746");
            }
            //return View(postedFile);  
            //return Json("Success");
            Response.Redirect("/ProductManagment");
        }
        #endregion
    }


}



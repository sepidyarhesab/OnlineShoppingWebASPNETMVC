using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersInventory.ViewModels.Inventory;

namespace OrdersInventory.Repository.Inventory
{
    public class RepProducts
    {

        //public  bool CheckProductDiscount(Guid id)
        //{
        //   
        //    var query = db.Table_Discount_Product_Selection.FirstOrDefault();
        //    if (query.ProductRef == id)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        //public  bool CheckCategoryDiscount(Guid? catRef)
        //{
        //   
        //    var query = db.Table_DiscountOnProducts.FirstOrDefault();
        //    if (query.CategoriesRef == catRef)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public  int GetDiscountPercentByCatRef(Guid? catRef)
        //{
        //   
        //    var query = db.Table_DiscountOnProducts.FirstOrDefault();
        //    if (query.CategoriesRef == catRef)
        //    {
        //        return query.Discount;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        //public  int GetDiscountPercentByProductRef(Guid Id)
        //{
        //   
        //    var Selectquery = db.Table_Discount_Product_Selection.FirstOrDefault(c => c.ProductRef == Id);

        //    var query = db.Table_DiscountOnProducts.FirstOrDefault();
        //    if (query.Id == Selectquery.DiscountRef)
        //    {
        //        return query.Discount;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        public Orders_Entities db = new Orders_Entities();

        public int UseDiscount(int sum, string code)
        {
            var discount = db.Table_Price_DisCount.SingleOrDefault(c => c.DiscountCode == code);
            if (discount == null)
                return 0;
            if (discount.StartDate != null && discount.StartDate < DateTime.Now)
                return 0;
            if (discount.EndDate != null && discount.EndDate >= DateTime.Now)
                return 0;
            if (discount.UsableCount < 1)
                return 0;
            int precent = (sum * discount.DiscountPrecent) / 100;
            if (discount.UsableCount != 0)
            {
                discount.UsableCount -= 1;
            }

            db.SaveChanges();
            return precent;
        }

        public string DeleteProperty(Guid id)
        {
            try
            {
                var query = db.Table_Product_Property.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";
                            }
                        case false:
                            {
                                db.Table_Product_Property.Remove(query);
                                db.SaveChanges();
                                return "success";
                            }
                    }
                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;
            }






        }

        public string AddNewRowProperty(VMProduct.ProductProperty values)
        {
            try
            {

                var userRef = Guid.Parse("5549aa2b-763c-41f1-b721-1cb9a8d7d066");
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var Ref = Guid.Parse(values.Id);
                var query = db.Table_Product_Property.Add(new Table_Product_Property()
                {
                    Id = id,
                    CreatorDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = true,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    Keys = values.Key,
                    Value = values.Value,
                    Entity = "Property",
                    IsMain = true,
                    ModifierDate = DateTime.Now,
                    PrimaryTitle = "",
                    Ref = Ref,
                    SecondaryTitle = "",
                });
                db.Table_Product_Property.Add(query);
                db.SaveChanges();



                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string RepositoryMainColorManagemenetEditById(VMProduct.ViewModelProductColor values, Guid UserId)
        {
            try
            {

                var userRef = UserId;
                var query = db.Table_Product_Color.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.ColorCode = values.ColorCode;


                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string RepositoryMainProductListManagemenetEditById(VMProduct.ViewModelProductPropertyList values, Guid UserId)
        {

            var userRef = UserId;
            var query = db.Table_Product_PropertyList.FirstOrDefault(c => c.Id == values.Id);
            if (query != null)
            {
                query.Id = values.Id;
                query.PrimaryTitle = values.PrimaryTitle;
                query.SecondaryTitle = values.SecondaryTitle;
                query.ModifierDate = DateTime.Now;
                query.ModifierRef = userRef;
                query.Version++;
                query.ProductRef = values.ProductRef;
                query.ColorRef = values.ColorRef;
                query.SizeRef = values.SizeRef;
                query.Count = values.Count;


                db.SaveChanges();
            }

            return "Success";



        }
        public string RepositoryMainProductManagemenetEditById(VMProduct.VmMainProductMangement values,
             Guid userid)
        {
            try
            {

                var userRef = userid;
                var query = db.Table_Product.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.Url = values.Url;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Note = values.Note;
                    query.Quantity = values.Quantity;
                    query.Fee = values.Fee;
                    query.CategoriesRef = values.CategoriesRef;
                    query.Discount = values.Discount;

                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }



        }

        public string RepositoryConfigurationMangmentEditById(VMProduct.ViewModelConfiguration values, Guid userId)
        {
            try
            {

                var userRef = userId;
                var query = db.Table_Selection_Configuration.FirstOrDefault(c => c.Id == values.Id);
                var catRef = values.CategoriesRef;
                if (values.CategoriesRef == Guid.Empty)
                {
                    catRef = null;
                }

                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.CategoriesRef = catRef;
                    query.Sort = values.Sort;
                    query.Count = values.Count;
                    query.Sort = values.Sort;
                    query.Url = values.Url;


                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string RepositorySizeMangmentEditById(VMProduct.ViewModelProductSize values, Guid UserId)
        {
            try
            {

                var userRef = UserId;
                var query = db.Table_Product_Size.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.SizeTitle = values.SizeTitle;
                    query.Sort = values.Sort;


                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string RepositoryColorMangmentEditById(VMProduct.ViewModelProductColor values)
        {

            try
            {

                var userRef = Guid.Parse("5549aa2b-763c-41f1-b721-1cb9a8d7d066");
                var query = db.Table_Product_Color.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.ColorCode = values.ColorCode;
                    query.Code = values.Code;
                    query.CreatorDate = values.CreatorDate;
                    query.CreatorRef = values.CreatorRef;

                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string RepositoryCategoryMangmentEditById(VMProduct.ViewModelCategoreis values, HttpPostedFileBase FileNameDesktop, Guid userId)
        {
            try
            {

                var userRef = userId;
                var query = db.Table_Product_Category.FirstOrDefault(c => c.Id == values.Id);
                var parentRef = values.ParentRef;

                if (values.ParentRef == Guid.Empty)
                {
                    parentRef = null;
                }

                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.ParentRef = parentRef;
                    query.IsMain = values.IsMain;
                    query.Sort = values.Sort;
                    query.Url = values.Url;

                    db.SaveChanges();

                    if (FileNameDesktop != null)
                    {
                        var filename = "Default";
                        var fileExtention = "png";
                        var time = DateTime.Now.Ticks.ToString();
                        var code = "SP-" + time;
                        var filelenght = 200;
                        var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id).ToList();
                        if (qPics.Count > 0)
                        {
                            foreach (var fileUpload in qPics)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainCategories + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainCategories + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght = FileNameDesktop.ContentLength;
                        filename = "Category_" + SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        fileExtention = Path.GetExtension(FileNameDesktop.FileName);
                        string pathCombine =
                            HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainCategories + filename + fileExtention);
                        FileNameDesktop.SaveAs(pathCombine);
                        //if (qPics != null)
                        //{
                        //    qPics.FileName = filename;
                        //    qPics.FileExtensions = fileExtention;
                        //    qPics.ModifierDate = DateTime.Now;
                        //    db.SaveChanges();
                        //}
                        var queryPic = db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            FileName = filename,
                            IsOk = true,
                            CreatorDate = DateTime.Now,
                            CreatorRef = userRef,
                            FileExtensions = fileExtention,
                            FileLenght = filelenght,
                            IsDelete = false,
                            IsMain = true,
                            LanguageRef = Guid.Empty,
                            ModifierDate = DateTime.Now,
                            ModifierRef = userRef,
                            Path = "",
                            Ref = query.Id,
                            Schemas = "Inventory",
                            Tables = "Table_Product_Category",
                            Sort = 1,
                            Version = 1,
                        });
                        db.Table_File_Upload.Add(queryPic);
                        db.SaveChanges();

                    }

                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string DeleteConfiguration(Guid id)
        {
            try
            {

                var query = db.Table_Selection_Configuration.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                db.Table_Selection_Configuration.Remove(query);
                                db.SaveChanges();
                                return "success";
                            }


                    }



                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }


        }
        public string Delete(Guid id)
        {
            try
            {

                var query = db.Table_Product.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id).ToList();
                                if (qPics.Count > 0)
                                {
                                    foreach (var fileUpload in qPics)
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProduct + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProduct + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        db.Table_File_Upload.Remove(fileUpload);
                                        db.SaveChanges();
                                    }
                                }
                                db.Table_Product.Remove(query);
                                db.SaveChanges();
                                break;
                            }


                    }



                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }






        }

        public string DeletePropertyList(Guid id)
        {
            try
            {

                var query = db.Table_Product_PropertyList.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                db.Table_Product_PropertyList.Remove(query);
                                db.SaveChanges();
                                return "success";
                            }


                    }



                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }






        }

        public string DeleteSummary(Guid id)
        {
            try
            {

                var querysummary = db.Table_Product_Summary.FirstOrDefault(c => c.Id == id);
                if (querysummary != null)
                {
                    switch (querysummary.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                db.Table_Product_Summary.Remove(querysummary);
                                db.SaveChanges();
                                return "success";
                            }
                    }
                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }

        public string DeleteSize(Guid id)
        {
            try
            {

                var query = db.Table_Product_Size.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                db.Table_Product_Size.Remove(query);
                                db.SaveChanges();
                                return "success";
                            }
                    }
                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }

        public string DeleteColor(Guid id)
        {
            try
            {

                var query = db.Table_Product_Color.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                db.Table_Product_Color.Remove(query);
                                db.SaveChanges();
                                return "success";
                            }
                    }
                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }

        public string DeleteDiscount(Guid id)
        {
            try
            {

                var query = db.Table_Price_DisCount.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                db.Table_Price_DisCount.Remove(query);
                                db.SaveChanges();
                                return "success";
                            }
                    }
                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }

        public string DeleteCategory(Guid id)
        {
            try
            {

                var query = db.Table_Product_Category.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "true";

                            }
                        case false:
                            {
                                var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id).ToList();
                                if (qPics.Count > 0)
                                {
                                    foreach (var fileUpload in qPics)
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainCategories + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainCategories + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        db.Table_File_Upload.Remove(fileUpload);
                                        db.SaveChanges();
                                    }
                                }
                                db.Table_Product_Category.Remove(query);
                                db.SaveChanges();
                                break;

                            }

                    }
                }

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }

        public string ChangeSizeStatus(Guid id)
        {

            var Result = db.Table_Product_Size.FirstOrDefault(c => c.Id == id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";
        }

        public string ChangeStatus(Guid Id)
        {

            var Result = db.Table_Product.FirstOrDefault(c => c.Id == Id);
            if (Result != null)
            {
                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }
            }

            db.SaveChanges();
            return "Sucsess";
        }

        public string ChangeSummary(Guid Id)
        {

            var Result = db.Table_Product_Summary.FirstOrDefault(c => c.Id == Id);
            if (Result != null)
            {
                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            Result.Quantity = 0;
                            db.SaveChanges();
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            db.SaveChanges();
                            break;
                        }
                }
            }


            return "Sucsess";
        }

        public string IsMain(Guid Id, Guid productid)
        {

            var check = db.Table_Product_Summary.ToList().Exists(c => c.ProductRef == productid && c.IsMain);
            if (!check)
            {
                var Result = db.Table_Product_Summary.FirstOrDefault(c => c.Id == Id);
                if (Result != null)
                {
                    switch (Result.IsMain)
                    {
                        case true:
                            {
                                Result.IsMain = false;
                                break;
                            }
                        case false:
                            {
                                Result.IsMain = true;
                                break;
                            }
                    }
                }

                db.SaveChanges();
                return "Sucsess";
            }
            else
            {
                var Result = db.Table_Product_Summary.FirstOrDefault(c => c.Id == Id && c.IsMain);
                if (Result != null)
                {
                    switch (Result.IsMain)
                    {
                        case true:
                            {
                                Result.IsMain = false;
                                break;
                            }
                        case false:
                            {
                                Result.IsMain = true;
                                break;
                            }
                    }
                    db.SaveChanges();
                    return "Sucsess";
                }
                else
                {
                    return "Exist";
                }


            }
        }
        public string ChangeConfigurationStatus(Guid id)
        {

            var Result = db.Table_Selection_Configuration.FirstOrDefault(c => c.Id == id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";
        }

        public string ChangeDiscountstatus(Guid id)
        {

            var Result = db.Table_Price_DisCount.FirstOrDefault(c => c.Id == id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";
        }
        public string ChangeColorStatus(Guid Id)
        {

            var Result = db.Table_Product_Color.FirstOrDefault(c => c.Id == Id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";
        }

        public string ChangeCategoryStatus(Guid Id)
        {

            var Result = db.Table_Product_Category.FirstOrDefault(c => c.Id == Id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";

        }

        public string ChangeStatusProperty(Guid Id)
        {

            var Result = db.Table_Product_Property.FirstOrDefault(c => c.Id == Id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";

        }

        public string AddNewProductListRow(VMProduct.ViewModelProductPropertyList values, Guid id, Guid UserId)
        {

            var userRef = UserId;
            var random = new Random();
            var productRef = id;
            var code = "SP-" + DateTime.Now.Ticks;
            var isok = false;

            switch (values.IsOk)
            {
                case true:

                    {
                        isok = true;
                        break;
                    }
            }

            var query = db.Table_Product_PropertyList.Add(new Table_Product_PropertyList()
            {
                Id = Guid.NewGuid(),
                PrimaryTitle = values.PrimaryTitle,
                SecondaryTitle = values.SecondaryTitle,
                CreatorDate = DateTime.Now,
                ModifierDate = DateTime.Now,
                Code = code,
                Version = 1,
                IsOk = isok,
                CreatorRef = userRef,
                ModifierRef = userRef,
                IsDelete = false,
                TertiaryTitle = values.SecondaryTitle,
                SizeRef = values.SizeRef,
                ColorRef = values.ColorRef,
                ProductRef = productRef,
                Count = values.Count,

            });
            db.Table_Product_PropertyList.Add(query);
            db.SaveChanges();



            return "Success";

        }
        public string AddNewRow(VMProduct.VmMainProductMangement values, HttpPostedFileBase file, Guid Userid)
        {
            try
            {

                var userRef = Userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product");
                var isok = false;
                var catRef = values.CategoriesRef;
                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }

                if (values.CategoriesRef == Guid.Empty)
                {
                    catRef = null;
                }

                var query = db.Table_Product.Add(new Table_Product()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    Url = values.Url,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    Click = 1,
                    TertiaryTitle = values.SecondaryTitle,
                    Note = values.Note,
                    CategoriesRef = catRef,
                    Fee = values.Fee,
                    Quantity = values.Quantity,
                    Discount = values.Discount,
                    
                });
                db.Table_Product.Add(query);
                db.SaveChanges();

                var filename = "Default";
                var fileExtention = "png";
                var filelenght = 200;
                if (file != null)
                {
                    var newidpic = Guid.NewGuid();
                    filelenght = file.ContentLength;
                    filename = "Id_" + newidpic + "PId_" + id;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProduct + filename +
                                                           fileExtention);
                    file.SaveAs(pathCombine);
                    var qPics = db.Table_File_Upload.Where(c => c.Ref == id).AsNoTracking().ToList();
                    if (qPics.Count != 0)
                    {
                        foreach (var item in qPics)
                        {
                            var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProduct +
                                                                           item.FileName + item.FileExtensions);
                            File.Delete(filee);
                            db.Table_File_Upload.Remove(item);
                            db.SaveChanges();
                        }

                        var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                        qAddPic.Id = newidpic;
                        qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        qAddPic.Tables = "Table_Product";
                        qAddPic.Schemas = "General";
                        qAddPic.Ref = id;
                        qAddPic.FileExtensions = fileExtention;
                        qAddPic.FileLenght = filelenght;
                        qAddPic.FileUniqeName = filename + fileExtention;
                        qAddPic.Sort = 1;
                        qAddPic.IsDelete = false;
                        qAddPic.FileName = filename;
                        qAddPic.Version = 1;
                        qAddPic.CreatorDate = DateTime.Now;
                        qAddPic.CreatorRef = userRef;
                        qAddPic.ModifierRef = userRef;
                        qAddPic.ModifierDate = DateTime.Now;
                        qAddPic.IsOk = true;
                        qAddPic.IsMain = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                        qAddPic.Id = Guid.NewGuid();
                        qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        qAddPic.Tables = "Table_Product";
                        qAddPic.Schemas = "General";
                        qAddPic.Ref = id;
                        qAddPic.FileExtensions = fileExtention;
                        qAddPic.FileLenght = filelenght;
                        qAddPic.FileUniqeName = filename + fileExtention;
                        qAddPic.Sort = 1;
                        qAddPic.IsDelete = false;
                        qAddPic.FileName = filename;
                        qAddPic.Version = 1;
                        qAddPic.CreatorDate = DateTime.Now;
                        qAddPic.CreatorRef = userRef;
                        qAddPic.ModifierRef = userRef;
                        qAddPic.ModifierDate = DateTime.Now;
                        qAddPic.IsOk = true;
                        qAddPic.IsMain = true;

                        db.SaveChanges();
                    }
                }
                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        public List<VMProduct.VmMainProduct> RepositoryMainProductsWithFilter()
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product.OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProduct
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            Fee = product.Fee,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            CategoriesRef = product.CategoriesRef,

                        };



                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c =>
                                c.IsOk && c.Ref == product.Id && c.IsMain);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }

                        var querySummary =
                            db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsMain && c.IsOk && c.IsMain);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = querySummary.Fee;

                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = product.Fee;
                        }

                        var queryCategories =
                            db.Table_Product_Category.AsNoTracking()
                                .FirstOrDefault(c => c.Id == product.CategoriesRef);
                        if (queryCategories != null)
                        {
                            vm.CategoreisTitle = queryCategories.PrimaryTitle;
                            vm.CategoriesRef = queryCategories.Id;

                        }

                        //var queryDiscount = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.CategoriesRef == vm.CategoriesRef);
                        //if (queryDiscount != null)
                        //{
                        //    vm.Discount = queryDiscount.Discount;
                        //}


                        //var queryProductDiscount = db.Table_Discount_Product_Selection.AsNoTracking().FirstOrDefault(c => c.ProductRef == vm.Id);
                        //if (queryProductDiscount != null)
                        //{
                        //    var queryFindCatRef = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.Id == queryProductDiscount.DiscountRef);
                        //    if (queryFindCatRef != null)
                        //    {
                        //        vm.ProductDiscount = queryFindCatRef.Discount;
                        //    }
                        //}


                        list.Add(vm);
                    }



                }
            }
            catch (Exception e)
            {

            }

            return list;
        }
        public List<VMProduct.VmMainProduct> RepositoryMainProducts()
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product.Where(c => c.IsOk == true)
                    .OrderByDescending(c => c.ModifierDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProduct
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            Discount = product.Discount,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c =>
                                c.IsOk && c.Ref == product.Id && c.IsMain);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }

                        var querySummary =
                            db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsOk && c.IsMain);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = querySummary.Fee;

                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = product.Fee;
                        }

                        var queryCategories =
                            db.Table_Product_Category.AsNoTracking()
                                .FirstOrDefault(c => c.Id == product.CategoriesRef);
                        if (queryCategories != null)
                        {
                            vm.CategoreisTitle = queryCategories.PrimaryTitle;
                            vm.CategoriesRef = queryCategories.Id;

                        }
                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelCategoreis> RepositoryMainCategoreis()
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {

                var query = db.Table_Product_Category.Where(c => c.IsMain).OrderBy(c => c.Sort)
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var category in query)
                    {
                        var vm = new VMProduct.ViewModelCategoreis
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            Code = category.Code,
                        };
                        var listsubcategorie = new List<VMProduct.ViewModelCategoreis>();

                        var querySub = db.Table_Product_Category
                            .Where(c => c.ParentRef == category.Id && c.IsOk && !c.IsDelete && !c.IsMain).AsNoTracking()
                            .ToList();
                        if (querySub.Count > 0)
                        {
                            foreach (var productCategory in querySub)
                            {
                                var vmm = new VMProduct.ViewModelCategoreis
                                {
                                    Id = productCategory.Id,
                                    PrimaryTitle = productCategory.PrimaryTitle,
                                };
                                listsubcategorie.Add(vmm);
                            }

                            vm.ViewModelSubCategoreis = listsubcategorie;
                        }
                        var queryFiles =
                            db.Table_File_Upload.FirstOrDefault(c => c.Ref == category.Id);
                        if (queryFiles != null)
                        {
                            vm.FileName = "/Static/Content/images/Categories/" + queryFiles.FileName +
                                          queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Helper/Main/img/bg/bg.svg";
                        }


                        list.Add(vm);
                    }
                }

                return list;
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelCategoreis> RepositoryMainCategoreisDefault()
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {

                var query = db.Table_Product_Category.Where(c => c.IsOk && c.Url != null).OrderBy(c => c.Sort)
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var category in query)
                    {
                        var vm = new VMProduct.ViewModelCategoreis
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            //SecondaryTitle = "/Static/Content/Images/Categories/" + category.Code + ".jpg",
                            ParentRef = category.ParentRef,
                            Code = category.Code,
                            Url = category.Url,
                            Sort = category.Sort,
                        };

                        var queryFiles =
                            db.Table_File_Upload.FirstOrDefault(c => c.Ref == category.Id);
                        if (queryFiles != null)
                        {
                            vm.FileName = "/Static/Content/images/Categories/" + queryFiles.FileName +
                                                 queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Helper/Main/img/bg/bg.svg";
                        }


                        list.Add(vm);
                    }
                }

                return list;
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelCategoreis> RepositoryCategoryManagment()
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {


                var query = db.Table_Product_Category.OrderByDescending(c => c.CreatorDate).AsNoTracking()
                    .ToList();

                if (query.Count > 0)
                {

                    foreach (var category in query)
                    {

                        var vm = new VMProduct.ViewModelCategoreis()
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            IsMain = category.IsMain,
                            Code = category.Code,
                            IsDelete = category.IsDelete,
                            SecondaryTitle = category.SecondaryTitle,
                            Sort = category.Sort,
                            IsOk = category.IsOk,
                            Url = category.Url,
                        };


                        var queryFiles =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && c.IsMain && c.Ref == category.Id);
                        if (queryFiles != null)
                        {
                            vm.FileName = ServerPath.ServerPathFileUploadMainCategories + queryFiles.FileName +
                                                 queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Helper/Main/img/bg/bg.svg";

                        }



                        if (category.IsOk)
                        {
                            vm.IsOkClass = "btn btn-success";
                            vm.IsOkTitle = "فعال";
                        }
                        else
                        {
                            vm.IsOkClass = "btn btn-danger";
                            vm.IsOkTitle = "غیر فعال";
                        }

                        list.Add(vm);
                    }
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }


        public List<VMProduct.ViewModelCategoreis> RepositoryCategoryManagmentSearch(string searchnew)
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);

                var query = db.Table_Product_Category.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search)).AsNoTracking()
                    .ToList();

                if (query.Count > 0)
                {

                    foreach (var category in query)
                    {

                        var vm = new VMProduct.ViewModelCategoreis()
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            IsMain = category.IsMain,
                            Code = category.Code,
                            IsDelete = category.IsDelete,
                            SecondaryTitle = category.SecondaryTitle,
                            Sort = category.Sort,
                            IsOk = category.IsOk,

                        };
                        var queryFiles =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && c.IsMain && c.Ref == category.Id);
                        if (queryFiles != null)
                        {
                            vm.FileName = "/Static/Content/images/Categories/Desktop/" + queryFiles.FileName +
                                                 queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Helper/Main/img/bg/bg.svg";

                        }


                        if (category.IsOk)
                        {
                            vm.IsOkClass = "btn btn-success";
                            vm.IsOkTitle = "فعال";
                        }
                        else
                        {
                            vm.IsOkClass = "btn btn-danger";
                            vm.IsOkTitle = "غیر فعال";
                        }


                        list.Add(vm);
                    }
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }


        public List<VMProduct.ViewModelCategoreis> RepositorySubCategoryManagmentDropDown()
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {

                var query = db.Table_Product_Category
                    .Where(c => c.IsMain == false).ToList();
                if (query.Count > 0)
                {
                    foreach (var category in query)
                    {
                        var vm = new VMProduct.ViewModelCategoreis()
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            IsMain = category.IsMain,
                            Code = category.Code,
                            IsDelete = category.IsDelete,
                            SecondaryTitle = category.SecondaryTitle,
                        };


                        list.Add(vm);
                    }


                    var NoOne = new VMProduct.ViewModelCategoreis
                    {
                        Id = Guid.Empty,
                        PrimaryTitle = "--- بدون دسته بندی ---",
                        Code = "1",
                    };


                    list.Add(NoOne);


                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelProductSize> RepositorySizeManagmentDropDown()
        {
            var list = new List<VMProduct.ViewModelProductSize>();
            try
            {

                var query = db.Table_Product_Size.OrderByDescending(c => c.Sort)
                    .ToList();

                if (query.Count > 0)
                {
                    foreach (var size in query)
                    {
                        var vm = new VMProduct.ViewModelProductSize()
                        {
                            Id = size.Id,
                            PrimaryTitle = size.PrimaryTitle,
                            SizeTitle = size.SizeTitle,
                            Code = size.Code,
                            IsDelete = size.IsDelete,
                            SecondaryTitle = size.SecondaryTitle,
                        };


                        list.Add(vm);
                    }


                    return list;
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelProductColor> RepositoryColorManagmentDropDown()
        {
            var list = new List<VMProduct.ViewModelProductColor>();
            try
            {

                var query = db.Table_Product_Color
                    .ToList();

                if (query.Count > 0)
                {
                    foreach (var color in query)
                    {
                        var vm = new VMProduct.ViewModelProductColor()
                        {
                            Id = color.Id,
                            PrimaryTitle = color.PrimaryTitle,
                            ColorCode = color.ColorCode,
                            Code = color.Code,
                            IsDelete = color.IsDelete,
                            SecondaryTitle = color.SecondaryTitle,
                        };


                        list.Add(vm);
                    }


                    return list;
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelProductColor> RepositoryColorShowInProducts(Guid id)
        {
            var list = new List<VMProduct.ViewModelProductColor>();

            try
            {

                var query = db.Table_Product_Summary.Where(c => c.ProductRef == id && c.IsOk).OrderBy(c => c.Sort).ToList();

                if (query.Count > 0)
                {
                    var vmm = new VMProduct.ViewModelProductColor
                    {
                        ColorCode = "000000",
                        ColorRef = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                        PrimaryTitle = "--- رنگ را انتخاب کنید ---",
                    };

                    list.Add(vmm);
                    foreach (var summary in query)
                    {
                        var vm = new VMProduct.ViewModelProductColor();

                        if (list.FindIndex(c => c.ColorRef == summary.ColorRef) < 0)
                        {
                            var colorquery =
                                db.Table_Product_Color.FirstOrDefault(c => c.Id == summary.ColorRef && c.IsOk);
                            if (colorquery != null)
                            {
                                vm.ColorCode = colorquery.ColorCode;
                                vm.ColorRef = colorquery.Id;
                                vm.PrimaryTitle = colorquery.PrimaryTitle;
                            }

                            list.Add(vm);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
            //try
            //{
            //   
            //    var query = db.Table_Product_PropertyList.Where(c => c.ProductRef == id).ToList();

            //    if (query.Count > 0)
            //    {

            //        foreach (var color in query)
            //        {
            //            if (list.FindIndex(c => c.ColorRef == color.ColorRef) < 0)
            //            {
            //                var vm = new VMProduct.ViewModelProductPropertyList()
            //                {
            //                    Id = color.Id,
            //                    PrimaryTitle = color.PrimaryTitle,
            //                    Code = color.Code,
            //                    IsDelete = color.IsDelete,
            //                    SecondaryTitle = color.SecondaryTitle,
            //                };


            //                var colorquery =
            //                    db.Table_Product_Color.FirstOrDefault(c => c.Id == color.ColorRef);
            //                if (colorquery != null)
            //                {
            //                    vm.ColorTitle = colorquery.PrimaryTitle;
            //                    vm.ColorCode = colorquery.ColorCode;
            //                    vm.ColorRef = colorquery.Id;
            //                }

            //                list.Add(vm);
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{

            //}

        }

        public List<VMProduct.ViewModelProductPropertyList> RepositorySizeShowInProducts(Guid id, Guid colorRef)
        {
            var list = new List<VMProduct.ViewModelProductPropertyList>();
            try
            {

                var query = db.Table_Product_Summary
                    .Where(c => c.ProductRef == id && c.ColorRef == colorRef && c.Quantity != 0 && c.IsOk).OrderBy(c => c.Sort).ToList();

                if (query.Count > 0)
                {

                    foreach (var item in query)
                    {
                        if (list.FindIndex(c => c.SizeRef == item.SizeRef) < 0)
                        {
                            var vm = new VMProduct.ViewModelProductPropertyList()
                            {
                                Id = item.Id,
                                Code = item.Code,
                                IsDelete = item.IsDelete,
                            };


                            var SizeQuery =
                                db.Table_Product_Size.FirstOrDefault(c => c.Id == item.SizeRef && c.IsOk);
                            if (SizeQuery != null)
                            {
                                vm.SizeRef = SizeQuery.Id;
                                vm.SizeTitle = SizeQuery.PrimaryTitle;
                            }

                            vm.Fee = item.Fee;

                            list.Add(vm);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;



            //var list = new List<VMProduct.ViewModelProductPropertyList>();
            //try
            //{
            //   
            //    var query = db.Table_Product_PropertyList
            //        .Where(c => c.ProductRef == id && c.ColorRef == colorRef).ToList();

            //    if (query.Count > 0)
            //    {

            //        foreach (var color in query)
            //        {
            //            if (list.FindIndex(c => c.SizeRef == color.SizeRef) < 0)
            //            {
            //                var vm = new VMProduct.ViewModelProductPropertyList()
            //                {
            //                    Id = color.Id,
            //                    PrimaryTitle = color.PrimaryTitle,
            //                    Code = color.Code,
            //                    IsDelete = color.IsDelete,
            //                    SecondaryTitle = color.SecondaryTitle,
            //                };


            //                var SizeQuery =
            //                    db.Table_Product_Size.FirstOrDefault(c => c.Id == color.SizeRef);
            //                if (SizeQuery != null)
            //                {
            //                    vm.SizeRef = SizeQuery.Id;
            //                    vm.SizeTitle = SizeQuery.SizeTitle;
            //                }

            //                list.Add(vm);
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{

            //}

            //return list;
        }

        public decimal RepositoryCheckQuantityProducts(Guid id, Guid colorRef, Guid sizeRef)
        {
            try
            {
                var query = db.Table_Product_Summary
                    .FirstOrDefault(c => c.ProductRef == id && c.ColorRef == colorRef && c.SizeRef == sizeRef);

                if (query != null)
                {
                    return query.Quantity;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<VMProduct.ViewModelProductPropertyList> RepositoryColorSizeList(Guid id)
        {
            var list = new List<VMProduct.ViewModelProductPropertyList>();
            try
            {

                var query = db.Table_Product_Summary.Where(c => c.ProductRef == id).OrderByDescending(c => c.Sort).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMProduct.ViewModelProductPropertyList
                        {
                            Id = item.Id,
                            Code = item.Code,
                            ProductRef = id,
                            ColorRef = item.ColorRef,
                            SizeRef = item.SizeRef,
                            Quantity = item.Quantity,
                            Fee = item.Fee,
                            Sort = item.Sort,
                        };


                        var querySize = db.Table_Product_Size.FirstOrDefault(c => c.Id == item.SizeRef);
                        if (querySize != null)
                        {
                            vm.SizeTitle = querySize.PrimaryTitle;
                        }

                        var queryColor = db.Table_Product_Color.FirstOrDefault(c => c.Id == item.ColorRef);
                        if (queryColor != null)
                        {
                            vm.ColorTitle = queryColor.PrimaryTitle;
                        }


                        if (item.IsOk)
                        {
                            vm.IsOkClass = "btn btn-success";
                            vm.IsOkTitle = "فعال";
                        }
                        else
                        {
                            vm.IsOkClass = "btn btn-danger";
                            vm.IsOkTitle = "غیر فعال";
                        }

                        if (item.IsMain)
                        {
                            vm.IsMainClass = "btn btn-success";
                            vm.IsMainTitle = "اصلی";
                        }
                        else
                        {
                            vm.IsMainClass = "btn btn-danger";
                            vm.IsMainTitle = "غیر اصلی";
                        }
                        list.Add(vm);
                    }

                }
            }
            catch (Exception e)
            {

            }
            return list;
        }

        //public List<VMProduct.ViewModelProductPropertyList> RepositoryColorSizeListsearch(Guid id , string searchnew)
        //{
        //    var list = new List<VMProduct.ViewModelProductPropertyList>();
        //    try
        //    {
                
        //        var search = searchnew.Replace("	", "");
        //        var size = Guid.Parse(search);
        //        var color = Guid.Parse(search);
        //        var query = db.Table_Product_Summary.Where(c => c.ProductRef == id ).OrderByDescending(c => c.Sort).ToList();
        //        if (query.Count > 0)
        //        {
        //            foreach (var item in query)
        //            {
        //                var vm = new VMProduct.ViewModelProductPropertyList
        //                {
        //                    Id = item.Id,
        //                    Code = item.Code,
        //                    ProductRef = id,
        //                    ColorRef = item.ColorRef,
        //                    SizeRef = item.SizeRef,
        //                    Quantity = item.Quantity,
        //                    Fee = item.Fee,
        //                    Sort = item.Sort,
        //                };


        //                var querySize = db.Table_Product_Size.FirstOrDefault(c => c.Id == item.SizeRef && );
        //                if (querySize != null)
        //                {
        //                    vm.SizeTitle = querySize.PrimaryTitle;
        //                }

        //                var queryColor = db.Table_Product_Color.FirstOrDefault(c => c.Id == item.ColorRef);
        //                if (queryColor != null)
        //                {
        //                    vm.ColorTitle = queryColor.PrimaryTitle;
        //                }


        //                if (item.IsOk)
        //                {
        //                    vm.IsOkClass = "btn btn-success";
        //                    vm.IsOkTitle = "فعال";
        //                }
        //                else
        //                {
        //                    vm.IsOkClass = "btn btn-danger";
        //                    vm.IsOkTitle = "غیر فعال";
        //                }

        //                if (item.IsMain)
        //                {
        //                    vm.IsMainClass = "btn btn-success";
        //                    vm.IsMainTitle = "اصلی";
        //                }
        //                else
        //                {
        //                    vm.IsMainClass = "btn btn-danger";
        //                    vm.IsMainTitle = "غیر اصلی";
        //                }
        //                list.Add(vm);
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return list;
        //}

        public List<VMProduct.VmMainProductMangement> RepositoryEditListColorSize(Guid id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var query = db.Table_Product_Summary.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {

                    var vm = new VMProduct.VmMainProductMangement
                    {
                        Id = query.Id,
                        Code = query.Code,
                        ProductRef = query.ProductRef,
                        ColorRef = query.ColorRef,
                        SizeRef = query.SizeRef,
                        Quantity = query.Quantity,
                        Fee = query.Fee,
                        Sort = query.Sort,
                    };

                    var querySize = db.Table_Product_Size.FirstOrDefault(c => c.Id == query.SizeRef);
                    if (querySize != null)
                    {
                        vm.SizeTitle = querySize.PrimaryTitle;
                    }

                    var queryColor = db.Table_Product_Color.FirstOrDefault(c => c.Id == query.ColorRef);
                    if (queryColor != null)
                    {
                        vm.ColorTitle = queryColor.PrimaryTitle;
                    }


                    if (query.IsOk)
                    {
                        vm.IsOkClass = "btn btn-success";
                        vm.IsOkTitle = "فعال";
                    }
                    else
                    {
                        vm.IsOkClass = "btn btn-danger";
                        vm.IsOkTitle = "غیر فعال";
                    }

                    if (query.IsMain)
                    {
                        vm.IsMainClass = "btn btn-success";
                        vm.IsMainTitle = "اصلی";
                    }
                    else
                    {
                        vm.IsMainClass = "btn btn-danger";
                        vm.IsMainTitle = "غیر اصلی";
                    }
                    list.Add(vm);


                }
            }
            catch (Exception e)
            {

            }
            return list;
        }


        public string RepositoryEditColorSize(VMProduct.VmMainProductMangement value, Guid userid)
        {
            try
            {

                var userRef = userid;
                var query = db.Table_Product_Summary.FirstOrDefault(c => c.Id == value.Id);
                if (query != null)
                {
                    query.Quantity = value.Quantity;
                    query.Fee = value.Fee;
                    query.ColorRef = value.ColorRef;
                    query.SizeRef = value.SizeRef;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = value.Sort;


                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public List<VMProduct.ViewModelCategoreis> RepositoryCategoryManagmentDropDown()
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {

                var query = db.Table_Product_Category
                    .ToList();

                if (query.Count > 0)
                {
                    foreach (var category in query)
                    {
                        var vm = new VMProduct.ViewModelCategoreis()
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            IsMain = category.IsMain,
                            Code = category.Code,
                            IsDelete = category.IsDelete,
                            SecondaryTitle = category.SecondaryTitle,
                        };


                        list.Add(vm);
                    }


                    var nulll = new VMProduct.ViewModelCategoreis
                    {
                        Id = Guid.Empty,
                        PrimaryTitle = "--- بدون دسته بندی ---",
                        Code = "1",
                    };


                    list.Add(nulll);


                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.VmMainProduct> RepositoryMainProductsById(Guid id)
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product.FirstOrDefault(c => c.Id == id);


                if (query != null)
                {
                    var vm = new VMProduct.VmMainProduct
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        TertiaryTitle = query.TertiaryTitle,
                        Url = query.Url,
                        Discount = query.Discount,
                        Note = query.Note,
                        CategoriesRef = query.CategoriesRef,
                    };

                    var querySummary =
                        db.Table_Product_Summary.FirstOrDefault(c =>
                            c.ProductRef == query.Id && c.IsOk && c.IsMain && c.Quantity != 0);
                    if (querySummary != null)
                    {
                        if (querySummary.Quantity > 0)
                        {
                            vm.QuantityTitle = "موجود";
                            vm.QuantityClass = "product-card__badge--yes";
                        }
                        else
                        {
                            vm.QuantityTitle = "ناموجود";
                            vm.QuantityClass = "product-card__badge--no";
                        }

                        vm.Fee = querySummary.Fee;

                    }
                    else
                    {
                        if (query.Quantity > 0)
                        {
                            vm.QuantityTitle = "موجود";
                            vm.QuantityClass = "product-card__badge--yes";
                        }
                        else
                        {
                            vm.QuantityTitle = "ناموجود";
                            vm.QuantityClass = "product-card__badge--no";
                        }

                        vm.Fee = query.Fee;
                    }

                    var queryfile =
                        db.Table_File_Upload.FirstOrDefault(
                            c => c.IsOk && c.Ref == query.Id && c.IsMain);
                    if (queryfile != null)
                    {
                        vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                      queryfile.FileExtensions;
                    }
                    else
                    {
                        vm.FileName = "/Static/Content/Images/";
                    }

                    var list2 = new List<VMProduct.FileUploadName>();
                    var queryFullPictures = db.Table_File_Upload
                        .Where(c => c.IsOk && c.Ref == query.Id ).OrderBy(c=>c.IsMain).AsNoTracking().ToList();
                    if (queryFullPictures.Count > 0)
                    {
                        foreach (var fileUpload in queryFullPictures)
                        {
                            var vmm = new VMProduct.FileUploadName
                            {
                                FileName = "/Static/Content/Images/Products/" + fileUpload.FileName +
                                           fileUpload.FileExtensions,
                            };
                            list2.Add(vmm);
                        }

                        vm.FileList = list2;
                    }
                    else
                    {
                        vm.FileList = new List<VMProduct.FileUploadName>();
                    }


                    var list3 = new List<VMProduct.ProductProperty>();
                    var queryFullProperty = db.Table_Product_Property
                        .Where(c => c.IsOk && c.Ref == query.Id && c.Entity == "Property").AsNoTracking().ToList();
                    if (queryFullProperty.Count > 0)
                    {
                        foreach (var property in queryFullProperty)
                        {
                            var vmm = new VMProduct.ProductProperty
                            {
                                Key = property.Keys,
                                Value = property.Value,
                            };
                            list3.Add(vmm);
                        }

                        vm.PropertyList = list3;
                    }
                    else
                    {
                        vm.PropertyList = new List<VMProduct.ProductProperty>();
                    }

                    var queryCategories =
                        db.Table_Product_Category.FirstOrDefault(c => c.Id == query.CategoriesRef);
                    if (queryCategories != null)
                    {
                        vm.CategoreisTitle = queryCategories.PrimaryTitle;
                        vm.CategoriesRef = queryCategories.Id;

                    }

                    list.Add(vm);

                }
            }
            catch (Exception e)
            {

            }

            return list;
        }


        public List<VMProduct.VmMainProduct> RepositoryQuantitiesProducts(Guid id, Guid color, Guid size)
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {
                var query = db.Table_Product_Summary.FirstOrDefault(c =>
                    c.ProductRef == id && c.ColorRef == color && c.SizeRef == size && c.IsOk);
                if (query != null)
                {
                    var vmm = new VMProduct.VmMainProduct
                    {
                        Quantity = query.Quantity,
                    };

                    list.Add(vmm);
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }





        public List<VMProduct.ViewModelProductSize> RepositoryMainSizeMangment()
        {
            var list = new List<VMProduct.ViewModelProductSize>();
            try
            {

                var query = db.Table_Product_Size
                    .OrderByDescending(c => c.SizeTitle).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.ViewModelProductSize()
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            SizeTitle = product.SizeTitle,
                            IsDelete = product.IsDelete,
                            CreatorDate = product.CreatorDate,
                            CreatorRef = product.CreatorRef,
                            Sort = product.Sort

                        };

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }
        public List<VMProduct.ViewModelProductSize> RepositoryMainSizeMangmentSearch(string searchnew)
        {
            var list = new List<VMProduct.ViewModelProductSize>();
            try
            {
                var search = searchnew.Replace("	", "");

                var query = db.Table_Product_Size
                    .OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search) || c.SecondaryTitle.Contains(search) || c.SizeTitle.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.ViewModelProductSize()
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            SizeTitle = product.SizeTitle,
                            IsDelete = product.IsDelete,
                            CreatorDate = product.CreatorDate,
                            CreatorRef = product.CreatorRef,
                            Sort = product.Sort,

                        };

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelProductColor> RepositoryMainColorMangment()
        {
            var list = new List<VMProduct.ViewModelProductColor>();
            try
            {

                var query = db.Table_Product_Color
                    .OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.ViewModelProductColor
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            ColorCode = product.ColorCode,
                            IsDelete = product.IsDelete,
                            CreatorDate = product.CreatorDate,
                            CreatorRef = product.CreatorRef,

                        };

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }
        public List<VMProduct.ViewModelProductColor> RepositoryMainColorMangmentSearch(string searchnew)
        {
            var list = new List<VMProduct.ViewModelProductColor>();
            try
            {
                var search = searchnew.Replace("	", "");

                var query = db.Table_Product_Color
                    .OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search) || c.SecondaryTitle.Contains(search) || c.ColorCode.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.ViewModelProductColor
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            ColorCode = product.ColorCode,
                            IsDelete = product.IsDelete,
                            CreatorDate = product.CreatorDate,
                            CreatorRef = product.CreatorRef,

                        };

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }


        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsManagementByConfigRef(
            Guid id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate)
                    .AsNoTracking().ToList();
                if (queryProduct.Count > 0)
                {
                    foreach (var product in queryProduct)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            ModifierDateTime = DateTime.Now,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        var queryFind = db.Table_Product_Selection.AsNoTracking().ToList()
                            .Exists(c => c.ProductRef == product.Id && c.ConfigurationRef == id);
                        if (queryFind)
                        {
                            vm.IsOkTitle = "انتخاب شده است";
                            vm.IsOkClass = "btn btn-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "انتخاب  نشده است";
                            vm.IsOkClass = "btn btn-danger";
                        }

                        list.Add(vm);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentByConfigRefAndSearchKey(
            Guid id, string searchnew)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate)
                    .Where(c => c.PrimaryTitle.Contains(search) || c.Code == search).AsNoTracking().ToList();
                if (queryProduct.Count > 0)
                {
                    foreach (var product in queryProduct)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            ModifierDateTime = DateTime.Now,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        var queryFind = db.Table_Product_Selection.AsNoTracking().ToList()
                            .Exists(c => c.ProductRef == product.Id && c.ConfigurationRef == id);
                        if (queryFind)
                        {
                            vm.IsOkTitle = "انتخاب شده است";
                            vm.IsOkClass = "btn btn-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "انتخاب  نشده است";
                            vm.IsOkClass = "btn btn-danger";
                        }

                        list.Add(vm);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentByConfigRefAndByCagegoriesRef(
            Guid id, Guid searchnew)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate)
                    .Where(c => c.CategoriesRef == searchnew).AsNoTracking().ToList();
                if (queryProduct.Count > 0)
                {
                    foreach (var product in queryProduct)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            ModifierDateTime = DateTime.Now,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        var queryFind = db.Table_Product_Selection.AsNoTracking().ToList()
                            .Exists(c => c.ProductRef == product.Id && c.ConfigurationRef == id);
                        if (queryFind)
                        {
                            vm.IsOkTitle = "انتخاب شده است";
                            vm.IsOkClass = "btn btn-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "انتخاب  نشده است";
                            vm.IsOkClass = "btn btn-danger";
                        }

                        list.Add(vm);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }
        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentByConfigRefAndSearchKeySearch(
            Guid id, string SearchKey, string searchnew)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate)
                    .Where(c => c.Code.Contains(search)).AsNoTracking().ToList();
                if (queryProduct.Count > 0)
                {
                    foreach (var product in queryProduct)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            ModifierDateTime = DateTime.Now,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        var queryFind = db.Table_Product_Selection.AsNoTracking().ToList()
                            .Exists(c => c.ProductRef == product.Id && c.ConfigurationRef == id);
                        if (queryFind)
                        {
                            vm.IsOkTitle = "انتخاب شده است";
                            vm.IsOkClass = "btn btn-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "انتخاب  نشده است";
                            vm.IsOkClass = "btn btn-danger";
                        }

                        list.Add(vm);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentByConfigRef(Guid id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate).AsNoTracking()
                    .ToList();
                if (queryProduct.Count > 0)
                {
                    foreach (var product in queryProduct)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Discount = product.Discount,
                            Note = product.Note,

                        };

                        var querySummary =
                            db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsOk && c.IsMain);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }

                            vm.Quantity = querySummary.Quantity;
                            vm.Fee = querySummary.Fee;

                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = product.Fee;
                        }

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        var queryFind = db.Table_Product_Selection.AsNoTracking().ToList()
                            .Exists(c => c.ProductRef == product.Id && c.ConfigurationRef == id);
                        if (queryFind)
                        {
                            vm.IsOkTitle = "انتخاب شده است";
                            vm.IsOkClass = "btn btn-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "انتخاب  نشده است";
                            vm.IsOkClass = "btn btn-danger";
                        }

                        list.Add(vm);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }





        public List<VMProduct.VmMainProductMangement> RepositoryShowProductsMangmentByConfigRef(Guid id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var query = db.Table_Product_Selection.Where(c => c.ConfigurationRef == id)
                    .AsNoTracking().ToList();

                if (query.Count > 0)
                {
                    foreach (var selection in query)
                    {
                        var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate)
                            .AsNoTracking().ToList();
                        if (queryProduct.Count > 0)
                        {
                            foreach (var product in queryProduct)
                            {
                                var vm = new VMProduct.VmMainProductMangement
                                {
                                    Id = product.Id,
                                    Code = product.Code,
                                    IsOk = product.IsOk,
                                    PrimaryTitle = product.PrimaryTitle,
                                    SecondaryTitle = product.SecondaryTitle,
                                    TertiaryTitle = product.TertiaryTitle,
                                    Url = product.Url,
                                    CreatorDateTime = product.CreatorDate,
                                    Discount = product.Discount,
                                    Note = product.Note,

                                };

                                var queryfile =
                                    db.Table_File_Upload.FirstOrDefault(c =>
                                        c.IsOk && c.Ref == product.Id);
                                if (queryfile != null)
                                {
                                    vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                                  queryfile.FileExtensions;
                                }
                                else
                                {
                                    vm.FileName = "/Static/Content/Images/";
                                }


                                var queryFind = db.Table_Product_Selection.AsNoTracking().ToList()
                                    .Exists(c => c.ProductRef == product.Id && c.ConfigurationRef == id);
                                if (queryFind)
                                {
                                    vm.IsOkTitle = "انتخاب شده است";
                                    vm.IsOkClass = "btn btn-success";
                                }
                                else
                                {
                                    vm.IsOkTitle = "انتخاب  نشده است";
                                    vm.IsOkClass = "btn btn-danger";
                                }



                                list.Add(vm);
                            }
                        }
                    }
                }
                else
                {
                    var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate)
                        .AsNoTracking().ToList();
                    if (queryProduct.Count > 0)
                    {
                        foreach (var product in queryProduct)
                        {
                            var vm = new VMProduct.VmMainProductMangement
                            {
                                Id = product.Id,
                                Code = product.Code,
                                IsOk = product.IsOk,
                                PrimaryTitle = product.PrimaryTitle,
                                SecondaryTitle = product.SecondaryTitle,
                                TertiaryTitle = product.TertiaryTitle,
                                Url = product.Url,
                                CreatorDateTime = product.CreatorDate,
                                Discount = product.Discount,
                                Note = product.Note,
                            };
                            var querySummary =
                                db.Table_Product_Summary.FirstOrDefault(c =>
                                    c.ProductRef == product.Id && c.IsOk && c.IsMain);
                            if (querySummary != null)
                            {
                                if (querySummary.Quantity > 0)
                                {
                                    vm.QuantityTitle = "موجود";
                                    vm.QuantityClass = "product-card__badge--yes";
                                }
                                else
                                {
                                    vm.QuantityTitle = "ناموجود";
                                    vm.QuantityClass = "product-card__badge--no";
                                }


                                vm.Fee = querySummary.Fee;

                            }
                            else
                            {
                                if (product.Quantity > 0)
                                {
                                    vm.QuantityTitle = "موجود";
                                    vm.QuantityClass = "product-card__badge--yes";
                                }
                                else
                                {
                                    vm.QuantityTitle = "ناموجود";
                                    vm.QuantityClass = "product-card__badge--no";
                                }


                                vm.Fee = product.Fee;
                            }

                            var queryfile =
                                db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                            if (queryfile != null)
                            {
                                vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                              queryfile.FileExtensions;
                            }
                            else
                            {
                                vm.FileName = "/Static/Content/Images/";
                            }

                            vm.IsOkTitle = "انتخاب شده نشده است";
                            vm.IsOkClass = "btn btn-danger";

                            list.Add(vm);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public string RepositoryMainProductAddToConfigurationSelection(Guid configRef, Guid productRef)
        {
            var message = "Application Error : Null";
            try
            {

                var query = db.Table_Product_Selection.ToList()
                    .Exists(c => c.ConfigurationRef == configRef && c.ProductRef == productRef);

                if (!query)
                {
                    var queryAdd = db.Table_Product_Selection.Add(new Table_Product_Selection
                    {
                        Id = Guid.NewGuid(),
                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_Product_Selection"),
                        ConfigurationRef = configRef,
                        CreatorDate = DateTime.Now,
                        CreatorRef = Guid.NewGuid(),
                        IsDelete = false,
                        IsOk = true,
                        ModifierDate = DateTime.Now,
                        ModifierRef = Guid.NewGuid(),
                        PrimaryTitle = "",
                        ProductRef = productRef,
                        SecondaryTitle = "",
                        TertitatyTitle = "",
                        Version = 1,
                        Sort = 1,
                    });
                    db.Table_Product_Selection.Add(queryAdd);
                    db.SaveChanges();
                    return "Success";
                }
                else
                {
                    var queryDelete = db.Table_Product_Selection.FirstOrDefault(c =>
                        c.ConfigurationRef == configRef && c.ProductRef == productRef);
                    if (queryDelete != null)
                    {
                        db.Table_Product_Selection.Remove(queryDelete);
                        db.SaveChanges();
                        return "Success";
                    }
                    else
                    {
                        return "Application Error : Not Found";
                    }

                }
            }
            catch (Exception e)
            {
                message = "Application Error : " + e.Message;
            }

            return message;
        }

        public List<VMProduct.ViewModelProductPropertyList> RepositoryMainProductListMangment(Guid id)
        {
            var list = new List<VMProduct.ViewModelProductPropertyList>();
            try
            {

                var query = db.Table_Product_PropertyList
                    .OrderByDescending(c => c.SizeRef).Where(c => c.ProductRef == id).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.ViewModelProductPropertyList()
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            ProductRef = product.ProductRef,
                            ColorRef = product.ColorRef,
                            SizeRef = product.SizeRef,
                            Count = product.Count,

                        };

                        var queryProduct =
                            db.Table_Product.FirstOrDefault(c => c.Id == product.ProductRef);
                        if (queryProduct != null)
                        {
                            vm.ProductTitle = queryProduct.PrimaryTitle;
                        }

                        var queryColor =
                            db.Table_Product_Color.FirstOrDefault(c => c.Id == product.ColorRef);
                        if (queryColor != null)
                        {
                            vm.ColorTitle = queryColor.PrimaryTitle;
                        }

                        var querySize =
                            db.Table_Product_Size.FirstOrDefault(c => c.Id == product.SizeRef);
                        if (querySize != null)
                        {
                            vm.SizeTitle = querySize.SizeTitle;
                        }

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }




        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangment()
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var query = db.Table_Product
                    .OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Discount = product.Discount,
                            Note = product.Note,
                            };

                        var querySummary =
                            db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsOk && c.IsMain);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }

                            vm.Quantity = querySummary.Quantity;
                            vm.Fee = querySummary.Fee;

                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }

                            vm.Quantity = product.Quantity;
                            vm.Fee = product.Fee;
                        }

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }

                        if (product.IsOk == true)
                        {
                            vm.IsOkTitle = "فعال";
                            vm.IsOkClass = "btn btn-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "غیرفعال";
                            vm.IsOkClass = "btn btn-danger";
                        }

                        var queryCategories =
                            db.Table_Product_Category.AsNoTracking()
                                .FirstOrDefault(c => c.Id == product.CategoriesRef);
                        if (queryCategories != null)
                        {
                            vm.CategoreisTitle = queryCategories.PrimaryTitle;
                            vm.CategoriesRef = queryCategories.Id;

                        }

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }


        public List<VMProduct.VmMainProductMangementExport> RepositoryMainProductsMangmentExcelReport()
        {
            var list = new List<VMProduct.VmMainProductMangementExport>();
            try
            {

                var querySummary =
                    db.Table_Product_Summary.OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (querySummary.Count > 0)
                {
                    foreach (var summary in querySummary)
                    {
                        var vm = new VMProduct.VmMainProductMangementExport
                        {
                            CodeSummary = summary.Code,
                            Quantity = summary.Quantity,
                            Fee = summary.Fee,
                            ColorRef = summary.ColorRef,
                            SizeRef = summary.SizeRef,
                            ProductRef = summary.ProductRef,
                        };
                        var queryProduct = db.Table_Product.FirstOrDefault(c => c.Id == summary.ProductRef);
                        if (queryProduct != null)
                        {
                            vm.CodeProduct = queryProduct.Code;
                            vm.PrimaryTitle = queryProduct.PrimaryTitle;
                        }
                        var querySize = db.Table_Product_Size.FirstOrDefault(c => c.Id == summary.SizeRef);
                        if (querySize != null)
                        {
                            vm.Size = querySize.PrimaryTitle;
                            vm.SizeRef = querySize.Id;
                        }

                        var queryColor = db.Table_Product_Color.FirstOrDefault(c => c.Id == summary.ColorRef);
                        if (queryColor != null)
                        {
                            vm.Color = queryColor.PrimaryTitle;
                            vm.ColorRef = queryColor.Id;
                        }
                        list.Add(vm);
                    }

                }


            }


            catch (Exception e)
            {

            }

            return list;
        }


        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentSearch(string searchnew)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {
                var search = searchnew.Replace("	", "");

                var query = db.Table_Product
                    .OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search) || c.SecondaryTitle.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Discount = product.Discount,
                            Note = product.Note,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }

                        var querySummary =
                            db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsOk && c.IsMain);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = querySummary.Fee; 
                            vm.Quantity = querySummary.Quantity;

                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = product.Fee; 
                            vm.Quantity = product.Quantity;
                        }



                        if (product.IsOk == true)
                        {
                            vm.IsOkTitle = "فعال";
                            vm.IsOkClass = "btn btn-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "غیرفعال";
                            vm.IsOkClass = "btn btn-danger";
                        }

                        var queryCategories =
                            db.Table_Product_Category.AsNoTracking()
                                .FirstOrDefault(c => c.Id == product.CategoriesRef);
                        if (queryCategories != null)
                        {
                            vm.CategoreisTitle = queryCategories.PrimaryTitle;
                            vm.CategoriesRef = queryCategories.Id;

                        }


                        //var querySummary = db.Table_Product_Summary.FirstOrDefault(C=>C.ProductRef == product.Id);
                        //if (querySummary.IsMain == true)
                        //{
                        //    vm.IsMainTitle = "اصلی";
                        //    vm.IsMainClass = "btn btn-success";
                        //}
                        //else
                        //{
                        //    vm.IsMainTitle = "اصلی نیست";
                        //    vm.IsMainClass = "btn btn-danger";
                        //}


                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelProductPropertyList> RepositoryMainProductListMangmentByid(Guid Id)
        {
            var list = new List<VMProduct.ViewModelProductPropertyList>();
            try
            {


                var query = db.Table_Product_PropertyList.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMProduct.ViewModelProductPropertyList()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        ColorRef = query.ColorRef,
                        ProductRef = query.ProductRef,
                        ProductTitle = query.PrimaryTitle,
                        Count = query.Count,
                        SizeRef = query.SizeRef,

                    };




                    list.Add(vm);
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentByid(Guid Id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var query = db.Table_Product.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMProduct.VmMainProductMangement
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        TertiaryTitle = query.TertiaryTitle,
                        Url = query.Url,
                        CreatorDateTime = query.CreatorDate,
                        Discount = query.Discount,
                        Note = query.Note,
                        CategoriesRef = query.CategoriesRef,
                    };

                    var queryfile =
                        db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == query.Id);
                    if (queryfile != null)
                    {
                        vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                      queryfile.FileExtensions;
                    }
                    else
                    {
                        vm.FileName = "/Static/Content/Images/";
                    }

                    var querySummary =
                        db.Table_Product_Summary.FirstOrDefault(c =>
                            c.ProductRef == query.Id && c.IsOk && c.IsMain);
                    if (querySummary != null)
                    {
                        if (querySummary.Quantity > 0)
                        {
                            vm.QuantityTitle = "موجود";
                            vm.QuantityClass = "product-card__badge--yes";
                        }
                        else
                        {
                            vm.QuantityTitle = "ناموجود";
                            vm.QuantityClass = "product-card__badge--no";
                        }


                        vm.Fee = querySummary.Fee; 
                        vm.Quantity = querySummary.Quantity;

                    }
                    else
                    {
                        if (query.Quantity > 0)
                        {
                            vm.QuantityTitle = "موجود";
                            vm.QuantityClass = "product-card__badge--yes";
                        }
                        else
                        {
                            vm.QuantityTitle = "ناموجود";
                            vm.QuantityClass = "product-card__badge--no";
                        }


                        vm.Fee = query.Fee;
                        vm.Quantity = query.Quantity;
                    }



                    if (query.IsOk == true)
                    {
                        vm.IsOkTitle = "فعال";
                        vm.IsOkClass = "label label-success";
                    }
                    else
                    {
                        vm.IsOkTitle = "فعال نیست";
                        vm.IsOkClass = "label label-danger";
                    }


                    list.Add(vm);
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelConfiguration> RepositoryConfigurationMangmentByid(Guid Id)
        {
            var list = new List<VMProduct.ViewModelConfiguration>();
            try
            {

                var query = db.Table_Selection_Configuration.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMProduct.ViewModelConfiguration()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        CreatorDate = query.CreatorDate,
                        IsDelete = query.IsDelete,
                        ModifierDate = query.ModifierDate,
                        ModifierRef = query.ModifierRef,
                        Count = query.Count,
                        Sort = query.Sort,
                        CategoriesRef = query.CategoriesRef,
                        CreatorRef = query.CreatorRef,
                        Url = query.Url,

                    };

                    list.Add(vm);
                }

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public List<VMProduct.ViewModelProductSize> RepositorySizeMangmentByid(Guid id)
        {
            var list = new List<VMProduct.ViewModelProductSize>();
            try
            {

                var query = db.Table_Product_Size.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    var vm = new VMProduct.ViewModelProductSize()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        CreatorDate = query.CreatorDate,
                        IsDelete = query.IsDelete,
                        SizeTitle = query.SizeTitle,
                        CreatorRef = query.CreatorRef,
                        Version = query.Version,
                        Sort = query.Sort
                    };

                    list.Add(vm);
                }

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public List<VMProduct.ViewModelProductColor> RepositoryColorMangmentByid(Guid id)
        {
            var list = new List<VMProduct.ViewModelProductColor>();
            try
            {

                var query = db.Table_Product_Color.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    var vm = new VMProduct.ViewModelProductColor()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        CreatorDate = query.CreatorDate,
                        IsDelete = query.IsDelete,
                        ColorCode = query.ColorCode,
                        CreatorRef = query.CreatorRef,
                        Version = query.Version,

                    };

                    list.Add(vm);
                }

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public List<VMProduct.ViewModelCategoreis> RepositoryCategoryMangmentByid(Guid Id)
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {

                var query = db.Table_Product_Category.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMProduct.ViewModelCategoreis
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        ParentRef = query.ParentRef,
                        CreatorDate = query.CreatorDate,
                        IsDelete = query.IsDelete,
                        IsMain = query.IsMain,
                        ModifierDate = query.ModifierDate,
                        Sort = query.Sort,
                        Url = query.Url,
                    };
                    var queryFiles =
                        db.Table_File_Upload.FirstOrDefault(c =>
                            c.IsOk && c.IsMain && c.Ref == query.Id);
                    if (queryFiles != null)
                    {
                        vm.FileName = "/Static/Content/images/Categories/" + queryFiles.FileName +
                                      queryFiles.FileExtensions;
                    }
                    else
                    {
                        vm.FileName = "/Helper/Main/img/bg/bg.svg";
                    }

                    list.Add(vm);
                }

            }
            catch (Exception e)
            {

            }
            return list;
        }

        public List<VMProduct.VmMainProductMangement> RepositoryGetFullPictures(Guid id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var result = db.Table_File_Upload.Where(c => c.Ref == id).AsNoTracking().ToList();
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            FileName = ServerPath.ServerPathFileUploadMainProduct + item.FileUniqeName,
                            Id = item.Id,
                            CreatorDateTime = item.CreatorDate,
                        };
                        switch (item.IsOk)
                        {
                            case true:
                                {
                                    vm.IsOkClass = "label label-success";
                                    vm.IsOkTitle = "فعال";
                                    break;
                                }
                            case false:
                                {
                                    vm.IsOkClass = "label label-danger";
                                    vm.IsOkTitle = "غیر فعال";
                                    break;
                                }
                        }
                        switch (item.IsMain)
                        {
                            case true:
                                {
                                    vm.IsMainClass = "label label-success";
                                    vm.IsMainTitle = "اصلی";
                                    break;
                                }
                            case false:
                                {
                                    vm.IsMainClass = "label label-danger";
                                    vm.IsMainTitle = "غیر اصلی";
                                    break;
                                }
                        }
                        list.Add(vm);
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }

        public List<VMProduct.ProductProperty> RepositoryProperty(Guid id)
        {
            var list = new List<VMProduct.ProductProperty>();
            try
            {

                var result = db.Table_Product_Property.Where(c => c.Ref == id).AsNoTracking().ToList();
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        var vm = new VMProduct.ProductProperty()
                        {
                            Id = item.Id.ToString(),
                            Key = item.Keys,
                            Value = item.Value,

                        };
                        switch (item.IsOk)
                        {
                            case true:
                                {
                                    vm.IsOkClass = "label label-success";
                                    vm.IsOkTitle = "فعال";
                                    break;
                                }
                            case false:
                                {
                                    vm.IsOkClass = "label label-danger";
                                    vm.IsOkTitle = "غیر فعال";
                                    break;
                                }
                        }
                        switch (item.IsMain)
                        {
                            case true:
                                {
                                    vm.IsMainClass = "label label-success";
                                    vm.IsMainTitle = "اصلی";
                                    break;
                                }
                            case false:
                                {
                                    vm.IsMainClass = "label label-danger";
                                    vm.IsMainTitle = "غیر اصلی";
                                    break;
                                }
                        }

                        list.Add(vm);
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }


        public string PictureAdd(HttpPostedFileBase file, Guid id, Guid userid)
        {

            var userRef = userid;

            var filename = "Default";
            var fileExtention = "png";
            var filelenght = 200;
            if (file != null)
            {
                var idpic = Guid.NewGuid();
                filelenght = file.ContentLength;
                filename = "Id_" + idpic + "PId_" + id;
                fileExtention = Path.GetExtension(file.FileName);
                string pathCombine =
                    HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProduct + filename + fileExtention);
                file.SaveAs(pathCombine);
                var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());

                qAddPic.Id = idpic;
                qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                qAddPic.Tables = "Table_Product";
                qAddPic.Schemas = "General";
                qAddPic.Ref = id;
                qAddPic.FileExtensions = fileExtention;
                qAddPic.FileLenght = filelenght;
                qAddPic.FileUniqeName = filename + fileExtention;
                qAddPic.Sort = 1;
                qAddPic.IsDelete = false;
                qAddPic.FileName = filename;
                qAddPic.Version = 1;
                qAddPic.CreatorDate = DateTime.Now;
                qAddPic.CreatorRef = userRef;
                qAddPic.ModifierRef = userRef;
                qAddPic.ModifierDate = DateTime.Now;
                qAddPic.IsOk = true;
                qAddPic.IsMain = false;

                db.SaveChanges();


            }

            return "Success";
        }


        public string ChangeStatusPictures(Guid id)
        {
            var query = db.Table_File_Upload.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {

                switch (query.IsOk)
                {
                    case true:

                        {
                            query.IsOk = false;
                            break;
                        }
                    case false:

                        {
                            query.IsOk = true;
                            break;
                        }
                }

                db.SaveChanges();

            }
            return "Success";
        }
        
        public string ChangeStatusIsMainProperty(Guid id)
        {
            var query = db.Table_Product_Property.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {

                switch (query.IsMain)
                {
                    case true:

                        {
                            query.IsMain = false;
                            break;
                        }
                    case false:

                        {
                            query.IsMain = true;
                            break;
                        }
                }

                db.SaveChanges();

            }
            return "Success";
        }
        public string ChangeStatusPicturesIsMain(Guid id)
        {
            var query = db.Table_File_Upload.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {

                switch (query.IsMain)
                {
                    case true:

                        {
                            query.IsMain = false;
                            break;
                        }
                    case false:

                        {
                            query.IsMain = true;
                            break;
                        }
                }

                db.SaveChanges();

            }
            return "Success";
        }
        
        public string DeletePicturesIsMain(Guid id)
        {
            var query = db.Table_File_Upload.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {

                switch (query.IsOk)
                {
                    case true:

                        {
                            break;
                        }
                    case false:

                        {
                            try
                            {
                                var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainProduct +
                                                                               query.FileName + query.FileExtensions);
                                File.Delete(filee);
                            }
                            catch (Exception e)
                            {

                            }

                            //query.IsDelete = true;
                            db.Table_File_Upload.Remove(query);
                            db.SaveChanges();
                            break;
                        }
                }


            }
            return "Success";
        }
        
        public List<VMProduct.VmMainProduct> RepositoryMainProductSearch(string values)
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {
                var query = db.Table_Product.Where(c =>
                        c.PrimaryTitle.Contains(values) || c.Code.Contains(values) || c.SecondaryTitle.Contains(values) && c.IsOk)
                    .AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMProduct.VmMainProduct
                        {
                            Id = item.Id,
                            Code = item.Code,
                            IsOk = item.IsOk,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            TertiaryTitle = item.TertiaryTitle,
                            Url = item.Url,
                            Fee = item.Fee,
                            Quantity = item.Quantity,
                            Discount = item.Discount,
                            Note = item.Note,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == item.Id && c.IsMain);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }

                        var list2 = new List<VMProduct.FileUploadName>();
                        var queryFullPictures = db.Table_File_Upload.Where(c => c.IsOk && c.Ref == item.Id && !c.IsMain).AsNoTracking().ToList();
                        if (queryFullPictures.Count > 0)
                        {
                            foreach (var fileUpload in queryFullPictures)
                            {
                                var vmm = new VMProduct.FileUploadName
                                {
                                    FileName = "/Static/Content/Images/Products/" + fileUpload.FileName +
                                               fileUpload.FileExtensions,
                                };
                                list2.Add(vmm);
                            }

                            vm.FileList = list2;
                        }
                        else
                        {
                            vm.FileList = new List<VMProduct.FileUploadName>();
                        }


                        var list3 = new List<VMProduct.ProductProperty>();
                        var queryFullProperty = db.Table_Product_Property.Where(c => c.IsOk && c.Ref == item.Id && c.Entity == "Property").AsNoTracking().ToList();
                        if (queryFullProperty.Count > 0)
                        {
                            foreach (var property in queryFullProperty)
                            {
                                var vmm = new VMProduct.ProductProperty
                                {
                                    Key = property.Keys,
                                    Value = property.Value,
                                };
                                list3.Add(vmm);
                            }

                            vm.PropertyList = list3;
                        }
                        else
                        {
                            vm.PropertyList = new List<VMProduct.ProductProperty>();
                        }



                        var querySummary = db.Table_Product_Summary.FirstOrDefault(c =>
                            c.ProductRef == item.Id && c.IsMain && c.IsOk && !c.IsDelete);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }

                            vm.Fee = querySummary.Fee;
                        }
                        else
                        {
                            if (item.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }
                            vm.Fee = querySummary.Fee;
                        }


                        list.Add(vm);
                    }

                }
            }
            catch (Exception e)
            {

            }
            return list;
        }

        #region color

        public string AddNewProductSize(VMProduct.ViewModelProductSize values, Guid UserId)
        {
            try
            {
                var userRef = UserId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "CL-" + random.Next(40000, 49999);
                var isok = true;




                var query = db.Table_Product_Size.Add(new Table_Product_Size()
                {
                    Id = id,
                    Code = code,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    TertiaryTitle = values.SecondaryTitle,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Version = 1,
                    IsOk = false,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    SizeTitle = values.SizeTitle,
                    Sort = values.Sort,

                });
                db.Table_Product_Size.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string AddNewProductColor(VMProduct.ViewModelProductColor values, Guid UserId)
        {
            try
            {
                var userRef = UserId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "CL-" + random.Next(40000, 49999);
                var isok = true;




                var query = db.Table_Product_Color.Add(new Table_Product_Color()
                {
                    Id = id,
                    Code = code,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    TertiaryTitle = values.SecondaryTitle,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Version = 1,
                    IsOk = false,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    ColorCode = values.ColorCode,

                });
                db.Table_Product_Color.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        #endregion

        #region Selection

        public string AddNewConfiguration(VMProduct.ViewModelConfiguration values, Guid userId)
        {
            try
            {
                var userRef = userId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(40000, 49999);
                var isok = true;
                var catRef = values.CategoriesRef;
                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }

                if (values.CategoriesRef == Guid.Empty)
                {
                    catRef = null;
                }

                var query = db.Table_Selection_Configuration.Add(new Table_Selection_Configuration()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    CategoriesRef = catRef,
                    Sort = values.Sort,
                    Count = values.Count,

                });
                db.Table_Selection_Configuration.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public List<VMProduct.ViewModelConfiguration> RepositoryMainConfigurationForShow()
        {
            var list = new List<VMProduct.ViewModelConfiguration>();
            try
            {
                var query = db.Table_Selection_Configuration.Where(c => c.IsOk && !c.IsDelete)
                    .OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var config in query)
                    {
                        var vm = new VMProduct.ViewModelConfiguration()
                        {
                            Id = config.Id,
                            Code = config.Code,
                            IsOk = config.IsOk,
                            PrimaryTitle = config.PrimaryTitle,
                            SecondaryTitle = config.SecondaryTitle,
                            CreatorDate = config.CreatorDate,
                            CategoriesRef = config.CategoriesRef,
                            Sort = config.Sort,
                            Url = config.Url,
                        };

                        var querycatName =
                            db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == vm.CategoriesRef);
                        if (querycatName != null)
                        {

                            vm.CategoryTitle = querycatName.PrimaryTitle;

                        }

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.ViewModelConfiguration> RepositoryMainConfiguration()
        {
            var list = new List<VMProduct.ViewModelConfiguration>();
            try
            {
                var query = db.Table_Selection_Configuration.Where(c => !c.IsDelete)
                    .OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var config in query)
                    {
                        var vm = new VMProduct.ViewModelConfiguration()
                        {
                            Id = config.Id,
                            Code = config.Code,
                            IsOk = config.IsOk,
                            PrimaryTitle = config.PrimaryTitle,
                            SecondaryTitle = config.SecondaryTitle,
                            CreatorDate = config.CreatorDate,
                            CategoriesRef = config.CategoriesRef,
                            Sort = config.Sort,


                        };

                        var querycatName =
                            db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == vm.CategoriesRef);
                        if (querycatName != null)
                        {

                            vm.CategoryTitle = querycatName.PrimaryTitle;

                        }

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }
        public List<VMProduct.ViewModelConfiguration> RepositoryMainConfigurationSearch(string searchnew)
        {
            var list = new List<VMProduct.ViewModelConfiguration>();
            try
            {
                var search = searchnew.Replace("	", "");
                var query = db.Table_Selection_Configuration.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search) || c.SecondaryTitle.Contains(search))
                    .OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var config in query)
                    {
                        var vm = new VMProduct.ViewModelConfiguration()
                        {
                            Id = config.Id,
                            Code = config.Code,
                            IsOk = config.IsOk,
                            PrimaryTitle = config.PrimaryTitle,
                            SecondaryTitle = config.SecondaryTitle,
                            CreatorDate = config.CreatorDate,
                            CategoriesRef = config.CategoriesRef,
                            Sort = config.Sort,


                        };

                        var querycatName =
                            db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == vm.CategoriesRef);
                        if (querycatName != null)
                        {

                            vm.CategoryTitle = querycatName.PrimaryTitle;

                        }

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }


        #endregion

        #region Selection

        public List<VMProduct.VmMainProduct> RepositorySelectionProduct(Guid configRef)
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product_Selection.Where(c => c.IsOk && !c.IsDelete && c.ConfigurationRef == configRef)
                    .OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var selection in query)
                    {
                        var queryProduct = db.Table_Product.Where(c => c.IsOk && !c.IsDelete && c.Id == selection.ProductRef)
                               .OrderByDescending(c => c.ModifierDate).AsNoTracking().ToList();
                        if (queryProduct.Count > 0)
                        {
                            foreach (var product in queryProduct)
                            {
                                var vm = new VMProduct.VmMainProduct
                                {
                                    Id = product.Id,
                                    Code = product.Code,
                                    IsOk = product.IsOk,
                                    PrimaryTitle = product.PrimaryTitle,
                                    SecondaryTitle = product.SecondaryTitle,
                                    TertiaryTitle = product.TertiaryTitle,
                                    Url = product.Url,
                                    CreatorDateTime = product.CreatorDate,
                                    Note = product.Note,
                                    CategoriesRef = product.CategoriesRef,
                                    Discount = product.Discount,
                                };

                                var queryfile =
                                    db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id && c.IsMain);
                                if (queryfile != null)
                                {
                                    vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                                  queryfile.FileExtensions;
                                }
                                else
                                {
                                    vm.FileName = "/Static/Content/Images/";
                                }


                                var querySummary =
                                    db.Table_Product_Summary.FirstOrDefault(c =>
                                        c.ProductRef == product.Id && c.IsOk && c.IsMain);
                                if (querySummary != null)
                                {
                                    if (querySummary.Quantity > 0)
                                    {
                                        vm.QuantityTitle = "موجود";
                                        vm.QuantityClass = "product-card__badge--yes";
                                    }
                                    else
                                    {
                                        vm.QuantityTitle = "ناموجود";
                                        vm.QuantityClass = "product-card__badge--no";
                                    }


                                    vm.Fee = querySummary.Fee;

                                }
                                else
                                {
                                    if (product.Quantity > 0)
                                    {
                                        vm.QuantityTitle = "موجود";
                                        vm.QuantityClass = "product-card__badge--yes";
                                    }
                                    else
                                    {
                                        vm.QuantityTitle = "ناموجود";
                                        vm.QuantityClass = "product-card__badge--no";
                                    }


                                    vm.Fee = product.Fee;
                                }


                                var queryCategories =
                                    db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == product.CategoriesRef);
                                if (queryCategories != null)
                                {
                                    vm.CategoreisTitle = queryCategories.PrimaryTitle;
                                    vm.CategoriesRef = queryCategories.Id;

                                }



                                list.Add(vm);
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        #endregion


        #region Discount
        public List<VMProduct.ViewModelDiscount> RepositoryDiscountManagment()
        {
            var list = new List<VMProduct.ViewModelDiscount>();
            try
            {


                var query = db.Table_Price_DisCount.AsNoTracking()
                    .ToList();

                if (query.Count > 0)
                {

                    foreach (var discount in query)
                    {

                        var vm = new VMProduct.ViewModelDiscount()
                        {
                            Id = discount.Id,
                            Code = discount.Code,
                            IsDelete = discount.IsDelete,
                            IsOk = discount.IsOk,
                            DiscountCode = discount.DiscountCode,
                            EndDate = discount.EndDate,
                            StartDate = discount.StartDate,
                            UsableCount = discount.UsableCount,
                            DiscountPercent = discount.DiscountPrecent,
                        };


                        list.Add(vm);
                    }
                }


            }
            catch (Exception e)
            {

            }
            return list;
        }
        public string AddNewDiscount(VMProduct.ViewModelDiscount values, Guid userId)
        {
            try
            {

                var userRef = userId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(20000, 29999);
                var isok = false;


                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }
                var query = db.Table_Price_DisCount.Add(new Table_Price_DisCount()
                {
                    Id = id,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    DiscountCode = values.DiscountCode,
                    DiscountPrecent = values.DiscountPercent,
                    EndDate = values.EndDate,
                    StartDate = values.StartDate,
                    UsableCount = values.UsableCount,
                });
                db.Table_Price_DisCount.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public List<VMProduct.ViewModelDiscount> RepositoryDiscountMangmentByid(Guid Id)
        {
            var list = new List<VMProduct.ViewModelDiscount>();
            try
            {

                var query = db.Table_Price_DisCount.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMProduct.ViewModelDiscount
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        IsDelete = query.IsDelete,
                        DiscountCode = query.DiscountCode,
                        StartDate = query.StartDate,
                        EndDate = query.EndDate,
                        DiscountPercent = query.DiscountPrecent,
                        UsableCount = query.UsableCount,

                    };

                    list.Add(vm);
                }

            }
            catch (Exception e)
            {

            }
            return list;
        }
        public string RepositoryDiscountMangmentEditById(VMProduct.ViewModelDiscount values, Guid Userid)
        {
            try
            {

                var userRef = Userid;
                var query = db.Table_Price_DisCount.FirstOrDefault(c => c.Id == values.Id);

                if (query != null)
                {
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.StartDate = values.StartDate;
                    query.DiscountCode = values.DiscountCode;
                    query.DiscountPrecent = values.DiscountPercent;
                    query.EndDate = values.EndDate;
                    query.UsableCount = values.UsableCount;



                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public string AddNewProductDiscount(VMProduct.ViewModelProductDiscount values, Guid userId)
        {
            try
            {

                var userRef = userId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(50000, 59999);
                var isok = true;
                var catRef = values.CategoriesRef;
                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }

                if (values.CategoriesRef == Guid.Empty)
                {
                    catRef = null;
                }

                //var query = db.Table_DiscountOnProducts.Add(new Table_DiscountOnProducts()
                //{
                //    Id = id,
                //    PrimaryTitle = values.PrimaryTitle,
                //    SecondaryTitle = values.SecondaryTitle,
                //    CreatorDate = DateTime.Now,
                //    ModifierDate = DateTime.Now,
                //    Code = code,
                //    Version = 1,
                //    IsOk = isok,
                //    CreatorRef = userRef,
                //    ModifierRef = userRef,
                //    IsDelete = false,
                //    CategoriesRef = catRef,
                //    Discount = values.Discount,
                //    Count = values.Count,

                //});
                //db.Table_DiscountOnProducts.Add(query);
                //db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        public string DeleteProductDiscount(Guid id)
        {
            try
            {

                //var query = db.Table_DiscountOnProducts.FirstOrDefault(c => c.Id == id);
                //if (query != null)
                //{
                //    switch (query.IsOk)
                //    {
                //        case true:
                //            {
                //                return "true";

                //            }
                //        case false:
                //            {
                //                db.Table_DiscountOnProducts.Remove(query);
                //                db.SaveChanges();
                //                return "success";
                //            }


                //}



                //}

                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }


        }
        public string ChangeProductDiscountStatus(Guid Id)
        {

            //var Result = db.Table_DiscountOnProducts.FirstOrDefault(c => c.Id == Id);
            //if (Result != null)
            //{

            //    switch (Result.IsOk)
            //    {
            //        case true:
            //            {
            //                Result.IsOk = false;
            //                break;
            //            }
            //        case false:
            //            {
            //                Result.IsOk = true;
            //                break;
            //            }
            //    }


            //}

            db.SaveChanges();

            return "Sucsess";

        }
        public List<VMProduct.ViewModelProductDiscount> RepositoryMainProductDiscount()
        {
            var list = new List<VMProduct.ViewModelProductDiscount>();
            try
            {

                //var query = db.Table_DiscountOnProducts.Where(c => !c.IsDelete)
                //    .OrderBy(c => c.Code).AsNoTracking().ToList();

                //if (query.Count > 0)
                //{
                //    foreach (var config in query)
                //    {
                //        var vm = new VMOrders.ViewModelProductDiscount()
                //        {
                //            Id = config.Id,
                //            Code = config.Code,
                //            IsOk = config.IsOk,
                //            PrimaryTitle = config.PrimaryTitle,
                //            SecondaryTitle = config.SecondaryTitle,
                //            CreatorDate = config.CreatorDate,
                //            CategoriesRef = config.CategoriesRef,
                //            Discount = config.Discount,
                //        };

                //        var querycatName =
                //            db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == vm.CategoriesRef);
                //        if (querycatName != null)
                //        {
                //            vm.CategoryTitle = querycatName.PrimaryTitle;
                //        }

                //        list.Add(vm);
                //    }
                //}
            }
            catch (Exception e)
            {

            }

            return list;
        }
        //public  List<VMOrders.ViewModelProductDiscount> RepositoryProductDiscountMangmentByid(Guid Id)
        //{
        //    var list = new List<VMOrders.ViewModelProductDiscount>();
        //    try
        //    {
        //       
        //        var query = db.Table_DiscountOnProducts.FirstOrDefault(c => c.Id == Id);
        //        if (query != null)
        //        {
        //            var vm = new VMOrders.ViewModelProductDiscount()
        //            {
        //                Id = query.Id,
        //                Code = query.Code,
        //                IsOk = query.IsOk,
        //                PrimaryTitle = query.PrimaryTitle,
        //                SecondaryTitle = query.SecondaryTitle,
        //                CreatorDate = query.CreatorDate,
        //                IsDelete = query.IsDelete,
        //                ModifierDate = query.ModifierDate,
        //                ModifierRef = query.ModifierRef,
        //                Count = query.Count,
        //                Discount = query.Discount,
        //                CategoriesRef = query.CategoriesRef,
        //                CreatorRef = query.CreatorRef,
        //            };

        //            list.Add(vm);
        //        }

        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return list;
        //}
        //public  string RepositoryProductDiscountEditById(VMOrders.ViewModelProductDiscount values, Guid userId)
        //{
        //    try
        //    {
        //       
        //        var userRef = userId;
        //        var query = db.Table_DiscountOnProducts.FirstOrDefault(c => c.Id == values.Id);
        //        var catRef = values.CategoriesRef;
        //        if (values.CategoriesRef == Guid.Empty)
        //        {
        //            catRef = null;
        //        }

        //        if (query != null)
        //        {
        //            query.PrimaryTitle = values.PrimaryTitle;
        //            query.SecondaryTitle = values.SecondaryTitle;
        //            query.ModifierDate = DateTime.Now;
        //            query.ModifierRef = userRef;
        //            query.Version++;
        //            query.CategoriesRef = catRef;
        //            query.Discount = values.Discount;
        //            query.Count = values.Count;



        //            db.SaveChanges();
        //        }

        //        return "Success";
        //    }
        //    catch (Exception e)
        //    {
        //        return "Application Error : " + e.Message;
        //    }
        //}
        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentByDiscountRef(Guid id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate).AsNoTracking()
                    .ToList();
                if (queryProduct.Count > 0)
                {
                    foreach (var product in queryProduct)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            Fee = product.Fee,
                            CreatorDateTime = product.CreatorDate,
                            Quantity = product.Quantity,
                            Note = product.Note,

                        };


                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);

                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }
                        //var queryFind = db.Table_Discount_Product_Selection.AsNoTracking().ToList()
                        //    .Exists(c => c.ProductRef == product.Id && c.DiscountRef == id);
                        //if (queryFind)
                        //{
                        //    vm.IsOkTitle = "انتخاب شده است";
                        //    vm.IsOkClass = "btn btn-success";
                        //}
                        //else
                        //{
                        //    vm.IsOkTitle = "انتخاب  نشده است";
                        //    vm.IsOkClass = "btn btn-danger";
                        //}


                        list.Add(vm);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }
        public List<VMProduct.VmMainProductMangement> RepositoryMainProductsMangmentByProductRefAndSearchKey(
            Guid id, string SearchKey)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var queryProduct = db.Table_Product.OrderByDescending(c => c.CreatorDate)
                    .Where(c => c.PrimaryTitle.Contains(SearchKey)).AsNoTracking().ToList();
                if (queryProduct.Count > 0)
                {
                    foreach (var product in queryProduct)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            Fee = product.Fee,
                            CreatorDateTime = product.CreatorDate,
                            Discount = product.Discount,
                            Quantity = product.Quantity,
                            Note = product.Note,

                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        //var queryFind = db.Table_Discount_Product_Selection.AsNoTracking().ToList()
                        //    .Exists(c => c.ProductRef == product.Id && c.DiscountRef == id);
                        //if (queryFind)
                        //{
                        //    vm.IsOkTitle = "انتخاب شده است";
                        //    vm.IsOkClass = "btn btn-success";
                        //}
                        //else
                        //{
                        //    vm.IsOkTitle = "انتخاب  نشده است";
                        //    vm.IsOkClass = "btn btn-danger";
                        //}


                        list.Add(vm);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }

        //public  string RepositoryMainProductAddToDiscountProductSelection(Guid DiscountRef, Guid productRef)
        //{
        //    var message = "Application Error : Null";
        //    try
        //    {
        //       
        //        var query = db.Table_Discount_Product_Selection.ToList()
        //            .Exists(c => c.DiscountRef == DiscountRef && c.ProductRef == productRef);

        //        if (!query)
        //        {
        //            var queryAdd = db.Table_Discount_Product_Selection.Add(new Table_Discount_Product_Selection
        //            {
        //                Id = Guid.NewGuid(),
        //                Code = SepidyarHesabCodeGenerator.GenerateCode(),
        //                DiscountRef = DiscountRef,
        //                CreatorDate = DateTime.Now,
        //                CreatorRef = Guid.NewGuid(),
        //                IsDelete = false,
        //                IsOk = true,
        //                ModifierDate = DateTime.Now,
        //                ModifierRef = Guid.NewGuid(),
        //                PrimaryTitle = "",
        //                ProductRef = productRef,
        //                SecondaryTitle = "",
        //                TertitatyTitle = "",
        //                Version = 1,
        //                Sort = 1,
        //            });
        //            db.Table_Discount_Product_Selection.Add(queryAdd);
        //            db.SaveChanges();
        //            return "Success";
        //        }
        //        else
        //        {
        //            var queryDelete = db.Table_Discount_Product_Selection.FirstOrDefault(c =>
        //                c.DiscountRef == DiscountRef && c.ProductRef == productRef);
        //            if (queryDelete != null)
        //            {
        //                db.Table_Discount_Product_Selection.Remove(queryDelete);
        //                db.SaveChanges();
        //                return "Success";
        //            }
        //            else
        //            {
        //                return "Application Error : Not Found";
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        message = "Application Error : " + e.Message;
        //    }

        //    return message;
        //}


        //public  string RepositoryMainProductAddALLProductSelection(Guid DiscountRef)
        //{
        //    var message = "Application Error : Null";
        //    try
        //    {
        //       
        //        var productQuery = db.Table_Product.AsNoTracking().ToList();
        //        var DiscountQuery = db.Table_Discount_Product_Selection.AsNoTracking().Where(c => c.DiscountRef == DiscountRef).ToList();
        //        if (productQuery != null)
        //        {
        //            foreach (var product in productQuery)
        //            {

        //                if (DiscountQuery.Count > 0)
        //                {
        //                    foreach (var discount in DiscountQuery)
        //                    {
        //                        if (discount.ProductRef != product.Id)
        //                        {
        //                            var queryAdd = db.Table_Discount_Product_Selection.Add(new Table_Discount_Product_Selection
        //                            {
        //                                Id = Guid.NewGuid(),
        //                                Code = SepidyarHesabCodeGenerator.GenerateCode(),
        //                                DiscountRef = DiscountRef,
        //                                CreatorDate = DateTime.Now,
        //                                CreatorRef = Guid.NewGuid(),
        //                                IsDelete = false,
        //                                IsOk = true,
        //                                ModifierDate = DateTime.Now,
        //                                ModifierRef = Guid.NewGuid(),
        //                                PrimaryTitle = "",
        //                                ProductRef = product.Id,
        //                                SecondaryTitle = "",
        //                                TertitatyTitle = "",
        //                                Version = 1,
        //                                Sort = 1,
        //                            });
        //                            db.Table_Discount_Product_Selection.Add(queryAdd);
        //                            db.SaveChanges();
        //                            break;
        //                        }
        //                        return "Success";
        //                    }
        //                }
        //                else
        //                {
        //                    var queryAdd = db.Table_Discount_Product_Selection.Add(new Table_Discount_Product_Selection
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        Code = SepidyarHesabCodeGenerator.GenerateCode(),
        //                        DiscountRef = DiscountRef,
        //                        CreatorDate = DateTime.Now,
        //                        CreatorRef = Guid.NewGuid(),
        //                        IsDelete = false,
        //                        IsOk = true,
        //                        ModifierDate = DateTime.Now,
        //                        ModifierRef = Guid.NewGuid(),
        //                        PrimaryTitle = "",
        //                        ProductRef = product.Id,
        //                        SecondaryTitle = "",
        //                        TertitatyTitle = "",
        //                        Version = 1,
        //                        Sort = 1,
        //                    });
        //                    db.Table_Discount_Product_Selection.Add(queryAdd);
        //                    db.SaveChanges();


        //                }
        //            }
        //            return "Success";
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        message = "Application Error : " + e.Message;
        //    }

        //    return message;
        //}
        //public  string RepositoryMainProductRemoveALLProductSelection(Guid DiscountRef)
        //{
        //    var message = "Application Error : Null";
        //    try
        //    {
        //       
        //        var queryDelete = db.Table_Discount_Product_Selection.Where(c =>
        //                 c.DiscountRef == DiscountRef).ToList();
        //        if (queryDelete != null)
        //        {
        //            foreach (var item in queryDelete)
        //            {

        //                db.Table_Discount_Product_Selection.Remove(item);
        //                db.SaveChanges();
        //            }
        //            return "Success";
        //        }
        //        else
        //        {
        //            return "Application Error : Not Found";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        message = "Application Error : " + e.Message;
        //    }

        //    return message;
        //}
        #endregion
        #region CategoreisProduct

        public List<VMProduct.VmMainProduct> RepositoryMainSelectionCategoreisProducts(Guid? catRef)
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product.Where(c => c.IsOk && !c.IsDelete && c.CategoriesRef == catRef)
                    .OrderByDescending(c => c.ModifierDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProduct
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            Discount = product.Discount,

                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id && c.IsMain);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        var querySummary =
                            db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsOk && c.IsMain);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = querySummary.Fee;

                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = product.Fee;
                        }




                        var queryCategories =
                            db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == product.CategoriesRef);
                        if (queryCategories != null)
                        {
                            vm.CategoreisTitle = queryCategories.PrimaryTitle;
                            vm.CategoriesRef = queryCategories.Id;

                        }

                        //var queryDiscount = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.CategoriesRef == vm.CategoriesRef);
                        //if (queryDiscount != null)
                        //{
                        //    vm.Discount = queryDiscount.Discount;
                        //}


                        //var queryProductDiscount = db.Table_Discount_Product_Selection.AsNoTracking().FirstOrDefault(c => c.ProductRef == vm.Id);
                        //if (queryProductDiscount != null)
                        //{
                        //    var queryFindCatRef = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.Id == queryProductDiscount.DiscountRef);
                        //    if (queryFindCatRef != null)
                        //    {
                        //        vm.ProductDiscount = queryFindCatRef.Discount;
                        //    }
                        //}

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        #endregion

        #region Sub Categories Product


        public string AddNewCategories(VMProduct.ViewModelCategoreis values, HttpPostedFileBase file, Guid UserId)
        {
            try
            {

                var userRef = UserId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(20000, 29999);
                var isok = false;
                var parentRef = values.ParentRef;
                var ismain = values.IsMain;

                if (values.ParentRef == Guid.Empty)
                {
                    parentRef = null;
                }

                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }
                var query = db.Table_Product_Category.Add(new Table_Product_Category
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = true,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    IsMain = ismain,
                    ParentRef = parentRef,
                    TertiaryTitle = values.SecondaryTitle,
                    Sort = values.Sort,
                    Url = values.Url,
                });
                db.Table_Product_Category.Add(query);
                db.SaveChanges();

                var filename = "Default";
                var fileExtention = "png";
                var filelenght = 200;
                if (file != null)
                {
                    filelenght = file.ContentLength;
                    filename = "Category_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    var codee = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                    var idfile = Guid.NewGuid();
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainCategories + filename + fileExtention);
                    file.SaveAs(pathCombine);
                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload
                    {
                        Id = idfile,
                        Code = codee,
                        Tables = "Table_Product_Category",
                        Schemas = "General",
                        Ref = id,
                        FileExtensions = fileExtention,
                        FileLenght = filelenght,
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
                        Path = "",
                        LanguageRef = Guid.Empty
                    });
                    db.Table_File_Upload.Add(qAddPic);
                    db.SaveChanges();

                }


                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public List<VMProduct.VmMainProduct> RepositoryMainProductsByMainCategories(Guid id)
        {
            /*var list = new List<VMProduct.VmMainProduct>();
            try
            {
           
                var query = db.Table_Product_Category.Where(c => (c.IsOk && c.ParentRef == id )|| c.Id == id ).AsNoTracking().ToList();

                if (query.Count > 0)
                {
                    foreach (var selection in query)
                    {
                        var SubId = selection.Id;
                        var product = db.Table_Product.FirstOrDefault(c => c.CategoriesRef == SubId);
                        if (product != null)
                        {
                            var vm = new VMProduct.VmMainProduct
                            {
                                Id = product.Id,
                                Code = product.Code,
                                IsOk = product.IsOk,
                                PrimaryTitle = product.PrimaryTitle,
                                SecondaryTitle = product.SecondaryTitle,
                                TertiaryTitle = product.TertiaryTitle,
                                Url = product.Url,
                                Fee = product.Fee,
                                CreatorDateTime = product.CreatorDate,
                                Discount = product.Discount,
                                Quantity = product.Quantity,
                                Note = product.Note,

                            };


                            var queryfile =
                                db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id && c.IsMain);
                            if (queryfile != null)
                            {
                                vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                              queryfile.FileExtensions;
                            }
                            else
                            {
                                vm.FileName = "/Static/Content/Images/";
                            }

                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }





                            list.Add(vm);
                        }
                    }
                }
                
            }
            catch (Exception e)
            {

            }
            return list;*/
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product_Category.Where(c => (c.IsOk && c.ParentRef == id) || c.Id == id)
                    .OrderByDescending(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var selection in query)
                    {
                        var SubId = selection.Id;
                        decimal a = 0;
                        var queryProduct = db.Table_Product.Where(c => c.CategoriesRef == SubId)
                               .OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                        if (queryProduct.Count > 0)
                        {
                            foreach (var product in queryProduct)
                            {
                                var vm = new VMProduct.VmMainProduct
                                {
                                    Id = product.Id,
                                    Code = product.Code,
                                    IsOk = product.IsOk,
                                    PrimaryTitle = product.PrimaryTitle,
                                    SecondaryTitle = product.SecondaryTitle,
                                    TertiaryTitle = product.TertiaryTitle,
                                    Url = product.Url,
                                    Fee = product.Fee,
                                    CreatorDateTime = product.CreatorDate,
                                    Note = product.Note,
                                    Discount = product.Discount,
                                };
            

                                var queryfile =
                                    db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id && c.IsMain);
                                if (queryfile != null)
                                {
                                    vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                                  queryfile.FileExtensions;
                                }
                                else
                                {
                                    vm.FileName = "/Static/Content/Images/";
                                }




                                var queryCategories =
                                    db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == product.CategoriesRef);
                                if (queryCategories != null)
                                {
                                    vm.CategoreisTitle = queryCategories.PrimaryTitle;
                                    vm.CategoriesRef = queryCategories.Id;

                                }

                                //var queryDiscount = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.CategoriesRef == vm.CategoriesRef);
                                //if (queryDiscount != null)
                                //{
                                //    vm.Discount = queryDiscount.Discount;
                                //}


                                //var queryProductDiscount = db.Table_Discount_Product_Selection.AsNoTracking().FirstOrDefault(c => c.ProductRef == vm.Id);
                                //if (queryProductDiscount != null)
                                //{
                                //    var queryFindCatRef = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.Id == queryProductDiscount.DiscountRef);
                                //    if (queryFindCatRef != null)
                                //    {
                                //        vm.ProductDiscount = queryFindCatRef.Discount;
                                //    }
                                //}
                                var querySummary = db.Table_Product_Summary.FirstOrDefault(c =>
                                    c.ProductRef == product.Id && c.IsMain && c.IsOk && !c.IsDelete);
                                if (querySummary != null)
                                {
                                    vm.Fee = querySummary.Fee;
                                    if (querySummary.Quantity > 0)
                                    {
                                        vm.QuantityTitle = "موجود";
                                        vm.QuantityClass = "product-card__badge--yes";
                                        list.Add(vm);

                                    }
                                    else
                                    {
                                        vm.QuantityTitle = "ناموجود";
                                        vm.QuantityClass = "product-card__badge--no";
                                    }

                                }
                                else
                                {
                                    vm.Fee = product.Fee;
                                    if (product.Quantity > 0)
                                    {
                                        list.Add(vm);
                                        vm.QuantityTitle = "موجود";
                                        vm.QuantityClass = "product-card__badge--yes";
                                    }
                                    else
                                    {
                                        vm.QuantityTitle = "ناموجود";
                                        vm.QuantityClass = "product-card__badge--no";
                                    }
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        public List<VMProduct.VmMainProduct> RepositoryMainProductsBySubCategoreis(Guid id)
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product.Where(c => c.IsOk && c.CategoriesRef == id)
                    .OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProduct
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            Fee = product.Fee,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            Discount = product.Discount
                        };
                        var querySummary = db.Table_Product_Summary.FirstOrDefault(c =>
                            c.ProductRef == product.Id && c.IsMain && c.IsOk && !c.IsDelete);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }

                            vm.Fee = querySummary.Fee;
                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }
                            vm.Fee = product.Fee;
                        }

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id && c.IsMain);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }




                        var queryCategories =
                            db.Table_Product_Category.FirstOrDefault(c => c.Id == product.CategoriesRef && c.IsOk && !c.IsDelete);
                        if (queryCategories != null)
                        {
                            vm.CategoreisTitle = queryCategories.PrimaryTitle;
                            vm.CategoriesRef = queryCategories.Id;

                        }
                        else
                        {
                            LogWriter.Logger("Application Error : " + "Table_Product_Category null", DateTime.Now.ToString(), "5846");
                        }

                        list.Add(vm);
                    }
                }
                else
                {
                    LogWriter.Logger("Application Error : " + "Table_Product null", DateTime.Now.ToString(), "5847");
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    LogWriter.Logger("Application Error : " + e.InnerException.Message, DateTime.Now.ToString(), "5849");
                }
                else
                {
                    LogWriter.Logger("Application Error : " + e.Message, DateTime.Now.ToString(), "5848");
                }
            }

            return list;
        }

        #endregion


        #region Product Show All Configuration 

        public List<VMProduct.VmMainProductMangement> RepositoryMainProductShowAllProductByConfigurationRef(Guid id)
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {

                var query = db.Table_Product_Selection.Where(c => c.ConfigurationRef == id).OrderByDescending(c => c.ModifierDate).AsNoTracking().ToList();

                if (query.Count > 0)
                {
                    foreach (var selection in query)
                    {
                        var product = db.Table_Product.FirstOrDefault(c => c.Id == selection.ProductRef);
                        if (product != null)
                        {
                            var vm = new VMProduct.VmMainProductMangement
                            {
                                Id = product.Id,
                                Code = product.Code,
                                IsOk = product.IsOk,
                                PrimaryTitle = product.PrimaryTitle,
                                SecondaryTitle = product.SecondaryTitle,
                                TertiaryTitle = product.TertiaryTitle,
                                Url = product.Url,
                                Fee = product.Fee,
                                CreatorDateTime = product.CreatorDate,
                                Discount = product.Discount,
                                Quantity = product.Quantity,
                                Note = product.Note,

                            };
                            var querySummary = db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsMain && c.IsOk && !c.IsDelete);
                            if (querySummary != null)
                            {
                                if (querySummary.Quantity > 0)
                                {
                                    vm.QuantityTitle = "موجود";
                                    vm.QuantityClass = "product-card__badge--yes";
                                }
                                else
                                {
                                    vm.QuantityTitle = "ناموجود";
                                    vm.QuantityClass = "product-card__badge--no";
                                }
                                vm.Quantity = querySummary.Quantity;
                                vm.Fee = querySummary.Fee;
                            }
                            else
                            {
                                if (product.Quantity > 0)
                                {
                                    vm.QuantityTitle = "موجود";
                                    vm.QuantityClass = "product-card__badge--yes";
                                }
                                else
                                {
                                    vm.QuantityTitle = "ناموجود";
                                    vm.QuantityClass = "product-card__badge--no";
                                }

                                vm.Quantity = product.Quantity;
                                vm.Fee = product.Fee;
                            }

                            var queryfile =
                                db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id);
                            if (queryfile != null)
                            {
                                vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                              queryfile.FileExtensions;
                            }
                            else
                            {
                                vm.FileName = "/Static/Content/Images/";
                            }



                            var queryFind = db.Table_Product_Selection.AsNoTracking().ToList()
                                .Exists(c => c.ProductRef == product.Id && c.ConfigurationRef == id);
                            if (queryFind)
                            {
                                vm.IsOkTitle = "انتخاب شده است";
                                vm.IsOkClass = "btn btn-success";
                            }
                            else
                            {
                                vm.IsOkTitle = "انتخاب  نشده است";
                                vm.IsOkClass = "btn btn-danger";
                            }

                            //var queryDiscount = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.CategoriesRef == product.CategoriesRef);
                            //if (queryDiscount != null)
                            //{
                            //    vm.Discount = queryDiscount.Discount;
                            //}


                            //var queryProductDiscount = db.Table_Discount_Product_Selection.AsNoTracking().FirstOrDefault(c => c.ProductRef == vm.Id);
                            //if (queryProductDiscount != null)
                            //{
                            //    var queryFindCatRef = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.Id == queryProductDiscount.DiscountRef);
                            //    if (queryFindCatRef != null)
                            //    {
                            //        vm.ProductDiscount = queryFindCatRef.Discount;
                            //    }
                            //}

                            list.Add(vm);
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        #endregion

        #region latestProducts

        public List<VMProduct.VmMainProduct> RepositoryMainlatestProducts()
        {
            var list = new List<VMProduct.VmMainProduct>();
            try
            {

                var query = db.Table_Product.Where(c => c.IsOk && !c.IsDelete)
                    .OrderByDescending(c => c.ModifierDate).AsNoTracking().Take(9).ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProduct
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                            Url = product.Url,
                            CreatorDateTime = product.CreatorDate,
                            Note = product.Note,
                            Discount = product.Discount,
                        };

                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == product.Id && c.IsMain);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Products/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
                        }


                        var querySummary =
                            db.Table_Product_Summary.FirstOrDefault(c =>
                                c.ProductRef == product.Id && c.IsOk && c.IsMain);
                        if (querySummary != null)
                        {
                            if (querySummary.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = querySummary.Fee;

                        }
                        else
                        {
                            if (product.Quantity > 0)
                            {
                                vm.QuantityTitle = "موجود";
                                vm.QuantityClass = "product-card__badge--yes";
                            }
                            else
                            {
                                vm.QuantityTitle = "ناموجود";
                                vm.QuantityClass = "product-card__badge--no";
                            }


                            vm.Fee = product.Fee;
                        }




                        var queryCategories =
                            db.Table_Product_Category.AsNoTracking().FirstOrDefault(c => c.Id == product.CategoriesRef);
                        if (queryCategories != null)
                        {
                            vm.CategoreisTitle = queryCategories.PrimaryTitle;
                            vm.CategoriesRef = queryCategories.Id;

                        }

                        //var queryDiscount = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.CategoriesRef == vm.CategoriesRef);
                        //if (queryDiscount != null)
                        //{
                        //    vm.Discount = queryDiscount.Discount;
                        //}


                        //var queryProductDiscount = db.Table_Discount_Product_Selection.AsNoTracking().FirstOrDefault(c => c.ProductRef == vm.Id);
                        //if (queryProductDiscount != null)
                        //{
                        //    var queryFindCatRef = db.Table_DiscountOnProducts.AsNoTracking().FirstOrDefault(c => c.Id == queryProductDiscount.DiscountRef);
                        //    if (queryFindCatRef != null)
                        //    {
                        //        vm.ProductDiscount = queryFindCatRef.Discount;
                        //    }
                        //}

                        list.Add(vm);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }

        #endregion
        //End------------------------------------
        
    }
}
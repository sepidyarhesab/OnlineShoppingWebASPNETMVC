using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.Repository.General;
using SepidyarHesabExtensions.Classes;
using OrdersOrders.ViewModels.Orders;
using OrdersSettings.Repository.Settings;
using Rozhano_WebApplication2.Extensions;

namespace OrdersOrders.Repository.Orders
{
    public static class RepOrders
    {
        public static string RepositorySubmitOrders(VMOrders.VmOrderSubmit values, List<VMOrders.VmOrderSubmit> carts, HttpPostedFileBase file, string phone, decimal transfer, string browser, string ip, decimal sumdis, string DisCode, bool DisUse)
        {
            var code = "error";
            try
            {
                var idusers = Guid.NewGuid();
                var db = new Orders_Entities();
                var tick = DateTime.Now.Ticks;
                var query = db.Table_User.ToList().Exists(c => c.Phone == phone);
                if (query)
                {
                    var queryusers = db.Table_User.FirstOrDefault(c => c.Phone == phone);
                    if (queryusers != null)
                    {

                        idusers = queryusers.Id;
                        #region Order

                        decimal sum = 0;
                        foreach (var cart in carts)
                        {
                            var sum2 = cart.Quantity * cart.Fee;
                            sum += sum2;
                            if (cart.DisCode != "" && cart.DisCode != null)
                            {
                                if (cart.DisUse)
                                {
                                    var queryDis = db.Table_Discount.FirstOrDefault(c => c.DiscountCode == cart.DisCode);
                                    if (queryDis != null)
                                    {
                                        if (queryDis.DiscountQuantity > 0)
                                        {
                                            queryDis.DiscountQuantity--;
                                            if (queryDis.DiscountQuantity == 0)
                                            {
                                                queryDis.IsOk = false;
                                            }
                                        }

                                        if (queryDis.DiscountCount > 0)
                                        {
                                            queryDis.DiscountPercent = queryDis.DiscountPercent - 10;
                                            queryDis.DiscountCount--;
                                            if (queryDis.DiscountCount == 0)
                                            {
                                                queryDis.IsOk = false;
                                            }
                                        }
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else if (DisCode != "" && DisCode != null)
                            {
                                if (DisUse)
                                {
                                    var queryDis = db.Table_Discount.FirstOrDefault(c => c.DiscountCode == DisCode);
                                    if (queryDis != null)
                                    {
                                        if (queryDis.DiscountQuantity > 0)
                                        {
                                            queryDis.DiscountQuantity--;
                                            if (queryDis.DiscountQuantity == 0)
                                            {
                                                queryDis.IsOk = false;
                                            }
                                        }

                                        if (queryDis.DiscountCount > 0)
                                        {
                                            queryDis.DiscountPercent = queryDis.DiscountPercent - 10;
                                            queryDis.DiscountCount--;
                                            if (queryDis.DiscountCount == 0)
                                            {
                                                queryDis.IsOk = false;
                                            }
                                        }
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                        var random = new Random();
                        code = "SP-" + random.Next(10000, 99999);
                        var id = Guid.NewGuid();
                        //if (transfer <= 0)
                        //{
                        //    sum = sum - transfer;
                        //}
                        //else
                        //{
                        //    var resultPay = Repfooter.RepInformationFooter();
                        //    if (resultPay.Count > 0)
                        //    {
                        //        foreach (var item in resultPay)
                        //        {
                        //            transfer = decimal.Parse(item.TarnsferPay ?? "0");
                        //        }
                        //    }

                        //    sum = sum + transfer;
                        //}





                        var inlog = db.SP_InsertOrder(id, code, values.Name, values.Family, phone,
                             values.Address, "", "", 0, tick.ToString(), "", values.Note, values.Quantity.ToString(), false, sumdis, 0, 0, transfer, sum, false,
                             queryusers.Id, DateTime.Now, queryusers.Id, DateTime.Now, 1);
                        if (inlog == -1)
                        {
                            LogWriter.Orders("-----------------------------------------------------Order--------------------------------------------------------------", "----------", "---------------------");
                            LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Order Success Insert In Database : " + "Name : " + values.Name + " - " + "Family : " + values.Family + " - " + " Phone : " + values.Phone + " - " + " Address : " + values.Address + " - " + " Code : " + code + " - " + " Note : " + values.Note + " - " + " Sum : " + string.Format("{0:##,###}", sum) + " - " + " UserRef : " + queryusers.Id + " - " + " Browser : " + browser + " - " + " IP : " + ip, string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now), "94");
                        }
                        else
                        {
                            LogWriter.Orders("-----------------------------------------------------Order--------------------------------------------------------------", "----------", "---------------------");
                            LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Order Failed Insert In Database : " + "Name : " + values.Name + " - " + "Family : " + values.Family + " - " + " Phone : " + values.Phone + " - " + " Address : " + values.Address + " - " + " Code : " + code + " - " + " Note : " + values.Note + " - " + " Sum : " + string.Format("{0:##,###}", sum) + " - " + " UserRef : " + queryusers.Id + " - " + " Browser : " + browser + " - " + " IP : " + ip, string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now), "94");
                        }




                        var sms = new SmsProviders();
                        sms.SendGenerateQuotations(long.Parse(phone), code);
                        if (carts.Count > 0)
                        {
                            foreach (var cart in carts)
                            {
                                var inlogg = db.SP_InsertOrderItem(Guid.NewGuid(),
                                       SepidyarHesabCodeGenerator.GenerateCode("Orders", "Table_Order_Item"), id, cart.Code, cart.Quantity, cart.Fee,
                                       cart.SizeRef, cart.ColorRef, cart.ProductId, sumdis, 0, 0, 0, 0);
                                if (inlogg == -1)
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Success Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                                else
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Failed Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                            }

                        }

                        var filename = "Default";
                        var fileExtention = "png";
                        var filelenght = 200;
                        if (file != null)
                        {
                            var userRef = Guid.NewGuid();
                            filelenght = file.ContentLength;
                            filename = "Request_" + code;
                            fileExtention = Path.GetExtension(file.FileName);
                            string pathCombine =
                                HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainRequest + filename + fileExtention);
                            file.SaveAs(pathCombine);
                            var qPics = db.Table_File_Upload.Where(c => c.Ref == id).AsNoTracking().ToList();
                            if (qPics.Count != 0)
                            {
                                foreach (var item in qPics)
                                {
                                    var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainRequest +
                                                                                  item.FileName + item.FileExtensions);
                                    File.Delete(filee);
                                    db.Table_File_Upload.Remove(item);
                                    db.SaveChanges();
                                }

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



                        LogWriter.Orders("--------------------------------------------------------------------------------------------------------------------------------------", "----------", "---------------------");
                        return code + "&" + tick + "&" + idusers + "&" + id;

                        #endregion
                    }
                    else
                    {
                        idusers = Guid.NewGuid();
                        var queyadd = new Table_User
                        {
                            Id = idusers,
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_User"),
                            Phone = phone,
                            ArchiveCode = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_User"),
                            CreatorDate = DateTime.Now,
                            CreatorRef = Guid.NewGuid(),
                            Family = values.Family,
                            ModifierDate = DateTime.Now,
                            ModifierRef = Guid.NewGuid(),
                            Version = 1,
                            RoleRef = Guid.Parse("66718116-6427-4f72-9a19-5c5aacdf73c0"),
                            Name = values.Name,
                            Password = PasswordGenerator.PasswordGenerate(),
                            IsOk = true,
                            IsSpecial = false,
                            IsInBlacklist = false,
                        };
                        db.Table_User.Add(queyadd);
                        db.SaveChanges();

                        #region Order

                        decimal sum = 0;

                        foreach (var cart in carts)
                        {
                            var sum2 = cart.Quantity * cart.Fee;
                            sum += sum2;
                        }

                        var random = new Random();
                        code = "SP-" + random.Next(10000, 99999);
                        var id = Guid.NewGuid();
                        if (transfer <= 0)
                        {
                            sum = sum - transfer;
                        }
                        else
                        {
                            var resultPay = Repfooter.RepInformationFooter();
                            if (resultPay.Count > 0)
                            {
                                foreach (var item in resultPay)
                                {
                                    transfer = decimal.Parse(item.TarnsferPay ?? "0");
                                }
                            }

                            sum = sum + transfer;
                        }
                        var inlog = db.SP_InsertOrder(id, code, values.Name, values.Family, phone,
                            values.Address, "", "", 0, tick.ToString(), "", values.Note, values.Quantity.ToString(), false, sumdis, 0, 0, transfer, sum, false,
                            idusers, DateTime.Now, idusers, DateTime.Now, 1);
                        if (inlog == -1)
                        {
                            LogWriter.Orders("-----------------------------------------------------Order--------------------------------------------------------------", "----------", "---------------------");

                            LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Order Success Insert In Database : " + "Name : " + values.Name + " - " + "Family : " + values.Family + " - " + " Phone : " + values.Phone + " - " + " Address : " + values.Address + " - " + " Code : " + code + " - " + " Note : " + values.Note + " - " + " Sum : " + string.Format("{0:##,###}", sum) + " - " + " UserRef : " + idusers + " - " + " Browser : " + browser + " - " + " IP : " + ip, string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now), "94");
                        }
                        else
                        {
                            LogWriter.Orders("-----------------------------------------------------Order--------------------------------------------------------------", "----------", "---------------------");

                            LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Order Failed Insert In Database : " + "Name : " + values.Name + " - " + "Family : " + values.Family + " - " + " Phone : " + values.Phone + " - " + " Address : " + values.Address + " - " + " Code : " + code + " - " + " Note : " + values.Note + " - " + " Sum : " + string.Format("{0:##,###}", sum) + " - " + " UserRef : " + idusers + " - " + " Browser : " + browser + " - " + " IP : " + ip, string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now), "94");
                        }
                        //var queryAdd = db.Table_Order.Add(new Table_Order
                        //{
                        //    Id = id,
                        //    Code = code,
                        //    IsOk = false,
                        //    ModifierDate = DateTime.Now,
                        //    CreatorRef = idusers,
                        //    ModifierRef = idusers,
                        //    CreatorDate = DateTime.Now,
                        //    Address = values.Address,
                        //    DeliveryCode = "",
                        //    Firstname = values.Name,
                        //    Lastname = values.Family,
                        //    Note = values.Note + "_______" + "IDPageInstagram :" + values.PageId,
                        //    Phone = phone,
                        //    PostalCode = "",
                        //    Status = 0,
                        //    TransactionCode = code,
                        //    Version = 1,
                        //    Price = sum,
                        //    IsPay = false,
                        //    Addition = 0,
                        //    Quantity = 0,
                        //    Discount = 0,
                        //    Duty = 0,
                        //    Tax = 0,
                        //});
                        //db.Table_Order.Add((queryAdd));
                        //db.SaveChanges();
                        var sms = new SmsProviders();
                        sms.SendGenerateQuotations(long.Parse(phone), code);
                        if (carts.Count > 0)
                        {
                            foreach (var cart in carts)
                            {
                                var type = RepSettings.RepositoryTypeSelect();
                                if (type == Guid.Parse("aa7490c8-31b2-47a4-951b-2a52a4488738"))
                                {
                                    var inlogg = db.SP_InsertOrderItem(Guid.NewGuid(),
                                         SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary"), id, cart.Code, cart.Quantity, cart.Fee,
                                         cart.SizeRef, cart.ColorRef, cart.ProductId, sumdis, transfer, 0, 0, 0);
                                    if (inlogg == -1)
                                    {
                                        LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Success Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                    }
                                    else
                                    {
                                        LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Failed Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                    }
                                }
                                else if (type == Guid.Parse("59a29f20-ad77-4b85-9bb3-31a0db9e7c2f"))
                                {
                                    var inlogg = db.SP_InsertOrderItem(Guid.NewGuid(),
                                        SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary"), id, cart.Code, cart.Quantity, cart.Fee,
                                        Guid.Empty, Guid.Empty, cart.ProductId, sumdis, transfer, 0, 0, 0);
                                    if (inlogg == -1)
                                    {
                                        LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Success Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                    }
                                    else
                                    {
                                        LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Failed Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                    }
                                }
                                else
                                {
                                    var inlogg = db.SP_InsertOrderItem(Guid.NewGuid(),
                                        SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary"), id, cart.Code, cart.Quantity, cart.Fee,
                                        Guid.Empty, Guid.Empty, cart.ProductId, sumdis, transfer, 0, 0, 0);
                                    if (inlogg == -1)
                                    {
                                        LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Success Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                    }
                                    else
                                    {
                                        LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");
                                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Failed Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                    }
                                }

                                //var bb = db.Table_Order_Item.Add(new Table_Order_Item
                                //{
                                //    Id = Guid.NewGuid(),
                                //    OrderRef = id,
                                //    Code = "SP-" + DateTime.Now.Ticks,
                                //    Fee = cart.Fee,
                                //    Quantity = cart.Quantity,
                                //    ItemCode = cart.Code,
                                //});
                                //db.Table_Order_Item.Add(bb);
                                //db.SaveChanges();
                            }
                        }

                        var filename = "Default";
                        var fileExtention = "png";
                        var filelenght = 200;
                        if (file != null)
                        {
                            var userRef = Guid.NewGuid();
                            filelenght = file.ContentLength;
                            filename = "Request_" + code;
                            fileExtention = Path.GetExtension(file.FileName);
                            string pathCombine =
                                HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainRequest + filename + fileExtention);
                            file.SaveAs(pathCombine);
                            var qPics = db.Table_File_Upload.Where(c => c.Ref == id).AsNoTracking().ToList();
                            if (qPics.Count != 0)
                            {
                                foreach (var item in qPics)
                                {
                                    var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainRequest +
                                                                                  item.FileName + item.FileExtensions);
                                    File.Delete(filee);
                                    db.Table_File_Upload.Remove(item);
                                    db.SaveChanges();
                                }

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
                        LogWriter.Orders("--------------------------------------------------------------------------------------------------------------------------------------", "----------", "---------------------");
                        return code + "&" + tick + "&" + idusers + "&" + id;

                        #endregion
                    }
                }
                else
                {
                    idusers = Guid.NewGuid();
                    var queyadd = new Table_User
                    {
                        Id = idusers,
                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_User"),
                        Phone = phone,
                        ArchiveCode = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_User"),
                        CreatorDate = DateTime.Now,
                        CreatorRef = Guid.NewGuid(),
                        Family = values.Family,
                        ModifierDate = DateTime.Now,
                        ModifierRef = Guid.NewGuid(),
                        Version = 1,
                        RoleRef = Guid.Parse("66718116-6427-4f72-9a19-5c5aacdf73c0"),
                        Name = values.Name,
                        Password = PasswordGenerator.PasswordGenerate(),
                        IsOk = true,
                        IsSpecial = false,
                        IsInBlacklist = false,
                    };
                    db.Table_User.Add(queyadd);
                    db.SaveChanges();

                    #region Order

                    decimal sum = 0;

                    foreach (var cart in carts)
                    {
                        var sum2 = cart.Quantity * cart.Fee;
                        sum += sum2;
                    }

                    var random = new Random();
                    code = "SP-" + random.Next(10000, 99999);
                    var id = Guid.NewGuid();
                    if (transfer <= 0)
                    {
                        sum = sum - transfer;
                    }
                    else
                    {
                        var resultPay = Repfooter.RepInformationFooter();
                        if (resultPay.Count > 0)
                        {
                            foreach (var item in resultPay)
                            {
                                transfer = decimal.Parse(item.TarnsferPay ?? "0");
                            }
                        }

                        sum = sum + transfer;
                    }
                    var inlog = db.SP_InsertOrder(id, code, values.Name, values.Family, phone,
                        values.Address, "", "", 0, tick.ToString(), "", values.Note, values.Quantity.ToString(), false, sumdis, 0, 0, transfer, sum, false,
                        idusers, DateTime.Now, idusers, DateTime.Now, 1);
                    if (inlog == -1)
                    {
                        LogWriter.Orders("-----------------------------------------------------Order--------------------------------------------------------------", "----------", "---------------------");

                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Order Success Insert In Database : " + "Name : " + values.Name + " - " + "Family : " + values.Family + " - " + " Phone : " + values.Phone + " - " + " Address : " + values.Address + " - " + " Code : " + code + " - " + " Note : " + values.Note + " - " + " Sum : " + string.Format("{0:##,###}", sum) + " - " + " UserRef : " + idusers + " - " + " Browser : " + browser + " - " + " IP : " + ip, string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now), "94");
                    }
                    else
                    {
                        LogWriter.Orders("-----------------------------------------------------Order--------------------------------------------------------------", "----------", "---------------------");
                        LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Order Failed Insert In Database : " + "Name : " + values.Name + " - " + "Family : " + values.Family + " - " + " Phone : " + values.Phone + " - " + " Address : " + values.Address + " - " + " Code : " + code + " - " + " Note : " + values.Note + " - " + " Sum : " + string.Format("{0:##,###}", sum) + " - " + " UserRef : " + idusers + " - " + " Browser : " + browser + " - " + " IP : " + ip, string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now), "94");
                    }
                    //var queryAdd = db.Table_Order.Add(new Table_Order
                    //{
                    //    Id = id,
                    //    Code = code,
                    //    IsOk = false,
                    //    ModifierDate = DateTime.Now,
                    //    CreatorRef = idusers,
                    //    ModifierRef = idusers,
                    //    CreatorDate = DateTime.Now,
                    //    Address = values.Address,
                    //    DeliveryCode = "",
                    //    Firstname = values.Name,
                    //    Lastname = values.Family,
                    //    Note = values.Note + "_______" + "IDPageInstagram :" + values.PageId,
                    //    Phone = phone,
                    //    PostalCode = "",
                    //    Status = 0,
                    //    TransactionCode = code,
                    //    Version = 1,
                    //    Price = sum,
                    //    IsPay = false,
                    //    Addition = 0,
                    //    Quantity = 0,
                    //    Discount = 0,
                    //    Duty = 0,
                    //    Tax = 0,
                    //});
                    //db.Table_Order.Add((queryAdd));
                    //db.SaveChanges();
                    var sms = new SmsProviders();
                    sms.SendGenerateQuotations(long.Parse(phone), code);
                    if (carts.Count > 0)
                    {
                        foreach (var cart in carts)
                        {
                            var type = RepSettings.RepositoryTypeSelect();
                            if (type == Guid.Parse("aa7490c8-31b2-47a4-951b-2a52a4488738"))
                            {
                                var inlogg = db.SP_InsertOrderItem(Guid.NewGuid(),
                                    SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary"), id, cart.Code, cart.Quantity, cart.Fee,
                                    cart.SizeRef, cart.ColorRef, cart.ProductId, sumdis, transfer, 0, 0, 0);
                                if (inlogg == -1)
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");

                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Success Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                                else
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");

                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Failed Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                            }
                            else if (type == Guid.Parse("59a29f20-ad77-4b85-9bb3-31a0db9e7c2f"))
                            {
                                var inlogg = db.SP_InsertOrderItem(Guid.NewGuid(),
                                    SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary"), id, cart.Code, cart.Quantity, cart.Fee,
                                    Guid.Empty, Guid.Empty, cart.ProductId, sumdis, transfer, 0, 0, 0);
                                if (inlogg == -1)
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");

                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Success Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                                else
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");

                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Failed Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                            }
                            else
                            {
                                var inlogg = db.SP_InsertOrderItem(Guid.NewGuid(),
                                    SepidyarHesabCodeGenerator.GenerateCode("Inventory", "Table_Product_Summary"), id, cart.Code, cart.Quantity, cart.Fee,
                                    Guid.Empty, Guid.Empty, cart.ProductId, sumdis, transfer, 0, 0, 0);
                                if (inlogg == -1)
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");

                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Success Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                                else
                                {
                                    LogWriter.Orders("-----------------------------------------------------OrderItem--------------------------------------------------------------", "----------", "---------------------");

                                    LogWriter.Orders(string.Format("{0:dddd dd MMMM yyyy}", DateTime.Now) + " - New Item Order Failed Insert In Database : " + "OrderId : " + id + " " + "ItemCode : " + cart.Code + " " + "ItemQuantity : " + cart.Quantity + " " + "ItemFee : " + cart.Fee + " " + "ItemSize : " + cart.SizeTitle + "(" + cart.SizeRef + ")" + " " + "ItemColor : " + cart.ColorTitle + "(" + cart.ColorRef + ")" + " " + "Product Id : " + cart.ProductTitle + "(" + cart.ProductId + ")", "94", "115");
                                }
                            }

                            //var bb = db.Table_Order_Item.Add(new Table_Order_Item
                            //{
                            //    Id = Guid.NewGuid(),
                            //    OrderRef = id,
                            //    Code = "SP-" + DateTime.Now.Ticks,
                            //    Fee = cart.Fee,
                            //    Quantity = cart.Quantity,
                            //    ItemCode = cart.Code,
                            //});
                            //db.Table_Order_Item.Add(bb);
                            //db.SaveChanges();
                        }
                    }

                    var filename = "Default";
                    var fileExtention = "png";
                    var filelenght = 200;
                    if (file != null)
                    {
                        var userRef = Guid.NewGuid();
                        filelenght = file.ContentLength;
                        filename = "Request_" + code;
                        fileExtention = Path.GetExtension(file.FileName);
                        string pathCombine =
                            HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainRequest + filename + fileExtention);
                        file.SaveAs(pathCombine);
                        var qPics = db.Table_File_Upload.Where(c => c.Ref == id).AsNoTracking().ToList();
                        if (qPics.Count != 0)
                        {
                            foreach (var item in qPics)
                            {
                                var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainRequest +
                                                                              item.FileName + item.FileExtensions);
                                File.Delete(filee);
                                db.Table_File_Upload.Remove(item);
                                db.SaveChanges();
                            }

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
                    LogWriter.Orders("--------------------------------------------------------------------------------------------------------------------------------------", "----------", "---------------------");
                    return code + "&" + tick + "&" + idusers + "&" + id;

                    #endregion
                }


            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    LogWriter.Logger("Application Order In Error : " + e.InnerException.Message, DateTime.Now.ToLongDateString(), "440");
                }
                else
                {
                    LogWriter.Logger("Application Order In Error : " + e.Message, DateTime.Now.ToLongDateString(), "440");
                }
                return "application Error :" + e.Message;
            }

        }

        public static string MyOrders(string code)
        {
            var message = "Error";
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.AsNoTracking().FirstOrDefault(c => c.Code == code);
                if (query != null)
                {
                    switch (query.Status)
                    {
                        case 0:
                            {
                                message = "مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است";
                                break;
                            }
                        case 1:
                            {
                                message = "مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد";
                                break;
                            }
                        case 2:
                            {
                                message = "مشتری گرامی ، سفارش شما آماده ارسال شد";
                                break;
                            }
                        case 3:
                            {
                                message = "مشتری گرامی ، سفارش شما ارسال شد";
                                break;
                            }
                        case 4:
                            {
                                message = "مشتری گرامی ، سفارش شما تحویل داده شده است";
                                break;
                            }
                        case 5:
                            {
                                message = "مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است ";
                                break;
                            }
                        case 6:
                            {
                                message = "مشتری گرامی ، سفارش شما لغو شد ";
                                break;
                            }
                        case 7:
                            {
                                message = "مشتری گرامی ، سفارش شما حذف شد ";
                                break;
                            }
                        case 8:
                            {
                                message = "مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید ";
                                break;
                            }
                        case 9:
                            {
                                message = "مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم ";
                                break;
                            }
                        case 10:
                            {
                                message = "مشتری گرامی ، در دست برسی ";
                                break;
                            }

                    }
                }
                else
                {
                    message = "Error";
                }

                return message;
            }
            catch (Exception e)
            {
                return "Error : " + e.Message;
            }
        }
        //End--------------------------------------------
        //Repository Change Statuse IsPay
        public static string RepChangeStatusIsPay(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsPay)
                    {
                        case true:
                            {
                                query.IsPay = false;
                                db.SaveChanges();
                                break;
                            }
                        case false:
                            {
                                query.IsPay = true;
                                db.SaveChanges();
                                break;
                            }
                    }

                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        //End------------------------------------------------
        public static List<VMOrders.VmOrderMangment> RepositoryListOrders(string phone)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.OrderByDescending(c => c.CreatorDate).Where(c => c.Phone == phone).AsNoTracking()
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var order in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            Id = order.Id,
                            Code = order.Code,
                            Address = order.Address,
                            Family = order.Lastname,
                            Name = order.Firstname,
                            Note = order.Note,
                            PageId = order.Note,
                            Phone = order.Phone,
                            ModifierDateTime = order.ModifierDate,
                            CreatorDateTime = order.CreatorDate,
                            Price = order.Price,
                        };
                        switch (order.Status)
                        {
                            case 0:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + "0";
                                    break;
                                }
                            case 1:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + "1";
                                    break;
                                }
                            case 2:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما آماده ارسال شد" + "2";
                                    break;
                                }
                            case 3:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما ارسال شد" + "3";
                                    break;
                                }
                            case 4:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تحویل داده شده است" + "4";
                                    break;
                                }
                            case 5:
                                {
                                    vm.Status = "مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است " + "5";
                                    break;
                                }
                            case 6:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما لغو شد " + "6";
                                    break;
                                }
                            case 7:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما حذف شد " + "7";
                                    break;
                                }
                            case 8:
                                {
                                    vm.Status = "مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید " + "8";
                                    break;
                                }
                            case 9:
                                {
                                    vm.Status = "مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم " + "9";
                                    break;
                                }
                            case 10:
                                {
                                    vm.Status = "مشتری گرامی ، در دست برسی " + "10";
                                    break;
                                }

                        }

                        if (order.IsPay)
                        {
                            vm.IsPayClass = "label label-success";
                            vm.IsPayTitle = "پرداخت شده";
                        }
                        else
                        {
                            vm.IsPayClass = "label label-danger";
                            vm.IsPayTitle = "پرداخت نشده";
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


        public static List<VMOrders.VmOrderMangment> RepositoryListOrdersAdmin()
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.OrderByDescending(c => c.CreatorDate).Where(c => c.IsPay).AsNoTracking()
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var order in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            Id = order.Id,
                            Code = order.Code,
                            Family = order.Lastname,
                            Name = order.Firstname,
                            Note = order.Note,
                            PageId = order.Note,
                            Phone = order.Phone,
                            ModifierDateTime = order.ModifierDate,
                            CreatorDateTime = order.CreatorDate,
                            Discount = order.Discount,
                            Address = order.Address,
                            Price = order.Price
                        };


                        try
                        {
                            var a = Guid.Parse(order.Address);
                            var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                            if (queryaddress != null)
                            {
                                vm.Address = queryaddress.Address;
                                vm.PostalCode = queryaddress.PostalCode;
                            }

                            var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                            if (querycity != null)
                            {
                                vm.CityTitle = querycity.Title;
                            }

                        }
                        catch (Exception e)
                        {
                            vm.Address = order.Address;
                        }

                        switch (order.Status)
                        {
                            case 0:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + " " + " 0 ";
                                    break;
                                }
                            case 1:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + " " + " 1 ";
                                    break;
                                }
                            case 2:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما آماده ارسال شد" + "  " + " 2 ";
                                    break;
                                }
                            case 3:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما ارسال شد" + " " + " 3 ";
                                    break;
                                }
                            case 4:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما تحویل داده شده است" + " " + " 4 ";
                                    break;
                                }
                            case 5:
                                {
                                    vm.Status = ".مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است" + " " + " 5 ";
                                    break;
                                }
                            case 6:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما لغو شد" + " " + " 6 ";
                                    break;
                                }
                            case 7:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما حذف شد" + " " + " 7 ";
                                    break;
                                }
                            case 8:
                                {
                                    vm.Status = ".مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید" + " " + " 8 ";
                                    break;
                                }
                            case 9:
                                {
                                    vm.Status = ".مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم" + " " + " 9 ";
                                    break;
                                }
                            case 10:
                                {
                                    vm.Status = ".مشتری گرامی ، در دست برسی" + "  " + " 10 ";
                                    break;
                                }

                        }

                        if (order.IsPay == true)
                        {
                            vm.IsPayClass = "btn btn-success";
                            vm.IsPayTitle = "پرداخت شده";
                        }
                        else
                        {
                            vm.IsPayClass = "btn btn-danger";
                            vm.IsPayTitle = "پرداخت نشده";
                        }


                        list.Add(vm);
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    LogWriter.Logger("Application Error : " + e.InnerException.Message, "", "");
                }
                else
                {
                    LogWriter.Logger("Application Error : " + e.Message, "", "");
                }
                return list;

            }
        }

        public static List<VMOrders.VmOrderMangment> RepositoryOrdersSearch(string searchnew)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var date = new PersianDate();
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_Order.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.Firstname.Contains(search) || c.Lastname.Contains(search) || (c.Firstname + " " + c.Lastname).Contains(search) || c.Phone.Contains(search)).AsNoTracking()
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var order in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            Id = order.Id,
                            Code = order.Code,
                            Address = order.Address,
                            Family = order.Lastname,
                            Name = order.Firstname,
                            Note = order.Note,
                            PageId = order.Note,
                            Phone = order.Phone,
                            ModifierDateTime = order.ModifierDate,
                            CreatorDateTime = order.CreatorDate,
                            Price = order.Price,
                            Discount = order.Discount,
                        };
                        try
                        {
                            var a = Guid.Parse(order.Address);
                            var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                            if (queryaddress != null)
                            {
                                vm.Address = queryaddress.Address;
                                vm.PostalCode = queryaddress.PostalCode;
                            }

                            var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                            if (querycity != null)
                            {
                                vm.CityTitle = querycity.Title;
                            }

                        }
                        catch (Exception e)
                        {
                            vm.Address = order.Address;
                        }
                        switch (order.Status)
                        {
                            case 0:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + "0";
                                    break;
                                }
                            case 1:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + "1";
                                    break;
                                }
                            case 2:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما آماده ارسال شد" + "2";
                                    break;
                                }
                            case 3:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما ارسال شد" + "3";
                                    break;
                                }
                            case 4:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تحویل داده شده است" + "4";
                                    break;
                                }
                            case 5:
                                {
                                    vm.Status = "مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است " + "5";
                                    break;
                                }
                            case 6:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما لغو شد " + "6";
                                    break;
                                }
                            case 7:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما حذف شد " + "7";
                                    break;
                                }
                            case 8:
                                {
                                    vm.Status = "مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید " + "8";
                                    break;
                                }
                            case 9:
                                {
                                    vm.Status = "مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم " + "9";
                                    break;
                                }
                            case 10:
                                {
                                    vm.Status = "مشتری گرامی ، در دست برسی " + "10";
                                    break;
                                }

                        }

                        if (order.IsOk == true)
                        {
                            vm.IsPayClass = "btn btn-success";
                            vm.IsPayTitle = "پرداخت شده";
                        }
                        else
                        {
                            vm.IsPayClass = "btn btn-danger";
                            vm.IsPayTitle = "پرداخت نشده";
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

        public static VMOrders.VmOrderCarts RepositoryCarts(List<VMOrders.VmOrderSubmit> carts, Guid UserRef)
        {
            var list = new VMOrders.VmOrderCarts();
            try
            {
                list = RepDiscount.RepositoryCartsNoDiscount(carts, UserRef);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    LogWriter.Logger(e.InnerException.Message, DateTime.Now.ToString(), "");
                }
                LogWriter.Logger(e.Message.ToString(), DateTime.Now.ToString(), "");
            }

            return list;
        }

        public static VMOrders.VmOrderCarts RepositoryCartsCode(List<VMOrders.VmOrderSubmit> carts, string code)
        {
            var list = new VMOrders.VmOrderCarts();
            try
            {
                if (code == "")
                {
                    if (carts.Count > 0)
                    {
                        list = RepDiscount.RepositoryCartsNoDiscount(carts, Guid.Empty);
                    }
                }
                else
                {
                    if (carts.Count > 0)
                    {
                        list = RepDiscount.RepositoryCartsDiscount(carts, code);
                    }
                }

            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static string AddToArchive(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                query.IsOk = false;
                                db.SaveChanges();
                                return "success";
                            }
                        case false:
                            {
                                return "true";
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


        public static List<VMOrders.VmOrderMangment> RepositoryListOrdersArchive()
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.Where(c => !c.IsOk).AsNoTracking()
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var order in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            Id = order.Id,
                            Code = order.Code,
                            Address = order.Address,
                            Family = order.Lastname,
                            Name = order.Firstname,
                            Note = order.Note,
                            PageId = order.Note,
                            Phone = order.Phone,
                            ModifierDateTime = order.ModifierDate,
                            CreatorDateTime = order.CreatorDate,
                            IsOk = order.IsOk,
                            Price = order.Price,
                            Discount = order.Discount
                        };

                        try
                        {
                            var a = Guid.Parse(order.Address);
                            var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                            if (queryaddress != null)
                            {
                                vm.Address = queryaddress.Address;
                                vm.PostalCode = queryaddress.PostalCode;
                            }

                            var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                            if (querycity != null)
                            {
                                vm.CityTitle = querycity.Title;
                            }

                        }
                        catch (Exception e)
                        {
                            vm.Address = order.Address;
                        }


                        switch (order.Status)
                        {
                            case 0:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + "0";
                                    break;
                                }
                            case 1:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + "1";
                                    break;
                                }
                            case 2:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما آماده ارسال شد" + "2";
                                    break;
                                }
                            case 3:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما ارسال شد" + "3";
                                    break;
                                }
                            case 4:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تحویل داده شده است" + "4";
                                    break;
                                }
                            case 5:
                                {
                                    vm.Status = "مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است " + "5";
                                    break;
                                }
                            case 6:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما لغو شد " + "6";
                                    break;
                                }
                            case 7:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما حذف شد " + "7";
                                    break;
                                }
                            case 8:
                                {
                                    vm.Status = "مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید " + "8";
                                    break;
                                }
                            case 9:
                                {
                                    vm.Status = "مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم " + "9";
                                    break;
                                }
                            case 10:
                                {
                                    vm.Status = "مشتری گرامی ، در دست برسی " + "10";
                                    break;
                                }

                        }

                        if (order.IsPay == true)
                        {
                            vm.IsPayClass = "btn btn-success";
                            vm.IsPayTitle = "پرداخت شده";
                        }
                        else
                        {
                            vm.IsPayClass = "btn btn-danger";
                            vm.IsPayTitle = "پرداخت نشده";
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

        public static List<VMOrders.VmOrderMangment> RepositoryListOrdersArchiveSearch(string searchnew)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var search = searchnew.Replace(" ", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_Order.OrderByDescending(c => !c.IsOk).Where(c => c.Code == search || c.Firstname.Contains(search) || c.Lastname.Contains(search) || (c.Firstname + " " + c.Lastname).Contains(search) || c.Phone == search).AsNoTracking()
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var order in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            Id = order.Id,
                            Code = order.Code,
                            Address = order.Address,
                            Family = order.Lastname,
                            Name = order.Firstname,
                            Note = order.Note,
                            PageId = order.Note,
                            Phone = order.Phone,
                            ModifierDateTime = order.ModifierDate,
                            CreatorDateTime = order.CreatorDate,
                            IsOk = order.IsOk,
                            Price = order.Price,
                            Discount = order.Discount
                        };

                        try
                        {
                            var a = Guid.Parse(order.Address);
                            var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                            if (queryaddress != null)
                            {
                                vm.Address = queryaddress.Address;
                                vm.PostalCode = queryaddress.PostalCode;
                            }

                            var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                            if (querycity != null)
                            {
                                vm.CityTitle = querycity.Title;
                            }

                        }
                        catch (Exception e)
                        {
                            vm.Address = order.Address;
                        }
                        switch (order.Status)
                        {
                            case 0:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + "0";
                                    break;
                                }
                            case 1:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + "1";
                                    break;
                                }
                            case 2:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما آماده ارسال شد" + "2";
                                    break;
                                }
                            case 3:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما ارسال شد" + "3";
                                    break;
                                }
                            case 4:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تحویل داده شده است" + "4";
                                    break;
                                }
                            case 5:
                                {
                                    vm.Status = "مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است " + "5";
                                    break;
                                }
                            case 6:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما لغو شد " + "6";
                                    break;
                                }
                            case 7:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما حذف شد " + "7";
                                    break;
                                }
                            case 8:
                                {
                                    vm.Status = "مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید " + "8";
                                    break;
                                }
                            case 9:
                                {
                                    vm.Status = "مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم " + "9";
                                    break;
                                }
                            case 10:
                                {
                                    vm.Status = "مشتری گرامی ، در دست برسی " + "10";
                                    break;
                                }

                        }

                        if (order.IsPay == true)
                        {
                            vm.IsPayClass = "btn btn-success";
                            vm.IsPayTitle = "پرداخت شده";
                        }
                        else
                        {
                            vm.IsPayClass = "btn btn-danger";
                            vm.IsPayTitle = "پرداخت نشده";
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

        public static string AddToOrder(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.FirstOrDefault(c => c.Id == id);
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

                                query.IsOk = true;
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

        public static List<VMOrders.VmOrderMangment> RepositoryAllOrdersAdmin()
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.OrderByDescending(c => c.CreatorDate).AsNoTracking()
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var order in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            Id = order.Id,
                            Code = order.Code,
                            Family = order.Lastname,
                            Fullname = order.Firstname + " " + order.Lastname,
                            Name = order.Firstname,
                            Note = order.Note,
                            PageId = order.Note,
                            Phone = order.Phone,
                            ModifierDateTime = order.ModifierDate,
                            CreatorDateTime = order.CreatorDate,
                            Discount = order.Discount,
                            ProductCode = order.ProductCode,
                            Address = order.Address,
                            Price = order.Price
                        };

                        try
                        {
                            var a = Guid.Parse(order.Address);
                            var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                            if (queryaddress != null)
                            {
                                vm.Address = queryaddress.Address;
                                vm.PostalCode = queryaddress.PostalCode;
                            }

                            var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                            if (querycity != null)
                            {
                                vm.CityTitle = querycity.Title;
                            }

                        }
                        catch (Exception e)
                        {
                            vm.Address = order.Address;
                        }


                        switch (order.Status)
                        {
                            case 0:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + " " + " 0 ";
                                    break;
                                }
                            case 1:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + " " + " 1 ";
                                    break;
                                }
                            case 2:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما آماده ارسال شد" + "  " + " 2 ";
                                    break;
                                }
                            case 3:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما ارسال شد" + " " + " 3 ";
                                    break;
                                }
                            case 4:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما تحویل داده شده است" + " " + " 4 ";
                                    break;
                                }
                            case 5:
                                {
                                    vm.Status = ".مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است" + " " + " 5 ";
                                    break;
                                }
                            case 6:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما لغو شد" + " " + " 6 ";
                                    break;
                                }
                            case 7:
                                {
                                    vm.Status = ".مشتری گرامی ، سفارش شما حذف شد" + " " + " 7 ";
                                    break;
                                }
                            case 8:
                                {
                                    vm.Status = ".مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید" + " " + " 8 ";
                                    break;
                                }
                            case 9:
                                {
                                    vm.Status = ".مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم" + " " + " 9 ";
                                    break;
                                }
                            case 10:
                                {
                                    vm.Status = ".مشتری گرامی ، در دست برسی" + "  " + " 10 ";
                                    break;
                                }

                        }

                        if (order.IsPay == true)
                        {
                            vm.IsPayClass = "btn btn-success";
                            vm.IsPayTitle = "پرداخت شده";
                        }
                        else
                        {
                            vm.IsPayClass = "btn btn-danger";
                            vm.IsPayTitle = "پرداخت نشده";
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


        public static List<VMOrders.VmOrderMangment> RepositoryAllOrdersSearch(string searchnew)
        {
            var list = new List<VMOrders.VmOrderMangment>();
            try
            {
                var date = new PersianDate();
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_Order.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.Firstname.Contains(search) || c.Lastname.Contains(search) || (c.Firstname + " " + c.Lastname).Contains(search) || c.Phone.Contains(search)).AsNoTracking()
                    .ToList();
                if (query.Count > 0)
                {
                    foreach (var order in query)
                    {
                        var vm = new VMOrders.VmOrderMangment
                        {
                            Id = order.Id,
                            Code = order.Code,
                            Address = order.Address,
                            Family = order.Lastname,
                            Name = order.Firstname,
                            Note = order.Note,
                            PageId = order.Note,
                            Phone = order.Phone,
                            ModifierDateTime = order.ModifierDate,
                            CreatorDateTime = order.CreatorDate,
                            Price = order.Price,
                            Discount = order.Discount
                        };
                        try
                        {
                            var a = Guid.Parse(order.Address);
                            var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                            if (queryaddress != null)
                            {
                                vm.Address = queryaddress.Address;
                                vm.PostalCode = queryaddress.PostalCode;
                            }

                            var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                            if (querycity != null)
                            {
                                vm.CityTitle = querycity.Title;
                            }

                        }
                        catch (Exception e)
                        {
                            vm.Address = order.Address;
                        }
                        switch (order.Status)
                        {
                            case 0:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + "0";
                                    break;
                                }
                            case 1:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + "1";
                                    break;
                                }
                            case 2:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما آماده ارسال شد" + "2";
                                    break;
                                }
                            case 3:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما ارسال شد" + "3";
                                    break;
                                }
                            case 4:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما تحویل داده شده است" + "4";
                                    break;
                                }
                            case 5:
                                {
                                    vm.Status = "مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است " + "5";
                                    break;
                                }
                            case 6:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما لغو شد " + "6";
                                    break;
                                }
                            case 7:
                                {
                                    vm.Status = "مشتری گرامی ، سفارش شما حذف شد " + "7";
                                    break;
                                }
                            case 8:
                                {
                                    vm.Status = "مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید " + "8";
                                    break;
                                }
                            case 9:
                                {
                                    vm.Status = "مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم " + "9";
                                    break;
                                }
                            case 10:
                                {
                                    vm.Status = "مشتری گرامی ، در دست برسی " + "10";
                                    break;
                                }
                        }

                        if (order.IsOk == true)
                        {
                            vm.IsPayClass = "btn btn-success";
                            vm.IsPayTitle = "پرداخت شده";
                        }
                        else
                        {
                            vm.IsPayClass = "btn btn-danger";
                            vm.IsPayTitle = "پرداخت نشده";
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

        public static string Generate(VMOrders.VmOrderMangment values, Guid UserId)
        {
            try
            {
                var userRef = UserId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(20000, 29999);
                var isok = false;
                var db = new Orders_Entities();

                var query = db.Table_Order.Add(new Table_Order
                {
                    Id = id,
                    Lastname = values.Family,
                    Firstname = values.Name,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = true,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsPay = values.IsPay,
                    Address = values.Address,
                    Phone = values.Phone,
                    Status = values.Case,
                    ProductCode = values.ProductCode,
                    Note = values.Note,
                    Price = values.Price,
                    Quantity = values.Quantity,
                    Discount = values.Discount,
                    Tax = 0,
                    Duty = 0,
                    Addition = 0,
                    PostalCode = values.PostalCode,
                });
                db.Table_Order.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        //End------------------------------------
        //Repository Show Selected Order
        public static VMOrders.VmOrderMangment RepositorySelectedOrdersAdmin(Guid id)
        {
            var vmm = new VMOrders.VmOrderMangment();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Order.AsNoTracking().FirstOrDefault(c => c.Id == id);
                if (query != null)
                {

                    var vm = new VMOrders.VmOrderMangment
                    {
                        Id = query.Id,
                        Code = query.Code,
                        Family = query.Lastname,
                        Name = query.Firstname,
                        Note = query.Note,
                        PageId = query.Note,
                        Phone = query.Phone,
                        ModifierDateTime = query.ModifierDate,
                        CreatorDateTime = query.CreatorDate,
                        Discount = query.Discount,
                        Address = query.Address,
                        Price = query.Price
                    };


                    try
                    {
                        var a = Guid.Parse(query.Address);
                        var queryaddress = db.Table_Address.FirstOrDefault(c => c.Id == a);
                        if (queryaddress != null)
                        {
                            vm.Address = queryaddress.Address;
                            vm.PostalCode = queryaddress.PostalCode;
                        }

                        var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == queryaddress.CityRef);
                        if (querycity != null)
                        {
                            vm.CityTitle = querycity.Title;
                        }

                    }
                    catch (Exception e)
                    {
                        vm.Address = query.Address;
                    }

                    switch (query.Status)
                    {
                        case 0:
                            {
                                vm.Status = ".مشتری گرامی ، سفارش شما تازه ثبت شده است و هنوز اقدامی صورت نگرفته است" + " " + " 0 ";
                                break;
                            }
                        case 1:
                            {
                                vm.Status = ".مشتری گرامی ، سفارش شما دریافت شد و در دست پیگیری میباشد" + " " + " 1 ";
                                break;
                            }
                        case 2:
                            {
                                vm.Status = ".مشتری گرامی ، سفارش شما آماده ارسال شد" + "  " + " 2 ";
                                break;
                            }
                        case 3:
                            {
                                vm.Status = ".مشتری گرامی ، سفارش شما ارسال شد" + " " + " 3 ";
                                break;
                            }
                        case 4:
                            {
                                vm.Status = ".مشتری گرامی ، سفارش شما تحویل داده شده است" + " " + " 4 ";
                                break;
                            }
                        case 5:
                            {
                                vm.Status = ".مشتری گرامی ، محصول مورد نظر شما موجود نبوده است یا مغایرت داشته است" + " " + " 5 ";
                                break;
                            }
                        case 6:
                            {
                                vm.Status = ".مشتری گرامی ، سفارش شما لغو شد" + " " + " 6 ";
                                break;
                            }
                        case 7:
                            {
                                vm.Status = ".مشتری گرامی ، سفارش شما حذف شد" + " " + " 7 ";
                                break;
                            }
                        case 8:
                            {
                                vm.Status = ".مشتری گرامی ، اطلاعات شما ناقص بوده است با مدیریت تماس بگیرید" + " " + " 8 ";
                                break;
                            }
                        case 9:
                            {
                                vm.Status = ".مشتری گرامی ، ما در انتظار ثبت نظر و لایک شما هستیم" + " " + " 9 ";
                                break;
                            }
                        case 10:
                            {
                                vm.Status = ".مشتری گرامی ، در دست برسی" + "  " + " 10 ";
                                break;
                            }

                    }

                    if (query.IsPay == true)
                    {
                        vm.IsPayClass = "btn btn-success";
                        vm.IsPayTitle = "پرداخت شده";
                    }
                    else
                    {
                        vm.IsPayClass = "btn btn-danger";
                        vm.IsPayTitle = "پرداخت نشده";
                    }

                    vmm = vm;
                }

                return vmm;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    LogWriter.Logger("Application Error : " + e.InnerException.Message, "", "");
                }
                else
                {
                    LogWriter.Logger("Application Error : " + e.Message, "", "");
                }
                return vmm;

            }
        }


    }

}
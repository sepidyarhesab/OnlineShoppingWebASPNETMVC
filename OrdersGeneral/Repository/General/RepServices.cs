using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;
using SepidyarHesabExtensions.Classes;
using SepidyarHesabExtensions.Extentions;


namespace OrdersGeneral.Repository.General
{
    public class RepServices
    {
        public static string DeleteRow(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Service.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                        {
                            return "TRUE";
                        }
                        case false:
                        {
                            var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id).ToList();
                            if (qPics.Count > 0)
                            {
                                foreach (var fileUpload in qPics)
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainServices + fileUpload.FileName + fileUpload.FileExtensions)))
                                    {
                                        File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainServices + fileUpload.FileName + fileUpload.FileExtensions));
                                    }
                                    db.Table_File_Upload.Remove(fileUpload);
                                    db.SaveChanges();
                                }
                            }

                            db.Table_Service.Remove(query);
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

        public static string AddnewRow(VMServices.VMMainServicesManagement values, HttpPostedFileBase files, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = false;
                switch (values.IsOk)
                {
                    case 1:
                    {
                        isok = true;
                        break;
                    }
             
                }
                var query = db.Table_Service.Add(new Table_Service()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecendryTitle = values.SecendaryTitle,
                    Url = values.Url,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    Sort = values.Sort,
                    TertiaryTitle = values.SecendaryTitle,
                });
                db.Table_Service.Add(query);
                db.SaveChanges();

                if (files != null)
                {
                    var filename = "Default";
                    var fileExtention = "png";
                    var filelenght = 200;
                    filelenght = files.ContentLength;
                    filename = "Services_" + SepidyarHesabCodeGenerator.GenerateCode("General", "Table_Service");
                    fileExtention = Path.GetExtension(files.FileName);
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainServices + filename + fileExtention);
                    files.SaveAs(pathCombine);
                    var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id).ToList();
                    if (qPics.Count != 0)
                    {
                        foreach (var item in qPics)
                        {

                            db.Table_File_Upload.Remove(item);
                            db.SaveChanges();


                            var file = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainServices +
                                                                          item.FileName + item.FileExtensions);
                            File.Delete(file);
                        }

                        var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                        qAddPic.Id = Guid.NewGuid();
                        qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        qAddPic.Tables = "Table_Service";
                        qAddPic.Schemas = "General";
                        qAddPic.Ref = query.Id;
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
                        qAddPic.Tables = "Table_Service";
                        qAddPic.Schemas = "General";
                        qAddPic.Ref = query.Id;
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


        public static List<VMServices.VMMainServices> RepositoryMainServices()
        {
            var list = new List<VMServices.VMMainServices>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Service.Where(c => c.IsOk).OrderBy(c=>c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var service in query)
                    {
                        var vm = new VMServices.VMMainServices
                        {
                            Id = service.Id,
                            Sort = service.Sort,
                            Url = service.Url,
                            SecendryTitle = service.SecendryTitle,
                            PrimaryTitle = service.PrimaryTitle,
                        };

                        var queryFiles =
                            db.Table_File_Upload.FirstOrDefault(c =>
                                c.IsMain && c.IsOk && c.Ref == service.Id);
                        if (queryFiles != null)
                        {
                            vm.FileName = "/Static/Content/Images/Services/" + queryFiles.FileName + 
                                          queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = service.TertiaryTitle;
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

        public static List<VMServices.VMMainServicesManagement> RepositoryMainServicesManagemenet()
        {
            var list = new List<VMServices.VMMainServicesManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Service.OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var service in query)
                    {
                        var vm = new VMServices.VMMainServicesManagement
                        {
                            Id = service.Id,
                            Sort = service.Sort,
                            Url = service.Url,
                            SecendaryTitle = service.SecendryTitle,
                            PrimaryTitle = service.PrimaryTitle,
                            CreatorDateTime = service.CreatorDate,
                            Code = service.Code,
                        };

                        var queryFiles =
                            db.Table_File_Upload.FirstOrDefault(c =>
                                c.IsMain && c.IsOk && c.Ref == service.Id);
                        if (queryFiles != null)
                        {
                            vm.FileName = "/Static/Content/Images/Services/" + queryFiles.FileName + 
                                          queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = service.TertiaryTitle;
                        }


                        if (service.IsOk)
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


        public static List<VMServices.VMMainServicesManagement> RepositoryMainServicesManagemenetSearch(string searchnew)
        {
            var list = new List<VMServices.VMMainServicesManagement>();
            try
            {
                var search = searchnew.Replace("	", "");
                var db = new Orders_Entities();
                var query = db.Table_Service.OrderByDescending(c=>c.CreatorDate).Where(c=>c.Code == search||c.PrimaryTitle.Contains(search)||c.SecendryTitle.Contains(search)||c.Url.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var service in query)
                    {
                        var vm = new VMServices.VMMainServicesManagement
                        {
                            Id = service.Id,
                            Sort = service.Sort,
                            Url = service.Url,
                            SecendaryTitle = service.SecendryTitle,
                            PrimaryTitle = service.PrimaryTitle,
                            CreatorDateTime = service.CreatorDate,
                            Code = service.Code,
                        };

                        var queryFiles =
                            db.Table_File_Upload.FirstOrDefault(c =>
                                c.IsMain && c.IsOk && c.Ref == service.Id);
                        if (queryFiles != null)
                        {
                            vm.FileName = "/Static/Content/Images/Services/" + queryFiles.FileName +
                                          queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = service.TertiaryTitle;
                        }


                        if (service.IsOk)
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

        public static List<VMServices.VMMainServicesManagement> RepositoryMainServicesManagemenetById(Guid Id)
        {
            var list = new List<VMServices.VMMainServicesManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Service.OrderBy(c => c.Sort).FirstOrDefault(c => c.Id == Id);
                if (query !=null)
                {

                    var vm = new VMServices.VMMainServicesManagement
                    {
                        Id = query.Id,
                        Sort = query.Sort,
                        Url = query.Url,
                        SecendaryTitle = query.SecendryTitle,
                        PrimaryTitle = query.PrimaryTitle,
                        CreatorDateTime = query.CreatorDate,
                        Code = query.Code,
                    };

                    var queryFiles =
                        db.Table_File_Upload.FirstOrDefault(c =>
                            c.IsMain && c.IsOk && c.Ref == query.Id);
                    if (queryFiles != null)
                    {
                        vm.FileName = "/Static/Content/Images/Services/" + queryFiles.FileName + "." +
                                      queryFiles.FileExtensions;
                    }
                    else
                    {
                        vm.FileName = query.TertiaryTitle;
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
                    
                    list.Add(vm);
                }
            }
            catch (Exception e)
            {
            }

            return list;
        }

        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Service.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                        {
                            query.IsOk = false;
                            db.SaveChanges();
                            break;
                        }
                        case false:
                        {
                            query.IsOk = true;
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

        #region Edit

        public static string RepositoryMainServicesManagemenetEditById(VMServices.VMMainServicesManagement values, HttpPostedFileBase files, Guid userId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userId;
                var query = db.Table_Service.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecendryTitle = values.SecendaryTitle;
                    query.Url = values.Url;
                    query.ModifireDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = values.Sort;
                    db.SaveChanges();

                    if (files != null)
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
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainServices + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainServices + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght = files.ContentLength;
                        filename = "Category_" + SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        fileExtention = Path.GetExtension(files.FileName);
                        string pathCombine =
                            HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainServices + filename + fileExtention);
                        files.SaveAs(pathCombine);
                    
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
                            Schemas = "General",
                            Tables = "Table_Service",
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


        #endregion
    }
}
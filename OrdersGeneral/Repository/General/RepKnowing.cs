using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;

namespace OrdersGeneral.Repository.General
{
    public class RepKnowing
    {
        public static List<VMKnowing.VmKnowingManagement> RepositoryKnowingManagements()
        {
            var list = new List<VMKnowing.VmKnowingManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Knowing.AsNoTracking().ToList();
                if (query.Count >0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMKnowing.VmKnowingManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Url = item.Url,
                            Sort = item.Sort,
                            CreatorDate = item.CreatorDate,
                        };
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
                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == item.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Knowing/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
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


        public static List<VMKnowing.VmKnowingManagement> RepositoryKnowing()
        {
            var list = new List<VMKnowing.VmKnowingManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Knowing.Where(c=>c.IsOk && !c.IsDelete).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMKnowing.VmKnowingManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Url = item.Url,
                            Sort = item.Sort,
                            CreatorDate = item.CreatorDate,
                        };
                        var queryfile =
                            db.Table_File_Upload.FirstOrDefault(c => c.IsOk && c.Ref == item.Id);
                        if (queryfile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Knowing/" + queryfile.FileName +
                                          queryfile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/";
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



        public static List<VMKnowing.VmKnowingManagement> EditKnowingManagement(Guid Id)
        {
            var list = new List<VMKnowing.VmKnowingManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Knowing.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMKnowing.VmKnowingManagement
                    {
                        Id = query.Id,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        Url = query.Url,
                        Sort = query.Sort,
                    };
                    var queryFiles =
                        db.Table_File_Upload.FirstOrDefault(c =>
                            c.IsOk && c.IsMain && c.Ref == query.Id);
                    if (queryFiles != null)
                    {
                        vm.FileName = "/Static/Content/images/Knowing/" + queryFiles.FileName +
                                      queryFiles.FileExtensions;
                    }
                    else
                    {
                        vm.FileName = "/Helper/Main/img/bg/bg.svg";
                    }



                    list.Add(vm);
                }
                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }

        public static string Edit(VMKnowing.VmKnowingManagement value, HttpPostedFileBase FileName, Guid UserId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = UserId;
                var query = db.Table_Knowing.FirstOrDefault(c => c.Id == value.Id);
                if (query != null)
                {
                    query.PrimaryTitle = value.PrimaryTitle;
                    query.SecondaryTitle = value.SecondaryTitle;
                    query.Url = value.Url;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = value.Sort;
                    db.SaveChanges();


                    var filename = "Default";
                    var fileExtention = "png";
                    var time = DateTime.Now.Ticks.ToString();
                    var code = "SP-" + time;
                    var filelenght = 200;
                    if (FileName != null)
                    {
                        var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id && c.IsMain).ToList();
                        if (qPics.Count > 0)
                        {
                            foreach (var fileUpload in qPics)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght = FileName.ContentLength;
                        filename = "Slider_" + code;
                        fileExtention = Path.GetExtension(FileName.FileName);
                        string pathCombine =
                            System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + filename + fileExtention);
                        FileName.SaveAs(pathCombine);
                        var qadd = db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            Tables = "Table_Slider",
                            Schemas = "General",
                            Ref = query.Id,
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
                        db.Table_File_Upload.Add(qadd);
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
        


        public static string Generate(VMKnowing.VmKnowingManagement value, HttpPostedFileBase FileName, Guid UserId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = UserId;
                var random = new Random();
                var id = Guid.NewGuid();
                var codes = "SP-" + DateTime.Now.Ticks;
                var isok = true;
                var query = db.Table_Knowing.Add(new Table_Knowing()
                {
                    Id = id,
                    Code = codes,
                    PrimaryTitle = value.PrimaryTitle,
                    SecondaryTitle = value.SecondaryTitle,
                    Url = value.Url,
                    CreatorDate = DateTime.Now,
                    CreatorRef = userRef,
                    IsDelete = false,
                    ModifierDate = DateTime.Now,
                    ModifierRef = userRef,
                    IsOk = isok,
                    Sort = 1,
                    Version = 1,
                });
                db.Table_Knowing.Add(query);
                db.SaveChanges();

                var filename = "Default";
                var fileExtention = "png";
                var time = DateTime.Now.Ticks.ToString();
                var code = "SP-" + time;
                var filelenght = 200;
                if (FileName != null)
                {
                    var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id && c.IsMain).ToList();
                    if (qPics.Count > 0)
                    {
                        foreach (var fileUpload in qPics)
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadKnowing + fileUpload.FileName + fileUpload.FileExtensions)))
                            {
                                File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadKnowing + fileUpload.FileName + fileUpload.FileExtensions));
                            }
                            db.Table_File_Upload.Remove(fileUpload);
                            db.SaveChanges();
                        }
                    }
                    filelenght = FileName.ContentLength;
                    filename = "Knowing_" + code;
                    fileExtention = Path.GetExtension(FileName.FileName);
                    string pathCombine =
                        System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadKnowing + filename + fileExtention);
                    FileName.SaveAs(pathCombine);
                    var qadd = db.Table_File_Upload.Add(new Table_File_Upload
                    {
                        Id = Guid.NewGuid(),
                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                        Tables = "Table_Knowing",
                        Schemas = "General",
                        Ref = query.Id,
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
                    db.Table_File_Upload.Add(qadd);
                    db.SaveChanges();
                }


                return "Success";
            }
            
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        #region Delete
        public static string DeleteRow(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Knowing.FirstOrDefault(c => c.Id == id);
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
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadKnowing + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadKnowing + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        db.Table_File_Upload.Remove(fileUpload);
                                        db.SaveChanges();
                                    }
                                }
                                db.Table_Knowing.Remove(query);
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


        #endregion

        #region Change
        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Knowing.FirstOrDefault(c => c.Id == id);
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


        #endregion
    }
}

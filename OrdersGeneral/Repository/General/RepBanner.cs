using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;

namespace OrdersGeneral.Repository.General
{
    public static class RepBanner
    {
        public static List<VMBanner.VmBannerManagement> RepositoryBannerManagements()
        {
            var list = new List<VMBanner.VmBannerManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.AsNoTracking().AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMBanner.VmBannerManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Url = item.Url,
                            CreatorDate = item.CreatorDate,
                            Sort = item.Sort,
                            Entites = item.Entites,
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

                        var queryFiles =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && c.IsMain && c.Ref == item.Id);
                        if (queryFiles != null)
                        {
                            vm.FileNameDesktop = "/Static/Content/images/Banners/Desktop/" + queryFiles.FileName +
                                                 queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";

                        }


                        var queryFiless =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && !c.IsMain && c.Ref == item.Id);
                        if (queryFiless != null)
                        {
                            vm.FileNameMobile = "/Static/Content/images/Banner/Mobile/" + queryFiless.FileName +
                                                queryFiless.FileExtensions;
                        }
                        else
                        {
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
                        }
                        list.Add(vm);
                    }
                }
            }
            catch  (Exception e)
            {
                
            }

            return list;
        }

        public static VMBanner.VmBannerManagement EditBanner(Guid Id)
        {
            var db = new Orders_Entities();
            var query = db.Table_Banner.FirstOrDefault(c => c.Id == Id);
            if (query != null)
            {
                var vm = new VMBanner.VmBannerManagement
                {
                    Id = query.Id,
                    PrimaryTitle = query.PrimaryTitle,
                    Code = query.Code,
                    Url = query.Url,
                    SecondaryTitle = query.SecondaryTitle,
                    Sort = query.Sort,
                    Entites = query.Entites,
                };
                var queryFiles =
                    db.Table_File_Upload.FirstOrDefault(c =>
                        c.IsOk && c.IsMain && c.Ref == query.Id);
                if (queryFiles != null)
                {
                    vm.FileName = "/Static/Content/images/Banner/" + queryFiles.FileName +
                                  queryFiles.FileExtensions;
                }
                else
                {
                    vm.FileName = "/Helper/Main/img/bg/bg.svg";
                }
                return vm;
            }
            else
            {
                return new VMBanner.VmBannerManagement();
            }
        }

        public static string AddNewRow(VMBanner.VmBannerManagement values,HttpPostedFileBase file,  Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = false;

                var query = db.Table_Banner.Add(new Table_Banner()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    ModifierRef = userRef,
                    Sort = values.Sort,
                    Url = values.Url,
                    Entites = values.Entites,
                });
                db.Table_Banner.Add(query);
                db.SaveChanges();

                var filename = "Default";
                var fileExtention = "png";
                var filelenght = 200;
                var filename2 = "Default";
                var fileExtention2 = "png";
                var filelenght2 = 200;
                if (file != null)
                {

                    filelenght = file.ContentLength;
                    filename = "Banner_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerDesktop + filename + fileExtention);
                    file.SaveAs(pathCombine);

                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                    qAddPic.Id = Guid.NewGuid();
                    qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                    qAddPic.Tables = "Table_Banner";
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



                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static string RepositoryBannerEdit(VMBanner.VmBannerManagement values, HttpPostedFileBase FileNameDesktop, HttpPostedFileBase FileNameMobile, Guid userId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userId;
                var query = db.Table_Banner.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.Url = values.Url;
                    query.ModifireDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = values.Sort;
                    query.Entites = values.Entites;

                    db.SaveChanges();

                    var filename = "Default";
                    var fileExtention = "png";
                    var time = DateTime.Now.Ticks.ToString();
                    var code = "SP-" + time;
                    var filelenght = 200;
                    if (FileNameDesktop != null)
                    {
                        var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id && c.IsMain).ToList();
                        if (qPics.Count > 0)
                        {
                            foreach (var fileUpload in qPics)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerDesktop + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerDesktop + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght = FileNameDesktop.ContentLength;
                        filename = "Banner_" + code;
                        fileExtention = Path.GetExtension(FileNameDesktop.FileName);
                        string pathCombine =
                            System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerDesktop + filename + fileExtention);
                        FileNameDesktop.SaveAs(pathCombine);
                        var qadd = db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            Tables = "Table_Banner",
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

                    var filename2 = "Default";
                    var fileExtention2 = "png";
                    var time2 = DateTime.Now.Ticks.ToString();
                    var code2 = "SP-" + time2;
                    var filelenght2 = 200;

                    if (FileNameMobile != null)
                    {
                        var qPics = db.Table_File_Upload.Where(c => c.Ref == query.Id && !c.IsMain).ToList();
                        if (qPics.Count > 0)
                        {
                            foreach (var fileUpload in qPics)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerMobile + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerMobile + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }

                        filelenght2 = FileNameMobile.ContentLength;
                        filename2 = "Banner_" + code2;
                        fileExtention2 = Path.GetExtension(FileNameMobile.FileName);
                        string pathCombine =
                            System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerMobile + filename2 + fileExtention2);
                        FileNameMobile.SaveAs(pathCombine);
                        var queryadd = db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            Tables = "Table_Banner",
                            Schemas = "General",
                            Ref = query.Id,
                            FileExtensions = fileExtention2,
                            FileLenght = filelenght2,
                            FileUniqeName = filename2 + fileExtention2,
                            Sort = 1,
                            IsDelete = false,
                            FileName = filename2,
                            Version = 1,
                            CreatorDate = DateTime.Now,
                            CreatorRef = userRef,
                            ModifierRef = userRef,
                            ModifierDate = DateTime.Now,
                            IsOk = true,
                            IsMain = false,
                        });
                        db.Table_File_Upload.Add(queryadd);
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

        public static string DeleteRow(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.FirstOrDefault(c => c.Id == id);
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
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerDesktop + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerDesktop + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerMobile + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainBannerMobile + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        db.Table_File_Upload.Remove(fileUpload);
                                        db.SaveChanges();
                                    }
                                }
                                db.Table_Banner.Remove(query);
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

        #region Change
        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.FirstOrDefault(c => c.Id == id);
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


        public static List<VMBanner.VmBannerManagement> RepositoryMainBanner()
        {
            var list = new List<VMBanner.VmBannerManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.Where(c => c.Entites == "MainBanner" && c.IsOk).OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var Banner in query)
                    {
                        var vm = new VMBanner.VmBannerManagement
                        {
                            Id = Banner.Id,
                            Code = Banner.Code,
                            PrimaryTitle = Banner.PrimaryTitle,
                            SecondaryTitle = Banner.SecondaryTitle,
                            Url = Banner.Url,
                            CreatorDate = Banner.CreatorDate,
                            Sort = Banner.Sort,
                            Entites = Banner.Entites,
                        };
                        var queryFiles =
                            db.Table_File_Upload.Where(c =>
                                c.IsOk && c.Ref == Banner.Id).AsNoTracking().ToList();
                        if (queryFiles.Count > 0)
                        {
                            foreach (var item in queryFiles)
                            {
                                if (item.IsMain)
                                {
                                    vm.FileNameDesktop = "/Static/Content/images/Banner/Desktop/" + item.FileName +
                                                  item.FileExtensions;
                                }
                                else
                                {
                                    vm.FileNameMobile = "/Static/Content/images/Banner/Mobile/" + item.FileName +
                                                         item.FileExtensions;
                                }

                            }
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
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

        public static List<VMBanner.VmBannerManagement> RepositoryMainBannerBS()
        {
            var list = new List<VMBanner.VmBannerManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.Where(c =>c.Entites == "BesideSlider" && c.IsOk).OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var Banner in query)
                    {
                        var vm = new VMBanner.VmBannerManagement
                        {
                            Id = Banner.Id,
                            Code = Banner.Code,
                            PrimaryTitle = Banner.PrimaryTitle,
                            SecondaryTitle = Banner.SecondaryTitle,
                            Url = Banner.Url,
                            CreatorDate = Banner.CreatorDate,
                            Sort = Banner.Sort,
                            Entites = Banner.Entites,
                        };
                        var queryFiles =
                            db.Table_File_Upload.Where(c =>
                                c.IsOk && c.Ref == Banner.Id).AsNoTracking().ToList();
                        if (queryFiles.Count > 0)
                        {
                            foreach (var item in queryFiles)
                            {
                                if (item.IsMain)
                                {
                                    vm.FileNameDesktop = "/Static/Content/images/Banners/Desktop/" + item.FileName +
                                                  item.FileExtensions;
                                }
                                else
                                {
                                    vm.FileNameMobile = "/Static/Content/images/Banners/Mobile/" + item.FileName +
                                                         item.FileExtensions;
                                }

                            }
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
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

        public static List<VMBanner.VmBannerManagement> RepositoryMainTopBanner()
        {
            var list = new List<VMBanner.VmBannerManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.Where(c => c.Entites == "TopBanner" && c.IsOk).OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var Banner in query)
                    {
                        var vm = new VMBanner.VmBannerManagement
                        {
                            Id = Banner.Id,
                            Code = Banner.Code,
                            PrimaryTitle = Banner.PrimaryTitle,
                            SecondaryTitle = Banner.SecondaryTitle,
                            Url = Banner.Url,
                            CreatorDate = Banner.CreatorDate,
                            Sort = Banner.Sort,
                            Entites = Banner.Entites,
                        };
                        var queryFiles =
                            db.Table_File_Upload.Where(c =>
                                c.IsOk && c.Ref == Banner.Id).AsNoTracking().ToList();
                        if (queryFiles.Count > 0)
                        {
                            foreach (var item in queryFiles)
                            {
                                if (item.IsMain)
                                {
                                    vm.FileNameDesktop = "/Static/Content/images/Banner/Desktop/" + item.FileName +
                                                  item.FileExtensions;
                                }
                                else
                                {
                                    vm.FileNameMobile = "/Static/Content/images/Banner/Mobile/" + item.FileName +
                                                         item.FileExtensions;
                                }

                            }
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
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

        public static List<VMBanner.VmBannerManagement> RepositoryBannerDashboardDown()
        {
            var list = new List<VMBanner.VmBannerManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.Where(c => c.Entites == "DashboardDown" && c.IsOk).OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var Banner in query)
                    {
                        var vm = new VMBanner.VmBannerManagement
                        {
                            Id = Banner.Id,
                            Code = Banner.Code,
                            PrimaryTitle = Banner.PrimaryTitle,
                            SecondaryTitle = Banner.SecondaryTitle,
                            Url = Banner.Url,
                            CreatorDate = Banner.CreatorDate,
                            Sort = Banner.Sort,
                            Entites = Banner.Entites,
                        };
                        var queryFiles =
                            db.Table_File_Upload.Where(c =>
                                c.IsOk && c.Ref == Banner.Id).AsNoTracking().ToList();
                        if (queryFiles.Count > 0)
                        {
                            foreach (var item in queryFiles)
                            {
                                if (item.IsMain)
                                {
                                    vm.FileNameDesktop = "/Static/Content/images/Banner/Desktop/" + item.FileName +
                                                  item.FileExtensions;
                                }
                                else
                                {
                                    vm.FileNameMobile = "/Static/Content/images/Banner/Mobile/" + item.FileName +
                                                         item.FileExtensions;
                                }

                            }
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
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

        public static List<VMBanner.VmBannerManagement> RepositoryBannerDashbordTop()
        {
            var list = new List<VMBanner.VmBannerManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Banner.Where(c => c.Entites == "DashbordTop" && c.IsOk).OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var Banner in query)
                    {
                        var vm = new VMBanner.VmBannerManagement
                        {
                            Id = Banner.Id,
                            Code = Banner.Code,
                            PrimaryTitle = Banner.PrimaryTitle,
                            SecondaryTitle = Banner.SecondaryTitle,
                            Url = Banner.Url,
                            CreatorDate = Banner.CreatorDate,
                            Sort = Banner.Sort,
                            Entites = Banner.Entites,
                        };
                        var queryFiles =
                            db.Table_File_Upload.Where(c =>
                                c.IsOk && c.Ref == Banner.Id).AsNoTracking().ToList();
                        if (queryFiles.Count > 0)
                        {
                            foreach (var item in queryFiles)
                            {
                                if (item.IsMain)
                                {
                                    vm.FileNameDesktop = "/Static/Content/images/Banner/Desktop/" + item.FileName +
                                                  item.FileExtensions;
                                }
                                else
                                {
                                    vm.FileNameMobile = "/Static/Content/images/Banner/Mobile/" + item.FileName +
                                                         item.FileExtensions;
                                }

                            }
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
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
    }
}

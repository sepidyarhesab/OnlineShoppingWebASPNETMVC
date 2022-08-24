using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using SepidyarHesabExtensions.Classes;
using OrdersGeneral.ViewModels.General;

namespace OrdersGeneral.Repository.General
{
    public static class RepSliders
    {
        #region Main

        public static List<VMSliders.VmMainSliders> RepositoryMainSliders()
        {
            var list = new List<VMSliders.VmMainSliders>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Slider.Where(c => c.IsOk).OrderBy(c=>c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var slider in query)
                    {
                        var vm = new VMSliders.VmMainSliders
                        {
                            Id = slider.Id,
                            Code = slider.Code,
                            PrimaryTitle = slider.PrimaryTitle,
                            SecendaryTitle = slider.SecendaryTitle,
                            Url = slider.Url,
                            CreatorDateTime = slider.CreatorDate,
                            Sort = slider.Sort,
                        };
                        var queryFiles =
                            db.Table_File_Upload.Where(c =>
                                c.IsOk && c.Ref == slider.Id).AsNoTracking().ToList();
                        if (queryFiles.Count > 0)
                        {
                            foreach (var item in queryFiles)
                            {
                                if (item.IsMain)
                                {
                                    vm.FileNameDesktop = "/Static/Content/images/Sliders/Desktop/" + item.FileName +
                                                  item.FileExtensions;
                                }
                                else
                                {
                                    vm.FileNameMobile = "/Static/Content/images/Sliders/Mobile/" + item.FileName +
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

        #endregion

        #region Management

        public static List<VMSliders.VmMainSliders> RepositoryMainSlidersManagemenet()
        {
            var list = new List<VMSliders.VmMainSliders>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Slider.AsNoTracking().AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var slider in query)
                    {
                        var vm = new VMSliders.VmMainSliders
                        {
                            Id = slider.Id,
                            Code = slider.Code,
                            PrimaryTitle = slider.PrimaryTitle,
                            SecendaryTitle = slider.SecendaryTitle,
                            Url = slider.Url,
                            CreatorDateTime = slider.CreatorDate,
                            Sort = slider.Sort,
                        };
                        var queryFiles =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && c.IsMain && c.Ref == slider.Id);
                        if (queryFiles != null)
                        {
                            vm.FileNameDesktop = "/Static/Content/images/Sliders/Desktop/" + queryFiles.FileName +
                                                 queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";
            
                        }
                                   
                        
                        var queryFiless =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && !c.IsMain && c.Ref == slider.Id);
                        if (queryFiless != null)
                        {
                            vm.FileNameMobile = "/Static/Content/images/Sliders/Mobile/" + queryFiless.FileName +
                                                 queryFiless.FileExtensions;
                        }
                        else
                        {
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
                        }

                        if (slider.IsOk)
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

                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }


        public static List<VMSliders.VmMainSliders> RepositoryMainSlidersManagemenetSearch(string searchnew)
        {
            var list = new List<VMSliders.VmMainSliders>();
            try
            {
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_Slider.OrderByDescending(c=>c.CreatorDate).Where(c=>c.Code == search || c.PrimaryTitle.Contains(search) || c.SecendaryTitle.Contains(search) || c.Url.Contains(search)).AsNoTracking().AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var slider in query)
                    {
                        var vm = new VMSliders.VmMainSliders
                        {
                            Id = slider.Id,
                            Code = slider.Code,
                            PrimaryTitle = slider.PrimaryTitle,
                            SecendaryTitle = slider.SecendaryTitle,
                            Url = slider.Url,
                            CreatorDateTime = slider.CreatorDate,
                            Sort = slider.Sort,


                        };
                        var queryFiles =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && c.IsMain && c.Ref == slider.Id);
                        if (queryFiles != null)
                        {
                            vm.FileNameDesktop = "/Static/Content/images/Sliders/Desktop/" + queryFiles.FileName +
                                                 queryFiles.FileExtensions;
                        }
                        else
                        {
                            vm.FileNameDesktop = "/Helper/Main/img/bg/bg.svg";

                        }


                        var queryFiless =
                            db.Table_File_Upload.AsNoTracking().FirstOrDefault(c =>
                                c.IsOk && c.Ref == slider.Id);
                        if (queryFiless != null)
                        {
                            vm.FileNameMobile = "/Static/Content/images/Sliders/Mobile/" + queryFiless.FileName +
                                                 queryFiless.FileExtensions;
                        }
                        else
                        {
                            vm.FileNameMobile = "/Helper/Main/img/bg/bg.svg";
                        }

                        if (slider.IsOk)
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

                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }

        #endregion

        #region Change
        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Slider.FirstOrDefault(c => c.Id == id);
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

        #region Delete
        public static string DeleteRow(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Slider.FirstOrDefault(c => c.Id == id);
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
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderMobile + fileUpload.FileName + fileUpload.FileExtensions)))
                                        {
                                            File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderMobile + fileUpload.FileName + fileUpload.FileExtensions));
                                        }
                                        db.Table_File_Upload.Remove(fileUpload);
                                        db.SaveChanges();
                                    }
                                }
                                db.Table_Slider.Remove(query);
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

        #region Add

        public static string AddNewRow(VMSliders.VmMainSlidersGenerate values, HttpPostedFileBase file, HttpPostedFileBase file2, Guid userid)
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
                var query = db.Table_Slider.Add(new Table_Slider
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecendaryTitle = values.SecendaryTitle,
                    Url = values.Url,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    Sort = values.Sort,
                });
                db.Table_Slider.Add(query);
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
                    filename = "Slider_" + code;
                    filename2 = "Slider_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    fileExtention2 = Path.GetExtension(file2.FileName);
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + filename + fileExtention);
                    file.SaveAs(pathCombine);


                    if (file2 != null)
                    {
                        string pathCombine2 =
                            HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderMobile + filename2 + fileExtention2);
                        file2.SaveAs(pathCombine2);
                    }


                    //var qPics = db.Table_File_Upload.Where(c => c.Ref == id).AsNoTracking().ToList();
                    //if (qPics.Count != 0)
                    //{
                    //    foreach (var item in qPics)
                    //    {
                    //        try
                    //        {
                    //            var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop +
                    //                                                           item.FileName + item.FileExtensions);
                    //            File.Delete(filee);
                    //            db.Table_File_Upload.Remove(item);
                    //            db.SaveChanges();
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            try
                    //            {
                    //                var filee = HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderMobile +
                    //                    item.FileName + item.FileExtensions);
                    //                File.Delete(filee);
                    //                db.Table_File_Upload.Remove(item);
                    //                db.SaveChanges();
                    //            }
                    //            catch (Exception exception)
                    //            {

                    //            }
                    //        }

                    //    }

                    //    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                    //    qAddPic.Id = Guid.NewGuid();
                    //    qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode();
                    //    qAddPic.Tables = "Table_Sliders";
                    //    qAddPic.Schemas = "General";
                    //    qAddPic.Ref = id;
                    //    qAddPic.FileExtensions = fileExtention;
                    //    qAddPic.FileLenght = filelenght;
                    //    qAddPic.FileUniqeName = filename + fileExtention;
                    //    qAddPic.Sort = 1;
                    //    qAddPic.IsDelete = false;
                    //    qAddPic.FileName = filename;
                    //    qAddPic.Version = 1;
                    //    qAddPic.CreatorDate = DateTime.Now;
                    //    qAddPic.CreatorRef = userRef;
                    //    qAddPic.ModifierRef = userRef;
                    //    qAddPic.ModifierDate = DateTime.Now;
                    //    qAddPic.IsOk = true;
                    //    qAddPic.IsMain = true;
                    //    db.SaveChanges();

                    //    if (file2 != null)
                    //    {
                    //        var qAddPic2 = db.Table_File_Upload.Add(new Table_File_Upload());
                    //        qAddPic2.Id = Guid.NewGuid();
                    //        qAddPic2.Code = SepidyarHesabCodeGenerator.GenerateCode();
                    //        qAddPic2.Tables = "Table_Sliders";
                    //        qAddPic2.Schemas = "General";
                    //        qAddPic2.Ref = id;
                    //        qAddPic2.FileExtensions = fileExtention2;
                    //        qAddPic2.FileLenght = filelenght2;
                    //        qAddPic2.FileUniqeName = filename2 + fileExtention2;
                    //        qAddPic2.Sort = 1;
                    //        qAddPic2.IsDelete = false;
                    //        qAddPic2.FileName = filename2;
                    //        qAddPic2.Version = 1;
                    //        qAddPic2.CreatorDate = DateTime.Now;
                    //        qAddPic2.CreatorRef = userRef;
                    //        qAddPic2.ModifierRef = userRef;
                    //        qAddPic2.ModifierDate = DateTime.Now;
                    //        qAddPic2.IsOk = true;
                    //        qAddPic2.IsMain = false;
                    //        db.SaveChanges();
                    //    }


                    //}
                    //else
                    //{


                    //}
                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                    qAddPic.Id = Guid.NewGuid();
                    qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                    qAddPic.Tables = "Table_Sliders";
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
                    if (file2 != null)
                    {
                        var qAddPic2 = db.Table_File_Upload.Add(new Table_File_Upload());
                        qAddPic2.Id = Guid.NewGuid();
                        qAddPic2.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                        qAddPic2.Tables = "Table_Sliders";
                        qAddPic2.Schemas = "General";
                        qAddPic2.Ref = id;
                        qAddPic2.FileExtensions = fileExtention2;
                        qAddPic2.FileLenght = filelenght2;
                        qAddPic2.FileUniqeName = filename2 + fileExtention2;
                        qAddPic2.Sort = 1;
                        qAddPic2.IsDelete = false;
                        qAddPic2.FileName = filename2;
                        qAddPic2.Version = 1;
                        qAddPic2.CreatorDate = DateTime.Now;
                        qAddPic2.CreatorRef = userRef;
                        qAddPic2.ModifierRef = userRef;
                        qAddPic2.ModifierDate = DateTime.Now;
                        qAddPic2.IsOk = true;
                        qAddPic2.IsMain = false;
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

        #region ManagemenetById
        public static List<VMSliders.VmMainSlidersGenerate> RepositoryMainSlidersManagemenetById(Guid id)
        {
            var list = new List<VMSliders.VmMainSlidersGenerate>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Slider.AsNoTracking().FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    var vm = new VMSliders.VmMainSlidersGenerate
                    {
                        Id = query.Id,
                        Code = query.Code,
                        PrimaryTitle = query.PrimaryTitle,
                        SecendaryTitle = query.SecendaryTitle,
                        Url = query.Url,
                        Sort = query.Sort,
                        

                    };
                    var queryFiles =
                        db.Table_File_Upload.FirstOrDefault(c =>
                            c.IsOk && c.IsMain && c.Ref == query.Id);
                    if (queryFiles != null)
                    {
                        vm.FileName = "/Static/Content/images/Sliders/" + queryFiles.FileName + 
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
        #endregion

        #region Edit

        public static string RepositoryMainSlidersManagemenetEditById(VMSliders.VmMainSlidersGenerate values, HttpPostedFileBase FileNameDesktop, HttpPostedFileBase FileNameMobile, Guid userId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userId;
                var query = db.Table_Slider.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecendaryTitle = values.SecendaryTitle;
                    query.Url = values.Url;
                    query.ModifireDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = values.Sort;
                    
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
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght = FileNameDesktop.ContentLength;
                        filename = "Slider_" + code;
                        fileExtention = Path.GetExtension(FileNameDesktop.FileName);
                        string pathCombine =
                            System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderDesktop + filename + fileExtention);
                        FileNameDesktop.SaveAs(pathCombine);
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
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderMobile + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderMobile + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }

                        filelenght2 = FileNameMobile.ContentLength;
                        filename2 = "Slider_" + code2;
                        fileExtention2 = Path.GetExtension(FileNameMobile.FileName);
                        string pathCombine =
                            System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainSliderMobile + filename2 + fileExtention2);
                        FileNameMobile.SaveAs(pathCombine);
                        var queryadd =db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            Tables = "Table_Slider",
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



        #endregion




    }
}





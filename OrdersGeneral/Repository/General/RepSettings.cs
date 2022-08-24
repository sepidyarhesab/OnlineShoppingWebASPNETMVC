using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;
using OrdersSettings.ViewModels.Settings;
using SepidyarHesabExtensions.Classes;

namespace OrdersGeneral.Repository.General
{
    public class RepSettings
    {
        public static Guid RepositoryTypeSelect()
        {
            
            try
            {
                var db = new Orders_Entities();
                var settingquery = db.Table_Settings.AsNoTracking().FirstOrDefault();
                if (settingquery != null)
                {
                    return settingquery.SiteTypeRef;
                }
                else
                {
                    return Guid.Empty;
                }
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }

        }


        public static bool RepositoryLoginTypeSelect()
        {

            try
            {
                var db = new Orders_Entities();
                var settingquery = db.Table_Settings.AsNoTracking().FirstOrDefault();
                if (settingquery != null)
                {
                    return settingquery.LoginType;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public static string AddNewRow(VMSettings.VMSettingsManagmet values, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = false;

                var query = db.Table_Settings.Add(new Table_Settings()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SeconderyTitle = values.SecondaryTitle,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    Facebook = values.Facebook,
                    Aparat = values.Aparat,
                    Phone = values.Phone,
                    Instagram = values.Instagram,
                    Telegram = values.Telegram,
                    Whatsapp = values.Whatsapp,
                    Email = values.Email,
                    Note = values.Note,


                });
                db.Table_Settings.Add(query);
                db.SaveChanges();



                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static List<VMSettings.VMSettingsManagmet> RepositoryMainsSettingsManagmets()
        {
            var list = new List<VMSettings.VMSettingsManagmet>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Settings.AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var settings in query)
                    {
                        var vm = new VMSettings.VMSettingsManagmet
                        {
                            Id = settings.Id,
                            Code = settings.Code,
                            PrimaryTitle = settings.PrimaryTitle,
                            SecondaryTitle = settings.SeconderyTitle,
                            Aparat = settings.Aparat,
                            CreatorDate = settings.CreatorDate,
                            CreatorRef = settings.CreatorRef,
                            Email = settings.Email,
                            Facebook = settings.Facebook,
                            Instagram = settings.Instagram,
                            ModifierRef = settings.ModifierRef,
                            Note = settings.Note,
                            Phone = settings.Phone,
                            Telegram = settings.Telegram,
                            Version = settings.Version,
                            ModifierDate = settings.ModifireDate,
                            Whatsapp = settings.Whatsapp,

                        };


                        if (settings.IsOk)
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



        public static List<VMSettings.VMSettingsManagmet> RepositoryMainsSettingsManagmetsById(Guid id)
        {
            var list = new List<VMSettings.VMSettingsManagmet>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Settings.AsNoTracking().FirstOrDefault();
                if (query != null)
                {
                    var vm = new VMSettings.VMSettingsManagmet
                    {
                        Id = query.Id,
                        Code = query.Code,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SeconderyTitle,
                        Aparat = query.Aparat,
                        CreatorDate = query.CreatorDate,
                        CreatorRef = query.CreatorRef,
                        Email = query.Email,
                        Facebook = query.Facebook,
                        Instagram = query.Instagram,
                        ModifierRef = query.ModifierRef,
                        Whatsapp = query.Whatsapp,
                        Note = query.Note,
                        Phone = query.Phone,
                        Telegram = query.Telegram,
                        Version = query.Version,
                        ModifierDate = query.ModifireDate,
                        
                    };


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

                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }


        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Settings.FirstOrDefault(c => c.Id == id);
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

        public static string RepositoryMainSettingsManagemenetEditById(VMSettings.VMSettingsManagmet values, HttpPostedFileBase file, HttpPostedFileBase file2, Guid userId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userId;
                var query = db.Table_Settings.FirstOrDefault(c => c.Id == values.Id);
                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SeconderyTitle = values.SecondaryTitle;
                    query.Facebook = values.Facebook;
                    query.Aparat = values.Aparat;
                    query.Email = values.Email;
                    query.Instagram = values.Instagram;
                    query.Whatsapp = values.Whatsapp;
                    query.Phone = values.Phone;
                    query.Note = values.Note;
                    query.Telegram = values.Telegram;
                    query.ModifireDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.IsOk = true;
                    db.SaveChanges();

                    var filename = "Default";
                    var fileExtention = "png";
                    var time = DateTime.Now.Ticks.ToString();
                    var code = "SP-" + time;
                    var filelenght = 200;
                    if (file != null)
                    {

                        var queryFile =
                            db.Table_File_Upload.Where(c =>c.Ref == query.Id && c.IsMain).ToList();
                        if (queryFile.Count > 0)
                        {
                            foreach (var fileUpload in queryFile)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainLogo + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainLogo + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght = file.ContentLength;
                        filename = "Logo_" + code;
                        fileExtention = Path.GetExtension(file.FileName);
                        string pathCombine =
                            System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainLogo + filename + fileExtention);
                        file.SaveAs(pathCombine);
                        db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            Tables = "Table_Setting",
                            Schemas = "Settings",
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
                        db.SaveChanges();
                    }



                    var filename2 = "Default";
                    var fileExtention2 = "png";
                    var time2 = DateTime.Now.Ticks.ToString();
                    var code2 = "SP-" + time2;
                    var filelenght2 = 200;
                    if (file2 != null)
                    {

                        var queryFile =
                            db.Table_File_Upload.Where(c => c.Ref == query.Id && !c.IsMain).ToList();
                        if (queryFile.Count  > 0)
                        {
                            foreach (var fileUpload in queryFile)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainLogo + fileUpload.FileName + fileUpload.FileExtensions)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainLogo + fileUpload.FileName + fileUpload.FileExtensions));
                                }
                                db.Table_File_Upload.Remove(fileUpload);
                                db.SaveChanges();
                            }
                        }
                        filelenght2 = file2.ContentLength;
                        filename2 = "Logo_" + code2;
                        fileExtention2 = Path.GetExtension(file2.FileName);
                        string pathCombine = System.Web.HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainLogo + filename2 + fileExtention2);
                        file2.SaveAs(pathCombine);
                        db.Table_File_Upload.Add(new Table_File_Upload
                        {
                            Id = Guid.NewGuid(),
                            Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload"),
                            Tables = "Table_Setting",
                            Schemas = "Settings",
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

                        db.SaveChanges();

                    }

                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }



            #endregion
        }
        public static string DeleteSetting(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Settings.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Settings.Remove(query);
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


        public static List<VMSettings.VMSettingsManagmet> RepositorySiteType()
        {
            var list = new List<VMSettings.VMSettingsManagmet>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Site_Type.AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var settings in query)
                    {
                        var vm = new VMSettings.VMSettingsManagmet
                        {
                            Id = settings.Id,
                            Code = settings.Code,
                            PrimaryTitle = settings.TypeTitle,
                            CreatorDate = settings.CreatorDate,
                            ModifierDate= settings.ModifierDate,
                            CreatorRef = settings.CreatorRef,
                            ModifierRef = settings.ModifierRef,
                            Version = settings.Version,
                           
                        };

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

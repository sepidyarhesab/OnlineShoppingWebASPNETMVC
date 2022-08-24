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


namespace OrdersGeneral.Repository.General
{
    public static class RepMenuAdmin
    {
        public static List<VmMenuAdmin.VmMenuAccessClient> RepAccessClients(Guid id, Guid catRef)
        {
            var list = new List<VmMenuAdmin.VmMenuAccessClient>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_MenuAccess.OrderBy(c => c.Sort).Where(c => c.UserRef == id && c.IsOk && !c.IsDelete).ToList();

                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {

                        var queryMenu =
                            db.Table_MenuAdmin.FirstOrDefault(c => c.Id == item.MenuRef && c.CategoryRef == catRef && c.IsOk && !c.IsDelete);
                        if (queryMenu != null)
                        {
                            var vm = new VmMenuAdmin.VmMenuAccessClient
                            {
                                Id = item.Id,
                                Code = item.Code,
                                MenuRef = item.MenuRef,
                                UserRef = item.UserRef,
                                PrimaryTitle = queryMenu.PrimaryTitle,
                                Url = queryMenu.Url,
                                Sort = item.Sort,
                            };

                            var queryIconPic =
                                db.Table_File_Upload.FirstOrDefault(c =>
                                    c.Ref == queryMenu.Id && c.IsOk && !c.IsDelete);
                            if (queryIconPic!=null)
                            {
                                vm.FileName = "/Static/Content/Images/Menu/" + queryIconPic.FileName +
                                              queryIconPic.FileExtensions;
                            }
                            else
                            {
                                vm.FileName = "/Static/Content/Images/Menu/home_100px.png";
                            }



                            list.Add(vm);
                        }
                        else
                        {

                        }
                    }
                }


                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }
        public static List<VmMenuAdmin.VmMenuAccessClient> RepAccessMangment(Guid id)
        {
            var list = new List<VmMenuAdmin.VmMenuAccessClient>();
            try
            {
                var db = new Orders_Entities();
                var rol = "5bf16139-6715-4190-988a-4849c1a0241d";
                var query = db.Table_MenuAccess.Where(c => c.UserRef == id).OrderBy(c => c.Sort).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmMenuAdmin.VmMenuAccessClient
                        {
                            Id = item.Id,
                            Code = item.Code,
                            MenuRef = item.MenuRef,
                            UserRef = item.UserRef,
                            RoleRef = item.RoleRef,
                            IsOk = item.IsOk

                        };



                        var queryMenu =
                            db.Table_MenuAdmin.FirstOrDefault(c => c.Id == item.MenuRef && c.IsOk && !c.IsDelete);
                        if (queryMenu != null)
                        {
                            vm.PrimaryTitle = queryMenu.PrimaryTitle;
                            vm.Url = queryMenu.Url;

                        }
                        else
                        {
                            vm.PrimaryTitle = "فهرست یافت نشد";
                        }


                        var queryFile =
                            db.Table_File_Upload.FirstOrDefault(c => c.Ref == item.MenuRef && c.IsOk && !c.IsDelete);
                        if (queryFile != null)
                        {
                            vm.FileName = "/Static/Content/Images/Menu/" + queryFile.FileName +
                                          queryFile.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/Menu/" + "home_100px.png";
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
        public static List<VmMenuAdmin.VmMenuAccessClient> RepAdminAccessManagement(Guid id)
        {
            var list = new List<VmMenuAdmin.VmMenuAccessClient>();
            try
            {
                var db = new Orders_Entities();
                var role = Guid.Parse("5bf16139-6715-4190-988a-4849c1a0241d");
                var queryUser = db.Table_User.FirstOrDefault(c => c.Id == id && c.RoleRef == role);
                if (queryUser != null)
                {
                    var query = db.Table_MenuAccess.Where(c => c.UserRef == id).OrderBy(c => c.Sort).ToList();
                    if (query.Count < 1)
                    {
                        var listdmin = db.Table_MenuAdmin.ToList();
                        if (listdmin.Count > 0)
                        {
                            foreach (var admin in listdmin)
                            {
                                var addadmin = db.Table_MenuAccess.Add(new Table_MenuAccess
                                {
                                    Id = Guid.NewGuid(),
                                    Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_MenuAccess"),
                                    Sort = 1,
                                    IsOk = true,
                                    UserRef = id,
                                    CreatorDate = DateTime.Now,
                                    CreatorRef = id,
                                    IsDelete = false,
                                    MenuRef =admin.Id,
                                    ModifierRef = id,
                                    ModifireDate = DateTime.Now,
                                    RoleRef = role,
                                    Version = 1,
                                });
                                db.Table_MenuAccess.Add(addadmin);
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var queryMenuAdmin = db.Table_MenuAdmin.ToList();
                        if (queryMenuAdmin.Count > 0)
                        {
                            foreach (var menuAdmin in queryMenuAdmin)
                            {
                                var queryCheckExist =
                                    db.Table_MenuAccess.ToList().Exists(c =>c.UserRef == id && c.MenuRef == menuAdmin.Id);
                                if (!queryCheckExist)
                                {
                                    var addadmin = db.Table_MenuAccess.Add(new Table_MenuAccess
                                    {
                                        Id = Guid.NewGuid(),
                                        Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_MenuAccess"),
                                        Sort = 1,
                                        IsOk = true,
                                        UserRef = id,
                                        CreatorDate = DateTime.Now,
                                        CreatorRef = id,
                                        IsDelete = false,
                                        MenuRef = menuAdmin.Id,
                                        ModifierRef = id,
                                        ModifireDate = DateTime.Now,
                                        RoleRef = role,
                                        Version = 1,
                                    });
                                    db.Table_MenuAccess.Add(addadmin);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }



                return list;
            }
            catch (Exception e)
            {
                return list;
            }
        }
        public static List<VmMenuAdmin.VmMenuAdminManagement> RepAdminMenuManagements()
        {
            var list = new List<VmMenuAdmin.VmMenuAdminManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_MenuAdmin.AsNoTracking().OrderBy(c => c.Sort).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmMenuAdmin.VmMenuAdminManagement()
                        {
                            Id = item.Id,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Code = item.Code,
                            Url = item.Url,
                            Sort = item.Sort,
                            CategoryRef = item.CreatorRef,
                            IsOk = item.IsOk,
                            IsDelete = item.IsDelete,
                            CreatorDate = item.CreatorDate,
                        };

                        var queryIconPic =
                            db.Table_File_Upload.FirstOrDefault(c => c.Ref == item.Id && c.IsOk && !c.IsDelete);
                        if (queryIconPic != null)
                        {
                            vm.FileName = "/Static/Content/Images/Menu/" + queryIconPic.FileName +
                                          queryIconPic.FileExtensions;
                        }
                        else
                        {
                            vm.FileName = "/Static/Content/Images/Menu/home_100px.png";
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
        public static List<VmMenuAdmin.VmMenuAdminManagement> RepAdminMenuManagementsSearch(string searchnew)
        {
            var list = new List<VmMenuAdmin.VmMenuAdminManagement>();
            try
            {
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_MenuAdmin.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search) || c.SecondaryTitle.Contains(search) || c.Url.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmMenuAdmin.VmMenuAdminManagement()
                        {
                            Id = item.Id,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            TertiaryTitle = "/Static/Content/Images/Menu/" + item.TertiaryTitle,
                            Code = item.Code,
                            Url = item.Url,
                            Sort = item.Sort,
                            CategoryRef = item.CreatorRef,
                            IsOk = item.IsOk,
                            IsDelete = item.IsDelete,
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


        public static List<VmMenuAdmin.VmMenuAdminManagement> RepAdminMenuCategoreis()
        {
            var list = new List<VmMenuAdmin.VmMenuAdminManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_MenuAdmin_Category.AsNoTracking().OrderBy(c => c.Sort).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmMenuAdmin.VmMenuAdminManagement()
                        {
                            Id = item.Id,
                            PrimaryTitle = item.PrimaryTitle,
                            Sort = item.Sort,
                            IsOk = item.IsOk,
                            IsDelete = item.IsDelete,
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

        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_MenuAdmin.FirstOrDefault(c => c.Id == id);
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

        public static string Delete(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_MenuAdmin.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsOk)
                    {
                        case true:
                            {
                                return "True";
                            }
                        case false:
                            {
                                db.Table_MenuAdmin.Remove(query);
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

        public static string Add(VmMenuAdmin.VmMenuAdminManagement value, Guid UserId,HttpPostedFileBase file)
        {
            try
            {
                var random = new Random();
                var db = new Orders_Entities();
                var UserRef = UserId;
                var id = Guid.NewGuid();
                var code =  SepidyarHesabCodeGenerator.GenerateCode("","");
                var query = db.Table_MenuAdmin.Add(new Table_MenuAdmin()
                {
                    Id = id,
                    PrimaryTitle = value.PrimaryTitle,
                    SecondaryTitle = value.SecondaryTitle,
                    TertiaryTitle = value.TertiaryTitle,
                    Code = code,
                    Url = value.Url,
                    Sort = value.Sort,
                    IsDelete = value.IsDelete,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Version = 1,
                    CreatorRef = UserRef,
                    ModifierRef = UserRef,
                    CategoryRef = value.CategoryRef,
                });
                db.Table_MenuAdmin.Add(query);
                db.SaveChanges();




                var filename = "Default";
                var fileExtention = "png";
                var filelenght = 200;
                if (file != null)
                {

                    filelenght = file.ContentLength;
                    filename = "Menu_" + code;
                    fileExtention = Path.GetExtension(file.FileName);
                    string pathCombine =
                        HttpContext.Current.Server.MapPath(ServerPath.ServerPathFileUploadMainMenuAdmin + filename + fileExtention);
                    file.SaveAs(pathCombine);


                    var qAddPic = db.Table_File_Upload.Add(new Table_File_Upload());
                    qAddPic.Id = Guid.NewGuid();
                    qAddPic.Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_File_Upload");
                    qAddPic.Tables = "Table_Menu_Admin";
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
                    qAddPic.CreatorRef = UserId;
                    qAddPic.ModifierRef = UserId;
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

        public static VmMenuAdmin.VmMenuAdminManagement Edit(Guid? id)
        {
            var db = new Orders_Entities();
            var query = db.Table_MenuAdmin.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {
                var vm = new VmMenuAdmin.VmMenuAdminManagement
                {
                    Id = query.Id,
                    PrimaryTitle = query.PrimaryTitle,
                    Code = query.Code,
                    Url = query.Url,
                    SecondaryTitle = query.SecondaryTitle,
                    TertiaryTitle = query.TertiaryTitle,
                    Sort = query.Sort,
                    IsOk = query.IsOk,
                    CategoryRef = query.CategoryRef,
                };
                return vm;
            }
            else
            {
                return new VmMenuAdmin.VmMenuAdminManagement();
            }
        }

    }


}
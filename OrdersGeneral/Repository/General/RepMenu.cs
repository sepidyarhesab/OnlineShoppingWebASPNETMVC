using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersGeneral.ViewModels.General;


namespace OrdersGeneral.Repository.General
{
    public static class RepMenu
    {
        public static string DeleteRow(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Menu.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Menu.Remove(query);
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


        public static string AddNewRow(VMMenu.VmMenuMainMenuManagement values, Guid UserId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef =UserId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = false;

                var query = db.Table_Menu.Add(new Table_Menu()
                {
                    Id = id,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondryTitle,
                    Url = values.Url,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = false,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    Sort = values.Sort,
                    TertiaryTitle = values.Icon,
                });
                db.Table_Menu.Add(query);
                db.SaveChanges();



                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static List<VMMenu.VmMenuMainMenu> RepositoryMainMenu()
        {
            var list = new List<VMMenu.VmMenuMainMenu>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Menu.Where(c => c.IsOk).OrderBy(c=>c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var menu in query)
                    {
                        var vm = new VMMenu.VmMenuMainMenu
                        {
                            Id = menu.Id,
                            Code = menu.Code,
                            IsOk = menu.IsOk,
                            PrimaryTitle = menu.PrimaryTitle,
                            SecondryTitle = menu.SecondaryTitle,
                            Sort = menu.Sort,
                            Url =  menu.Url,
                            Icon = "/Helper/Layouts/images/icon/" + menu.TertiaryTitle,
                            IconMobile = "/Helper/Layouts/images/icon/" + menu.TertiaryTitle.Replace("White", "Black"),
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


        public static List<VMMenu.VmMenuMainMenuManagement> RepositoryMainMenuManagementById(Guid Id)
        {
            var list = new List<VMMenu.VmMenuMainMenuManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Menu.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMMenu.VmMenuMainMenuManagement
                    {
                        Id = query.Id,
                        Code = query.Code,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondryTitle = query.SecondaryTitle,
                        Sort = query.Sort,
                        Url = query.Url,
                        Icon = query.TertiaryTitle,
                        CreatorDateTime = query.CreatorDate,
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

        public static string RepositoryMainMenuManagementEdit(VMMenu.VmMenuMainMenuManagement value, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var query = db.Table_Menu.FirstOrDefault(c => c.Id == value.Id);
                if (query != null)
                {
                    query.PrimaryTitle = value.PrimaryTitle;
                    query.Url = value.Url;
                    query.SecondaryTitle = value.SecondryTitle;
                    query.Sort = value.Sort;
                    query.TertiaryTitle = value.Icon;
                    query.Version++;
                    query.ModifireDate = DateTime.Now;
                    query.ModifierRef = userRef;

                    db.SaveChanges();
                }

                return "Success ";
            }
            catch (Exception e)
            {

                return "ApplictationError" + e.Message;
            }
        }
        public static string ChangeStatus(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Menu.FirstOrDefault(c => c.Id == id);
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


        public static List<VMMenu.VmMenuMainMenuManagement> RepositoryMainMenuManagement()
        {
            var list = new List<VMMenu.VmMenuMainMenuManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Menu.OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var menu in query)
                    {
                        var vm = new VMMenu.VmMenuMainMenuManagement
                        {
                            Id = menu.Id,
                            Code = menu.Code,
                            PrimaryTitle = menu.PrimaryTitle,
                            SecondryTitle = menu.SecondaryTitle,
                            Sort = menu.Sort,
                            Url = menu.Url,
                            Icon = menu.TertiaryTitle,
                            CreatorDateTime = menu.CreatorDate,
                        };
                        if (menu.IsOk)
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



        public static List<VMMenu.VmMenuMainMenuManagement> RepositoryMainSearch(string searchnew)
        {
            var list = new List<VMMenu.VmMenuMainMenuManagement>();
            try
            {
                var search = searchnew.Replace("	", "");
                var db = new Orders_Entities();
                var query = db.Table_Menu.OrderByDescending(c => c.CreatorDate).Where(c =>
                    c.Code == search || c.PrimaryTitle.Contains(search) || c.SecondaryTitle.Contains(search) ||
                    c.Url.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)    
                {
                    foreach (var item in query)
                    {
                        var vm = new VMMenu.VmMenuMainMenuManagement
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondryTitle = item.SecondaryTitle,
                            Sort = item.Sort,
                            Url = item.Url,
                            Icon = item.TertiaryTitle,
                            CreatorDateTime = item.CreatorDate,
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
            }
            catch (Exception e)
            {
                return list;
            }

            return list;
        }

    }
}
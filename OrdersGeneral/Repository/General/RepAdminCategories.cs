using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersGeneral.ViewModels.General;


namespace OrdersGeneral.Repository.General
{
    public static class RepAdminCategories
    {
        public static List<VmAdminCategories.VmAdminCategoriesManagement> RepositoryAdminCategoriesManagement()
        {
            var list = new List<VmAdminCategories.VmAdminCategoriesManagement>();
            var db = new Orders_Entities();
            try
            {
                var query = db.Table_MenuAdmin_Category.OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmAdminCategories.VmAdminCategoriesManagement()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
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

        public static List<VmAdminCategories.VmAdminCategoriesManagement> RepositoryAdminCategoriesManagementSearch(string searchnew)
        {
            var list = new List<VmAdminCategories.VmAdminCategoriesManagement>();
            var db = new Orders_Entities();
            try
            {
                var search = searchnew.Replace("  ", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var query = db.Table_MenuAdmin_Category.OrderByDescending(c=>c.CreatorDate).Where(c=>c.Code == search || c.PrimaryTitle.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VmAdminCategories.VmAdminCategoriesManagement()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
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
                var query = db.Table_MenuAdmin_Category.FirstOrDefault(c => c.Id == id);
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

        public static string Add(VmAdminCategories.VmAdminCategoriesManagement value, Guid UserId)
        {
            try
            {
                var random = new Random();
                var db = new Orders_Entities();
                var UserRef = UserId;
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var query = db.Table_MenuAdmin_Category.Add(new Table_MenuAdmin_Category
                {
                    Id = id,
                    Code = code,
                    PrimaryTitle = value.PrimaryTitle,
                    Sort = value.Sort,
                    IsDelete = value.IsDelete,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Version = 1,
                    CreatorRef = UserRef,
                    ModifierRef = UserRef,
                });
                db.Table_MenuAdmin_Category.Add(query);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public static VmAdminCategories.VmAdminCategoriesManagement Edit(Guid? id)
        {
            var db = new Orders_Entities();
            var query = db.Table_MenuAdmin_Category.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {
                var vm = new VmAdminCategories.VmAdminCategoriesManagement
                {
                    Id = query.Id,
                    PrimaryTitle = query.PrimaryTitle,
                    Sort = query.Sort,
                    IsOk = query.IsOk
                };
                return vm;
            }

            return new VmAdminCategories.VmAdminCategoriesManagement();
        }

        public static string Delete(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_MenuAdmin_Category.FirstOrDefault(c => c.Id == id);
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
                            db.Table_MenuAdmin_Category.Remove(query);
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
    }
}
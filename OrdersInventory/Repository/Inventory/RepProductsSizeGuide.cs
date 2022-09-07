using OrdersDatabase.Models;
using OrdersInventory.ViewModels.Inventory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersInventory.Repository.Inventory
{
    public class RepProductsSizeGuide
    {
        //<<<<<<<<<<<<<<SIZE GUIDE REPOSITOREIS>>>>>>>>>>>>>>>>>

        //Repository AdminProductSizeGuideList
        public static List<VMProductsSizeGuides.ViewModelProductSizeGuide> RepositoryAdminProductSizeGuideList()
        {
            var list = new List<VMProductsSizeGuides.ViewModelProductSizeGuide>();
            var db = new Orders_Entities();
            //var db = new Orders_Entities();
            try
            {
                var query = db.Table_Product_SizeGuide.OrderBy(c => c.Sort).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMProductsSizeGuides.ViewModelProductSizeGuide()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PrimaryTitle = item.PrimaryTitle,
                            CategoriesRef = item.CategoryRef,
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
        //End-----------------------------------------
        //Repository AdminProductSizeGuideSearch
        public static List<VMProductsSizeGuides.ViewModelProductSizeGuide> RepositoryAdminProductSizeGuideSearch(string searchnew)
        {
            var list = new List<VMProductsSizeGuides.ViewModelProductSizeGuide>();
            var db = new Orders_Entities();
            try
            {
                var search = searchnew.Replace("  ", "");
                //var searcha = string.IsNullOrWhiteSpace(searchnew);
                var query = db.Table_Product_SizeGuide.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PrimaryTitle.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMProductsSizeGuides.ViewModelProductSizeGuide()
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

        //End-----------------------------
        //Repository ListCategory
        public static List<VMProductsSizeGuides.ViewModelProductSizeGuide> ListCategory()
        {
            var list = new List<VMProductsSizeGuides.ViewModelProductSizeGuide>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Product_Category.OrderByDescending(c => c.CreatorDate).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMProductsSizeGuides.ViewModelProductSizeGuide
                        {
                            CategoriesRef = item.Id,
                            CategoryTitle = item.PrimaryTitle,
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

        //End---------------------------------
        //Repository Add New SizeGuide
        public static string Add(VMProductsSizeGuides.ViewModelProductSizeGuide value, Guid UserId)
        {
            try
            {
                var random = new Random();
                var db = new Orders_Entities();
                var UserRef = UserId;
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var query = db.Table_Product_SizeGuide.Add(new Table_Product_SizeGuide()
                {
                    Id = id,
                    Code = code,
                    PrimaryTitle = value.PrimaryTitle,
                    SecondaryTitle = value.SecondaryTitle,
                    CategoryRef = value.CategoriesRef,
                    Sort = value.Sort,
                    IsDelete = value.IsDelete,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Version = 1,
                    CreatorRef = UserRef,
                    ModifierRef = UserRef,
                });
                db.Table_Product_SizeGuide.Add(query);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        //End--------------------------------
        //Repository Delete SizeGuide for a Category
        public static string DeleteSizeGuideCategory(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Product_SizeGuide.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Product_SizeGuide.Remove(query);
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
        //End-----------------------------------------
        //Repository Change Status ProductSizeGuide
        public static string ChangeStatusProductSizeGuide(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Product_SizeGuide.FirstOrDefault(c => c.Id == id);
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
        //End-----------------------------
        //Repository Edit ProductSizeGuide
        public static VMProductsSizeGuides.ViewModelProductSizeGuide EditSizeGuide(Guid id)
        {
            var db = new Orders_Entities();
            var query = db.Table_Product_SizeGuide.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {
                var vm = new VMProductsSizeGuides.ViewModelProductSizeGuide
                {
                    Id = query.Id,
                    PrimaryTitle = query.PrimaryTitle,
                    SecondaryTitle = query.SecondaryTitle,
                    CategoriesRef = query.CategoryRef,
                    Sort = query.Sort,
                    IsOk = query.IsOk
                };
                var qcattitle = db.Table_Product_Category.FirstOrDefault(c => c.Id == query.CategoryRef);
                if (qcattitle != null)
                {
                    vm.CategoryTitle = qcattitle.PrimaryTitle;
                }
                return vm;
            }

            return new VMProductsSizeGuides.ViewModelProductSizeGuide();
        }
        //End----------------------------------------------------
        // <<<<<<<<<<<<Table_Product_SizeGuideValues Repositories>>>>>>>>>>>>>
        //Repository AdminProductSizeGuideValuesList
        public static List<VMProductsSizeGuides.ViewModelProductSizeValuesGuide> RepositoryAdminProductSizeGuideValuesList(Guid id)
        {
            var list = new List<VMProductsSizeGuides.ViewModelProductSizeValuesGuide>();
            var db = new Orders_Entities();
            try
            {
                var query = db.Table_Product_SizeGuideValues.Where(c => c.SizeGuideRef==id).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMProductsSizeGuides.ViewModelProductSizeValuesGuide()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PropertyTitle = item.PropertyTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            SizeGuideRef = item.SizeGuideRef,
                            SizeValue = item.SizeValue,
                            Sort = item.Sort,
                            IsOk = item.IsOk,
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

                        var qcat = db.Table_Product_SizeGuide.FirstOrDefault(c => c.Id == item.SizeGuideRef);
                        if (qcat != null)
                        {
                            vm.SizeGuideTitle = qcat.PrimaryTitle;
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
        //End-----------------------------------------
        //Repository AdminProductSizeGuideSearch
        public static List<VMProductsSizeGuides.ViewModelProductSizeValuesGuide> RepositoryAdminProductSizeGuideValuesSearch(string searchnew)
        {
            var list = new List<VMProductsSizeGuides.ViewModelProductSizeValuesGuide>();
            var db = new Orders_Entities();
            try
            {
                var search = searchnew.Replace("  ", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var query = db.Table_Product_SizeGuideValues.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.PropertyTitle.Contains(search)).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMProductsSizeGuides.ViewModelProductSizeValuesGuide()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            PropertyTitle = item.PropertyTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            SizeGuideRef = item.SizeGuideRef,
                            Sort = item.Sort,
                            SizeValue = item.SizeValue,
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
        //End--------------------------------
        //Repository Delete SizeGuide for Table productSizeGuideValues
        public static string DeleteSizeGuideValues(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Product_SizeGuideValues.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Product_SizeGuideValues.Remove(query);
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
        //End-----------------------------------------
        //Repository Change Status ProductSizeGuide
        public static string ChangeStatusProductSizeGuideValues(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Product_SizeGuideValues.FirstOrDefault(c => c.Id == id);
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
        //End---------------------------------
        //Repository Add New SizeGuideValue
        public static string AddSizeGuideValues(VMProductsSizeGuides.ViewModelProductSizeValuesGuide value, Guid UserId)
        {
            try
            {
                var random = new Random();
                var db = new Orders_Entities();
                var UserRef = UserId;
                var iid = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var query = db.Table_Product_SizeGuideValues.Add(new Table_Product_SizeGuideValues()
                {
                    Id = iid,
                    Code = code,
                    PropertyTitle = value.PropertyTitle,
                    SecondaryTitle = value.SecondaryTitle,
                    CreatorRef = UserRef,
                    SizeGuideRef = value.SizeGuideRef,
                    Sort = value.Sort,
                    IsOk = value.IsOk,
                    IsDelete = false,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Version = 1,
                    ModifierRef = UserRef,
                    SizeValue = value.SizeValue,
                });
                db.Table_Product_SizeGuideValues.Add(query);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        //End-----------------------------
        //Repository Edit ProductSizeGuideValues
        public static VMProductsSizeGuides.ViewModelProductSizeValuesGuide EditSizeGuideValues(Guid id)
        {
            var db = new Orders_Entities();
            var query = db.Table_Product_SizeGuideValues.FirstOrDefault(c => c.Id == id);
            if (query != null)
            {
                var vm = new VMProductsSizeGuides.ViewModelProductSizeValuesGuide()
                {
                    Id = query.Id,
                    PropertyTitle= query.PropertyTitle,
                    SecondaryTitle = query.SecondaryTitle,
                    SizeGuideRef = query.SizeGuideRef,
                    SizeValue = query.SizeValue,
                    Sort = query.Sort,
                    IsOk = query.IsOk
                };
                //var qcattitle = db.Table_Product_Category.FirstOrDefault(c => c.Id == query.CategoryRef);
                //if (qcattitle != null)
                //{
                //    vm.CategoryTitle = qcattitle.PrimaryTitle;
                //}
                return vm;
            }

            return new VMProductsSizeGuides.ViewModelProductSizeValuesGuide();
        }
        //End---------------------------------------------------
        //<<<<<<<<<<<<<<<<<<<Client Repositories>>>>>>>>>>>>>>>>>>>>>>>>>>
        //Repository Show Size Guide Table for a Category
        public static List<VMProductsSizeGuides.ViewModelProductSizeValuesGuide> RepositoryClientProductCategorySizeGuideValuesList(Guid? CategoryId)
        {
            var list = new List<VMProductsSizeGuides.ViewModelProductSizeValuesGuide>();
            var db = new Orders_Entities();
            try
            {
                var qcat = db.Table_Product_SizeGuide.FirstOrDefault(c => c.CategoryRef == CategoryId);
                if (qcat != null)
                {
                    var query = db.Table_Product_SizeGuideValues.Where(c => c.SizeGuideRef == qcat.Id).AsNoTracking().ToList();
                    if (query.Count > 0)
                    {

                        foreach (var item in query)
                        {
                            var vm = new VMProductsSizeGuides.ViewModelProductSizeValuesGuide()
                            {
                                Id = item.Id,
                                Code = item.Code,
                                PropertyTitle = item.PropertyTitle,
                                SecondaryTitle = item.SecondaryTitle,
                                SizeGuideRef = item.SizeGuideRef,
                                SizeValue = item.SizeValue,
                                Sort = item.Sort,
                                IsOk = item.IsOk,
                                CreatorDate = item.CreatorDate,
                                SizeGuideTitle = qcat.PrimaryTitle,
                            };

                            list.Add(vm);
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

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;
using OrdersInventory.ViewModels.Inventory;
using OrdersOrders.ViewModels.Orders;
using OrdersSettings.Repository.Settings;
using Rozhano_WebApplication2.Extensions;
using SepidyarHesabExtensions.Classes;

namespace OrdersOrders.Repository.Orders
{
    public static class RepDiscount
    {

        #region Managment

        public static List<VMDiscount.VmDiscountManagement> RepositoryDiscountManagements()
        {
            var list = new List<VMDiscount.VmDiscountManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Discount.Where(c => c.Entities != "AllDiscount").OrderByDescending(c => c.CreatorDate).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var dt = new PersianDate();
                        var end = item.EndDate;
                        var start = item.StartDate;
                        var dr = dt.MiladiToShamsi(end);
                        var dr1 = dt.MiladiToShamsi(start);
                        var endd = dr + ' ' + string.Format("{0:HH:mm:ss}", item.EndDate.TimeOfDay.ToString());
                        var startd = dr1 + ' ' + string.Format("{0:HH:mm:ss}", item.StartDate.TimeOfDay.ToString());

                        var vm = new VMDiscount.VmDiscountManagement()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Discount = item.Discount,
                            DiscountCode = item.DiscountCode,
                            ProductRef = item.ProductRef,
                            CategoriesRef = item.CategoriesRef,
                            Sort = item.Sort,
                            IsMain = item.IsMain,
                            DiscountCount = item.DiscountCount,
                            DiscountFee = item.DiscountFee,
                            DiscountPercent = item.DiscountPercent,
                            DiscountQuantity = item.DiscountQuantity,
                            DiscountUsed = item.DiscountUsed,
                            DiscountUser = item.DiscountUser,
                            StartDate1 = startd,
                            EndDate1 = endd,
                            Entities = item.Entities
                        };
                        if (item.IsOk)
                        {
                            vm.IsOkTitle = "فعال";
                            vm.IsOkClass = "label label-success";
                        }
                        else
                        {
                            vm.IsOkTitle = "غیر فعال";
                            vm.IsOkClass = "label label-danger";
                        }

                        var queryProduct = db.Table_Product.FirstOrDefault(c => c.Id == item.ProductRef);
                        if (queryProduct != null)
                        {
                            vm.ProductTitle = queryProduct.PrimaryTitle;
                        }

                        var queryCategory = db.Table_Product_Category.FirstOrDefault(c => c.Id == item.CategoriesRef);
                        if (queryCategory != null)
                        {
                            vm.CategoriesTitle = queryCategory.PrimaryTitle;
                        }
                        else
                        {
                            vm.CategoriesTitle = "ندارد";
                        }

                        var queryUser = db.Table_User.FirstOrDefault(c => c.Id == item.DiscountUser);
                        if (queryUser != null)
                        {

                            vm.FullName = queryUser.Name + " " + queryUser.Family;
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


        public static string AddNewDiscount(VMDiscount.VmDiscountManagement values, Guid userId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userId;
                var random = new Random();
                var id = Guid.NewGuid();
                var catRef = values.CategoriesRef;
                var ProductRef = values.ProductRef;
                var code = "SP-" + random.Next(20000, 29999);
                var isok = false;
                var dt = new PersianDate();
                var endd = values.EndDate1.Split(' ');
                var a = PersianDigits.PersianToEnglish(endd[0]);
                var dr = dt.shamsitomiladi(a);
                var z = PersianDigits.PersianToEnglish(endd[1].Split(':')[0]);
                var x = PersianDigits.PersianToEnglish(endd[1].Split(':')[1]);
                var v = PersianDigits.PersianToEnglish(endd[1].Split(':')[2]);
                var endtime = z + ':' + x + ':' + v;
                var b = dr.Split(' ');
                var end = b[0] + ' ' + endtime;
                var e = DateTime.Parse(end);

                var start = values.StartDate1.Split(' ');
                var d = PersianDigits.PersianToEnglish(start[0]);
                var dr1 = dt.shamsitomiladi(d);
                var z1 = PersianDigits.PersianToEnglish(start[1].Split(':')[0]);
                var x1 = PersianDigits.PersianToEnglish(start[1].Split(':')[1]);
                var v1 = PersianDigits.PersianToEnglish(start[1].Split(':')[2]);
                var starttime = z1 + ':' + x1 + ':' + v1;
                var c = dr1.Split(' ');
                var starts = c[0] + ' ' + starttime;
                var s = DateTime.Parse(starts);
                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }

                if (values.CategoriesRef == Guid.Empty)
                {
                    catRef = null;
                }
                if (values.ProductRef == Guid.Empty)
                {
                    catRef = null;
                }


                var query = db.Table_Discount.Add(new Table_Discount()
                {
                    Id = id,
                    Code = code,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Sort = values.Sort,
                    Version = 1,
                    IsDelete = false,
                    DiscountCode = values.DiscountCode,
                    Discount = values.Discount,
                    CategoriesRef = catRef,
                    ProductRef = ProductRef,
                    DiscountUser = values.DiscountUser,
                    DiscountUsed = values.DiscountUsed,
                    DiscountCount = values.DiscountCount,
                    DiscountFee = values.DiscountFee,
                    DiscountPercent = values.DiscountPercent,
                    DiscountQuantity = values.DiscountQuantity,
                    EndDate = e,
                    StartDate = s,
                    Entities = values.Entities,
                    IsMain = values.IsMain
                });
                db.Table_Discount.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static string ChangeDiscountStatus(Guid id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_Discount.FirstOrDefault(c => c.Id == id);
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";
        }


        public static string DeleteDiscount(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Discount.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Discount.Remove(query);
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
        public static string AllDelete(string Entities)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Discount.Where(c => c.Entities == "AllDiscount").ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        db.Table_Discount.Remove(item);
                        db.SaveChanges();

                    }
                    return "success";
                }


                return "success";
            }
            catch (Exception e)
            {

                return "Application Error : " + e.Message;

            }
        }

        public static VMDiscount.VmDiscountManagement EditDiscount(Guid Id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Discount.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var dt = new PersianDate();
                    var end = query.EndDate;
                    var start = query.StartDate;
                    var dr = dt.MiladiToShamsi(end);
                    var dr1 = dt.MiladiToShamsi(start);
                    var endd = dr + ' ' + string.Format("{0:HH:mm:ss}", query.EndDate.TimeOfDay.ToString());
                    var startd = dr1 + ' ' + string.Format("{0:HH:mm:ss}", query.StartDate.TimeOfDay.ToString());
                    var vm = new VMDiscount.VmDiscountManagement()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        Discount = query.Discount,
                        DiscountCode = query.DiscountCode,
                        Sort = query.Sort,
                        ProductRef = query.ProductRef,
                        CategoriesRef = query.CategoriesRef,
                        DiscountPercent = query.DiscountPercent,
                        DiscountCount = query.DiscountCount,
                        DiscountUser = query.DiscountUser,
                        DiscountFee = query.DiscountFee,
                        DiscountQuantity = query.DiscountQuantity,
                        Entities = query.Entities,
                        EndDate1 = endd,
                        StartDate1 = startd,
                    };
                    return vm;
                }


            }
            catch (Exception e)
            {

            }

            return new VMDiscount.VmDiscountManagement();
        }



        public static string RepositoryDiscountManagementEdit(VMDiscount.VmDiscountManagement values, Guid Userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = Userid;
                var query = db.Table_Discount.FirstOrDefault(c => c.Id == values.Id);

                if (query != null)
                {
                    var dt = new PersianDate();
                    var endd = values.EndDate1.Split(' ');
                    var a = PersianDigits.PersianToEnglish(endd[0]);
                    var dr = dt.shamsitomiladi(a);
                    var z = PersianDigits.PersianToEnglish(endd[1].Split(':')[0]);
                    var x = PersianDigits.PersianToEnglish(endd[1].Split(':')[1]);
                    var v = PersianDigits.PersianToEnglish(endd[1].Split(':')[2]);
                    var endtime = z + ':' + x + ':' + v;
                    var b = dr.Split(' ');
                    var end = b[0] + ' ' + endtime;
                    var e = DateTime.Parse(end);

                    var start = values.StartDate1.Split(' ');
                    var d = PersianDigits.PersianToEnglish(start[0]);
                    var dr1 = dt.shamsitomiladi(d);
                    var z1 = PersianDigits.PersianToEnglish(start[1].Split(':')[0]);
                    var x1 = PersianDigits.PersianToEnglish(start[1].Split(':')[1]);
                    var v1 = PersianDigits.PersianToEnglish(start[1].Split(':')[2]);
                    var starttime = z1 + ':' + x1 + ':' + v1;
                    var c = dr1.Split(' ');
                    var starts = c[0] + ' ' + starttime;
                    var s = DateTime.Parse(starts);

                    query.DiscountCode = values.DiscountCode;
                    query.Discount = values.Discount;
                    query.CategoriesRef = values.CategoriesRef;
                    query.ProductRef = values.ProductRef;
                    query.ModifireDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Sort = values.Sort;
                    query.DiscountFee = values.DiscountFee;
                    query.DiscountQuantity = values.DiscountQuantity;
                    query.DiscountUser = values.DiscountUser;
                    query.DiscountCount = values.DiscountCount;
                    query.DiscountPercent = values.DiscountPercent;
                    query.EndDate = e;
                    query.StartDate = s;
                    query.Entities = values.Entities;

                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static List<VMProduct.VmMainProductMangement> RepositoryProductsDropDown()
        {
            var list = new List<VMProduct.VmMainProductMangement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Product.ToList();
                if (query.Count > 0)
                {
                    foreach (var product in query)
                    {
                        var vm = new VMProduct.VmMainProductMangement
                        {
                            Id = product.Id,
                            Code = product.Code,
                            IsOk = product.IsOk,
                            PrimaryTitle = product.PrimaryTitle,
                            SecondaryTitle = product.SecondaryTitle,
                            TertiaryTitle = product.TertiaryTitle,
                        };

                        list.Add(vm);
                    }

                    var nulll = new VMProduct.VmMainProductMangement
                    {
                        Id = Guid.Empty,
                        PrimaryTitle = "--- بدون محصول ---",
                        Code = "1",
                    };
                    list.Add(nulll);
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }


        public static List<VMProduct.ViewModelCategoreis> RepositoryCategoryDropDown()
        {
            var list = new List<VMProduct.ViewModelCategoreis>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Product_Category
                    .ToList();

                if (query.Count > 0)
                {
                    foreach (var category in query)
                    {
                        var vm = new VMProduct.ViewModelCategoreis()
                        {
                            Id = category.Id,
                            PrimaryTitle = category.PrimaryTitle,
                            ParentRef = category.ParentRef,
                            IsMain = category.IsMain,
                            Code = category.Code,
                            IsDelete = category.IsDelete,
                            SecondaryTitle = category.SecondaryTitle,
                        };


                        list.Add(vm);
                    }


                    var nulll = new VMProduct.ViewModelCategoreis
                    {
                        Id = Guid.Empty,
                        PrimaryTitle = "--- بدون دسته بندی ---",
                        Code = "1",
                    };


                    list.Add(nulll);


                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static List<VMUser.VMUsers> RepositoryUserManagment()
        {
            var list = new List<VMUser.VMUsers>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.Where(c => !c.IsDelete).ToList();
                if (query.Count > 0)
                {

                    foreach (var user in query)
                    {

                        var vm = new VMUser.VMUsers()
                        {
                            Id = user.Id,
                            Code = user.Code,
                            IsOk = user.IsOk,
                            Name = user.Name,
                            Family = user.Family,
                            Phone = user.Phone,
                            IdentificationCode = user.Identification,
                            IsInBlacklist = user.IsInBlacklist,
                            CreatorDate = user.CreatorDate,
                            IsDelete = user.IsDelete,
                            RoleRef = user.RoleRef,
                            FullName = user.Name + " " + user.Family,
                        };

                        var queryRole =
                            db.Table_Role.FirstOrDefault(c => c.Id == user.RoleRef);
                        if (queryRole != null)
                        {
                            vm.RoleTitle = queryRole.PrimaryTitle;
                        }
                        list.Add(vm);
                    }
                    var nulll = new VMUser.VMUsers
                    {
                        Id = Guid.Empty,
                        FullName = "--- بدون کاربر ---",
                        Code = "1",
                    };

                    list.Add(nulll);
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }


        public static List<VMDiscount.VmDiscountManagement> RepositoryDiscountManagement()
        {
            var list = new List<VMDiscount.VmDiscountManagement>();
            try
            {
                var db = new Orders_Entities();
                var rows = 0;
                var query = db.Table_Discount.Where(c => c.Entities == "AllDiscount").AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var dt = new PersianDate();
                        var end = item.EndDate;
                        var start = item.StartDate;
                        var dr = dt.MiladiToShamsi(end);
                        var dr1 = dt.MiladiToShamsi(start);
                        var endd = dr + ' ' + string.Format("{0:HH:mm:ss}", item.EndDate.TimeOfDay.ToString());
                        var startd = dr1 + ' ' + string.Format("{0:HH:mm:ss}", item.StartDate.TimeOfDay.ToString());

                        var vm = new VMDiscount.VmDiscountManagement()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Discount = item.Discount,
                            DiscountFee = item.DiscountFee,
                            DiscountCode = item.DiscountCode,
                            ProductRef = item.ProductRef,
                            DiscountQuantity = item.DiscountQuantity,
                            StartDate1 = startd,
                            EndDate1 = endd,
                            Entities = item.Entities
                        };
                        var queryProduct = db.Table_Product.FirstOrDefault(c => c.Id == item.ProductRef);
                        if (queryProduct != null)
                        {
                            vm.ProductTitle = queryProduct.PrimaryTitle;
                        }
                        var queryUser = db.Table_User.FirstOrDefault(c => c.Id == item.DiscountUser);
                        if (queryUser != null)
                        {

                            vm.FullName = queryUser.Name + " " + queryUser.Family;
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
            return list;
        }


        public static List<VMDiscount.VmDiscountManagement> RepositoryDiscountEntities()
        {
            var list = new List<VMDiscount.VmDiscountManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Discount_Entities.Where(c => !c.IsDelete).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMDiscount.VmDiscountManagement()
                        {
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
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


        public static List<VMDiscount.VmDiscountManagement> RepositoryDiscountManagement(string search)
        {
            var list = new List<VMDiscount.VmDiscountManagement>();
            try
            {
                var rows = 0;
                var text = "";


                if (search != null)
                {
                    text = search;
                }
                var db = new Orders_Entities();
                var query = db.Table_Product.Where(c => c.PrimaryTitle.Contains(text)).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMDiscount.VmDiscountManagement();
                        var Discount = db.Table_Discount.FirstOrDefault(c => c.ProductRef == item.Id);
                        var dt = new PersianDate();
                        var end = Discount.EndDate;
                        var start = Discount.StartDate;
                        var dr = dt.MiladiToShamsi(end);
                        var dr1 = dt.MiladiToShamsi(start);
                        var endd = dr + ' ' + string.Format("{0:HH:mm:ss}", Discount.EndDate.TimeOfDay.ToString());
                        var startd = dr1 + ' ' + string.Format("{0:HH:mm:ss}", Discount.StartDate.TimeOfDay.ToString());
                        vm.PrimaryTitle = item.PrimaryTitle;
                        vm.Id = item.Id;

                        if (Discount != null)
                        {
                            rows++;
                            vm.Row = rows;
                            vm.Discount = Discount.Discount;
                            vm.DiscountCode = Discount.DiscountCode;
                            vm.ProductRef = Discount.ProductRef;
                            vm.EndDate1 = endd;
                            vm.StartDate1 = startd;
                            vm.DiscountQuantity = Discount.DiscountQuantity;
                            vm.IsOkTitle = "انتخاب شده";
                            vm.IsOkClass = "btn btn-labeled bg-green-active";

                        }
                        else
                        {
                            rows++;
                            vm.Row = rows;
                            vm.Discount = 0;
                            vm.DiscountCode = "";
                            vm.EndDate1 = endd;
                            vm.StartDate1 = startd;
                            vm.DiscountQuantity = Discount.DiscountQuantity;
                            vm.IsOkTitle = "انتخاب نشده";
                            vm.IsOkClass = "btn btn-labeled bg-red-active";
                            vm.ProductRef = item.Id;

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


        public static string Discount(Guid Id, Guid UserRef, int Discount, string DiscountCode)
        {
            try
            {
                var id = Guid.NewGuid();
                var userRef = UserRef;
                var random = new Random();
                var code = "SP-" + random.Next(20000, 29999);
                var db = new Orders_Entities();
                var query = db.Table_Discount.FirstOrDefault(c => c.ProductRef == Id);
                if (query != null)
                {
                    query.Discount = Discount;
                    query.DiscountCode = DiscountCode;
                    query.ModifierRef = userRef;
                    query.ModifireDate = DateTime.Now;
                    query.Version++;


                    db.SaveChanges();

                }
                else
                {
                    db.Table_Discount.Add(new Table_Discount
                    {
                        Code = code,
                        Discount = Discount,
                        DiscountCode = DiscountCode,
                        Id = id,
                        ProductRef = Id,
                        IsOk = true,
                        IsDelete = false,
                        CreatorRef = userRef,
                        CreatorDate = DateTime.Now,
                        ModifierRef = userRef,
                        ModifireDate = DateTime.Now,
                        Version = 1,
                    }
                    );


                    //db.Table_Discount.Add(query);
                    db.SaveChanges();
                }
                return "Success";
            }

            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }

        }



        public static List<VMDiscount.VmDiscountManagement> AllDiscount(int? Discount, int? DiscountFee, string DiscountCode, Guid DiscountUser, string startdate, string enddate, int discountquantity, Guid UserRef)
        {
            var list = new List<VMDiscount.VmDiscountManagement>();
            try
            {
                var db = new Orders_Entities();
                var dt = new PersianDate();
                var endd = enddate.Split(' ');
                var a = PersianDigits.PersianToEnglish(endd[0]);
                var dr = dt.shamsitomiladi(a);
                var z = PersianDigits.PersianToEnglish(endd[1].Split(':')[0]);
                var x = PersianDigits.PersianToEnglish(endd[1].Split(':')[1]);
                var v = PersianDigits.PersianToEnglish(endd[1].Split(':')[2]);
                var endtime = z + ':' + x + ':' + v;
                var b = dr.Split(' ');
                var end = b[0] + ' ' + endtime;
                var e = DateTime.Parse(end);

                var start = startdate.Split(' ');
                var d = PersianDigits.PersianToEnglish(start[0]);
                var dr1 = dt.shamsitomiladi(d);
                var z1 = PersianDigits.PersianToEnglish(start[1].Split(':')[0]);
                var x1 = PersianDigits.PersianToEnglish(start[1].Split(':')[1]);
                var v1 = PersianDigits.PersianToEnglish(start[1].Split(':')[2]);
                var starttime = z1 + ':' + x1 + ':' + v1;
                var c1 = dr1.Split(' ');
                var starts = c1[0] + ' ' + starttime;
                var s = DateTime.Parse(starts);
                var query = db.Table_Product.ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var queryDiscount = db.Table_Discount.FirstOrDefault(c => c.ProductRef == item.Id);

                        if (queryDiscount != null)
                        {
                            queryDiscount.Discount = Discount;
                            queryDiscount.DiscountFee = DiscountFee;
                            queryDiscount.DiscountCode = DiscountCode;
                            queryDiscount.EndDate = e;
                            queryDiscount.StartDate = s;
                            queryDiscount.DiscountQuantity = discountquantity;
                            queryDiscount.Entities = "AllDiscount";
                            queryDiscount.DiscountUser = DiscountUser;
                            db.SaveChanges();
                        }
                        else
                        {
                            var id = Guid.NewGuid();
                            var Users = UserRef;
                            var code = "SP-" + new Random().Next(20000, 29999).ToString();

                            var querydis = db.Table_Discount.Add(new Table_Discount()
                            {
                                Code = code,
                                Discount = Discount,
                                DiscountFee = DiscountFee,
                                DiscountCode = DiscountCode,
                                DiscountUser = DiscountUser,
                                Id = id,
                                ProductRef = item.Id,
                                IsOk = true,
                                IsDelete = false,
                                CreatorRef = Users,
                                CreatorDate = DateTime.Now,
                                ModifierRef = Users,
                                ModifireDate = DateTime.Now,
                                Version = 1,
                                DiscountQuantity = discountquantity,
                                Entities = "AllDiscount",
                                Sort = 1,
                                IsMain = true,
                                EndDate = e,
                                StartDate = s,
                            });

                            db.Table_Discount.Add(querydis);
                            db.SaveChanges();


                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        #endregion

        #region RepositoryCartsNoDiscount

        public static VMOrders.VmOrderCarts RepositoryCartsNoDiscount(List<VMOrders.VmOrderSubmit> carts, Guid _user)
        {
            var list = new VMOrders.VmOrderCarts();
            var db = new Orders_Entities();
            decimal _RowSum = 0;
            decimal _RowPrice = 0;
            decimal _FinalSum = 0;
            decimal _Transfer = 0;
            decimal _discount = 0;
            decimal _discountFree = 0;
            decimal _sumdis = 0;
            var _FinalOrder = new VMOrders.VmOrderCarts();
            var _FinalCarts = new List<VMOrders.VmOrderSubmit>();
            foreach (var itemCarts in carts)
            {
                _discount = 0;
                var vmCartRow = new VMOrders.VmOrderSubmit();
                var queryProduct = db.Table_Product.FirstOrDefault(c => c.Id == itemCarts.ProductId);
                if (queryProduct != null)
                {
                    vmCartRow.ProductTitle = queryProduct.PrimaryTitle;
                    vmCartRow.ProductId = queryProduct.Id;
                    vmCartRow.Code = queryProduct.Code;
                    vmCartRow.Discount = queryProduct.Discount;
                    var fileUpload = db.Table_File_Upload.FirstOrDefault(c => c.Ref == queryProduct.Id);
                    if (fileUpload != null)
                    {
                        vmCartRow.FileName = "/Static/Content/Images/Products/" + fileUpload.FileName +
                                             fileUpload.FileExtensions;
                    }
                    else
                    {
                        vmCartRow.FileName = "/Static/Content/Images/Products/" + "Default.png";
                    }

                    if (queryProduct.Discount > 0)
                    {
                        vmCartRow.Fee = itemCarts.Fee;
                        _discount = (queryProduct.Discount * itemCarts.Fee) / 100;
                        _discount = (_discount * itemCarts.Quantity);
                        _RowPrice = itemCarts.Quantity * itemCarts.Fee;
                        vmCartRow.RowDiscount = _discount;
                        //var diss = itemCarts.Fee - _discount;
                        vmCartRow.Price = _RowPrice - _discount;
                    }
                    else
                    {
                        vmCartRow.Fee = itemCarts.Fee;
                        _RowPrice = (itemCarts.Fee * itemCarts.Quantity);
                        vmCartRow.Price = _RowPrice;
                        vmCartRow.RowDiscount = 0;
                    }

                    var size = db.Table_Product_Size.FirstOrDefault(c => c.Id == itemCarts.SizeRef);
                    var color = db.Table_Product_Color.FirstOrDefault(c => c.Id == itemCarts.ColorRef);
                    if (size != null && color != null)
                    {
                        vmCartRow.SizeRef = itemCarts.SizeRef;
                        vmCartRow.ColorRef = itemCarts.ColorRef;
                        vmCartRow.ColorTitle = color.PrimaryTitle;
                        vmCartRow.SizeTitle = size.PrimaryTitle;
                    }
                    else
                    {
                        vmCartRow.SizeRef = Guid.Empty;
                        vmCartRow.ColorRef = Guid.Empty;
                        vmCartRow.ColorTitle = "بدون رنگ";
                        vmCartRow.SizeTitle = "بدون سایز";
                    }
                }

                vmCartRow.Quantity = itemCarts.Quantity;
                vmCartRow.SumDiscount = _discount;
                _RowSum += _RowPrice;
                _sumdis += _discount;
                //vmCartRow.Discount = (_discount * itemCarts.Quantity);
                _FinalCarts.Add(vmCartRow);
            }



            _FinalOrder.CartsItems = _FinalCarts;
            _FinalOrder.Discount = _sumdis;
            _FinalOrder.Sum = _RowSum;

            //var resultPay = Repfooter.RepTransferPay();
            //if (resultPay.Count > 0)
            //{
            //    foreach (var item in resultPay)
            //    {
            //        _Transfer = decimal.Parse(item.TarnsferPay ?? "0");
            //        _FinalOrder.Transfer = decimal.Parse(item.TarnsferPay ?? "0");
            //    }
            //}

            _FinalSum += (_RowSum + _Transfer) - _sumdis;
            _FinalOrder.DiscountFree = 0;
            if (_user != Guid.Empty)
            {
                var ordercheck = db.Table_Order.ToList().Exists(c => c.CreatorRef == _user && c.IsPay == true);
                if (!ordercheck)
                {
                    var disFree = db.Table_Discount.FirstOrDefault(c => c.Entities == "DiscountFree" && c.IsOk);
                    if (disFree != null)
                    {
                        var a = (disFree.Discount * _FinalSum) / 100;
                        _discountFree = _FinalSum - a ?? 0;
                        _FinalSum = _discountFree;
                        _FinalOrder.DiscountFree = a ?? 0;
                        _FinalOrder.DiscountPercent = disFree.Discount ?? 0;
                    }
                }
            }

            _FinalOrder.SumPay = _FinalSum;
            //_FinalOrder.sumdis = _sumdis;
            list = _FinalOrder;

            return list;
        }

        #endregion


        public static VMOrders.VmOrderCarts RepositoryCartsDiscount(List<VMOrders.VmOrderSubmit> carts, string code)
        {
            var list = new VMOrders.VmOrderCarts();
            var db = new Orders_Entities();
            decimal _RowSum = 0;
            decimal _RowPrice = 0;
            decimal _FinalSum = 0;
            decimal _Transfer = 0;
            decimal _discount = 0;
            decimal _discountFactor = 0;
            decimal _discountFacUser = 0;
            decimal _discountFree = 0;
            decimal _sumdis = 0;
            var _FinalOrder = new VMOrders.VmOrderCarts();
            var _FinalCarts = new List<VMOrders.VmOrderSubmit>();
            foreach (var itemCarts in carts)
            {
                decimal _discountCode = 0;
                _discount = 0;
                var vmCartRow = new VMOrders.VmOrderSubmit();
                var queryProduct = db.Table_Product.FirstOrDefault(c => c.Id == itemCarts.ProductId);
                if (queryProduct != null)
                {
                    vmCartRow.ProductTitle = queryProduct.PrimaryTitle;
                    vmCartRow.ProductId = queryProduct.Id;
                    vmCartRow.Code = queryProduct.Code;
                    vmCartRow.Discount = queryProduct.Discount;
                    var fileUpload = db.Table_File_Upload.FirstOrDefault(c => c.Ref == queryProduct.Id);
                    if (fileUpload != null)
                    {
                        vmCartRow.FileName = "/Static/Content/Images/Products/" + fileUpload.FileName +
                                             fileUpload.FileExtensions;
                    }
                    else
                    {
                        vmCartRow.FileName = "/Static/Content/Images/Products/" + "Default.png";
                    }

                    //if (queryProduct.Discount > 0)
                    //{
                    //    vmCartRow.Fee = itemCarts.Fee;
                    //    _discount = (queryProduct.Discount * itemCarts.Fee) / 100;
                    //    _discount = (_discount * itemCarts.Quantity);
                    //    _RowPrice = itemCarts.Quantity * itemCarts.Fee;
                    //    vmCartRow.RowDiscount = _discount;
                    //    //var diss = itemCarts.Fee - _discount;
                    //    vmCartRow.Price = _RowPrice - _discount;
                    //}
                    //else
                    //{
                    //    vmCartRow.Fee = itemCarts.Fee;
                    //    _RowPrice = (itemCarts.Fee * itemCarts.Quantity);
                    //    vmCartRow.Price = _RowPrice;
                    //    vmCartRow.RowDiscount = 0;
                    //}

                    vmCartRow.Fee = itemCarts.Fee;
                    _RowPrice = (itemCarts.Fee * itemCarts.Quantity);
                    vmCartRow.Price = _RowPrice;
                    vmCartRow.RowDiscount = 0;


                    var size = db.Table_Product_Size.FirstOrDefault(c => c.Id == itemCarts.SizeRef);
                    var color = db.Table_Product_Color.FirstOrDefault(c => c.Id == itemCarts.ColorRef);
                    if (size != null && color != null)
                    {
                        vmCartRow.ColorTitle = color.PrimaryTitle;
                        vmCartRow.SizeTitle = size.PrimaryTitle;
                    }
                    else
                    {
                        vmCartRow.ColorTitle = "بدون رنگ";
                        vmCartRow.SizeTitle = "بدون سایز";
                    }
                }


                var queryDiscount = db.Table_Discount.FirstOrDefault(c => c.DiscountCode == code && c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now && c.IsOk && c.ProductRef == queryProduct.Id);
                if (queryDiscount != null)
                {
                    #region DiscountPercent
                    if (queryDiscount.Entities == "DiscountPercent" && queryDiscount.ProductRef == queryProduct.Id && queryDiscount.DiscountQuantity > 0)
                    {
                        if (queryDiscount.CategoriesRef != null && itemCarts.CategoryRef != null)
                        {
                            if (queryDiscount.CategoriesRef == itemCarts.CategoryRef)
                            {
                                _discountCode = ((itemCarts.Fee * queryDiscount.Discount ?? 1) / 100) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }
                            else
                            {
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = false;
                                vmCartRow.Message = "Error";
                            }
                        }
                        else
                        {
                            _discountCode = ((itemCarts.Fee * queryDiscount.Discount ?? 1) / 100) * itemCarts.Quantity;
                            vmCartRow.DisCode = queryDiscount.DiscountCode;
                            vmCartRow.DisUse = true;
                            vmCartRow.Message = "Success";
                        }
                    }




                    #endregion

                    #region DiscountFee

                    if (queryDiscount.Entities == "DiscountFee" && queryDiscount.ProductRef == queryProduct.Id && queryDiscount.DiscountQuantity > 0)
                    {
                        if (queryDiscount.CategoriesRef != null && itemCarts.CategoryRef != null)
                        {
                            if (queryDiscount.CategoriesRef == itemCarts.CategoryRef)
                            {
                                _discountCode = (queryDiscount.DiscountFee ?? 1) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }
                            else
                            {
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = false;
                                vmCartRow.Message = "Error";
                            }
                        }
                        else
                        {
                            _discountCode = (queryDiscount.DiscountFee ?? 1) * itemCarts.Quantity;
                            vmCartRow.DisCode = queryDiscount.DiscountCode;
                            vmCartRow.DisUse = true;
                            vmCartRow.Message = "Success";
                        }
                    }


                    #endregion

                    #region DiscountUser

                    if (queryDiscount.Entities == "DiscountUser" && queryDiscount.ProductRef == queryProduct.Id && queryDiscount.DiscountQuantity > 0)
                    {
                        if (HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            var userid = Guid.Parse(HttpContext.Current.User.Identity.Name);
                            if ((queryDiscount.Discount != null && queryDiscount.Discount > 0) && queryDiscount.DiscountUser == userid)
                            {
                                _discountCode = ((itemCarts.Fee * queryDiscount.Discount ?? 1) / 100) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }
                            else if (queryDiscount.DiscountFee > 0 && queryDiscount.DiscountUser == userid)
                            {
                                _discountCode = (queryDiscount.DiscountFee ?? 1) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }
                            else
                            {
                                vmCartRow.Message = "Error DiscountUser";
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = false;
                            }

                        }
                        else
                        {
                            vmCartRow.Message = "Error UserNotLogin";
                            vmCartRow.DisCode = queryDiscount.DiscountCode;
                            vmCartRow.DisUse = false;
                        }

                    }



                    #endregion

                    #region DiscountStep

                    if (queryDiscount.Entities == "DiscountStep" && queryDiscount.ProductRef == queryProduct.Id && queryDiscount.DiscountQuantity > 0)
                    {
                        if (queryDiscount.DiscountPercent > 0 && queryDiscount.DiscountCount > 0)
                        {
                            _discountCode = ((itemCarts.Fee * queryDiscount.DiscountPercent ?? 1) / 100) * itemCarts.Quantity;
                            vmCartRow.DisCode = queryDiscount.DiscountCode;
                            vmCartRow.DisUse = true;
                            vmCartRow.Message = "Success";
                        }
                        else
                        {
                            vmCartRow.DisCode = queryDiscount.DiscountCode;
                            vmCartRow.DisUse = false;
                            vmCartRow.Message = "Error";
                        }
                    }



                    #endregion

                    //#region DiscountTransfer

                    //if (queryDiscount.Entities == "DiscountTransfer" && queryDiscount.ProductRef == queryProduct.Id && queryDiscount.DiscountQuantity > 0)
                    //{
                    //    if (queryDiscount.CategoriesRef != null && itemCarts.CategoryRef != null)
                    //    {
                    //        if (queryDiscount.CategoriesRef == itemCarts.CategoryRef)
                    //        {
                    //            _discountCode = (queryDiscount.DiscountFee ?? 1) * itemCarts.Quantity;
                    //            vmCartRow.DisCode = queryDiscount.DiscountCode;
                    //            vmCartRow.DisUse = true;
                    //            vmCartRow.Message = "Success";
                    //        }
                    //        else
                    //        {
                    //            vmCartRow.DisCode = queryDiscount.DiscountCode;
                    //            vmCartRow.DisUse = false;
                    //            vmCartRow.Message = "Error";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        _discountCode = (queryDiscount.DiscountFee ?? 0) * itemCarts.Quantity;
                    //        vmCartRow.DisCode = queryDiscount.DiscountCode;
                    //        vmCartRow.DisUse = true;
                    //        vmCartRow.Message = "Success";
                    //    }
                    //}
                    //#endregion

                    #region DiscountStepUsers

                    if (queryDiscount.Entities == "DiscountStepUsers" && queryDiscount.ProductRef == queryProduct.Id && queryDiscount.DiscountQuantity > 0)
                    {
                        if (HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            var userid = Guid.Parse(HttpContext.Current.User.Identity.Name);
                            if (queryDiscount.DiscountPercent > 0 && queryDiscount.DiscountUser == userid && queryDiscount.DiscountCount > 0)
                            {
                                _discountCode = ((itemCarts.Fee * queryDiscount.DiscountPercent ?? 1) / 100) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }
                            else
                            {
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = false;
                                vmCartRow.Message = "Error";
                            }

                        }
                        else
                        {
                            vmCartRow.Message = "Error UserNotLogin";
                        }

                    }



                    #endregion

                    #region AllDiscount


                    if (queryDiscount.Entities == "AllDiscount" && queryDiscount.ProductRef == queryProduct.Id && queryDiscount.DiscountQuantity > 0)
                    {
                        if (HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            var userid = Guid.Parse(HttpContext.Current.User.Identity.Name);
                            if ((queryDiscount.Discount != null && queryDiscount.Discount > 0) && queryDiscount.DiscountUser == userid)
                            {
                                _discountCode = ((itemCarts.Fee * queryDiscount.Discount ?? 1) / 100) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }

                            else if (queryDiscount.DiscountFee > 0 && queryDiscount.DiscountUser == userid)
                            {
                                _discountCode = (queryDiscount.DiscountFee ?? 1) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }
                        }
                        else
                        {
                            if (queryDiscount.Discount > 0)
                            {
                                _discountCode = ((itemCarts.Fee * queryDiscount.Discount ?? 1) / 100) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }

                            else if (queryDiscount.DiscountFee > 0)
                            {
                                _discountCode = (queryDiscount.DiscountFee ?? 1) * itemCarts.Quantity;
                                vmCartRow.DisCode = queryDiscount.DiscountCode;
                                vmCartRow.DisUse = true;
                                vmCartRow.Message = "Success";
                            }
                            else
                            {
                                _discountCode = 0;
                                vmCartRow.Message = "Error";
                            }
                        }

                    }


                    #endregion
                }
                else
                {
                    _discountCode = 0;
                    vmCartRow.Message = "Error";
                }

                vmCartRow.Quantity = itemCarts.Quantity;
                //vmCartRow.SumDiscount = _discount + _discountCode;
                vmCartRow.SumDiscount = _discountCode;
                _RowSum += _RowPrice;
                //_sumdis += _discount + _discountCode;
                _sumdis += _discountCode;
                //vmCartRow.Discount = (_discount * itemCarts.Quantity);
                _FinalCarts.Add(vmCartRow);
            }




            _FinalOrder.CartsItems = _FinalCarts;
            _FinalOrder.Discount = _sumdis;
            _FinalOrder.Sum = _RowSum;

            //var resultPay = Repfooter.RepInformationFooter();
            //if (resultPay.Count > 0)
            //{
            //    foreach (var item in resultPay)
            //    {
            //        _Transfer = decimal.Parse(item.TarnsferPay ?? "0");
            //        _FinalOrder.Transfer = decimal.Parse(item.TarnsferPay ?? "0");
            //    }
            //}

            _FinalSum += (_RowSum + _Transfer) - _sumdis;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var userid = Guid.Parse(HttpContext.Current.User.Identity.Name);
                _FinalOrder.DiscountFree = 0;
                if (userid != Guid.Empty)
                {
                    var disFactor = db.Table_Discount.FirstOrDefault(c => c.DiscountCode == code && c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now && c.IsOk);
                    if (disFactor != null)
                    {
                        if (disFactor.Entities == "DiscountFactorUser" && disFactor.DiscountQuantity > 0)
                        {
                            if ((disFactor.Discount != null && disFactor.Discount > 0) && disFactor.DiscountUser == userid)
                            {
                                var a = (disFactor.Discount * _FinalSum) / 100;
                                _discountFacUser = _FinalSum - a ?? 0;
                                _FinalSum = _discountFacUser;
                                _FinalOrder.Discount = a ?? 0;
                                _FinalOrder.DisCode = disFactor.DiscountCode;
                                _FinalOrder.DisUse = true;
                                _FinalOrder.Message = "Success";

                            }
                            else if ((disFactor.DiscountFee != null && disFactor.DiscountFee > 0) && disFactor.DiscountUser == userid)
                            {
                                var a = disFactor.DiscountFee;
                                _discountFacUser = _FinalSum - a ?? 0;
                                _FinalSum = _discountFacUser;
                                _FinalOrder.Discount = a ?? 0;
                                _FinalOrder.DisCode = disFactor.DiscountCode;
                                _FinalOrder.DisUse = true;
                                _FinalOrder.Message = "Success";
                            }
                            else
                            {
                                _FinalOrder.DisUse = false;
                                _FinalOrder.Message = "Error NoDiscountFactorUser";
                            }
                        }
                        else
                        {
                            _FinalOrder.DisUse = false;
                            _FinalOrder.Message = "Error NoEoQDiscountFactorUser";
                        }

                        if (disFactor.Entities == "DiscountFactor" && disFactor.DiscountQuantity > 0)
                        {
                            if ((disFactor.Discount > 0 || disFactor.Discount != null))
                            {
                                var a = (disFactor.Discount * _FinalSum) / 100;
                                _discountFactor = _FinalSum - a ?? 0;
                                _FinalSum = _discountFactor;
                                _FinalOrder.Discount = a ?? 0;
                                _FinalOrder.DisCode = disFactor.DiscountCode;
                                _FinalOrder.DisUse = true;
                                _FinalOrder.Message = "Success";
                            }
                            else if ((disFactor.DiscountFee > 0 || disFactor.DiscountFee != null))
                            {
                                var a = disFactor.DiscountFee;
                                _discountFactor = _FinalSum - a ?? 0;
                                _FinalSum = _discountFactor;
                                _FinalOrder.Discount = a ?? 0;
                                _FinalOrder.DisCode = disFactor.DiscountCode;
                                _FinalOrder.DisUse = true;
                                _FinalOrder.Message = "Success";
                            }
                            else
                            {
                                _FinalOrder.DisUse = false;
                                _FinalOrder.Message = "Error NoDisDiscountFactor";
                            }
                        }
                        else
                        {
                            _FinalOrder.DisUse = false;
                            _FinalOrder.Message = "Error NoEoQDiscountFactor";
                        }
                    }
                    else
                    {
                        _FinalOrder.Message = "Error";
                    }
                }
                else
                {
                    _FinalOrder.Message = "Error UserNotLogin";

                }
            }
            else
            {
                _FinalOrder.Message = "Error UserNotLogin";
            }

            _FinalOrder.SumPay = _FinalSum;
            //_FinalOrder.sumdis = _sumdis;
            list = _FinalOrder;
            //var vmall = new VMOrders.VmOrderCarts();
            //vmall.CartsItems = vm;
            //vmall.Discount = sumdispro;
            //vmall.Sum = summ;
            //var resultPay = Repfooter.RepInformationFooter();
            //if (resultPay.Count > 0)
            //{
            //    foreach (var item in resultPay)
            //    {
            //        transfer = decimal.Parse(item.TarnsferPay ?? "0");
            //        vmall.Transfer = decimal.Parse(item.TarnsferPay ?? "0");
            //    }
            //}

            ///vmall.sumdis = sumdispro;
            //sumpay = +(summ + transfer) - sumdispro;
            //vmall.SumPay = sumpay;

            //var queryDiscounts = db.Table_Discount.FirstOrDefault(c => c.DiscountCode == code && c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now && c.IsOk && c.Entities == "DiscountTransfer");
            //if (queryDiscounts != null)
            //{
            //    discount = queryDiscounts.DiscountFee ?? 0;
            //    vmall.Transfer = transfer - discount;
            //}
            //else
            //{

            //}

            //list = vmall;

            return list;
        }

    }
}
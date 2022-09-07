using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersSettings.ViewModels.Settings;

namespace OrdersSettings.Repository.Settings
{
    public class RepViewConfiguration
    {

        public static List<ViewModelsMainViewConfigurations> RepositoryConfigurationViewAdmin()
        {
            var list = new List<ViewModelsMainViewConfigurations>();
            var db = new Orders_Entities();
            var query = db.Table_View_Configuration.OrderBy(c => c.Sort)
                .AsNoTracking().ToList();
            if (query.Count > 0)
            {
                int row = 1;
                foreach (var item in query)
                {
                    var vm = new ViewModelsMainViewConfigurations
                    {
                        Row = row,
                        Code = item.Code,
                        Id = item.Id,
                        PrimaryTitle = item.PrimaryTitle,
                        SecondaryTitle = item.SecondaryTitle,
                        TertiaryTitle = item.TertiaryTitle,
                        View = item.View,
                        Sort = item.Sort,
                    };

                    if (item.IsMain)
                    {
                        vm.IsMainTitle = "اصلی";
                        vm.IsMainClass = "btn-success";
                    }
                    else
                    {
                        vm.IsMainTitle = "فرعی";
                        vm.IsMainClass = "btn-danger";
                    }


                    if (item.IsOk)
                    {
                        vm.IsOkClass = "btn-success ";
                        vm.IsOkTitle = "فعال";
                    }
                    else
                    {
                        vm.IsOkTitle = "غیر فعال";
                        vm.IsOkClass = "btn-danger ";
                    }


                    row++;

                    list.Add(vm);
                }
            }
            return list;
        }

        public static string Add(ViewModelsMainViewConfigurations values , Guid userRef)
        {
            try
            {
                var list = new List<ViewModelsMainViewConfigurations>();
                var db = new Orders_Entities();
                var id = Guid.NewGuid();
                var random = new Random();
                var code = "SP-" + random.Next(20000, 29999);
                var isok = false;

                switch (values.IsOk)
                {
                    case true:
                        {
                            isok = true;
                            break;
                        }
                }

                var query = db.Table_View_Configuration.Add(new Table_View_Configuration()
                {
                    Id = id,
                    Code = code,
                    IsOk = isok,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    Sort = values.Sort,
                    View = values.View,
                    TertiaryTitle = values.TertiaryTitle,
                    IsMain = true,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Version = 1,
                    IsDelete = false,
                });
                db.Table_View_Configuration.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static ViewModelsMainViewConfigurations Edit(Guid Id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_View_Configuration.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new ViewModelsMainViewConfigurations()
                    {
                        Code = query.Code,
                        Id = query.Id,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        TertiaryTitle = query.TertiaryTitle,
                        View = query.View,
                        Sort = query.Sort,

                    };
                    return vm;
                }


            }
            catch (Exception e)
            {

            }

            return new ViewModelsMainViewConfigurations();
        }

        public static string EditRow(ViewModelsMainViewConfigurations values, Guid userRef)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_View_Configuration.FirstOrDefault(c => c.Id == values.Id);

                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.TertiaryTitle = values.TertiaryTitle;
                    query.View = values.View;
                    query.Sort = values.Sort;
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;

                    var a = db.SaveChanges();
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
                var query = db.Table_View_Configuration.FirstOrDefault(c => c.Id == id);
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
                                db.Table_View_Configuration.Remove(query);
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

        public static string ChangeStatus(Guid id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_View_Configuration.FirstOrDefault(c => c.Id == id);
            var vm = new ViewModelsMainViewConfigurations();
            if (Result != null)
            {

                switch (Result.IsOk)
                {
                    case true:
                        {
                            Result.IsOk = false;
                            vm.IsOkTitle = "انتخاب نشده";
                            vm.IsOkClass = "btn btn-labeled bg-red-active";

                            break;
                        }
                    case false:
                        {
                            Result.IsOk = true;
                            vm.IsOkTitle = "انتخاب شده";
                            vm.IsOkClass = "btn btn-labeled bg-green-active";
                            break;
                        }
                }

            }

            db.SaveChanges();

            return "Sucsess";
        }

        public static string ChangeMain(Guid id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_View_Configuration.FirstOrDefault(c => c.Id == id);
            var vm = new ViewModelsMainViewConfigurations();
            if (Result != null)
            {

                switch (Result.IsMain)
                {
                    case true:
                        {
                            Result.IsMain = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsMain = true;
                            break;
                        }
                }

            }

            db.SaveChanges();

            return "Sucsess";
        }

        public static List<ViewModelsMainViewConfigurations> RepositoryConfigurationView(string str, string entity)
        {
            var list = new List<ViewModelsMainViewConfigurations>();
            var db = new Orders_Entities();
            var query = db.Table_View_Configuration.Where(c => c.IsOk && c.IsMain && !c.IsDelete && c.TertiaryTitle == str && c.SecondaryTitle == entity).OrderBy(c => c.Sort)
                .AsNoTracking().ToList();
            if (query.Count > 0)
            {
                foreach (var item in query)
                {
                    var vm = new ViewModelsMainViewConfigurations
                    {
                        SecondaryTitle = item.View,
                    };
                    list.Add(vm);
                }
            }
            return list;
        }
    }
}

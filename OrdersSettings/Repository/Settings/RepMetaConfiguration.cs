using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdersDatabase.Models;
using OrdersSettings.ViewModels.Settings;

namespace OrdersSettings.Repository.Settings
{
    public class RepMetaConfiguration
    {
        public static List<VMMetaConfiguration.ViewModelsMetaConfigurations> RepositoryMetaConfigurationAdmin()
        {
            var list = new List<VMMetaConfiguration.ViewModelsMetaConfigurations>();
            var db = new Orders_Entities();
            var query = db.Table_Meta_Configuration.OrderBy(c => c.Sort)
                .AsNoTracking().ToList();
            if (query.Count > 0)
            {
                int row = 1;
                foreach (var item in query)
                {
                    var vm = new VMMetaConfiguration.ViewModelsMetaConfigurations
                    {
                        Row = row,
                        Code = item.Code,
                        Id = item.Id,
                        PrimaryTitle = item.PrimaryTitle,
                        SecondaryTitle = item.SecondaryTitle,
                        TertiaryTitle = item.TertiaryTitle,
                        Note = item.Note,
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

        public static List<VMMetaConfiguration.ViewModelsMetaConfigurations> RepositoryMetaConfiguration()
        {
            var list = new List<VMMetaConfiguration.ViewModelsMetaConfigurations>();
            var db = new Orders_Entities();
            var query = db.Table_Meta_Configuration.Where(c =>c.IsOk).OrderBy(c=>c.Sort)
                .AsNoTracking().ToList();
            if (query.Count > 0)
            {
                
                foreach (var item in query)
                {
                    var vm = new VMMetaConfiguration.ViewModelsMetaConfigurations
                    {
                        PrimaryTitle = item.PrimaryTitle,
                        SecondaryTitle = item.SecondaryTitle,
                        TertiaryTitle = item.TertiaryTitle,
                        Note = item.Note,
                    };
                    list.Add(vm);
                }
            }
            return list;
        }

        public static string Add(VMMetaConfiguration.ViewModelsMetaConfigurations values, Guid userRef)
        {
            try
            {
                var list = new List<VMMetaConfiguration.ViewModelsMetaConfigurations>();
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

                var query = db.Table_Meta_Configuration.Add(new Table_Meta_Configuration()
                {
                    Id = id,
                    Code = code,
                    IsOk = isok,
                    PrimaryTitle = values.PrimaryTitle,
                    SecondaryTitle = values.SecondaryTitle,
                    Sort = values.Sort,
                    Note = values.Note,
                    TertiaryTitle = values.TertiaryTitle,
                    IsMain = true,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Version = 1,
                    IsDelete = false,
                });
                db.Table_Meta_Configuration.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static VMMetaConfiguration.ViewModelsMetaConfigurations Edit(Guid Id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Meta_Configuration.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMMetaConfiguration.ViewModelsMetaConfigurations()
                    {
                        Code = query.Code,
                        Id = query.Id,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        TertiaryTitle = query.TertiaryTitle,
                        Note = query.Note,
                        Sort = query.Sort,

                    };
                    return vm;
                }


            }
            catch (Exception e)
            {

            }

            return new VMMetaConfiguration.ViewModelsMetaConfigurations();
        }

        public static string EditRow(VMMetaConfiguration.ViewModelsMetaConfigurations values, Guid userRef)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Meta_Configuration.FirstOrDefault(c => c.Id == values.Id);

                if (query != null)
                {
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.TertiaryTitle = values.TertiaryTitle;
                    query.Note = values.Note;
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
                var query = db.Table_Meta_Configuration.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Meta_Configuration.Remove(query);
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
            var Result = db.Table_Meta_Configuration.FirstOrDefault(c => c.Id == id);
            var vm = new VMMetaConfiguration.ViewModelsMetaConfigurations();
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
            var Result = db.Table_Meta_Configuration.FirstOrDefault(c => c.Id == id);
            var vm = new VMMetaConfiguration.ViewModelsMetaConfigurations();
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
    }
}

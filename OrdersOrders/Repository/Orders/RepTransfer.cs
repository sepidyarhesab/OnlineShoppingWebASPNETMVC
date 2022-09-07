using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdersDatabase.Models;
using OrdersGeneral.ViewModels.General;
using OrdersOrders.ViewModels.Orders;

namespace OrdersGeneral.Repository.General
{
    public class RepTransfer
    {
        public static List<VMTransfer.VMTransferManagement> RepositoryMainTransferManagemenet()
        {
            var list = new List<VMTransfer.VMTransferManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Transfer.AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMTransfer.VMTransferManagement()
                        {
                            Code = item.Code,
                            Id = item.Id,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Fee = item.Fee,
                            TransferPay = item.TransferPay,
                            Weight = item.Weight,
                        };
                        var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == item.CityRef);
                        if (querycity != null)
                        {
                            vm.CityTitle = querycity.Title;
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

        public static string AddNew(VMTransfer.VMTransferManagement values, Guid userId)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userId;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(2000, 2999);

                var query = db.Table_Transfer.Add(new Table_Transfer()
                {
                    Id = id,
                    Code = code,
                    IsOk = false,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    CreatorDate = DateTime.Now,
                    ModifireDate = DateTime.Now,
                    Version = 1,
                    IsDelete = false,
                   CityRef = values.CityRef,
                   Fee = values.Fee,
                   Weight = values.Weight,
                   PrimaryTitle = values.PrimaryTitle,
                   SecondaryTitle = values.SecondaryTitle,
                   TransferPay = values.TransferPay,
                });
                db.Table_Transfer.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }


        public static string ChangeStatus(Guid id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_Transfer.FirstOrDefault(c => c.Id == id);
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


        public static string Delete(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Transfer.FirstOrDefault(c => c.Id == id);
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
                                db.Table_Transfer.Remove(query);
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


        public static VMTransfer.VMTransferManagement Edit(Guid Id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Transfer.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMTransfer.VMTransferManagement()
                    {
                        Id = query.Id,
                        Code = query.Code,
                        CityRef = query.CityRef,
                        Fee = query.Fee,
                        PrimaryTitle = query.PrimaryTitle,
                        SecondaryTitle = query.SecondaryTitle,
                        Weight = query.Weight,
                        TransferPay = query.TransferPay,
                    };
                    return vm;
                }


            }
            catch (Exception e)
            {

            }

            return new VMTransfer.VMTransferManagement();
        }



        public static string RepositoryManagementEdit(VMTransfer.VMTransferManagement values, Guid Userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = Userid;
                var query = db.Table_Transfer.FirstOrDefault(c => c.Id == values.Id);

                if (query != null)
                {
                    query.CityRef = values.CityRef;
                    query.Fee = values.Fee;
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SecondaryTitle = values.SecondaryTitle;
                    query.TransferPay = values.TransferPay;
                    query.Weight = values.Weight;
                    query.ModifierRef = Userid;
                    query.ModifireDate = DateTime.Now;
                    query.Version++;

                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }



        public static List<VMTransfer.VMTransferManagement> RepositoryMainTransfer()
        {
            var list = new List<VMTransfer.VMTransferManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Transfer.Where(c=>c.IsOk && !c.IsDelete).AsNoTracking().ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMTransfer.VMTransferManagement()
                        {
                            Code = item.Code,
                            Id = item.Id,
                            PrimaryTitle = item.PrimaryTitle,
                            SecondaryTitle = item.SecondaryTitle,
                            Fee = item.Fee,
                            TransferPay = item.TransferPay,
                            Weight = item.Weight,
                        };
                        var querycity = db.Table_Location.FirstOrDefault(c => c.LocationId == item.CityRef && c.Parent != 2);
                        if (querycity != null)
                        {
                            vm.CityTitle = querycity.Title;
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

        public static List<VMTransfer.VMTransferManagement> SelectionCity()
        {
            var list = new List<VMTransfer.VMTransferManagement>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Location.Where(c => c.Type == "2").ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        var vm = new VMTransfer.VMTransferManagement
                        {
                            CityRef = item.LocationId,
                            CityTitle = item.Title,
                        };
                        list.Add(vm);
                    }
                    var nulll = new VMTransfer.VMTransferManagement
                    {
                        Id = Guid.Empty,
                        CityTitle = "--- بدون دسته بندی ---",
                        Code = "1",
                    };
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

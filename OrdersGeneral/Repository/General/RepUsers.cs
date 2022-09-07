using SepidyarHesabExtensions.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersGeneral.ViewModels.General;
using Rozhano_WebApplication2.Extensions;
using Exception = System.Exception;

namespace OrdersGeneral.Repository.General
{
    public static class RepUsers
    {

        public static List<VMUser.VMRole> RepositoryUserDropDown()
        {
            var list = new List<VMUser.VMRole>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_Role.ToList();

                if (query.Count > 0)
                {
                    foreach (var role in query)
                    {
                        var vm = new VMUser.VMRole()
                        {
                            Id = role.Id,
                            PrimaryTitle = role.PrimaryTitle,
                            IsMain = role.IsMain,
                            Code = role.Code,
                            IsDelete = role.IsDelete,
                            SecondaryTitle = role.SecondaryTitle,
                        };
                        list.Add(vm);
                    }

                }


            }
            catch (Exception e)
            {

            }

            return list;
        }
        public static string RepositoryRegisterUser(string name, string family, string phone)
        {
            try
            {
                var db = new Orders_Entities();
                var id = Guid.NewGuid();
                var random = new Random();
                var code = random.Next(10000, 99999);
                var query = db.Table_User.Add(new Table_User
                {
                    Id = id,
                    Code = SepidyarHesabCodeGenerator.GenerateCode("General", "Table_User"),
                    Password = PasswordGenerator.PasswordGenerate(),
                    Phone = PersianDigits.PersianToEnglish(phone),
                    IsOk = true,
                    ModifierDate = DateTime.Now,
                    Age = 0,
                    ArchiveCode = "SP-" + random.Next(10000, 99999),
                    BirthDay = DateTime.Now,
                    Color = Guid.Empty,
                    CommissionPercent = 0,
                    CreatorDate = DateTime.Now,
                    CreatorRef = Guid.NewGuid(),
                    DiscountPercent = 0,
                    EconomicCode = "SP-" + random.Next(10000, 99999),
                    Family = family,
                    Favorite = 0,
                    Height = 0,
                    Identification = null,
                    IsInBlacklist = false,
                    IsMarried = false,
                    IsSpecial = false,
                    Jobs = Guid.Empty,
                    LanguageRef = Guid.Empty,
                    MarriedDay = DateTime.Now,
                    ModifierRef = Guid.Empty,
                    Name = name,
                    PersonalCode = "SP-" + random.Next(10000, 99999),
                    RegisterCode = "SP-" + random.Next(10000, 99999),
                    RoleRef = Guid.Parse("66718116-6427-4f72-9a19-5c5aacdf73c0"),
                    Sex = false,
                    Size = 1,
                    SizeRef = Guid.Empty,
                    Version = 1,
                    Weight = 1,

                });
                db.Table_User.Add(query);
                db.SaveChanges();
                var sms = new SmsProviders();
                sms.SendAuthentication(long.Parse(query.Phone), code.ToString());
                return id.ToString();
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        public static string RepositoryLoginUser(string username)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c =>
                    c.Phone == username);
                if (query != null)
                {
                    var random = new Random();
                    var code = random.Next(10000, 99999);
                    var sms = new SmsProviders();
                    sms.SendAuthentication(long.Parse(query.Phone), code.ToString());
                    return query.Id.ToString() + "&" + code;
                }
                else
                {
                    return "Application Error User Not Found";
                }

            }
            catch (Exception exception)
            {
                return "Application Error : " + exception.Message;
            }
        }

        #region UserAdmin

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
                        };

                        var queryRole =
                            db.Table_Role.FirstOrDefault(c => c.Id == user.RoleRef);
                        if (queryRole != null)
                        {
                            vm.RoleTitle = queryRole.PrimaryTitle;
                        }
                        list.Add(vm);
                    }
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static List<VMUser.VMUsers> RepositoryUserManagmentSearch(string searchnew)
        {
            var list = new List<VMUser.VMUsers>();
            try
            {
                var search = searchnew.Replace("	", "");
                var searcha = string.IsNullOrWhiteSpace(searchnew);
                var db = new Orders_Entities();
                var query = db.Table_User.OrderByDescending(c => c.CreatorDate).Where(c => c.Code == search || c.Phone.Contains(search) || c.Identification.Contains(search) || c.Name.Contains(search) || c.Family.Contains(search)).AsNoTracking().ToList();
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
                        };

                        var queryRole =
                            db.Table_Role.FirstOrDefault(c => c.Id == user.RoleRef);
                        if (queryRole != null)
                        {
                            vm.RoleTitle = queryRole.PrimaryTitle;
                        }
                        list.Add(vm);
                    }
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        public static string ChangeUserStatus(Guid Id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_User.FirstOrDefault(c => c.Id == Id);
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
        public static string ChangeUserBlackList(Guid Id)
        {
            var db = new Orders_Entities();
            var Result = db.Table_User.FirstOrDefault(c => c.Id == Id);
            if (Result != null)
            {

                switch (Result.IsInBlacklist)
                {
                    case true:
                        {
                            Result.IsInBlacklist = false;
                            break;
                        }
                    case false:
                        {
                            Result.IsInBlacklist = true;
                            break;
                        }
                }


            }

            db.SaveChanges();

            return "Sucsess";

        }

        public static string DeleteUser(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c => c.Id == id);
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
                                db.Table_User.Remove(query);
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
        public static string UnDeleteUser(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c => c.Id == id);
                if (query != null)
                {
                    switch (query.IsDelete)
                    {
                        case true:
                            {
                                query.IsDelete = false;
                                db.SaveChanges();
                                return "success";

                            }
                        case false:
                            {
                                return "true";

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

        public static string AddToDeleteUser(Guid id)
        {
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c => c.Id == id);
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
                                query.IsDelete = true;
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

        public static List<VMUser.VMUsers> RepositoryDeletedUserManagment()
        {
            var list = new List<VMUser.VMUsers>();
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.Where(c => (bool)c.IsDelete).ToList();
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
                            IsInBlacklist = user.IsInBlacklist,
                            CreatorDate = user.CreatorDate,
                            IsDelete = user.IsDelete,
                        };


                        list.Add(vm);
                    }
                }


            }
            catch (Exception e)
            {

            }

            return list;
        }

        #endregion

        #region Add

        public static string AddNewUser(VMUser.VMUsers values, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + random.Next(20000, 29999);
                var ecCode = "SP-" + random.Next(20000, 29999);
                var arCode = "SP-" + random.Next(20000, 29999);
                var isok = false;
                var roleRef = Guid.Parse("66718116-6427-4f72-9a19-5c5aacdf73c0");
                var nullref = Guid.Parse("00000000-0000-0000-0000-000000000000");
                switch (values.IsOk)
                {
                    case true:

                        {
                            isok = true;
                            break;
                        }
                }
                var query = db.Table_User.Add(new Table_User()
                {
                    Id = id,
                    CreatorDate = DateTime.Now,
                    ModifierDate = DateTime.Now,
                    Code = code,
                    Version = 1,
                    IsOk = isok,
                    CreatorRef = userRef,
                    ModifierRef = userRef,
                    IsDelete = false,
                    Name = values.Name,
                    Family = values.Family,
                    IsInBlacklist = false,
                    Phone = values.Phone,
                    Password = values.Password,
                    RoleRef = roleRef,
                    EconomicCode = ecCode,
                    ArchiveCode = arCode,
                    Sex = values.Sex,
                });
                db.Table_User.Add(query);
                db.SaveChanges();

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }

        #endregion

        #region Edit

        public static VMUser.VMUsers RepositoryUserMangmentByid(Guid Id)
        {
            
            try
            {
                var db = new Orders_Entities();
                var query = db.Table_User.FirstOrDefault(c => c.Id == Id);
                if (query != null)
                {
                    var vm = new VMUser.VMUsers
                    {
                        Id = query.Id,
                        Code = query.Code,
                        IsOk = query.IsOk,
                        CreatorDate = query.CreatorDate,
                        IsDelete = query.IsDelete,
                        Family = query.Family,
                        Name = query.Name,
                        Password = query.Password,
                        Phone = query.Phone,
                        Sex = query.Sex ?? false,
                        RoleRef = query.RoleRef,
                    };

                    return vm;
                }

            }
            catch (Exception e)
            {

            }
            return new VMUser.VMUsers();
        }
        public static string RepositoryUserMangmentEditById(VMUser.VMUsers values, Guid userid)
        {
            try
            {
                var db = new Orders_Entities();
                var userRef = userid;
                var query = db.Table_User.FirstOrDefault(c => c.Id == values.Id);


                if (query != null)
                {
                    query.ModifierDate = DateTime.Now;
                    query.ModifierRef = userRef;
                    query.Version++;
                    query.Name = values.Name;
                    query.Family = values.Family;
                    query.Phone = values.Phone;
                    query.Sex = values.Sex;
                    query.Password = values.Password;
                    query.RoleRef = values.RoleRef;
                    query.RoleRef = values.RoleRef;

                    db.SaveChanges();
                }

                return "Success";
            }
            catch (Exception e)
            {
                return "Application Error : " + e.Message;
            }
        }
        #endregion

        public static string GetRole(Guid id)
        {
            var db = new Orders_Entities();
            var roleuser = db.Table_User.FirstOrDefault(c => c.Id == id);
            if (roleuser != null)
            {
                if (roleuser.RoleRef == Guid.Parse("5bf16139-6715-4190-988a-4849c1a0241d"))
                {
                    return "Admin";
                }
                else
                {
                    return "Users";
                }
            }
            else
            {
                return "Users";
            }
        }
    }
}

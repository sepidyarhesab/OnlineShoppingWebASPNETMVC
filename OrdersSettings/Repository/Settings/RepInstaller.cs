using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using OrdersDatabase.Models;
using OrdersExtentions.Extensions;
using OrdersSettings.ViewModels.Settings;


namespace OrdersSettings.Repository.Settings
{
    public class RepInstaller
    {

        public static object RepositoryToken()
        {
            try
            {
                var db = new Orders_Entities();
                var settingquery = db.Table_Settings.AsNoTracking().FirstOrDefault();
                if (settingquery != null)
                {
                    if (Crypto.Decrypt(settingquery.Token) == "Success")
                    {
                        DateTime endtime = new DateTime(long.Parse(Crypto.Decrypt(settingquery.EndDate)));
                        var now = DateTime.Now;
                        if (endtime > now)
                        {
                            return true;
                        }
                        else
                        {
                            return "Application Error : End Time Expired";
                        }
                    }
                    else
                    {
                        return "Application Error : Token Failed";
                    }
                }
                else
                {
                    return "Application Error : Token Not Found";
                }

            }
            catch (Exception e)
            {
                return "Application Error : Exception : " + e.Message;

            }

            //try
            //{
            //    var db = new Orders_Entities();
            //    var settingquery = db.Table_Settings.AsNoTracking().FirstOrDefault();
            //    if (settingquery != null)
            //    {
            //        var token = settingquery.Token;
            //        var detoken = Crypto.Decrypt(token.ToString());
            //        if (token.ToString().Contains("Succeess"))
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch (Exception e)
            //{
            //    return false;
            //}

        }


        public static string Generate(VMSettings.VMSettingsManagmet values, Guid Userid)
        {
            try
            {

                var db = new Orders_Entities();
                var userRef = Userid;
                var random = new Random();
                var id = Guid.NewGuid();
                var code = "SP-" + DateTime.Now.Ticks;
                var isok = false;
                var exists = db.Table_Settings.FirstOrDefault();
                if (exists == null)
                {
                    var query = db.Table_Settings.Add(new Table_Settings());
                    query.Id = id;
                    query.Name = values.Name;
                    query.Family = values.Family;
                    query.NationalCode = values.NationalCode;
                    query.PrimaryTitle = values.PrimaryTitle;
                    query.SeconderyTitle = values.SecondaryTitle;
                    query.TertiaryTitle = values.PrimaryTitle;
                    query.Code = code;
                    query.Version = 1;
                    query.IsOk = isok;
                    query.CreatorRef = userRef;
                    query.ModifierRef = userRef;
                    query.CreatorDate = DateTime.Now;
                    query.ModifireDate = DateTime.Now;
                    query.Facebook = values.Facebook;
                    query.Aparat = values.Aparat;
                    query.Phone = values.Phone;
                    query.Instagram = values.Instagram;
                    query.Telegram = values.Telegram;
                    query.Whatsapp = values.Whatsapp;
                    query.Email = values.Email;
                    query.Twitter = values.Twitter;
                    query.Note = values.Note;
                    query.SiteTypeRef = Guid.Parse("aa7490c8-31b2-47a4-951b-2a52a4488738");
                    query.EndDate = Crypto.Encrypt(DateTime.Now.AddYears(1).Ticks.ToString());
                    query.StartDate = Crypto.Encrypt(DateTime.Now.Ticks.ToString());
                    query.Password = Crypto.ComputeSha256Hash(id.ToString());
                    query.Username = "SP-" + random.Next(10000, 99999);
                    query.Token = Crypto.Encrypt("ErrorLicense");

                    db.Table_Settings.Add(query);
                    db.SaveChanges();
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
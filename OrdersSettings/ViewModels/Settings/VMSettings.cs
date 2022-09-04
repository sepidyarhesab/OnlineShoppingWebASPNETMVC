using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersSettings.ViewModels.Settings
{
    public class VMSettings
    {
        public class VMSettingsManagmet
        {
            public Guid Id { get; set; }
            public Guid SiteType { get; set; }
            public string Code { get; set; }
            public int Sort { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string Url { get; set; }
            public string Name { get; set; }
            public string Family { get; set; }
            public string NationalCode { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Token { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public string Instagram { get; set; }
            public string Telegram { get; set; }
            public string Aparat { get; set; }
            public string Email { get; set; }
            public string Note { get; set; }
            public string Facebook { get; set; }
            public int Version { get; set; }
            public DateTime ModifierDate { get; set; }
            public DateTime CreatorDate { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid ModifierRef { get; set; }
            public string Phone { get; set; }
            public  string Whatsapp { get; set; }
            public string Twitter { get; set; }
            public string TertiaryTitle { get; set; }
            

        }
    }
}
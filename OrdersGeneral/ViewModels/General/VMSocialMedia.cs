using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VmSocialMedia
    {
        public class VmSocialMediaManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string Url { get; set; }
            public string Class { get; set; }
            public string Icon { get; set; }
            public string IconView { get; set; }
            public int Sort { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public Guid ModifierRef { get; set; }
            public DateTime ModifierDate { get; set; }
            public bool IsOk { get; set; }
            public bool IsDelete { get; set; }
        }
     
    }
}
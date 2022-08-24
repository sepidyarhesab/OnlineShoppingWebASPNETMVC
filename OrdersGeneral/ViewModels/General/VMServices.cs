using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VMServices
    {
        public class VMMainServices
        {
            public Guid Id { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecendryTitle { get; set; }
            public string FileName { get; set; }
            public int Sort { get; set; }
            public string Url { get; set; }
        }

        public class VMMainServicesManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public int Sort { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecendaryTitle { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public int IsOk { get; set; }
            public DateTime CreatorDateTime { get; set; }
        }

        public class VMType
        {
            public Guid Id { get; set; }
            public string Type { get; set; }
        }
    }
}
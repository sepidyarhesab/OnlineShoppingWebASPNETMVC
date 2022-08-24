using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VMLinks
    {
        public class VMMainLinks
        {
            public Guid Id { get; set; }
            public string Url { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
        }

        public class VMMainLinksManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string IsOkClass { get; set; }
            public string IsOkTitle { get; set; }
            public int Sort { get; set; }
            public string Url { get; set; }
            public int IsOk { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public DateTime CreatorDateTime { get; set; }
        }



        public class VMMainLinksManagementGenerate
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public int IsOk { get; set; }
            public int Sort { get; set; }
            public string Url { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
        }
    }
}
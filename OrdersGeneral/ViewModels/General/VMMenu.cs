using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VMMenu
    {
        public class VmMenuMainMenu
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondryTitle { get; set; }
            public string Icon { get; set; }
            public string IconMobile { get; set; }
            public bool IsOk { get; set; }
            public int Sort { get; set; }
            public string Url { get; set; }
        }

        public class VmMenuMainMenuManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public bool IsOk { get; set; }
            public string IsOkClass { get; set; }
            public string IsOkTitle { get; set; }
            public int Sort { get; set; }
            public string Url { get; set; }
            public string PrimaryTitle { get; set; }
            public string Icon { get; set; }
            public string IconMobile { get; set; }
            public string SecondryTitle { get; set; }
            public DateTime CreatorDateTime { get; set; }
        }
     



    }
}
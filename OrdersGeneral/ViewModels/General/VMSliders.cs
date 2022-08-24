using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace OrdersGeneral.ViewModels.General
{
    public class VMSliders
    {
        public class VmMainSliders
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public int? Sort { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecendaryTitle { get; set; }
            public string Url { get; set; }
            public string FileNameDesktop { get; set; }
            public string FileNameMobile { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public DateTime CreatorDateTime { get; set; }


        }

        public class VmMainSlidersGenerate
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public int? Sort { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecendaryTitle { get; set; }
            public string Url { get; set; }
            public string FileName { get; set; }
            public int IsOk { get; set; }
        }
    }
}
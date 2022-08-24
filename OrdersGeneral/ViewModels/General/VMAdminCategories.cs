using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VmAdminCategories
    {
        public class VmAdminCategoriesManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public int Sort { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public Guid ModifierRef { get; set; }
            public DateTime ModifierDate { get; set; }
            public bool IsOk { get; set; }
            public bool IsDelete { get; set; }
            public string IsOkClass { get; set; }
            public string IsOkTitle { get; set; }
        }
    }
}
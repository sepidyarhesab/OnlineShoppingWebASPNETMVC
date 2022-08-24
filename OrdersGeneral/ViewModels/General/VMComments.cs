using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VMComments
    {
        public class VMCommentsManagement
        {
            public Guid Id { get; set; }
            public Guid ProductRef { get; set; }
            public string ProductTitle { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Note { get; set; }
            public bool IsOk { get; set; }
            public int Rating { get; set; }
            public long Likes { get; set; }
            public long Dislikes { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public bool IsDelete { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid ModifierRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public DateTime ModifierDate { get; set; }
        }

        public class VMCommentsClient
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Note { get; set; }
            public bool IsOk { get; set; }
            public long Likes { get; set; }
            public long Dislikes { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }

        }
    }
}
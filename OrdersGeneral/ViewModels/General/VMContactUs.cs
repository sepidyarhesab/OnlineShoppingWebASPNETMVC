using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersGeneral.ViewModels.General
{
   public class VMContactUs
    {
        public class VmContactUsManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string Email { get; set; }
            public string Object { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Note { get; set; }
            public bool IsOk { get; set; }
            public string IsOkTitle { get; set; }
            public string IsOkClass { get; set; }
            public Guid CreatorRef { get; set; }
            public Guid ModifierRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public DateTime ModifierDate { get; set; }
            public int Sort { get; set; }
            public int Version { get; set; }
            public bool IsDelete { get; set; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdersGeneral.ViewModels.General
{
    public class VmMenuAdmin
    {
        public class VmMenuAdminClient
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string TertiaryTitle { get; set; }
            public Guid CategoryRef { get; set; }
            public int Sort { get; set; }
            public string Url { get; set; }
        }
        public class VmMenuAdminManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public string TertiaryTitle { get; set; }
            public Guid CategoryRef { get; set; }
            public int Sort { get; set; }
            public string Url { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
            public bool IsOk { get; set; }
            public bool IsDelete { get; set; }
            public string IsOkClass { get; set; }
            public string IsOkTitle { get; set; }
            public string FileName { get; set; }
        }
        public class VmMenuAccessClient
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public Guid MenuRef { get; set; }
            public Guid? UserRef { get; set; }
            public string FileName { get; set; }
            public string PrimaryTitle { get; set; }
            public string Url { get; set; }
            public Guid? RoleRef { get; set; }
            public int Sort { get; set; }
            public bool IsOk { get; set; }
        }
        public class VmMenuAccessManagement
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public Guid MenuRef { get; set; }
            public Guid UserRef { get; set; }
            public string FileName { get; set; }
            public Guid RoleRef { get; set; }
            public int Sort { get; set; }
            public Guid CreatorRef { get; set; }
            public DateTime CreatorDate { get; set; }
        }
    }
}
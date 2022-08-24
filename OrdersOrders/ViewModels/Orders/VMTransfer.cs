using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersGeneral.ViewModels.General
{
    public class VMTransfer
    {
        public class VMTransferManagement
        {
            public Guid Id { get; set; }
            public int? CityRef { get; set; } 
            public string CityTitle { get; set; }
            public string Code { get; set; }
            public string PrimaryTitle { get; set; }
            public string SecondaryTitle { get; set; }
            public decimal TransferPay { get; set; }
            public decimal? Weight { get; set; }
            public decimal? Fee { get; set; }
            public string IsOkClass { get; set; }
            public bool IsOk { get; set; }
            public string IsOkTitle { get; set; }
            public DateTime CreatorDateTime { get; set; }
        }
    }
}

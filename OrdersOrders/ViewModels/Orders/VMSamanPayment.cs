using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersOrders.ViewModels.Orders
{
    public class VMSamanPayment
    {
        public decimal Amount { get; set; }
        public string ResNum { get; set; }
        public string MID { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public string RedirectURL { get; set; }
        public string Action { get; set; }
        public long CellNumber { get; set; }
    }


    public class VMSamanPaymentResult
    {
        public bool IsOk { get; set; }
        public string RefNumber { get; set; }
        public string ResNumber { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public string Messages { get; set; }
        public string Class { get; set; }

    }
}

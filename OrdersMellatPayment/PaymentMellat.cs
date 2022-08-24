using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SepidyarHesabMellatPayment
{
    public static class OrdersMellatPayment
    {
        public static void PaymentRequest(string value)
        {
            var Url = "https://bpm.shaparak.ir/pgwchannel/startpay.mellat";
            var formId = "myForm1";
            var htmlForm = new StringBuilder();
            htmlForm.AppendLine("<html>");
            htmlForm.AppendLine(string.Format("<body onload='document.forms[\"{0}\"].submit()'>", formId));
            htmlForm.AppendLine(string.Format("<form id='{0}' method='POST' action='{1}'>", formId, Url));
            htmlForm.AppendLine("<input type='text' name='RefId' value='" + value + "' />");
            htmlForm.AppendLine("</form>");
            htmlForm.AppendLine("</body>");
            htmlForm.AppendLine("</html>");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(htmlForm.ToString());
            HttpContext.Current.Response.End();
        }
    }
}

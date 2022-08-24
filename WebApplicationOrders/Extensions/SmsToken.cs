
using SepidyarHesabExtensions.Classes;
using SmsIrRestful;
using WebApplicationOrders.Extensions;

namespace WebApplicationOrders.Extensions
{
    public  class SmsToken
    {
        public  string Token = new Token().GetToken(SmsAuthentication.GetUserApiKey, SmsAuthentication.GetSecretKey);
    }
}

using SepidyarHesabExtensions.Classes;
using SmsIrRestful;
using OrdersExtentions.Extensions;

namespace OrdersExtentions.Extensions
{
    public  class SmsToken
    {
        public  string Token = new Token().GetToken(SmsAuthentication.GetUserApiKey, SmsAuthentication.GetSecretKey);
    }
}
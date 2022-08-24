using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SepidyarHesabExtensions.Classes
{
    public static class LanguageConvertor
    {
        public static string ArabicConvertor(string str)
        {
            return str.Replace("ی", "ي").Replace("ک", "ك");
        }
        public static string PersianConvertor(string str)
        {
            return str.Replace("ي", "ی").Replace("ك", "ک");
        }

        public static string ChangeCharacter(string str)
        {
            return str.Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9").Replace("۰", "0");
        }
    }
}
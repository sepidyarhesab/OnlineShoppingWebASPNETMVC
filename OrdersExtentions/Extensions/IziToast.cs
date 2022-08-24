using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SepidyarHesabExtensions.Extentions
{
    public static class IziToast
    {
        public static string Error(string title, string message)
        {
            return "iziToast.error({ title: '" + title + "', message: '" + message + "',rtl: true,position: 'topLeft', });";
        }
        public static string Success(string title, string message)
        {
            return "iziToast.success({ title: '" + title + "', message: '" + message + "',rtl: true,position: 'topLeft', });";
        }
        public static string Warning(string title, string message)
        {
            return "iziToast.warning({ title: '" + title + "', message: '" + message + "',rtl: true,position: 'topLeft', });";
        }
        public static string Info(string title, string message)
        {
            return "iziToast.warning({ title: '" + title + "', message: '" + message + "',rtl: true,position: 'topLeft', });";
        }
    }
}
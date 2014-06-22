using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;

namespace Audomoticar.Library.Core
{
    public static class Credencials
    {
        public static IDictionary<string, string> Cuenta(string Usuario, string Token, string Mac, string Cam, string IdUser)
        {
            IDictionary<string, string> retorno = new Dictionary<string, string>();
            retorno.Add("User", Usuario);
            retorno.Add("Token", Token);
            retorno.Add("Mac", Mac);
            retorno.Add("Cam", Cam);
            retorno.Add("IdUser", IdUser);
            return retorno;
        }
    }
}
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
using Audomoticar.Library.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Audomoticar.Library.HTTP;
using Audomoticar.Library.Algoritmos;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace Audomoticar.Library.Core
{
    public class Login
    {
        private tblUsuario entity = new tblUsuario();
        RestClient Client = new RestClient(WebServicesInfo.Dir);
        public Login(string Usuario, string Clave)
        {
            entity.Usuario = Usuario;
            entity.Password = SHA1Util.SHA1HashStringForUTF8String(Clave);
        }

        public RespuestaLogin getResponse()
        {
            try
            {
                RespuestaLogin salida = new RespuestaLogin();
                var request = new RestRequest("api/Usuario/Login", Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddParameter("text/json", JsonConvert.SerializeObject(entity), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                var t = Client.Execute(request);
                if (string.IsNullOrWhiteSpace(t.Content) || t.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    salida = JsonConvert.DeserializeObject<RespuestaLogin>(t.Content);
                }
                else
                {
                    if (JsonConvert.DeserializeObject<RespuestaLogin>(t.Content).Respuesta.ToString().ToLower() == "true")
                    {
                        salida = JsonConvert.DeserializeObject<RespuestaLogin>(t.Content);
                    }
                }
                return salida;
            }
            catch(Exception ex)
            {
                return new RespuestaLogin() { Respuesta= false, Token="" };
            }
        }

        public string IdUser(string Name)
        {
            try
            {
                RespuestaLogin salida = new RespuestaLogin();
                var request = new RestRequest("api/Usuario/UserId", Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddParameter("text/json", JsonConvert.SerializeObject(entity), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                var t = Client.Execute(request);
                if (string.IsNullOrWhiteSpace(t.Content) || t.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    salida = JsonConvert.DeserializeObject<RespuestaLogin>(t.Content);
                }
                else
                {
                    if (JsonConvert.DeserializeObject<RespuestaLogin>(t.Content).Respuesta.ToString().ToLower() == "true")
                    {
                        salida = JsonConvert.DeserializeObject<RespuestaLogin>(t.Content);
                    }
                }
                return salida.Token;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static bool TIsValid(string token)
        {
            try
            {
                bool salida = true;
                if (token != "None")
                {
                    tblToken tok = new tblToken() { Token = token };
                    RestClient Client = new RestClient(WebServicesInfo.Dir);
                    var request = new RestRequest("api/Token/IsValid", Method.POST);
                    request.AddHeader("Content-Type", "application/json; charset=utf-8");
                    request.AddParameter("text/json", JsonConvert.SerializeObject(tok), ParameterType.RequestBody);
                    request.RequestFormat = DataFormat.Json;
                    var t = Client.Execute(request);
                    if (string.IsNullOrWhiteSpace(t.Content) || t.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        salida = false;
                    }
                    else
                    {
                        if (t.Content.ToLower() == "true")
                        {
                            salida = true;
                        }
                        else
                        {
                            salida = false;
                        }
                    }
                }
                else
                    salida = true;
                return salida;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
    }
}
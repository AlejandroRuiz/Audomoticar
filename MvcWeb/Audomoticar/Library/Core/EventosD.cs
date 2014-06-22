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
using SQLite;
using RestSharp;
using Audomoticar.Library.HTTP;
using Newtonsoft.Json;

namespace Audomoticar.Library.Core
{
    public class EventosD
    {
        
        public tblEventoL entity = new tblEventoL();
        SQLiteConnection db = new SQLiteConnection(DataBase.FileDir());
        RestClient Client = new RestClient(WebServicesInfo.Dir);
        public EventosD()
        {
            entity.Descripcion = string.Empty;
            entity.Evento = string.Empty;
            entity.Fecha = string.Empty;
            entity.fkIdUsuario = 0;
            entity.Hora = string.Empty;
            entity.IdEvento = 0;
            entity.IdEventoS = 0;
            entity.Sync = 0;
        }

        public bool UploadData()
        {
            try
            {
                tblEvento entityS = new tblEvento();
                entityS.Descripcion = entity.Descripcion;
                entityS.Evento = entity.Evento;
                entityS.Fecha = entity.Fecha;
                entityS.fkIdUsuario = entity.fkIdUsuario;
                entityS.Hora = entity.Hora;
                entityS.IdEvento = entity.IdEventoS;
                bool salida = false;
                var request = new RestRequest("api/Evento/PosttblEvento", Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.AddParameter("text/json", JsonConvert.SerializeObject(entityS), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                var t = Client.Execute(request);
                if (string.IsNullOrWhiteSpace(t.Content) || t.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    salida = false;
                }
                else
                {
                    if (t.Content.ToLower() == "true")
                    {
                        salida = true;
                    }
                }
                return salida;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public EventosD(int Id)
        {
            var n = db.Query<tblEventoL>("SELECT * FROM tblEventoL WHERE IdEvento = ?", Id).FirstOrDefault();
            if (n != null)
                entity = n;
            db.Close();
        }
        public bool Create()
        {
            try
            {
                db.Insert(entity);
                db.Close();
                return true;
            }
            catch (Exception)
            {
                db.Close();
                return false;
            }
        }
        public bool Update()
        {
            try
            {
                db.Update(entity);
                db.Close();
                return true;
            }
            catch (Exception)
            {
                db.Close();
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                db.Delete(entity);
                db.Close();
                return true;
            }
            catch (Exception)
            {
                db.Close();
                return false;
            }
        }
        public static IEnumerable<tblEventoL> arrayList()
        {
            SQLiteConnection db = new SQLiteConnection(DataBase.FileDir());
            IEnumerable<tblEventoL> salida = db.Query<tblEventoL>("select * from tblEventoL");
            db.Close();
            return salida;
        }
    }
}
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

namespace Audomoticar.Library.Core
{
    public class TokensD
    {
        public tblTokenL entity = new tblTokenL();
        SQLiteConnection db = new SQLiteConnection(DataBase.FileDir());
        public TokensD()
        {
            entity.Descripcion = string.Empty;
            entity.Estado = 0;
            entity.Fecha = string.Empty;
            entity.fkIdUsuario = 0;
            entity.Hora = string.Empty;
            entity.IdToken = 0;
            entity.IdTokenS = 0;
            entity.Ip = string.Empty;
            entity.Sync = 0;
            entity.Token = string.Empty;
        }
        public TokensD(int Id)
        {
            var n = db.Query<tblTokenL>("SELECT * FROM tblTokenL WHERE IdToken = ?", Id).FirstOrDefault();
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
        public static IEnumerable<tblTokenL> arrayList()
        {
            SQLiteConnection db = new SQLiteConnection(DataBase.FileDir());
            IEnumerable<tblTokenL> salida = db.Query<tblTokenL>("select * from tblTokenL");
            db.Close();
            return salida;
        }
    }
}
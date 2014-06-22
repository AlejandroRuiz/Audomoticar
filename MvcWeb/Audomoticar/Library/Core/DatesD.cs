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
    public static class DatesD
    {
        public static bool setDate()
        {
            SQLiteConnection db = new SQLiteConnection(DataBase.FileDir());
            try
            {
                DateL Entity = null;
                Entity = db.Query<DateL>("SELECT * FROM DateL").FirstOrDefault();
                if(Entity==null)
                {
                    Entity = new DateL();
                    Entity.Date = DateTime.Now.ToShortDateString();
                    Entity.Hour = DateTime.Now.ToShortTimeString();
                    db.Insert(Entity);
                }
                else
                {
                    Entity.Date = DateTime.Now.ToShortDateString();
                    Entity.Hour = DateTime.Now.ToShortTimeString();
                    db.Update(Entity);
                }
                db.Close();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                db.Close();
                return false;
            }
        }
    }
}
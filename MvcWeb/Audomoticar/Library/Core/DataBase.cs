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
using SQLite;
using Audomoticar.Library.Models;
using System.IO;

namespace Audomoticar.Library.Core
{
    public static class DataBase
    {
        public static void Create()
        {
            try
            {
                var conn = new SQLiteConnection(FileDir());
                conn.CreateTable<tblTokenL>();
                conn.CreateTable<tblEventoL>();
                conn.CreateTable<DateL>();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        public static void Delete()
        {
            try
            {
                var conn = FileDir();
                if (File.Exists(conn))
                    File.Delete(conn);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        public static string FileDir()
        {
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var conn = System.IO.Path.Combine(folder, "DB.db");
            return conn;
        }
    }
}
namespace Audomoticar.Library.Models
{
    using SQLite;
    using System;
    using System.Collections.Generic;

    public partial class tblToken
    {
        public int IdToken { get; set; }
        public string Token { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Ip { get; set; }
        public Nullable<int> fkIdUsuario { get; set; }
        public Nullable<int> Estado { get; set; }
        public string Descripcion { get; set; }
    }
    public partial class tblTokenL
    {
        [PrimaryKey, AutoIncrement]
        public int IdToken { get; set; }
        public int IdTokenS { get; set; }
        public string Token { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Ip { get; set; }
        public Nullable<int> fkIdUsuario { get; set; }
        public Nullable<int> Estado { get; set; }
        public string Descripcion { get; set; }
        public int Sync { get; set; }
    }
}

namespace Audomoticar.Library.Models
{
    using SQLite;
    using System;
    using System.Collections.Generic;

    public partial class tblUsuario
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Idfacebook { get; set; }
    }

    public partial class tblUsuarioL
    {
        [PrimaryKey, AutoIncrement]
        public int IdUsuario { get; set; }
        public int IdUsuarioS { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Idfacebook { get; set; }
        public int Sync { get; set; }
    }
}


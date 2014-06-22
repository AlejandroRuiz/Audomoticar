namespace Audomoticar.Library.Models
{
    using SQLite;
    using System;
    using System.Collections.Generic;

    public partial class tblEvento
    {
        public int IdEvento { get; set; }
        public string Evento { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> fkIdUsuario { get; set; }
    }
     public partial class tblEventoL
    {
        [PrimaryKey, AutoIncrement]
        public int IdEvento { get; set; }
        public int IdEventoS { get; set; }
        public string Evento { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> fkIdUsuario { get; set; }
        public int Sync { get; set; }
    }
}


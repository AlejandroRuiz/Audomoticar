using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWeb.Models
{
    public class RespuestaLogin
    {
        public bool Respuesta { get; set; }
        public string Token { get; set; }
    }
}
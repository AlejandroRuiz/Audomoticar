using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MvcWeb.Models;

namespace MvcWeb.Controllers
{
    public class EventoController : ApiController
    {
        private AutoEntities db = new AutoEntities();

        // GET api/Evento
        public IEnumerable<tblEvento> GettblEventoes()
        {
            return db.tblEventoes.AsEnumerable();
        }

        // GET api/GettblEvento/5
        public IEnumerable<tblEvento> GettblEvento(int id)
        {
            var t = from p in db.tblEventoes where p.fkIdUsuario == id select p;

            if (t == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return t;
        }

        // POST api/Evento/PosttblEvento
        public HttpResponseMessage PosttblEvento(tblEvento tblevento)
        {
            if (ModelState.IsValid)
            {
                db.tblEventoes.Add(tblevento);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse<bool>(HttpStatusCode.Created, true);
                return response;
            }
            else
            {
                return Request.CreateResponse<bool>(HttpStatusCode.BadRequest, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
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
    public class TokenController : ApiController
    {
        private AutoEntities db = new AutoEntities();

        // GET api/GettblToken/5
        public IEnumerable<tblToken> GettblToken(int id)
        {
            var t = from p in db.tblTokens where p.fkIdUsuario == id && p.Estado == 1 select p;

            if (t == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return t;
        }

        // POST api/Token/downToken
        public HttpResponseMessage downToken(tblToken tbltoken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = db.tblTokens.Find(tbltoken.IdToken);
                    if (user != null)
                    {
                        user.Estado = 0;
                            using (AutoEntities d = new AutoEntities())
                            {
                                d.Entry(user).State = EntityState.Modified;
                                d.SaveChanges();
                            }

                            return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
                    }
                    else
                    {
                        return Request.CreateResponse<bool>(HttpStatusCode.OK, false);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateResponse<bool>(HttpStatusCode.OK, false);
                }
            }
            else
            {
                return Request.CreateResponse<bool>(HttpStatusCode.OK, false);
            }
        }

        // POST api/Token/IsValid
        public HttpResponseMessage IsValid(tblToken tbltoken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tblToken t = db.tblTokens.FirstOrDefault(tok => tok.Token == tbltoken.Token);
                    if (t.Estado == 1)
                    {
                        tblUsuario u = db.tblUsuarios.FirstOrDefault(use => use.IdUsuario == t.fkIdUsuario);
                        if(u!=null)
                            return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
                        else
                            return Request.CreateResponse<bool>(HttpStatusCode.OK, false);
                    }
                    else
                    {
                        return Request.CreateResponse<bool>(HttpStatusCode.OK, false);
                    }
                }
                catch (Exception)
                {
                    return Request.CreateResponse<bool>(HttpStatusCode.OK, false);
                }
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
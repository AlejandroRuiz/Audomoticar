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
    public class UsuarioController : ApiController
    {
        private AutoEntities db = new AutoEntities();

        // GET api/Usuario/GettblUsuarios
        public IEnumerable<tblUsuario> GettblUsuarios()
        {
            return db.tblUsuarios.AsEnumerable();
        }

        // GET api/Usuario/GettblUsuario/5
        public tblUsuario GettblUsuario(int id)
        {
            tblUsuario tblusuario = db.tblUsuarios.Find(id);
            if (tblusuario == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblusuario;
        }

        //POST api/Usuario/UpdateUsuario
        public HttpResponseMessage UpdateUsuario(tblUsuario tblusuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = db.tblUsuarios.Find(tblusuario.IdUsuario);
                    if (user != null)
                    {
                        var claves = tblusuario.Password.Split('*');
                        if (claves[0] == user.Password)
                        {
                            tblusuario.Password = claves[1];

                            using(AutoEntities d = new AutoEntities())
		                    {
			                    d.Entry(tblusuario).State = EntityState.Modified; 
			                    d.SaveChanges();
		                    }

                            return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
                        }
                        else
                        {
                            return Request.CreateResponse<bool>(HttpStatusCode.OK, false);
                        }
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // POST api/Usuario/PosttblUsuario
        public HttpResponseMessage PosttblUsuario(tblUsuario tblusuario)
        {
            if (ModelState.IsValid)
            {
                tblUsuario myUser = db.tblUsuarios.SingleOrDefault(user => user.Usuario == tblusuario.Usuario);
                if (myUser == null)
                {
                    db.tblUsuarios.Add(tblusuario);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tblusuario);
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tblusuario);
                    return response;
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // POST api/Usuario/Login
        public HttpResponseMessage Login(tblUsuario tblusuario)
        {
            if (ModelState.IsValid)
            {
                string u = tblusuario.Usuario;
                tblUsuario myUser = db.tblUsuarios.SingleOrDefault(user => user.Usuario == u);
                if (myUser != null)
                {
                    if (myUser.Password == tblusuario.Password)
                    {
                        HttpRequestBase r = ((HttpContextBase)Request.Properties["MS_HttpContext"]).Request;
                        tblToken t = new tblToken();
                        t.Descripcion = "User-Agent:"+r.UserAgent+", OS:"+r.Browser.Platform;
                        t.Estado = 1;
                        t.Fecha = DateTime.Now.ToShortDateString();
                        t.fkIdUsuario = myUser.IdUsuario;
                        t.Hora = DateTime.Now.ToShortTimeString();
                        t.Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] ?? "No Identificada";
                        t.Token = SHA1Util.SHA1HashStringForUTF8String(t.fkIdUsuario+DateTime.Now.ToString());
                        db.tblTokens.Add(t);
                        db.SaveChanges();
                        return Request.CreateResponse<RespuestaLogin>(HttpStatusCode.OK, new RespuestaLogin() { Respuesta = true, Token = t.Token });
                    }
                    else
                    {
                        return Request.CreateResponse<RespuestaLogin>(HttpStatusCode.OK, new RespuestaLogin() { Respuesta= false, Token = "" });
                    }
                }
                else
                {
                    return Request.CreateResponse<RespuestaLogin>(HttpStatusCode.OK, new RespuestaLogin() { Respuesta = false, Token = "" });
                }
            }
            else
            {
                return Request.CreateResponse<RespuestaLogin>(HttpStatusCode.BadRequest, new RespuestaLogin() { Respuesta = false, Token = "" });
            }
        }

        // POST api/Usuario/UserId
        public HttpResponseMessage UserId(tblUsuario tblusuario)
        {
            if (ModelState.IsValid)
            {
                string u = tblusuario.Usuario;
                tblUsuario myUser = db.tblUsuarios.SingleOrDefault(user => user.Usuario == u);
                if (myUser != null)
                {
                    return Request.CreateResponse<RespuestaLogin>(HttpStatusCode.OK, new RespuestaLogin { Respuesta = true, Token = myUser.IdUsuario.ToString() });
                }
                else
                {
                    return Request.CreateResponse<RespuestaLogin>(HttpStatusCode.Forbidden, new RespuestaLogin() { Respuesta = false, Token = "" });
                }
            }
            else
            {
                return Request.CreateResponse<RespuestaLogin>(HttpStatusCode.Forbidden, new RespuestaLogin() { Respuesta = false, Token = "" });
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
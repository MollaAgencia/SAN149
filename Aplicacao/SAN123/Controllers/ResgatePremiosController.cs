using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aplicacao.Models.COMMON;

namespace Aplicacao.Controllers
{
    public class ResgatePremiosController : Controller
    {
        // GET: ResgatePremios
        public ActionResult Index()
        {
            if (values.PRP_UsuarioAutenticadoSite == null)
            {
                return RedirectToAction("Autenticacao", "Login");
            }

            return View();
        }
    }
}
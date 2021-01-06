using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aplicacao.Models.COMMON;

namespace Aplicacao.Controllers
{
    /****************************************************************************************
     *        Area destinada as paginas que contém unica e exclusivamente HTML             **
     ****************************************************************************************/
    public class ConteudoController : Controller
    {
        // GET: Conteudo
        public ActionResult Campanha()
        {
            if (values.PRP_UsuarioAutenticadoSite == null)
            {
                return RedirectToAction("Autenticacao", "Login");
            }

            return View();
        }
        public ActionResult Home()
        {
            //if(Session["USU_CPF"] == null)
            //{
            //    return RedirectToAction("Autenticacao", "Login");
            //}
            return View();
        }
        public ActionResult Novidades()
        {
            if (values.PRP_UsuarioAutenticadoSite == null)
            {
                return RedirectToAction("Autenticacao", "Login");
            }

            return View();
        }
        public ActionResult Destaques()
        {
            if (values.PRP_UsuarioAutenticadoSite == null)
            {
                return RedirectToAction("Autenticacao", "Login");
            }

            return View();
        }
    }
}
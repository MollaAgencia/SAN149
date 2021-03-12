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
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Aguarde()
        {
            return View();
        }
        public ActionResult Novidades()
        {
            return View();
        }
        public ActionResult Destaques()
        {
            return View();
        }
        public ActionResult ResgatePremios()
        {
            return View();
        }
    }
}
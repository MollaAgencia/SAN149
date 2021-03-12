using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Aplicacao.Models.COMMON;
using Aplicacao.Models.SITE.Desempenho;

namespace Aplicacao.Controllers
{
    public class DesempenhoController : Controller
    {
        private readonly DesempenhoService _desempenhoService = new DesempenhoService();
        // GET: Desempenho
        public ActionResult Desempenho()
        {
            return View();
        }

        //[HttpGet]
        //public JsonResult GetDesempenho(string documentoUsuario)
        //{
        //    return Json(_desempenhoService.GetDesempenho(documentoUsuario), JsonRequestBehavior.AllowGet);
        //}

    }
}
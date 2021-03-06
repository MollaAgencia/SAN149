﻿using System;
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
        public JsonResult MTD_Desempenho()
        {
            RetornoDesempenho ret = new DesempenhoService().MTD_RetornoDesempenho();
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}
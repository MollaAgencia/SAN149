using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aplicacao.Models.COMMON;
using Aplicacao.Models.SITE.Ranking;

namespace Aplicacao.Controllers
{
    public class RankingController : Controller
    {
        private readonly RankingService _rankingService = new RankingService();

        //RANKING DESCONTINUADO, SERÁ TUDO CENTRALIZADO NO DESEMPENHO

        // GET: Ranking
        //public ActionResult Ranking()
        //{
        //    return View();
        //}
        //public JsonResult GetRanking(string documentoUsuario)
        //{
        //    return Json(_rankingService.GetRanking(documentoUsuario), JsonRequestBehavior.AllowGet);
        //}
    }
}
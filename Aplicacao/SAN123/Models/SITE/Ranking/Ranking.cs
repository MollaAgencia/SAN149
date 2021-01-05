using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MollaLibrary.COMMON;

namespace Aplicacao.Models.SITE.Ranking
{
    public class RankingRetorno
    {
        public RankingRetorno()
        {
            Ranking = new List<Ranking>();
        }

        public List<Ranking> Ranking { get; set; }
        public RetornoRequisicao RetornoRequisicao { get; set; }
    }

    public class Ranking
    {
        public int Posicao { get; set; }
        public string Documento { get; set; }
        public string Nome { get; set; }
        public int Pontuacao { get; set; }
        public double CriterioDesempate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MollaLibrary.COMMON;

namespace Aplicacao.Models.SITE.Desempenho
{
    public class RetornoDesempenho
    {
        public RetornoDesempenho()
        {
            PRP_RetornoRequisicao = new RetornoRequisicao();
            OBJ_Desempenho = new DesempenhoHelper();
        }
        public DesempenhoHelper OBJ_Desempenho { get; set; }
        public RetornoRequisicao PRP_RetornoRequisicao { get; set; }
    }
    public class DesempenhoHelper
    {
        public string PRP_GrupoCompetidor { get; set; }
        public string PRP_CodSac { get; set; }
        public string PRP_Target { get; set; }
        public string PRP_Nome { get; set; }
        public double PRP_PayOut { get; set; }
        public long PRP_PontosPayOut { get; set; }
        public double PRP_Objetivo { get; set; }
        public double PRP_Realizado { get; set; }
        public double PRP_PontosExec { get; set; }
        public long PRP_Pontostotais { get; set; }
        public int PRP_PisicaoRanking { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MollaLibrary.COMMON;

namespace Aplicacao.Models.SITE.Desempenho
{
    public class DesempenhoRetorno
    {
        public DesempenhoRetorno()
        {
            Fases = new List<DesempenhoFase>();
        }

        public List<DesempenhoFase> Fases { get; set; }
        public RetornoRequisicao RetornoRequisicao { get; set; }
    }

    public class DesempenhoFase
    {
        public DesempenhoFase()
        {
            Desempenhos = new List<Desempenho>();
        }

        public string Fase { get; set; }
        public List<Desempenho> Desempenhos { get; set; }
    }

    public class Desempenho
    {
        public Desempenho()
        {
            DesempenhoDetalhes = new List<DesempenhoDetalhe>();
        }

        public string Kpi { get; set; }
        public List<DesempenhoDetalhe> DesempenhoDetalhes { get; set; }

    }

    public class DesempenhoDetalhe
    {
        public string Descricao { get; set; }
        public string Prefixo { get; set; }
        public string Sufixo { get; set; }
        public double Valor { get; set; }
    }
}
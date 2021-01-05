using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aplicacao.Models.COMMON
{
    /// <summary>
    /// Classe contém os dados basicos para preencher DropDownList, Select etc
    /// </summary>
    public class DropDownList
    {
        public string PRP_value { get; set; }
        public string PRP_Display { get; set; }
    }

    public class DropDownList_Ajax
    {
        public DropDownList_Ajax()
        {
            PRP_Requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            PRP_Lista = new List<DropDownList>();
        }

        public MollaLibrary.COMMON.RetornoRequisicao PRP_Requisicao { get; set; }
        public List<DropDownList> PRP_Lista { get; set; }
    }
}
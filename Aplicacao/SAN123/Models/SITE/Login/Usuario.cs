using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MollaLibrary.COMMON;

namespace Aplicacao.Models.SITE.Login
{
    public class Usuario
    {
        public string PRP_Documento { get; set; }
        public string PRP_Nome { get; set; }
        public string PRP_Email { get; set; }
        public string PRP_Telefone { get; set; }
        public RetornoRequisicao PRP_Requisicao { get; set; }
    }
}
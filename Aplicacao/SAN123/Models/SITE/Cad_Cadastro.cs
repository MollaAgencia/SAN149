using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aplicacao.Models.SITE
{
    public class Cad_Cadastro
    {
        public int id { get; set; }
        public string CPF { get; set; }
        public string CPFMASCK { get; set; }
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string DtNascimento { get; set; }
        public int MES { get; set; }
        public int ANO { get; set; }
        public string BU { get; set; }
        public string CELULAR { get; set; }
        public string SENHA { get; set; }
        public string FOTOPERFIL { get; set; }
        public bool Ativo { get; set; }
        public string PRP_MENSAGEM { get; set; }
        public bool PRP_STATUS { get; set; }
    }
}
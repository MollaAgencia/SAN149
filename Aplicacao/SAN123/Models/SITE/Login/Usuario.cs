using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MollaLibrary.COMMON;
using Aplicacao.Models.ENTITY;

namespace Aplicacao.Models.SITE.Login
{
    public class Usuario
    {
        public Usuario()
        {
            PRP_Requisicao = new RetornoRequisicao();
        }
        public DadosPreCadastro OBJ_Usuario { get; set; }
        public RetornoRequisicao PRP_Requisicao { get; set; }
    }
    public class DadosPreCadastro
    {
        public int PRP_IdUsuario { get; set; }
        public string PRP_NomeUsuario { get; set; }
        public string PRP_CPF { get; set; }
        public string PRP_EmailUsuario { get; set; }
        public string PRP_Celular { get; set; }
        public bool PRP_PrimeiroAcesso { get; set; }
        public int PRP_IdBU { get; set; }
        public string PRP_NomeBU { get; set; }
        public string PRP_Cod_Sac { get; set; }
    }
    public class DadosUsuarioSessao
    {
        public int PRP_IdUsuario { get; set; }
        public string PRP_NomeUsuario { get; set; }
        public string PRP_CPF { get; set; }
        public string PRP_Celular { get; set; }
        public int PRP_IdBU { get; set; }
        public string PRP_NomeBU { get; set; }
    }
}
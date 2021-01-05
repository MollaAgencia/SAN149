using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aplicacao.Models.SITE.Login
{
    /// <summary>
    /// Classe que contém as informações Necessárias para realizar o cadastro
    /// </summary>
    public class DadosUsuario
    {
        public string PRP_Nome { get; set; }
        public string PRP_Email { get; set; }        
        public string PRP_Telefone { get; set; }
        public string PRP_Senha { get; set; }

        public string  PRP_Documento { get; set; }

        public MollaLibraryAPI.COMMON.EnunsAPP.CRUD_Acao PRP_TipoAcao { get; set; }
    }

}
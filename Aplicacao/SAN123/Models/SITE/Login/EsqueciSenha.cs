using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aplicacao.Models.SITE.Login
{
    public class EsqueciSenha
    {
        /// <summary>
        /// Identificador do usuário para o esqueci a senha
        /// </summary>
        public long PRP_UsuarioID { get; set; }
        /// <summary>
        /// Identificador do PIT que o usuário esta participanto
        /// </summary>
        public long PRP_PitID { get; set; }
        public string PRP_DataSolicitacao { get; set; }
        public int PRP_HorasValidade { get; set; }
        public string PRP_Login { get; set; }
    }
}
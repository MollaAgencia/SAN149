using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MollaLibrary.ExtendMethods;

namespace MollaLibrary.ExtendMethods
{
    public static partial class ExtendMethods
    {

        /// <summary>
        /// Retorna o HTML para uma div já no esquema para ser exibido
        /// </summary>
        /// <param name="helper">Helper HTML</param>
        /// <param name="TipoMensagem">Tipo da mensagem que será exibida</param>
        /// <param name="TextoMensagem">Texto que sera exibido</param>
        /// <returns>string já no esquema para adicionar na tela</returns>
        public static System.Web.Mvc.MvcHtmlString MTD_Mensagem(this System.Web.Mvc.HtmlHelper helper, MollaLibrary.EnunsApp.enum_TipoMensagem TipoMensagem, string TextoMensagem)
        {
            return System.Web.Mvc.MvcHtmlString.Create(TextoMensagem.MTD_MensagemHTML(TipoMensagem));
        }

        public static void MTD_EnviaEmailTI(this Exception ex, string pMethod = "", Dictionary<string, string> pDadosComplementares = null)
        {
            Aplicacao.Models.COMMON.Email.LogError erro = new Aplicacao.Models.COMMON.Email.LogError(ex);

            System.Text.StringBuilder stb_mensagem = new System.Text.StringBuilder();
            stb_mensagem.AppendFormat(@"<p style=""margin-top:12px; margin-bottom:12px""><b>Método :</b> {0} ", pMethod);
            stb_mensagem.Append(@"<br />");
            //stb_mensagem.AppendFormat(@"Identificador Usuario Site  :{0}", Commons.Values.values.PRP_DadosUsuario == null ? "Sem dados do usuário na sessão": Commons.Values.values.PRP_DadosUsuario.ToStringExtention());
            //stb_mensagem.AppendFormat(@"Identificador Usuario ADMIN :{0}", Commons.Values.values.PRP_DadosUsuarioAdmin == null ? "Sem dados do usuário na sessão" : Commons.Values.values.PRP_DadosUsuarioAdmin.ToString());

            if (pDadosComplementares != null)
            {
                stb_mensagem.AppendLine(@"<p><span lang=""pt - br"" style=""padding - top:16px; padding - right:40px; padding - bottom:16px; padding - left:10px; color:#ff671f;font-size:14px""><b> Dados Complementares</b></span>");
                foreach (var item in pDadosComplementares)
                {
                    stb_mensagem.AppendLine(string.Format("<br /><b>{0}</b> : {1}", item.Key, item.Value));
                }
            }
            erro.PRP_Mensagem = stb_mensagem.ToString();
            erro.PRP_Descricao = "Erro na aplicação";
            erro.PRP_NomePagina = Aplicacao.Models.COMMON.values.PRP_NomePaginaAtual;

            new Aplicacao.Models.COMMON.Email.Email_Disparo().MTD_DisparoEmailExcecaoTI(erro);
        }

        public static Dictionary<string,string> MTD_ObterDadosPropriedade(this Aplicacao.Models.SITE.Login.DadosUsuario p)
        {
            Dictionary<string, string> dados = new Dictionary<string, string>();
            dados.Add("PRP_Documento", p.PRP_Documento);
            dados.Add("PRP_Email", p.PRP_Email);
            dados.Add("PRP_Nome", p.PRP_Nome);
            dados.Add("PRP_Senha", p.PRP_Senha);
            return dados;
        }
       
    }
}
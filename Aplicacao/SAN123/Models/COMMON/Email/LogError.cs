using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aplicacao.Models.COMMON.Email
{
    /// <summary>
    /// Classe que contém as informações sobre a exeção no sistema
    /// </summary>
    public class LogError
    {
        private Exception ex_execao = null;
        public LogError(Exception ex)
        {
            ex_execao = ex;
        }
        public LogError()
        {

        }

        /// <summary>
        /// Nome da pagina que houve a execao 
        /// </summary>
        public string PRP_NomePagina { get; set; }
        /// <summary>
        /// Descrição da excecao
        /// </summary>
        public string PRP_Descricao { get; set; }
        /// <summary>
        /// Mensagem para enviar no email
        /// </summary>
        public string PRP_Mensagem { get; set; }
        /// <summary>
        /// Retorna os dados de exceção já no esquema
        /// </summary>
        public string PRP_Excecao
        {
            get
            {
                System.Text.StringBuilder stb_error = new System.Text.StringBuilder();
                if (ex_execao != null)
                {
                    stb_error.Append(System.Environment.NewLine);
                    stb_error.Append(@"<br />");
                    stb_error.AppendFormat("Message: {0}", ex_execao.Message);
                    stb_error.Append(@"<br />");
                    stb_error.Append(System.Environment.NewLine);
                    stb_error.AppendFormat("Inner Exception: {0}", ex_execao.InnerException);
                    stb_error.Append(@"<br />");
                    stb_error.Append(System.Environment.NewLine);
                    stb_error.AppendFormat("Source: {0}", ex_execao.Source);
                    stb_error.Append(@"<br />");
                    stb_error.Append(System.Environment.NewLine);
                    stb_error.AppendFormat("StackTrace: {0}", ex_execao.StackTrace);
                    stb_error.Append(@"<br />");
                    stb_error.Append(System.Environment.NewLine);
                    stb_error.AppendFormat("TargetSite: {0}", ex_execao.TargetSite);
                    stb_error.Append(@"<br />");
                }
                else
                {
                    stb_error.Append(@"Não houve erro na aplicação");
                    stb_error.Append(System.Environment.NewLine);
                    stb_error.Append(@"<br />");
                }
                stb_error.Append(System.Environment.NewLine);
                stb_error.AppendFormat("Navegador: {0}:", values.PRP_DadosNavegador);
                return stb_error.ToString();
            }
        }
        ///// <summary>
        ///// Retorna o HTML já no esquema para enviar o email.
        ///// </summary>
        //public string PRP_EmailHTML
        //{
        //    get
        //    {


        //        return MetodosApp.Comuns.MetodosAplicacao.PRP_ObterHtmlTiMolla(
        //         pNomePagina: this.PRP_NomePagina
        //         , pMensagem: this.PRP_Mensagem
        //         , pExcecao: PRP_Excecao
        //         , pDescricao: PRP_Descricao
        //         );
        //    }
        //}
    }
}
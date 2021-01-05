using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aplicacao.Models.ENTITY;
using MollaLibrary.ExtendMethods;
using static Aplicacao.Models.COMMON.EnumAPP;

namespace Aplicacao.Models.COMMON
{
    /// <summary>
    /// Classe que contém as informações pertinentes a regra da campanha
    /// </summary>
    public class ComplementoDadosAcesso
    {
        public string PRP_Telefone { get; set; }
        public string PRP_Senha { get; set; }


        /// <summary>
        /// Método resposavel por preencher os dados complementares especificos de cada projeto
        /// </summary>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        public static ComplementoDadosAcesso MTD_ObterDadosComplemento(USU_Usuario pUsuario)
        {
            ComplementoDadosAcesso complemento = new ComplementoDadosAcesso();
            try
            {
                SAN123_Entities EF = new SAN123_Entities();

                var usuario = EF.USU_Usuario.FirstOrDefault(u => u.USU_ID == pUsuario.USU_ID);

                if (usuario != null)
                {
                    complemento.PRP_Telefone = usuario.USU_Telefone;
                    complemento.PRP_Senha = usuario.USU_Senha;
                }
            }
            catch (Exception ex)
            {

                Dictionary<string, string> Dados = new Dictionary<string, string>();
                Dados.Add("pUsuario", pUsuario.USU_Login);
                ex.MTD_EnviaEmailTI("Aplicacao.Models.COMMON.ComplementoDadosAcesso.MTD_ObterDadosComplemento", Dados);
                throw ex;
            }
            

            return complemento;
        }
        /// <summary>
        /// Método resposavel por realizar o cadastro das informações especidicas de cada projeto
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static MollaLibrary.COMMON.RetornoRequisicao MTD_CRUD(Models.SITE.Login.DadosUsuario p)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            try
            {
                requisicao.PRP_Status = true;
            }
            catch(Exception ex)
            {

                requisicao.PRP_Mensagem = "Falha ao realizar o cadastro";
                requisicao.PRP_Status = false;
                requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger; 


                Dictionary<string, string> Dados = new Dictionary<string, string>();

                ex.MTD_EnviaEmailTI("Aplicacao.Models.COMMON.ComplementoDadosAcesso.MTD_CRUD", Dados);
            }
            return requisicao;
        }
    }
}
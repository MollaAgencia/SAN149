using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Aplicacao.Models.COMMON;
using Aplicacao.Models.COMMON.API;
using Aplicacao.Models.ENTITY;
using MollaLibrary;
using MollaLibrary.ExtendMethods;
using MollaLibrary.Web;
using MollaLibraryAPI.COMMON;
using MollaLibraryAPI.COMMON.Pessoas;
using MollaLibraryAPI.PessoaFisica;
using static Aplicacao.Models.COMMON.EnumAPP;
using RetornoRequisicao = MollaLibrary.COMMON.RetornoRequisicao;

namespace Aplicacao.Models.SITE.Login
{
    public class LoginService
    {

        db_SAN149Entities EF = new db_SAN149Entities();
        public RetornoRequisicao MTD_Autenticacao(string pLogin, string pSenha, out bool pAcessoFake)
        {
            RetornoRequisicao objRetorno = new RetornoRequisicao();
            pAcessoFake = false;
            pLogin = pLogin.MTD_ApenasNumeros();
            USU_Usuario usuario = EF.USU_Usuario.FirstOrDefault(u => u.USU_Login.Equals(pLogin) && u.USU_Ativo);

            if (usuario != null && usuario.USU_Ativo)
            {
                if(usuario.USU_PrimeiroAcesso == false && usuario.USU_Senha == null)
                {
                    objRetorno.PRP_Status = true;
                    objRetorno.PRP_Mensagem = "Usuário deve atualizar o cadastro";
                    objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                }
                else if (usuario.USU_Senha == pSenha.MTD_CriptografiaIrreversivel())
                {
                    pAcessoFake = usuario.USU_AcessoFake;
                    var retornoSessao = MTD_AlimentarSessao(usuario);

                    if (retornoSessao.PRP_Status == true)
                    {
                        objRetorno.PRP_Status = retornoSessao.PRP_Status;
                        objRetorno.PRP_Mensagem = retornoSessao.PRP_Mensagem;
                        objRetorno.PRP_TipoMensagem = retornoSessao.PRP_TipoMensagem;
                    }
                    else
                    {
                        objRetorno.PRP_Status = retornoSessao.PRP_Status;
                        objRetorno.PRP_Mensagem = retornoSessao.PRP_Mensagem;
                        objRetorno.PRP_TipoMensagem = retornoSessao.PRP_TipoMensagem;
                    }
                }
                else
                {
                    objRetorno.PRP_Status = false;
                    objRetorno.PRP_Mensagem = "Senha incorreta".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Info);
                    objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                }
            }
            else
            {
                objRetorno.PRP_Status = false;
                objRetorno.PRP_Mensagem = "Usuário não encontrado ".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
            }
            return objRetorno;
        }

        private RetornoRequisicao MTD_AlimentarSessao(USU_Usuario objUsuario)
        {
            RetornoRequisicao retorno = new RetornoRequisicao();

            try
            {
                DadosUsuarioSessao objSessao = new DadosUsuarioSessao();
                objSessao.PRP_Celular = objUsuario.USU_Celular;
                objSessao.PRP_CPF = objUsuario.USU_CPF;
                objSessao.PRP_IdBU = objUsuario.UNG_ID;
                objSessao.PRP_IdUsuario = objUsuario.USU_ID;
                objSessao.PRP_NomeBU = objUsuario.UNG_UnidadeNegocio.UNG_Nome;
                objSessao.PRP_NomeUsuario = objUsuario.USU_Nome;

                SessionVar.Set("AutenticacaoSite", objSessao);
                retorno.PRP_Status = true;
                retorno.PRP_Mensagem = "Autenticaçao efetuada com sucesso.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Success);
                retorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;

            }
            catch (Exception)
            {
                retorno.PRP_Status = false;
                retorno.PRP_Mensagem = "Erro ao processar a sua requisição. Tente novamente mais tarde.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger);
                retorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
            }
            return retorno;
        }

        public RetornoRequisicao MTD_EsqueciSenhaEnvio(string pCpf)
        {
            string st_json = "";
            string key = "";
 
            Cad_Cadastro usu = new Cad_Cadastro();
            RetornoRequisicao requisicao = new RetornoRequisicao();
            pCpf = pCpf.MTD_ApenasNumeros();
            try
            {
                MollaLibrary.DataSource.MicrosoftSqlServer sqlServer = new MollaLibrary.DataSource.MicrosoftSqlServer(Models.COMMON.values.PRP_StringConexao);
                System.Data.SqlClient.SqlParameterCollection parameterCollection = sqlServer.InicializaSqlParameterCollection;
                parameterCollection.Add("@pCPF", System.Data.SqlDbType.VarChar).Value = pCpf.MTD_ApenasNumeros();
                System.Data.DataTable dtb_result = sqlServer.DbExecute("sp_site_EsqueceuSenha", parameterCollection, System.Data.CommandType.StoredProcedure);
                if (dtb_result != null)
                {
                    foreach (System.Data.DataRow linha in dtb_result.Rows)
                    {
                        //usu.id = int.Parse(linha["id"].ToString());
                        usu.NOME = linha["USU_NOME"].ToString();
                        usu.EMAIL = linha["USU_Email"].ToString();
                        usu.CPF = linha["USU_CPF"].ToString();
                    }

                    Models.COMMON.Email.Email_Disparo disparo = new Models.COMMON.Email.Email_Disparo();
                    key = string.Format("{0}", usu.CPF).MTD_CriptografiaReversivel(MollaLibrary.EnunsApp.en_Criptografia.Encriptar);
                    disparo.PRP_EmailDestinatario = usu.EMAIL;
                    requisicao.PRP_Status = true;
                    requisicao.PRP_Mensagem = string.Format(@"Olá, foi enviado um link para recadastramento de sua senha para o e-mail cadastrado {0}.", usu.EMAIL).MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Success);
                    disparo.MTD_DisparoEmailEsqueciSenha(usu.NOME, key);
                }
                else if (dtb_result == null)
                {
                    requisicao.PRP_Status = false;
                    requisicao.PRP_Mensagem = string.Format(@"Olá, algo inesperado ocorreu tente novamente em instantes.", usu.EMAIL).MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Alert);
                }
            }
            catch (Exception ex)
            {
                requisicao.PRP_Status = false;
                requisicao.PRP_Mensagem = string.Format(@"Olá, algo inesperado ocorreu entre em contato com o Fale Conosco.", usu.EMAIL).MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);
            }
            return requisicao;
        }

        public RetornoRequisicao MTD_Contato(Contato parametros)
        {
            COMMON.Email.Email_Disparo disparo = new COMMON.Email.Email_Disparo();
            RetornoRequisicao retorno = disparo.MTD_DisparoEmailContato(pNome: parametros.PRP_Nome, pAssunto: parametros.PRP_Assunto, pEmail: parametros.PRP_Email, pMensagem: parametros.PRP_Mensagem);
            return retorno;
        }


        public RetornoRequisicao MTD_AlterarSenha(string pLogin, string pSenha)
        {
            RetornoRequisicao requisicao = new RetornoRequisicao();
            try
            {
                using (db_SAN149Entities EF = new db_SAN149Entities())
                {
                    pLogin = pLogin.MTD_ApenasNumeros();
                    USU_Usuario USU = EF.USU_Usuario.FirstOrDefault(x => x.USU_Login.Equals(pLogin));
                    if (USU == null)
                    {
                        requisicao.PRP_Mensagem = "Usuário não localizado.";
                        requisicao.PRP_Status = false;
                        requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Info;
                    }
                    else if (!USU.USU_Ativo)
                    {
                        requisicao.PRP_Mensagem = "Usuário desativado da campanha, qualquer duvida entre em contato <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#ContatoModal' data-dismiss='modal'><u>clicando aqui</u></a>.";
                        requisicao.PRP_Status = false;
                        requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Info;
                    }
                    else if (!USU.USU_DataAceite.HasValue)
                    {
                        requisicao.PRP_Mensagem = "Para alterar a senha é necessário primeiramente realizar o cadastro";
                        requisicao.PRP_Status = false;
                        requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Info;
                    }
                    else
                    {
                        USU.USU_Senha = pSenha.MTD_CriptografiaIrreversivel();
                        EF.SaveChanges();
                        requisicao.PRP_Mensagem = "Senha alterada com sucesso";
                        requisicao.PRP_Status = true;
                        requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                requisicao.PRP_Mensagem = "Falha ao realizar a requisição.";
                requisicao.PRP_Status = false;
                requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
            }
            return requisicao;
        }
        public Usuario MTD_GetPreCadastro(string pCPF)
        {
            Usuario retornoUsuario = new Usuario();
            try
            {
                var objUsuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_CPF == pCPF);
                if(objUsuario != null)
                {
                    DadosPreCadastro dadosUsuario = new DadosPreCadastro();
                    dadosUsuario.PRP_IdBU = objUsuario.UNG_ID;
                    dadosUsuario.PRP_NomeBU = objUsuario.UNG_UnidadeNegocio.UNG_Nome;
                    dadosUsuario.PRP_Celular = objUsuario.USU_Celular;
                    dadosUsuario.PRP_Cod_Sac = objUsuario.USU_CodSac;
                    dadosUsuario.PRP_CPF = objUsuario.USU_CPF.MTD_FormataValores(EnunsApp.en_Formatacoes.CPF);
                    dadosUsuario.PRP_NomeUsuario = objUsuario.USU_Nome;
                    dadosUsuario.PRP_PrimeiroAcesso = objUsuario.USU_PrimeiroAcesso;
                    dadosUsuario.PRP_EmailUsuario = objUsuario.USU_Email;
                    dadosUsuario.PRP_IdUsuario = objUsuario.USU_ID;

                    retornoUsuario.OBJ_Usuario = dadosUsuario;

                    if (objUsuario.USU_PrimeiroAcesso == true && objUsuario.USU_DataAceite == null)
                    {                        
                        retornoUsuario.PRP_Requisicao.PRP_Status = true;
                        retornoUsuario.PRP_Requisicao.PRP_Mensagem = "Usuario localizado. Completar cadastro.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Success);
                        retornoUsuario.PRP_Requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                    }
                    else if (objUsuario.USU_PrimeiroAcesso == false && objUsuario.USU_DataAceite != null && objUsuario.USU_Senha == null)
                    {
                        retornoUsuario.PRP_Requisicao.PRP_Status = true;
                        retornoUsuario.PRP_Requisicao.PRP_Mensagem = "Usuário já cadastrado. Atualizar dados.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Info);
                        retornoUsuario.PRP_Requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Info;
                    }
                    else
                    {
                        retornoUsuario.PRP_Requisicao.PRP_Status = false;
                        retornoUsuario.PRP_Requisicao.PRP_Mensagem = "Usuario já cadastrado. Efetuar login.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                        retornoUsuario.PRP_Requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                    }                    
                }
                else
                {
                    retornoUsuario.PRP_Requisicao.PRP_Status = false;
                    retornoUsuario.PRP_Requisicao.PRP_Mensagem = "CPF não localizado em nossa base.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger);
                    retornoUsuario.PRP_Requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
                }

            }
            catch (Exception)
            {
                retornoUsuario.PRP_Requisicao.PRP_Status = false;
                retornoUsuario.PRP_Requisicao.PRP_Mensagem = "Erro ao processar a sua requisição. Tente novamente mais tarde.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger);
                retornoUsuario.PRP_Requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
            }
            return retornoUsuario;
        }
        public RetornoRequisicao MTD_RealizarCadastro(string pCelular, string pSenha, int pIdentificador)
        {
            RetornoRequisicao ret = new RetornoRequisicao();
            try
            {
                USU_Usuario objUsuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_ID == pIdentificador);
                if (objUsuario != null)
                {
                    objUsuario.USU_Celular = pCelular.MTD_ApenasNumeros();
                    objUsuario.USU_Senha = pSenha.MTD_CriptografiaIrreversivel();
                    objUsuario.USU_DataAceite = DateTime.Now.MTD_DataHoraBrasil();
                    objUsuario.USU_PrimeiroAcesso = false;                   
                    EF.SaveChanges();

                    var retornoSessao = MTD_AlimentarSessao(objUsuario);

                    ret.PRP_Status = true;
                    ret.PRP_Mensagem = "Dados salvos com sucesso!";
                    ret.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                }
                else
                {
                    ret.PRP_Status = false;
                    ret.PRP_Mensagem = "Usuario não localizado em nossa base.";
                    ret.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                }
            }
            catch (Exception)
            {
                ret.PRP_Status = false;
                ret.PRP_Mensagem = "Erro ao processar a sua requisição";
                ret.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
            }
            return ret;
        }
    }
}
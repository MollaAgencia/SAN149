using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Aplicacao.Models.ENTITY;
using Aplicacao.Models.SITE;
using Aplicacao.Models.SITE.Login;
using MollaLibrary.COMMON;
using MollaLibrary.ExtendMethods;
using MollaLibrary.Web;
using static Aplicacao.Models.COMMON.EnumAPP;

namespace Aplicacao.Models.COMMON.API
{
    public class PessoaFisica : IMetodosAPI
    {

        private MollaLibraryAPI.PessoaFisica.Metodos PessoaMetodos = new MollaLibraryAPI.PessoaFisica.Metodos(values.PRP_MollaAPI);
        SAN123_Entities db = new SAN123_Entities();

        public RetornoRequisicao MTD_AceiteRegulamento(string pLogin)
        {
            RetornoRequisicao requisicao = new RetornoRequisicao();
            var retorno = PessoaMetodos.MTD_AceiteRegulamento(pLogin, values.PRP_PIT);
            requisicao.PRP_Status = retorno.PRP_Status;
            requisicao.PRP_TipoMensagem = (MollaLibrary.EnunsApp.enum_TipoMensagem)((int)retorno.PRP_TipoMensagem);
            requisicao.PRP_Mensagem = retorno.PRP_Mensagem;
            return requisicao;
        }

        public RetornoRequisicao MTD_CRUD(Models.SITE.Login.DadosUsuario cadastrais)
        {

            MollaLibraryAPI.PessoaFisica.CRUD pessoa = new MollaLibraryAPI.PessoaFisica.CRUD();

            pessoa.PRP_Acao = cadastrais.PRP_TipoAcao;
            pessoa.PRP_Pessoa = new MollaLibraryAPI.PessoaFisica.CRUD_Pessoa();
            pessoa.PRP_Pessoa.PRP_UsuarioAtivo = true;
            pessoa.PRP_Pessoa.PRP_UsuarioCPF = cadastrais.PRP_Documento.MTD_ApenasNumeros();
            pessoa.PRP_Pessoa.PRP_UsuarioDataNascimento = "";
            pessoa.PRP_Pessoa.PRP_UsuarioEmail = cadastrais.PRP_Email;
            pessoa.PRP_Pessoa.PRP_UsuarioNome = cadastrais.PRP_Nome;
            pessoa.PRP_Pessoa.PRP_UsuarioSexo = "";
            
            pessoa.PRP_PIT = new MollaLibraryAPI.PessoaFisica.CRUD_PessoaPIT();
            pessoa.PRP_PIT.PRP_PIT = Models.COMMON.values.PRP_PIT;
            pessoa.PRP_PIT.PRP_PitAceiteRegulamento = false;
            pessoa.PRP_PIT.PRP_PitAlterarSenhaPrimeiroAcesso = false;
            pessoa.PRP_PIT.PRP_PitAtivo = false;
            pessoa.PRP_PIT.PRP_PitRealizarPreCadastro = false;
            pessoa.PRP_PIT.PRP_PitSenha = cadastrais.PRP_Senha;
            RetornoRequisicao requisicao = new RetornoRequisicao();
            try
            {
                requisicao.PRP_Status = true;
                if (values.PRP_UsuarioFake != null)
                {
                    requisicao.PRP_Status = false;
                    requisicao.PRP_Mensagem = "Não é possivel realizar essa ação em um acesso FAKE";
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                }
                else if (!pessoa.PRP_Pessoa.PRP_UsuarioCPF.MTD_Validacao(MollaLibrary.EnunsApp.en_Validacoes.CPF))
                {
                    requisicao.PRP_Status = false;
                    requisicao.PRP_Mensagem = "CPF inválido";
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                }
                else if (!pessoa.PRP_Pessoa.PRP_UsuarioEmail.MTD_Validacao(MollaLibrary.EnunsApp.en_Validacoes.Email))
                {
                    requisicao.PRP_Status = false;
                    requisicao.PRP_Mensagem = "E-mail inválido";
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                }
                else
                {

                    var retornoComplemento = ComplementoDadosAcesso.MTD_CRUD(cadastrais);
                    if (retornoComplemento.PRP_Status)
                    {

                        SAN123_Entities db = new SAN123_Entities();
                        
                        var usuCPF = cadastrais.PRP_Documento.MTD_ApenasNumeros();
                        var usuario = db.USU_Usuario.FirstOrDefault(x => x.USU_CPF.Equals(usuCPF));

                        usuario.USU_Nome = cadastrais.PRP_Nome;
                        usuario.USU_EMAIL = cadastrais.PRP_Email;
                        usuario.USU_Telefone = cadastrais.PRP_Telefone.MTD_ApenasNumeros();

                        db.USU_Usuario.AddOrUpdate(usuario);
                        db.SaveChanges();

                        var retorno = PessoaMetodos.MTD_CRUD(pessoa);

                        requisicao.PRP_Mensagem = retorno.PRP_Mensagem;
                        requisicao.PRP_Status = retorno.PRP_Status;
                        requisicao.PRP_TipoMensagem = (MollaLibrary.EnunsApp.enum_TipoMensagem)(int)requisicao.PRP_TipoMensagem;
                    }
                    else
                    {
                        requisicao = retornoComplemento;
                    }


                }
            }
            catch (Exception ex)
            {
                requisicao.PRP_Mensagem = "Falha ao realizar o aceite do regulamento";
                requisicao.PRP_Status = false;
                requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;

                ex.MTD_EnviaEmailTI("Aplicacao.Models.COMMON.API.MetodosAPI.MTD_AlterarPerfil");
            }
            return requisicao;
        }

        public RetornoRequisicao MTD_AlterarSenha(string pNovaSenha)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            try
            {
                if (values.PRP_UsuarioAutenticadoSite == null)
                {
                    requisicao.PRP_Status = false;
                    requisicao.PRP_Mensagem = $"Sua sessão expirou. Faça novamente o login novamente.";
                }
                else if (values.PRP_UsuarioAutenticadoSite.PRP_PedidoInternoTrabalho.PRP_AcessoFake.Value == true)
                {
                    requisicao.PRP_Status = false;
                    requisicao.PRP_Mensagem = "Não é possivel realizar essa ação em um acesso FAKE";
                }
                else
                {
                    string pLogin = values.PRP_UsuarioAutenticadoSite.PRP_DadosCadastrais.PRP_CPF;

                    var retorno = PessoaMetodos.MTD_AlterarSenha(pLogin.MTD_ApenasNumeros(), values.PRP_PIT, pNovaSenha);

                    requisicao.PRP_Status = retorno.PRP_Status;
                    requisicao.PRP_TipoMensagem = (MollaLibrary.EnunsApp.enum_TipoMensagem)(int)retorno.PRP_TipoMensagem;
                    requisicao.PRP_Mensagem = retorno.PRP_Mensagem.MTD_MensagemHTML(requisicao.PRP_TipoMensagem);
                }
            }
            catch (Exception ex)
            {
                requisicao.PRP_Mensagem = $"Falha ao realizar a requisição";
                requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;
                requisicao.PRP_Status = false;
                Dictionary<string, string> Dados = new Dictionary<string, string>();
                Dados.Add("pNovaSenha", pNovaSenha);
                ex.MTD_EnviaEmailTI("Aplicacao.Models.SITE.Login.Metodos.MTD_AlterarSenha", Dados);
            }
            return requisicao;
        }

        public RetornoRequisicao MTD_Autenticacao(string pLogin, string pSenha, out bool pAcessoFake)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new RetornoRequisicao();
            pAcessoFake = false;
            try
            {

                if (string.IsNullOrWhiteSpace(pLogin))
                {
                    requisicao.PRP_Mensagem = "Login é um campo obrigatório".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);
                    requisicao.PRP_Status = false;
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                }
                else if (string.IsNullOrWhiteSpace(pSenha))
                {
                    requisicao.PRP_Mensagem = "Senha é um campo obrigatório".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);
                    requisicao.PRP_Status = false;
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                }
                else
                {
                    MollaLibraryAPI.COMMON.Pessoas.AutenticacaoParametros parametros = new MollaLibraryAPI.COMMON.Pessoas.AutenticacaoParametros();
                    parametros.PRP_Ambiente = values.PRP_Ambiente;
                    parametros.PRP_DadosNavegador = values.PRP_DadosNavegador;
                    parametros.PRP_IpAcesso = values.PRP_IpAcesso;
                    parametros.Senha = pSenha;
                    parametros.Login = pLogin.MTD_ApenasNumeros();
                    parametros.PIT = values.PRP_PIT;
                    MollaLibraryAPI.PessoaFisica.PessoaRetorno retorno = PessoaMetodos.MTD_Autenticacao(parametros);

                    requisicao.PRP_Status = retorno.PRP_Requisicao.PRP_Status;
                    requisicao.PRP_TipoMensagem = (MollaLibrary.EnunsApp.enum_TipoMensagem)((int)retorno.PRP_Requisicao.PRP_TipoMensagem);
                    requisicao.PRP_Mensagem = retorno.PRP_Requisicao.PRP_Mensagem.MTD_MensagemHTML(requisicao.PRP_TipoMensagem);
                    if (requisicao.PRP_Status && retorno.PRP_Dados.PRP_PedidoInternoTrabalho.PRP_AcessoFake.Value == false)
                    {
                        ComplementoDadosAcesso dadosComplemetares = new ComplementoDadosAcesso();

                        var usuario = db.USU_Usuario.FirstOrDefault(x => x.USU_CPF.Equals(retorno.PRP_Dados.PRP_DadosCadastrais.PRP_CPF));

                        //dadosComplemetares.Cargo = (Cargo)usuario.USU_CAR_ID;
                        //dadosComplemetares.Time = (Time)usuario.USU_TIM_ID;

                        SessionVar.Set("ComplementoDados", dadosComplemetares);
                        SessionVar.Set("AutenticacaoSite", retorno.PRP_Dados);
                        SessionVar.Set("Usuario", usuario);

                    }
                    else
                    {
                        pAcessoFake = true;
                        MollaLibrary.Web.SessionVar.Set("AutenticacaoFake", retorno.PRP_Dados);
                    }
                }

            }
            catch (Exception ex)
            {
                requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
                requisicao.PRP_Status = false;
                requisicao.PRP_Mensagem = "Falha ao realizar a autenticação".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);

                Dictionary<string, string> Dados = new Dictionary<string, string>();
                Dados.Add("pLogin", pLogin);
                Dados.Add("pSenha", pSenha);
                ex.MTD_EnviaEmailTI("Aplicacao.Models.COMMON.API.PessoaFisica.MTD_Autenticacao", Dados);
            }
            return requisicao;
        }


        /// <summary>
        /// Lista todos os usuários cadastrados na base
        /// </summary>
        /// <param name="pPIT"></param>
        /// <returns></returns>
        public dynamic MTD_ListarUsuarios(string pPIT)
        {
            dynamic obj_retorno = new System.Dynamic.ExpandoObject();
            try
            {
                var dados = PessoaMetodos.MTD_ListaUsuarios(values.PRP_PIT);
                
                obj_retorno = dados;
            }
            catch (Exception ex)
            {
                MollaLibraryAPI.COMMON.RetornoRequisicao requisicao = new MollaLibraryAPI.COMMON.RetornoRequisicao();
                requisicao.PRP_Mensagem = "Falha oao listar os usuários";
                requisicao.PRP_RetornoTipo = MollaLibraryAPI.COMMON.EnunsAPP.TiposRetorno.Requisicao_Exception;
                requisicao.PRP_TipoMensagem = MollaLibraryAPI.COMMON.EnunsAPP.enum_TipoMensagem.Danger;
                requisicao.PRP_Status = false;
                obj_retorno.PRP_Requisicao = requisicao;
            }

            return obj_retorno;
        }
        /// <summary>
        /// Método para obter as informações de autenticação do usuário
        /// </summary>
        /// <param name="pLogin"></param>
        /// <returns>MollaLibraryAPI.PessoaFisica.PessoaRetorno</returns>
        public dynamic MTD_ObterUsuario(string pLogin)
        {
            dynamic obj_retorno = new System.Dynamic.ExpandoObject();
            try
            {
                MollaLibraryAPI.PessoaFisica.PessoaRetorno dados = PessoaMetodos.MTD_ObterPessoa(pLogin,values.PRP_PIT);
                obj_retorno = dados;
            }
            catch (Exception ex)
            {
                MollaLibraryAPI.COMMON.RetornoRequisicao requisicao = new MollaLibraryAPI.COMMON.RetornoRequisicao();
                MollaLibraryAPI.PessoaFisica.PessoaRetorno dados = new MollaLibraryAPI.PessoaFisica.PessoaRetorno();
                dados.PRP_Requisicao.PRP_Mensagem = "Falha oao listar os usuários";
                dados.PRP_Requisicao.PRP_RetornoTipo = MollaLibraryAPI.COMMON.EnunsAPP.TiposRetorno.Requisicao_Exception;
                dados.PRP_Requisicao.PRP_TipoMensagem = MollaLibraryAPI.COMMON.EnunsAPP.enum_TipoMensagem.Danger;
                dados.PRP_Requisicao.PRP_Status = false;
                obj_retorno = dados;
            }

            return obj_retorno;
        }

       public dynamic MTD_ObterUsuarioCrud(string pLogin)
        {
            dynamic obj_retorno = new System.Dynamic.ExpandoObject();
            try
            {
                MollaLibraryAPI.PessoaFisica.CRUD_Retorno dados = PessoaMetodos.MTD_ObterDadosCRUD(pLogin, values.PRP_PIT);
                obj_retorno = dados;
            }
            catch (Exception ex)
            {
                MollaLibraryAPI.PessoaFisica.CRUD_Retorno dados = new MollaLibraryAPI.PessoaFisica.CRUD_Retorno();
                dados.PRP_Requisicao.PRP_Mensagem = "Falha ao Obter o usuário";
                dados.PRP_Requisicao.PRP_RetornoTipo = MollaLibraryAPI.COMMON.EnunsAPP.TiposRetorno.Requisicao_Exception;
                dados.PRP_Requisicao.PRP_TipoMensagem = MollaLibraryAPI.COMMON.EnunsAPP.enum_TipoMensagem.Danger;
                dados.PRP_Requisicao.PRP_Status = false;
                obj_retorno = dados;
                Dictionary<string, string> dadosParametros = new Dictionary<string, string>();
                dadosParametros.Add("pLogin", pLogin);
                ex.MTD_EnviaEmailTI("Aplicacao.Models.COMMON.API.PessoaFisica.MTD_ObterUsuarioCrud", dadosParametros);
            }

            return obj_retorno;
        }
    }
}
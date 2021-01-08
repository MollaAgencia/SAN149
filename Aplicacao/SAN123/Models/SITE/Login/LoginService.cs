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
    public class LoginService : MetodosAPI
    {

        SAN123_Entities EF = new SAN123_Entities();
        public LoginService() : base((values.PRP_TipoPessoa.Equals("F") ? EnunsAPP.TipoPessoa.Fisica : EnunsAPP.TipoPessoa.Juridica))
        {

        }

        /// <summary>
        /// Obtêm o usuário, baseado no CPF informado
        /// </summary>
        /// <param name="pCpf"></param>
        /// <returns></returns>
        public override dynamic MTD_ObterUsuario(string pCpf)
        {
            dynamic usuarioRetorno = new ExpandoObject();
            try
            {
                pCpf = pCpf.MTD_ApenasNumeros();
                var usuario = EF.USU_Usuario.FirstOrDefault(u => u.USU_CPF.Equals(pCpf));

                if (usuario != null)
                {
                    if (string.IsNullOrEmpty(usuario.USU_Senha))
                    {

                        usuarioRetorno.PRP_Dados = new Usuario
                        {
                            PRP_Documento = usuario.USU_CPF,
                            PRP_Nome = usuario.USU_Nome,
                            PRP_Email = usuario.USU_EMAIL,
                            PRP_Telefone = usuario.USU_Telefone
                        };

                        usuarioRetorno.PRP_RetornoRequisicao = new RetornoRequisicao
                        {
                            PRP_Mensagem = "Sucesso",
                            PRP_Status = true,
                            PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success
                        };
                    }
                    else
                    {
                        usuarioRetorno.PRP_RetornoRequisicao = new RetornoRequisicao
                        {
                            PRP_Mensagem = $"Usuário cadastrado em {usuario.USU_DataCadastro}".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert),
                            PRP_Status = false,
                            PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert
                        };
                    }
                }
                else
                {
                    usuarioRetorno.PRP_RetornoRequisicao = new RetornoRequisicao
                    {
                        PRP_Mensagem = "Usuário não encontrado.<br/><a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>CLIQUE AQUI</u></a> e entre em contato para liberar o seu acesso.<br/>Lembrete: o site está liberado apenas para o time de campo.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert),
                        PRP_Status = false,
                        PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert
                    };
                }

            }
            catch (Exception e)
            {
                usuarioRetorno.PRP_RetornoRequisicao = new RetornoRequisicao
                {
                    PRP_Mensagem = "Não foi possível processar a Requisição. Entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger),
                    PRP_Status = false,
                    PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger
                };

            }

            return usuarioRetorno;
        }

        /// <summary>
        /// Realiza o cadastro/alteração do usuário na base
        /// </summary>
        /// <param name="dadosUsuario"></param>
        /// <returns></returns>
        public override RetornoRequisicao MTD_CRUD(DadosUsuario dadosUsuario)
        {
            RetornoRequisicao objRetorno = new RetornoRequisicao();
            try
            {
                dadosUsuario.PRP_Documento = dadosUsuario.PRP_Documento.MTD_ApenasNumeros();

                var usuarioBase = EF.USU_Usuario.FirstOrDefault(u => u.USU_CPF.Equals(dadosUsuario.PRP_Documento));

                if (usuarioBase != null)
                {
                    switch (dadosUsuario.PRP_TipoAcao)
                    {
                        case EnunsAPP.CRUD_Acao.Cadastro:

                            if (!string.IsNullOrEmpty(usuarioBase.USU_Senha))
                            {
                                objRetorno.PRP_Status = false;
                                objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                                objRetorno.PRP_Mensagem = $"Usuário cadastrado em {usuarioBase.USU_DataCadastro}".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                            }
                            else
                            {
                                usuarioBase.USU_CPF = dadosUsuario.PRP_Documento.MTD_ApenasNumeros();
                                usuarioBase.USU_Nome = dadosUsuario.PRP_Nome;
                                usuarioBase.USU_EMAIL = dadosUsuario.PRP_Email;
                                usuarioBase.USU_Telefone = !string.IsNullOrEmpty(dadosUsuario.PRP_Telefone) ? dadosUsuario.PRP_Telefone.MTD_ApenasNumeros() : "";
                                usuarioBase.USU_Login = dadosUsuario.PRP_Documento.MTD_ApenasNumeros();
                                usuarioBase.USU_Senha = dadosUsuario.PRP_Senha.MTD_CriptografiaIrreversivel();
                                usuarioBase.USU_DataCadastro = DateTime.Now.MTD_DataHoraBrasil();
                                usuarioBase.USU_DataAceiteRegulamento = DateTime.Now.MTD_DataHoraBrasil();

                                EF.SaveChanges();

                                objRetorno.PRP_Status = true;
                                objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                                objRetorno.PRP_Mensagem = "Cadastro Realizado".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Success);
                            }

                            break;
                        case EnunsAPP.CRUD_Acao.Edicao:

                            if (values.PRP_UsuarioAutenticadoSite == null)
                            {
                                objRetorno.PRP_Status = false;
                                objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                                objRetorno.PRP_Mensagem = "Sua sessão expirou. Faça o Login novamente para prosseguir.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                            }
                            else
                            {
                                usuarioBase.USU_Nome = dadosUsuario.PRP_Nome;
                                usuarioBase.USU_EMAIL = dadosUsuario.PRP_Email;
                                usuarioBase.USU_Telefone = dadosUsuario.PRP_Telefone.MTD_ApenasNumeros();
                                usuarioBase.USU_Senha = dadosUsuario.PRP_Senha == usuarioBase.USU_Senha ? usuarioBase.USU_Senha : dadosUsuario.PRP_Senha.MTD_CriptografiaIrreversivel();

                                EF.SaveChanges();

                                MTD_AdicionarUsuarioSite(usuarioBase, false);

                                objRetorno.PRP_Status = true;
                                objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                                objRetorno.PRP_Mensagem = "Dados alterados com Sucesso!".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Success);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                }
                else
                {
                    objRetorno.PRP_Status = false;
                    objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                    objRetorno.PRP_Mensagem = "Usuário não encontrado. Entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                }

            }
            catch (Exception e)
            {
                objRetorno.PRP_Mensagem = "Não foi possível processar a Requisição. Entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger);
                objRetorno.PRP_Status = false;
                objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;

                Dictionary<string, string> Dados = new Dictionary<string, string>();
                Dados.Add("Nome", dadosUsuario.PRP_Nome);
                Dados.Add("CPF", dadosUsuario.PRP_Documento);
                Dados.Add("E-mail", dadosUsuario.PRP_Email);
                e.MTD_EnviaEmailTI("Aplicacao.Models.SITE.Login.LoginService.MTD_ObterUsuario", Dados);
            }

            return objRetorno;
        }

        /// <summary>
        /// Realiza a autenticação do usuário no sistema
        /// </summary>
        /// <param name="pLogin"></param>
        /// <param name="pSenha"></param>
        /// <param name="pAcessoFake"></param>
        /// <returns></returns>
        public override RetornoRequisicao MTD_Autenticacao(string pLogin, string pSenha, out bool pAcessoFake)
        {
            RetornoRequisicao objRetorno = new RetornoRequisicao();
            pAcessoFake = false;

            pLogin = pLogin.MTD_ApenasNumeros();
            //Alterei aqui, pois o usuário precisa estar ativo para realizar o login.
            var usuario = EF.USU_Usuario.FirstOrDefault(u => u.USU_Login.Equals(pLogin) && u.USU_Ativo);

            if (usuario != null && usuario.USU_Ativo)
            {
                if (usuario.USU_Senha == pSenha.MTD_CriptografiaIrreversivel())
                {
                    pAcessoFake = usuario.USU_AcessoFake;
                    MTD_AdicionarUsuarioSite(usuario);

                    objRetorno.PRP_Status = true;
                    objRetorno.PRP_Mensagem = "Login realizado com Suceso!".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Success);
                    objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
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
                objRetorno.PRP_Mensagem = "Usuário não encontrado. Entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#ContatoModal' data-dismiss='modal'><u>clicando aqui</u></a>".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                objRetorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
            }

            return objRetorno;
        }

        /// <summary>
        /// Alimenta as sessões com as informações pertinentes ao usuário.
        /// </summary>
        /// <param name="usuario">Classe que representa os dados do usuário</param>
        /// <param name="autenticacao">Indica se a requisição veio da autenticação do usuário ou do acesso fake</param>
        /// <returns>Dados do usuário</returns>
        public Pessoa MTD_AdicionarUsuarioSite(USU_Usuario usuario, bool autenticacao = true)
        {
            Pessoa pessoa = new Pessoa();
            pessoa.PRP_DadosCadastrais.PRP_CPF = usuario.USU_CPF;
            pessoa.PRP_DadosCadastrais.PRP_Email = usuario.USU_EMAIL;
            pessoa.PRP_DadosCadastrais.PRP_Identificador = usuario.USU_ID;
            pessoa.PRP_PedidoInternoTrabalho.PRP_AcessoFake = usuario.USU_AcessoFake;
            pessoa.PRP_DadosCadastrais.PRP_Nome = usuario.USU_Nome;

            ComplementoDadosAcesso complemento = ComplementoDadosAcesso.MTD_ObterDadosComplemento(usuario);

            if (autenticacao)
            {
                if (!usuario.USU_AcessoFake)
                {
                    if (values.PRP_Ambiente.Equals("P"))
                    {
                        LAU_LogAutenticacaoUsuario acesso = new LAU_LogAutenticacaoUsuario();
                        acesso.LAU_AcessoIP = values.PRP_IpAcesso;
                        acesso.LAU_Ambiente = values.PRP_Ambiente;
                        acesso.LAU_DadosComplementares = values.PRP_DadosNavegador;
                        acesso.LAU_DataCadastro = DateTime.Now.MTD_DataHoraBrasil();
                        acesso.USU_ID = usuario.USU_ID;

                        EF.LAU_LogAutenticacaoUsuario.Add(acesso);
                        EF.SaveChanges();
                        pessoa.PRP_IdentificadorLogAcesso = acesso.LAU_ID;
                    }

                    SessionVar.Set("AutenticacaoSite", pessoa);
                    SessionVar.Set("ComplementoDados", complemento);
                }
                else
                {
                    SessionVar.Set("AutenticacaoFake", pessoa);
                }
            }
            else
            {
                SessionVar.Set("AutenticacaoSite", pessoa);
                SessionVar.Set("ComplementoDados", complemento);
            }

            return pessoa;
        }


        /// <summary>
        /// Retorna a lista de usuários na base para seleção no Acesso Fake
        /// </summary>
        /// <returns></returns>
        public List<DropDownList> MTD_ListaUsuarios()
        {
            var objRetorno = new List<DropDownList>();
            objRetorno.Add(new DropDownList { PRP_Display = "Selecione", PRP_value = "0" });

            var listaUsuarios = EF.USU_Usuario.OrderBy(u => u.USU_Nome).ToList();

            foreach (var usuario in listaUsuarios)
            {
                objRetorno.Add(new DropDownList
                {
                    PRP_Display = $"{usuario.CAR_Cargo.CAR_DESCRICAO} - {usuario.USU_Nome}",
                    PRP_value = $"{usuario.USU_CPF}"
                });
            }

            return objRetorno;
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
                using (SAN123_Entities EF = new SAN123_Entities())
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
                    else if (!USU.USU_DataAceiteRegulamento.HasValue)
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
    }
}
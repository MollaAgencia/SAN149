using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Aplicacao.Models.COMMON;
using Aplicacao.Models.COMMON.API;
using Aplicacao.Models.COMMON.Email;
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
                if (usuario.USU_PrimeiroAcesso == true)
                {
                    objRetorno.PRP_Status = false;
                    objRetorno.PRP_Mensagem = "Para efetuar o login é necessário se cadastrar.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
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
                    objRetorno.PRP_Mensagem = "Senha incorreta"; //.MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Info);
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
                objSessao.PRP_Email = objUsuario.USU_Email;
                objSessao.PRP_CodSac = objUsuario.USU_CodSac;
                objSessao.PRP_CodNetFlix = objUsuario.USU_CodNetFlix;

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

        public RetornoRequisicao MTD_EsqueciSenhaEnvio(string pLogin)
        {
            RetornoRequisicao ret = new RetornoRequisicao();

            try
            {
                var objUsuario = MTD_ObterUsuario(pLogin.Replace("-", "").Replace(".", ""));
                if (objUsuario.PRP_Requisicao.PRP_Status == false)
                {
                    ret.PRP_TipoMensagem = (MollaLibrary.EnunsApp.enum_TipoMensagem)((int)objUsuario.PRP_Requisicao.PRP_TipoMensagem);
                    ret.PRP_Mensagem = objUsuario.PRP_Requisicao.PRP_Mensagem;
                    ret.PRP_Status = false;
                    ret.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                }
                else
                {
                    Email_Disparo disparo = new Email_Disparo();
                    DadosUsuarioSessao Dados = objUsuario.PRP_Dados;
                    EsqueciSenha dados = new EsqueciSenha();
                    dados.PRP_DataSolicitacao = DateTime.Now.MTD_DataHoraBrasil().ToString("yyyy-MM-dd HH:mm:ss");
                    dados.PRP_HorasValidade = 48;
                    dados.PRP_UsuarioID = Dados.PRP_IdUsuario;
                    disparo.PRP_EmailDestinatario = Dados.PRP_Email;
                    disparo.PRP_Assunto = "Esqueci a senha";
                    string st_Chave = MollaLibrary.Web.JsonUtil.Serialize(dados).MTD_CriptografiaReversivel(MollaLibrary.EnunsApp.en_Criptografia.Encriptar);

                    var item = disparo.MTD_DisparoEmailEsqueciSenha(Dados.PRP_NomeUsuario, st_Chave);
                    ret.PRP_Status = item.PRP_Status;
                    ret.PRP_Mensagem = item.PRP_Mensagem;
                }
            }
            catch (Exception)
            {
                ret.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;
                ret.PRP_Mensagem = "falha ao enviar a requisição";
                ret.PRP_Status = false;
            }
            return ret;
        }

        public dynamic MTD_ObterUsuario(string pLogin)
        {
            dynamic objRetorno = new System.Dynamic.ExpandoObject();
            RetornoRequisicao ret = new RetornoRequisicao();
            DadosUsuarioSessao objCadastro = new DadosUsuarioSessao();
            try
            {
                using (db_SAN149Entities EF = new db_SAN149Entities())
                {
                    USU_Usuario objUsuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_CPF.Equals(pLogin.Replace(".", "").Replace("/", "").Replace("-", "")));
                    if (objUsuario == null)
                    {
                        ret.PRP_Mensagem = "Usuário não encontrado";
                        ret.PRP_Status = false;
                        ret.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                    }
                    else if (!objUsuario.USU_Ativo)
                    {
                        ret.PRP_Status = false;
                        ret.PRP_Mensagem = "O login solicitado está desativado.";
                        ret.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                    }
                    else
                    {

                        objCadastro.PRP_Celular = objUsuario.USU_Celular;
                        objCadastro.PRP_CPF = objUsuario.USU_CPF;
                        objCadastro.PRP_IdBU = objUsuario.UNG_ID;
                        objCadastro.PRP_IdUsuario = objUsuario.USU_ID;
                        objCadastro.PRP_NomeBU = objUsuario.UNG_UnidadeNegocio.UNG_Nome;
                        objCadastro.PRP_NomeUsuario = objUsuario.USU_Nome;
                        objCadastro.PRP_Email = objUsuario.USU_Email;
                        objCadastro.PRP_CodSac = objUsuario.USU_CodSac;
                        ret.PRP_Status = true;
                        objRetorno.PRP_Dados = objCadastro;
                    }
                }
            }
            catch (Exception ex)
            {
                ret.PRP_Mensagem = "Falha ao realizar a requisição.";
                ret.PRP_Status = false;
                ret.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;
            }
            objRetorno.PRP_Requisicao = ret;
            return objRetorno;
        }

        public RetornoRequisicao MTD_Contato(Contato parametros)
        {
            COMMON.Email.Email_Disparo disparo = new COMMON.Email.Email_Disparo();
            RetornoRequisicao retorno = disparo.MTD_DisparoEmailContato(pNome: parametros.PRP_Nome, pAssunto: parametros.PRP_Assunto, pCodSac: parametros.PRP_CodigoSac, pEmail: parametros.PRP_Email, pMensagem: parametros.PRP_Mensagem);
            return retorno;
        }

        public RetornoRequisicao MTD_ContatoUsuario(int pIdUsuario, string pMensagem)
        {
            RetornoRequisicao retorno = new RetornoRequisicao();
            int idUsuario = values.PRP_UsuarioAutenticadoSite.PRP_IdUsuario;

            if (idUsuario == pIdUsuario)
            {
                using (db_SAN149Entities EF = new db_SAN149Entities())
                {
                    var objUsuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_ID == idUsuario);

                    if (objUsuario != null)
                    {
                        COMMON.Email.Email_Disparo disparo = new COMMON.Email.Email_Disparo();
                        retorno = disparo.MTD_DisparoEmailContato(pNome: objUsuario.USU_Nome, pAssunto: "Contato usuario: " + objUsuario.USU_Nome + ".", pCodSac: objUsuario.USU_CodSac, pEmail: objUsuario.USU_Email, pMensagem: pMensagem);
                        return retorno;
                    }
                    else
                    {
                        retorno.PRP_Mensagem = "Erro ao obeter dados do usuário. Tente novamente mais tarde".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                        retorno.PRP_Status = false;
                        retorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                        return retorno;
                    }
                }
            }
            else
            {
                retorno.PRP_Mensagem = "Falha ao processar a sua requisição. Tente novamente mais tarde".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger);
                retorno.PRP_Status = false;
                retorno.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
                return retorno;
            }
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
        public Usuario MTD_GetCadastro(string pCPF)
        {
            Usuario retornoUsuario = new Usuario();
            try
            {
                var objUsuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_CPF == pCPF && x.USU_Ativo == true);
                if (objUsuario != null)
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
                    dadosUsuario.PRP_SenhaHasValue = (objUsuario.USU_Senha == null ? false : true);
                    retornoUsuario.OBJ_Usuario = dadosUsuario;

                    retornoUsuario.PRP_Requisicao.PRP_Status = true;
                    retornoUsuario.PRP_Requisicao.PRP_Mensagem = "Usuario localizado. Completar cadastro.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Success);
                    retornoUsuario.PRP_Requisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                }
                else
                {
                    retornoUsuario.PRP_Requisicao.PRP_Status = false;
                    retornoUsuario.PRP_Requisicao.PRP_Mensagem = "CPF não localizado em nossa base."; //.MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger);
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
        public RetornoRequisicao MTD_AtualizarDados(string PRP_Documento, string PRP_Nome, string PRP_Telefone, string PRP_Email, string PRP_Senha)
        {
            RetornoRequisicao ret = new RetornoRequisicao();
            try
            {
                int idUsuario = values.PRP_UsuarioAutenticadoSite.PRP_IdUsuario;

                USU_Usuario objUsuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_ID == idUsuario);

                if (objUsuario != null)
                {
                    objUsuario.USU_Nome = PRP_Nome;
                    objUsuario.USU_Celular = PRP_Telefone.MTD_ApenasNumeros();
                    objUsuario.USU_Email = PRP_Email;
                    if (PRP_Senha != "")
                    {
                        objUsuario.USU_Senha = PRP_Senha.MTD_CriptografiaIrreversivel();
                    }
                    EF.SaveChanges();
                    ret.PRP_Status = true;
                    ret.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                    ret.PRP_Mensagem = "Dados atualizados com sucesso."; //.MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Success);
                }
                else
                {
                    ret.PRP_Status = false;
                    ret.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert;
                    ret.PRP_Mensagem = "Erro ao salvar, dados do usuário não conferem.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert);
                }

            }
            catch (Exception)
            {
                ret.PRP_Status = false;
                ret.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
                ret.PRP_Mensagem = "Erro ao processar sua requisição.".MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger);
            }
            return ret;
        }
    }
}
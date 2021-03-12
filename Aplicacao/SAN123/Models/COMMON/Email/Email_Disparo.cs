using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MollaLibrary.ExtendMethods;

namespace Aplicacao.Models.COMMON.Email
{
    public class Email_Disparo
    {
        public Email_Disparo()
        {
            st_EmailRemetente = "contato@campanhaplaytowin.com.br";
            st_EmailSmtpRemetente = "email-ssl.com.br";
            st_EmailCredencialSenha = "@@Iot2019@@";
            it_EmailSmtpPorta = 587;
            st_EmailUsuario = "contato@campanhaplaytowin.com.br";
            EmailRemetenteAlias = "Contato SAN149";
            //MTD_ListaDadosConfiguracao();

        }

        #region A T R I B U T O S

        private string st_EmailRemetente;
        private string st_EmailSmtpRemetente;
        private string st_EmailCredencialSenha;
        private int it_EmailSmtpPorta;
        private string st_EmailUsuario;
        private string EmailRemetenteAlias;
        /// <summary>
        /// Email que será destinados os contatos interno e externo do site
        /// </summary>
        private string EmailContato;

        #endregion A T R I B U T O S

        #region P R O P R I E D A D E S
        /// <summary>
        /// Retorna o caminho para o arquivo texto que contem o HTML para o envio do esqueci a senha
        /// </summary>
        private string PRP_CaminhoContatoHTML { get { return string.Format("{0}HTMLcontatoInterno.txt", HttpContext.Current.Request.PhysicalApplicationPath); } }

        /// <summary>
        /// Retorna o caminho para o arquivo texto que contem o HTML para o envio do esqueci a senha
        /// </summary>
        private string PRP_CaminhoEsqueciSenhaHTML { get { return string.Format("{0}HTMLEsqueciSenha.txt", HttpContext.Current.Request.PhysicalApplicationPath); } }
        /// <summary>
        /// Caminho para obter o arquivo que contem o HTML para enviar o email para a TI
        /// </summary>
        private string PRP_CaminhoMensagemTiHTML { get { return string.Format("{0}HTML_MensagemTI.txt", HttpContext.Current.Request.PhysicalApplicationPath); } }

        /// <summary>
        /// Caminho para obter o arquivo que contem o HTML para enviar o email para a TI
        /// </summary>
        private string PRP_CaminhoMensagemCadastro { get { return string.Format("{0}HTML_CadastroMensagem.txt", HttpContext.Current.Request.PhysicalApplicationPath); } }


        /// <summary>
        /// Configuração: Email que será enviado o Email
        /// </summary>
        public string PRP_EmailRemetente { get { return st_EmailRemetente; } set { st_EmailRemetente = value; } }
        /// <summary>
        /// Configuração: SMTP de saida do email
        /// </summary>
        public string PRP_EmailSmtpRemetente { get { return st_EmailSmtpRemetente; } set { st_EmailSmtpRemetente = value; } }
        /// <summary>
        /// Configuração: Senha do email
        /// </summary>
        public string PRP_EmailCredencialSenha { get { return st_EmailCredencialSenha; } set { st_EmailCredencialSenha = value; } }
        /// <summary>
        /// Configuração: Porta que o email será disparado
        /// </summary>
        public int PRP_EmailSmtpPorta { get { return it_EmailSmtpPorta; } set { it_EmailSmtpPorta = value; } }
        /// <summary>
        /// Configuração: Valor para logar com a credencial.
        /// </summary>
        public string PRP_EmailUsuario { get { return st_EmailUsuario; } set { PRP_EmailUsuario = value; } }
        /// <summary>
        /// Configuração: Alias que será utilizado no email para disparo
        /// </summary>
        public string PRP_EmailRemetenteAlias { get { return EmailRemetenteAlias; } set { EmailRemetenteAlias = value; } }
        /// <summary>
        /// Não sei para que serve mas sem essa parada corretamente não envia o e-mail
        /// </summary>
        public bool PRP_SmtpHabilitaSsl { get; set; }

        /// <summary>
        /// Indica qual o destinatario do e-mail
        /// </summary>
        public string PRP_EmailDestinatario { get; set; }
        /// <summary>
        /// Assunto do E-mail | Default = 'Contato Sayerlack DES'
        /// </summary>
        public string PRP_Assunto { get; set; }


        #endregion P R O P R I E D A D E S

        private void MTD_ListaDadosConfiguracao()
        {

            MollaLibraryAPI.COMMON.Metodos metodos = new MollaLibraryAPI.COMMON.Metodos(values.PRP_MollaAPI);

            MollaLibraryAPI.COMMON.DadosEmailRetorno dadosRetorno = metodos.MTD_Email_ObterDados(values.PRP_PIT);
            

            if (dadosRetorno.PRP_Dados!= null && dadosRetorno.PRP_Dados.Count != 0)
            {
                foreach (MollaLibraryAPI.COMMON.DadosEmail linha in dadosRetorno.PRP_Dados)
                {
                    switch (linha.DEC_TipoDado)
                    {
                        case "EmailRemetente":
                            st_EmailRemetente = linha.DEC_ValorInformacao;
                            break;

                        case "EmailSmtpRemetente":
                            st_EmailSmtpRemetente = linha.DEC_ValorInformacao;
                            break;

                        case "EmailCredencialSenha":
                            st_EmailCredencialSenha = linha.DEC_ValorInformacao;
                            break;
                        case "EmailSmtpPorta":
                            it_EmailSmtpPorta = int.Parse(linha.DEC_ValorInformacao);
                            break;
                        case "EmailRemetenteAlias":
                            EmailRemetenteAlias = linha.DEC_ValorInformacao;
                            break;
                        case "EmailContato":
                            EmailContato = linha.DEC_ValorInformacao;
                            break;
                        case "EmailContatoAssunto":
                            PRP_Assunto = linha.DEC_ValorInformacao;
                            break;
                        case "EnableSsl":
                            PRP_SmtpHabilitaSsl = bool.Parse(linha.DEC_ValorInformacao);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public MollaLibrary.COMMON.RetornoRequisicao MTD_DisparoEmailContato(string pNome, string pAssunto, string pCodSac, string pMensagem, string pEmail)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            
            try
            {
                Models.COMMON.Email.Email_MensagemCompleta EnviarEmail = new Models.COMMON.Email.Email_MensagemCompleta();
                EnviarEmail.PRP_EmailDestinatarios = new List<string>();
                EnviarEmail.PRP_EmailAssunto = PRP_Assunto;
                EnviarEmail.PRP_EmailCorpo = MTD_ObterHtmlContato(pNome, pMensagem, pCodSac, pAssunto, pEmail);
                EnviarEmail.PRP_EmailDestinatarios.Add("contato@campanhaplaytowin.com.br");
                EnviarEmail.PRP_EmailRemetente = PRP_EmailRemetente;
                EnviarEmail.PRP_EmailRemetenteAlias = PRP_EmailRemetenteAlias;
                EnviarEmail.PRP_SmtpEndereco = PRP_EmailSmtpRemetente;
                EnviarEmail.PRP_CredencialSenha = PRP_EmailCredencialSenha;
                EnviarEmail.PRP_RepplyToListEmail = pEmail;
                EnviarEmail.PRP_SmtpPorta = it_EmailSmtpPorta;
                EnviarEmail.PRP_SmtpHabilitaSsl = PRP_SmtpHabilitaSsl;


                requisicao.PRP_Mensagem = "Mensagem enviada. Em até 48 horas você receberá um contato da Central de Atendimento.".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Success);
                requisicao.PRP_Status = true;

                EnviarEmail.mtd_EnviarEmail();
            }
            catch (Exception ex)
            {
                requisicao.PRP_Mensagem = "Não foi possível processar a requisição, por favor tente mais tarde".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger); ;
                requisicao.PRP_Status = true;

                Dictionary<string, string> Dados = new Dictionary<string, string>();

                Dados.Add("pNome", pNome);
                Dados.Add("pAssunto", pAssunto);
                Dados.Add("pMensagem", pMensagem);
                Dados.Add("pEmail", pEmail);

                ex.MTD_EnviaEmailTI("public COMMON.RetornoRequisicao MTD_DisparoEmailContato(string pNome,string pAssunto, string pMensagem,string pEmail)", Dados);
            }

            return requisicao;

        }

        public MollaLibrary.COMMON.RetornoRequisicao MTD_DisparoEmailEsqueciSenha(string pNome, string pLink)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            try
            {
                Models.COMMON.Email.Email_MensagemCompleta EnviarEmail = new Models.COMMON.Email.Email_MensagemCompleta();
                EnviarEmail.PRP_EmailDestinatarios = new List<string>();
                EnviarEmail.PRP_EmailAssunto = PRP_Assunto;
                EnviarEmail.PRP_EmailCorpo = MTD_ObterHtmlEsqueciSenha(pNome, pLink);
                EnviarEmail.PRP_EmailDestinatarios.Add(PRP_EmailDestinatario);
                EnviarEmail.PRP_EmailRemetente = PRP_EmailRemetente;
                EnviarEmail.PRP_EmailRemetenteAlias = PRP_EmailRemetenteAlias;
                EnviarEmail.PRP_SmtpEndereco = PRP_EmailSmtpRemetente;
                EnviarEmail.PRP_CredencialSenha = PRP_EmailCredencialSenha;
                EnviarEmail.PRP_SmtpHabilitaSsl = PRP_SmtpHabilitaSsl;
                EnviarEmail.PRP_SmtpPorta = PRP_EmailSmtpPorta;



                EnviarEmail.mtd_EnviarEmail();
                requisicao.PRP_Status = true;
                requisicao.PRP_Mensagem = $"Email enviado com sucesso para {PRP_EmailDestinatario}".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Success);
                requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Success;


            }
            catch (Exception ex)
            {
                requisicao.PRP_Status = false;
                requisicao.PRP_Mensagem = $"Não Foi possivel realizar a requisição, caso continue por favor entre em contato".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);
                requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;
                Dictionary<string, string> Dados = new Dictionary<string, string>();
                Dados.Add("pNome", pNome);
                Dados.Add("pLink", pLink);

                ex.MTD_EnviaEmailTI("public COMMON.RetornoRequisicao MTD_DisparoEmailEsqueciSenha(string pNome, string pLink)");
            }

            return requisicao;
        }

        public MollaLibrary.COMMON.RetornoRequisicao MTD_DisparoEmailExcecaoTI(LogError logError)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            try
            {
                Models.COMMON.Email.Email_MensagemCompleta EnviarEmail = new Models.COMMON.Email.Email_MensagemCompleta();
                EnviarEmail.PRP_EmailDestinatarios = new List<string>();
                EnviarEmail.PRP_EmailAssunto = string.Format("{1} - Erro no Ambiente {0}", values.PRP_Ambiente, values.PRP_PIT);
                EnviarEmail.PRP_EmailCorpo = this.MTD_ObterHtmlTiMolla(logError.PRP_NomePagina, logError.PRP_Mensagem, logError.PRP_Excecao);
                EnviarEmail.PRP_EmailDestinatarios.Add("ti@agenciamolla.com.br");
                EnviarEmail.PRP_EmailRemetente = PRP_EmailRemetente;
                EnviarEmail.PRP_EmailRemetenteAlias = PRP_EmailRemetenteAlias;
                EnviarEmail.PRP_SmtpEndereco = PRP_EmailSmtpRemetente;
                EnviarEmail.PRP_CredencialSenha = PRP_EmailCredencialSenha;
                EnviarEmail.PRP_SmtpHabilitaSsl = PRP_SmtpHabilitaSsl;

                EnviarEmail.mtd_EnviarEmail();
                requisicao.PRP_Status = true;
            }
            catch (Exception ex)
            {
                requisicao.PRP_Status = false;
                requisicao.PRP_Mensagem = "Erro ao enviar";
            }
            return requisicao;
        }

        public MollaLibrary.COMMON.RetornoRequisicao MTD_DisparoEmailCadastro(string pMensagem, string pDestinatario)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            try
            {
                Models.COMMON.Email.Email_MensagemCompleta EnviarEmail = new Models.COMMON.Email.Email_MensagemCompleta();
                EnviarEmail.PRP_EmailDestinatarios = new List<string>();
                EnviarEmail.PRP_EmailAssunto = PRP_Assunto;
                EnviarEmail.PRP_EmailCorpo = MTD_ObterHtmlCadastro(pMensagem);
                EnviarEmail.PRP_EmailDestinatarios.Add(pDestinatario);
                EnviarEmail.PRP_EmailRemetente = PRP_EmailRemetente;
                EnviarEmail.PRP_EmailRemetenteAlias = PRP_EmailRemetenteAlias;
                EnviarEmail.PRP_SmtpEndereco = PRP_EmailSmtpRemetente;
                EnviarEmail.PRP_CredencialSenha = PRP_EmailCredencialSenha;
                EnviarEmail.PRP_SmtpHabilitaSsl = PRP_SmtpHabilitaSsl;


                EnviarEmail.mtd_EnviarEmail();
                requisicao.PRP_Status = true;
                requisicao.PRP_Mensagem = $"Email enviado com sucesso para {PRP_EmailDestinatario}".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Success);


            }
            catch (Exception ex)
            {
                requisicao.PRP_Status = false;
                requisicao.PRP_Mensagem = $"Não Foi possivel realizar a requisição, caso continue por favor entre em contato";
                Dictionary<string, string> Dados = new Dictionary<string, string>();
                Dados.Add("pMensagem", pMensagem);
                Dados.Add("pDestinatario", pDestinatario);
                ex.MTD_EnviaEmailTI("public COMMON.RetornoRequisicao MTD_DisparoEmailCadastro(string pMensagem,string pDestinatario)", Dados);
            }

            return requisicao;
        }

        public string MTD_ObterHtmlContato(string pNome, string pMensagem, string pCodSac, string pAssunto, string pEmail)
        {
            string st_HTML = "";
            st_HTML = System.IO.File.ReadAllText(PRP_CaminhoContatoHTML);
            return st_HTML
                .Replace("[Nome do Usuário]", pNome)
                .Replace("[Data Solicitação]", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                .Replace("[Email]", pEmail)
                .Replace("[Assunto]", pAssunto)
                .Replace("[CodigoSac]", pCodSac)
                .Replace("[Mensagem]", pMensagem)
                ;
        }

        /// <summary>
        /// Método retorna o HTML para enviar o email para a TI MOLLA
        /// </summary>
        /// <param name="pNomePagina">Nome da pagina que ocorreu a exceção</param>
        /// <param name="pMetodo">Nome do método</param>
        /// <param name="pMensagem">Mensagem personalizada</param>
        /// <param name="pExcecao">Valores da exeption gerada</param>
        /// <returns>HTML no esquema para enviar</returns>
        private string MTD_ObterHtmlTiMolla(string pNomePagina, string pMensagem, string pExcecao, string pDescricao = "")
        {

            string st_usuario = "";
            if (COMMON.values.PRP_UsuarioAutenticadoSite == null)
            {
                st_usuario += "Usuario Site: Não há usuário logado";
            }
            else
            {
                st_usuario += "Usuario Site: ";
            }

            //if (Values.PRP_UsuarioAutenticadoSite == null)
            //{
            //    st_usuario += " | Usuario Admin: Não há usuário logado";
            //}
            //else
            //{
            //    st_usuario = " | Usuario Admin: " + "";// Values.PRP_UsuarioAutenticadoAdmin.MTD_ToString();
            //}



            string st_HTML = "";
            st_HTML = System.IO.File.ReadAllText(PRP_CaminhoMensagemTiHTML);
            return st_HTML
                .Replace("[Pagina]", pNomePagina)
                .Replace("[Data]", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                .Replace("[Descricao]", pDescricao)
                .Replace("[Usuario]", st_usuario)
                .Replace("[Mensagem]", pMensagem)
                .Replace("[Execao]", pExcecao)
                ;
        }


        /// <summary>
        /// Método para obter o HTML para ser enviado no esqueci a senha para o usuario cadastrado na base
        /// </summary>
        /// <param name="pNome">Nome do usuário cadastrado na base</param>
        /// <param name="pSenha">Senha do usuário cadastrado na base</param>
        /// <returns>HTML já preenchido com as informações necessarias</returns>
        private string MTD_ObterHtmlEsqueciSenha(string pNome, string pSenha)
        {
            string st_HTML = "";
            string st_URL = string.Format("{0}/{1}", COMMON.values.PRP_NomePaginaAtualCompleto.Replace("Login/MTD_EsqueciSenha", "EsqueciSenha"), pSenha);
            st_HTML = System.IO.File.ReadAllText(PRP_CaminhoEsqueciSenhaHTML);
            return st_HTML.Replace("[Data Solicitação]", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")).Replace("[Nome do Usuário]", pNome).Replace("[link]", st_URL);
        }
        /// <summary>
        /// Método para obter o html para o envio de email de confirmação de cadastro ou avalaição do cadastro
        /// </summary>
        /// <param name="pMensagem"></param>
        /// <returns></returns>
        private string MTD_ObterHtmlCadastro(string pMensagem)
        {
            string st_HTML = "";
            st_HTML = System.IO.File.ReadAllText(PRP_CaminhoMensagemCadastro);
            return st_HTML.Replace("[Mensagem]", pMensagem).Replace("[Data]", DateTime.Now.MTD_DataHoraBrasil().ToString("dd/MM/yyyy HH:mm:ss"));
        }
        /// <summary>
        /// Indica qual tipo de e-mail será disparado
        /// </summary>
        public enum enum_Email
        {
            /// <summary>
            /// Email informar a TI que algo deu errado
            /// </summary>
            EmailTI,
            /// <summary>
            /// Email para reenviar a senha para o usuário
            /// </summary>
            EsqueciSenha,
            /// <summary>
            /// Email para a aréa de contato do site
            /// </summary>
            Contato,

        }
    }
}
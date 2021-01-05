using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Net.Mail;
namespace Aplicacao.Models.COMMON.Email
{
    public class Email_MensagemCompleta
    {
        #region A T R I B U T O S
        private System.IO.MemoryStream stream = null;
        #endregion A T R I B U T O S

        #region C O N S T R U T O R E S
        public Email_MensagemCompleta()
        {
            this.PRP_SmtpHabilitaSsl = false;
            this.PRP_EmailAnexos = new List<string>();
            this.PRP_EmailDestinatarios = new List<string>();

        }
        #endregion C O N S T R U T O R E S

        #region M É T O D O S
        /// <summary>
        /// Envia E-mail's onde o email esta local
        /// </summary>
        public void mtd_EnviarEmail()
        {
            System.Net.Mail.SmtpClient smtp = null;

            using (MailMessage emailMensagem = new MailMessage())
            {
                if (!string.IsNullOrEmpty(PRP_EmailRemetenteAlias))
                {
                    emailMensagem.From = new MailAddress(PRP_EmailRemetente, PRP_EmailRemetenteAlias);
                }
                else
                {
                    emailMensagem.From = new MailAddress(PRP_EmailRemetente);
                }

                if (PRP_EmailDestinatarios != null)
                {
                    for (int i = 0; i < PRP_EmailDestinatarios.Count; i++)
                    {
                        if (i == 0)
                        {
                            emailMensagem.To.Add(PRP_EmailDestinatarios[i]);
                            if (!string.IsNullOrWhiteSpace(PRP_RepplyToListEmail))
                                emailMensagem.ReplyToList.Add(PRP_RepplyToListEmail);
                        }
                        else
                        {
                            emailMensagem.CC.Add(PRP_EmailDestinatarios[i]);
                        }
                    }

                }

                emailMensagem.Subject = PRP_EmailAssunto;
                emailMensagem.Body = PRP_EmailCorpo;
                emailMensagem.IsBodyHtml = true;

                if (PRP_EmailAnexos != null)
                {
                    if (PRP_EmailAnexos.Count == 1 && stream != null)
                    {
                        foreach (string anexos in PRP_EmailAnexos)
                        {
                            emailMensagem.Attachments.Add(new Attachment(stream, anexos));
                        }
                    }
                    else
                    {

                        foreach (string anexos in PRP_EmailAnexos)
                        {
                            emailMensagem.Attachments.Add(new Attachment(anexos));
                        }
                    }
                }



                if (this.PRP_SmtpPorta != 0)
                {
                    smtp = new SmtpClient(PRP_SmtpEndereco, PRP_SmtpPorta);
                }
                else
                {
                    smtp = new SmtpClient(PRP_SmtpEndereco);
                }


                smtp.Credentials = new NetworkCredential(PRP_EmailRemetente, PRP_CredencialSenha);
                smtp.EnableSsl = PRP_SmtpHabilitaSsl;
                smtp.Send(emailMensagem);

            }
        }
        /// <summary>
        /// Envia um email e insere o anxo via array de bytes.
        /// Bom para usar na web. Nesse caso não esquecer de colocar o nome na propriedade de anexo mas só o nome.
        /// </summary>
        /// <param name="MS"></param>
        public void mtd_EnviarEmail(System.IO.MemoryStream MS)
        {
            stream = MS;
            mtd_EnviarEmail();
        }

        #endregion M É T O D O S

        #region P R O P R I E D A D E S
        /*********************************************************************************************************************************
     *                                      Informações Basicas para autenticar o envio do e-mail
     *********************************************************************************************************************************/
        /// <summary>
        /// Endereço SMTP. Exemplo: Servidor do gmail = smtp.gmail.com
        /// </summary>
        public string PRP_SmtpEndereco { get; set; }
        /// <summary>
        /// Um Int32 maior que zero que contém a porta a ser usado no host.
        /// </summary>
        public int PRP_SmtpPorta { get; set; }
        /// <summary>
        /// Senha para autenticar o envio do e-mail, que é a senha do email remetente
        /// </summary>
        public string PRP_CredencialSenha { set; get; }

        /**********************************************************************************************************************************/


        /*********************************************************************************************************************************
         *                                      Informações Basicas o Envio do email
         *********************************************************************************************************************************/
        /// <summary>
        /// Email que irá enviar o email e tambem quem irá autenticar o  envio da mensagem
        /// </summary>
        public string PRP_EmailRemetente { get; set; }
        public string PRP_EmailRemetenteAlias { get; set; }
        /// <summary>
        /// Lista de e-mails  que receberá o e-mail
        /// </summary>
        public List<string> PRP_EmailDestinatarios { get; set; }
        /// <summary>
        /// Mensagem do corpo do email
        /// </summary>
        public string PRP_EmailCorpo { get; set; }
        /// <summary>
        /// Assuto do E-mail que será enviado
        /// </summary>
        public string PRP_EmailAssunto { get; set; }
        /// <summary>
        /// Lista com o caminhos para os anexos do e-mail
        /// </summary>
        public List<string> PRP_EmailAnexos { get; set; }
        /// <summary>
        /// Especifica se a SmtpClient usa o Secure Sockets Layer (SSL) para criptografar a conexão.
        /// Para o gmail tem que ser true para o corporativo da molla tem que ser false
        /// </summary>
        public bool PRP_SmtpHabilitaSsl { get; set; }

        /**********************************************************************************************************************************/
        /*********************************************************************************************************************************
         *                                      Informações Basicas o Envio do email
         *********************************************************************************************************************************/
        /// <summary>
        /// Indica qual a prioridade do email, o default é normal
        /// </summary>
        public EmailPrioridade PRP_EmailPrioridade { get; set; }
        /// <summary>
        /// Indica se o email será respondido para o email do destinatario | Default false
        /// </summary>

        /// <summary>
        /// Email para onde o email será respondido quando for solicitado
        /// </summary>
        public string PRP_RepplyToListEmail { get; set; }
        #endregion P R O P R I E D A D E S

        /// <summary>
        /// Indica qual o nivel de prioridade do e-mail
        /// </summary>
        public enum EmailPrioridade
        {
            Alta,
            Baixa,
            Normal
        }
    }
}
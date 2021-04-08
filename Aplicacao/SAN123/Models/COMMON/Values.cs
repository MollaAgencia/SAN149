using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Aplicacao.Models.ENTITY;
using Aplicacao.Models.SITE.Login;

namespace Aplicacao.Models.COMMON
{
    public class values
    {
        public static string PRP_StringConexao { get { return @"Data Source=molla.csjvcqt790vh.us-east-1.rds.amazonaws.com;Initial Catalog=SAN123;User ID=SAN123;Password=@@iot2019@@;Connection Timeout=120"; } }
        #region C O N F I G U R A Ç Ã O  B U C K E T

        public static string PRP_AWSBucketName { get { return "molla-whi138"; } }
        public static string PRP_AWSBucketAccessKey { get { return "AKIAJHU4L4KPN5U6L4WQ"; } }
        public static string PRP_AWSBucketSecretKey { get { return "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"; } }
        public static Amazon.RegionEndpoint PRP_AWSBucketRegionEndPoint { get { return Amazon.RegionEndpoint.SAEast1; } }

        #endregion C O N F I G U R A Ç Ã O  B U C K E T


        #region D A D O S  W E B  C O N F I G
        /// <summary>
        /// Obtem em qual ambiente a aplicação esta diponivel
        /// </summary>
        public static string PRP_Ambiente { get { return System.Configuration.ConfigurationManager.AppSettings["Ambiente"].ToString(); } }
        /// <summary>
        /// Contém o código do projeto
        /// </summary>
        public static string PRP_PIT { get { return System.Configuration.ConfigurationManager.AppSettings["PIT"].ToString(); } }
        /// <summary>
        /// Contém o tipo da pessoa que esta acessando o Site (Caso seja os dois tipos faça uma customização)
        /// </summary>
        public static string PRP_TipoPessoa { get { return System.Configuration.ConfigurationManager.AppSettings["TipoPessoa"].ToString(); } }
        /// <summary>
        /// Contém a URL para realizar o consumo da API
        /// </summary>
        public static string PRP_MollaAPI { get { return System.Configuration.ConfigurationManager.AppSettings["ApiBase"].ToString(); } }

        

        #endregion D A D O S  W E B  C O N F I G


        #region D A D O S  A U T E N T I C A Ç Ã O

        /// <summary>
        /// Contém os dados necessários de autenticação para Pessoa Fisica
        /// </summary>
        public static DadosUsuarioSessao PRP_UsuarioAutenticadoSite { get { return HttpContext.Current.Session["AutenticacaoSite"] == null ? null : (DadosUsuarioSessao)HttpContext.Current.Session["AutenticacaoSite"]; } }

        public static DadosUsuarioSessao PRP_UsuarioFake { get { return HttpContext.Current.Session["AutenticacaoFake"] == null ? null : (DadosUsuarioSessao)HttpContext.Current.Session["AutenticacaoFake"]; } }
        /// <summary>
        /// Dados complementares do usuário autenticado.
        /// </summary>
        public static COMMON.ComplementoDadosAcesso PRP_UsuarioAutenticadoSiteComplemento { get { return HttpContext.Current.Session["ComplementoDados"] == null ?null: (ComplementoDadosAcesso)HttpContext.Current.Session["ComplementoDados"]; } }
        #endregion D A D O S  A U T E N T I C A Ç Ã O


        #region D A D O S  N A V E G A Ç Ã O  P A G I N A
        /// <summary>
        /// Retorna o nome da pagina que estamos acessando
        /// </summary>
        public static string PRP_NomePaginaAtual { get { return HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf("/") + 1); } }
        /// <summary>
        /// Retorna a url completa da pagina que estamos acessando
        /// </summary>
        public static string PRP_NomePaginaAtualCompleto { get { return HttpContext.Current.Request.Url.OriginalString; } }

        /// <summary>
        /// Retorna a string de conexão para a base de dados
        /// </summary>

        public static string PRP_IpAcesso { get { return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); } }
        public static string PRP_DadosNavegador
        {
            get
            {
                System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                System.Text.StringBuilder stb_browserInfo = new System.Text.StringBuilder();
                stb_browserInfo.AppendFormat("Type: {0}", browser.Type);
                stb_browserInfo.AppendFormat("Name: {0}", browser.Browser);
                stb_browserInfo.AppendFormat("Version: {0}", browser.Version);
                return stb_browserInfo.ToString();
            }
        }

        #endregion D A D O S  N A V E G A Ç Ã O  P A G I N A


        /// <summary>
        /// Codigo do projeto para obter apenas os produtos pertinentes a campanha
        /// </summary>
        public static string PRP_GifftyCodigoProjeto { get { return "P44"; } }


        public static string Criptografar(string Message)
        {
            byte[] Results = null;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Message));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }
    }
}
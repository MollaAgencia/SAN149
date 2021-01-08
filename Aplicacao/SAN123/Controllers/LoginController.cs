using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao.Models.ENTITY;
using Aplicacao.Models.SITE.Login;
using MollaLibrary.ExtendMethods;
using Newtonsoft.Json;

namespace Aplicacao.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService = new LoginService();


        // GET: Login
        public ActionResult Autenticacao()
        {
            return View();
        }

        public ActionResult EsqueciSenha(string chave)
        {
            if (TempData["PPR_Requisicao"] == null)
            {
                MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
                string st_Chave = chave.MTD_CriptografiaReversivel(MollaLibrary.EnunsApp.en_Criptografia.Decriptar);
                Session["GuardeiaChave"] = st_Chave;
                TempData["PPR_Requisicao"] = requisicao;
            }
            return View();
        }

        [HttpPost]
        public JsonResult MTD_AtualizaSenha(string senha, string confSenha)
        {
            MollaLibrary.COMMON.RetornoRequisicao _Requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            string senhaCripto = Models.COMMON.values.Criptografar(senha.Trim());
            try
            {
                MollaLibrary.DataSource.MicrosoftSqlServer sqlServer = new MollaLibrary.DataSource.MicrosoftSqlServer(Models.COMMON.values.PRP_StringConexao);
                System.Data.SqlClient.SqlParameterCollection parameterCollection = sqlServer.InicializaSqlParameterCollection;
                parameterCollection.Add("@pSenha", System.Data.SqlDbType.VarChar).Value = senhaCripto;
                parameterCollection.Add("@pID", System.Data.SqlDbType.Int).Value = Session["GuardeiaChave"];
                int dtb_result = sqlServer.DbExecuteNonQuery("sp_site_resetSenha", parameterCollection, System.Data.CommandType.StoredProcedure);
                if (dtb_result > 0)
                {
                    _Requisicao.PRP_Status = true;
                    _Requisicao.PRP_Mensagem = "Senha alterada com sucesso".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Success);
                }
                else
                {
                    _Requisicao.PRP_Status = false;
                    _Requisicao.PRP_Mensagem = "Erro ao alterar sua senha, tente novamente em instantes".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Alert);
                }
            }
            catch (Exception ex)
            {
                _Requisicao.PRP_Status = false;
                _Requisicao.PRP_Mensagem = "Erro ao alterar sua senha, tente novamente em instantes".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Alert);
            }
            return Json(MollaLibrary.Web.JsonUtil.Serialize(_Requisicao));
        }

        public ActionResult AcessoFake()
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = null;
            if (TempData["Retorno"] != null)
            {
                requisicao = (MollaLibrary.COMMON.RetornoRequisicao)TempData["Retorno"];
            }
            else if (Models.COMMON.values.PRP_UsuarioFake == null)
            {
                return RedirectToAction("Autenticacao");
            }
            else
            {
                requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
                requisicao.PRP_Status = true;
            }

            ViewBag.PRP_Usuarios = new SelectList(_loginService.MTD_ListaUsuarios(), "PRP_value", "PRP_Display", "0");

            return View(requisicao);
        }

        [HttpPost]
        public ActionResult AcessoFakeRedirecionar(string Usuarios)
        {
            /**********************************************************************************
             * Adicionar logica para o acesso fake
             **********************************************************************************/
            var requisicao = new MollaLibrary.COMMON.RetornoRequisicao();

            try
            {
                if (Models.COMMON.values.PRP_UsuarioFake != null)
                {
                    SAN123_Entities EF = new SAN123_Entities();
                    var usuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_Login.Equals(Usuarios));

                    if (usuario == null)
                    {
                        requisicao.PRP_Mensagem = "O usuário solicitado não foi encontrado";
                        requisicao.PRP_Status = false;
                        requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                        TempData["requisicao"] = requisicao;
                    }
                    else
                    {
                        _loginService.MTD_AdicionarUsuarioSite(usuario, false);

                        Models.COMMON.values.PRP_UsuarioAutenticadoSite.PRP_PedidoInternoTrabalho.PRP_AcessoFake = true;
                        return RedirectToAction("Home", new { controller = "Conteudo" });
                    }
                }
            }
            catch (Exception ex)
            {

                requisicao.PRP_Mensagem = "Erro ao configurar o acesso fake";
                requisicao.PRP_Status = false;
                requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;
                TempData["requisicao"] = requisicao;
            }

            return View("AcessoFake");
        }



       

        [HttpPost]
        public ActionResult MTD_CadastroParticipante(string CPF)
        {
            string CPFMASCA = CPF;
            List<Models.SITE.Cad_Cadastro> _listCadastro = new List<Models.SITE.Cad_Cadastro>();
            try
            {
                MollaLibrary.DataSource.MicrosoftSqlServer sqlServer = new MollaLibrary.DataSource.MicrosoftSqlServer(Models.COMMON.values.PRP_StringConexao);
                System.Data.SqlClient.SqlParameterCollection sqlParameterCollection = sqlServer.InicializaSqlParameterCollection;
                sqlParameterCollection.Add("@CPF", System.Data.SqlDbType.VarChar).Value = CPF.MTD_ApenasNumeros();
                System.Data.DataTable dtb_result = sqlServer.DbExecute("sp_site_valida_cadastro_SAN149", sqlParameterCollection, System.Data.CommandType.StoredProcedure);
                if (dtb_result != null)
                {
                    foreach (System.Data.DataRow linha in dtb_result.Rows)
                    {
                        Models.SITE.Cad_Cadastro cadastro = new Models.SITE.Cad_Cadastro();
                        //cadastro.id = int.Parse(linha["id"].ToString());
                        cadastro.NOME = linha["USU_NOME"].ToString();
                        cadastro.CPF = linha["USU_CPF"].ToString();
                        cadastro.CPFMASCK = CPFMASCA;
                        cadastro.EMAIL = linha["USU_EMAIL"].ToString();

                        Session["USU_Nome"] = cadastro.NOME;
                        Session["USU_CPF"] = cadastro.CPF;
                        Session["USUCPFMASCK"] = cadastro.CPFMASCK;
                        Session["USU_EMAIL"] = cadastro.EMAIL;
                        cadastro.PRP_STATUS = true;
                        _listCadastro.Add(cadastro);
                    }
                }
                else
                {
                    Models.SITE.Cad_Cadastro cadastro = new Models.SITE.Cad_Cadastro();
                    cadastro.PRP_STATUS = false;
                    cadastro.PRP_MENSAGEM = "Usuário já cadastrado ou não encontrado, verifique o CPF digitado".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Info);
                    _listCadastro.Add(cadastro);
                }
            }
            catch (Exception ex)
            {
                Models.SITE.Cad_Cadastro cadastro = new Models.SITE.Cad_Cadastro();
                cadastro.PRP_STATUS = false;
                cadastro.PRP_MENSAGEM = "Falha na Requisição , tente novamente em instantes".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);
                _listCadastro.Add(cadastro);
            }
            return Json(MollaLibrary.Web.JsonUtil.Serialize(_listCadastro));
        }

        [HttpPost]
        public ActionResult MTD_Cadastra(string Celular, string senha)
        {
            MollaLibrary.COMMON.RetornoRequisicao retornoRequisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            string st_json = "";
            string senhaCripto = Models.COMMON.values.Criptografar(senha.Trim());
            try
            {
                MollaLibrary.DataSource.MicrosoftSqlServer sqlServer = new MollaLibrary.DataSource.MicrosoftSqlServer(Models.COMMON.values.PRP_StringConexao);
                System.Data.SqlClient.SqlParameterCollection sqlParameterCollection = sqlServer.InicializaSqlParameterCollection;
                sqlParameterCollection.Add("@Celular", System.Data.SqlDbType.VarChar).Value = Celular;
                sqlParameterCollection.Add("@Senha", System.Data.SqlDbType.VarChar).Value = senhaCripto;
                sqlParameterCollection.Add("@CPF", System.Data.SqlDbType.VarChar).Value = Session["USU_CPF"];

                int dtb_result = sqlServer.DbExecuteNonQuery("sp_site_CadastroSAN149", sqlParameterCollection, System.Data.CommandType.StoredProcedure);
                if (dtb_result > 0)
                {
                    retornoRequisicao.PRP_Status = true;
                    retornoRequisicao.PRP_Mensagem = "Cadastro Realizado com sucesso!".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Success);
                }
                else
                {
                    retornoRequisicao.PRP_Status = false;
                    retornoRequisicao.PRP_Mensagem = "Erro ao cadastrar, tente novamente em instantes!".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(MollaLibrary.Web.JsonUtil.Serialize(retornoRequisicao));
        }

        [HttpPost]
        public ActionResult MTD_AcessoLogin(string CPF, string Senha)
        {
            List<Models.SITE.Cad_Cadastro> _ListCadLogin = new List<Models.SITE.Cad_Cadastro>();
            string senhaCripto = Models.COMMON.values.Criptografar(Senha.Trim());
            try
            {
                MollaLibrary.DataSource.MicrosoftSqlServer sqlServer = new MollaLibrary.DataSource.MicrosoftSqlServer(Models.COMMON.values.PRP_StringConexao);
                System.Data.SqlClient.SqlParameterCollection sqlParameterCollection = sqlServer.InicializaSqlParameterCollection;
                sqlParameterCollection.Add("@CPF", System.Data.SqlDbType.VarChar).Value = CPF.MTD_ApenasNumeros();
                sqlParameterCollection.Add("@Senha", System.Data.SqlDbType.VarChar).Value = senhaCripto;
                System.Data.DataTable dtb_result = sqlServer.DbExecute("sp_site_ValidaLogin", sqlParameterCollection, System.Data.CommandType.StoredProcedure);
                if (dtb_result != null)
                {
                    foreach (System.Data.DataRow linha in dtb_result.Rows)
                    {
                        Models.SITE.Cad_Cadastro CadLogin = new Models.SITE.Cad_Cadastro();
                        //cadastro.id = int.Parse(linha["id"].ToString());
                        CadLogin.NOME = linha["USU_NOME"].ToString();
                        CadLogin.CPF = linha["USU_CPF"].ToString();
                        CadLogin.EMAIL = linha["USU_EMAIL"].ToString();
                        CadLogin.CELULAR = linha["USU_CELULAR"].ToString();
                        CadLogin.BU = linha["USU_BU"].ToString();

                        Session["USU_Nome"] = CadLogin.NOME;
                        Session["USU_CPF"] = CadLogin.CPF;
                        Session["USU_EMAIL"] = CadLogin.EMAIL;
                        Session["USU_BU"] = CadLogin.BU;
                        CadLogin.PRP_STATUS = true;
                        _ListCadLogin.Add(CadLogin);
                    }
                }
                else
                {
                    Models.SITE.Cad_Cadastro CadLogin = new Models.SITE.Cad_Cadastro();
                    CadLogin.PRP_STATUS = false;
                    CadLogin.PRP_MENSAGEM = "Não foi localizado seu acesso, verifique novamente".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Alert);
                    _ListCadLogin.Add(CadLogin);
                }
            }
            catch (Exception ex)
            {
                Models.SITE.Cad_Cadastro CadLogin = new Models.SITE.Cad_Cadastro();
                CadLogin.PRP_STATUS = false;
                CadLogin.PRP_MENSAGEM = "Erro ao processar requisição".MTD_MensagemHTML(MollaLibrary.EnunsApp.enum_TipoMensagem.Danger);
                _ListCadLogin.Add(CadLogin);
            }
            return Json(MollaLibrary.Web.JsonUtil.Serialize(_ListCadLogin));
        }

        [HttpPost]
        public JsonResult MTD_EsqueciSenha(string pCpf) => Json(MollaLibrary.Web.JsonUtil.Serialize(new LoginService().MTD_EsqueciSenhaEnvio(pCpf)));
    }
}
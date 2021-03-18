using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao.Models.ENTITY;
using Aplicacao.Models.SITE.Login;
using MollaLibrary.COMMON;
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
        [HttpPost]
        public JsonResult MTD_BuscaCadastro(string pCPF)
        {
            Usuario retornoUsuario = new LoginService().MTD_GetCadastro(pCPF.MTD_ApenasNumeros());

            return Json(retornoUsuario, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MTD_Cadastra(string pCelular, string pSenha, int pIdentificador)
        {
            RetornoRequisicao ret = new LoginService().MTD_RealizarCadastro(pCelular, pSenha, pIdentificador);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MTD_AcessoLogin(string pCpf, string pSenha) => Json(MollaLibrary.Web.JsonUtil.Serialize(new LoginService().MTD_Autenticacao(pCpf, pSenha, out bool pAcessoFake)));

        public JsonResult MTD_EsqueciSenha(string pCpf) => Json(Newtonsoft.Json.JsonConvert.SerializeObject(new LoginService().MTD_EsqueciSenhaEnvio(pCpf.MTD_ApenasNumeros())));

        public ActionResult EsqueciSenha(string chave)
        {
            if (TempData["PPR_Requisicao"] == null)
            {
                MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
                try
                {
                    string st_Chave = chave.MTD_CriptografiaReversivel(MollaLibrary.EnunsApp.en_Criptografia.Decriptar);
                    Models.SITE.Login.EsqueciSenha Dados = MollaLibrary.Web.JsonUtil.ConvertToObject<Models.SITE.Login.EsqueciSenha>(st_Chave);
                    DateTime dt = DateTime.Parse(Dados.PRP_DataSolicitacao);
                    TimeSpan ts = DateTime.Now.MTD_DataHoraBrasil() - dt;
                    MollaLibrary.Web.SessionVar.Set<Models.SITE.Login.EsqueciSenha>("EsqueciSenhaDados", Dados);
                    if (ts.TotalHours > Dados.PRP_HorasValidade)
                    {
                        requisicao.PRP_Status = false;
                        requisicao.PRP_Mensagem = "A validade da solicitação expirou, por favor refaça o processo e tente novamente dentro do periodo indicado";
                        requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                    }
                    else
                    {
                        requisicao.PRP_Status = true;
                    }
                }
                catch (Exception ex)
                {
                    requisicao.PRP_Mensagem = "A chave de segurança foi corrompida, por favor solicite a alteração da senha novamente";
                    requisicao.PRP_Status = false;
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;
                }

                TempData["PPR_Requisicao"] = requisicao;
            }


            return View();
        }

        public JsonResult AlterarSenha(string pNovaSenha)
        {
            MollaLibrary.COMMON.RetornoRequisicao requisicao = new MollaLibrary.COMMON.RetornoRequisicao();
            try
            {
                if (Session["EsqueciSenhaDados"] == null)
                {
                    requisicao.PRP_Mensagem = "Sua sessão expirou, por favor refaça o processo.";
                    requisicao.PRP_Status = false;
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Info;
                }
                else
                {
                    Models.SITE.Login.EsqueciSenha Dados = MollaLibrary.Web.SessionVar.Get<Models.SITE.Login.EsqueciSenha>("EsqueciSenhaDados");
                    string st_Senha = pNovaSenha.MTD_CriptografiaIrreversivel();

                    db_SAN149Entities EF = new Models.ENTITY.db_SAN149Entities();

                    LoginService metodos = new LoginService();

                    var usuario = EF.USU_Usuario.FirstOrDefault(x => x.USU_ID == Dados.PRP_UsuarioID);

                    usuario.USU_Senha = pNovaSenha.MTD_CriptografiaIrreversivel();
                    usuario.USU_DataAlteracao = DateTime.Now.MTD_DataHoraBrasil();
                    EF.SaveChanges();

                    requisicao.PRP_Mensagem = "Senha alterada com sucesso";
                    requisicao.PRP_Status = true;
                    requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Success;
                }
            }
            catch (Exception ex)
            {
                requisicao.PRP_Mensagem = "Falha ao realizar a requisição";
                requisicao.PRP_Status = false;
                requisicao.PRP_TipoMensagem = MollaLibrary.EnunsApp.enum_TipoMensagem.Danger;
            }

            TempData["PPR_Requisicao"] = requisicao;
            return Json(requisicao, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MTD_Contato(Contato pContato) => Json(MollaLibrary.Web.JsonUtil.Serialize(new LoginService().MTD_Contato(pContato)));
        [HttpPost]
        public JsonResult MTD_ContatoUsuario(int pIdUsuario, string pMensagem) => Json(MollaLibrary.Web.JsonUtil.Serialize(new LoginService().MTD_ContatoUsuario(pIdUsuario, pMensagem)));
        [HttpPost]
        public JsonResult MTD_SalvarPerfil(string PRP_Documento, string PRP_Nome, string PRP_Telefone,  string PRP_Email, string PRP_Senha) => Json(MollaLibrary.Web.JsonUtil.Serialize(new LoginService().MTD_AtualizarDados(PRP_Documento, PRP_Nome, PRP_Telefone, PRP_Email, PRP_Senha)));
    }
}
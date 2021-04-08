using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aplicacao.Models.COMMON;
using Aplicacao.Models.ENTITY;
using MollaLibrary;
using MollaLibrary.COMMON;
using MollaLibrary.ExtendMethods;

namespace Aplicacao.Models.SITE.Desempenho
{
    public class DesempenhoService
    {
        private db_SAN149Entities EF = new db_SAN149Entities();
        public RetornoDesempenho MTD_RetornoDesempenho()
        {
            int idUsuario = values.PRP_UsuarioAutenticadoSite.PRP_IdUsuario;
            RetornoDesempenho ret = new RetornoDesempenho();
            try
            {
                DSP_Desemempenho desempenhoUsuario = new DSP_Desemempenho();
                if (values.PRP_UsuarioFake != null)
                {
                    desempenhoUsuario = EF.DSP_Desemempenho.FirstOrDefault(x => x.USU_ID == idUsuario && x.DSP_Ativo == true);
                }
                else
                {
                    desempenhoUsuario = EF.DSP_Desemempenho.FirstOrDefault(x => x.USU_ID == idUsuario && x.DSP_Ativo == true && x.DSP_Ambiente == "P");
                }
                
                if (desempenhoUsuario != null)
                {
                    ret.OBJ_Desempenho.PRP_Nome = desempenhoUsuario.USU_Usuario.USU_Nome;
                    ret.OBJ_Desempenho.PRP_CodSac = desempenhoUsuario.USU_Usuario.USU_CodSac;
                    ret.OBJ_Desempenho.PRP_GrupoCompetidor = desempenhoUsuario.DSP_GrupoCompetidor;
                    ret.OBJ_Desempenho.PRP_Objetivo = desempenhoUsuario.DSP_Objetivo;
                    ret.OBJ_Desempenho.PRP_PayOut = desempenhoUsuario.DSP_PayOut;
                    ret.OBJ_Desempenho.PRP_PisicaoRanking = desempenhoUsuario.DSP_PosicaoRanking;
                    ret.OBJ_Desempenho.PRP_PontosExec = desempenhoUsuario.DSP_Pontos;
                    ret.OBJ_Desempenho.PRP_PontosPayOut = desempenhoUsuario.DSP_PontosPayOut;
                    ret.OBJ_Desempenho.PRP_Pontostotais = desempenhoUsuario.DSP_PontosTotais;
                    ret.OBJ_Desempenho.PRP_Realizado = desempenhoUsuario.DSP_Realizado;
                    ret.OBJ_Desempenho.PRP_Target = (desempenhoUsuario.DSP_Target == true ? "SIM" : "NÃO");

                    ret.PRP_RetornoRequisicao.PRP_Status = true;
                    ret.PRP_RetornoRequisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success;
                    ret.PRP_RetornoRequisicao.PRP_Mensagem = "Dados carregados.";
                }
                else
                {
                    ret.PRP_RetornoRequisicao.PRP_Status = false;
                    ret.PRP_RetornoRequisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Info;
                    ret.PRP_RetornoRequisicao.PRP_Mensagem = "Nenhum dado de desempenho foi encontrado para este usuário.";
                }              
            }
            catch (Exception)
            {
                ret.PRP_RetornoRequisicao.PRP_Status = false;
                ret.PRP_RetornoRequisicao.PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger;
                ret.PRP_RetornoRequisicao.PRP_Mensagem = "Erro ao processar a requisição.";
            }
            return ret;
        }
    }
}
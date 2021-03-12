using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aplicacao.Models.ENTITY;
using MollaLibrary;
using MollaLibrary.COMMON;
using MollaLibrary.ExtendMethods;

namespace Aplicacao.Models.SITE.Desempenho
{
    public class DesempenhoService
    {
        db_SAN149Entities EF = new db_SAN149Entities();

        //public DesempenhoRetorno GetDesempenho(string documentoUsuario)
        //{
        //    var desempenhoRetorno = new DesempenhoRetorno();
        //    try
        //    {
        //        var fases = EF.DSF_DesempenhoFase.ToList();

        //        if (fases.Count > 0)
        //        {
        //            foreach (var fase in fases)
        //            {
        //                desempenhoRetorno.Fases.Add(new DesempenhoFase
        //                {
        //                    Fase = fase.DSF_Fase,
        //                    Desempenhos = PreencheDesempenhos(documentoUsuario, fase)
        //                });
        //            }
                    
        //            desempenhoRetorno.RetornoRequisicao = new RetornoRequisicao
        //            {
        //                PRP_Status = true,
        //                PRP_Mensagem = "Sucesso",
        //                PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Success
        //            };

        //        }
        //        else
        //        {
        //            desempenhoRetorno.RetornoRequisicao = new RetornoRequisicao
        //            {
        //                PRP_Status = false,
        //                PRP_Mensagem = "Não foram encontrados dados de desempenho. Caso haja algo de errado, entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>"
        //                    .MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert),
        //                PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert
        //            };
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        desempenhoRetorno.RetornoRequisicao = new RetornoRequisicao
        //        {
        //            PRP_Status = false,
        //            PRP_Mensagem = "Não foi possível processar a requisição. Em caso de persistência, entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>"
        //                .MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger),
        //            PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger
        //        };
        //    }

        //    return desempenhoRetorno;
        //}

        //private List<Desempenho> PreencheDesempenhos(string documentoUsuario, DSF_DesempenhoFase fase)
        //{

        //    var desempenhoRetorno = new List<Desempenho>();

        //    var usuario = EF.USU_Usuario.FirstOrDefault(u => u.USU_CPF.Equals(documentoUsuario));
        //    var kpis = EF.DSK_DesempenhoKpi.ToList();

        //    foreach (var kpi in kpis)
        //    {
        //        var desempenhoUsuario = EF.DES_Desempenho
        //            .Where(d => d.USU_ID == usuario.USU_ID)
        //            .Where(d => d.DSF_ID == fase.DSF_ID)
        //            .Where(d => d.DSK_ID == kpi.DSK_ID)
        //            .ToList();

        //        if (desempenhoUsuario.Count > 0)
        //        {
        //            desempenhoRetorno.Add(new Desempenho
        //            {
        //                Kpi = kpi.DSK_Display,
        //                DesempenhoDetalhes = PreencheDesempenho(desempenhoUsuario)
        //            });
        //        }

        //    }

        //    return desempenhoRetorno;
        //}

        //private List<DesempenhoDetalhe> PreencheDesempenho(List<DES_Desempenho> desempenhos)
        //{
        //    var desempenhoDetalhes = new List<DesempenhoDetalhe>();
        //    foreach (var desempenho in desempenhos)
        //    {
        //        var detalhe = new DesempenhoDetalhe
        //        {
        //            Descricao = desempenho.DSD_DesempenhoDescricao.DSD_Display,
        //            Prefixo = desempenho.DSD_DesempenhoDescricao.DSD_Prefixo,
        //            Sufixo = desempenho.DSD_DesempenhoDescricao.DSD_Sufixo,
        //            Valor = desempenho.DES_Valor
        //        };

        //        desempenhoDetalhes.Add(detalhe);
        //    }

        //    return desempenhoDetalhes;
        //}
    }
}
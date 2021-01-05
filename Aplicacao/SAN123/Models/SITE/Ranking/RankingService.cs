using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aplicacao.Models.ENTITY;
using MollaLibrary;
using MollaLibrary.COMMON;
using MollaLibrary.ExtendMethods;

namespace Aplicacao.Models.SITE.Ranking
{
    public class RankingService
    {
        SAN123_Entities EF = new SAN123_Entities();

        public RankingRetorno GetRanking(string documentoUsuario)
        {
            var rankingRetorno = new RankingRetorno();
            try
            {
                var usuario = EF.USU_Usuario.FirstOrDefault(u => u.USU_CPF.Equals(documentoUsuario));
                var rankingUsuario = EF.RKG_RankingGrupo.FirstOrDefault(r => r.USU_ID == usuario.USU_ID);

                if (rankingUsuario != null && rankingUsuario.RKG_Posicao != 0)
                {
                    var classificacao = rankingUsuario.RKG_Posicao;

                    if (classificacao > 2)
                    {
                        classificacao -= 2; //Classificação = 10, então classificação = 8 
                    }
                    else if (classificacao == 2)
                    {
                        classificacao -= 1; //Classificação = 2, então classificação = 1
                    }

                    var listaRanking = EF.RKG_RankingGrupo
                        .Where(r => r.RKG_Posicao >= classificacao)
                        .Where(r => r.GRC_ID == rankingUsuario.GRC_ID)
                        .Take(5)
                        .ToList();

                    foreach (var ranking in listaRanking)
                    {
                        var pontuacao = EF.DES_Desempenho
                            .Where(d => d.DSD_ID == 6)//Id de Pontuação
                            .FirstOrDefault(d => d.USU_ID == ranking.USU_Usuario.USU_ID);

                        rankingRetorno.Ranking.Add(new Ranking
                        {
                            Posicao = ranking.RKG_Posicao,
                            Documento = ranking.USU_Usuario.USU_CPF,
                            Nome = ranking.USU_Usuario.USU_Nome.Split(' ')[0],
                            Pontuacao = (int)pontuacao.DES_Valor,
                            CriterioDesempate = ranking.RKG_CriterioDesempate
                        });
                    }

                    rankingRetorno.RetornoRequisicao = new RetornoRequisicao{PRP_Status = true};

                }
                else if (rankingUsuario != null && rankingUsuario.RKG_Posicao == 0)
                {
                    rankingRetorno.RetornoRequisicao = new RetornoRequisicao
                    {
                        PRP_Status = false,
                        PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert,
                        PRP_Mensagem = "Seu ranking não pôde ser exibido. Caso haja algo de errado, entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>"
                            .MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert),

                    };
                }
                else
                {
                    rankingRetorno.RetornoRequisicao = new RetornoRequisicao
                    {
                        PRP_Status = false,
                        PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Alert,
                        PRP_Mensagem = "Seu ranking não foi encontrado. Caso haja algo de errado, entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>"
                            .MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Alert)
                    };
                }
            }
            catch (Exception e)
            {
                rankingRetorno.RetornoRequisicao = new RetornoRequisicao
                {
                    PRP_Status = false,
                    PRP_Mensagem = "Não foi possível processar a requisição. Em caso de persistência, entre em contato conosco <a class='text-uppercase cur-pointer font-weight-bold' data-toggle='modal' data-target='#modal-contato' data-dismiss='modal'><u>clicando aqui</u></a>"
                        .MTD_MensagemHTML(EnunsApp.enum_TipoMensagem.Danger),
                    PRP_TipoMensagem = EnunsApp.enum_TipoMensagem.Danger
                };
            }

            return rankingRetorno;
        }
    }
}
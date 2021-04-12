$(document).ready(function() {
    fnGetDesempenho();
});


function fnGetDesempenho() {
    $.ajax({
        type: 'POST',
        url: '/Desempenho/MTD_Desempenho',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        beforeSend: function () {

        },
        success: function (ret) {
            if (ret.PRP_RetornoRequisicao.PRP_Status == true) {
                fnPreencherDesempenho(ret.OBJ_Desempenho);
            }
            else {
                fnExibeMensagemRetorno(ret.PRP_RetornoRequisicao)
            }
        }
    });
}

function fnPreencherDesempenho(dadosDesempenho) {
    var linha = "<td>" + dadosDesempenho.PRP_GrupoCompetidor + "</td>" +
                "<td>" + dadosDesempenho.PRP_CodSac + " </td>" +
                //"<td>" + dadosDesempenho.PRP_Target + "</td>" +
                "<td>" + dadosDesempenho.PRP_Nome + " </td>" +
                "<td>" + dadosDesempenho.PRP_PayOut + "%</td>" +
                "<td>" + dadosDesempenho.PRP_PontosPayOut + "</td>" +
                "<td>" + dadosDesempenho.PRP_Objetivo + "%</td> " +
                "<td>" + dadosDesempenho.PRP_Realizado + "%</td>" +
                "<td>" + dadosDesempenho.PRP_PontosExec + "</td>" +
                "<td>" + dadosDesempenho.PRP_Pontostotais + "</td> " +
                "<td>" + dadosDesempenho.PRP_PisicaoRanking + "</td>";
    $('#dadosDesempenhi').empty();
    $('#dadosDesempenhi').append(linha);
}
function fnExibeMensagemRetorno(retorno) {
    var linha = "<td colspan='11'>" + retorno.PRP_Mensagem + "</td>";
    $('#dadosDesempenhi').empty();
    $('#dadosDesempenhi').append(linha);
}

//function getDesempenho() {

//    var param = {}

//    param.documentoUsuario = $('#documento-usuario').val();

//    $.ajax({
//        type: 'GET',
//        url: '/Desempenho/GetDesempenho',
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        data: param,
//        beforeSend: function () {
            
//        },
//        success: function (retorno) {

//            if (retorno.RetornoRequisicao.PRP_Status) {

//                preencheFase(retorno.Fases);

//            } else {
//                exibeMensagemRetorno(retorno.RetornoRequisicao);
//            }

//        },
//        complete: function () {
//        }
//    });

//}

//function preencheFase(fases) {

//    fases.forEach(function (fase) {
        
//        $(`#fase-${fase.Fase.toLowerCase()}`).removeClass('user-iteration-none');
//        preencheTabelaDesempenho(fase.Desempenhos, fase.Fase.toLowerCase());

//    });

//}

//function preencheTabelaDesempenho(desempenhos, fase) {

//    desempenhos.forEach(function(desempenho) {

//        var tableHeader = '<thead>' +
//            '<tr>' +
//            '<th>KPI</th>';

//        var tableBody = '<tbody>' +
//            '<tr>' +
//            `<td>${desempenho.Kpi}</td>`;

//        desempenho.DesempenhoDetalhes.forEach(function(detalhes) {

//            tableHeader += `<th>${detalhes.Descricao}</th>`;

//            if (detalhes.Prefixo || detalhes.Sufixo) {
//                tableBody += `<td>${detalhes.Prefixo ? detalhes.Prefixo + " " + numberToReal(detalhes.Valor) : ""}
//                ${detalhes.Sufixo ? (detalhes.Valor * 100) + detalhes.Sufixo : ""}</td>`;
//            } else {
//                tableBody += `<td>${detalhes.Valor}</td>`;
//            }
            

//        });

//        tableHeader += '</tr>' +
//            '</thead>';

//        tableBody += '</tr>' +
//            '</tbody>';

//        $(`#tabela-desempenho-${fase}`).empty();
//        $(`#tabela-desempenho-${fase}`).append(tableHeader);
//        $(`#tabela-desempenho-${fase}`).append(tableBody);

//    });

//}
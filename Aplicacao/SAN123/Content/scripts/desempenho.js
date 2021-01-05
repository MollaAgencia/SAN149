$(document).ready(function() {
    getDesempenho();
});

function getDesempenho() {

    var param = {}

    param.documentoUsuario = $('#documento-usuario').val();

    $.ajax({
        type: 'GET',
        url: '/Desempenho/GetDesempenho',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: param,
        beforeSend: function () {
            
        },
        success: function (retorno) {

            if (retorno.RetornoRequisicao.PRP_Status) {

                preencheFase(retorno.Fases);

            } else {
                exibeMensagemRetorno(retorno.RetornoRequisicao);
            }

        },
        complete: function () {
        }
    });

}

function preencheFase(fases) {

    fases.forEach(function (fase) {
        
        $(`#fase-${fase.Fase.toLowerCase()}`).removeClass('user-iteration-none');
        preencheTabelaDesempenho(fase.Desempenhos, fase.Fase.toLowerCase());

    });

}

function preencheTabelaDesempenho(desempenhos, fase) {

    desempenhos.forEach(function(desempenho) {

        var tableHeader = '<thead>' +
            '<tr>' +
            '<th>KPI</th>';

        var tableBody = '<tbody>' +
            '<tr>' +
            `<td>${desempenho.Kpi}</td>`;

        desempenho.DesempenhoDetalhes.forEach(function(detalhes) {

            tableHeader += `<th>${detalhes.Descricao}</th>`;

            if (detalhes.Prefixo || detalhes.Sufixo) {
                tableBody += `<td>${detalhes.Prefixo ? detalhes.Prefixo + " " + numberToReal(detalhes.Valor) : ""}
                ${detalhes.Sufixo ? (detalhes.Valor * 100) + detalhes.Sufixo : ""}</td>`;
            } else {
                tableBody += `<td>${detalhes.Valor}</td>`;
            }
            

        });

        tableHeader += '</tr>' +
            '</thead>';

        tableBody += '</tr>' +
            '</tbody>';

        $(`#tabela-desempenho-${fase}`).empty();
        $(`#tabela-desempenho-${fase}`).append(tableHeader);
        $(`#tabela-desempenho-${fase}`).append(tableBody);

    });

}

function numberToReal(numero) {
    console.log(numero);
    numero = numero.toFixed(2).split('.');
    console.log(numero);
    numero[0] = numero[0].split(/(?=(?:...)*$)/).join('.');
    console.log(numero);
    return numero.join(',');
}

function exibeMensagemRetorno(retornoRequisicao) {
    console.log(retornoRequisicao);
    var tabelaMensagem =
        '<tbody>' +
            '<tr>' +
            '<td class="pt-4 mb-0">' +
        '<div class="d-flex justify-content-center va-middle mb-0">' +
            retornoRequisicao.PRP_Mensagem +
            '</div>' +
            '</td>' +
            '</tr>' +
        '</tbody>';

    $(`#tabela-desempenho-${fase}`).empty();
    $(`#tabela-desempenho-${fase}`).append(tabelaMensagem);
}
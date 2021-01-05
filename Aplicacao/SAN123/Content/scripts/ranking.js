$(document).ready(function () {
    getRanking();
});

function getRanking() {

    var param = {}

    param.documentoUsuario = $('#documento-usuario').val();

    $.ajax({
        type: 'GET',
        url: '/Ranking/GetRanking',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: param,
        beforeSend: function () {

        },
        success: function (retorno) {

            if (retorno.RetornoRequisicao.PRP_Status) {

                preencheRanking(retorno.Ranking, param.documentoUsuario);

            } else {
                exibeMensagemRetorno(retorno.RetornoRequisicao);
            }

        },
        complete: function () {
        }
    });

}

function preencheRanking(listaRanking, documentoUsuario) {
    //TODO: adaptar o retorno quando houver ranking anual

    $(`#tabela-ranking-semestral`).empty();

    listaRanking.forEach(function (ranking) {

        var linhaTabela = `<tr ${documentoUsuario === ranking.Documento ? "class='bg-warning'" : ""}>` +
            `<td>${ranking.Posicao} º</td>` +
            `<td>${ranking.Nome}</td>` +
            `<td>${ranking.Pontuacao}</td>` +
            `<td>${numberToReal(ranking.CriterioDesempate)} %</td>` +
            '</tr>';

        $(`#tabela-ranking-semestral`).append(linhaTabela);
    });
}

function exibeMensagemRetorno(retornoRequisicao) {

    //TODO: adaptar o retorno quando houver ranking anual
    var tabelaMensagem =
            '<tr>' +
            '<td class="pt-4 mb-0">' +
            '<div class="d-flex justify-content-center va-middle mb-0">' +
            retornoRequisicao.PRP_Mensagem +
            '</div>' +
            '</td>' +
            '</tr>';

    $(`#tabela-ranking-semestral`).empty();
    $(`#tabela-ranking-semestral`).append(tabelaMensagem);
}

function numberToReal(numero) {
    numero = numero * 100;
    numero = numero.toFixed(2).split('.');
    numero[0] = numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}
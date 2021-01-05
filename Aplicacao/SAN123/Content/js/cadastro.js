$(document).ready(function () {
});



$(document).on('click', '#btnlogin_Cadastro', function (event) {
    var stb_html = "";
    var parametros = {};
    parametros.CPF = $('#cadastrocpf').val();

    $.ajax({
        type: 'POST',
        url: '/Login/MTD_CadastroParticipante',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('#btnlogin_Cadastro').addClass('d-none');
            $('#btnlogin_Cadastro').removeClass('d-none').html('<span class="py-2"><i class="fa fa-1x fa-spinner fa-spin"></i> Acessando...</span>');
            $('#MontaHTMLCadastro').addClass('d-none');
        },
        success: function (returnValue) {
            var jsonResult = JSON.parse(returnValue);
            if (jsonResult[0].PRP_STATUS == false) {
                stb_html += jsonResult[0].PRP_MENSAGEM;
                $('#MontaHTMLCadastro').removeClass('d-none');
                $('#btnlogin_Cadastro').addClass('d-none');
                $('#btnlogin_Cadastro').removeClass('d-none').html('<span class="py-2">CADASTRE-SE AQUI</span>');
            } else {
                $('#cadas-CPF_').val(jsonResult[0].CPFMASCK);
                $('#cadas-nome').val(jsonResult[0].NOME);
                $('#cadas-email').val(jsonResult[0].EMAIL);

                $('#cadastroModal').modal("show");
            }
            $('#MontaHTMLCadastro').html(stb_html);

        }
    });
});

$(document).on('click', '#btn_cadastrar_MTD', function (event) {

    var Celular = $('#cadas-celular').val();
    var Senha = $('#cadas-senha').val();
    var confi = $('#cadas-conf-senha').val();
    var parametros = {};
    if (Senha == confi) {
        parametros = { Celular, senha };
        $.ajax({
            type: 'POST',
            url: '/Login/MTD_Cadastra',
            data: JSON.stringify(parametros),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            beforeSend: function () {
                $('#btn_cadastrar_MTD').addClass('d-none');
                $('#btn_cadastrar_MTD').removeClass('d-none').html('<span class="py-2"><i class="fa fa-1x fa-spinner fa-spin"></i> Cadastrando...</span>');
                $('#msg_informativa').addClass('d-none');
            },
            success: function (returnValue) {
                var jsonResult = JSON.parse(returnValue);
                if (jsonResult.PRP_Status == true) {
                    $('#cadastroModal').modal("hide");
                    Swal.fire(
                        'Cadastro não efetuado!',
                        jsonResult.PRP_Mensagem,
                        'info',
                    ).then(function () {
                        location.href = '/Conteudo/Home';
                    });

                } else if (jsonResult.PRP_STATUS == false) {
                    $('#msg_informativa').removeClass('d-none');
                    $('#msg_informativa').val(jsonResult.PRP_Mensagem);
                }
            }
        });

    } else {
        $('#msg_informativa').html('<div class="alert alert-info text-center">As Senhas não conferem, digite novamente antes de continuar com o cadastro</div>');
    }
});


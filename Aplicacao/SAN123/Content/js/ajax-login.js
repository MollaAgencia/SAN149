
/* --- AJAX Esqueci a senha --- */
$('#btn-esqueci-senha').click(function () {
    var param = {}
    param.pCpf = $('#ipt-cpf').val();
    if (param.pCpf) {
        $.ajax({
            type: 'POST',
            url: '/Login/MTD_EsqueciSenha',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(param),
            beforeSend: function () {
                $('#btn-esqueci-senha').addClass('d-none');
                $('#alert-esqueci-senha')
                    .removeClass('d-none')
                    .html("<span class='alert alert-default w-100 text-center'><i class='fa fa-1x fa-spinner fa-spin'></i> Processando... </span>");
            },
            success: function (returnValue) {

                var retorno = JSON.parse(returnValue);

                $("#alert-esqueci-senha").removeClass("d-none").html(retorno.PRP_Mensagem);
            },
            complete: function () {
            }
        });
    } else {
        $("#alert-esqueci-senha")
            .removeClass("d-none")
            .html("<span class='alert alert-warning'>Insira seu CPF para continuar</span>");
    }
});

$(document).on('click', '#btn_EsqueceuSenhaReset', function (event) {


    var parametros = {};
    parametros.pNovaSenha = $('#pnovaSenhaReset').val();
    parametros.confSenha = $('#confSenhaReset').val();

    if (parametros.pNovaSenha != parametros.confSenha) {
        $('#MensagemErro').html('<p class="text-center alert alert-info">Necessário que as senhas sejam iguais!</p>');
        return false;
    }

    $.ajax({
        type: 'POST',
        url: '/Login/AlterarSenha',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(parametros),
        beforeSend: function () {
            $('#btn_login_Reset').attr('disabled', 'disabled');
            $('#btn_login_Reset').html('<i class="fa fa-1x fa-spinner fa-spin"></i><span> Processando...</span>');
        },
        success: function (returnValue) {
            if (returnValue.PRP_Status == true) {
                $('#btn_login_Reset').addClass('d-none');
                swal.fire({
                    title: "Atenção",
                    text: "Senha alterada com sucesso!",
                    type: "success"
                }).then(function () {
                    location.href = '/Login/Autenticacao';
                });
            } else {
                swal.fire({
                    title: "Atenção",
                    text: "Falha em alterar sua senha, tente novamente em instantes!",
                    type: "info"
                }).then(function () {
                    location.href = '/Login/Autenticacao';
                });
            }

        }
    });

});


/* --- AJAX Contato --- */
$('#btn_contato').bind('click', function (event) {
    event.preventDefault();
    
    // Valida Nome
        if ($('#contato_nome').val().length <= 3) {
            $('#alert_contato').removeClass('d-none').html('<span class="alert alert-danger py-2">Nome incompleto.</span>');
            $('#contato_nome').addClass('border border-danger');
            return false;
        }else{
            $('#alert_contato').addClass('d-none');
            $('#contato_nome').removeClass('border border-danger');
            // return true;
        }

    // Valida E-mail
        usuario = $('#contato_email').val().substring(0, $('#contato_email').val().indexOf('@'));
        dominio = $('#contato_email').val().substring($('#contato_email').val().indexOf('@')+ 1, $('#contato_email').val().length);
        if ((usuario.length >=1) &&
            (dominio.length >=3) && 
            (usuario.search('@')==-1) && 
            (dominio.search('@')==-1) &&
            (usuario.search(' ')==-1) && 
            (dominio.search(' ')==-1) &&
            (dominio.search('.')!=-1) &&      
            (dominio.indexOf('.') >=1)&& 
            (dominio.lastIndexOf('.') < dominio.length - 1)) {
            $('#alert_contato').addClass('d-none');
            $('#contato_email').removeClass('border border-danger');
            // return true;
        }else{
            $('#alert_contato').removeClass('d-none').html('<span class="alert alert-danger py-2">E-mail inválido.</span>');
            $('#contato_email').addClass('border border-danger');
            return false;
        }

    // Valida Assunto
        if($('#contato_assunto').val().length <= 3){
            $('#alert_contato').removeClass('d-none').html('<span class="alert alert-danger py-2">Insira o Assunto.</span>');
            $('#contato_assunto').addClass('border border-danger');
            return false;
        }else{
            $('#alert_contato').addClass('d-none');
            $('#contato_assunto').removeClass('border border-danger');
            // return true;
        }

    // Validar mensagem / observação
        if ($('#contato_mensagem').val().length <= 3) {
            $('#alert_contato').removeClass('d-none').html('<span class="alert alert-danger py-2">Deixe sua mensagem.</span>');
            $('#contato_mensagem').addClass('border border-danger');
            return false;
        }else{
            $('#alert_contato').addClass('d-none');
            $('#contato_mensagem').removeClass('border border-danger');
            // return true;
        }

    var parametros = {};
    parametros.PRP_Nome = $('#contato_nome').val();
    parametros.PRP_Email = $('#contato_email').val();
    parametros.PRP_Assunto = $('#contato_assunto').val();
    parametros.PRP_Mensagem = $('#contato_mensagem').val();
    parametros.PRP_CodigoSac = $('#contato_codsac').val();

    $.ajax({
        type: 'POST',
        url: '/Login/MTD_Contato',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('#btn_contato').attr('disabled', 'disabled');
            $('#alert_contato').removeClass('d-none').html('<span class="alert alert-info py-2"><i class="fa fa-1x fa-spinner fa-spin"></i> Enviando...</span>');
        },
        success: function (returnValue) {
            var jsonResult = JSON.parse(returnValue);
            if (jsonResult.PRP_Status === true) {
                $('#alert_contato').removeClass('d-none').html(jsonResult.PRP_Mensagem);
                $('#btn_contato').removeAttr('disabled', 'disabled');
            }
            else {
                $('#alert_contato').removeClass('d-none').html(jsonResult.PRP_Mensagem);
                $('#btn_contato').removeAttr('disabled', 'disabled');
            }
        }
    });
});
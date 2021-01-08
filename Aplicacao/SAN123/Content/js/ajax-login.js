
/* --- AJAX Esqueci a senha --- */
$('#btn_EsqueciSenha').bind('click', function (event) {
    event.preventDefault();

    if ($('#ipt_cpf').val() == '') {
        $('#alert_EsqueciSenha').removeClass('d-none').html('<span class="alert alert-danger py-2">Preencha o seu CPF.</span>');
        return false;
    }
    
    // Validar CPF
        var cpf = $('#ipt_cpf').val();
        cpf = cpf.replace('.', '');
        cpf = cpf.replace('.', '');
        cpf = cpf.replace('-', '');

        if( cpf.length != 11 ||
            cpf == '00000000000' ||
            cpf == '11111111111' ||
            cpf == '22222222222' ||
            cpf == '33333333333' ||
            cpf == '44444444444' ||
            cpf == '55555555555' ||
            cpf == '66666666666' ||
            cpf == '77777777777' ||
            cpf == '88888888888' ||
            cpf == '99999999999' ){
            $('#alert_EsqueciSenha').removeClass('d-none').html('<span class="alert alert-danger py-2">CPF inválido.</span>');
            return false;
        }
        soma = 0;
        for(i = 0; i < 9; i++){
            soma += parseInt(cpf.charAt(i)) * (10 - i);
        }   
        resto = 11 - (soma % 11);
        if(resto == 10 || resto == 11){
            resto = 0;
        }
        if(resto != parseInt(cpf.charAt(9))){
            $('#alert_EsqueciSenha').removeClass('d-none').html('<span class="alert alert-danger py-2">CPF inválido.</span>');
            return false;
        }
        soma = 0;
        for(i = 0; i < 10; i ++){
            soma += parseInt(cpf.charAt(i)) * (11 - i);
        }
        resto = 11 - (soma % 11);
        if(resto == 10 || resto == 11){
            resto = 0;
        }   
        if(resto != parseInt(cpf.charAt(10))){
            $('#alert_EsqueciSenha').removeClass('d-none').html('<span class="alert alert-danger py-2">CPF inválido.</span>');
            return false;
        }else{
            $('#alert_EsqueciSenha').addClass('d-none');
            // return true;
        }

    var parametros = {};
    parametros.pCpf = $('#ipt_cpf').val();
    
    $.ajax({
        type: 'POST',
        url: '/Login/MTD_EsqueciSenha',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('#divEnviarEsqueciSenha').addClass('d-none');
            $('#alert_EsqueciSenha').removeClass('d-none').html('<span class="alert alert-info py-2"><i class="fa fa-1x fa-spinner fa-spin"></i> Enviando...</span>');
        },
        success: function (returnValue) {
            var jsonResult = JSON.parse(returnValue);
            if (jsonResult.PRP_Status == true) {
                $('#btn_EsqueciSenha').removeAttr('disabled', 'disabled');
                $('#alert_EsqueciSenha').removeClass('d-none').html(jsonResult.PRP_Mensagem);

                $('#divFecharEsqueciSenha').removeClass('d-none');
                $('#divEnviarEsqueciSenha').addClass('d-none');

            } else {
                $('#btn_EsqueciSenha').removeAttr('disabled', 'disabled');
                $('#alert_EsqueciSenha').removeClass('d-none').html('<p class="alert alert-warning py-2">'+jsonResult.PRP_Mensagem+'</p>');
            }
        }
    });
});
$('#btnFecharEsqueciSenha').bind('click', function() {
    $('#divFecharEsqueciSenha').addClass('d-none');
    $('#divEnviarEsqueciSenha').removeClass('d-none');
    $('#alert_EsqueciSenha').addClass('d-none');
});

$(document).on('click', '#btn_EsqueceuSenhaReset', function (event) {


    var parametros = {};
    parametros.senha = $('#pnovaSenhaReset').val();
    parametros.confSenha = $('#confSenhaReset').val();

    if (parametros.senha != parametros.confSenha) {
        $('#MensagemErro').html('<p class="text-center alert alert-info">Necessário que as senhas sejam iguais!</p>');
        return false;
    }

    $.ajax({
        type: 'POST',
        url: '/Login/MTD_AtualizaSenha',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(parametros),
        beforeSend: function () {
            $('#btn_login_Reset').attr('disabled', 'disabled');
            $('#btn_login_Reset').html('<i class="fa fa-1x fa-spinner fa-spin"></i><span> Processando...</span>');
        },
        success: function (returnValue) {
            var jsonResult = JSON.parse(returnValue);
            if (jsonResult.PRP_Status == true) {
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
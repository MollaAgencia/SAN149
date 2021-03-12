
/* --- Chamadas AJAX --- */{

    // Obter Usuário
    $("#cadas-cpf").keyup(function () {

        var cpf = $("#cadas-cpf").val();

        if (cpf.length === 14) {

            var param = {}

            param.pCpf = cpf;

            $.ajax({
                type: 'GET',
                url: '/Login/MTD_ObterUsuario',
                data: param,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    $("#busca_cadastro").removeClass("d-none");
                    $("#cadas-cpf").blur();
                    $("#cadas-cpf").prop('disabled', true);

                    $("#alert-cadastro").addClass("d-none");
                    $("#alert-cadastro").removeClass("alert");
                    $("#alert-cadastro").removeClass("alert-danger");
                },
                success: function (json) {

                    var retorno = JSON.parse(json);

                    if (retorno.PRP_RetornoRequisicao.PRP_Status) {

                        dados = retorno.PRP_Dados;

                        $("#lbl-cpf").text('CPF');
                        $("#cadas-nome").val(dados.PRP_Nome);
                        $("#cadas-email").val(dados.PRP_Email);
                        $("#cadas-celular").val(dados.PRP_Telefone);

                        $("#cadas-nome, #cadas-celular, #cadas-email").trigger('change'); //Chama a função Change para aplicar a máscara e validação

                        $("#div-campos-cadastro").removeClass('d-none');
                        $('#btn-cadastrar').removeClass('d-none');

                    } else {
                        $("#alert-cadastro")
                            .removeClass("d-none")
                            .html(retorno.PRP_RetornoRequisicao.PRP_Mensagem);

                        $("#cadas-cpf").prop('disabled', false);
                    }
                },
                error: function () {

                    $("#alert-cadastro")
                        .removeClass("d-none")
                        .html("<span class='alert alert-danger'>Erro na conexão com o Servidor. Tente novamente mais tarde.</span>");

                    $("#cadas-cpf").prop('disabled', false);

                },
                complete: function () {

                    $("#busca_cadastro").addClass("d-none");

                }
            });
        }

    });

    $("#btn-cadastrar").click(function () {

        if (ValidaVaziosCadastro()) {

            var param = {};
            param.PRP_Documento = $("#cadas-cpf").val();
            param.PRP_Nome = $("#cadas-nome").val();
            //param.PRP_Telefone = $("#cadas-celular").val();
            param.PRP_Email = $("#cadas-email").val();
            param.PRP_Senha = $("#cadas-senha").val();

            $.ajax({
                type: 'POST',
                url: '/Login/MTD_Cadasto',
                data: JSON.stringify(param),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    $("#alert-cadastro")
                        .removeClass("d-none")
                        .html("<span class='alert alert-info'><i class='fa fa-1x fa-spinner fa-spin'></i> Processando... </span>");
                    $("#cadas-nome").prop('disabled', true);
                    $("#cadas-email").prop('disabled', true);
                    $("#cadas-telefone").prop('disabled', true);
                    $("#cadas-senha").prop('disabled', true);
                    $("#cadas-conf-senha").prop('disabled', true);
                    $("#btn-cadastrar").prop('disabled', true);
                },
                success: function (json) {

                    var retorno = JSON.parse(json);

                    if (retorno.PRP_Status) {

                        $("#alert-cadastro")
                            .removeClass("d-none")
                            .html("<p class='alert alert-success'>Cadastro realizado com Sucesso! <br/> Feche a janela, e faça o login!</p>");

                    } else {
                        $("#alert-cadastro").removeClass("d-none").html(retorno.PRP_Mensagem);

                        $("#cadas-nome").prop('disabled', false);
                        $("#cadas-email").prop('disabled', false);
                        $("#cadas-telefone").prop('disabled', false);
                        $("#cadas-senha").prop('disabled', false);
                        $("#cadas-conf-senha").prop('disabled', false);
                        $("#btn-cadastrar").prop('disabled', false);
                    }
                },
                error: function () {

                    $("#alert-cadastro")
                        .removeClass("d-none")
                        .html("<span class='alert alert-danger'>Erro na conexão com o Servidor. Tente novamente mais tarde.</span>");

                    $("#cadas-nome").prop('disabled', false);
                    $("#cadas-email").prop('disabled', false);
                    $("#cadas-telefone").prop('disabled', false);
                    $("#cadas-senha").prop('disabled', false);
                    $("#cadas-conf-senha").prop('disabled', false);
                    $("#btn-cadastrar").prop('disabled', false);
                },
                complete: function () {

                }
            });

        } else {

            $("#alert-cadastro")
                .removeClass("d-none")
                .html("<span class='alert alert-warning d-flex'>Ainda existem campos inválidos ou vazios. Preencha-os corretamente para continuar</span>");

        }

    });

    $('#btn-login').click(function () {

        autenticar();

    });

    function autenticar(pLogin, pSenha) {

        if (($('#login-cpf').val() && $('#login-senha').val()) || (pLogin && pSenha)) {
            var param = {}
            param.pLogin = pLogin ? pLogin : $('#login-cpf').val();
            param.pSenha = pSenha ? pSenha : $('#login-senha').val();

            $.ajax({
                type: 'POST',
                url: '/Login/MTD_Autenticacao',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(param),
                beforeSend: function () {
                    $('#btn-login').addClass('d-none');
                    $('#alert-login')
                        .removeClass('d-none')
                        .html("<span class='alert alert-default w-100 text-center'><i class='fa fa-1x fa-spinner fa-spin'></i> Processando... </span>");
                },
                success: function (returnValue) {
                    var jsonResult = JSON.parse(returnValue);
                    if (jsonResult.PRP_Requisicao.PRP_Status == false) {
                        $('#alert-login').removeClass('d-none');
                        $('#alert-login').html(jsonResult.PRP_Requisicao.PRP_Mensagem);
                        $('#btn-login').text('Login');
                        $('#btn-login').removeClass('d-none');
                        $('#alerConfSenha').addClass('d-none');
                    }
                    else {
                        window.location.href = jsonResult.URL;
                    }
                },
                complete: function () {
                }
            });
        } else {

            $("#alert-login")
                .removeClass("d-none")
                .html("<span class='alert alert-warning'>Ainda existem campos inválidos ou vazios. Preencha-os corretamente para continuar</span>");

        }

    }
}/* --- Chamadas AJAX --- */

/* --- Manipulações do Formulário --- */{

    //Fecha e reseta o formulário de Esqueci Senha
    $('button[name="btn-fecha-esqueci-senha"]').click(function() {
        $("#ipt-cpf").val();
        $("#alert-esqueci-senha").addClass("d-none");
        $("#btn-esqueci-senha").removeClass("d-none");
    });

    //Fecha e reseta o formulário de Contato
    //$('button[name="btn-fecha-contato"]').click(function () {
    //    $("#contato_nome").val('');
    //    $("#contato_email").val('');
    //    $("#contato_assunto").val('');
    //    $("#contato_mensagem").val('');
    //    $("#alert_contato").addClass("d-none");
    //    $("#btn_enviarContato").removeClass("d-none");
    //    $(".alertError").addClass('d-none');
    //    $(this).prop('disabled', true);
    //    $('#div-campos-cadastro').prop('disabled', true);
    //});

    //Fecha o Formulário de cadastro
    $("#btn-fechar-cadastro, #btn-fechar-cadastro-modal").click(function () {

        formReset();

    });

    $('#cadastroModal').on('hide.bs.modal',
        function () {

            formReset();
        });

    //Reseta o Formulário
    function formReset() {

        //Retorna ao valor original
        $("#lbl-cpf").text('Insira seu CPF para realizar a busca');

        //Limpa os campos
        $("#cadas-cpf").prop('disabled', false);
        $("#cadas-cpf").val('');
        $("#cadas-nome").val('');
        $("#cadas-celular").val('');
        $("#cadas-email").val('');
        $("#cadas-nome-loja").val('');
        $("#cadas-codigo").val('');
        $("#cadas-senha").val('');
        $("#cadas-conf-senha").val('');

        //Remove classe de Erro do campo
        $("#cadas-cpf").removeClass('custom-is-invalid');
        $("#cadas-nome").removeClass('custom-is-invalid');
        $("#cadas-celular").removeClass('custom-is-invalid');
        $("#cadas-email").removeClass('custom-is-invalid');
        $("#cadas-nome-loja").removeClass('custom-is-invalid');
        $("#cadas-codigo").removeClass('custom-is-invalid');
        $("#cadas-senha").removeClass('custom-is-invalid');
        $("#cadas-conf-senha").removeClass('custom-is-invalid');

        //Mensagens de erro
        $(".alertError").addClass('d-none');
        $(".custom-invalid-feedback").addClass('d-none');

        //Arupamento de dados do Usuário
        $("#div-campos-cadastro").addClass('d-none');

        //Mensagem de alerta do Cadastro
        $("#alert-cadastro").addClass("d-none");

    }

    //Valida Campos Vazios
    function ValidaVaziosCadastro() {

        var validCpf = $("#cadas-cpf").prop('valid') ? $("#cadas-cpf").prop('valid') : false;
        var validNome = $("#cadas-nome").prop('valid') ? $("#cadas-nome").prop('valid') : false;
        //var validCelular = $("#cadas-celular").prop('valid') ? $("#cadas-celular").prop('valid') : false;
        var validEmail = $("#cadas-email").prop('valid') ? $("#cadas-email").prop('valid') : false;
        var validSenha = $("#cadas-senha").prop('valid') ? $("#cadas-senha").prop('valid') : false;
        var validConfSenha = $("#cadas-conf-senha").prop('valid') ? $("#cadas-conf-senha").prop('valid') : false;
        var validRegulamento = $("#regulamento").prop('valid') ? $("#regulamento").prop('valid') : false;


        return (validCpf &&
            validNome &&
            //validCelular &&
            validEmail &&
            validSenha &&
            validConfSenha &&
            validRegulamento);
    }

}/* --- Manipulações do Formulário --- */
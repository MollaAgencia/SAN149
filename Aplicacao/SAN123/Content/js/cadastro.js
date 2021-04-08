$(document).ready(function () {
    $('#cadas_Confirmarcelular').bind('paste', function (e) {
        e.preventDefault();
    });
    $('#cadas-conf-senha').bind('paste', function (e) {
        e.preventDefault();
    });
});



$(document).on('click', '#btnVerificarCadasro', function (event) {
    var stb_html = "";
    var parametros = {};
    parametros.pCPF = $('#cadastrocpf').val();
    if ($('#cadastrocpf').val() == "") { 
        $('#MontaHTMLCadastro').html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'>Informe o CPF para prosseguir</div>").removeClass('d-none');
        $('#cadastrocpf').focus();
        return false;
    }
    $.ajax({
        type: 'POST',
        url: '/Login/MTD_BuscaCadastro',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('#btnVerificarCadasro').addClass('d-none')
            $('#MontaHTMLCadastro').html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block'><i class='fa fa-1x fa-spinner fa-spin'></i> Verificando cadastro...</div>").removeClass('d-none');
        },
        success: function (ret) {
            if (ret.PRP_Requisicao.PRP_Status == true) {
                if (ret.OBJ_Usuario.PRP_PrimeiroAcesso == true) {
                    //USUÁRIO PRÉ CADASTRADO, EFETUANDO O PRIMEIRO ACESSO.
                    $('#cadas-CPF').val(ret.OBJ_Usuario.PRP_CPF);
                    $('#cadas-nome').val(ret.OBJ_Usuario.PRP_NomeUsuario);
                    $('#cadas-email').val(ret.OBJ_Usuario.PRP_EmailUsuario);
                    $('#hdnIdentificadorCadastro').val(ret.OBJ_Usuario.PRP_IdUsuario);
                    $('#cadas-unidadeNegocio').val(ret.OBJ_Usuario.PRP_NomeBU);
                    $('#primeiro-acesso').addClass('d-none');
                    $('#cadastroModal').modal('show');
                    $('#cadastroModalLabel').text("Complete o seu cadastro");
                }
                else if (ret.OBJ_Usuario.PRP_SenhaHasValue == false && ret.OBJ_Usuario.PRP_PrimeiroAcesso == false) {
                    //CADASTRO ATIVO IMPORTADO DE BASE ANTERIOR, NECESSÁRIO RENOVAR SENHA
                    $('#cadas-CPF').val(ret.OBJ_Usuario.PRP_CPF);
                    $('#cadas-nome').val(ret.OBJ_Usuario.PRP_NomeUsuario);
                    $('#cadas-email').val(ret.OBJ_Usuario.PRP_EmailUsuario);
                    $('#hdnIdentificadorCadastro').val(ret.OBJ_Usuario.PRP_IdUsuario);
                    $('#cadas-unidadeNegocio').val(ret.OBJ_Usuario.PRP_NomeBU);
                    $('#cadas-celular').val(ret.OBJ_Usuario.PRP_Celular).prop('disabled', true);
                    $('#exibeCelular').val(ret.OBJ_Usuario.PRP_Celular).prop('disabled', true);
                    $('#cadas_Confirmarcelular').val(ret.OBJ_Usuario.PRP_Celular).prop('disabled', true);
                    $('#divConfCelular').addClass('d-none');
                    $('#divCelular').addClass('d-none');
                    $('#divExibeCelular').removeClass('d-none');
                    $('#primeiro-acesso').addClass('d-none');
                    $('#cadastroModal').modal('show');
                    $('#cadastroModalLabel').text("Atualize a sua senha");
                }
                else {
                    //CADASTRO ATIVO COM SENHA EM CIRPTOGRAFIA PADRÃO. SOMENTE EFETUAR LOGIN
                    $('#divSenha').removeClass('d-none');
                    $('#btnEfetuarLogin').removeClass('d-none');
                    $('#btnVerificarCadasro').addClass('d-none');
                    // $('#cadastrocpf').prop('disabled', true);
                    $('#MontaHTMLCadastro').empty().addClass('d-none');
                }
            }
            else {
                $('#btnVerificarCadasro').removeClass('d-none')
                $('#MontaHTMLCadastro').removeClass('d-none').html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'>" + ret.PRP_Requisicao.PRP_Mensagem + "</div>");
            }
        }
    });
});

function fnRestarCardInicio() {
    $('#primeiro-acesso').removeClass('d-none');
    $('#cadastroModal').modal('hide');
    $('#btnVerificarCadasro').removeClass('d-none')
    $('#MontaHTMLCadastro').empty();
}

$(document).on('click', '#btn_cadastrar_MTD', function (event) {

    var pCelular = $('#cadas-celular').val();
    var pSenha = $('#cadas-senha').val();
    var confi = $('#cadas-conf-senha').val();
    var ConfCelular = $('#cadas_Confirmarcelular').val();
    var pIdentificador = parseInt($('#hdnIdentificadorCadastro').val());

    if (pCelular != ConfCelular) {
        $('.alertCelular').removeClass('d-none');
        $('.alertCelular').html('<p class="alert alert-danger text-center">Celulares informados não são iguais, tente novamente</p>');
        return false;
    } if (pCelular == "") {
        $('.alertCelular').removeClass('d-none');
        $('.alertCelular').html('<p class="alert alert-danger text-center">Celular é um campo obrigatório, preencha o campo e tenta novamente.</p>');
        return false;

    } if (ConfCelular == "") {
        $('.alertCelular').removeClass('d-none');
        $('.alertCelular').html('<p class="alert alert-danger text-center">Celular é um campo obrigatório, preencha o campo e tenta novamente.</p>');
        return false;
    }

    else {
        $('.alertCelular').addClass('d-none');
    }

    var parametros = {};
    if (pSenha == confi) {
        if (pSenha.length > 0) {
            if (confi.length > 0) {
                parametros = { pCelular, pSenha, pIdentificador };
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
                        $('.alertError').addClass('d-none');
                    },
                    success: function (ret) {
                        if (ret.PRP_Status == true) {
                            $('#cadastroModal').modal("hide");
                            Swal.fire(
                                'Cadastro efetuado!',
                            ).then(function () {
                                location.href = '/Conteudo/Home';
                            });

                        } else if (ret.PRP_Status == false) {
                            $('#MontaHTMLCadastro').removeClass('d-none');
                            $('#MontaHTMLCadastro').val(ret.PRP_Mensagem);
                        }
                    }
                });
            }
            else {
                $('.alertSenha').removeClass('d-none');
                $('.alertSenha').html('<div class="alert alert-danger text-center">Preencha o campo Senha para continuar</div>');
                return false;
            }
        }
        else {
            $('.alertSenha').removeClass('d-none');
            $('.alertSenha').html('<div class="alert alert-danger text-center">Preencha o campo Senha para continuar</div>');
            return false;
        }
    } else {
        $('.alertSenha').removeClass('d-none');
        $('.alertSenha').html('<div class="alert alert-danger text-center">As Senhas não conferem, digite novamente antes de continuar com o cadastro</div>');
        return false;
    }
});


$(document).on('click', '#btnEfetuarLogin', function (event) {
    var parametros = {};
    parametros.pCPF = $('#cadastrocpf').val();
    parametros.pSenha = $('#txtSenha').val();

    if ($('#cadastrocpf').val() == "") {
        $('#MontaHTMLCadastro').html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'><i class='fa fa-info-circle fa-lg'></i> <span>Informe seu CPF.</span></div>").removeClass('d-none');
        $('#cadastrocpf').focus();
        return false;
    }
    if ($('#txtSenha').val() == "") {
        $('#MontaHTMLCadastro').html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'><i class='fa fa-info-circle fa-lg'></i> <span>Informe sua senha.</span></div>").removeClass('d-none');
        $('#txtSenha').focus();
        return false;
    }

    $.ajax({
        type: 'POST',
        url: '/Login/MTD_AcessoLogin',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('#btnEfetuarLogin').addClass('disabled').html('<span class="py-2"><i class="fa fa-1x fa-spinner fa-spin"></i> acessando...</span>');
        },
        success: function (returnValue) {
            var jsonResult = JSON.parse(returnValue);
            if (jsonResult.PRP_Requisicao.PRP_Status == true) {
                location.href = jsonResult.URL;
            } else {
                $('#btnEfetuarLogin').removeClass('disabled').html('ENTRAR');
                $('#MontaHTMLCadastro').removeClass('d-none').html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'>" + jsonResult.PRP_Mensagem + "</div>");
            }
        }
    })

});

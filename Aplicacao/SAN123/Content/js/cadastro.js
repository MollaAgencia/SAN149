$(document).ready(function () {
});



$(document).on('click', '#btnlogin_Cadastro', function (event) {
    var stb_html = "";
    var parametros = {};
    parametros.pCPF = $('#cadastrocpf').val();

    $.ajax({
        type: 'POST',
        url: '/Login/MTD_BuscaPreCadastro',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('#btnlogin_Cadastro').addClass('disabled').html('<span class="py-2"><i class="fa fa-1x fa-spinner fa-spin"></i> Acessando...</span>');
            $('#MontaHTMLCadastro').addClass('d-none');
        },
        success: function (ret) {
            if (ret.PRP_Requisicao.PRP_Status == true) {
                if (ret.PRP_Requisicao.PRP_TipoMensagem == 1) {
                    $('#cadas-CPF').val(ret.OBJ_Usuario.PRP_CPF);
                    $('#cadas-nome').val(ret.OBJ_Usuario.PRP_NomeUsuario);
                    $('#cadas-email').val(ret.OBJ_Usuario.PRP_EmailUsuario);
                    $('#hdnIdentificadorCadastro').val(ret.OBJ_Usuario.PRP_IdUsuario);
                    $('#cadas-unidadeNegocio').val(ret.OBJ_Usuario.PRP_NomeBU);

                    $('#cadastroModal').modal('show');
                }
                else if (ret.PRP_Requisicao.PRP_TipoMensagem == 2) {
                    $('#btnlogin_Cadastro').removeClass('disabled').html('CADASTRE-SE AQUI');
                    $('#MontaHTMLCadastro').removeClass('d-none').html(ret.PRP_Requisicao.PRP_Mensagem);
                }
                else if (ret.PRP_Requisicao.PRP_TipoMensagem == 3) {
                    $('#btnlogin_Cadastro').removeClass('disabled').html('CADASTRE-SE AQUI');
                    $('#MontaHTMLCadastro').removeClass('d-none').html(ret.PRP_Requisicao.PRP_Mensagem);
                }
            } else {
                $('#btnlogin_Cadastro').removeClass('disabled').html('CADASTRE-SE AQUI');
                $('#MontaHTMLCadastro').removeClass('d-none').html(ret.PRP_Requisicao.PRP_Mensagem);
            }
        }
    });
});

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
                parametros = { pCelular, pSenha, pIdentificador};
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
                            $('#msg_informativa').removeClass('d-none');
                            $('#msg_informativa').val(ret.PRP_Mensagem);
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


$(document).on('click', '#btn_login_Acesso', function (event) {
    var parametros = {};
    parametros.pCPF = $('#logincpf_').val();
    parametros.pSenha = $('#loginsenha_').val();

    $.ajax({
        type: 'POST',
        url: '/Login/MTD_AcessoLogin',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('#btn_login_Acesso').addClass('disabled').html('<span class="py-2"><i class="fa fa-1x fa-spinner fa-spin"></i> acessando...</span>');
        },
        success: function (returnValue) {
            var jsonResult = JSON.parse(returnValue);
            if (jsonResult.PRP_Status == true) {
                location.href = '/Conteudo/Home';
            } else {
                $('#btn_login_Acesso').removeClass('disabled').html('ENTRAR');
                $('#Msg_informativa_Login').removeClass('d-none').html(jsonResult.PRP_Mensagem);
            }
        }
    })

});

$(document).ready(
    function () {

        $('#perfil-cpf, #perfil-celular').trigger('change');

    });

var modoEdicao = false, perfilNome, perfilEmail, perfilCelular, perfilSenha;

//Manipula os campos, dependendo do valor em modoEdicao
function manipularCampos() {

    $('#perfil-nome').prop('disabled', !modoEdicao);
    $('#perfil-celular').prop('disabled', !modoEdicao);
    $('#perfil-email').prop('disabled', !modoEdicao);
    $('#perfil-senha').prop('disabled', !modoEdicao);
    $('#perfil-conf-senha').prop('disabled', !modoEdicao);

    if (modoEdicao) {
        $('#perfil-div-conf-senha').removeClass('d-none');
        perfilNome = $('#perfil-nome').val();
        perfilEmail = $('#perfil-email').val();
        perfilCelular = $('#perfil-celular').val();
        perfilSenha = $('#perfil-senha').val();

        $('#div-editar').addClass('d-none');
        $('#div-salvar').removeClass('d-none');
        $(".custom-invalid-feedback").removeClass('d-none');
    } else {
        $('#perfil-div-conf-senha').addClass('d-none');
        $('#perfil-nome').val(perfilNome);
        $('#perfil-email').val(perfilEmail);
        $('#perfil-celular').val(perfilCelular);
        $('#perfil-senha').val(perfilSenha);

        $('#div-editar').removeClass('d-none');
        $('#div-salvar').addClass('d-none');
        $(".custom-invalid-feedback").addClass('d-none');
    }

}

//Valida Campos Vazios
function ValidaVazios() {

    var validNome = $("#perfil-nome").prop('valid') ? $("#perfil-nome").prop('valid') : true;
    var validCelular = $("#perfil-celular").prop('valid') ? $("#perfil-celular").prop('valid') : true;
    var validEmail = $("#perfil-email").prop('valid') ? $("#perfil-email").prop('valid') : true;
    var validSenha = $("#perfil-senha").prop('valid') ? $("#perfil-senha").prop('valid') : true;
    var validConfSenha = $("#perfil-conf-senha").prop('valid') ? $("#perfil-conf-senha").prop('valid') : true;

    console.log($("#perfil-nome").prop('valid'));
    console.log($("#perfil-celular").prop('valid'));
    console.log($("#perfil-email").prop('valid'));
    console.log($("#perfil-senha").prop('valid'));
    console.log($("#perfil-conf-senha").prop('valid'));

    return (validNome && validCelular && validEmail && validSenha && validConfSenha);
}

$('#btn-editar').click(function () {
    modoEdicao = true;
    manipularCampos();
    $("#alert-perfil").addClass("d-none");
});

$('#modal_detalhesPerfil').on('hide.bs.modal',
    function (e) {
    if (modoEdicao) {
        e.preventDefault();
        e.stopImmediatePropagation();

        const swalWithCustomizedButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-dark text-white',
                cancelButton: 'btn btn-danger text-white ml-4'
            },
            buttonsStyling: false
        });

        swalWithCustomizedButtons.fire({
            title: "Ops!",
            text: "Quer mesmo sair do modo edição, e fechar?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "SIM",
            cancelButtonText: "NÃO"
        })
            .then((result) => {
                if (result.value) {

                    modoEdicao = false;
                    manipularCampos();
                    $('#modal_detalhesPerfil').modal('hide');

                } else if (result.dismiss === Swal.DismissReason.cancel) {


                }
            });
        }
    });

$('#btn-cancelar').click(function () {
    modoEdicao = false;
    manipularCampos();
    $("#alert-perfil").addClass("d-none");
});

$('#btn-salvar').click(function () {
    if (ValidaVazios()) {

        var param = {};
        param.PRP_Documento = $("#perfil-cpf").val();
        param.PRP_Nome = $("#perfil-nome").val();
        param.PRP_Telefone = $("#perfil-celular").val();
        param.PRP_Email = $("#perfil-email").val();
        param.PRP_Senha = $("#perfil-senha").val();

        $.ajax({
            type: 'POST',
            url: '/Login/MTD_SalvarPerfil',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            beforeSend: function () {
                $("#alert-perfil")
                    .removeClass("d-none")
                    .html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'><i class='fa fa-1x fa-spinner fa-spin'></i> Processando... </div>");
                $(this).prop('disabled', true);
                $('#div-campos-cadastro').prop('disabled', true);
            },
            success: function (json) {

                var retorno = JSON.parse(json);

                console.log(retorno);   

                if (retorno.PRP_Status) {
                    $('#perfil-nome').val(perfilNome);
                    $('#perfil-email').val(perfilEmail);
                    $('#perfil-celular').val(perfilCelular);
                    $('#perfil-senha').val(perfilSenha);
                }

                $("#alert-perfil").removeClass("d-none").html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'>" + retorno.PRP_Mensagem + "</div>");
            },
            error: function () {

                $("#alert-perfil")
                    .removeClass("d-none")
                    .html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'>Erro na conexão com o Servidor. Tente novamente mais tarde.</div>");

            },
            complete: function () {
                modoEdicao = false;
                manipularCampos();
            }
        });

    } else {
        $("#alert-perfil")
            .removeClass("d-none")
            .html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'>Ainda existem campos inválidos ou vazios. Preencha-os corretamente para continuar</div>");
    }
});


/* ------------- CONTATO ------------- */

$('button[name="btn_contato"]').bind('click', function (event) {
    event.preventDefault();

    $('#contato_mensagem').trigger('change');
    
    if ($('#contato_mensagem').val() == "") {
        $('#alert_contato').removeClass('d-none').html("<div class='px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2'> Deixe sua mensagem.</div>");
        $('#contato_mensagem').focus();
        return false;
    }

    var parametros = {};
    parametros.pIdUsuario = parseInt($('#id-usuario').val());
    parametros.pMensagem = $('#contato_mensagem').val();


    $.ajax({
        type: 'POST',
        url: '/Login/MTD_Contato',
        data: JSON.stringify(parametros),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $('button[name="btn_contato"]').addClass('disabled');
            $('#alert_contato').removeClass('d-none').html('<div class="px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2 line-height-2"><i class="fa fa-1x fa-spinner fa-spin"></i> Enviando...</div>');
        },
        success: function (returnValue) {
            console.log(returnValue);
            var jsonResult = JSON.parse(returnValue);
            if (jsonResult.PRP_Status === true) {
                $('#alert_contato').removeClass('d-none').html('<div class="px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2 line-height-2">Mensagem enviada!<br>Aguarde o retorno via e-mail ou telefone.</div>');
                $('button[name="btn_contato"]').removeAttr('disabled', 'disabled');
            } else {
                $('button[name="btn_contato"]').addClass('disabled');
                $('#alert_contato').removeClass('d-none').html('<div class="px-3 py-2 bg-gray text-dark rounded d-inline-block mt-2 line-height-2">Erro ao enviar mensagem!<br>Favor tentar novamente.</div>');
            }
        }
    });
});

$(".btn-contato").click(function () {
    $("#contato_cpf").trigger('change');
});

function validaCamposContato() {
    var validCpf = $('#contato_cpf').prop('valid');
    var validEmail = $('#contato_email').prop('valid');
    var validNome = $('#contato_nome').prop('valid');
    var validAssunto = $('#contato_assunto').prop('valid');
    var validMensagem = $('#contato_mensagem').prop('valid');

    return (validCpf && validEmail && validNome && validAssunto && validMensagem);
}

//Fecha e reseta o formulário de Contato
$('button[name="btn-fecha-contato"]').click(function () {
    $("#contato_nome").val('');
    $("#contato_email").val('');
    $("#contato_assunto").val(0);
    $("#contato_mensagem").val('');
    $("#alert_contato").addClass("d-none");
    $("#btn_enviarContato").addClass("disabled");
});

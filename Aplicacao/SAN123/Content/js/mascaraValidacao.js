/*

    Script de VALIDAÇÃO e MÁSCARA de formulário
    Publicado em 20/09/2019
    Por Anderson Romão
    Github: https://github.com/andblade

*/


/******* MÁSCARA DO CAMPO "[mask-cnpj-cpf]" *******/
// document.write('<script src="//cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.js" type="text/javascript"></script>');


/******* MASCARA *******/

// Mascara CPF
$('[mask-cpf]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskCpf($this.val()));
        });

    function maskCpf(cpf) {
        cpf = cpf.replace(/\D/g, "");
        cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
        cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2");
        cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
        return cpf;
    }

});

// Mascara CNPJ
$('[mask-cnpj]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskCnpj($this.val()));
        });

    function maskCnpj(cnpj) {
        cnpj = cnpj.replace(/\D/g, "");
        cnpj = cnpj.replace(/^(\d{2})$/, "$1");
        cnpj = cnpj.replace(/^(\d{2})(\d{3})$/, "$1.$2");
        cnpj = cnpj.replace(/^(\d{2})(\d{3})(\d{3})$/, "$1.$2.$3");
        cnpj = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})$/, "$1.$2.$3/$4");
        cnpj = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, "$1.$2.$3/$4-$5");
        return cnpj;
    }

});


// Mascara RG
$('[mask-rg]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskRg($this.val()))
        });

    function maskRg(rg) {
        rg = rg.replace(/\D/g, "");
        rg = rg.replace(/^(\d{2})$/, "$1");
        rg = rg.replace(/^(\d{2})(\d{3})$/, "$1.$2");
        rg = rg.replace(/^(\d{2})(\d{3})(\d{3})$/, "$1.$2.$3");
        rg = rg.replace(/^(\d{2})(\d{3})(\d{3})(\d{1})$/, "$1.$2.$3-$4");
        return rg;
    }

});

// Mascara Nascimento
$('[mask-nascimento]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskDataNascimento($this.val()))
        });


    function maskDataNascimento(nascimento) {
        nascimento = nascimento.replace(/\D/g, "");
        nascimento = nascimento.replace(/^(\d{2})/, '$1');
        nascimento = nascimento.replace(/^(\d{2})(\d{2})$/, '$1/$2');
        nascimento = nascimento.replace(/^(\d{2})(\d{2})(\d{4})$/, '$1/$2/$3');
        return nascimento;
    }

});

// Mascara Periodo
$('[mask-periodo]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskPeriodo($this.val()))
        });

    function maskPeriodo(periodo) {
        periodo = periodo.replace(/\D/g, "");
        periodo = periodo.replace(/(\d{2})(\d{2})(\d{4})$/, "$1/$2/$3");
        return periodo;
    }

});

// Mascara Telefone
$('[mask-telefone]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskTelefone($this.val()))
        });

    function maskTelefone(telefone) {
        telefone = telefone.replace(/\D/g, "");
        telefone = telefone.replace(/^(\d{2})/, '($1) ');
        telefone = telefone.replace(/(\d{4})(\d{4})$/, '$1-$2');
        return telefone;
    }

});

// Mascara Celular
$('[mask-celular]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskCelular($this.val()))
        });

    function maskCelular(celular) {

        celular = celular.replace(/\D/g, "");
        celular = celular.replace(/(\d{2})(\d)/, "($1) $2");
        celular = celular.replace(/(\d{5})(\d{4})/, "$1-$2");

        return celular;
    }

});

// Mascara CEP
$('[mask-cep]').each(function () {
    var $this = $(this);
    $this.on('keyup change',
        function () {
            $this.val(maskCep($this.val()))
        });

    function maskCep(cep) {
        cep = cep.replace(/\D/g, "");
        cep = cep.replace(/(\d{2})(\d{3})(\d{3})$/, "$1.$2-$3");
        return cep;
    }
});


/******* VALIDAÇÃO *******/


// CSS - Alerta de Erro
$('.alertError').css({
    'font-style': 'italic',
    'font-size': '13px',
    'color': '#dc3545'
});

// Validar CPF
$('[valida-cpf]').bind('keyup blur change', function () {
    var cpf = this.value;
    if (cpf.length > 0) {
        if (cpf.length === 14) {
            cpf = cpf.replace('.', '');
            cpf = cpf.replace('.', '');
            cpf = cpf.replace('-', '');

            if (cpf.length != 11 ||
                cpf == '00000000000' ||
                cpf == '11111111111' ||
                cpf == '22222222222' ||
                cpf == '33333333333' ||
                cpf == '44444444444' ||
                cpf == '55555555555' ||
                cpf == '66666666666' ||
                cpf == '77777777777' ||
                cpf == '88888888888' ||
                cpf == '99999999999') {
                $('[alert-cpf]').removeClass('d-none').html('<span class="alertError">CPF inválido</span>');
                return false;
            }
            soma = 0;
            for (i = 0; i < 9; i++) {
                soma += parseInt(cpf.charAt(i)) * (10 - i);
            }
            resto = 11 - (soma % 11);
            if (resto == 10 || resto == 11) {
                resto = 0;
            }
            if (resto != parseInt(cpf.charAt(9))) {
                $('[alert-cpf]').removeClass('d-none').html('<span class="alertError">CPF inválido</span>');
                return false;
            }
            soma = 0;
            for (i = 0; i < 10; i++) {
                soma += parseInt(cpf.charAt(i)) * (11 - i);
            }
            resto = 11 - (soma % 11);
            if (resto == 10 || resto == 11) {
                resto = 0;
            }
            if (resto != parseInt(cpf.charAt(10))) {
                $('[alert-cpf]').removeClass('d-none').html('<span class="alertError">CPF inválido</span>');
                $(this).addClass('custom-is-invalid');
                $(this).prop('valid', false);
                return false;
            } else {
                $('[alert-cpf]').addClass('d-none');
                $(this).removeClass('custom-is-invalid');
                $(this).prop('valid', true);
                return true;
            }
        } else {
            $('[alert-cpf]').removeClass('d-none').html('<span class="alertError">CPF incompleto</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        }
    } else {
        $('[alert-cpf]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar CNPJ
$('[valida-cnpj]').bind('blur keyup change', function () {
    var cnpj = this.value;
    var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
    var dig1 = new Number;
    var dig2 = new Number;

    exp = /\.|\-|\//g
    cnpj = cnpj.toString().replace(exp, "");

    if (cnpj.length > 0) {
        if (cnpj.length === 18) {
            if (cnpj == '00000000000000' ||
                cnpj == '11111111111111' ||
                cnpj == '22222222222222' ||
                cnpj == '33333333333333' ||
                cnpj == '44444444444444' ||
                cnpj == '55555555555555' ||
                cnpj == '66666666666666' ||
                cnpj == '77777777777777' ||
                cnpj == '88888888888888' ||
                cnpj == '99999999999999') {
                $('[alert-cnpj]').removeClass('d-none').html('<span class="alertError">CNPJ inválido</span>');
                return false;
            }

            var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

            for (i = 0; i < valida.length; i++) {
                dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
                dig2 += cnpj.charAt(i) * valida[i];
            }
            dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
            dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

            if (((dig1 * 10) + dig2) != digito) {
                $('[alert-cnpj]').removeClass('d-none').html('<span class="alertError">CNPJ inválido</span>');
                $(this).addClass('custom-is-invalid');
                $(this).prop('valid', false);
                return false;
            } else {
                $('[alert-cnpj]').addClass('d-none');
                $(this).removeClass('custom-is-invalid');
                $(this).prop('valid', true);
                return true;
            }
        } else {
            $('[alert-cnpj]').removeClass('d-none').html('<span class="alertError">CNPJ Incompleto</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        }
    } else {
        $('[alert-cnpj]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar RG
$('[valida-rg]').bind('blur keyup change', function (numero) {
    var numero = $(this).val();
    numero = numero.replace('.', '');
    numero = numero.replace('.', '');
    numero = numero.replace('-', '');

    tamanho = numero.length;
    vetor = new Array(tamanho);

    if (tamanho >= 1) { vetor[0] = parseInt(numero[0]) * 2; }
    if (tamanho >= 2) { vetor[1] = parseInt(numero[1]) * 3; }
    if (tamanho >= 3) { vetor[2] = parseInt(numero[2]) * 4; }
    if (tamanho >= 4) { vetor[3] = parseInt(numero[3]) * 5; }
    if (tamanho >= 5) { vetor[4] = parseInt(numero[4]) * 6; }
    if (tamanho >= 6) { vetor[5] = parseInt(numero[5]) * 7; }
    if (tamanho >= 7) { vetor[6] = parseInt(numero[6]) * 8; }
    if (tamanho >= 8) { vetor[7] = parseInt(numero[7]) * 9; }
    if (tamanho >= 9) { vetor[8] = parseInt(numero[8]) * 100; }

    total = 0;

    if (tamanho >= 1) { total += vetor[0]; }
    if (tamanho >= 2) { total += vetor[1]; }
    if (tamanho >= 3) { total += vetor[2]; }
    if (tamanho >= 4) { total += vetor[3]; }
    if (tamanho >= 5) { total += vetor[4]; }
    if (tamanho >= 6) { total += vetor[5]; }
    if (tamanho >= 7) { total += vetor[6]; }
    if (tamanho >= 8) { total += vetor[7]; }
    if (tamanho >= 9) { total += vetor[8]; }

    resto = total % 11;

    if (resto != 0 || numero.length != 9) {
        $('[alert-rg]').removeClass('d-none').html('<span>RG inválido</span>');
    } else {
        $('[alert-rg]').addClass('d-none');
    }
});

// Validar CNPJ e CPF mesmo input
$('[valida-cnpj-cpf]').bind('blur keyup change', function () {
    valorInput = this.value;

    exp = /\.|\-|\//g
    valorInput = valorInput.toString().replace(exp, "");

    if (valorInput == '') {
        $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">Campo inválido. Favor preencher somente números o campo CPF/CNPJ</span>');
        return (false);
    } if (((valorInput.length == 11) && (valorInput == 11111111111) ||
        (valorInput == 22222222222) || (valorInput == 33333333333) ||
        (valorInput == 44444444444) || (valorInput == 55555555555) ||
        (valorInput == 66666666666) || (valorInput == 77777777777) ||
        (valorInput == 88888888888) || (valorInput == 99999999999) ||
        (valorInput == 00000000000))) {
        $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
        return (false);
    }
    if (!((valorInput.length == 11) || (valorInput.length == 14))) {
        $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
        return (false);
    }

    var checkOK = '0123456789';
    var checkStr = valorInput;
    var allValid = true;
    var allNum = '';

    for (i = 0; i < checkStr.length; i++) {
        ch = checkStr.charAt(i);
        for (j = 0; j < checkOK.length; j++)
            if (ch == checkOK.charAt(j))
                break;
        if (j == checkOK.length) {
            allValid = false;
            break;
        } allNum += ch;
    } if (!allValid) {
        $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">Campo inválido. Favor preencher somente números o campo CPF/CNPJ</span>');
        return (false);
    }

    var chkVal = allNum;
    var prsVal = parseFloat(allNum);
    if (chkVal != '' && !(prsVal > '0')) {
        alert('CPF zerado !');
        return (false);
    }

    if (valorInput.length == 11) {
        var tot = 0;
        for (i = 2; i <= 10; i++)
            tot += i * parseInt(checkStr.charAt(10 - i));

        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(9))) {
            $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
            return (false);
        }

        tot = 0;
        for (i = 2; i <= 11; i++)
            tot += i * parseInt(checkStr.charAt(11 - i));

        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(10))) {
            $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
            return (false);
        } else {
            $('[alert-cpf-cnpj]').addClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
            return (true);
        }
    }
    else {
        var tot = 0;
        var peso = 2;

        for (i = 0; i <= 11; i++) {
            tot += peso * parseInt(checkStr.charAt(11 - i));
            peso++;
            if (peso == 10) {
                peso = 2;
            }
        }
        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(12))) {
            $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
            return (false);
        } else {
            $('[alert-cpf-cnpj]').addClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
            return (true);
        }

        tot = 0;
        peso = 2;
        for (i = 0; i <= 12; i++) {
            tot += peso * parseInt(checkStr.charAt(12 - i));
            peso++;
            if (peso == 10) {
                peso = 2;
            }
        }

        if ((tot * 10 % 11 % 10) != parseInt(checkStr.charAt(13))) {
            $('[alert-cpf-cnpj]').removeClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
            return (false);
        } else {
            $('[alert-cpf-cnpj]').addClass('d-none').html('<span class="alertError">CPF/CNPJ inválido</span>');
            return (true);
        }
    }
});

// Validar Nome
$('[valida-nome]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        if ($(this).val().length <= 3) {
            $('[alert-nome]').removeClass('d-none').html('<span class="alertError">Nome incompleto</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-nome]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-nome]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar E-mail
$('[valida-email]').bind('blur keyup change', function () {
    usuario = this.value.substring(0, this.value.indexOf('@'));
    dominio = this.value.substring(this.value.indexOf('@') + 1, this.value.length);
    if ($(this).val().length > 0) {
        if ((usuario.length >= 1) &&
            (dominio.length >= 3) &&
            (usuario.search('@') == -1) &&
            (dominio.search('@') == -1) &&
            (usuario.search(' ') == -1) &&
            (dominio.search(' ') == -1) &&
            (dominio.search('.') != -1) &&
            (dominio.indexOf('.') >= 1) &&
            (dominio.lastIndexOf('.') < dominio.length - 1)) {
            $('[alert-email]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        } else {
            $('[alert-email]').removeClass('d-none').html('<span class="alertError">E-mail inválido</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        }
    } else {
        $('[alert-email]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar confirmação de e-mail
$('[valida-confEmail]').bind('blur keyup change', function () {

    if ($(this).val().length > 0) {

        email = $('[valida-email]').val();

        if ($(this).val() == false) {
            $('[alert-conf-email]').removeClass('d-none').html('<span class="alertError">Repita o e-mail corretamente</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else if (email != $(this).val()) {
            $('[alert-conf-email]').removeClass('d-none').html('<span class="alertError">Repita o e-mail corretamente</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-conf-email]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }

    } else {
        $('[alert-conf-email]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar envio de arquivo
$('[valida-arquivo]').bind('blur keyup change', function () {
    if ($(this).val().length == 0) {
        $('[alert-arquivo]').removeClass('d-none').html('<span class="alertError">Arquivo não adicionado</span>');
        // $(this).focus();
        return false;
    } else {
        $('[alert-arquivo]').addClass('d-none');
        return true;
    }
});

// Validar data de nascimento
$('[valida-nascimento]').bind('blur keyup change', function () {
    var data = this.value;

    if (data.length > 0) {

        data = data.replace(/\//g, '-'); // substitui eventuais barras (ex. IE) "/" por hífen "-"
        var data_array = data.split('-'); // quebra a data em array

        // para o IE onde será inserido no formato dd/MM/yyyy
        if (data_array[0].length != 4) {
            data =
                data_array[2] + '-' + data_array[1] + '-' + data_array[0]; // remonta a data no formato yyyy/MM/dd
        }

        // comparo as datas e calculo a idade
        var hoje = new Date();
        var nasc = new Date(data);
        var idade = hoje.getFullYear() - nasc.getFullYear();
        var m = hoje.getMonth() - nasc.getMonth();
        if (m < 0 || (m === 0 && hoje.getDate() < nasc.getDate())) idade--;

        if (idade >= 0 && idade < 18) {
            $('[alert-data-nascimento]').removeClass('d-none')
                .html('<span class="alertError">Menor de 18 anos não pode se cadastrar</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else if (idade >= 18 && idade <= 90) {
            $('[alert-data-nascimento]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        } else if (idade < 0) {
            $('[alert-data-nascimento]').removeClass('d-none')
                .html('<span class="alertError">Data de nascimento inválida</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-data-nascimento]').removeClass('d-none')
                .html('<span class="alertError">Data de nascimento inválida</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        }
    } else {
        $('[alert-data-nascimento]').removeClass('d-none')
            .html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        $(this).addClass('custom-is-invalid');

        return false;
    }
});

// Validar data de período
$('[valida-periodo]').bind('focus change', function () {

    var entrada = new Date($(this).val());
    entrada.setDate(entrada.getDate() + 1);

    var inicio = new Date('2020-01-01');
    inicio.setDate(inicio.getDate() + 1);

    var final = new Date('2020-12-31');
    final.setDate(final.getDate() + 1);

    if (entrada < inicio || entrada > final || entrada == 'Invalid Date') {
        $('[alert-periodo]').removeClass('d-none').html('Fora do período estipulado');
    } else {
        $('[alert-periodo]').removeClass('d-none').html('<span class="text-success">Dentro do periodo</span>').fadeOut(4000);
    }
});

// Validar telefone
$('[valida-telefone]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        if ($(this).val().length < 10) {
            $('[alert-telefone]').removeClass('d-none').html('<span class="alertError">Telefone inválido</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-telefone]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-telefone]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar Celular
$('[valida-celular]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        if ($(this).val().length < 13) {
            $('[alert-celular]').removeClass('d-none').html('<span class="alertError">Celular inválido</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-celular]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-celular]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar Seleção
$('[valida-selecao]').bind('blur keypress change', function () {
    if ($(this).val() == '' || $(this).val() == '0') {
        $('[alert-selecao]').removeClass('d-none').html('<span class="alertError">Selecione uma opção</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    } else {
        $('[alert-selecao]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
        return true;
    }
});

// Validar opção radio
$('[valida-radio]').bind('blur keypress change', function () {
    if ($('input[valida-radio]:checked').length < 1) {
        $('[alert-radio]').removeClass('d-none').html('<span class="alertError">Escolha uma opção</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    } else {
        $('[alert-radio]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
        return true;
    }
});

// Validar CEP
$('[valida-cep]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        //Nova variável "cep" somente com dígitos.
        var cep = this.value.replace(/\D/g, '');
        if (cep != '') {
            //Expressão regular para validar o CEP.
            var validacep = /^[0-9]{8}$/;
            //Valida o formato do CEP.
            if (validacep.test(cep)) {
                //Preenche os campos com "carregando..." enquanto consulta webservice.
                $('[valida-endereco]').val('carregando...');
                $('[valida-bairro]').val('carregando...');
                $('[valida-cidade]').val('carregando...');
                $('[valida-uf]').val('carregando...');
                //Cria um elemento javascript.
                var script = document.createElement('script');
                //Sincroniza com o callback.
                script.src = 'https://viacep.com.br/ws/' + cep + '/json/?callback=meu_callback';
                //Insere script no documento e carrega o conteúdo.
                document.body.appendChild(script);
            } else {
                limpa_formulário_cep();
                $('[alert-cep]').removeClass('d-none').html('<span class="alertError">CEP não encontrado</span>');
                $(this).addClass('custom-is-invalid');
                $(this).prop('valid', false);
            }
        } else {
            //cep sem valor, limpa formulário.
            limpa_formulário_cep();
            $('[alert-cep]').removeClass('d-none').html('<span class="alertError">CEP não encontrado</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
        }
        return false;
    } else {
        $('[alert-cep]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

function limpa_formulário_cep() {
    //Limpa valores do formulário de cep.
    $('[valida-endereco]').val('');
    $('[valida-bairro]').val('');
    $('[valida-cidade]').val('');
    $('[valida-uf]').val('');
}

function meu_callback(conteudo) {
    if (!('erro' in conteudo)) {
        //Atualiza os campos com os valores.
        $('[valida-endereco]').val(conteudo.logradouro);
        $('[valida-bairro]').val(conteudo.bairro);
        $('[valida-cidade]').val(conteudo.localidade);
        $('[valida-uf]').val(conteudo.uf);

        $('[alert-cep]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
    } else {
        //CEP não Encontrado.
        limpa_formulário_cep();
        $('[alert-cep]').removeClass('d-none').html('<p class="alertError">CEP não encontrado</p>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
    }
}

// Validar endereço
$('[valida-endereco]').bind('blur keyup change', function () {
    if ($(this).val() == '') {
        $('[alert-endereco]').removeClass('d-none').html('<span class="alertError">Endereço não encontrado</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    } else {
        $('[alert-endereco]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
        return true;
    }
});

// Validar número residencial
$('[valida-numero]').bind('blur keyup change', function () {
    if ($(this).val() == '') {
        $('[alert-numero]').removeClass('d-none').html('<span class="alertError">Número não informado</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    } else {
        $('[alert-numero]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
        return true;
    }
});

// Validar bairro
$('[valida-bairro]').bind('blur keyup change', function () {
    if ($(this).val() == '') {
        $('[alert-bairro]').removeClass('d-none').html('<span class="alertError">Bairro não informado</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    } else {
        $('[alert-bairro]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
        return true;
    }
});

// Validar cidade
$('[valida-cidade]').bind('blur keyup change', function () {
    if ($(this).val() == '') {
        $('[alert-cidade]').removeClass('d-none').html('<span class="alertError">Cidade não informada</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    } else {
        $('[alert-cidade]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
        return true;
    }
});

// Validar UF
$('[valida-uf]').bind('blur keyup change', function () {
    if ($(this).val() == '') {
        $('[alert-uf]').removeClass('d-none').html('<span class="alertError">Estado não informado</span>');
        return false;
    } else {
        $('[alert-uf]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        return true;
    }
});

// Validar mensagem / observação
$('[valida-mensagem]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        if ($(this).val().length <= 3) {
            $('[alert-mensagem]').removeClass('d-none').html('<span class="alertError">Deixe sua mensagem</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-mensagem]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-mensagem]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar assunto input
$('[valida-assunto]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        if ($(this).val().length <= 3) {
            $('[alert-assunto]').removeClass('d-none').html('<span class="alertError">Informe o Assunto</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-assunto]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-assunto]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar assunto select
$('[valida-assunto-select]').bind('blur keyup change', function () {
    if ($(this).val() == 0) {
        $('[alert-assunto]').removeClass('d-none').html('<span class="alertError">Informe o Assunto</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    } else {
        $('[alert-assunto]').addClass('d-none');
        $(this).removeClass('custom-is-invalid');
        $(this).prop('valid', true);
        return true;
    }
});

// Validar senha Login
$('[valida-senha-login]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        if ($(this).val().length <= 3) {
            $('[alert-senha]').removeClass('d-none').html('<span class="alertError">Senha não informada</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-senha]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-senha]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar senha
$('[valida-senha]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        if ($(this).val().length <= 3) {
            $('[alert-senha]').removeClass('d-none').html('<span class="alertError">Senha não informada</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-senha]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-senha]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar comparação de senha
$('[valida-confsenha]').bind('blur keyup change', function () {
    if ($(this).val().length > 0) {
        senha = $('[valida-senha]').val();
        if ($(this).val() != senha) {
            $('[alert-conf-senha]').removeClass('d-none')
                .html('<span class="alertError">Repita a senha corretamente</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-conf-senha]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    } else {
        $('[alert-conf-senha]').removeClass('d-none').html('<span class="alertError">Campo Obrigatório</span>');
        $(this).addClass('custom-is-invalid');
        $(this).prop('valid', false);
        return false;
    }
});

// Validar Força de senha
$('[valida-forca-senha]').bind('blur keyup change', function () {

    var senha = $(this).val();

    var caracteres = '';

    var strength = senha.length >= 6 ? 1 : 0;

    // Adiciona um ponto caso a senha possua caractere maiúsculo ou minúsculo.
    if (senha.match(/([a-z])/)) { strength += 1 } else { caracteres += '(a-z) ' };

    // Adiciona um ponto caso a senha possua caractere maiúsculo ou minúsculo.
    if (senha.match(/([A-Z])/)) { strength += 1 } else { caracteres += '(A-Z) ' };

    // Adiciona um ponto caso a senha tenha números e caracteres.
    if (senha.match(/([0-9])/)) { strength += 1 } else { caracteres += '(0-9) ' };

    // Adiciona um ponto caso tenha ao menos um caractere especial.
    if (senha.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) { strength += 1; } else { caracteres += '(! % & @ # $ ^ * ? _ ~) ' };

    // Adiciona um ponto caso tenha dois caracteres especiais.
    if (senha.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1;

    if (strength >= 1) {

        var forcaSenha = '';
        var textClass = '';
        var progressClass = '';
        var dFlex = 'd-flex';
        var dNone = 'd-none';

        if (strength < 2) {
            forcaSenha = 'Fraca';
            textClass = 'text-danger';
            progressClass = 'bg-danger w-25';
            $('[valida-senha]').prop('valid', false);
        } else if (strength >= 2 && strength <= 4) {
            forcaSenha = 'Boa';
            textClass = 'text-warning';
            progressClass = 'bg-warning w-75';
            $('[valida-senha]').prop('valid', false);
        } else if (strength > 4) {
            forcaSenha = 'Forte';
            textClass = 'text-success';
            progressClass = 'bg-success w-100';
            $('[valida-senha]').prop('valid', true);
        }

        $("[valida-forca-senha]").removeClass("custom-is-invalid");
        $('[alert-forca-senha]').removeClass();
        $('[alert-forca-senha]').addClass('custom-invalid-feedback');
        $("[alert-forca-senha]").removeClass("d-none")
            .html(
                '<div class="d-flex justify-content-around">' +
                `<div class="font-weight-semibold ${textClass}">${forcaSenha}</div>` +
                '<div class="progress bg-transparent mt-2" style="width: 80%; height: 10px">' +
                `<div class="progress-bar ${progressClass}" role="progressbar" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">` +
                '</div >' +
                '</div >' +
                '</div>' +
                `<span class="${caracteres ? dFlex : dNone}"> Caracteres restantes: ${caracteres.split(' ').join(', ')}</span>` +
                `<span class="${senha.length < 6 ? dFlex : dNone}">A senha deve conter ao menos 6 caracteres</span>`);
    }
});

// Validar checkbox
$('[valida-checkbox]').bind('blur keyup change', function () {
    var checkOk = document.getElementsByName('checkbox');
    for (var i = 0; i < checkOk.length; i++) {
        if (checkOk[i].checked == false) {
            $('[alert-regulamento]').removeClass('d-none').html('<span class="alertError">Aceite o regulamento para proseguir</span>');
            $(this).addClass('custom-is-invalid');
            $(this).prop('valid', false);
            return false;
        } else {
            $('[alert-regulamento]').addClass('d-none');
            $(this).removeClass('custom-is-invalid');
            $(this).prop('valid', true);
            return true;
        }
    }
});

$('#btnEnviar').bind('click', function (event) {
    event.preventDefault();
    $('[required]').each(function () {
        if ($(this).val() == '') {
            Swal.fire('Existem campos que não foram preenchidos.');
            return false;
        } else {
            Swal.fire('Formulário validado!');
        }
    });
});
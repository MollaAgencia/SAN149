class HelperValidaForm{
    constructor(){
        throw new Error('Mascara não pode ser instanciada')
    }
    static finaliza(elemento,value,mensagemType,mensagemTexto){
        // let objs = {};
        // objs.elemento = elemento;
        // objs.value = value;
        // objs.name = elemento.getAttribute('name');
        // objs.mensagemType = mensagemType;
        // objs.mensagemTexto = mensagemTexto;
        // return objs;
        return new ValidaForms(elemento,value,mensagemType,mensagemTexto);
    }
    static operator(input,function_name){
        if(function_name === 'Validanull'){
            return this.finaliza(input,true,'success','ok');
        }
        return this[function_name](input);
    }
    static ValidaSelect(select,option){
        
            if(option.value != ''){
                return this.finaliza(select,true,'success','ok')
            }
            else{
                return this.finaliza(select,false,'danger','Selecione algo válido');
            }
        
        
    }
    static ValidaCNPJ(cnpj){

        // var filtro = /(\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2})/g;
        if(!/(\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2})/g.test(cnpj.value)){
            return this.finaliza(cnpj,false,'danger','CNPJ inválido');
        }
        cnpj.value = remove(cnpj.value, ".");
        cnpj.value = remove(cnpj.value, "-");
        cnpj.value = remove(cnpj.value, "/");
        
        // Elimina CNPJs invalidos conhecidos
        if (//cnpj == "00000000000000" ||  /*removi este cpf para ser usado como teste pela itamaraty*/
            //cnpj == "11111111111111" || 
            cnpj.value == "22222222222222" || 
            cnpj.value == "33333333333333" || 
            cnpj.value == "44444444444444" || 
            cnpj.value == "55555555555555" || 
            cnpj.value == "66666666666666" || 
            cnpj.value == "77777777777777" || 
            cnpj.value == "88888888888888" || 
            cnpj.value == "99999999999999")
        {
            return this.finaliza(cnpj,false,'danger','CNPJ inválido');
        }
        // Valida DVs
        let tamanho = cnpj.value.length - 2;
        let	numeros = cnpj.value.substring(0,tamanho);
        let digitos = cnpj.value.substring(tamanho);
        let soma = 0;
        let pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2)
                pos = 9;
        }
        let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
            return this.finaliza(cnpj,false,'danger','CNPJ inválido');
             
        tamanho = tamanho + 1;
        numeros = cnpj.value.substring(0,tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1))
            return this.finaliza(cnpj,false,'danger','CNPJ inválido');

               
        return this.finaliza(cnpj,true,'success','ok');

    }
    //Validar CPF
    
    static ValidaCPF(cpf){
        if(cpf.value.length > 14 )
            this.ValidaCNPJ(cpf);

        var filtro = /^\d{3}.\d{3}.\d{3}-\d{2}$/i;
        if(!filtro.test(cpf.value)){
            //window.alert("CPF inválido. Tente novamente.");
            return this.finaliza(cpf,false,'danger','CPF inválido');
            // return {elemento:cpf,value:false,mensagem:'CPF inválido'};
        }
        cpf.value = remove(cpf.value, ".");
        cpf.value = remove(cpf.value, "-");
        /*if(
            cpf.length != 11 || cpf == "00000000000" || cpf == "11111111111" ||
            cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" ||
            cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" ||
            cpf == "88888888888" || cpf == "99999999999"
            ){
            //window.alert("CPF inválido. Tente novamente.");
            return false;
        }*/
        let soma = 0;
        let resto = 0;
        for(i = 0; i < 9; i++){
            soma += parseInt(cpf.value.charAt(i)) * (10 - i);
        }	
        resto = 11 - (soma % 11);
        if(resto == 10 || resto == 11){
            resto = 0;
        }
        if(resto != parseInt(cpf.value.charAt(9))){
            //window.alert("CPF inválido. Tente novamente.");
            return this.finaliza(cpf,false,'danger','CPF inválido');
        }
        soma = 0;
        for(i = 0; i < 10; i ++){
            soma += parseInt(cpf.value.charAt(i)) * (11 - i);
        }
        resto = 11 - (soma % 11);
        if(resto == 10 || resto == 11){
            resto = 0;
        }	
        if(resto != parseInt(cpf.value.charAt(10))){
            //window.alert("CPF inválido. Tente novamente.");
            return this.finaliza(cpf,false,'danger','CPF inválido');
        }
        return this.finaliza(cpf,true,'success','ok');
    }
    static ValidaNumerico(input){
        console.log('numero',input);
    }
    static ValidaCEP(input){
        console.log('CEP',input);
    }

    static ValidaTelefone(input){
        let retorno = {};
        input = input.value.replace(/\D/g,'');
        let arrNumberNotAllowed = ["99999999","999999999","88888888","888888888","77777777","777777777","55555555","555555555","44444444","444444444","33333333","333333333","22222222","222222222","11111111","111111111","00000000","000000000"];
        if(input.length < 10){
            retorno.sucesso=false;
            retorno.mesg='Número de celular inválido';
            return retorno;
        }
        else{
            let digito = input.substring(0,2);
            console.log(digito)
            let regeZ = new RegExp(/(10)|([1-9][1-9])/g); 
            
            if(!regeZ.test(digito)){
                //return false
                retorno.sucesso=false;
                retorno.mesg='Código de área inválido';
                return retorno;
            }
        };
        for (let i = arrNumberNotAllowed.length - 1; i >= 0; i--) {
            if(remove(input,"-").search(arrNumberNotAllowed[i]) != -1){
                retorno.sucesso=false;
                retorno.mesg='Número inválido';
                return retorno;
            }
            
        };
        retorno.sucesso=true;
        retorno.mesg='';
        return retorno;
        
    }
    static ValidaEmail(input){
        let reEmail = new RegExp(/^[a-zA-Z0-9._%+-]+@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,4}$/gi);
        console.log(reEmail,'-',input.value)
        if(!reEmail.test(input.value)){
            if(input.value==''){
                return true
            }else{
                return false;
            }
        }
        return true;
    }
    static Validainput(elemento){
        this.Array = [] 
        elemento.forEach(input =>{
            if(input.elemento.tagName !== "SELECT"){
                if(input.valida == 'CPF' && input.elemento.value.length>14){
                    let inputValida = input.elemento.value !== '' ?  this.operator(input.elemento,`ValidaCNPJ`): this.finaliza(input.elemento,false,'danger','Campo vazio')
                    this.Array.push(inputValida);
                }
                else{
                    let inputValida = input.elemento.value !== '' ?  this.operator(input.elemento,`Valida${input.valida}`): this.finaliza(input.elemento,false,'danger','Campo vazio')
                    this.Array.push(inputValida);
                }
                
            }
            else{
                [...input.elemento.children].forEach(children => {
                    let select; 
                    if(children.attributes.selected !== undefined)
                    {
                        select = this.ValidaSelect(input.elemento,children);
                        this.Array.push(select);
                    }
                })
                
            }
        });
        return this.Array;
    }
    
}
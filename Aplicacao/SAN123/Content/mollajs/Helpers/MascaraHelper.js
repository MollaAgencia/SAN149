class MascaraHelper{
    constructor(){
        throw new Error('Mascara não pode ser instanciada')
    }
    static _detectar_mobile() { 
        if( navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/webOS/i)
        || navigator.userAgent.match(/iPhone/i)
        || navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/iPod/i)
        || navigator.userAgent.match(/BlackBerry/i)
        || navigator.userAgent.match(/Windows Phone/i)
        ){
            return true;
        }
        else {
            return false;
        }
    }
    static operate(function_name, valorDoInput) {
        if(function_name === null)
            return
        return MascaraHelper[`mask${function_name}`](valorDoInput);
    }
    /**
     * @description Passar o valor do input
     * @param {} valorDoInput 
     */
    static maskCPF(valorDoInput){
        if(!this._detectar_mobile()){
            valorDoInput.value = valorDoInput.value.replace( /\D/g , ""); //Remove tudo o que não é dígito
            valorDoInput.value = valorDoInput.value.replace( /^(\d{3})(\d)/g , "$1.$2"); //Coloca ponto entre o segundo e o terceiro dígitos
            valorDoInput.value = valorDoInput.value.replace( /^(\d{3})\.(\d{3})(\d)/g , "$1.$2.$3"); //Coloca ponto entre o quinto e o sexto dígitos
            valorDoInput.value = valorDoInput.value.replace( /\.(\d{3})(\d)/g , ".$1-$2"); //Coloca uma barra entre o oitavo e o nono dígitos
            valorDoInput.value = valorDoInput.value.replace( /^(\d{2})(\d{1})\.(\d{2})(\d{1})\.(\d{2})(\d)\-(\d{2}\d)/ , "$1.$2$3.$4$5/$6$7"); //Coloca uma barra entre o oitavo e o nono dígitos
               valorDoInput.value = valorDoInput.value.replace( /(\d{4})(\d)/ , "$1-$2"); //Coloca um hífen depois do bloco de quatro dígitos*/
        return valorDoInput;
        }
         else{
             valorDoInput.value = valorDoInput.value.replace( /\D/g , "");
            valorDoInput.value = valorDoInput.value.replace( /^(\d{3})(\d{3})(\d{3})(\d{2})$/g , "$1.$2.$3-$4");
            return valorDoInput;
        }
    
    }
    static maskCNPJ(valorDoInput){
        
        valorDoInput.value = valorDoInput.value.replace( /\D/g , ""); //Remove tudo o que não é dígito
        valorDoInput.value = valorDoInput.value.replace( /^(\d{2})(\d)/ , "$1.$2"); //Coloca ponto entre o segundo e o terceiro dígitos
        valorDoInput.value = valorDoInput.value.replace( /^(\d{2})\.(\d{3})(\d)/ , "$1.$2.$3");
        valorDoInput.value = valorDoInput.value.replace( /^(\d{2})\.(\d{3})\.(\d{3})(\d)/ , "$1.$2.$3/$4"); //Coloca ponto entre o quinto e o sexto dígitos
        valorDoInput.value = valorDoInput.value.replace( /^(\d{2})\.(\d{3})\.(\d{3})\/(\d{4})(\d+)/ , "$1.$2.$3/$4-$5"); //Coloca ponto entre o quinto e o sexto dígitos
        // valorDoInput.value = valorDoInput.value.replace( /(\d{4})(\d)/ , "$1-$2"); //Coloca um hífen depois do bloco de quatro dígitos
        return valorDoInput;
        
    
    }
    static maskCEP(valorDoInput){
        valorDoInput.value = valorDoInput.value.replace( /\D/g , "");
        valorDoInput.value = valorDoInput.value.replace(/^(\d{5})(\d{3})$/,'$1-$2');
    
        return valorDoInput;
    }
    static maskTelefone(valorDoInput){
        if(!this._detectar_mobile()){
        valorDoInput.value = valorDoInput.value.replace( /\D/g , "");
        valorDoInput.value = valorDoInput.value.replace(/^(\d{2})/,'($1)');
        valorDoInput.value = valorDoInput.value.replace(/(\d{4,5})(\d{4})$/,'$1-$2');
        return valorDoInput;
        }
        else{
            valorDoInput.value = valorDoInput.value.replace( /\D/g , "");
            valorDoInput.value = valorDoInput.value.replace(/(\d{2})(\d{4,5})(\d{4})$/,'($1)$2-$3');
            return valorDoInput;
        }
    }
    static maskNumerico(valorDoInput){

        valorDoInput.value = valorDoInput.value.replace( /\D/g , "");
        return valorDoInput;
    }
    static maskData(valorDoInput){
        if(!this._detectar_mobile()){
            valorDoInput.value = valorDoInput.value.replace( /\D/g , "");
            valorDoInput.value = valorDoInput.value.replace(/^(\d{2})/g,'$1/');
            valorDoInput.value = valorDoInput.value.replace(/^(\d{2})[/](\d{2})/g,'$1/$2/');
            valorDoInput.value = valorDoInput.value.replace(/(\d{2})[/](\d{2})[/](\d{4})/g,'$1/$2/$3');
            return valorDoInput;
        }
         else{
             valorDoInput.value = valorDoInput.value.replace( /\D/g , "");
            valorDoInput.value = valorDoInput.value.replace(/(\d{2})(\d{2})(\d{4})/g,'$1/$2/$3');
            return valorDoInput;
        }
    }
}
class ValidaForms{
    constructor(elemento,value,mensagemType,mensagemTexto){
        this._elemento = elemento;
        this._value = value;
        this._name = elemento.getAttribute('name');
        this._mensagemType = mensagemType;
        this._mensagemTexto = mensagemTexto;
    }
    get elemento(){
        return this._elemento;
    }
    get value(){
        return this._value;
    }
    get name(){
        return this._name;
    }
    get mensagemType(){
        return this._mensagemType;
    }
    get mensagemTexto(){
        return this._mensagemTexto;
    }

}
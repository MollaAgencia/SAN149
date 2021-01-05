class  Mensagem{
    constructor(elemento){
        this._elemento = elemento;
        this._tipo = mensagemType;
        this._texto = mensagemTexto;
    }
    get elemento(){
        return this._elemento;
    }
    set tipo (tipo){
        return  this._tipo = tipo;
    }
    set texto(texto){
        return  this._texto = texto;
    }
    get tipo (){
        return  this._tipo;
    }
    get texto(){
        return  this._texto;
    }
    
}
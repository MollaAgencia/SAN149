class ViewMask extends View{
    constructor(elemento){
            super(elemento);
            this.update();
    }
    _templete(tipo,mensagem){
        return `<div class="alert alert-${tipo}">${mensagem}</div>`
    }

    update(){
        
            if(this._elemento === undefined){
                return true
            }
            else{
                
                this._elemento.value = this._elemento.value;
            }
    }
}
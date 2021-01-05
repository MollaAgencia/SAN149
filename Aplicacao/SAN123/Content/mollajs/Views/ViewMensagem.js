class ViewMensagem extends View{
    constructor(mensagem){
            super(mensagem);
    }
    _templete(tipo,mensagem){
        return `<div class="text-center alert alert-${tipo} p-sm">${mensagem}</div>`;
    }

    update(){
        
            if(this._elemento.tipo === 'success'){
                return true;
            }
            else{
                if(this._elemento.elemento.className == 'mensagemBack'){
                    this._elemento.elemento.innerHTML = this._elemento.texto;
                }
                else{
                    let html = document.createElement('div');
                    html.classList = 'mensagem';
                    html.innerHTML = this._templete(this._elemento.tipo,this._elemento.texto);
                    this._elemento.elemento.parentElement.append(html);
                }
            }
    }
}
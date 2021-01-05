class AjaxTeste{
    constructor(elementos,metodo,mensagem){
        this._elementos = elementos;
        this._metodo = metodo;
        this._mensagem = mensagem;

    }
    _mensagemErroBack(mesagemElemento,texto){
        mesagemElemento.texto = texto;
        new ViewMensagem(mesagemElemento);
    }
    _mensagemErro(mesagemElemento){
        mesagemElemento.tipo = 'danger';
        mesagemElemento.texto = 'Erro ao carregar os dados, atualize a pagina';
        new ViewMensagem({elemento:mesagemElemento});
    }
    get postAjaxTeste(){
        let retornaMensagemElemento = this._mensagem.nextElementSibling.querySelector('.mensagemBack');
        let objs = MontaObjs.ObjValidadosTransformaString(this._elementos);
        if('equipes.aspx/MTD_UsuarioEditar' == this._metodo){
            let pJson = JSON.parse(objs);
            pJson.pAtivo='false';
            objs = JSON.stringify(pJson); 
        }
        let mensagem = new Mensagem({elemento:retornaMensagemElemento});
        $.ajax({
            type: 'POST',
            url: this._metodo,//Metodo para ir no banco e fazer os procedimentos
            contentType: 'application/json;charset=utf-8',
            data:objs,
            dataType: 'json',
            beforeSend: function(){
                mensagem.tipo = 'info';
                mensagem.texto = '<div class="col-md-12 alert alert-info"> <i class="fa fa-spinner fa-spin"></i> Carregando...</div>';
                new ViewMensagem(mensagem);
            },           
            success: function (data) {
                let json = JSON.parse(data.d);
                MensagemHelper.limparDentro(this._mensagem.parentElement.querySelector('.mensagemBack'));
                if(json.PRP_Status){
                    if(elemento.tagName=='SELECT'){
                        json.PRP_Lista.map(lista => new ViewSelect2(elemento,lista))
                    }
                    else if(elemento.tagName=='TABLE'){
                        json.PRP_Lista.map(lista => new ViewTable(elemento,lista))
                        
                    }
                }
                else{
                    this._mensagemErroBack(mensagem);
                    
                }
            }.bind(this),
            error: function () {
                this._mensagemErroBack(mensagem,'<div class="alert alert-danger">Ocorreu algum erro, tente novamente!</div>');
            }.bind(this)
		}).done(function () {})
    }
}
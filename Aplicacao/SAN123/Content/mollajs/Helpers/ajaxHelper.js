class ajaxHelper{
    constructor(){
        
        throw new Error('Mascara não pode ser instanciada')
    }

    /**
     * Realliza a chamada ajax
     * @param {String} pJson string
     * @param {any} form
     * @param {any} pURL
     */

    MTD_ajaxPost(pJson, form, pURL) {
        
        let objs = MontaObjs.ObjValidadosTransformaString(elementos);
        let mensagem = new Mensagem({ elemento: retornaMensagemElemento });
        
        $.ajax({
            type: 'POST',
            url: metodo,//Metodo para ir no banco e fazer os procedimentos
            contentType: 'application/json;charset=utf-8',
            data: objs,
            dataType: 'json',
            beforeSend: function () {
                mensagem.tipo = 'info';
                mensagem.texto = '<div class="col-md-12 alert alert-info"> <i class="fa fa-spinner fa-spin"></i> Carregando...</div>';
                new ViewMensagem(mensagem);
            },
            success: function (data) {
                let json = JSON.parse(data.d);
                MensagemHelper.limparDentro(form.parentElement.querySelector('.mensagemBack'));
                if (json.PRP_Status) {
                    if (elemento.tagName == 'SELECT') {
                        json.PRP_Lista.map(lista => new ViewSelect2(elemento, lista))
                    }
                    else if (elemento.tagName == 'TABLE') {
                        json.PRP_Lista.map(lista => new ViewTable(elemento, lista))

                    }
                }
                else {
                    _mensagemErroBack(mensagem);

                }
            }.bind(this),
            error: function () {
                this._mensagemErroBack(mensagem);
            }.bind(this)
        }).done(function () { })
    }
    static AjaxListaCargoError(){
        mensagem.tipo = 'error';
        mensagem.texto = '<i class="fa fa-alert"></i> Ops! ocorreu algum erro, tente novamente!';
        new ViewMensagem(mensagem.elemento);
    }
    static AjaxListaCargoBeforesend(){
        let mensagem = new Mensagem(document.querySelector('#primeiroForm #cargos'));
        mensagem.tipo = 'info';
        mensagem.texto = '<i class="fa fa-spinner fa-spin"></i> Carregando ...';
        new ViewMensagem(mensagem);        
    }
    static _mensagemErroBack(mesagemElemento,texto){
        mesagemElemento.texto = texto;
        new ViewMensagem(mesagemElemento);
    }
    static _mensagemErro(mesagemElemento){
        mesagemElemento.tipo = 'danger';
        mesagemElemento.texto = 'Erro ao carregar os dados, atualize a pagina';
        new ViewMensagem(mesagemElemento);
    }
    static _mensagemCarregando(mesagemElemento){
        mensagem.tipo = 'info';
        mensagem.texto = '<i class="fa fa-spinner fa-spin"></i> Carregando ...';
        new ViewMensagem(mesagemElemento);
    }
    static getAjaxSelectCargo(elemento,form,metodo){
        let mensagem = new Mensagem(elemento);
        $.ajax({
		    type: 'POST',
		    url: metodo,//Metodo para ir no banco e fazer os procedimentos
		    contentType: 'application/json;charset=utf-8',
		    data:'',
		    dataType: 'json',
		    beforeSend: function(){
		    	mensagem.tipo = 'info';
                mensagem.texto = '<i class="fa fa-spinner fa-spin"></i> Carregando ...';
                new ViewMensagem(mensagem);
		    },           
		    success: function (data) {
                let json = JSON.parse(data.d);
                MensagemHelper.limpar2(elemento.parentElement.querySelector('.mensagem'));
                if(json.PRP_Requisicao.PRP_Status){
                        json.PRP_Lista.map(lista => new ViewSelect2(elemento,lista))
                }
                else{
                    this._mensagemErro(mensagem);
                    
                }
		    }.bind(this),
		    error: function () {
                this._mensagemErro(mensagem);
            }
		}).done(function () {
            if(form.className == "modal-body"){
                let select = form.querySelectorAll('.cargoSelectModal');
                Array.prototype.slice.call(select).forEach(el =>{
                    el.setAttribute('onchange', 'mollalibaryController.mudarSelecao(event)');
                    mudarSelecaoHelper.mudaSelecaoPrimeiro(el,el.getAttribute('data-selecionado'),0);
                    Select2Helper.select2Js(el);
                })
            }
        });
    }
    static getAjaxSelect(elemento,form,metodo){
        let mensagem = new Mensagem({elemento:elemento});
        $.ajax({
		    type: 'POST',
		    url: metodo,//Metodo para ir no banco e fazer os procedimentos
		    contentType: 'application/json;charset=utf-8',
		    data:'',
		    dataType: 'json',
		    beforeSend: function(){
		    	mensagem.tipo = 'info';
                mensagem.texto = '<i class="fa fa-spinner fa-spin"></i> Carregando ...';
                new ViewMensagem(mensagem);
		    },           
		    success: function (data) {
                let json = JSON.parse(data.d);
                MensagemHelper.limpar2(elemento.parentElement.querySelector('.mensagem'));
                if(json.PRP_Requisicao.PRP_Status){
                    json.PRP_Lista.map(lista => new ViewSelect2(elemento,lista))
                }
                else{
                    this._mensagemErro(mensagem);    
                }
		    }.bind(this),
		    error: function () {
                this._mensagemErro(mensagem);
            }
		}).done(function () {
            
            if(form.className == "modal-body"){
                let select = form.querySelector('.modalDistribuidorCpf');
                select.setAttribute('onchange', 'mollalibaryController.mudarSelecao(event)');
                mudarSelecaoHelper.mudaSelecaoPrimeiro(select,select.getAttribute('data-valor'),0);
                Select2Helper.select2Js(select);
            }
            
            else if(form.className == "modal-body segundaModal"){
                let select = form.querySelectorAll('.cargoSelectModal');
                Array.prototype.slice.call(select).forEach(el =>{
                    el.setAttribute('onchange', 'mollalibaryController.mudarSelecao(event)');
                    mudarSelecaoHelper.mudaSelecaoPrimeiro(el,el.getAttribute('data-selecionado'),0);
                    Select2Helper.select2Js(el);
                })
                cont ++;
                if(Array.prototype.slice.call(select).length-1 == cont){
                    form.classList.remove('segundaModal');
                    form.classList.remove('modal-body');
                    form.classList.add('modal-body');
                }
            }
            else{
                Select2Helper.select2Js(elemento);
            }
        });
    }
    
    static getAjaxTable(elemento,form,metodo){
        let mensagem = new Mensagem({elemento:elemento});
        $.ajax({
		    type: 'POST',
		    url: metodo,//Metodo para ir no banco e fazer os procedimentos
		    contentType: 'application/json;charset=utf-8',
		    data:'',
		    dataType: 'json',
		    beforeSend: function(){
		    	mensagem.tipo = 'info';
                mensagem.texto = '<i class="fa fa-spinner fa-spin"></i> Carregando ...';
                new ViewMensagem(mensagem);
		    },           
		    success: function (data) {
                let json = JSON.parse(data.d);
                MensagemHelper.limpar2(elemento.parentElement.querySelector('.mensagem'));
                if(json.PRP_Requisicao.PRP_Status){
                    new ViewTable(elemento,json.PRP_Usuarios)
                }
                else{
                    this._mensagemErro(mensagem);
                }
		    }.bind(this),
		    error: function () {
                this._mensagemErro(mensagem);
            }
		}).done(function () {
                $(elemento).DataTable();
        });
    }
    //TODO : Lucas depois coloca um comentario nesse m�todo
    /**
     * Lucas depois coloca um comentario nesse m�todo
     * @param {any} elementos
     * @param {any} form
     * @param {any} metodo
     */
    static postAjaxButton(elementos,form,metodo){
        let retornaMensagemElemento = form.nextElementSibling.querySelector('.mensagemBack');
        let objs = MontaObjs.ObjTransformaString(elementos); 
        let mensagem = new Mensagem({elemento:retornaMensagemElemento});
        $.ajax({
            type: 'POST',
            url: metodo,//Metodo para ir no banco e fazer os procedimentos
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
                MensagemHelper.limparDentro(form.parentElement.querySelector('.mensagemBack'));
                if(json.PRP_Requisicao.PRP_Status){
                    if('equipes.aspx/MTD_ObterUsuarioEditar'==metodo){
                        let ViewEditarUsuarios = new ViewEditarUsuario(json.PRP_Usuario);
                        ViewEditarUsuarios.criaInputs(form.querySelector('#mod_EditarPrimeiroTab'));
                        ajaxHelper.getAjaxSelect(form.querySelector('.modalDistribuidorCpf'),form,'equipes.aspx/MTD_ListaPdvs');
                    // form.querySelector('.modalDistribuidorCpf').setAttribute('onchange','mollalibaryController.mudarSelecao(event)');
                    }
                    else{
                        let ViewEditarUsuarios = new ViewEditarUsuario(json.PRP_Cargos);
                        ViewEditarUsuarios.criaInputs2(form.querySelector('#mod_EditarSegundoTab'));
                        let select = form.querySelectorAll('.cargoSelectModal');
                        Array.prototype.slice.call(select).forEach(el =>{
                            ajaxHelper.getAjaxSelectCargo(el,form,'equipes.aspx/MTD_ListaCargos');
                        });
                        
                    }
                }
                else{
                    this._mensagemErroBack(mensagem,json.PRP_Requisicao.PRP_Mensagem);
                    
                }
            }.bind(this),
            error: function () {
                this._mensagemErroBack(mensagem);
            }.bind(this)
		}).done(function () {
            
            
        }.bind(this))
    }
    static postAjax(elementos,form,metodo){
        let retornaMensagemElemento = form.nextElementSibling.querySelector('.mensagemBack');
        let objs = MontaObjs.ObjValidadosTransformaString(elementos);
        if('equipes.aspx/MTD_UsuarioEditar' == metodo){
            let pJson = JSON.parse(objs);
            pJson.pAtivo='false';
            objs = JSON.stringify(pJson); 
        }
        let mensagem = new Mensagem({elemento:retornaMensagemElemento});
        $.ajax({
            type: 'POST',
            url: metodo,
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
                MensagemHelper.limparDentro(form.parentElement.querySelector('.mensagemBack'));
                if(json.PRP_Status){
                    if(elemento.tagName=='SELECT'){
                        json.PRP_Lista.map(lista => new ViewSelect2(elemento,lista))
                    }
                    else if(elemento.tagName=='TABLE'){
                        json.PRP_Lista.map(lista => new ViewTable(elemento,lista))
                        
                    }
                }
                else{
                    _mensagemErroBack(mensagem);
                    
                }
            }.bind(this),
            error: function () {
                this._mensagemErroBack(mensagem,'<div class="alert alert-danger">Ocorreu algum erro, tente novamente!</div>');
            }.bind(this)
		}).done(function () {
            console.log(this);
        }.bind(this))
    }
    static postAjaxCargoUsuPeriodo(elementos,form,metodo){
        let retornaMensagemElemento = form.nextElementSibling.querySelector('.mensagemBack');
        let objs = MontaObjs.ObjValidadosTransformaString(elementos); 
        let mensagem = new Mensagem({elemento:retornaMensagemElemento});
        // let parametros = {};
        // parametros.pUSU_ID = parseInt(Usuario);
        // parametros.pFAS_ID = parseInt(Fase);
        // parametros.pCAR_ID = parseInt(Cargo);
        
        $.ajax({
            type: 'POST'
            , url: metodo
            , data: objs
            , contentType: 'application/json;charset=utf-8'
            , dataType: 'json'
            , beforeSend: function () {
                this._mensagemCarregando(mensagem);
            }.bind(this)
            , success: function (data) {
                let json = JSON.parse(data.d);
                console.log(json);
                mensagem.texto = json.PRP_Mensagem;
                new ViewMensagem(mensagem);
                
            }
            , error: function () {
                //ajustar a exibição da mensagem
                console.log('erro');
            }
            , complete: function () {
                
            }
        });
    }
    static postDesativarButton(elementos,form,metodo){
        let retornaMensagemElemento = form.nextElementSibling.querySelector('.mensagemBack');
        let objs = MontaObjs.ObjValidadosTransformaString(elementos); 
        let mensagem = new Mensagem({elemento:retornaMensagemElemento});
        // let parametros = {};
        // parametros.pEquipe = parseInt($(this).data('identificador'));
        // let $this = $(this);
        $.ajax({
            type: 'POST'
            , url: metodo//'equipes.aspx/MTD_EquipeDesabilita'
            , data: objs//JSON.stringify(parametros)
            , contentType: 'application/json;charset=utf-8'
            , dataType: 'json'
            , beforeSend: function () {
                this._mensagemCarregando(mensagem);
            }
            , success: function (data) {
                var json = JSON.parse(data.d);
                console.log(json);
                if (json.PRP_Status) {
                    if ($('#Equipe_PDV_Filtro').val() == '0') {
                        alert('selecione uma distribuidora');
                    }
                    else {
                        FUN_ListaEquipes($('#Equipe_PDV_Filtro').val());
                    }
                }
                
                $('#div_modalMensagem').html(json.PRP_Mensagem);
                $('#mod_Mensagem').modal('show');
            }
            , error: function () {
                //ajustar a exibição da mensagem
                console.log('erro');
            }
            , complete: function () {
                
            }
        })
    }
}
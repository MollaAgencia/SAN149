class MollalibaryController{
    constructor(){
        
        this._All = document.querySelectorAll.bind(document);
        this._$ = document.querySelector.bind(document);
        this._form1 = this._$('#loginForm');    
        this._form2 = this._$('form #segundoForm');
        
    }
    mask(event){
        event.preventDefault();
        MascaraHelper.operate(event.target.getAttribute('data-valida'),event.target);
    }

    login(event){
        event.preventDefault();
        if(validaForm($(this._form1))){
            $(this._form1).submit(); 
        }
    }
    cadastro(event){
        event.preventDefault();
        if(validaForm($(event.target).parents('.modal-body'))){
            //$(event.target).submit(); 
        }
    }
    esqueciSenha(event){
        event.preventDefault();
        if(validaForm($(event.target).parents('.modal-body'))){
            //$(event.target).submit(); 
        }
    }
    contato(event){
        event.preventDefault();
        if(validaForm($(event.target).parents('.modal-body'))){
            //$(event.target).submit(); 
        }
    }
}
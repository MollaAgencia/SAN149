class FormInputs{
    constructor(elemento)
    {
        this._elemento = elemento;
        this._inputs = this._elemento.querySelectorAll('[data-obrigatorio]');
        this._inputArray = [];
        this.TransformaInputsArray = this._inputs;
    }

    
    set TransformaInputsArray(inputs){
        inputs.forEach(input => this._inputArray.push({elemento:input,valida:input.getAttribute('data-valida')}));
    }
    get InputsArray(){
        return [].concat(this._inputArray);
    }
    _limpaArray(){
        this._inputArray = [];
    }
}
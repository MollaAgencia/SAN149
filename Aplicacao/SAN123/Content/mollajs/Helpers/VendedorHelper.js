class CodigoVendedorHelper{
    constructor(){
        throw new Error('Mascara não pode ser instanciada')
    }
    static removeAtribute(input){
        return input.querySelector('input').removeAttribute('data-obrigatorio');
    }
    static addAtribute(input){
        return input.querySelector('input').setAttribute('data-obrigatorio','true');
    }
    static valida(elemento){
        return elemento.value;
    }
}
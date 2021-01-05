class MensagemHelper{
    constructor(){
        throw new Error('Mascara não pode ser instanciada')
    }
    static limpar(elementos){
        elementos.forEach(elemento => elemento.remove())
    }
    static limpar2(elemento){
        elemento.remove() !== null ? elemento.remove() : false;
    }
    //TODO : Lucas depois coloca um comentario nesse método
    /**
     * Lucas depois coloca um comentario nesse método
     * @param {any} elemento
     */
    static limparDentro(elemento){
        elemento.firstChild !== null ? elemento.firstChild.remove():false;
    }
    static limpa(elementos){
        elementos !== null ? elementos.length !==0 ? this.limpar(elementos):false :false
            
    }
}
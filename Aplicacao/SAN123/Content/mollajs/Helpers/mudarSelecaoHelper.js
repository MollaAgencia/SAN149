class mudarSelecaoHelper {
    constructor(){
        throw new Error('Mascara não pode ser instanciada')
    }
    /**
     * Isira o elemento desejado
     * @param {object} ele Insira o Select desejado
     * @param {string} sel Valor a ser mudado
     */
    static mudaSelecaoPrimeiro(ele,sel){
        Array.prototype.slice.call( ele ).forEach(function(el,cont){
            el.removeAttribute('selected');
            if(Array.prototype.slice.call(ele).length-1 == cont){
                Array.prototype.slice.call(ele).forEach(elemento=>{
                    
                    if(sel==elemento.value)
                    {
                        if(sel!=null||sel!=''||sel!=undefined){
                            elemento.setAttribute('selected','selected');
                        }
                    }            
                })
            }
        });
        
    }
    /**
     * Isira o elemento desejado
     * @param {object} ele Insira o Select desejado
     * @param {string} sel Valor a ser mudado
     */
    static mudaSelecao(ele,sel){
        
        Array.prototype.slice.call(ele).forEach(el=>{
                if(sel==el.value)
                {
                    
                    el.removeAttribute('selected');
                    
                }
            // if(el.text == "Selecione" && el.getAttribute('selected') == 'selected'){
            //     el.removeAttribute('selected');
            //     if(sel!=null||sel!=''||sel!=undefined){
            //         [...ele].forEach(els=>{
                        
            //         });
            //     }
            // }
        })
    }
}

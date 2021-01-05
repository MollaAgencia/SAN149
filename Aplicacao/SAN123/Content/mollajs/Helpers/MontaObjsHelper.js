class MontaObjs{
    constructor(){
        throw new Error('Mascara não pode ser instanciada')
    }
    static ObjTransformaString(elementos){
        let objs = `{${elementos.map((elemento,index) =>{
            
                if(index != elementos.length-1){
                    return `"${elemento.name}":"${elemento.value}",`
                } 
                else{
                    return `"${elemento.name}":"${elemento.value}"`
                }
            
        }
        ).join('')}}`
        return objs;
    }
    static ObjValidadosTransformaString(elementos){
        let objs = `{${elementos.map((elemento,index) =>{
            if(elemento.elemento.tagName=='INPUT'){
                if(index != elementos.length-1){
                    return `"${elemento.name}":"${elemento.elemento.value}",`
                } 
                else{
                    return `"${elemento.name}":"${elemento.elemento.value}"`
                }
            }
            else if(elemento.elemento.tagName == 'SELECT'){
                if(index != elementos.length-1){
                    return `"${elemento.name}":"${elemento.elemento.querySelector('[selected]').value}",`
                } 
                else{
                    return `"${elemento.name}":"${elemento.elemento.querySelector('[selected]').value}"`
                }
            }
            else if(elemento.elemento.tagName == 'BUTTON'){
                if(index != elementos.length-1){
                    return `"${elemento.name}":"${elemento.elemento.getAttribute('data-valor')}",`
                } 
                else{
                    return `"${elemento.name}":"${elemento.elemento.getAttribute('data-valor')}"`
                }
            }
        }
        ).join('')}}`
        return objs;
    }
}
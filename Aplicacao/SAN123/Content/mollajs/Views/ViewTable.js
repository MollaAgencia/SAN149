class ViewTable extends View{
    constructor(elemento,obj){
        super(elemento);
        this.update(obj);
    }

    _template(obj){
        let tableTd = [];
        let html='';
        obj.map(el=>{
            html +=
                `<tr data-indentificador="${el.USU_ID}">
                ${
                    Object.entries(el).map(p => {
                        if (p[0] != 'USU_Ativo') {
                            return `<td>${p[1]}</td>`
                        }
                        else{
                            return `
                            <td>
                                <button class="btn btn-xs btn-primary" name="pUSU_ID" value="${el.USU_ID}" onclick="mollalibaryController.editarUsuarios(event)">Editar</button>
                                <button class="btn btn-xs btn-secondary detalhe" name="pUSU_ID" onclick="mollalibaryController.usuarioDetalhe(event)">Detalhe</button>
                                ${
                                    (el.USU_Ativo == false)
                                    ?
                                        `<button type="button" class="btn btn-xs btn-success ativar" value="${el.USU_ID} name="pUSU_ID" onclick="mollalibaryController.usuarioAtivar(event)">Ativar</button>`
                                    :
                                        `<button type="button" class="btn btn-xs btn-danger desativar" value="${el.USU_ID}" name="pUSU_ID" onclick="mollalibaryController.usuarioDesativar(event)">Desativar</button>`                                
                                }
                            </td>
                            `
                        }               
                    }).join('')
                }
                </tr>`
        });
        return html;
    }
    update(obj){
        let tbody = this._elemento.querySelector('tbody');
        let retorno = this._template(obj);
        tbody.innerHTML = retorno;
        // retorno.forEach(el=>tbody.append(el));
        // tbody.append(retorno);
    }
}
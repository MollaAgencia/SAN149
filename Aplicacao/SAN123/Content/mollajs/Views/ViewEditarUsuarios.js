class ViewEditarUsuario extends View{
    constructor(elemento){
        super(elemento);
    }
    _templeteCriaInputs(json){
        let InputsEditar = [];
        for(let item in json){
            if(/PDV_ID/g.test(item)){
                var pdvId = json[item];
            }
            if(/PDV_ID/g.test(item))continue;
            let divCol = document.createElement('div');
            divCol.className='col-md-4 mt-md';
            let divFormGroup = document.createElement('div');
            divFormGroup.className='form-group';
            let label = document.createElement('label');
            if(/PDV_RazaoSocial/g.test(item)){
                var input = document.createElement('select');
                let option = document.createElement('option');
                input.setAttribute('data-valor',pdvId);
                option.setAttribute('selected','selected');
                option.setAttribute('hidden','hidden');
                option.innerText="Selecione";
                input.append(option);

            }
            else{
                var input = document.createElement('input');
            }

            if(/USU_Nome|USU_Login|CAR_Display|PDV_ID|USU_Email|USU_Celular|USU_ID|PDV_RazaoSocial/g.test(item)){
                input.setAttribute('data-obrigatorio','true');    
            }
            
            if(/PDV_RazaoSocial|CAR_Display/g.test(item)){
                if(/PDV_RazaoSocial/g.test(item)){
                    
                    input.classList.add('modalDistribuidorCpf');
                }
                else{
                    input.classList.add('modalCargo');
                }
            }
            let campo = item.replace(/USU_|FAS_|PDV_|CAR_/g,'');
            if('RazaoSocial'==campo){

                label.innerHTML='Razão Social';
            }
            else{
                
                label.innerHTML=campo;
            }
            if(/USU_CPF|USU_ID|USU_Login|FAS_Display|CAR_Display/g.test(item)){
                input.setAttribute('disabled','true');    
            }
            
            input.classList.add('form-control');
            input.setAttribute('value',json[item]);
            input.setAttribute('name',item);
            divCol.append(divFormGroup);
            divFormGroup.append(label);
            divFormGroup.append(input);
            InputsEditar.push(divCol);
        }
        InputsEditar
        return InputsEditar;
    }
    _templeteCriaInputs2(json){
        // for(let item in json){
            let html =`
            <div class="col-md-4 text-center">
                <h4>${json.fase_Display}</h4>
                <div class="form-group">
                    <input class="form-control cargoinputModal hide" name="pFAS_ID" data-obrigatorio="true" value="${json.fase_Identificador}" disabled="true">
                </div>           
            </div>
            <div class="col-xs-6 col-md-4">
                <div class="form-group">
                    <i class="iconeLock fa fa-${json.fase_Encerrada?'lock':'unlock'}"></i>
                    <select class="form-control cargoSelectModal" name="pCAR_ID" data-obrigatorio="true" data-selecionado="${json.cargo_IDentificador}" ${json.fase_Encerrada?`disabled="true"`:''}>
                        <option hidden selected>Selecione</option>
                    </select>
                </div>
            </div>
            <div class="col-xs-6 col-md-4 text-center">
                <button class="btn btn-primary" data-obrigatorio="true"  name="pUSU_ID" onclick="mollalibaryController.enviaEditarUsuCargo(event)" ${json.fase_Encerrada?`disabled="true"`:''} data-valor="${json.Usuario}">${json.fase_Encerrada?`<i class="fa fa-lock"></i> Salvar`:'<i class="fa lock-open"></i> Salvar'}</button>
            </div>
            `
            var row = document.createElement('div');
            row.className='row';
            row.innerHTML = html;
        // }
            
        return row;
        
        
        
    }
    criaInputs(elemento){
        let elementosRertorno = this._templeteCriaInputs(this._elemento);
        let row =  document.createElement('div');
        let container =  document.createElement('div');
        container.className='container novoInput';
        row.className='row';
        elementosRertorno.map(elementoRetorno => row.append(elementoRetorno));
        container.append(row);
        elemento.append(container);
    }
    criaInputs2(elemento){
        let InputsEditar = [];
        this._elemento.forEach(el=>{
            var elementosRertorno = this._templeteCriaInputs2(el);
            InputsEditar.push(elementosRertorno);
        });
        let row =  document.createElement('div');
        let container =  document.createElement('div');
        container.className='container novoInput';
        row.className='col-md-12';
        InputsEditar.map(elementoRetorno => row.append(elementoRetorno));
        container.append(row);
        elemento.append(container);
        // let row =  document.createElement('div');
        // let container =  document.createElement('div');
        // container.className='container novoInput';
        // row.className='row';
        // elementosRertorno.map(elementoRetorno => row.append(elementoRetorno));
        // container.append(row);
        // elemento.append(container);
    }
}
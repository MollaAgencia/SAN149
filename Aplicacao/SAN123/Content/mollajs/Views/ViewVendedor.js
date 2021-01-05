class ViewCodigoVendedor extends View{
    constructor(elemento,objs){
        super(elemento);
        this.update(objs);
    }
    _templete(elemento){

    }
    update(objs){
        $(this._elemento).append(new Option(objs.codigo, objs.id, true, true)).trigger(0,"change");
    }
}
class ViewSelect2 extends View{
    constructor(elemento,objs){
        super(elemento);
        this.update(objs);
    }
    _templete(objs){
        return new Option(objs.PRP_Display, objs.PRP_value, false, false)
    }
    update(objs){
        $(this._elemento).append(this._templete(objs)).trigger("change");
    }
}
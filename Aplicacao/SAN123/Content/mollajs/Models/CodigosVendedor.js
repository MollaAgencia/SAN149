class CodigosVendedor{
    constructor(id,codigo){
        this._codigo = codigo ;
        this._id = id ;
        
    }
    get id(){
        return this._id;
    }
    get codigo(){
        return this._codigo;
    }
}
class ajaxGET{
    constructor(){
        this._elemento ='';
        this._url ='';
        this._json ='';
        this._ajaxBefore ='';
        this._ajaxSuccess ='';
        this._ajaxError ='';
        this._ajaxDone ='';
    }
set setUrl(url){
    return this._url = url;
}
set setElemento(elemento){
    return this._elemento = elemento;
}
set setJson(json){
    return this._json = json;
}
set setAjaxBefore(ajaxBefore){
    return this._ajaxBefore = ajaxBefore;
}
set setAjaxSuccess(ajaxSuccess){
    return this._ajaxSuccess = ajaxSuccess;
}
set setAjaxError(ajaxError){
    return this._ajaxError = ajaxError;
}
set setAjaxDone(ajaxDone){
    return this._ajaxDone = ajaxDone;
}


get url(){
    return this._url;
}
get elemento(){
    return this._elemento;
}
get json(){
    return this._json;
}
get ajaxBefore(){
    return this._ajaxBefore;
}
get ajaxSuccess(){
    return this._ajaxSuccess;
}
get ajaxError(){
    return this._ajaxError;
}
get ajaxDone(){
    return this._ajaxDone;
}


ajax(){
    $.ajax({
        type: 'POST',
        url: this.url,
        contentType: 'application/json;charset=utf-8',
        data:this.json,
        dataType: 'json',
        beforeSend: function(){
            window[this.ajaxBefore]();
        }.bind(this),           
        success: function (data) {
            let json = JSON.parse(data.d);
            this.ajaxSuccess(json);
        }.bind(this),
        error: function (requestObject, error, errorThrown) {
            window[this.ajaxError]();
        }.bind(this)
    }).done(function () {
        window[this.ajaxDone]();
    });
    
}
}
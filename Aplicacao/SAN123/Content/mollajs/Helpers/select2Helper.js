class Select2Helper{
    constructor(){
        throw new Error('Mascara não pode ser instanciada')
    }

    static select2Js(select){
        $(select).select2();
        $(select).on("select2:select", function (e) {
            var options = $(this).find('option');
            var select2Option = $(this).next().text();
            $.each(options, function (index, option) {
                if ($(option).text() == select2Option) {
                    options.removeAttr('selected');
                    $(option).attr('selected', true)
                    console.log(option)
                }
            })
        });
    }
}


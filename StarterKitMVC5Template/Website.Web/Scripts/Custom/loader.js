
Website__Name.Loader = (function () {

    var objToReturn = {};

    /*
    PRIVATE
    */
    var initializeLoader = function (appendSelector) {
        $("<div>").attr({ 'id': "overlay_with_loader", 'style': "background-color: rgba(0, 0, 0, 0);" }).appendTo(appendSelector);
    }

    /*
    PUBLIC
    */
    var showLoader = function () {
        if ($('#overlay_with_loader').length == 0)
            initializeLoader("body");
        return $('#overlay_with_loader').show();
    }

    var hideLoader = function () {
        var objLoader = $('#overlay_with_loader');
        if (objLoader.length == 0)
            return;
        return objLoader.hide();
    }


    objToReturn.showLoader = showLoader;
    objToReturn.hideLoader = hideLoader;
    return objToReturn;
})();
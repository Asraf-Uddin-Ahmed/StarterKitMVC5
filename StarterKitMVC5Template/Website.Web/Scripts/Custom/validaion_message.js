
Website__Name.ValidationMessage.Bootstrap = (function () {

    var objToReturn = {};

    /*
    PRIVATE
    */
    var showMessage = function ($element, message, alertClass) {
        $element.empty();
        $element.prepend("<div class='alert " + alertClass + "'><a class='close' data-dismiss='alert' href='#'>×</a><p>" + message + "</p></div>");
    }


    /*
    PUBLIC
    */
    var showSuccessMessage = function ($element, message) {
        showMessage($element, message, 'alert-success');
        return $element;
    }

    var showErrorMessage = function ($element, message) {
        showMessage($element, message, 'alert-danger');
        return $element;
    }

    var showInfoMessage = function ($element, message) {
        showMessage($element, message, 'alert-info');
        return $element;
    }

    var showWarningMessage = function ($element, message) {
        showMessage($element, message, 'alert-warning');
        return $element;
    }

    objToReturn.showSuccessMessage = showSuccessMessage;
    objToReturn.showErrorMessage = showErrorMessage;
    objToReturn.showInfoMessage = showInfoMessage;
    objToReturn.showWarningMessage = showWarningMessage;
    return objToReturn;
})();


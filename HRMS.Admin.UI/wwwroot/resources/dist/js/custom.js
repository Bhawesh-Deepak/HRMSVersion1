/*
 * toast notifications
 */

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
    "tapToDismiss": false
}

/*
 * jQuery plugin to allow only numerics in a text box
 * Also disables copy paste.
 */
$.fn.allowNumeric = function () {

    $this = $(this);
    $this.on('keypress', function (e) {
        var specialKeys = new Array();
        specialKeys.push($.ui.keyCode.BACKSPACE);
        specialKeys.push($.ui.keyCode.PERIOD);
        specialKeys.push(46);

        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    });
};
$.fn.allowNegNumeric = function () {

    $this = $(this);
    $this.on('keypress', function (e) {
        var specialKeys = new Array();
        specialKeys.push($.ui.keyCode.BACKSPACE);
        specialKeys.push($.ui.keyCode.PERIOD);
        specialKeys.push(46);
        specialKeys.push(45);
        specialKeys.push(37);
        specialKeys.push(39);
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    });
};
$.fn.allowInteger = function () {

    $this = $(this);
    $this.on('keypress', function (e) {
        var specialKeys = new Array();
        specialKeys.push($.ui.keyCode.BACKSPACE);
        specialKeys.push($.ui.keyCode.PERIOD);

        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    });
};

$.fn.allowAlphabets = function () {

    $this = $(this);
    $this.on('keypress', function (e) {
        var specialKeys = new Array();
        specialKeys.push($.ui.keyCode.BACKSPACE);
        specialKeys.push($.ui.keyCode.SPACE);

        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode > 64 && keyCode < 91) || (keyCode > 96 && keyCode < 123) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    });
};

function disableCopyPaste() {
    //below javascript is used for Disabling right-click on HTML page
    document.oncontextmenu = new Function("return false"); //Disabling right-click

    //below javascript is used for Disabling text selection in web page
    document.onselectstart = new Function("return false"); //Disabling text selection in web page

    if (window.sidebar) {
        document.onmousedown = new Function("return false");
        document.onclick = new Function("return true");

        //Disable Cut into HTML form using Javascript 
        document.oncut = new Function("return false");

        //Disable Copy into HTML form using Javascript 
        document.oncopy = new Function("return false");

        //Disable Paste into HTML form using Javascript 
        document.onpaste = new Function("return false");
    }
};


function LimtCharacters(txtMsg, CharLength, indicator) {
    chars = txtMsg.value.length;
    document.getElementById(indicator).innerHTML = CharLength - chars;
    if (chars > CharLength) {
        txtMsg.value = txtMsg.value.substring(0, CharLength);
    }
};

function CheckLength(Id, DId, totlen) {
    var len = Id.value.replace(/(\r\n|\n|\r)/gm, "").length;
    if (len > totlen) {
        Id.value = Id.value.substring(0, totlen);
    }
    else {
        $('#' + DId).text(len + "/" + totlen + " characters");
    }
}

function getQueryStringParam(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};
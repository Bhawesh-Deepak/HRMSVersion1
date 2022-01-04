function Success(response) {
    if (response == "Server error please contact admin team") {
       alertify.set('notifier', 'position', 'top-right');
        alertify.error(response);
    }
    else {
        alertify.set('notifier', 'position', 'top-right');
        alertify.success(response);
    }

    $("#form")[0].reset();//reset the form controll.
    $(".form-control").val('');//Clear the controll which is present inside the form.

    setTimeout(function () {
        location.reload();
    }, 300)
}


function CustomDelete(id, url) {
    alertify.set('notifier', 'position', 'top-right');
    var defered = $.Deferred();
    alertify.confirm("Are you sure want to Inactivate this record ?", function () {
        $.get(url, { Id: id }, function (response) {
            alertify.success('Record deactivated successfully')
            return defered.promise();

        });
    }, function () {
        alertify.error('There is some server Error please contact admin team .')
    });
}

function CustomDeleteRecord(id, getUrl, deleteUrl, event) {
    alertify.set('notifier', 'position', 'top-right');
    alertify.confirm("Are you sure want to delete the record ?", function () {

        var thisId = $(event);
        thisId.button('loading');

        $.get(deleteUrl, { id: id }, function (response) {
            alertify.success("Record deleted successfully");
        }).done(function () {
            $.get(getUrl, function (response) {
                $("#divHRMS").html(response);
            }).done(function () {
                $("#tblDataList").DataTable({
                    fixedHeader: true,
                    select: true,
                    responsive: true
                });
            });

            $(".form-control").val('');//Clear the controll which is present inside the form.
        });
    }, function () {
        alertify.warning("You cancel the delete.");
    });
}

function UpdateCustomRecord(id, updateUrl,textData) {
    $("#modalTitle").text(textData)
    $('#divLoader').modal('show');
    $.get(updateUrl, { id: id }, function (response) {
        $("#divHRMSCreate").html(response);
        $("#divModalPop").show();
    }).done(function () {
        $('#divLoader').modal('hide');
    });
}

function GetCustomRecord(getUrl, divId) {
    $('#divLoader').modal('show');
    $.get(getUrl, function (response) {
        $("#" + divId).html(response);
    }).done(function () {
        $('#divLoader').modal('hide');
    });
}

function NewCustomRecord(url, textData) {
    $("#modalTitle").text(textData)
    $('#divLoader').modal('show');
    $.get(url, function (response) {
        $("#divHRMSCreate").html(response);
        $("#divModalPop").show();
    }).done(function () {
        $('#divLoader').modal('hide');
    });
}

function AjaxOnBegin() {
    $('#divLoader').modal('show');
}
function AjaxComplete() {
    $('#divLoader').modal('hide');
}

function CustomFormSubmitBegin() {
    $('#divLoader').modal('show');
}

function CustomFormSubmitComplete() {
    $('#divLoader').modal('hide');
   
}
function HidePopUp() {
    $("#divModalPop").hide();
}
function GetInfo() {
    alert("Information")
}

//use onfocus in textbox
function RemoveZero(e) {
    if (e.value == "0") {
        e.value = "";
    }
}
//use onblur in textbox
function SetZeroIfEmpty(e) {
    if (e.value == "") {
        e.value = "0";
    }
}


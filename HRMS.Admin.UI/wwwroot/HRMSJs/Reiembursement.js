function GetReimbursementDetails() {
    GetCustomRecord("/EmployeeReimbursement/GetReimbursementDetails", "divHRMS")
}

$(document).ready(function () {
    GetReimbursementDetails();
})

function AddReiembursement() {
    NewCustomRecord("/EmployeeReimbursement/CreateReimbursement", "Create Reimbursement")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/EmployeeReimbursement/GetReimbursementDetails", "/EmployeeReimbursement/DeleteReimbursement", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/EmployeeReimbursement/CreateReimbursement", "Update Reimbursement", "Update Reimbursement");
}

function Accept(id) {
    $('#divLoader').modal('show');
    $.get("/EmployeeReimbursement/AcceptReimbursement", { id: id }, function (response) {
        alertify.success('Reimbursement Accepted !!!');
        $('#divLoader').modal('hide');
        location.reload();
        return defered.promise();
    }).done(function () {
        $('#divLoader').modal('hide');
    });
}
function Reject(id) {
    UpdateCustomRecord(id, "/EmployeeReimbursement/RejectReimbursement", "Reject  Reimbursement", "Reject Reimbursement");
}
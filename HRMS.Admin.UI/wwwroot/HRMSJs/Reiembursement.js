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
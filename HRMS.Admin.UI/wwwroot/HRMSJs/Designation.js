function GetDesignationList() {
    GetCustomRecord("/Designation/GetDesignationList", "divHRMS")
}

$(document).ready(function () {
    GetDesignationList();
})

function AddDesignation() {
    NewCustomRecord("/Designation/CreateDesignation", "Create Designation")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Designation/GetDesignationList", "/Designation/DeleteDesignation", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Designation/CreateDesignation", "Update Designation");
}
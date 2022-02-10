function GetDepartmentList() {
    GetCustomRecord("/Department/GetDepartmentList", "divHRMS")
}

$(document).ready(function () {
    GetDepartmentList();
})

function AddDepartment() {
    NewCustomRecord("/Department/CreateDepartment", "Create Department")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Department/GetDepartmentList", "/Department/DeleteDepartment", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Department/CreateDepartment", "Update Department", "Update Department");
}

function DeleteDesignation(id, eData) {
    CustomDeleteRecord(id, "/Department/GetDepartmentList", "/Department/DeleteDesignation", eData);
}

function UpdateDesignation(id) {
    UpdateCustomRecord(id, "/Department/GetDesignation", "Update Designation", "Update Designation");
}
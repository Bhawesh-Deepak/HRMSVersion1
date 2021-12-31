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
    UpdateCustomRecord(id, "/Department/CreateDepartment", "Create Department", "Update Department");
}
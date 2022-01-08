function GetEmployeeTypeList() {
    GetCustomRecord("/Employeetype/GetEmployeeTypeList", "divHRMS")
}

$(document).ready(function () {
    GetEmployeeTypeList();
})

function AddEmpType() {
    NewCustomRecord("/Employeetype/CreateEmployeeType", "Create Employee Type")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Employeetype/GetEmployeeTypeList", "/Employeetype/DeleteEmployeeType", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Employeetype/CreateEmployeeType", "Update Employee Type");
}
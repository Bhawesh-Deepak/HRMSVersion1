function GetEmployeeAssetsList() {
    GetCustomRecord("/EmployeeAssets/GetEmployeeAssetsList", "divHRMS")
}

$(document).ready(function () {
    GetEmployeeAssetsList();
})

function AddHolidays() {
    NewCustomRecord("/EmployeeAssets/EmployeeAssetsCreate", "Create Employee Assets")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/EmployeeAssets/GetEmployeeAssetsList", "/EmployeeAssets/DeleteEmployeeAssets", eData);
}

function UpdateRecordHolidays(id) {
    UpdateCustomRecord(id, "/EmployeeAssets/EmployeeAssetsCreate", "Update Employee Assets");
}
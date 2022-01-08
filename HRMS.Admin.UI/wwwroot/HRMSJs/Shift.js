function GetShiftList() {
    GetCustomRecord("/Shift/GetShiftList", "divHRMS")
}

$(document).ready(function () {
    GetShiftList();
})

function AddEmpType() {
    NewCustomRecord("/Shift/CreateShift", "Create Shift")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Shift/GetShiftList", "/Shift/DeleteShift", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Shift/CreateShift", "Update Shift");
}
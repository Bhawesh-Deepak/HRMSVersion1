function GetLeaveTypeList() {
    GetCustomRecord("/LeaveType/GetLeaveTypeList", "divHRMS")
}

$(document).ready(function () {
    GetLeaveTypeList();
})

function AddLeaveType() {
    NewCustomRecord("/LeaveType/CreateLeaveType", "Create  Leave Type")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/LeaveType/GetLeaveTypeList", "/LeaveType/DeleteLeaveType", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/LeaveType/CreateLeaveType", "Update  Leave Type", "Update Leave Type");
}
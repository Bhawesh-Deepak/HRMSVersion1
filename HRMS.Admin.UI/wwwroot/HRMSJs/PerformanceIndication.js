function GetBranchListList() {
    GetCustomRecord("/PerformanceIndication/GetBranchList", "divHRMS")
}

$(document).ready(function () {
    //GetBranchListList();
})

function AddRecord() {
    NewCustomRecord("/PerformanceIndication/CreatePerformanceIndication", "Create Performance Indication")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/PerformanceIndication/GetBranchList", "/PerformanceIndication/DeleteBranch", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/PerformanceIndication/CreatePerformanceIndication", "Update Performance Indication");
}
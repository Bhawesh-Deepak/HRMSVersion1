function GetBranchListList() {
    GetCustomRecord("/Termination/GetBranchList", "divHRMS")
}

$(document).ready(function () {
   // GetBranchListList();
})

function AddTermination() {
    NewCustomRecord("/Termination/CreateTermination", "Create Termination")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Termination/GetBranchList", "/Termination/DeleteBranch", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Termination/CreateBranch", "Update Branch");
}
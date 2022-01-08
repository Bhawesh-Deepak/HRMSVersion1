function GetBranchListList() {
    GetCustomRecord("/Branch/GetBranchList", "divHRMS")
}

$(document).ready(function () {
    GetBranchListList();
})

function AddRecord() {
    NewCustomRecord("/Branch/CreateBranch", "Create Branch")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Branch/GetBranchList", "/Branch/DeleteBranch", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Branch/CreateBranch", "Update Branch");
}
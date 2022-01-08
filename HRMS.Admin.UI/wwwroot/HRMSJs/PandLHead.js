function GetPandLnameList() {
    GetCustomRecord("/PandLname/GetPandLList", "divHRMS")
}

$(document).ready(function () {
    GetPandLnameList();
})

function AddPandL() {
    NewCustomRecord("/PandLname/CreatePandLname", "Create P and L Name")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/PandLname/GetPandLList", "/PandLname/DeletePandLname", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/PandLname/CreatePandLname", "Update P and L Name");
}
function GetAwardsList() {
    GetCustomRecord("/Awards/GetAwardsList", "divHRMS")
}

$(document).ready(function () {
    GetAwardsList();
})

function AddAwards() {
    NewCustomRecord("/Awards/CreateAwards", "Create Awards")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Awards/GetAwardsList", "/Awards/DeleteAwards", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Awards/CreateAwards", "Update Awards");
}
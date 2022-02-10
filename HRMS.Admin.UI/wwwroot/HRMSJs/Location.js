function GetLocationList() {
    GetCustomRecord("/Location/GetLoctionList", "divHRMS")
}

$(document).ready(function () {
    GetLocationList();
})

function AddLocation() {
    NewCustomRecord("/Location/CreateLocation", "Create Location")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Location/GetLoctionList", "/Location/DeleteLocation", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Location/CreateLocation", "Update Location");
}
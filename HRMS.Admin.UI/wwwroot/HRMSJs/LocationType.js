function GetLocationTypeList() {
    GetCustomRecord("/LocationType/GetLocationTypeList", "divHRMS")
}

$(document).ready(function () {
    GetLocationTypeList();
})

function AddRecord() {
    NewCustomRecord("/LocationType/CreateLocationType", "Create Location Type")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/LocationType/GetLocationTypeList", "/LocationType/DeleteLocationType", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/LocationType/CreateLocationType", "Update Location Type");
}
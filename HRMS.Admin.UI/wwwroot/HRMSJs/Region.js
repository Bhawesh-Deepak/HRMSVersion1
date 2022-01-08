function GetRegionList() {
    GetCustomRecord("/Region/GetRegionList", "divHRMS")
}

$(document).ready(function () {
    GetRegionList();
})

function AddRegion() {
    NewCustomRecord("/Region/CreateRegion", "Create Region")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Region/GetRegionList", "/Region/DeleteRegion", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Region/CreateRegion", "Update Region");
}
function GetSubsidiaryList() {
    GetCustomRecord("/Subsidiary/GetSubsidiaryList", "divHRMS")
}

$(document).ready(function () {
    GetSubsidiaryList();
})

function AddSubsidiary() {
    NewCustomRecord("/Subsidiary/CreateSubsidiary", "Create Subsidiary")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Subsidiary/GetSubsidiaryList", "/Subsidiary/DeleteSubsidiary", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Subsidiary/CreateSubsidiary", "Update Subsidiary", "Update Subsidiary");
}
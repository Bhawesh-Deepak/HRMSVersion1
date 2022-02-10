function GetSubsidiaryList() {
    GetCustomRecord("/LegalEntity/GetLegalEntityList", "divHRMS")
}

$(document).ready(function () {
    GetSubsidiaryList();
})

function AddSubsidiary() {
    NewCustomRecord("/LegalEntity/CreateLegalEntity", "Create Legal Entity")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/LegalEntity/GetLegalEntityList", "/LegalEntity/DeleteLegalEntity", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/LegalEntity/CreateLegalEntity", "Update Legal Entity", "Update Legal Entity");
}
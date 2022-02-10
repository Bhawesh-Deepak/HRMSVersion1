function GetRecord() {
    GetCustomRecord("/SubModule/GetSubModuleDetails", "divHRMS")
}

$(document).ready(function () {
    GetRecord();
})

function AddRecord() {
    NewCustomRecord("/SubModule/CreateSubModule", "Create  Sub Module")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/SubModule/GetSubModuleDetails", "/SubModule/DeleteSubModule", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/SubModule/CreateSubModule", "Update Sub Module", "Update Sub Module Detail");
}
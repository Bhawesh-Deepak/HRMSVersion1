function GetRecord() {
    GetCustomRecord("/ModuleMaster/GetModuleDetail", "divHRMS")
}

$(document).ready(function () {
    GetRecord();
})

function AddRecord() {
    NewCustomRecord("/ModuleMaster/CreateModule", "Create Module")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/ModuleMaster/GetModuleDetail", "/ModuleMaster/DeleteModule", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/ModuleMaster/CreateModule", "Update Module", "Update Module");
}
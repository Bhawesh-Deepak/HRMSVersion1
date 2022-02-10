function GetNewsAndUpdateList() {
    GetCustomRecord("/NewsAndUpdate/GetNewsAndUpdateList", "divHRMS")
}

$(document).ready(function () {
    GetNewsAndUpdateList();
})

function AddRecord() {
    NewCustomRecord("/NewsAndUpdate/NewsAndUpdateCreate", "Create News and Update")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/NewsAndUpdate/GetNewsAndUpdateList", "/NewsAndUpdate/DeleteNewsAndUpdate", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/NewsAndUpdate/NewsAndUpdateCreate", "Update News and Update");
}
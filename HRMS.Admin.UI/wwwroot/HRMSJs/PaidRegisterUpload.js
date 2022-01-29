function GetAnnouncementandupdateListList() {
    GetCustomRecord("/PaidRegisterUpload/GetPaidUploadRegister", "divHRMS")
}

$(document).ready(function () {
    GetAnnouncementandupdateListList();
})

function AddRecord() {
    NewCustomRecord("/PaidRegisterUpload/CreatePaidUploadRegister", "Create Paid Register")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/PaidRegisterUpload/GetPaidUploadRegister", "/PaidRegisterUpload/DeletePaidRegister", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/PaidRegisterUpload/CreatePaidUploadRegister", "Update Paid Register");
}
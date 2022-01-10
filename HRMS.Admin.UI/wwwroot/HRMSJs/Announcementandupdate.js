function GetAnnouncementandupdateListList() {
    GetCustomRecord("/Announcementandupdate/GetAnnounceandupdateList", "divHRMS")
}

$(document).ready(function () {
    GetAnnouncementandupdateListList();
})

function AddRecord() {
    NewCustomRecord("/Announcementandupdate/AnnounceandupdateCreate", "Create Announcement and Update")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Announcementandupdate/GetAnnounceandupdateList", "/Announcementandupdate/DeleteAnnouncementandupdate", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Announcementandupdate/AnnounceandupdateCreate", "Update Announcement and Update");
}
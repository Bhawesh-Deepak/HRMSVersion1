function GetCourseList() {
    GetCustomRecord("/Location/GetLoctionList", "divHRMS")
}

$(document).ready(function () {
    // GetCourseList();
})

function AddCourse() {
    NewCustomRecord("/Course/CreateCourse", "Create Course")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Location/GetLoctionList", "/Location/DeleteLocation", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Location/CreateLocation", "Update Location");
}
function GetCourseList() {
    GetCustomRecord("/Course/GetCourseList", "divHRMS")
}

$(document).ready(function () {
     GetCourseList();
})

function AddCourse() {
    NewCustomRecord("/Course/CreateCourse", "Create Course")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/Course/GetCourseList", "/Course/DeleteCourse", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Course/CreateCourse", "Update Course");
}
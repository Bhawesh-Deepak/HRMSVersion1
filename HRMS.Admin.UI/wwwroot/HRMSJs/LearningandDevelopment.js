function GetLearningandDevelopmentList() {
    // GetCustomRecord("/LearningandDevelopment/GetLearningandDepartmentList", "divHRMS")
}

$(document).ready(function () {
    GetLearningandDevelopmentList();
})

function AddLearningandDevelopment() {
    NewCustomRecord("/LearningandDevelopment/CreateLearningandDevelopment", "Create Learning And Development")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/LearningandDevelopment/GetLoctionList", "/Location/DeleteLocation", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/Location/CreateLocation", "Update Location");
}
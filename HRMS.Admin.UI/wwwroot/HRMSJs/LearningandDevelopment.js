function GetLearningandDevelopmentList() {
    GetCustomRecord("/LearningAndDevelopment/GetLearningAndDevelopmentList", "divHRMS")
}

$(document).ready(function () {
    GetLearningandDevelopmentList();
})

function AddLearningandDevelopment() {
    NewCustomRecord("/LearningAndDevelopment/CreateLearningAndDevelopment", "Create Learning And Development")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/LearningAndDevelopment/GetLearningAndDevelopmentList", "/LearningAndDevelopment/DeleteLearningAndDevelopment", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/LearningAndDevelopment/CreateLearningAndDevelopment", "Update Learning And Development");
}
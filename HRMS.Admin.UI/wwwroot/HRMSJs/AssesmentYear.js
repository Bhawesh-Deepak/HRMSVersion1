function GetAssesmentYearList() {
    GetCustomRecord("/AssesmentYear/GetAssesmentYearList", "divHRMS")
}

$(document).ready(function () {
    GetAssesmentYearList();
})

function AddAssesmentYear() {
    NewCustomRecord("/AssesmentYear/CreateAssesmentYear", "Create Assesment Year")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/AssesmentYear/GetAssesmentYearList", "/AssesmentYear/DeleteAssesmentYear", eData);
}

function UpdateRecordAssesmentYear(id) {
    UpdateCustomRecord(id, "/AssesmentYear/CreateAssesmentYear", "Update Assesment Year");
}
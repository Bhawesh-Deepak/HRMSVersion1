function GetCandidateDetails() {
    GetCustomRecord("/CandidateDetail/GetCandidateDetails", "divHRMS")
}

$(document).ready(function () {
    GetCandidateDetails();
})

function AddCandidateDetails() {
    NewCustomRecord("/CandidateDetail/CreateCandidateDetail", "Create Candidate Details")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/CandidateDetail/GetCandidateDetails", "/CandidateDetail/DeleteAssesmentYear", eData);
}

function UpdateRecordAssesmentYear(id) {
    UpdateCustomRecord(id, "/CandidateDetail/CreateCandidateDetail", "Update Candidate Details");
}
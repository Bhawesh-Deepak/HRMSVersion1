function GetReferCandidate() {
    GetCustomRecord("/CandidateReferal/GetCurrentOpeningDetails", "divHRMS")
}

$(document).ready(function () {
    GetReferCandidate();
})

function AddRecord() {
    NewCustomRecord("/CandidateReferal/CreateReferal", "Create Candidate Information")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/CandidateReferal/GetBranchList", "/CandidateReferal/DeleteCandidate", eData, "divCandidateDetails");
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/CandidateReferal/CreateReferal", "Update Candidate Information");
}

function GetCandidateDetails(id) {
    GetCustomRecord("/CandidateReferal/GetCandidateRefer?openingId=" + id, "divCandidateDetails");
    setTimeout(function () {
        location.reload();
    }, 3000)
}
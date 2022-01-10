function GetCandidateDetails() {
    GetCustomRecord("/ReferDetail/GetReferCandidateDetails", "divHRMS")
}

$(document).ready(function () {
    GetCandidateDetails();
})


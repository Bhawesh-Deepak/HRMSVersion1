function GetCompanyPolicyList() {
    GetCustomRecord("/CompanyPolicy/GetCompanyPolicyList", "divHRMS")
}

$(document).ready(function () {
    GetCompanyPolicyList();
})

function AddCompanyPolicy() {
    NewCustomRecord("/CompanyPolicy/CreateCompanyPolicy", "Create Company Policy")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/CompanyPolicy/GetCompanyPolicyList", "/CompanyPolicy/DeleteCompanyPolicy", eData);
}

function UpdateRecord(id) {
    UpdateCustomRecord(id, "/CompanyPolicy/CreateCompanyPolicy", "Update Company Policy");
}
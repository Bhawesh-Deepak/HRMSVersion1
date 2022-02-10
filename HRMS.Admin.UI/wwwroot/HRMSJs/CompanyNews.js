function GetCompanyNewsList() {
    GetCustomRecord("/CompanyNews/GetCompanyNewsList", "divHRMS")
}

$(document).ready(function () {
    GetCompanyNewsList();
})

function AddHolidays() {
    NewCustomRecord("/CompanyNews/CompanyNewsCreate", "Create Company News")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/CompanyNews/GetCompanyNewsList", "/CompanyNews/DeleteCompanyNews", eData);
}

function UpdateRecordHolidays(id) {
    UpdateCustomRecord(id, "/CompanyNews/CompanyNewsCreate", "Update Company News");
}
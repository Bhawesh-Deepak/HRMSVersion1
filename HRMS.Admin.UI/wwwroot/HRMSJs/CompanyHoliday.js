function GetHolidayList() {
    GetCustomRecord("/CompanyHolidays/GetHolidayList", "divHRMS")
}

$(document).ready(function () {
    GetHolidayList();
})

function AddHolidays() {
    NewCustomRecord("/CompanyHolidays/HolidayCreate", "Create Company Holiday")
}

function Delete(id, eData) {
    CustomDeleteRecord(id, "/CompanyHolidays/GetHolidayList", "/CompanyHolidays/DeleteCompanyHoliday", eData);
}

function UpdateRecordHolidays(id) {
    UpdateCustomRecord(id, "/CompanyHolidays/HolidayCreate", "Update Company Holidays");
}
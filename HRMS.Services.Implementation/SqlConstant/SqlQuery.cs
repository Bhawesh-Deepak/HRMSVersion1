using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Services.Implementation.SqlConstant
{
    public static class SqlQuery
    {
        public const string UploadAttendance = @"[Payroll].[usp_UploadEmployeeAttendance]";
        public const string GetReferedCandidate = @"Posting.usp_GetReferedCandidate";
        public const string GetFileteredEmployee = @"Payroll.usp_GetEmployeeList";
        public const string GetEmployeeSalaryByCode = @"[dbo].[usp_GetEmployeeSalaryByEmpCode]";
        public const string GetEmployeeSalary = @"[dbo].[usp_GetEmployeeSalary]";
        public const string EmployeeIncrement = @"[Payroll].[usp_EmployeeIncrement]";
        public const string EmployeeCount = @"[dbo].[usp_CountAllEmployee]";
        public const string EmployeeInformation = @"[dbo].[usp_GetEmployeeInformation]";
        public const string GetEmployeeDetails = @"[Payroll].[usp_GetEmployeeDetails]";
        public const string GetEmployeeSingleDetails = @"[dbo].[usp_GetSingleEmployeeDetail]";
        public const string GetIncentivePaidRegister = @"[Reporting].[usp_IncentivePaidRegister]";
        public const string GetECRReport = @"[Reporting].[usp_ECRReport]";
        public const string GetPaySlip = @"[Reporting].[usp_GetEmployeePaySlip]";
        public const string GetPTaxReport = @"[Reporting].[usp_GetProfessionTaxReport]";
        public const string GetLWFReport = @"[Reporting].[usp_GetLWFReport]";
        public const string GetAttendanceGraph= @"[Reporting].[usp_GetAttendanceGraph]";
        public const string GetESICReport = @"[Reporting].[usp_ESICReport]";
        public const string GetemployeeAutoComplete = @"[Payroll].[usp_getEmployeeAutoComplete]";
        public const string GetExportEmployee = @"[Payroll].[usp_ExportEmployeeDetail]";
        public const string GetBirtdayAnniversary = @"[Payroll].[usp_GetBirthdayAnniversary]";
        public const string GetNewHireAndExitEmployee = @"[Payroll].[usp_GetNewHireAndExitEmployee]";
        public const string GetEmployeeAttendanceDetails = @"Report.usp_GetEmployeeAttendanceDetail";
    }
}

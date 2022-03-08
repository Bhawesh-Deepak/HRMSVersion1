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
        public const string GetEmployeeMediClaimReport = @"[Reporting].[usp_GetMediclaimReport]";
        public const string calculateSalary = @"[Reporting].[usp_CalculateSalary]";
        public const string GetBonusRegister = @"[Reporting].[usp_GetBonusRegister]";
        public const string GetOverTimeRegister = @"[Reporting].[usp_GetOverTimeRegister]";
        public const string UploadCTCStructure = @"[Payroll].[usp_UpdateCTCStructure]";
        public const string UploadEmployeeSalary = @"[Payroll].[usp_UpdateEmployeeSalary]";
        public const string ExportNewHireAndExitEmployee = @"[Reporting].[usp_GetNewHireAndExitEmployee]";
        public const string GetSalaryRegisterBySalaryCalculation = @"[Reporting].[usp_GetSalaryRegisterBySalaryCalculation]";
        public const string GetComputationofTaxReport = @"[Reporting].[usp_GetComputationOfTaxReport]";
        public const string GetGratuityCalculation = @"[Reporting].[usp_GetGratuityCalculation]";
        public const string GetNetVsGrossSalary = @"[Reporting].[usp_GetNetVSGrossSalary]";
        public const string GetAttendanceStatus = @"[Reporting].[usp_GetAttendanceStatus]";
        public const string GetGrossAndPIReport = @"[Reporting].[usp_GrossAndPIReport]";



    }
}

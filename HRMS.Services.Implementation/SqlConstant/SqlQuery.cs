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
    }
}

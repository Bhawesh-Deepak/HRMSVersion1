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
    }
}

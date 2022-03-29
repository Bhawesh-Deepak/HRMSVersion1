using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Models.Response
{
    public class AuthenticateVM
    {
        public int EmployeeId { get; set; }
        public string EmpCode { get; set; }
        public int FinancialYear { get; set; }
        public string FinancialYearName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string Token { get; set; }

    }
}

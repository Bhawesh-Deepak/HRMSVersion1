using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class OverTimeRegisterVM
    {
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal SalaryAmount { get; set; }
        public string MonthsName { get; set; }
        public int DateYear { get; set; }
    }
}

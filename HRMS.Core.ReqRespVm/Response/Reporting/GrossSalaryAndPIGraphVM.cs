using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class GrossSalaryAndPIGraphVM
    {
        public string MonthsName { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal PIAmount { get; set; }
    }
}

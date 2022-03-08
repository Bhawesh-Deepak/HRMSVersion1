using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class GrossVsNetSalaryVM
    {
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public string MonthsName { get; set; }

    }
}

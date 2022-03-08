using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class ComputationOfTaxReportParams
    {
        public string EmpCode { get; set; }
        public int FinancialYear { get; set; }
    }
}

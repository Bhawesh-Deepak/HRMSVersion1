using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class NoOfEmployeeWhomIncentivePaidParams
    {
        public int FinancialYear { get; set; }
        public string LegalEntity { get; set; }
    }
}

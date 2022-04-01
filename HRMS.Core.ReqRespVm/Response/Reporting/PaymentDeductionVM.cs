using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class PaymentDeductionVM
    {
        public int Id { get; set; }
        public string ComponentName { get; set; }
        public int ComponentId { get; set; }
        public int ComponentType { get; set; }
        public int ComponentValueType { get; set; }
        public decimal SalaryAmount { get; set; }
        public string ComponentCategory { get; set; }
        public string EmpCode { get; set; }
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public string MonthName { get; set; }
       
    }
}

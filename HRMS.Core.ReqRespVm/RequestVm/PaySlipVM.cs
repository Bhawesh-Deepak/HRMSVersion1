using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class PaySlipVM
    {
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public string MonthsName { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
    }
}

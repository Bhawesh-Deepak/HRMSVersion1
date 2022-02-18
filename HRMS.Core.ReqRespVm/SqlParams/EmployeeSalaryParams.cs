using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class EmployeeSalaryParams
    {
        public string EmpCode { get; set; }
        public decimal CTC{ get; set; }
        public int UpdatedBy { get; set; }
    }
}

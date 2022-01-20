using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Employee
{
    public class EmployeeCountVM
    {
        public int TotalEmployee { get; set; }
        public int ActiveEmployee { get; set; }
        public int InActiveEmployee { get; set; }
        public int FNFIntiated { get; set; }
        public int SubaticalEmpoyee { get; set; }
    }
}

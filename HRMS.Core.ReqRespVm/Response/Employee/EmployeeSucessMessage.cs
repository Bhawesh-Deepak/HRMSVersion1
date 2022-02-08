using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Employee
{
    public class EmployeeSucessMessage
    {
        public int Steps { get; set; }
        public string Message { get; set; }
        public int  Id{ get; set; }
        public string EmpCode { get; set; }
    }
}

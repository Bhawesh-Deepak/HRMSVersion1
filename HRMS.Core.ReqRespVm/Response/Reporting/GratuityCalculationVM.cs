using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class GratuityCalculationVM
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal NoOfDays { get; set; }
        public decimal NoOfYear { get; set; }
        public decimal GratuityAmount { get; set; }
        public decimal BasicAmount { get; set; }
        public DateTime DateOfLeaving { get; set; }
    }
}

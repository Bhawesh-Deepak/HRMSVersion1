using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class ESICReportVM
    {
        public string ESICNew { get; set; }
        public string EmployeeName { get; set; }
        public string ExitDate { get; set; }
        public decimal WorkingDays { get; set; }
        public decimal ESIAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Reason { get; set; }
         
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class ECRReportModel
    {
        public string UANNumber { get; set; }
        public string EmployeeName { get; set; }
        public decimal GrossWages { get; set; }
        public decimal EPFWages { get; set; }
        public decimal EPSWages { get; set; }
        public decimal EDLIWages { get; set; }
        public decimal EPFCONTRIREMITTED { get; set; }
        public decimal NCPDays { get; set; }
    }
}

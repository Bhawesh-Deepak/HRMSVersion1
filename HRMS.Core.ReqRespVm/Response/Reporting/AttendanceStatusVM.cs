using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class AttendanceStatusVM
    {
        public decimal PresentDays { get; set; }
        public decimal LOPDays { get; set; }
        public decimal ArrearDays { get; set; }
        public string MonthsName { get; set; }
        public int DateYear { get; set; }
        public int DateMonth { get; set; }

    }
}

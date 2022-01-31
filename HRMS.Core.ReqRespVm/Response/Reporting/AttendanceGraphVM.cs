using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class AttendanceGraphVM
    {
        public int MonthsName { get; set; }
        public decimal PresentDays { get; set; }
        public decimal LOPDays { get; set; }
        public int TotalEmployee { get; set; }
    }
}

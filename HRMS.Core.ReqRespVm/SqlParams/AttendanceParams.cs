using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class AttendanceParams
    {
        public int MonthId { get; set; }
        public int YearId { get; set; }
        public string EmpCode { get; set; }
        public decimal LopDays { get; set; }
    }
}

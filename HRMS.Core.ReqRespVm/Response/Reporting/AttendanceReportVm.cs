using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reporting
{
    public class AttendanceReportVm
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Level { get; set; }
        public int DateYear { get; set; }
        public int DateMonth { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int LOPDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

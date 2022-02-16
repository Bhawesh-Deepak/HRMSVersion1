using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Attendance
{
    public class AttendanceVm
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeLevel { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalDays { get; set; }
        public int LOPDays { get; set; }
        public int PresentDays { get; set; }
        public string MonthsName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IDictionary<DateTime, string> DatWiseAttendance { get; set; }
    }
}

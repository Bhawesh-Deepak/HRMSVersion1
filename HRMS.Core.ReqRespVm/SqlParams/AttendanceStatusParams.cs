using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class AttendanceStatusParams
    {
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public string EmpCode { get; set; }
    }
}

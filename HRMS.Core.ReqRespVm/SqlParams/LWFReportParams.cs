using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class LWFReportParams
    {
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public string PTStateName { get; set; }
    }
}

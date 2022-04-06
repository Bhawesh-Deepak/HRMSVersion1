using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class ReadUtilityExcelDataVM
    {
        public string EmpCode { get; set; }
        public string ResponseValue { get; set; }
        public DateTime? ResponseValueDate { get; set; }
        public int  ResponseValueInt { get; set; }
    }
}

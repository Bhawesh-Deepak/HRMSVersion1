using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class FilteredEmployeeParams
    {
        public string Name { get; set; }
        public string empCode { get; set; }
        public string department { get; set; }
        public string designation { get; set; }

    }
}

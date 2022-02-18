using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class UploadCTCSTructureParams
    {
        public int UpdatedBy { get; set; }
        public int ComponentId { get; set; }
        public decimal ComponentValue { get; set; }
        public string EmpCode { get; set; }
    }
}

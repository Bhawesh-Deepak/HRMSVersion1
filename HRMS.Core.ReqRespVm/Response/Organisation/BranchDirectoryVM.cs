using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Organisation
{
    public class BranchDirectoryVM
    {
        public int SubsidiaryId { get; set; }
        public string SubsidiaryName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int TotalEmployee { get; set; }
    }
}

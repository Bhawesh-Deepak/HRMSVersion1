using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class BranchVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string RegionName { get; set; }
        public string LocationTypeName { get; set; }
    }
}

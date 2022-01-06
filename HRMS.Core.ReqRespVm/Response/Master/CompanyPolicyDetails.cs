using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class CompanyPolicyDetails
    {
        public int CompanyPolicyId { get; set; }
        public  string DepartmentName { get; set; }
        public string Name { get; set; }
        public DateTime CalenderDate { get; set; }
        public string DocumentUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class CompanyPolicyDetails
    {
        public int Id { get; set; }
        public  string LegalEntityName { get; set; }
        public string Name { get; set; }
        public string CalenderDate { get; set; }
        public string DocumentUrl { get; set; }
    }
}

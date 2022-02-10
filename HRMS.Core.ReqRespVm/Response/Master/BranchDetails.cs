using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class BranchDetails
    {
        public int BranchId { get; set; }
        public string  CompanyName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string Logo { get; set; }
        public string Address { get; set; }
        public int ZipCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
     


    }
}

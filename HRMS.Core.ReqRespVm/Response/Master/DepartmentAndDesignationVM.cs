using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class DepartmentAndDesignationVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string DesignationCode { get; set; }
        
    }
}

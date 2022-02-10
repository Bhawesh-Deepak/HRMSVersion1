using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.RequestVm
{
    public class InternalPostingVm
    {
        public int EmployeeId { get; set; }
        public int  DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int LocationId { get; set; }
    }
}

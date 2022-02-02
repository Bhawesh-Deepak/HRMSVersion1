using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.HR
{
    [Table("LeaveAllocation", Schema = "HR")]
    public class LeaveAllocation : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public string LeaveCode { get; set; }
        public decimal CountLeave { get; set; }

    }
}

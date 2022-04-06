using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.History
{
    [Table("EmployeeUpdateHistory", Schema = "History")]
    public class EmployeeUpdateHistory : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Location { get; set; }
        public string LegalEntity { get; set; }
        public string SuperVisorCode { get; set; }
        public string Level { get; set; }
        public string BranchOfficeId { get; set; }

    }
}

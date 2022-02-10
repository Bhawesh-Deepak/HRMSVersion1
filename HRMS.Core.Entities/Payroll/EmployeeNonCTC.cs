using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("EmployeeNonCTC", Schema = "Payroll")]
    public class EmployeeNonCTC : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public int DateYear { get; set; }
        public int DateMonth { get; set; }
        public int ComponentId { get; set; }
        public decimal ComponentValue { get; set; }
        [NotMapped]
        public string ComponentName { get; set; }
    }
}

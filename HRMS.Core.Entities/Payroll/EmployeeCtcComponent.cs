using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("EmployeeCtcComponent", Schema = "Payroll")]
    public class EmployeeCtcComponent : BaseModel<int>
    {
        public int EmployeeSalaryId { get; set; }
        public int ComponentId { get; set; }
        public string EmpCode { get; set; }
        public decimal ComponentValue { get; set; }
        [NotMapped]
        public string ComponentName { get; set; }

    }
}

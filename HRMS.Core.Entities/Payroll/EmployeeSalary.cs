using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("EmployeeSalary", Schema = "Payroll")]
    public class EmployeeSalary : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal CTC { get; set; }

    }
}

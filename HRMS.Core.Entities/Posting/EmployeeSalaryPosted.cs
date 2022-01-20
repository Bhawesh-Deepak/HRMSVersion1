using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Posting
{
    [Table("EmployeeSalaryPosted", Schema = "Payroll")]
    public class EmployeeSalaryPosted : BaseModel<int>
    {
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public string EmpCode { get; set; }
        public int ComponentId { get; set; }
        public decimal SalaryAmount { get; set; }
        public string LegalEntity { get; set; }
        public string Department { get; set; }
        public string DesignationId { get; set; }
    }
}

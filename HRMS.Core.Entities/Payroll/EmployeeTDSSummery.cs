using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("EmployeeTDSSummery", Schema = "Payroll")]
    public class EmployeeTDSSummery : BaseModel<int>
    {
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public string EmpCode { get; set; }
        public decimal CurrentCTC { get; set; }
        public decimal CurrentNONCTC { get; set; }
        public decimal TDSTaxableValue { get; set; }
        public decimal DeductPercentage { get; set; }
        public decimal DeductAGE { get; set; }
        public decimal Surcharge { get; set; }
        public decimal HECAmount { get; set; }
        public decimal TDSAmountYearly { get; set; }
        public decimal TDSAmountMonthly { get; set; }
         
    }
}
